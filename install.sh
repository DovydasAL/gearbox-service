dotnet restore
dotnet publish -c Release -o ./publish/GearboxService
sc.exe create GearboxService binpath= $(pwd)/publish/GearboxService/GearboxService.exe
