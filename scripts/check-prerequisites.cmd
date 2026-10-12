@echo off
rem Runs Check-Prerequisites.ps1 from Command Prompt or File Explorer, in Windows PowerShell.
rem Arguments pass through, e.g.  check-prerequisites.cmd -Offline
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Check-Prerequisites.ps1" %*
set "RC=%ERRORLEVEL%"
rem Opened by double-click: keep the window open so the results can be read.
echo %CMDCMDLINE% | findstr /i /c:"%~nx0" >/dev/null && pause
exit /b %RC%
