@echo off
setlocal

:: ==============================================================================
:: Thu muc AIO - Script Xoa Toan Bo (Clean All)
:: ==============================================================================

echo ====================================================================
echo = QLYT DATABASE CLEANUP SCRIPT (AIO)                               =
echo ====================================================================
echo [CANH BAO] Kich ban nay se XOA TOAN BO Schema, Du lieu va Cau hinh!
echo ====================================================================

:: Kiem tra sqlplus co ton tai trong PATH hay khong
where sqlplus >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERROR] sqlplus khong duoc tim thay! Vui long kiem tra Oracle Client.
    pause
    exit /b 1
)

echo Bat dau chay script don dep bang SQL*Plus...
chcp 65001 >nul
set NLS_LANG=AMERICAN_AMERICA.AL32UTF8
sqlplus /nolog @clean_all.sql

echo ====================================================================
echo Hoan tat xoa! Vui long kiem tra file AIO_clean_log.txt de biet ket qua.
echo ====================================================================
pause
