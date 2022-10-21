dotnet build -c Release ..\ToSic.Cre8Magic.Client\ToSic.Cre8Magic.Client.csproj
robocopy %~dp0..\ToSic.Cre8Magic.Client\bin\Pack\ %~dp0 *.nupkg
