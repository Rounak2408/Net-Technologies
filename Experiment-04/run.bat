@echo off
title Online Student Event Registration Portal Server
echo Creating build directories...
if not exist bin mkdir bin

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
echo Starting WebServer.exe...
WebServer.exe
pause
