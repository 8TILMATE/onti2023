using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using onti2024.Helpers;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseHelper.DatabaseStarter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            string Result = dialog.FileName;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.ImageLocation = Result;
            pictureBox1.Image = Image.FromFile(Result);
            
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodare = decoder.decode(new QRCodeBitmapImage(pictureBox1.Image as Bitmap));
            var line = decodare.Split('\n');
            textBox1.Text = line[1].ToString() ;
            textBox2.Text = line[2].ToString() ;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(DatabaseHelper.CheckUser(new Models.UserModel { email=textBox1.Text,password=textBox2.Text }))
            {
                MessageBox.Show("Logat!");
                DatabaseHelper.GetGames(new Models.UserModel { email = textBox1.Text, password = textBox2.Text });
                this.Hide();
                AlegeJoc alegeJoc = new AlegeJoc();
                alegeJoc.ShowDialog();
                this.Close();
                
            }
            else
            {
                MessageBox.Show("A aparut o problema la autentificare...");
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Pagina = new Inregistrare();
            Pagina.ShowDialog();
            
        }
    }
}
