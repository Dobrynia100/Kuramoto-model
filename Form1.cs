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
using System.IO;
using System.Globalization;
namespace NIR
{
    public partial class Form1 : Form
    {
        double sigma = 0.01;
        double dt = 0.01;
        int N=4;
        double tmax = 2000;
        int mult=1;
        bool downl = false;
        Random rnd = new Random();
        int type = 1;
        double[] getO(int N)
        {
            
            double[] Oi = new double[N + 1];
             for (int i = 0; i < N; i++)
                {
                    Oi[i] = rnd.NextDouble() * (2 * Math.PI);
                richTextBox2.Text += Convert.ToString(Oi[i])+" \n";
            }
            
            return Oi;
        }
        double[] getw(int N)
        {
          
            double[] w = new double[N + 1];
            
                for (int i = 0; i < N; i++)
                {
                    w[i] = rnd.NextDouble() * (10.5 - 9.5) + 9.5;
                richTextBox3.Text += Convert.ToString(w[i])+" \n";
            }
            
            return w;
        }
        void graph1(int N,double sigma)
        {
            double t=0,Oj,sum=0;
            
            //int max = 5;
            double[] O = new double[N];
           // double[] Oi = new double[N + 1];
            double[] w = new double[N + 1];
            double[] y = new double[N];
            double[,] A = new double[N,N];
            for (int i = 0; i < N; i++)
            {
                Series mySeries = new Series("O" + i);
                mySeries.ChartType = SeriesChartType.Line;
                mySeries.BorderWidth = 2;
                chart1.Series.Add(mySeries);

            }
            // Oi[0] = 5.72580022079391;
            //Oi[1] = 5.710908815279564;
            //Oi[2] = 5.87267642803626;
            //Oi[3] = 3.48154033368296;
            if (downl == false)
            {
                O=getO(N);
                w = getw(N);
                for (int i = 0; i < N; i++)
                {
                   
                    // richTextBox1.Text +=  Convert.ToString(Oi[i]) + " - o";
                    y[i] = O[i];
                    this.chart1.Series[i].Points.AddXY(0, y[i]);
                }

                //   textBox3.Text =Convert.ToString(O);
                //w[0] = 9.91044653645272;
                //w[1] = 9.73098717687232;
                //w[2] = 10.3641021851749;
                //w[3] = 9.86498762730741;
               
            }
            else
            {
                string o,w1;
                for (int i = 0; i < N; i++)
                {
                    o = richTextBox2.Text;
                    w1= richTextBox3.Text;
                    O[i] =Convert.ToDouble(o.Split(' ')[i]);
                    y[i] = O[i];
                    w[i]= Convert.ToDouble(w1.Split(' ')[i]);
                    this.chart1.Series[i].Points.AddXY(0, y[i]);
                }
            }
            if (type == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (j != i)
                        {
                            A[i, j] = 1;

                        }
                        else A[i, j] = 0;

                    }
                }
            }
            if(type==2)
            {
               
                
                    for (int i = 1; i < N; i++)
                {
                   
                    for (int j = 1; j < N; j++)
                    {
                        if (j==i+1 ||j==i-1)
                        {
                            A[i, j] = 1;

                        }
                        else A[i, j] = 0;

                    }
                }
                A[1, N - 1] = 1;
                A[N - 1, 1] = 1;
              
                for (int i = 0; i < N; i++)
                {
                    A[0, i] = 1;
                    A[i, 0] = 1;
                }
                A[0, 0] = 0;
            }
            
      
            int t1 = 0;
            t = dt;
            double dif = 0;
            //for (int i = 0; i < N; i++)
            //{
            //    O[i] = Oi[i];
            //}

