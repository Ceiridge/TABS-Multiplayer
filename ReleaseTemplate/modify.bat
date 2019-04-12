@echo off
REM please ignore my bad batch skills :((((
echo Enter the full/absolute path to the TABS root directory here:
set /p tabspath=
echo Set path to %tabspath%

cd MonoMod
mkdir GameDLLs
copy /Y "%tabspath%\TotallyAccurateBattleSimulator_Data\Managed\*.dll" GameDLLs\
copy /Y *.mm.dll GameDLLs\

cd GameDLLs
..\MonoMod.exe Assembly-CSharp.dll
copy /Y MONOMODDED_Assembly-CSharp.dll ..\..\Assembly-CSharp.dll

cd ..
echo .
echo .
echo .
echo .
echo .
echo .
echo .
echo .
echo DLL (probably) MODIFIED! YOU CAN NOW COPY THE UI AND THE DLL
echo THEN OPEN THE UI IN THE DIRECTORY OF TABS!
pause