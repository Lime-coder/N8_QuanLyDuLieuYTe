[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$DumpFile,

    [string]$OracleUser = "hospital_dba",
    [string]$Service = "localhost:1521/PDB_QLYT",
    [string]$DirectoryName = "HOSPITAL_BACKUP_DIR",
    [string]$BackupPath = "C:\OracleBackups",

    [string]$SourceSchema = "HOSPITAL",
    [string]$RestoreSchema = "HOSPITAL_RESTORE",
    [string]$RestorePassword = "123",
    [string]$RestoreTablespace = "HOSPITAL_DATA",

    [switch]$SkipDrop,
    [switch]$UseOsSysDba
)

$ErrorActionPreference = "Stop"

function Assert-CommandExists {
    param([string]$Name)

    if (-not (Get-Command $Name -ErrorAction SilentlyContinue)) {
        throw "$Name was not found in PATH. Open an Oracle-enabled PowerShell or add Oracle bin directory to PATH."
    }
}

function ConvertTo-PlainText {
    param([System.Security.SecureString]$SecureString)

    $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecureString)
    try {
        [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr)
    }
    finally {
        [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr)
    }
}

Assert-CommandExists "sqlplus"
Assert-CommandExists "impdp"

if ($UseOsSysDba.IsPresent) {
    throw @"
Do not use -UseOsSysDba for PDB Data Pump import.
On this Oracle XE setup it connects to CDB$ROOT, so PDB_QLYT objects such as
HOSPITAL_BACKUP_DIR are not visible and CREATE USER becomes a common-user action.

Run the script without -UseOsSysDba. The import user hospital_dba already has
DATAPUMP_IMP_FULL_DATABASE after BackupRecoveryGrants.sql.
"@
}

if ([System.IO.Path]::IsPathRooted($DumpFile)) {
    $physicalDumpPath = $DumpFile
    $dumpFileName = Split-Path -Path $DumpFile -Leaf
}
else {
    $physicalDumpPath = Join-Path -Path $BackupPath -ChildPath $DumpFile
    $dumpFileName = $DumpFile
}

if (-not (Test-Path -LiteralPath $physicalDumpPath)) {
    throw "Dump file does not exist on this machine: $physicalDumpPath"
}

$securePassword = Read-Host "Enter password for $OracleUser@$Service" -AsSecureString
$plainPassword = ConvertTo-PlainText $securePassword
$connectString = "$OracleUser/$plainPassword@$Service"
$impdpUserId = $connectString
$displayUser = "$OracleUser@$Service"

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$importLogFile = "import_${RestoreSchema}_${timestamp}.log".ToLowerInvariant()
$tempSql = Join-Path -Path $env:TEMP -ChildPath "prepare_${RestoreSchema}_${timestamp}.sql"
$verifySql = Join-Path -Path $env:TEMP -ChildPath "verify_${RestoreSchema}_${timestamp}.sql"

