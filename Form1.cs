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
        double sigma = 0.01;
       
        int N=3,A=1;
        static double Function(double x)
        {
            return Math.Sin(x);
        }

        static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        static double Integral(int a, int b, int n)
        {
            double total = 0;
            double yMax = 0;
            double x;
            double funct;

            int i = 0;
            do
            {
                x = GetRandomNumber(a, b);
                funct = Math.Abs(Function(x));
                if (yMax > funct)
                {
                    total += funct;
                    i++;
                }
                else
                {
                    yMax = funct * 2;
                    i = 0;
                }
            } while (i < n);

            return (b - a) * total / n;
        }
        void graph1(int N,double sigma)
        {
            double t,Oj,y,sum=0;
            Random rnd = new Random();
            double[] O = new double[N];
            double[] Oi = new double[N];
            double[] w=new double[N];
            //double[] y = new double[N];
            for (int i=0; i < N; i++)
            {
                Oi[i] = rnd.NextDouble() * (2 * Math.PI);
            }
             //   textBox3.Text =Convert.ToString(O);
            for (int k = 0; k < N; k++)
            {
                w[k] = rnd.NextDouble() + 9;
            }
          //  textBox4.Text = Convert.ToString(w);
           
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i != j)
                    {
                        sum += A * sigma * Math.Sin(Oi[j] - Oi[i]);
                    }
                   
                }
                O[i] = w[i] + sum;
                sum = 0;

            }
            t = 0;
            label6.Text = Convert.ToString(Oi[0]);
            label7.Text = Convert.ToString(Oi[1]);
            label8.Text = Convert.ToString(Oi[2]);
            this.chart1.Series[0].Points.Clear();
           
            for (int i = 0; i < N; i++)
            {
                Series mySeries = new Series("O"+i);
                mySeries.ChartType = SeriesChartType.Line;
                chart1.Series.Add(mySeries);
            }
            //todo почитать про методы интегрирования? треугольный.монте-карло.формула парабол (Симпсона)
            while (t <= 10)
            {
                  for (int i = 0; i < N; i++)
                 {
                //int i = 0;
                    y = O[i];
                    this.chart1.Series[i].Points.AddXY(t, y);
                }
                t += 1;

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
           // graph1();
            textBox1.Text = Convert.ToString(trackBar1.Value);
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            graph2();
            textBox2.Text = Convert.ToString(trackBar2.Value);
        }
        


        public Form1()
        {
            InitializeComponent();
          
        }

        private void Start_Click(object sender, EventArgs e)
        {
          //  N = Convert.ToInt32(textBox1.Text);
           // sigma = Convert.ToDouble(textBox2.Text);
           
            graph1(N,sigma);
            graph2();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("вы уверены ?", "Решение СЛАУ методом простых итераций", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Type.Text = "Все со всеми";
        }
    }

}
