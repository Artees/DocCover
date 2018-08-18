cd /D "%~dp0"
msbuild DocCover.csproj /t:rebuild /verbosity:quiet /p:Configuration=Pack
msbuild -t:Pack DocCover.csproj -p:NuspecFile=DocCover.nuspec -p:PackageOutputPath=..\..\..\LocalNuGet -p:Configuration=Pack -p:IsTool=true