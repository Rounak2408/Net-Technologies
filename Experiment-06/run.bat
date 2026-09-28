@echo off
title Product Catalog Application - ASP.NET Core MVC (Ex-06)

echo Starting ASP.NET Core MVC Product Catalog Application...
echo Server URL: http://localhost:5062
echo.
start http://localhost:5062
dotnet run --urls "http://localhost:5062"
pause
