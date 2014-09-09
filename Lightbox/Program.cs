using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using FreeImageAPI;
using Svg;

namespace Lightbox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string filename = String.Empty;

            // If no command line arguments are provided, we're opening a file!
            if (args.Length == 0)
            {
                OpenFileDialog d = new OpenFileDialog();
                // our long list of supported files
                d.Filter = Lightbox.Properties.Filetypes.Filter;
                d.AutoUpgradeEnabled = false;
                
                if (d.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
                filename = d.FileName;
            }
            // If there are, it's argument #1 we care of!
            else {
                filename = args[0];
            }

            // File doesn't exist, let's die
            if (!File.Exists(filename))
            {
                MessageBox.Show(
                    // no stack trace
                    String.Format(Lightbox.Properties.Resources.FileNotFoundDescriptionN,
                        args[0]),
                    Lightbox.Properties.Resources.FileNotFoundTitle +
                        " - lightbox"
                );
                Application.Exit();
                return;
            }
            
            
            // we got us a catch!
            Image img = null;
            try {
            	using (FileStream fs = File.OpenRead(filename))
            	{
                	img = LoadImage(fs);
            	}
            }
            catch (Exception e)
            {
                // one of the libs raised an exception
                MessageBox.Show(
                    // stack trace
                    String.Format(Lightbox.Properties.Resources.ExceptionDescription,
                        filename, e.ToString()),
                    Lightbox.Properties.Resources.ExceptionTitle +
                        " - lightbox"
                );
            }
            // all libs denied to load our image (maybe it wasn't one after all)
            // we could complain to the user but just go down for now
            if (img == null)
            {
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();

            Form f = new Form1(img);
            f.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(f);
        }
        
        static Image LoadImage(FileStream fs)
        {
            // try GDI+
            Image img = LoadImageGDI(fs);
            if (img == null)
            {
                // nah, try FreeImage
                img = LoadImageFI(fs);
            }
            if (img == null)
            {
                // maybe SVG works
                img = LoadImageSVG(fs);
            }
            return img;
        }
        
        static Image LoadImageGDI(FileStream fs)
        {
            try
            {
            	// GDI+ really hates it when its stream gets disposed
            	// so we first check if it can actually load it
            	Image.FromStream(fs);
            	// and then let it do its stuff from another stream
            	return Image.FromFile(fs.Name);
            }
            catch (ArgumentException)
            {
                // this is what GDI+ throws when it fails to load a bitmap
                return null;
            }
        }
        
        static Image LoadImageSVG(FileStream fs)
        {
            try
            {
                // try SVG.NET
                SvgDocument svg = SvgDocument.Open<SvgDocument>(fs, null);
                return svg.Draw();
            }
            catch (System.Xml.XmlException)
            {
                // this is what SVG.NET throws when it fails
                return null;
            }
        }
        
        static Image LoadImageFI(FileStream fs)
        {
            Image img = null;
            try
            {
                // let's try FreeImage
                FreeImageBitmap fib = new FreeImageBitmap(fs);
                img = (Bitmap)fib;
                return img;
            }
            catch (Exception e)
            {
                if (e is DllNotFoundException)
                {
                    // we can't reach the DLL, complain about that
                    MessageBox.Show(
                        // stack trace
                        String.Format(Lightbox.Properties.Resources.DllNotFoundDescription,
                            fs.Name, e.ToString()),
                        Lightbox.Properties.Resources.DllNotFoundTitle +
                            " - lightbox"
                    );
                    return null;
                }
                else {
                    // FI threw some other exception, so it can't load either
                    return null;
                }
            }
        }
    }
}
