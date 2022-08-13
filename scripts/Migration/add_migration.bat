:: use dotnet core cli to generate script
:: dotnet tool install --global dotnet-ef

echo off

cd ..

set /P migrationName="Please enter migration name: "

dotnet ef migrations add %migrationName% --project ../PCMS.Data.EF

echo generate migration successfully !

pause