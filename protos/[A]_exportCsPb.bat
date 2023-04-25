@echo off
set list=%cd%

for %%a in (%list%) do (
	for %%p in (%%a\*.proto) do (
    protoc --csharp_out=./ %%~np.proto
    rem 从这里往下都是注释，可忽略
    echo From %%p To %%~np.cs Successfully!      	)
)
cd .
copy *.cs ..\Assets\Scripts\Game\Manager\Net\Pb
pause
