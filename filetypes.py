import sys

filetypes =  [
    ["Portable Network Graphics", ["png"]],
    ["JPEG images", ["jpg", "jpeg", "jpe"]],
    ["Graphics Interchange Format", ["gif", "gfa", "giff"]],
    ["Windows Bitmap images", ["bmp", "dib", "rle", "2bp"]],
    ["Tagged Image File Format", ["tiff", "tif"]],
    ["Scalable Vector Graphics", ["svg", "svgz"]],
    ["Windows Icons", ["ico", "cur"]],
    ["Dr. Halo Bitmaps", ["cut"]],
    ["DirectDraw Surfaces", ["dds"]],
    ["OpenEXR images", ["exr"]],
    ["Raw Fax G3", ["g3"]],
    ["IFF images", ["iff"]],
    ["JBIG images", ["jbig", "bie", "jbg"]],
    ["JPEG Network Graphics", ["jng"]],
    ["JIF JPEG container", ["jif", "jfif", "jfi"]],
    ["JPEG 2000 images", ["jp2", "j2c", "jpc", "j2k", "jpx"]],
    ["JPEG XR images", ["wdp", "hdp", "jxr"]],
    ["KOALA images", ["koa", "kla", "gg"]],
    ["Kodak Photo CD images", ["pcd"]],
    ["Multiple-image Network Graphics", ["mng"]],
    ["ZSoft PC Paintbrush File", ["pcx", "pcc", "dcx"]],
    ["Netpbm image formats", ["ppm", "pgm", "pbm", "pfm"]],
    ["Macintosh PICT Format", ["pict", "pic", "pct", "pct1", "pct2"]],
    ["Photoshop Document", ["psd", "psb", "pdb", "pdd"]],
    ["Raw image formats", [
        "3fr", "ari", "arw", "ay", "rw", "cr2", "cap", "cs",
        "dcr", "dng", "drf", "ip", "erf", "ff", "iq", "25", "kdc", "dc", "mef",
        "mos", "mrw", "ef", "nrw", "bm", "orf", "ef", "ptx", "pxn", "3d", "raf",
        "raw", "rwl", "rw2", "rwz", "r2", "srf", "srw", "x3f"
    ]],
    ["Sun raster files", ["sun", "ras", "rs", "im1", "im8", "im24", "im32"]],
    ["Silicon Graphics Image", ["sgi", "rgb", "rgba", "bw", "int", "inta", "icon"]],
    ["Truevision Targa", ["tga", "tpic"]],
    ["Wireless Application Protocol Bitmap Format", ["wbmp", "wbm", "wbp"]],
    ["WebP", ["webp"]],
    ["X Bitmap", ["xbm", "bm", "icon", "bitmap"]],
    ["X Pixmap", ["xpm", "pm"]]
]


if __name__ == "__main__":
    # Write a C# source file
    with open("./Lightbox/Properties/Filetypes.cs", "wt") as f:
        f.write("// This file was generated automagically by filetypes.py\n"
            + "namespace Lightbox.Properties\n"
            + "{\n"
            + "    internal class Filetypes {\n")

        allexts = []
        lines = []
        for i in filetypes:
            exts = i[1]
            if len(exts) < 5:
                extstring = "." + ", .".join(exts)
            else:
                extstring = "." + ", .".join(exts[:5]) + "..."
            line = "\"{0} ({1})|*.{2}|\"".format(i[0], extstring, ";*.".join(exts))
            lines.append(line)
            allexts.append(";*.".join(exts))

        f.write(" "*8 + "public static string Filter = \"All supported images|*." + ";*.".join(allexts) + "|\"\n")
        for i in lines:
            f.write(" "*12 + "+ " + i + "\n")
        f.write(" "*12 + "+ \"All files (.*)|*.*\";\n"
            + " "*4 + "}\n"
            + "}\n")


    # Write the installer/uninstaller reg files
    with open("./win/add_to_open_with.reg", "wt") as a:
        with open("./win/remove_from_open_with.reg", "wt") as b:
            a.write("Windows Registry Editor Version 5.00\n; This file was generated automagically by filetypes.py\n")
            b.write("Windows Registry Editor Version 5.00\n; This file was generated automagically by filetypes.py\n")
            for i in filetypes:
                for ext in i[1]:
                    a.write("[HKEY_CLASSES_ROOT\\." + ext + "\\OpenWithProgids]\n")
                    a.write("\"lightboxasdgasfggre\"=hex(0):\n")

                    b.write("[HKEY_CLASSES_ROOT\\." + ext + "\\OpenWithProgids]\n")
                    b.write("\"lightboxasdgasfggre\"=-\n")

            a.write("\n")
            b.write("\n")

            a.write("[HKEY_CLASSES_ROOT\lightboxasdgasfggre]\n"
                + "@=\"Image File\"\n"
                + "\n"
                + "[HKEY_CLASSES_ROOT\lightboxasdgasfggre\shell]\n"
                + "[HKEY_CLASSES_ROOT\lightboxasdgasfggre\shell\open]\n"
                + "[HKEY_CLASSES_ROOT\lightboxasdgasfggre\shell\open\command]\n"
                + "@=\"\\\"C:\\\\lightbox\\\\Lightbox.exe\\\" \\\"%1\\\"\"\n")

            b.write("[-HKEY_CLASSES_ROOT\lightboxasdgasfggre\shell\open\command]\n"
                + "[-HKEY_CLASSES_ROOT\lightboxasdgasfggre\shell\open]\n"
                + "[-HKEY_CLASSES_ROOT\lightboxasdgasfggre\shell]\n"
                + "[-HKEY_CLASSES_ROOT\lightboxasdgasfggre]\n")
