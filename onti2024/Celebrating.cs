using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace onti2024
{
    public partial class Celebrating : Form
    {
        private int timp = 0;
        public Celebrating()
        {
            InitializeComponent();
        }

        private void Celebrating_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timp += 100;
            if (timp >= 2000)
            {
                this.Close();
            }
            else
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = new Size(30, 30);
                Random rnd = new Random();
                pictureBox.Image = imageList1.Images[rnd.Next(0,imageList1.Images.Count-1)];
                pictureBox.Location = new Point(rnd.Next(0,this.Width), rnd.Next(0,this.Height));
                this.Controls.Add(pictureBox);
            }
        }
    }
}
