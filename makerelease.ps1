del KeybooardUsage.zip
del KeyboordUsage.zip

& 'C:\Program Files\7-Zip\7z.exe' a -tzip KeyboordUsage.zip .\code\bin\debug\* -xr!bin -xr!obj -xr!*xml -xr!*pdb -xr!*"vshost.exe*"
