cd /D "%~dp0"
msbuild DocCover.csproj /t:rebuild /verbosity:quiet /p:Configuration=Pack
nuget pack DocCover.csproj -OutputDirectory ..\..\..\LocalNuGet -Prop Configuration=Pack -Tool