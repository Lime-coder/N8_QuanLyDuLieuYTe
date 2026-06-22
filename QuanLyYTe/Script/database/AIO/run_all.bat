@echo off
setlocal

:: ==============================================================================
:: Thu muc AIO - All In One Script
:: Tu dong chay tat ca cac kich ban khoi tao Co so du lieu Quan ly Y te
:: ==============================================================================

echo ====================================================================
echo = QLYT DATABASE SETUP SCRIPT (AIO)                                 =
echo ====================================================================

:: Kiem tra sqlplus co ton tai trong PATH hay khong
where sqlplus >nul 2>nul
if %errorlevel% neq 0 (
    echo [ERROR] sqlplus khong duoc tim thay! Vui long kiem tra Oracle Client.
    pause
    exit /b 1
)

echo Kich ban nay se chay toan bo Database script tu thu muc 00 den 07.
echo Log se duoc luu vao file: AIO_run_log.txt
echo ====================================================================

echo Bat dau chay script bang SQL*Plus...
chcp 65001 >nul
set NLS_LANG=AMERICAN_AMERICA.AL32UTF8
sqlplus /nolog @run_all.sql

echo ====================================================================
echo Hoan tat! Vui long kiem tra file AIO_run_log.txt de xem chi tiet.
echo Chu y: Mot so thay doi ve Audit/OLS can khoi dong lai Oracle Database.
echo ====================================================================
pause
