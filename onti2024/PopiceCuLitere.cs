using onti2024.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace onti2024
{
    public partial class PopiceCuLitere : Form
    {
        public PopiceCuLitere()
        {
            InitializeComponent();
        }
        int timp = 100;
        private bool moving =false;
        private PictureBox pictureBox = new PictureBox();
        private string mesaj = "";
        private void PopiceCuLitere_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            Random random = new Random();
            int x1, x2;
            x1 = random.Next(0, imageList1.Images.Count - 1);
            x2 = random.Next(0, imageList1.Images.Count - 1);
            Image c1 = imageList1.Images[x1];
            Image c2 = imageList1.Images[x2];
            pictureBox1.Image = c1;
            pictureBox2.Image = c2;
            mesaj = imageList1.Images.Keys[x1].Remove(imageList1.Images.Keys[x1].Length - 4, 4)+ imageList1.Images.Keys[x2].Remove(imageList1.Images.Keys[x2].Length - 4, 4);
            Console.WriteLine(mesaj);
            string Shuffled = Scramble(mesaj);
            int xx = 30, yy = 30;
            foreach(char x in Shuffled)
            {
                Label label = new Label();
                label.Size = new Size(40, 40);
                label.Location = new Point(xx, yy);
                label.Text = x.ToString();
                label.Font = new Font("Microsoft Sans Serif", 20);
                this.Controls.Add(label);
                xx += 60;
            }

        }
        private string Scramble(string s)
        {
            return new string(s.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void PopiceCuLitere_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void PopiceCuLitere_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Left)
            {
                pictureBox3.Location= new Point(pictureBox3.Location.X-10, pictureBox3.Location.Y);    
            }
            if(e.KeyCode == Keys.Right)
            {
                pictureBox3.Location = new Point(pictureBox3.Location.X + 10, pictureBox3.Location.Y);

            }
            if (e.KeyCode == Keys.Space)
            {
                pictureBox=new PictureBox();
                pictureBox.Image = pictureBox3.Image;
                pictureBox.Size= pictureBox3.Size;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Location = pictureBox3.Location;
                this.Controls.Add(pictureBox);
                moving = true;
                timer3.Start();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if(moving == true)
            {
                pictureBox.Location= new Point(pictureBox.Location.X, pictureBox.Location.Y-10);
                foreach(Control x in this.Controls) 
                {
                    try
                    {
                        Label lbl = (Label)x;
                        if (pictureBox.Bounds.IntersectsWith(lbl.Bounds))
                        {
                            label2.Text += lbl.Text;
                            pictureBox.Image = null;
                            pictureBox = null;
                            moving = false;
                            timer3.Stop();
                            break;
                        }

                    }
                    catch { }
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timp > 0)
            {
                label1.Text = timp.ToString();
                timp--;
                if (mesaj == label2.Text)
                {
                    timer1.Stop();
                    timer3.Stop();
                    timer2.Stop();
                    MessageBox.Show("Winner!!!");
                    DatabaseHelper.InsertGames(new Models.RezultateModel
                    {
                        Data  = DateTime.ParseExact(DateTime.Now.ToString("dd.MM.yyyy"), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EmailUser = DatabaseHelper.userLogat.email,
                        Punctaj = 100,
                        Tip = 1
                    });
                }
                if(!mesaj.Contains(label2.Text.Trim())&&label2.Text!="")
                {
                    timer1.Stop();
                    timer3.Stop();
                    timer2.Stop();

                    MessageBox.Show("Ai pierdut");
                    DatabaseHelper.InsertGames(new Models.RezultateModel
                    {
                        Data = DateTime.ParseExact(DateTime.Now.ToString("dd.MM.yyyy"), "dd.MM.yyyy",CultureInfo.InvariantCulture),
                        EmailUser = DatabaseHelper.userLogat.email,
                        Punctaj = 0,
                        Tip = 1
                    });
                }
            }
            else
            {
                timer1.Stop();
                timer3.Stop();
                timer2.Stop();

                MessageBox.Show("Ai pierdut");
                DatabaseHelper.InsertGames(new Models.RezultateModel
                {
                    Data = DateTime.ParseExact(DateTime.Now.ToString("dd.MM.yyyy"), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    EmailUser = DatabaseHelper.userLogat.email,
                    Punctaj = 0,
                    Tip = 1
                });
            }
        }
    }
}
