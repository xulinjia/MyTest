@echo off
set list=%cd%

for %%a in (%list%) do (
	for %%p in (%%a\*.proto) do (
    protoc --csharp_out=./ %%~np.proto
    protoc %%~np.proto -o %%~np.pb
    rem 从这里往下都是注释，可忽略
    echo From %%i To %%~ni.cs Successfully!      	)
)
cd .
del /q ..\Assets\Scripts\Game\Manager\Net\Pb
set FileExt=*.pb
for %%a in (%FileExt%) do copy  /y "%%~a" ..\Assets\Scripts\Game\Manager\Net\Pb
del /q %FileExt%
copy *.cs ..\Assets\Scripts\Game\Manager\Net\Pb
pause
