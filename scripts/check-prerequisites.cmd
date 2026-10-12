@echo off
rem Runs Check-Prerequisites.ps1 from Command Prompt or File Explorer, in Windows PowerShell.
rem Arguments pass through, e.g.  check-prerequisites.cmd -Offline
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Check-Prerequisites.ps1" %*
set "RC=%ERRORLEVEL%"
rem Opened by double-click, Windows runs  cmd.exe /c ""...\check-prerequisites.cmd" "  so this
rem file's name is in CMDCMDLINE; keep the window open so the results can be read.
rem (No pipes or redirection here: each pipe starts another cmd.exe.)
setlocal EnableDelayedExpansion
set "CL=!CMDCMDLINE:"=!"
if /i not "!CL:%~nx0=!"=="!CL!" pause
exit /b %RC%
