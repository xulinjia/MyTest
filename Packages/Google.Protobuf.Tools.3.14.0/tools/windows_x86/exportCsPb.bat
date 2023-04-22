@echo off
for %%i in (*.proto) do (
    protoc --csharp_out=./ %%i
    rem 从这里往下都是注释，可忽略
    echo From %%i To %%~ni.cs Successfully!  
)
del /q ..\Assets\Scripts\Game\Pb
set FileExt=*.pb
for %%a in (%FileExt%) do copy  /y "%%~a" ..\Assets\Scripts\Game\Pb
del /q %FileExt%
copy *.lua ..\Assets\Scripts\Game\Pb

pause
