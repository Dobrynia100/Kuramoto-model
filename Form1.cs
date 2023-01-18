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
        double sigma = 5, sigma2 = 9;
        double dt = 0.01;
        int N = 10;
        double tmax = 2000;
        int K = 1;
        bool downl = false;
        Random rnd = new Random();
        int type = 1;
        double[] getO(int N)
        {
            int i = 0;
            double[] Oi = new double[N + 1];
            while (i < N)
            {
                Oi[i] = rnd.NextDouble() * (2 * Math.PI);
                richTextBox2.Text += Convert.ToString(Oi[i]) + " \n";
                i++;
            }

            return Oi;
        }
        double getw(int N, double max, double min)
        {

            double w;


            w = rnd.NextDouble() * (max - min) + min;
            richTextBox3.Text += Convert.ToString(w) + " \n";


            return w;
        }
        double getrho(double[] O, bool G)
        {
            double sum_cos = 0;
            double sum_sin = 0;

            double dt = Convert.ToDouble(textBoxes[2].Text);
            //int iterations = (int)((tmax - 0) / dt);
            //double[] rho =new double[];
            double rho = 0;
            int j = 0;

            while (j < N / 2)
            {
                if (G)
                {
                    sum_cos += Math.Cos(O[j]);
                    sum_sin += Math.Sin(O[j]);
                }
                else
                {
                    sum_cos += Math.Cos(O[N / 2 + j]);
                    sum_sin += Math.Sin(O[N / 2 + j]);
                }
                j++;
            }
            sum_cos /= (N / 2);
            sum_sin /= (N / 2);
            rho = Math.Sqrt(Math.Pow(sum_cos, 2) + Math.Pow(sum_sin, 2));
            return rho;
        }
        void clearA(double[,] A)
        {
            int i = 0, j = 0;
            while (i < N)
            {
                while (j < N)
                {
                    A[i, j] = 0;
                    j++;
                }
                j = 0;
                i++;
            }
        }
        double[,] checkcon(int K, double[,] A)
        {

            int i = 0, j = 0;
            clearA(A);
            if (type == 1)
            {
                while (i < N)
                {
                    while (j < N)
                    {
                        if (j != i)
                        {
                            A[i, j] = 1;

                        }
                        j++;

                    }
                    j = 0;
                    i++;
                }
            }

            if (type == 2)
            {
                if (checkBox1.Checked)
                {
                    while (i < N / 2)
                    {
                        A[i, i + N / 2] = 1;
                        A[i + N / 2, i] = 1;
                        while (j < N / 2)
                        {
                            if (j != i)
                            {
                                A[i, j] = 1;
                                A[i + N / 2, j + N / 2] = 1;
                            }
                            j++;
                        }
                        j = 0;
                        i++;
                    }
                    i = 0;



                }
                else
                {



                    while (i < N)
                    {

                        while (j < N)
                        {

                            if (j == i + K || j == i - K)
                            {
                                A[i, j] = 1;

                            }
                            if (i - K < 0)
                            {
                                int x = i - K;
                                A[i, N + x] = 1;
                                A[N + x, i] = 1;
                            }
                            j++;

                        }

                        j = 0;
                        i++;
                    }

                    i = 0;
                    j = 0;

                }
            }

            return A;
        }
       static double sums(ref double[] O, int i, int j, ref double sigma)
        {
            double dif = 0;
            dif = (O[j] - O[i]);
            return sigma * Math.Sin(dif);
        }
        
        void graph1( int N,  double sigma,ref double[,]A)
        {
            double t=0,Oj,sum=0;
            int t1 = 0, i = 0, j = 0;
            double[] O = new double[N];
            double[] w = new double[N + 1];
            double[] y = new double[N];
            double[] sum1 = new double[N/2];
            double[] sum2 = new double[N/2];
            while (i < N / 2)
            { sum1[i] = 0;
                sum2[i] = 0;
                i++;
            }
            i = 0;
            while (i < N)
            {
                Series mySeries = new Series("O" + i);
                mySeries.ChartType = SeriesChartType.Line;
                mySeries.BorderWidth = 2;
                chart1.Series.Add(mySeries);
                i++;
            }
            i = 0;
            // Oi[0] = 5.72580022079391;
            //Oi[1] = 5.710908815279564;
            //Oi[2] = 5.87267642803626;
            //Oi[3] = 3.48154033368296;
            if (downl == false)
            {
                O=getO(N);
                //O[0] = 1.19746912786187;
                //O[1] = 5.37222573018109;
                //O[2] = 4.15683965357519;
                //O[3] = 3.393337227559;
                //O[4] = 2.76237955956287;
                //O[5] = 3.40766243446232;
                //O[6] = 1.9237057492475;
                //O[7] = 2.66084160479506;
                //O[8] = 2.47588373672276;
                //O[9] = 1.8025766354716;

                if (checkBox1.Checked)
                {
                    while (i < N)
                    {
                        if (i < N / 2)
                        {
                            w[i] = getw(N / 2, 1.5, 0.5d);

                        }
                        else w[i] = getw(N, 10.5d, 9.5d);
                        i++;
                    }
                       
                }
                else
                {
                    while (i < N)
                    {
                        w[i] = getw(N, 10.5d, 9.5d);
                        i++;
                    }
                    
                }
                i = 0;
                while (i < N)
                {
                    y[i] = O[i];
                    this.chart1.Series[i].Points.AddXY(0d, y[i]);
                    i++;
                }
                i = 0;

                //   textBox3.Text =Convert.ToString(O);
                //w[0] = 1.34370677305558;
                //w[1] = 1.21084567937574;
                //w[2] = 1.09845071127566;
                //w[3] = 0.653441705346779;
                //w[4] = 0.864471862262335;
                //w[5] = 9.53867130169583;
                //w[6] = 10.1679832230639;
                //w[7] = 10.0354543074665;
                //w[8] = 10.1036970664764;
                //w[9] = 10.1581068163077;


            }
            else
            {
                string o,w1;
                while (i < N)
                {
                    o = richTextBox2.Text;
                    w1= richTextBox3.Text;
                    O[i] =Convert.ToDouble(o.Split(' ')[i]);
                    y[i] = O[i];
                    w[i]= Convert.ToDouble(w1.Split(' ')[i]);
                    this.chart1.Series[i].Points.AddXY(0, y[i]);
                    i++;
                }
                i = 0;
                graph3(ref O,t);
            }
            
            
      
            
            t = dt;
            
            double dif = 0, dif2 = 0;
            if (checkBox1.Checked)
            {
                while (t1 < tmax)
                {

                    sigma = Convert.ToDouble(textBox2.Text);
                    sigma2 = Convert.ToDouble(textBox6.Text);
                    while (i < N / 2)
                    { 
                        
                        while (j < N / 2)
                        {
                            if ((j != i) || A[i, j] != 0)
                            {

                                sum1[i] += A[i, j] * sums(ref O, i, j, ref sigma);
                                sum2[i] += A[i + N / 2, j + N / 2] * sums(ref O, i + N / 2, j + N / 2, ref sigma2);

                            }
                            else
                            {
                                sum1[i] += 0;
                                sum2[i] += 0;
                            }
                            j++;
                        }
                        j = 0;
                        i++;
                    }
                    j = 0;
                    sigma = Convert.ToDouble(textBox7.Text);
                    while (i < N)
                    {

                        while (j < N / 2)
                        {
                            if ((j != i) || A[i, j] != 0)
                            {
                                //  dif = (O[i] - O[j]);
                                sum1[i - N / 2] += A[i, j] * sums(ref O, j, i, ref sigma);
                                // dif2 = (O[j] - O[i]);

                                sum2[i - N / 2] += A[j, i] * sums(ref O, i, j, ref sigma2);

                            }
                            else
                            {
                                sum1[i - N / 2] += 0;
                                sum2[i - N / 2] += 0;
                            }


                            j++;
                        }
                        y[i - N / 2] = O[i - N / 2] + (w[i - N / 2] + sum1[i - N / 2]) * dt;
                        O[i - N / 2] = y[i - N / 2];
                        this.chart1.Series[i - N / 2].Points.AddXY(t * tmax, O[i - N / 2]);

                        y[i] = O[i] + (w[i] + sum2[i - N / 2]) * dt;
                        O[i] = y[i];
                        this.chart1.Series[i].Points.AddXY(t * tmax, O[i]);


                        j = 0;
                        i++;
                    }
                    i = 0;
                    j = 0;

                
                graph3(ref O, t);

                    t += dt;
                    t1++;

                }
            }
            else
            {
                
                sigma = Convert.ToDouble(textBox2.Text);
                while (t1 < tmax)
                {
                    while (i < N)
                    {
                        sum = 0;
                        while (j < N)
                        {
                            if ((j != i) || A[i, j] != 0)
                            {
                                dif = (O[j] - O[i]);
                                sum += A[i, j] * sigma * Math.Sin(dif);

                            }
                            j++;
                        }
                        j = 0;

                        O[i] = O[i] + (w[i] + sum) * dt;
                        this.chart1.Series[i].Points.AddXY(t * tmax, O[i]);
                        i++;
                    }
                    i = 0;
                    graph3(ref O, t);
                    t += dt;
                    t1++;

                }
            }
            }
        void graph2(ref double[,]A)
        {
            this.chart2.Visible = true;
            double x, y=0;
            x = 0;
           
                this.chart2.Series[0].Points.Clear();
                this.chart2.Series[1].Points.Clear();
           

            this.chart2.Series[1].Name=Convert.ToString(sigma);
            
            this.chart2.ChartAreas[0].AxisY.Maximum = N;
            this.chart2.ChartAreas[0].AxisX.Maximum = N;
            if (type == 1)
            {
                for (int i = 0; i <= N; i++)
                {
                    this.chart2.Series[1].Points.AddXY(i, i);

                }
            }
            if (type == 2)
            {
               int i = 1,j = 1;
              
                    do
                    {
                        do
                        {

                            if (A[i - 1, j - 1] == 1)
                            {
                                this.chart2.Series[1].Points.AddXY(i, j);

                            }
                            j++;
                        } while (j <= N);
                        j = 1;
                        i++;
                    } while (i <= N);
                    i = 1;
                 

            }

        }
        void graph3(ref double[] O,double t)
        {
            double rho = getrho(O,false);
              this.chart3.Series[0].Points.AddXY(Math.Round(t*tmax,8), rho);
            
            //  this.chart3.Series[0].Points.AddY(rho);
            if (checkBox1.Checked)
            {
                double rhoG = getrho(O, true);
                this.chart3.Series[1].Points.AddXY(Math.Round(t * tmax, 8), rhoG);
            }

        }
      
            static int num1 = 8,num2=11;
        TextBox[] textBoxes = new TextBox[num1];
        Label[] labels = new Label[num2];
        public Form1()
        {
            InitializeComponent();
           
            textBox1.Text = Convert.ToString(N);
            textBox2.Text = Convert.ToString(sigma);
            textBox3.Text = Convert.ToString(dt);
            textBox4.Text = Convert.ToString(tmax);
            textBox5.Text = Convert.ToString(K);
            textBoxes[0] = textBox1;
            textBoxes[1] = textBox2;
            textBoxes[2] = textBox3;
            textBoxes[3] = textBox4;
            textBoxes[4] = textBox5;
            textBoxes[5] = textBox6;
            textBoxes[6] = textBox7;
            textBoxes[7] = textBox8;


            labels[0] = label3;
            labels[1] = label1;
            labels[2] = label4;
            labels[3] = label2;
            labels[4] = label5;
            labels[5] = label9;
            labels[6] = label6;
            labels[7] = label7;
            labels[8] = label8;
            labels[9] = label10;
            labels[10] = label11;


        }
      
        private void Start_Click(object sender, EventArgs e)
        {
            N = Convert.ToInt32(textBox1.Text);
            sigma = Convert.ToDouble(textBox2.Text);
            dt=Convert.ToDouble(textBox3.Text);
            tmax = Convert.ToDouble(textBox4.Text);
            K= Convert.ToInt32(textBox5.Text);
            chart1.Series.Clear();
            this.chart2.Enabled = true;
            double[,] A = new double[N, N];
            A = checkcon(K, A);
            graph2(ref A);
            for (int i = 0; i < this.chart3.Series.Count; i++)
            {

                this.chart3.Series[i].Points.Clear();
                this.chart3.Series[i].Points.AddY(0);
            }
            graph1(N, sigma,ref A);             
               
                     
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("нет данных\r\n введите данные и нажмите 'Старт'", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if ((Convert.ToInt32(textBox1.Text) < 1) || (Convert.ToDouble(textBox2.Text) <= 0) || (Convert.ToDouble(textBox3.Text) <= 0) || (Convert.ToInt32(textBox5.Text)>N/2))
            {
                MessageBox.Show("Одно из значений некорректно\r\n введите корректное значение", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (checkBox1.Checked)
            {
                if ((Convert.ToInt32(textBox6.Text) <= (N / 2 + 1)) || (Convert.ToInt32(textBox6.Text) >= N))
                {
                    MessageBox.Show("СигмаG должна быть в интервале [N/2+1,N]", "СигмаG", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (!(Convert.ToInt32(textBox2.Text) <= (N / 2)) || !(Convert.ToInt32(textBox2.Text) >= 0))
                {
                    MessageBox.Show("СигмаN должна быть в интервале [0,N/2]", "СигмаN", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }

        }
      
        private void Enter(object sender, KeyEventArgs e)
        {
            Start_Click(sender, e);
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
            for (int i = 0; i < 8; i++)
            {
                textBoxes[i].Visible = false;

            }
            checkBox1.Visible = true;
            for (int i = 0; i <= 8; i++)
            {
                if (i < 5) textBoxes[i].Visible = true;
                labels[i].Visible = true;
            }
            labels[6].Text = "K=";
            type = 2;
            richTextBox2.Visible = true;
            richTextBox3.Visible = true;
            labels[1].Text = "Сигма=";
            labels[5].Text = "Кольцо";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            
            
            if (checkBox1.Checked)
            {
                labels[1].Text = "СигмаN=";
                Series mySeries = new Series("RhoG");
                mySeries.ChartType = SeriesChartType.Line;
                mySeries.BorderWidth = 2;
                chart3.Series.Add(mySeries);
                label10.Visible = true;
                label11.Visible = true;
                textBoxes[4].Visible = false;
                textBoxes[5].Visible = true;
                textBoxes[6].Visible = true;
                labels[6].Visible = false;

            }
            else
            {
                labels[1].Text = "Сигма=";
                chart3.Series.RemoveAt(1);
                label10.Visible = false;
                label11.Visible = false;
                textBoxes[4].Visible = true;
                textBoxes[5].Visible = false;
                textBoxes[6].Visible = false;
                labels[6].Visible = true;
            }


        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int numericValue;
                bool isNumber = int.TryParse(textBox2.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "СигмаG", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
           
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {


            try
            {
                int numericValue;
                bool isNumber = int.TryParse(textBox2.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "Сигма", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
            
            
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int numericValue;
                bool isNumber = int.TryParse(textBox1.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "N", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int numericValue;
                bool isNumber = int.TryParse(textBox4.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "tmax", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double numericValue;
                bool isNumber = double.TryParse(textBox3.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "dt", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int numericValue;
                bool isNumber = int.TryParse(textBox5.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "K", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkBox1.Visible = false;
            for (int i = 0; i < 8; i++)
            {
               textBoxes[i].Visible = false;
                
            }
            for (int i = 0; i <= 8; i++)
            {
                if (i < 4) textBoxes[i].Visible = true;
                labels[i].Visible = true;
            }
            type = 1;
            richTextBox2.Visible = true;
            richTextBox3.Visible = true;
            textBoxes[4].Visible = false;
            labels[1].Text = "Сигма=";
            labels[6].Visible = false;
            labels[5].Text = "Все со всеми";
            
        }
    }

}
