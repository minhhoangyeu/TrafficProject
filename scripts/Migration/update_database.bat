
echo off

cd ..

dotnet ef migrations script --idempotent --output "Migration/PCMS.sql" --project ../PCMS.Data.EF

echo generate script successfully !

pause