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

namespace NIR
{
    public partial class Form1 : Form
    {
        void graph1()
        {
            double x, y;
            x = trackBar1.Value;
            this.chart1.Series[0].Points.Clear();
            while (x <= 40)
            {
                y = Math.Sin(x);
                this.chart1.Series[0].Points.AddXY(x, y);
                x += 1;

            }
        }
        void graph2()
        {
            double x, y;
            x = trackBar2.Value;
            this.chart2.Series[0].Points.Clear();
            while (x <= 40)
            {
                y = Math.Cos(x);
                this.chart2.Series[0].Points.AddXY(x, y);
                x += 1;

            }
        }
       
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            graph1();
            label1.Text = "Sin(x)=" + trackBar1.Value;
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            graph2();
            label2.Text = "Cos(x)="+trackBar2.Value;
        }
        


        public Form1()
        {
            InitializeComponent();
           /* Chart chart = chart1;
            //кладем его на форму и растягиваем на все окно.
           // chart.Parent = this;
            
            //добавляем в Chart область для рисования графиков, их может быть
            //много, поэтому даем ей имя.
            chart.ChartAreas.Add(new ChartArea("Math functions"));
            //Создаем и настраиваем набор точек для рисования графика, в том
            //не забыв указать имя области на которой хотим отобразить этот
            //набор точек.
            
            Series mySeriesOfPoint = new Series("Sinus");
            mySeriesOfPoint.ChartType = SeriesChartType.Line;
            mySeriesOfPoint.ChartArea = "Math functions";
            for (double x = -Math.PI; x <= Math.PI; x += Math.PI / 10.0)
            {
                mySeriesOfPoint.Points.AddXY(x, Math.Sin(x));
            }
            //Добавляем созданный набор точек в Chart
            chart.Series.Add(mySeriesOfPoint);*/
        }

        private void Start_Click(object sender, EventArgs e)
        {
            graph1();
            graph2();
        }
    }
   
}
