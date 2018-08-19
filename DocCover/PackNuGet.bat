cd /D "%~dp0"
msbuild DocCover.csproj -t:rebuild -verbosity:quiet -p:Configuration=Pack
msbuild -t:Pack DocCover.csproj -p:PackageOutputPath=..\..\..\LocalNuGet -p:Configuration=Pack