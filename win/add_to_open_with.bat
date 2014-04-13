@echo off 
mkdir C:\lightbox

:: Copy over files
copy /V Lightbox.exe /B C:\lightbox
copy /V README.md C:\lightbox
copy /V remove_from_open_with.bat C:\lightbox
copy /V remove_from_open_with.reg C:\lightbox

:: Registry witchcraft
echo Copied Lightbox.exe over to C:\lightbox. Associating file types...
echo Associating .jpg, .gif, .png, .bmp, .tif with this program...
echo (you still have to pick the program from the Open With menu) 
regedit /S add_to_open_with.reg