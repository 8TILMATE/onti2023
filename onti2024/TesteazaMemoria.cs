using onti2024.Helpers;
using onti2024.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace onti2024
{
    public partial class TesteazaMemoria : Form
    {
        List<PictureBox> pictureBoxes = new List<PictureBox>();
        int[] contori = new int[30];
        List<PictureBox> imaginideschise = new List<PictureBox>();
        private PictureBox pictureBox = null;
        private PictureBox picturebox2=null;
        private int scor = 0;
        private int nrelemente;
        private int timp = 0;
        private int dificultate;
        private int overallscorr;
        public TesteazaMemoria(int dif,int overallscor)
        {
            InitializeComponent();
            if (dificultate < 6)
            {
                dificultate = dif;
                
            }
            else
            {
                overallscorr = overallscor;
                MessageBox.Show("Winner!!!");
                RezultateModel model = new RezultateModel
                {
                    EmailUser = DatabaseHelper.userLogat.email,
                    Data = DateTime.Now,
                    Punctaj = overallscorr,
                    Tip = 0
                };
                DatabaseHelper.InsertGames(model);
            }
            overallscorr = overallscor;
        }

        private void TesteazaMemoria_Load(object sender, EventArgs e)
        {
            bool nextline = false;
            nrelemente = 2 * Functie(dificultate);
            Console.WriteLine(nrelemente.ToString());
            int x = 20, y = 20;
            for (int i = 1; i <= nrelemente; i++) 
            {
                Random random = new Random();
                int count;
                if (!nextline)
                {
                    count = random.Next(0, imageList1.Images.Count);
                    while (contori[count] >= 1)
                    {
                        count = random.Next(0, imageList1.Images.Count);
                    }
                }
                else
                {
                    count = random.Next(0, imageList1.Images.Count);
                    while (contori[count] !=1 )
                    {
                        count = random.Next(0, imageList1.Images.Count);
                    }

                }
                contori[count]++;

                PictureBox pictureBox = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size((this.Size.Width / nrelemente) - 30, (this.Size.Width / nrelemente) - 30),
                    Location = new Point(x, y),
                    BackColor = Color.Gray,
                    Tag = imageList1.Images.Keys[count]
                };
                pictureBox.Click += ShowImage;
                pictureBox.Visible = false;
                if (i < nrelemente / 2||nextline)
                {
                    x += (this.Size.Width / nrelemente) - 20;

                }
                else if (nextline == false)
                {
                    x = 20;
                    y += (this.Size.Width / nrelemente);
                    nextline = true;
     
                }

                pictureBoxes.Add(pictureBox);
                this.Controls.Add(pictureBox);
            }
        
            
        }
        private int Functie(int param)
        {
            if (param > 2)
            {
                return Functie(param-1)+Functie(param-2);
            }
            else
            {
                return 1;
            }
        }
        private void ShowImage(object obj,EventArgs e)
        {
            if (imaginideschise.Count < 2)
            {
                PictureBox pictureBox1 = obj as PictureBox;
                pictureBox1.ImageLocation = pictureBox1.Tag.ToString();
                pictureBox1.BackColor = Color.Transparent;
                imaginideschise.Add(pictureBox1);
                Thread x = new Thread(() => Start(pictureBox1));
                x.Start();
            }


        }
        private async void Start(PictureBox p)
        {
            bool matched=false;
            foreach (Control c in this.Controls)
            {
                try
                {
                    PictureBox poza = (PictureBox)c;
                    if (poza.Tag == p.Tag &&imaginideschise.Contains(poza)&&imaginideschise.Contains(p)&&poza!=p)
                    {
                      pictureBox = poza;
                        picturebox2 = p;
                      matched=true; break;
                    }
                }
                catch
                {

                }
            }
            if (!matched)
            {
                await Task.Delay(2000);
                foreach (Control c in this.Controls)
                {
                    try
                    {
                        PictureBox poza = (PictureBox)c;
                        if (poza == p)
                        {
                            if (pictureBox != poza)
                            {
                                poza.ImageLocation = string.Empty;
                                poza.BackColor = Color.Gray;
                            }
                        }
                    }
                    catch
                    {

                    }
                }

            }
            if (matched)
            {
              
                
            }
            imaginideschise.Remove(p);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                try
                {
                    PictureBox poza = (PictureBox)c;
                    poza.Visible = true;
                }
                catch
                {

                }
            }
            timer1.Start();
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text=(100-timp).ToString();
            if (timp == 100)
            {
                timer1.Stop();
                this.Hide();
                MessageBox.Show("Joc Pierdut");
                RezultateModel model = new RezultateModel
                {
                    EmailUser = DatabaseHelper.userLogat.email,
                    Data = DateTime.Now,
                    Punctaj = overallscorr + scor,
                    Tip = 0
                };
                DatabaseHelper.InsertGames(model);
                

            }
            if(scor==2*(dificultate-1))
            {
                this.Hide();
                timer1.Stop();
                var celebrate = new Celebrating();
                celebrate.ShowDialog();
                var newGame = new TesteazaMemoria(dificultate + 1,overallscorr+scor);
                newGame.ShowDialog();
                this.Close();
            }
            timp++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pictureBox != null)
            {
                pictureBox.ImageLocation = pictureBox.Tag.ToString();
                pictureBox.BackColor = Color.Transparent;
                imaginideschise.Remove(pictureBox);
                pictureBox = null;
                picturebox2.ImageLocation = picturebox2.Tag.ToString();
                picturebox2.BackColor = Color.Transparent;
                imaginideschise.Remove(picturebox2);
                picturebox2 = null;
                scor += 2;
            }
        
        }
    }
}
