@echo off
chcp 65001 >nul
set /p "VIETNAMESE_INPUT=Enter the Vietnamese string: "
echo.
echo === UNISTR Encoded Text ===
powershell -NoProfile -Command "$str = $env:VIETNAMESE_INPUT; $out = ''; foreach ($c in $str.ToCharArray()) { $code = [int]$c; if ($code -gt 127) { $out += '\' + $code.ToString('X4') } else { $out += $c } }; echo $out"
echo ===========================
echo.
pause
