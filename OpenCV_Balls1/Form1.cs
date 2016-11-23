using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace OpenCV_Balls1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (openFileDialog.ShowDialog(this) != DialogResult.Cancel)
            {
                Emgu.CV.Image<Rgb, Byte> Img = new Emgu.CV.Image<Rgb, Byte>(openFileDialog.FileName);
                imgBox.Image = Img;
                this.Text = openFileDialog.FileName;
                // Get image data that is stored in the image from first pixels?
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Size sz = this.ClientSize;
            sz.Height = sz.Height - this.MainMenuStrip.Height-5;
            imgBox.ClientSize = sz;
          
        }

        private void butClearOp_Click(object sender, EventArgs e)
        {
            imgBox.ClearOperation();
        }

    }
}
