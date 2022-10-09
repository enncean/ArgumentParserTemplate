@echo off

set COMPILER_PATH=C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe
set SRC_PATH=*.cs ..\*.cs

%COMPILER_PATH% %SRC_PATH%

pause