@echo off
echo Starting CRC WebPortal...
echo.

echo Starting API Server...
start "CRC WebPortal API" cmd /k "cd CRC.WebPortal.API && dotnet run --urls https://localhost:7000"

echo Waiting for API to start...
timeout /t 5 /nobreak > nul

echo Starting Blazor UI...
start "CRC WebPortal UI" cmd /k "cd CRC.WebPortal.BlazorUI && dotnet run --urls https://localhost:7001"

echo.
echo Both applications are starting...
echo API: https://localhost:7000
echo UI: https://localhost:7001
echo Swagger: https://localhost:7000/swagger
echo.
pause
