@echo off
title Student Management REST API (ASP.NET Core Web API - Ex-10)

echo =========================================================================
echo   Student Management REST API (Experiment 10)
echo   ASP.NET Core Web API, ADO.NET SQL Server & Custom Middleware
echo =========================================================================
echo.
echo Starting API Server & Swagger UI at http://localhost:5100 ...
echo.
start http://localhost:5100
dotnet run --urls "http://localhost:5100"
pause
