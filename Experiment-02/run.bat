@echo off
echo Compiling Program.cs...
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /out:EmployeePayrollSystem.exe Program.cs
if %errorlevel% neq 0 (
    echo Compilation failed!
    pause
    exit /b %errorlevel%
)
echo Compilation successful. Running EmployeePayrollSystem.exe...
echo.
EmployeePayrollSystem.exe
pause
