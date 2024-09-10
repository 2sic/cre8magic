del "*.nupkg"
"..\..\..\oqtane.framework\oqtane.package\nuget.exe" pack ToSic.Theme.Cre8magicTests.nuspec 
XCOPY "*.nupkg" "..\..\..\oqtane.framework\Oqtane.Server\wwwroot\Packages\" /Y
