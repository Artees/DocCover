cd /D "%~dp0"
cd ..\..\..\LocalNuGet
nuget push DocCover.1.1.2.nupkg -Source https://api.nuget.org/v3/index.json