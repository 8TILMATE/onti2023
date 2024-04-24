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
using System.Windows.Forms.DataVisualization.Charting;

namespace onti2024
{
    public partial class AlegeJoc : Form
    {
        public AlegeJoc()
        {
            InitializeComponent();
        }

        private void AlegeJoc_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            label1.Text= "Bine ai venit, "+DatabaseHelper.userLogat.Nume+"("+DatabaseHelper.userLogat.email+")";    
            foreach(var rezultat in DatabaseHelper.RezultateModel)
            {
                bool exista = false;
                
                foreach(Series series in chart1.Series)
                {
                    if(series.LegendText== ConvertTipIntoName(rezultat.Tip))
                    {
                        series.Points.AddXY(rezultat.Data, rezultat.Punctaj);
                        exista = true;
                        
                    }
                }
                if (!exista)
                {
                    chart1.Series.Add(new Series { LegendText= ConvertTipIntoName(rezultat.Tip) });
                    chart1.Series[chart1.Series.Count - 1].ChartType = SeriesChartType.Line;
                    chart1.Series[chart1.Series.Count-1].Points.AddXY(rezultat.Data, rezultat.Punctaj);


                }
            }
        }
        private string ConvertTipIntoName(int TipJoc)
        {
            switch (TipJoc)
            {
                case 0:
                    return "Testeaza Memoria";
                case 1:
                    return "Popice cu litere";

            }
            return string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var testeazaMemorie = new TesteazaMemoria(3,0);
            this.Hide();
            testeazaMemorie.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PopiceCuLitere popiceCuLitere = new PopiceCuLitere();
            this.Hide();
            popiceCuLitere.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrimQR popiceCuLitere = new PrimQR();
            this.Hide();
            popiceCuLitere.ShowDialog();
            this.Close();
        }
    }
}
