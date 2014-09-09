using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lightbox
{
    public partial class Form1 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public Form1(Image img)
        {
            InitializeComponent();
            
            // max possible w/h
            int miW = SystemInformation.VirtualScreen.Width - 
                Lightbox.Properties.Settings.Default.Padding * 2;
            int miH = SystemInformation.VirtualScreen.Height - 
                Lightbox.Properties.Settings.Default.Padding * 2;
            
            // if we need to resize...
            if (img.Width > miW || img.Height > miH)
            {   
                double ratio = Math.Min((double)miW / img.Width, (double)miH / img.Height);
                this.Width = (int)Math.Floor(img.Width * ratio);
                this.Height = (int)Math.Floor(img.Height * ratio);
            }
            // if not, let's just use its size!
            else
            {
                this.Width = img.Width;
                this.Height = img.Height;
            }

            pictureBox1.Image = img;

            // set up events to close the window et al
            this.Click += onClick;
            this.LostFocus += onClick;
            this.KeyDown += onKey;
            this.Closed += onClosed;

            pictureBox1.Click += onClick;
            // pictureBox1.MouseWheel += onWheel;
        }

        // Close action: toggled by 
        // clicking and the window losing focus. 
        public void onClick(object sender, EventArgs args)
        {
            Application.Exit();
        }

        public void onKey(object sender, KeyEventArgs args)
        {

        }
        
        public void onClosed(object sender, EventArgs args)
        {
            pictureBox1.Image.Dispose();
        }
    }
}
