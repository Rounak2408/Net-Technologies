@echo off
title PlacementHub - Placement Management Dashboard (ASP.NET Web Forms - Ex-09)

echo =========================================================================
echo   PlacementHub - Placement Management Dashboard (Experiment 09)
echo   ASP.NET Web Forms using Data Controls & ADO.NET SQL Server
echo =========================================================================
echo.
echo Creating build directories...
if not exist bin mkdir bin

echo.
echo Compiling WebServer.cs...
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /out:WebServer.exe WebServer.cs /r:System.Web.dll /r:System.dll
if %errorlevel% neq 0 (
    echo.
    echo Compilation of WebServer failed!
    pause
    exit /b %errorlevel%
)

echo.
echo Copying WebServer.exe to bin folder...
copy WebServer.exe bin\WebServer.exe /y

echo.
echo Starting WebServer at http://localhost:5090/Default.aspx ...
WebServer.exe
pause
