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
        double dt = 0.01;
        int N=4,A=1;
    
        static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
         double Integral(int a, int b, double dif)
        {
            double total = 0;
            double yMax = 0;
            double x;
            double funct;

            int i = 0;
            do
            {
                x = GetRandomNumber(a, b);
                funct = Math.Abs(Math.Sin(dif));
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
            } while (i < N);

            return (b - a) * total / N;
        }
        void graph1(int N,double sigma)
        {
            double t,Oj,y=0,sum=0;
            Random rnd = new Random();
            int max = 5;
            double[] O = new double[N];
            double[] Oi = new double[N];
            double[] w=new double[N];
            // int t=1;
            //double[] y = new double[N];
            // this.chart1.Series[0].Points.Clear();

            for (int i = 0; i < N; i++)
            {
                Series mySeries = new Series("O" + i);
                mySeries.ChartType = SeriesChartType.Line;
                mySeries.BorderWidth = 2;
                chart1.Series.Add(mySeries);

            }
            for (int i=0; i < N; i++)
            {
                Oi[i] = rnd.NextDouble() * (2 * Math.PI);
                O[i] = Oi[i];
                // richTextBox1.Text +=  Convert.ToString(Oi[i]) + "-";
                y = O[i];
                this.chart1.Series[i].Points.AddXY(0, y);
            }
             //   textBox3.Text =Convert.ToString(O);
            for (int k = 0; k < N; k++)
            {
                w[k] = rnd.NextDouble()*(10.5-9.5) + 9.5;
              //  richTextBox1.Text +=  Convert.ToString(w[k]) + "-";
            }
            //  textBox4.Text = Convert.ToString(w);
            double dif;
            /*
             for (int i = 0; i < N; i++)
             {

                for (int j = 0; j < N; j++)
                {
                    if (i != j)
                    {
                        dif = Oi[j] - Oi[i];

                        sum += A * sigma * Math.Sin(Oi[j] - Oi[i]);
                    }

                }
                O[i] = w[i] + sum;
                sum = 0;

             }*/
            t = 0;
          
            t = dt;
            while (t < max)
            {
                for (int i = 0; i < N; i++)
                {

                    sum = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (j != i)
                        {
                            dif = (Oi[j] - Oi[i]);
                            //richTextBox1.Text += " " + Convert.ToString(Oi[j]) + " - j="+j;
                            //richTextBox1.Text += " "+Convert.ToString(Oi[i]) + " - i="+i;
                            //richTextBox1.Text += " dif= " + dif;
                            //  richTextBox1.Text +=" Oj= "+Oi[j]+" Oi= "+Oi[i]+ " dif- " + dif + " sin-"+ Math.Sin(dif) + " - ";
                            sum += A * sigma * Math.Sin(dif);
                          //  richTextBox1.Text += " sum= "+sum;
                        }


                    }
                     O[i] = (w[i] + sum);// или O[i] += (w[i] + sum); ?
                    // richTextBox1.Text += " [" + i + "] " + Convert.ToString(O[i]) + "-";
                  //  y = O[i]*dt*1; или что то в друхе
                        y+=O[i] * dt;

                
                    this.chart1.Series[i].Points.AddXY(t, y);
                }
                for (int i = 0; i < N; i++)
                {
                    Oi[i] = O[i];
                }
                //int i = 0;
                //y = O[i];
                //this.chart1.Series[i].Points.AddXY(t, y);

                t += dt;

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
           // textBox2.Text = Convert.ToString(trackBar2.Value);
        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
          //  this.chart1.ChartAreas[0].AxisX.Maximum = trackBar3.Value;
            //this.chart1.ChartAreas[0].AxisX2.Maximum = trackBar3.Value;
            graph1(N, sigma);
        }


        public Form1()
        {
            InitializeComponent();
           textBox1.Text = Convert.ToString(N);
            textBox2.Text = Convert.ToString(sigma);
            textBox3.Text = Convert.ToString(dt);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            N = Convert.ToInt32(textBox1.Text);
            sigma = Convert.ToDouble(textBox2.Text);
            dt=Convert.ToDouble(textBox3.Text);
            chart1.Series.Clear();
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
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            label3.Visible = true;
            lable4.Visible = true;
            lable5.Visible = true;
            label5.Visible = true;
            Type.Visible = true;
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("нет данных\r\n введите данные и нажмите 'Старт'", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (Convert.ToInt32(textBox1.Text)<1 || Convert.ToDouble(textBox2.Text) <= 0 || Convert.ToDouble(textBox3.Text) <= 0)
            {
                MessageBox.Show("Одно из значений некорректно\r\n введите корректное значение", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            Type.Text = "Все со всеми";
            
        }
    }

}
