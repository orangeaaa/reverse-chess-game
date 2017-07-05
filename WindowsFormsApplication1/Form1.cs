using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           

        }
        private int player = 0;
        private Button[,] btn = new Button[15, 15];

        private void Form1_Load(object sender, EventArgs e)
        {
            
            BackColor = Color.White;
            Text = "WZQ";
            Size size = new Size(1000, 1000);
            this.Size = size;
            
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Location = new Point(50 + i * 60, 50 + j * 60);
                    btn[i, j].Size = new Size(60, 60);
                    btn[i, j].BackColor = Color.Yellow;
                    btn[i, j].Name = (i * 100 + j).ToString();
                    this.Controls.Add(btn[i, j]);
                    btn[i, j].Click += btn_Click;
                }
            }

           


        }

        void btn_Click(object sender,EventArgs e)
        {
            Button but = (Button)sender;
            int a = Convert.ToInt32(but.Name);

            int x = a / 100;
            int y = a % 100;

            MessageBox.Show("x="+x+"\ny="+y);

            player++;
            if (player % 2 == 0)
            {
                (sender as Button).BackColor = Color.White;
                (sender as Button).Enabled = false;
                rule(x, y,0);
            }
            else
            {
                (sender as Button).BackColor = Color.Black;
                (sender as Button).Enabled = false;
                rule(x, y, 1);
            }
        }
        private void rule(int x,int y,int z)
        {
            Color color = Color.White;
            if (z == 1)
            {
                color = Color.Black;
            }

            for (int i = x + 1; i < 15; i++)//look up the same pieces rightward
            {
                if (btn[i, y].BackColor == color)
                {
                    for (int j = x; j >= x && j <= i; j++)
                    {
                        btn[j, y].BackColor = color;
                        btn[j, y].Enabled = false;
                    }
                    break;
                }
            }


            for (int i = x - 1; i >= 0; i--)//look up the same pieces leftward
            {
                if (btn[i, y].BackColor == color)
                {
                    for (int j = x; j <= x && j >= i; j--)
                    {
                        btn[j, y].BackColor = color;
                        btn[j, y].Enabled = false;
                    }
                    break;
                }
            }

            for (int i = y + 1; i < 15; i++)//look up the same pieces downward
            {
                if (btn[x, i].BackColor == color)
                {
                    for (int j = y; j >= y && j <= i; j++)
                    {
                        btn[x, j].BackColor = color;
                        btn[j, y].Enabled = false;
                    }
                    break;
                }
            }

            for (int i = y - 1; i >= 0; i--)//look up the same pieces upward
            {
                if (btn[x, i].BackColor == color)
                {
                    for (int j = y; j >= i && j <= y; j--)
                    {
                        btn[x, j].BackColor = color;
                        btn[j, y].Enabled = false;
                    }
                    break;
                }
            }
        }
        
    }
}
