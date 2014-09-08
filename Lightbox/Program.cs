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
            Bitmap b = null;
            using (FileStream fs = File.OpenRead(filename))
            {
                try {
                    b = LoadImage(fs);
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
            }
            // all libs denied to load our image (maybe it wasn't one after all)
            // we could complain to the user but just go down for now
            if (b == null)
            {
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();

            Form f = new Form1(b);
            f.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(f);
        }
        
        static Bitmap LoadImage(FileStream fs)
        {
            // try .Net
            Bitmap b = LoadImageNet(fs);
            if (b == null)
            {
                // nah, try FreeImage
                b = LoadImageFI(fs);
            }
            if (b == null)
            {
                // maybe SVG works
                b = LoadImageSVG(fs);
            }
            return b;
        }
        
        static Bitmap LoadImageNet(FileStream fs)
        {
            try
            {
                // try to load with .Net only
                return new Bitmap(fs);
            }
            catch (ArgumentException)
            {
                // this is what .Net throws when it fails to load a bitmap
                return null;
            }
        }
        
        static Bitmap LoadImageSVG(FileStream fs)
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
        
        static Bitmap LoadImageFI(FileStream fs)
        {
            Bitmap b = null;
            try
            {
                // let's try FreeImage
                FreeImageBitmap fib = new FreeImageBitmap(fs);
                b = (Bitmap)fib;
                return b;
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
