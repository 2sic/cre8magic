dotnet build -c Release ..\ToSic.Cre8magic.Client\ToSic.Cre8magic.Client.csproj
robocopy %~dp0..\ToSic.Cre8magic.Client\bin\Pack\ %~dp0 *.nupkg
