@echo off
chcp 65001 >nul
set /p "UNISTR_INPUT=Enter the UNISTR string: "
echo.
echo === Translated Text ===
powershell -NoProfile -Command "$str = $env:UNISTR_INPUT; [regex]::Replace($str, '\\([0-9a-fA-F]{4})', { param($m) [char][int]('0x' + $m.Groups[1].Value) })"
echo =======================
echo.
pause