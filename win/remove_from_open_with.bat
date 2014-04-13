@echo off

:: Registry witchcraft
echo Removing associations from .png, .jpg, .tif, .bmp, .gif:
echo Also removing the ProgID...
regedit /S remove_from_open_with.reg

echo Removing files...
@echo on
:: Delete over files
del C:\lightbox\Lightbox.exe
del C:\lightbox\README.md
del C:\lightbox\remove_from_open_with.bat
del C:\lightbox\remove_from_open_with.reg

rmdir C:\lightbox
