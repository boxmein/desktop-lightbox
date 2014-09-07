# packages the lightbox executable into a zip file. 
# does not compile however!!!
# usage: package.py (win|lin) <result-filename>
import zipfile, sys

def usage():
  print ("package.py: packages the lightbox executable into a zip file. does not compile, however!")
  print ("usage: package.py (win|lin) <result-filename>")
  exit(0)

if len(sys.argv) < 2: 
  usage()

if sys.argv[1] == 'win': 
  with zipfile.ZipFile(sys.argv[2], 'w') as z: 
    try: 
      z.write('Lightbox.exe')
    except: 
      print ("warning: ./Lightbox.exe is unreadable. Probably doesn't exist, that means.")
      try:
        z.write('Lightbox/bin/Release/Lightbox.exe', 'Lightbox.exe')
      except:
        print("fatal: ./Lightbox/bin/release/Lightbox.exe doesn't seem to be readable either. Are you sure you compiled Lightbox?")
      	exit(0)

    z.write('Lightbox/lib/FreeImage.dll', arcname='FreeImage.dll')
    z.write('Lightbox/lib/FreeImageNET.dll', arcname='FreeImageNET.dll')
    z.write('README.md')
    z.write('win/remove_from_open_with.bat', arcname='remove_from_open_with.bat')
    z.write('win/remove_from_open_with.reg', arcname='remove_from_open_with.reg')
    z.write('win/add_to_open_with.bat', arcname='add_to_open_with.bat')
    z.write('win/add_to_open_with.reg', arcname='add_to_open_with.reg')
  # done!

elif sys.argv[1] == 'lin': 
  print ("not so soon young padawan")
else: 
  print (sys.argv)
  usage()