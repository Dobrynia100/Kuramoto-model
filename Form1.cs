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
        bool pressed = false;
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

            if (type == 2 )
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
            if (type == 3)
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
            return A;
        }
       static double sums(ref double[] O, int i, int j, ref double[,] sigma)
        {
            double dif = 0;
            dif = (O[j] - O[i]);
            return sigma[i,j] * Math.Sin(dif);
        }
        double shecksigma(bool check,int nom)
        {
            if (check)
            {
                if(nom==1)
                {
                    sigma = Convert.ToDouble(textBox2.Text);
                }
                if (nom == 2)
                {
                    sigma = Convert.ToDouble(textBox6.Text);
                }
                if (nom == 3)
                { sigma= Convert.ToDouble(textBox2.Text); }
            }
            else sigma = Convert.ToDouble(textBox2.Text);

            return sigma;
        }
        void conmatrix(double[,]A,double[,]B,double[]C)
        {
            for (int i = 0; i < N; i++)
            {
                int bIndex = 0;
                C[i] = 0;
                for (int j = 0; j < N; j++)
                {
                    if (A[i,j] == 1)
                    {
                        B[i,bIndex] = j;
                        bIndex++;
                        C[i]++;
                    }
                }
            }
        }
        double kuramoto(int i, double[] O,double[] w,double[,] sigma,double[,]B,double[] C) // С оптимизацией 1
        {
            double sum = 0;

            for (int j = 0; j < C[i]; j++)
            {
                sum += sigma[i,(int)B[i,j]] * Math.Sin(O[(int)B[i,j]] - O[i]);
            }

            return w[i] + sum;
        }
        void Fillsigma(double[,]sigma,bool check)
        {
            
                for (int i = 0; i < N; i++)
                {
                for (int j = 0; j < N; j++)
                {
                    if (check)
                    {
                        if ((i >= 0 && i < N/2) && (j >= 0 || j < N/2))
                    {
                        sigma[i,j] = Convert.ToDouble(textBox2.Text);
                    }
                    if ((((i >= N/2 && i < N) && (j >= 0 && j < N/2)) || ((i >= 0 && i < N/2) && (j >= N/2 && j < N))))
                    {
                        sigma[i,j] = Convert.ToDouble(textBox2.Text);
                    }
                    if ((i >= N/2 && i < N) && (j >= N/2 && j < N))
                    {
                        sigma[i,j] = Convert.ToDouble(textBox6.Text);
                    }
                }
                    else sigma[i, j] = Convert.ToDouble(textBox2.Text);

                }
                }
                
            
        }
        double RungeKutta(double[] O,int i, double[] w, double[,] sigma, double[,] B, double[] C)
         {
             double k1, k2, k3, k4;
            
            double[] theta_k1 = new double[N];
            double[] theta_k2 = new double[N];
            double[] theta_k3 = new double[N];
            k1 = kuramoto(i, O, w, sigma, B, C);
             theta_k1[i] = O[i] + k1 / 2;
             k2 = kuramoto(i, theta_k1,w,sigma,B,C) * dt;
             theta_k2[i] = O[i] + k2 / 2;
             k3 = kuramoto(i, theta_k2, w, sigma, B, C) * dt;
             theta_k3[i] = O[i] + k3 / 2;
             k4 = kuramoto(i, theta_k3, w, sigma, B, C) * dt;       
            return O[i] + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
        }
        void graph1( int N, ref double[,]A)
        {
            double t=0,Oj,sum=0;
            int t1 = 0, i = 0, j = 0, nom = 1 ;
            double[] O = new double[N];
            double[] w = new double[N + 1];
            double[] y = new double[N];
            double[] sum1 = new double[N/2];
            double[] sum2 = new double[N/2];
            double[,] sigma= new double[N, N];
            bool check = false;
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
            
            if (downl == false)
            {
                O=getO(N);
               
                if (checkBox1.Checked)
                {
                    check = true;
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


            }
            else
            {
                string o,w1;
                while (i < N)
                {
                    if (checkBox1.Checked)
                    {
                        check = true;
                    }
                    o = richTextBox2.Text;
                    w1= richTextBox3.Text;
                    O[i] =Convert.ToDouble(o.Split(' ')[i]);
                    y[i] = O[i];
                    w[i]= Convert.ToDouble(w1.Split(' ')[i]);

                    this.chart1.Series[i].Points.AddXY(0, y[i]);
                    i++;
                }
                i = 0;
                downl = false;
                graph3(ref O,t);
                
            }
            
            
      
            
            t = dt;
            double[,] B = new double[N, N];
            double[] C = new double[N];
            Fillsigma(sigma,check);
            conmatrix(A,B,C);
            double dif = 0, dif2 = 0;
         
                while (t1 < (tmax/dt))
                {

                // sigma = shecksigma(check, 1);//функция выбора необходимой сигмы для выбранного режима 
                // sigma2 = shecksigma(check, 2);
                if (checkBox2.Checked)
                {
                    while (i < N / 2)//вычисление первой и четвертой четверти
                    {

                        while (j < N / 2)
                        {
                            if ((j != i) || A[i, j] != 0)
                            {

                                sum1[i] += A[i, j] * sums(ref O, i, j, ref sigma);
                                sum2[i] += A[i + N / 2, j + N / 2] * sums(ref O, i + N / 2, j + N / 2, ref sigma);

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
                    //  sigma = shecksigma(check, 3);
                    while (i < N)
                    {

                        while (j < N / 2)//вычисление второй и третьей четверти
                        {
                            if ((j != i) || A[i, j] != 0)
                            {

                                sum1[i - N / 2] += A[i, j] * sums(ref O, j, i, ref sigma);

                                sum2[i - N / 2] += A[j, i] * sums(ref O, i, j, ref sigma);

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
                        this.chart1.Series[i - N / 2].Points.AddXY(t1, O[i - N / 2]);//первая половина 

                        y[i] = O[i] + (w[i] + sum2[i - N / 2]) * dt;
                        dif2 = (w[i] + sum2[i - N / 2]) * dt;

                        O[i] = y[i];
                        this.chart1.Series[i].Points.AddXY(t1, O[i]);//вторая половина


                        j = 0;
                        i++;
                    }
                    i = 0;
                    j = 0;
                }
                if (checkBox3.Checked)
                { y[i]=RungeKutta(O, i, w, sigma, B, C);
                    this.chart1.Series[i].Points.AddXY(t1, O[i]);
                }
                graph3(ref O, t);//график ро

                    t += dt;
                    t1++;

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
            if (type == 2||type==3)
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
            
            if (checkBox1.Checked)
            {
                double rhoG = getrho(O, true);
                this.chart3.Series[1].Points.AddXY(Math.Round(t * tmax, 8), rhoG);
            }

        }
      
            static int num1 = 6,num2=10;
        TextBox[] textBoxes = new TextBox[num1];
        Label[] labels = new Label[num2];
        public Form1()
        {
            InitializeComponent();
            chart3.Series[0].Name = "\u03C1";
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
            
            labels[1].Text = "\u03C3=";
            labels[7].Text = "Нач. \u03B8";
            labels[8].Text = "Нач. \u03C9";
            labels[9].Text = "\u03C3 =";
            


        }
      
        private void Start_Click(object sender, EventArgs e)
        {
            if (pressed||(checkBox2.Checked||checkBox3.Checked))
            {
                N = Convert.ToInt32(textBox1.Text);
                sigma = Convert.ToDouble(textBox2.Text);
                dt = Convert.ToDouble(textBox3.Text);
                tmax = Convert.ToDouble(textBox4.Text);
                K = Convert.ToInt32(textBox5.Text);
                chart1.Series.Clear();
                if (downl == false)
                {
                    richTextBox2.Clear();
                    richTextBox3.Clear();
                }
                this.chart2.Enabled = true;
                double[,] A = new double[N, N];
                A = checkcon(K, A);
                graph2(ref A);
                for (int i = 0; i < this.chart3.Series.Count; i++)
                {

                    this.chart3.Series[i].Points.Clear();
                    this.chart3.Series[i].Points.AddY(0);
                }
                graph1(N, ref A);
                
            }
            else
            {
                MessageBox.Show("Выберите режим и метод интегрирования", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" )
            {
                MessageBox.Show("нет всех данных\r\n введите недостающие данные и нажмите 'Старт'", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if ((Convert.ToInt32(textBox1.Text) < 1) || (Convert.ToDouble(textBox2.Text) <= 0) || (Convert.ToDouble(textBox3.Text) <= 0) || (Convert.ToInt32(textBox5.Text)>N/2))
            {
                MessageBox.Show("Одно из значений некорректно\r\n введите корректное значение", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (checkBox1.Checked && type==2)
            {
                if ((Convert.ToInt32(textBox6.Text) <= (N / 2 + 1)) || (Convert.ToInt32(textBox6.Text) >= N))
                {
                    MessageBox.Show("\u03C3 G должна быть в интервале [N/2+1,N]", "\u03C3 G", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (!(Convert.ToInt32(textBox2.Text) <= (N / 2)) || !(Convert.ToInt32(textBox2.Text) >= 0))
                {
                    MessageBox.Show("\u03C3 N должна быть в интервале [0,N/2]", "\u03C3 N", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("нет всех данных\r\n введите недостающие данные и нажмите 'Старт'", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
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

            if ((textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "") || (checkBox1.Checked && textBox6.Text == "")||(checkBox1.Checked==false && type==2 && textBox5.Text == ""))
            {
                
                MessageBox.Show("нет всех данных\r\n заполните пустые ячейки", "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            

            SaveFileDialog sf;
            sf = new SaveFileDialog();
            sf.Filter = "Text files(*.txt)|*.txt| All files(*.*)|*.*";
            string filename;
            string[] inf = new string[num1+N];
           
            sf.ShowDialog();
            filename = sf.FileName;

            inf[0] = Convert.ToString(type); 
            inf[1] = textBoxes[0].Text;
           
            
           
            for (int i = 1; i < num1-2; i++)
            {          
                inf[i+1] = textBoxes[i].Text;
            }
            if (type == 2)
            {
                inf[num1 - 1] = textBox5.Text;
                
            }
            if (type==3)
            { inf[num1] = textBox6.Text; }

            inf[1+num1] = richTextBox2.Text + richTextBox3.Text;
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
                richTextBox2.Clear();
                richTextBox3.Clear();
                var fileStream = of.OpenFile();
                StreamReader reader = new StreamReader(fileStream);
                type = Convert.ToInt32(reader.ReadLine());
                N = Convert.ToInt32(reader.ReadLine());
                textBoxes[0].Text = Convert.ToString(N);
                if (type == 1)
                {
                    button1_Click(sender, e);
                    checkBox1.Checked = false;
                }
                if (type == 2)
                {
                    button2_Click(sender, e);
                    checkBox1.Checked = false;
                }
                if (type == 3)
                {
                    
                    button2_Click(sender, e);
                    checkBox1.Checked = true;
                }
                for (int i = 1; i < 2 * N + num1; i++)
                {
                    text = reader.ReadLine();

                    if (text == "")
                    { text = "0"; }
                    if (i < num1)
                    {
                        textBoxes[i].Text = text;
                    }
                    else if (i >= num1 && i < num1 + N)
                    { richTextBox2.Text += text; }
                    else
                        if (i >= num1+ N)
                    {
                        richTextBox3.Text += text;

                    }
                }
                downl = true;                            
                
                
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pressed = true;
            for (int i = 0; i < 6; i++)
            {
                textBoxes[i].Visible = false;

            }
            checkBox1.Visible = true;
            for (int i = 0; i <= 7; i++)
            {
                if (i < 5) textBoxes[i].Visible = true;
                labels[i].Visible = true;
            }
            chart1.ChartAreas[0].AxisY.Maximum = 50;
            textBoxes[1].Text = "5";
            labels[6].Text = "K=";
            type = 2;
            richTextBox2.Visible = true;
            richTextBox3.Visible = true;
            labels[1].Text = "\u03C3 =";
            labels[5].Text = "Кольцо";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {


            if (checkBox1.Checked)
            {
                type = 3;
                labels[1].Text = "\u03C3\u2099 =";
                Series mySeries = new Series("\u03C1 G");
                label11.Visible = true;
                mySeries.ChartType = SeriesChartType.Line;
                mySeries.BorderWidth = 2;           
                chart3.Series.Add(mySeries);
                chart1.ChartAreas[0].AxisY.Maximum = 50;
                textBoxes[5].Text = "7";
                textBoxes[4].Enabled = false;
                textBoxes[5].Visible = true;
                labels[9].Visible = true;
                labels[6].Enabled = false;
                labels[5].Text = "Два кольца";

            }
            else
            {
                type = 2;
                labels[1].Text = "\u03C3 =";
                chart3.Series.RemoveAt(1);
                
                label11.Visible = false;
                chart1.ChartAreas[0].AxisY.Maximum = 50;
                textBoxes[4].Enabled = true;
                textBoxes[5].Visible = false;
                labels[9].Visible = false;
                labels[6].Enabled = true;
                labels[5].Text = "Кольцо";
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
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "\u03C3 G", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    e.Cancel = true;
                }


            }
            catch (Exception ex) { }
           
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {


            try
            {
                double numericValue;
                bool isNumber = double.TryParse(textBox2.Text, out numericValue);

                if (isNumber == false)
                {
                    DialogResult dr = MessageBox.Show("Значение должно быть числом", "\u03C3", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox3.Enabled = false;
            }
            else checkBox3.Enabled = true;



        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Enabled = false;
            }
            else checkBox2.Enabled = true;
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
            pressed = true;
            checkBox1.Visible = false;
            checkBox1.Checked = false;
            for (int i = 0; i < 6; i++)
            {
               textBoxes[i].Visible = false;
                
            }
            for (int i = 0; i <= 8; i++)
            {
                if (i < 4) textBoxes[i].Visible = true;
                labels[i].Visible = true;
            }
            type = 1;
            chart1.ChartAreas[0].AxisY.Maximum = 50;
            richTextBox2.Visible = true;
            richTextBox3.Visible = true;
            textBoxes[4].Visible = false;
            textBoxes[1].Text = "0,07";
            labels[1].Text = "\u03C3 =";
            labels[6].Visible = false;
            labels[5].Text = "Все со всеми";
            
        }
    }

}
