cd /D "%~dp0"
cd ..\..\..\LocalNuGet
nuget push DocCover.1.0.0.nupkg -Source https://api.nuget.org/v3/index.json