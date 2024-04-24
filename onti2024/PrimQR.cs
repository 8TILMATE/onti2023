using onti2024.Helpers;
using onti2024.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
namespace onti2024
{
    public partial class PrimQR : Form
    {
        
        public PrimQR()
        {
            InitializeComponent();
        }
        private UserModel model = new UserModel();

        private void PrimQR_Load(object sender, EventArgs e)
        {
            DatabaseHelper.GetAllGames();
            int difscor = 0;
            foreach(var game in DatabaseHelper.AllResultModels)
            {
                int difscorlocal = 0;
                for(int i = game.Punctaj; i < 100; i++)
                {
                    if (CheckifPrim(i))
                    {
                        difscorlocal = i - game.Punctaj;
                        break;
                        
                    }
                }
                if(difscorlocal > difscor) 
                {
                    model = DatabaseHelper.GetFromEmail(game.EmailUser);
                    difscor = difscorlocal;
                }
            }
        }
        private bool CheckifPrim(int nr)
        {
            bool eprim = true;
            for(int i = 2; i <= nr / 2; i++)
            {
                if(nr%i == 0)
                {
                    eprim = false;
                }
            }
            return eprim;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           QRCodeEncoder encoder = new QRCodeEncoder();
            string info = model.Nume.Trim() + " " + model.email.Trim() + " " + model.password.Trim();
            encoder.QRCodeScale = 8;
            pictureBox1.Image = encoder.Encode(info);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
