@echo off
setlocal

set PROJECT=a.csproj
set OUT=publish

dotnet publish %PROJECT% ^
 -c Release ^
 -r win-x64 ^
 --self-contained true ^
 --no-restore ^
 /p:PublishSingleFile=true ^
 /p:PublishTrimmed=false ^
 /p:PublishReadyToRun=false ^
 -o %OUT%

if %ERRORLEVEL% NEQ 0 (
  echo ❌ Publish FAILED
  exit /b 1
)

echo ✅ Publish SUCCESS
pause
