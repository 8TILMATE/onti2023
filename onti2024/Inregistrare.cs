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
    public partial class Inregistrare : Form
    {
        public Inregistrare()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseHelper.InsertUser(new Models.UserModel { email=textBox1.Text,password=textBox2.Text,Nume=textBox3.Text});
            
        }
    }
}
