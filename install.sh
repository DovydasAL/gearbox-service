dotnet restore
dotnet publish -c Release -o ./publish/GearboxService
sc.exe create 'Shift Redemption Service' binpath= $(pwd)/publish/GearboxService/GearboxService.exe
