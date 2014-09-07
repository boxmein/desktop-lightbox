using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using FreeImageAPI;

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
            Bitmap b = LoadImage(filename);
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
        
        static Bitmap LoadImage(String filename)
        {
            // now to check if it's a valid image!
            Bitmap b = null;
            try
            {
                // try loading with .Net first
                b = new Bitmap(filename);
            }
            catch (Exception e1)
            {
                if (e1 is FileNotFoundException)
                {
                    // File.exists(args[0]) = true,
                    // new Bitmap(args[0]) = not found, will this ever happen?
                    MessageBox.Show(
                        // stack trace
                        String.Format(Lightbox.Properties.Resources.FileNotFoundDescription,
                            filename, e1.ToString()),
                        Lightbox.Properties.Resources.FileNotFoundTitle +
                            " - lightbox"
                    );
                }
                else if (e1 is ArgumentException)
                {
                    try
                    {
                        // .Net doesn't want to load our image
                        // let's try FreeImage
                        FreeImageBitmap fib = new FreeImageBitmap(filename);
                        b = (Bitmap)fib;
                    }
                    catch (Exception e2)
                    {
                        if (e2 is DllNotFoundException)
                        {
                            // we can't reach the DLL somewhy, give up loading
                            MessageBox.Show(
                                // stack trace
                                String.Format(Lightbox.Properties.Resources.DllNotFoundDescription,
                                    filename, e2.ToString()),
                                Lightbox.Properties.Resources.DllNotFoundTitle +
                                    " - lightbox"
                            );
                        }
                        else {
                            // FI threw some other exception
                            MessageBox.Show(
                                // stack trace
                                String.Format(Lightbox.Properties.Resources.FIExceptionDescription,
                                    filename, e2.ToString()),
                                Lightbox.Properties.Resources.FIExceptionTitle +
                                    " - lightbox"
                            );
                        }
                    }
                }
                else
                {
                    // Some weird other exception appeared while loading
                    MessageBox.Show(
                        // stack trace
                        String.Format(Lightbox.Properties.Resources.ExceptionDescription,
                            filename, e1.ToString()),
                        Lightbox.Properties.Resources.ExceptionTitle +
                            " - lightbox"
                    );
                }
            }
            return b;
        }
    }
}