            while (t1 < tmax)
            {
                for (int i = 0; i < N; i++)
                {

                    sum = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (j != i)
                        {
                            dif = (O[j] - O[i]);

                            sum += A[i,j] * sigma * Math.Sin(dif) ;

                        }

                    }
                 /*   if (test < 4 && t <= 0.03)
                    {
                        //  richTextBox1.Text += " Oj= " + Oi[j] + " Oi= " + Oi[i] + " dif- " + dif + " sin-" + Math.Sin(dif) + " - ";
                        richTextBox1.Text += i + " sum= " + sum;
                        test++;
                        richTextBox1.Text += '\n';
                    }*/
                    y[i] = O[i] + (w[i] + sum)*dt;
                   
                    this.chart1.Series[i].Points.AddXY(t*tmax, O[i]);
                }
                for (int i = 0; i < N; i++)
                {
                    O[i] = y[i];
                 }

           
                t += dt;//t+=1;
                t1++;

            }
         
            dataGridView1.RowCount = N;
            dataGridView1.ColumnCount = N;
            for (int i = 0; i < N; i++)
            {
                dataGridView1.Columns[i].Width = 40;
                dataGridView1.Rows[i].Height = 15;
            }
          
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                   
                    dataGridView1.Rows[i].Cells[j].Value = A[i,j]; 
                    if (A[i, j] == 0)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Black;
                    }
                    


                }
            }
            
        }
        void graph2()
        {
            this.chart2.Visible = true;
            double x, y=0;
            x = 0;
            for (int i = 0; i < 3; i++)
            {
                this.chart2.Series[i].Points.Clear();
            }
            this.chart2.Series[1].Points.Clear();
            this.chart2.Series[2].Points.Clear();
            this.chart2.Series[0].Points.Clear();
            this.chart2.Series[1].Name=Convert.ToString(sigma);
            
            this.chart2.ChartAreas[0].AxisY.Maximum = N;
            this.chart2.ChartAreas[0].AxisX.Maximum = N;
            for (int i = 0; i <= N; i++)
            {
                this.chart2.Series[0].Points.AddXY(i, 1);
                this.chart2.Series[1].Points.AddXY(i,1);
               
                this.chart2.Series[2].Points.AddXY(i, N-i-1);
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
        static int num1 = 5,num2=9;
        TextBox[] textBoxes = new TextBox[num1];
        Label[] labels = new Label[num2];
        public Form1()
        {
            InitializeComponent();
           
            textBox1.Text = Convert.ToString(N);
            textBox2.Text = Convert.ToString(sigma);
            textBox3.Text = Convert.ToString(dt);
            textBox4.Text = Convert.ToString(tmax);
            textBox5.Text = Convert.ToString(mult);
            textBoxes[0] = textBox1;
            textBoxes[1] = textBox2;
            textBoxes[2] = textBox3;
            textBoxes[3] = textBox4;
            textBoxes[4] = textBox5;


            labels[0] = label3;
            labels[1] = label1;
            labels[2] = label4;
            labels[3] = label2;
            labels[4] = label5;
            labels[5] = label9;
            labels[6] = label6;
            labels[7] = label7;
            labels[8] = label8;

        }

        private void Start_Click(object sender, EventArgs e)
        {
            N = Convert.ToInt32(textBox1.Text);
            sigma = Convert.ToDouble(textBox2.Text);
            dt=Convert.ToDouble(textBox3.Text);
            tmax = Convert.ToDouble(textBox4.Text);
            mult= Convert.ToInt32(textBox5.Text);
            chart1.Series.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
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
        private void save(object sender, EventArgs e)
        {

            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("нет всех данных\r\n заполните пустые ячейки", "Метод курамото", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            SaveFileDialog sf;
            sf = new SaveFileDialog();
            sf.Filter = "Text files(*.txt)|*.txt| All files(*.*)|*.*";
            string filename;
            string[] inf = new string[num1+N];
           
            sf.ShowDialog();
            filename = sf.FileName;
            inf[0] = textBoxes[0].Text;
           
            
                inf[1] = richTextBox2.Text+ richTextBox3.Text;
            
           
            for (int i = 1; i < num1; i++)
            {
                inf[i+1] = textBoxes[i].Text;
            }
                try
            {
                 File.WriteAllLines(filename, inf);
            }
            catch (Exception ex)
            {
                MessageBox.Show("wrong name/path", "Метод курамото", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.chart2.SaveImage(@"E:\\учеба\НИР\Kuramoto-model\matrix.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);


        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of;
            of = new OpenFileDialog();
            of.InitialDirectory = "c:\\";
            of.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string text,tex1,tex2;
            if (of.ShowDialog() == DialogResult.OK)
            {
                var fileStream = of.OpenFile();
                StreamReader reader = new StreamReader(fileStream);
                N = Convert.ToInt32(reader.ReadLine()); 
                textBoxes[0].Text =Convert.ToString(N);
                for (int i = 1; i < 2*N+num1; i++)
                {
                    text = reader.ReadLine();
                 
                    
                    if (i<= N)
                    {                     
                        richTextBox2.Text += text;                    
                    }
                    else if(i>N && i<=2*N)
                    { richTextBox3.Text += text; }else
                        if (i > 2*N+1)
                    {
                        //richTextBox1.Text += text;
                        textBoxes[i-2*N-1].Text = text;
                    }
                    downl = true;
                    
                }
                
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 8; i++)
            {
                if (i < 5) textBoxes[i].Visible = true;
                labels[i].Visible = true;
            }
            type = 2;
            richTextBox2.Visible = true;
            richTextBox3.Visible = true;

            labels[5].Text = "Звезда";
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i <= 8; i++)
            {
                if (i < 5) textBoxes[i].Visible = true;
                labels[i].Visible = true;
            }
            type = 1;
            richTextBox2.Visible = true;
            richTextBox3.Visible = true;
          
            labels[5].Text = "Все со всеми";
            
        }
    }

}
