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

        public Form1(Bitmap b)
        {
            InitializeComponent();
            /// stole the approach from
            // https://github.com/lokesh/lightbox2/blob/master/js/lightbox.js
            
            // max possible w/h
            int miW = SystemInformation.VirtualScreen.Width - 
                Lightbox.Properties.Settings.Default.Padding * 2;
            int miH = SystemInformation.VirtualScreen.Height - 
                Lightbox.Properties.Settings.Default.Padding * 2;
            
            // if we need to resize...
            if (b.Width > miW || b.Height > miH)
            {   
                // if we're too large on the width side
                if (b.Width / miW > b.Height / miW)
                {
                    pictureBox1.Width = miW;
                    pictureBox1.Height = b.Height / (b.Width / miH);
                }
                // if we're too large on the height side
                else
                {
                    pictureBox1.Height = miH;
                    pictureBox1.Width = b.Width / (b.Height / miW);
                }
            }
            // if not, let's just use its size!
            else
            {
                pictureBox1.Width = b.Width;
                pictureBox1.Height = b.Height;
            }

            pictureBox1.Image = b;
            this.Width = pictureBox1.Width;
            this.Height = pictureBox1.Height;

            // set up events to close the window et al
            this.Click += onClick;
            this.LostFocus += onClick;
            this.KeyDown += onKey;

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
    }
}
