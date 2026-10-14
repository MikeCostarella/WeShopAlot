@echo off
rem Runs Publish-WeShopAlot.ps1 from Command Prompt or File Explorer, in Windows PowerShell.
rem Arguments pass through, e.g.  publish-weshopalot.cmd -CleanInstall
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Publish-WeShopAlot.ps1" %*
set "RC=%ERRORLEVEL%"
rem Opened by double-click, keep the window open so the results can be read (see check-prerequisites.cmd).
setlocal EnableDelayedExpansion
set "CL=!CMDCMDLINE:"=!"
if /i not "!CL:%~nx0=!"=="!CL!" pause
exit /b %RC%
