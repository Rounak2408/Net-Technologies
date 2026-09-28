@echo off
title Academic Calendar and Leave Management System Server (Ex-05)

echo Stopping any previous WebServer instance...
taskkill /f /im WebServer.exe >nul 2>&1

echo Creating build and upload directories...
if not exist bin mkdir bin
if not exist uploads mkdir uploads

echo.
echo Compiling WebServer.cs...
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /out:WebServer.exe WebServer.cs /r:System.Web.dll /r:System.dll
if %errorlevel% neq 0 (
    echo.
    echo Compilation failed!
    pause
    exit /b %errorlevel%
)

echo.
echo Copying WebServer.exe to bin folder...
copy WebServer.exe bin\WebServer.exe /y

echo.
echo Starting WebServer.exe on http://localhost:8085/Default.aspx ...
WebServer.exe
pause