try {
    Write-Host "Preparing restore schema $RestoreSchema..."

    $dropBlock = ""
    if (-not $SkipDrop.IsPresent) {
        $dropBlock = @"
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM DBA_USERS
    WHERE USERNAME = UPPER('$RestoreSchema');

    IF v_count > 0 THEN
        EXECUTE IMMEDIATE 'DROP USER $RestoreSchema CASCADE';
        DBMS_OUTPUT.PUT_LINE('[OK] Dropped old schema $RestoreSchema');
    END IF;
END;
/
"@
    }

    @"
WHENEVER SQLERROR EXIT SQL.SQLCODE
SET SERVEROUTPUT ON
$dropBlock
DECLARE
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM DBA_USERS
    WHERE USERNAME = UPPER('$RestoreSchema');

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE 'CREATE USER $RestoreSchema IDENTIFIED BY "$RestorePassword" DEFAULT TABLESPACE $RestoreTablespace TEMPORARY TABLESPACE TEMP QUOTA UNLIMITED ON $RestoreTablespace';
        DBMS_OUTPUT.PUT_LINE('[OK] Created schema $RestoreSchema');
    ELSE
        EXECUTE IMMEDIATE 'ALTER USER $RestoreSchema IDENTIFIED BY "$RestorePassword" ACCOUNT UNLOCK';
        EXECUTE IMMEDIATE 'ALTER USER $RestoreSchema QUOTA UNLIMITED ON $RestoreTablespace';
        DBMS_OUTPUT.PUT_LINE('[OK] Reused schema $RestoreSchema');
    END IF;
END;
/

GRANT CREATE SESSION TO $RestoreSchema;
GRANT CREATE TABLE TO $RestoreSchema;
GRANT CREATE VIEW TO $RestoreSchema;
GRANT CREATE SEQUENCE TO $RestoreSchema;
GRANT CREATE PROCEDURE TO $RestoreSchema;
GRANT CREATE TRIGGER TO $RestoreSchema;

EXIT
"@ | Set-Content -LiteralPath $tempSql -Encoding ASCII

    & sqlplus -S $connectString "@$tempSql"
    if ($LASTEXITCODE -ne 0) {
        throw "Restore schema preparation failed with exit code $LASTEXITCODE."
    }

    Write-Host "Importing $dumpFileName into $RestoreSchema as $displayUser..."

    $impdpArgs = @(
        $impdpUserId,
        "DIRECTORY=$DirectoryName",
        "DUMPFILE=$dumpFileName",
        "LOGFILE=$importLogFile",
        "REMAP_SCHEMA=$($SourceSchema):$($RestoreSchema)",
        "REMAP_TABLESPACE=SYSTEM:$RestoreTablespace",
        "TABLE_EXISTS_ACTION=REPLACE",
        "EXCLUDE=USER",
        "EXCLUDE=GRANT",
	    "EXCLUDE=RLS_POLICY",
        "EXCLUDE=FGA_POLICY",
        "EXCLUDE=PROCACT_INSTANCE",
        "TRANSFORM=SEGMENT_ATTRIBUTES:N"
    )

    & impdp @impdpArgs
    if ($LASTEXITCODE -notin @(0, 5)) {
        throw "impdp failed with exit code $LASTEXITCODE. Review $BackupPath\$importLogFile"
    }
    elseif ($LASTEXITCODE -eq 5) {
        Write-Warning "impdp completed with warnings. Continuing to verification. Review $BackupPath\$importLogFile if needed."
    }

    Write-Host "Verifying imported data..."

    @"
WHENEVER SQLERROR EXIT SQL.SQLCODE
SET PAGESIZE 200
SET LINESIZE 200
PROMPT ===== RESTORE SCHEMA VERIFY =====
SELECT COUNT(*) AS PRESCRIPTION_ROWS FROM $RestoreSchema.PRESCRIPTION;
SELECT COUNT(*) AS TABLE_COUNT FROM ALL_TABLES WHERE OWNER = UPPER('$RestoreSchema');
SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = UPPER('$RestoreSchema') ORDER BY TABLE_NAME;
EXIT
"@ | Set-Content -LiteralPath $verifySql -Encoding ASCII

    & sqlplus -S $connectString "@$verifySql"
    if ($LASTEXITCODE -ne 0) {
        throw "Restore verification failed with exit code $LASTEXITCODE."
    }

    Write-Host "[OK] Data Pump import completed."
    Write-Host "Dump file : $physicalDumpPath"
    Write-Host "Import log: $BackupPath\$importLogFile"
    Write-Host "Schema    : $RestoreSchema"
}
finally {
    if (Test-Path -LiteralPath $tempSql) {
        Remove-Item -LiteralPath $tempSql -Force
    }
    if (Test-Path -LiteralPath $verifySql) {
        Remove-Item -LiteralPath $verifySql -Force
    }
}
