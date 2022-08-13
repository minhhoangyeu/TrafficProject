
echo off

cd ..

dotnet ef database drop --project ../PCMS.Data.EF

echo drop database successfully !

pause