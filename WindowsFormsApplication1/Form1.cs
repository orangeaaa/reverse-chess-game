using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReversiClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            btn = new Button[board.N, board.N];
            gameNet = new comm();
        }
        reversi_board board = new reversi_board();
        comm gameNet;
        private Button[,] btn;

        private int square_size = 60;
        private int margin = 50;

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Reversi";

            init_display_board();

            board.game_start();
            ready_for_next_play();

        }

        void init_display_board() {
            BackColor = Color.White;
            Size size = new Size(margin * 2 + square_size * board.N, margin * 2 + square_size * board.N);
            this.ClientSize = size;
            CenterToScreen();

            for (int i = 0; i < board.N; i++) {
                for (int j = 0; j < board.N; j++) {
                    btn[i, j] = new Button();
                    btn[i, j].Location = new Point(margin + i * square_size, margin + j * square_size);
                    btn[i, j].Size = new Size(square_size, square_size);
                    update_btn(i, j);
                    btn[i, j].Name = (i * 100 + j).ToString();
                    this.Controls.Add(btn[i, j]);
                    btn[i, j].Click += btn_Click;
                }
            }
        }

        void update_btn(int x, int y, bool can_click = false)
        {
            switch (board.status[x, y])
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

        void update_all_buttons() {
            for(int i = 0; i < board.N; i++) {
                for(int j = 0; j < board.N; j++) {
                    update_btn(i, j);
                }
            }
            foreach(var choice in board.cur_valid_places) {
                update_btn(choice.Item1, choice.Item2, true);
            }
        }

        void ready_for_next_play() {
            board.find_valid_place(board.player);
            update_all_buttons();

            if (board.game_phase == 2) { // End of game
                game_over();
            }
        }

        void btn_Click(object sender,EventArgs e)
        {
            Button but = (Button)sender;
            int a = Convert.ToInt32(but.Name);

            int x = a / 100;
            int y = a % 100;

            //MessageBox.Show("x="+x+"\ny="+y);

            board.make_move(x, y);
            ready_for_next_play();
        }

        void game_over()
        {
            MessageBox.Show("Black: " + board.cnt[0] + "\nWhite: " + board.cnt[1]);
        }
        
    }
}
