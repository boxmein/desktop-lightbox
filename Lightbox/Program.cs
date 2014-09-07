using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

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
                d.Filter = "Image Files (*.bmp, *.gif, *.jpg, *.png, *.tif)|*.bmp;*.jpg;*.png;*.gif;*.tif";
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
            // now to check if it's a valid image!
            Bitmap b = null;
            try
            {
                b = new Bitmap(filename);
            }
            // File.exists(args[0]) = true,
            // new Bitmap(args[0]) = not found, will this ever happen?
            catch (Exception e)
            {
            	if (e is FileNotFoundException) {
	                MessageBox.Show(
	            		// stack trace
	                    String.Format(Lightbox.Properties.Resources.FileNotFoundDescription,
	                        args[0], e.ToString()),
	            		Lightbox.Properties.Resources.FileNotFoundTitle +
	                        " - lightbox"
	                );
            	} else {
            		MessageBox.Show(
	            		// stack trace
	                    String.Format(Lightbox.Properties.Resources.ExceptionDescription,
	                        args[0], e.ToString()),
	            		Lightbox.Properties.Resources.ExceptionTitle +
	                        " - lightbox"
	                );
            	}
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();

            Form f = new Form1(b);
            f.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(f);
        }
    }
}
