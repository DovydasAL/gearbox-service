dotnet restore
dotnet publish -c Release -o ./publish/GearboxService
sc.exe create GearboxService binpath= ./publish/GearboxService/GearboxService.exe
