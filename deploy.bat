@echo off
setlocal

title DriverReports Deploy

set SERVER=root@217.198.12.145
set REMOTE_PATH=/var/www/maniport-api
set PROJECT=C:\Users\Selecty\source\repos\DriverReports.Api\DriverReports.WebApi

echo.
echo =====================================
echo DriverReports Deploy
echo =====================================
echo.

cd /d %PROJECT%

echo [1/5] Publishing...
dotnet publish -c Release

if errorlevel 1 (
    echo.
    echo Publish FAILED!
    pause
    exit /b
)

echo.
echo [2/5] Uploading...

scp -r .\bin\Release\net8.0\publish\* %SERVER%:%REMOTE_PATH%

if errorlevel 1 (
    echo.
    echo Upload FAILED!
    pause
    exit /b
)

echo.
echo [3/5] Restarting service...

ssh %SERVER% "systemctl restart driverreports"

if errorlevel 1 (
    echo.
    echo Restart FAILED!
    pause
    exit /b
)

echo.
echo [4/5] Waiting...

timeout /t 3 >nul

echo.
echo [5/5] Checking service...

ssh %SERVER% "systemctl is-active driverreports"

echo.
echo =====================================
echo Deploy finished successfully!
echo =====================================
echo.

start https://manibase.ru/swagger

pause