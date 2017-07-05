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
        private const int N = 8;
        private int player = 0;
        private int[,] status = new int[N, N];
        private Button[,] btn = new Button[N, N];

        private int square_size = 60;
        private int margin = 50;

        private void Form1_Load(object sender, EventArgs e)
        {
            
            BackColor = Color.White;
            Text = "Reversi";
            Size size = new Size(margin * 2 + square_size * N, margin * 2 + square_size * N);
            this.Size = size;

            status_initialize();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Location = new Point(margin + i * square_size, margin + j * square_size);
                    btn[i, j].Size = new Size(square_size, square_size);
                    update_btn(i, j);
                    btn[i, j].Name = (i * 100 + j).ToString();
                    this.Controls.Add(btn[i, j]);
                    btn[i, j].Click += btn_Click;
                }
            }

            find_valid_place(player);

        }

        void status_initialize() {
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    status[i, j] = -1;
                }
            }
            status[3, 4] = status[4, 3] = 0;
            status[3, 3] = status[4, 4] = 1;
        }
        void update_btn(int x, int y, bool can_click = false)
        {
            switch (status[x, y])
            {
                case -1:
                    if (can_click)
                        btn[x, y].BackColor = Color.Orange;
                    else
                        btn[x, y].BackColor = Color.Yellow;
                    btn[x, y].Enabled = can_click;
                    break;
                case 0:
                    btn[x, y].BackColor = Color.Black;
                    btn[x, y].Enabled = false;
                    break;
                case 1:
                    btn[x, y].BackColor = Color.White;
                    btn[x, y].Enabled = false;
                    break;
            }
        }

        int find_valid_place(int pl)
        {
            int validCount = 0;
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    if (place(i, j, pl))
                    {
                        update_btn(i, j, true);
                        validCount++;
                    }
                    else update_btn(i, j);
                }
            }

            return validCount;
        }

        void btn_Click(object sender,EventArgs e)
        {
            Button but = (Button)sender;
            int a = Convert.ToInt32(but.Name);

            int x = a / 100;
            int y = a % 100;

            //MessageBox.Show("x="+x+"\ny="+y);

            place(x, y, player, true);

            // Change player
            player = next_player(player);
            int pass_count = 0;
            while (find_valid_place(player) == 0 && pass_count < 2)
            {
                pass_count++;
                player = next_player(player);
            }
            if (pass_count == 2)
                game_over();
        }

        void game_over()
        {
            int[] cnt = new int[2];
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    if (status[i, j] == 0) cnt[0]++;
                    else if (status[i, j] == 1) cnt[1]++;
                }
            }
            MessageBox.Show("Black: " + cnt[0] + "\nWhite: " + cnt[1]);
        }

        int next_player(int pl) { return (pl + 1) % 2; }

        bool place(int x, int y, int pl, bool real_move = false)
        {
            if (status[x, y] != -1) return false;

            bool isValid = false;
            int validCount;

            validCount = 0;
            for(int i = x + 1; i < N; i++) // R
            {
                if (status[i, y] == pl) { validCount = i - x - 1; break; }
                else if (status[i, y] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int i = x; i < N; i++)
                    {
                        if (status[i, y] == pl) break;
                        else
                        {
                            status[i, y] = pl;
                            update_btn(i, y);
                        }
                    }
                }
            }

            validCount = 0;
            for (int i = x + 1, j = y + 1; i < N && j < N; i++, j++) // BR
            {
                if (status[i, j] == pl) { validCount = i - x - 1; break; }
                else if (status[i, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int i = x + 1, j = y + 1; i < N && j < N; i++, j++)
                    {
                        if (status[i, j] == pl) break;
                        else
                        {
                            status[i, j] = pl;
                            update_btn(i, j);
                        }
                    }
                }
            }

            validCount = 0;
            for (int j = y + 1; j < N; j++) // B
            {
                if (status[x, j] == pl) { validCount = j - y - 1; break; }
                else if (status[x, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int j = y + 1; j < N; j++)
                    {
                        if (status[x, j] == pl) break;
                        else
                        {
                            status[x, j] = pl;
                            update_btn(x, j);
                        }
                    }
                }
            }

            validCount = 0;
            for (int i = x - 1, j = y + 1; i >= 0 && j < N; i--, j++) // BL
            {
                if (status[i, j] == pl) { validCount = x - i - 1; break; }
                else if (status[i, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int i = x - 1, j = y + 1; i >= 0 && j < N; i--, j++)
                    {
                        if (status[i, j] == pl) break;
                        else
                        {
                            status[i, j] = pl;
                            update_btn(i, j);
                        }
                    }
                }
            }

            validCount = 0;
            for (int i = x - 1; i >= 0; i--) // L
            {
                if (status[i, y] == pl) { validCount = x - i - 1; break; }
                else if (status[i, y] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int i = x - 1; i >= 0; i--)
                    {
                        if (status[i, y] == pl) break;
                        else
                        {
                            status[i, y] = pl;
                            update_btn(i, y);
                        }
                    }
                }
            }

            validCount = 0;
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--) // UL
            {
                if (status[i, j] == pl) { validCount = x - i - 1; break; }
                else if (status[i, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (status[i, j] == pl) break;
                        else
                        {
                            status[i, j] = pl;
                            update_btn(i, j);
                        }
                    }
                }
            }

            validCount = 0;
            for (int j = y - 1; j >= 0; j--) // U
            {
                if (status[x, j] == pl) { validCount = y - j - 1; break; }
                else if (status[x, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int j = y - 1; j >= 0; j--)
                    {
                        if (status[x, j] == pl) break;
                        else
                        {
                            status[x, j] = pl;
                            update_btn(x, j);
                        }
                    }
                }
            }

            validCount = 0;
            for (int i = x + 1, j = y - 1; i < N && j >= 0; i++, j--) // UR
            {
                if (status[i, j] == pl) { validCount = i - x - 1; break; }
                else if (status[i, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                if (real_move)
                {
                    for (int i = x + 1, j = y - 1; i < N && j >= 0; i++, j--)
                    {
                        if (status[i, j] == pl) break;
                        else
                        {
                            status[i, j] = pl;
                            update_btn(i, j);
                        }
                    }
                }
            }

            if(isValid && real_move)
            {
                status[x, y] = pl;
                update_btn(x, y);
            }

            return isValid;
        }

        
    }
}
