@echo off

echo Normal pattern:
Example.exe /paramInt=10 /paramBool /paramString=ABCDEFG
echo;

echo Format error/Unknown param pattern:
Example.exe ###paramInt=-10 /paramAAA
echo;

pause