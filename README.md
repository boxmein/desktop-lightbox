# lightbox

A simple desktop application to display images in an uncluttered way.

## Usage

lightbox has two usage modes: 

(1) just running the executable makes an Open File dialog upon which you can
pick an image file to then view. The file types it supports are listed on the
[FreeImage](http://freeimage.sourceforge.net/features.html) page, plus SVG.

(2) on the command line, one can pass the filename as the **first** argument to
the process.

## Windows oddities

This is also implemented as a file handler via the registry entries and batch 
files. You have to run those to get file handling, they're a sort of makeshift 
installer. 

I've made sure not to leave any cruft into the registry post-uninstallation, 
but just in case, the keys it modifies are `HKEY_CLASSES_ROOT\(.jpg|.png|.tif|.gif|.bmp)\OpenWithProgids\lightboxasdgasfggre` and 
`HKEY_CLASSES_ROOT\lightboxasdgasfggre`. Those are used to add the program into 
the Open With dialog. Another thing - since the registry needs fixed paths, the
'installation' process will create a copy of Lightbox.exe into `C:\lightbox`.

You can modify all of these inside the `add_to_open_with(.bat|.reg)` and 
`remove_from_open_with(.bat|.reg)` files.

## Compiling

I've got a Visual Studio 2012 project, but aside that, yeah, nothing really.
I'll try and figure out a Mono compile someday.

Most image types are loaded using [FreeImage](http://freeimage.sourceforge.net)
(through FreeImage.NET) and a fork of [SVG.NET](https://github.com/vvvv/SVG).
The 32-bit Dlls are included in the `lib` folder.

The large list of file extensions is maintained using the `filetypes.py` script.
It generates a C# property file and the .reg files automagically.

## Credit

Made primarily on Windows and for Windows, however I'm fairly sure this won't be 
hard to port to support Mono. 

Made by boxmein 2014-04-14 02:25  
[**Feedback**][eml].


[eml]: mailto:boxmein@boxmein.x10.mx

