using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if(openFileDialog.ShowDialog(this) != DialogResult.Cancel)
            {
                Bitmap img = new Bitmap(openFileDialog.FileName);
                this.pictBox.Image = img;
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Size sz = this.ClientSize;
            pictBox.ClientSize = sz;
        }


    }
}
