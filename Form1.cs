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
        double tmax = 10;
        int mult=1500;
     
        void graph1(int N,double sigma)
        {
            double t=0,Oj,sum=0;
            Random rnd = new Random();
            //int max = 5;
            double[] O = new double[N];
            double[] Oi = new double[N];
            double[] w=new double[N];
            double[] y = new double[N];
        
            for (int i = 0; i < N; i++)
            {
                Series mySeries = new Series("O" + i);
                mySeries.ChartType = SeriesChartType.Line;
                mySeries.BorderWidth = 2;
                chart1.Series.Add(mySeries);

            }
            //  Oi[0] = 5.72580022079391;
            //Oi[1] = 0.710908815279564;
            //Oi[2] = 1.87267642803626;
            //Oi[3] = 1.48154033368296;

            for (int i=0; i < N; i++)
             {
                 Oi[i] = rnd.NextDouble() * (2 * Math.PI);           
                 // richTextBox1.Text +=  Convert.ToString(Oi[i]) + " - o";
                 y[i] = Oi[i];
                 this.chart1.Series[i].Points.AddXY(0, y[i]);
             }


            //   textBox3.Text =Convert.ToString(O);
            //w[0] = 9.91044653645272;
            //w[1] = 9.73098717687232;
            //w[2] = 10.3641021851749;
            //w[3] = 9.86498762730741;
             for (int k = 0; k < N; k++)
             {
                 w[k] = rnd.NextDouble()*(10.5-9.5) + 9.5;
                // richTextBox1.Text +=  Convert.ToString(w[k]) + " - w";
             }

            double dif=0;

            int test = 0;

            /*  t = dt;
              while (t < tmax)
              {
                  for (int i = 0; i < N; i++)
                  {

                      sum = 0;
                      for (int j = 0; j < N; j++)
                      {
                          if (j != i)
                          {
                              dif = (Oi[j] - Oi[i]) ;

                              sum += A * sigma * Math.Sin(dif)* mult ;                                                                          

                          }

                      }
                      if (test < 4 && t <= 0.03)
                      {
                          //  richTextBox1.Text += " Oj= " + Oi[j] + " Oi= " + Oi[i] + " dif- " + dif + " sin-" + Math.Sin(dif) + " - ";
                          richTextBox1.Text += i + " sum= " + sum;
                          test++;
                          richTextBox1.Text += '\n';
                      }
                      O[i] = (w[i] + sum);
                      // richTextBox1.Text += " [" + i + "] " + Convert.ToString(O[i]) + "-";

                          y[i]+=O[i]*dt;

                  //4 графика колебаний осцелятора? как отдельное уранение и потом под моделью сходятся? спросить препода об этом
                      this.chart1.Series[i].Points.AddXY(t, y[i]);
                  }
                 for (int i = 0; i < N; i++)
                  {
                      Oi[i]= O[i];
                  }

                  test = 0;
                  t += dt;//t+=1;

              }
            */
            int t1=0;
            //t = dt;
            //double xi, k1, k2, k3, k4, k5, k6;
            //while (t1 < tmax)
            //{
            //    for (int i = 0; i < N; i++)
            //    {
            //        // Oi[i] = 1 * Math.Sin(w[i] * t + 0.3);
            //        sum = 0;
            //        for (int j = 0; j < N; j++)
            //        {
            //            if (j != i)
            //            {
            //                dif = (Oi[j] - Oi[i]);

            //                sum += A * sigma * Math.Sin(dif) * mult;

            //            }

            //        }
            //         if (test < 4 && t <= 0.03)
            //          {
            //              richTextBox1.Text += " Oi= " + Oi[i] + " dif- " + dif + " sin-" + Math.Sin(dif) + " - ";
            //              richTextBox1.Text += i + " sum= " + sum;
            //              test++;
            //              richTextBox1.Text += '\n';
            //          }

            //        Oi[i] = (w[i] + sum);
            //       /* if (Oi[i]>=Math.PI)
            //        { Oi[i] = -Oi[i]-; }*/
            //        if (Oi[i] > 2 * Math.PI|| (Oi[i] < -(2 * Math.PI)))
            //        { Oi[i] = 0; }
            //        k1 = dt * Oi[i];
            //        k2 = dt * (Oi[i] + 0.5 * k1 + t + 0.5 * dt);
            //        k3 = dt * (Oi[i] + 0.5 * k2 + t + 0.5 * dt);
            //        k4 = dt * (Oi[i]+ k3+ t + dt );

            //        y[i] += 1.0 / 6.0 * (k1 + 2 * k2 + 2 * k3 + k4);
            //        this.chart1.Series[i].Points.AddXY(t1, y[i]);
            //    }
            //    //for (int i = 0; i < N; i++)
            //    //{
            //    //    Oi[i] = O[i];
            //    //}
            //    test = 0;
            //    t += dt;
            //    t1++;
            // }
            t = dt;
            while (t < tmax)
            {
                for (int i = 0; i < N; i++)
                {

                    sum = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (j != i)
                        {
                            dif = (Oi[j] - Oi[i]);

                            sum += A * sigma * Math.Sin(dif) * mult;

                        }

                    }
                 /*   if (test < 4 && t <= 0.03)
                    {
                        //  richTextBox1.Text += " Oj= " + Oi[j] + " Oi= " + Oi[i] + " dif- " + dif + " sin-" + Math.Sin(dif) + " - ";
                        richTextBox1.Text += i + " sum= " + sum;
                        test++;
                        richTextBox1.Text += '\n';
                    }*/
                    O[i] = Oi[i] + (w[i] + sum)*dt;
                   
                    this.chart1.Series[i].Points.AddXY(t1, O[i]);
                }
                for (int i = 0; i < N; i++)
                {
                    Oi[i] = O[i];
                }
                
                test = 0;
                t += dt;//t+=1;
                t1++;

            }
        }
        void graph2()
        {
            double x, y=0;
            x = 0;
            this.chart2.Series[0].Points.Clear();
            while (x <= 40)
            {
                y += Math.Cos(x);
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
     
            graph1(N, sigma);
        }


        public Form1()
        {
            InitializeComponent();
           textBox1.Text = Convert.ToString(N);
            textBox2.Text = Convert.ToString(sigma);
            textBox3.Text = Convert.ToString(dt);
            textBox4.Text = Convert.ToString(tmax);
            textBox5.Text = Convert.ToString(mult);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            N = Convert.ToInt32(textBox1.Text);
            sigma = Convert.ToDouble(textBox2.Text);
            dt=Convert.ToDouble(textBox3.Text);
            tmax = Convert.ToDouble(textBox4.Text);
            mult= Convert.ToInt32(textBox5.Text);
            chart1.Series.Clear();
            richTextBox1.Clear();
            graph1(N,sigma);
            graph2();
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("нет данных\r\n введите данные и нажмите 'Старт'", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (Convert.ToInt32(textBox1.Text) < 1 || Convert.ToDouble(textBox2.Text) <= 0 || Convert.ToDouble(textBox3.Text) <= 0)
            {
                MessageBox.Show("Одно из значений некорректно\r\n введите корректное значение", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("вы уверены ?", "Метод курамото", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            textBox4.Visible = true;
            label3.Visible = true;
            lable4.Visible = true;
            label4.Visible = true;
            lable5.Visible = true;
            label5.Visible = true;
            Type.Visible = true;
            
            Type.Text = "Все со всеми";
            
        }
    }

}
