@echo off
REM Set codepage to UTF-8
chcp 65001

REM Set NLS_LANG to ensure Oracle client sends and receives UTF-8 characters correctly
set NLS_LANG=AMERICAN_AMERICA.AL32UTF8

echo =======================================================
echo Applying Database Fixes for Font Formatting and Procedures
echo =======================================================

echo Running UserLinking update...
sqlplus hospital_dba/123@localhost:1521/PDB_QLYT @"Part 1\05_UserLinking.sql"

echo Running staff format fixes...
sqlplus hospital_dba/123@localhost:1521/PDB_QLYT @update_all_staff.sql
sqlplus hospital_dba/123@localhost:1521/PDB_QLYT @fix_staff_format.sql

echo =======================================================
echo All fixes applied successfully!
echo =======================================================
pause
