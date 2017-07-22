﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ReversiClient.network;

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

        // Resources on the form
        // board
        private Button[,] btn;
        // menu
        Button menu_btn_single,
            menu_btn_create_lobby,
            menu_btn_find_lobby;
        // waiting for connection (server and client)
        Label waiting_conn;
        Button cancel_waiting_conn;
        PictureBox pic_waiting_conn;
        // finding lobby
        ListBox lobby_list;
        Button cancel_finding_lobby,
            btn_join_lobby;
        string ip_to_connect;

        private int square_size = 60;
        private int margin = 50;

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Reversi";
            BackColor = Color.White;

            init_display_menu();
        }

        void init_display_menu() {
            this.ClientSize = new Size(400, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            CenterToScreen();

            menu_btn_single = new Button();
            menu_btn_single.Location = new Point(50, 50);
            menu_btn_single.Size = new Size(300, 100);
            menu_btn_single.Text = "Single Player";
            menu_btn_single.Click += menu_btn_single_Click;
            this.Controls.Add(menu_btn_single);

            menu_btn_create_lobby = new Button();
            menu_btn_create_lobby.Location = new Point(50, 150);
            menu_btn_create_lobby.Size = new Size(300, 100);
            menu_btn_create_lobby.Text = "Create Lobby";
            menu_btn_create_lobby.Click += menu_btn_create_lobby_Click;
            this.Controls.Add(menu_btn_create_lobby);

            menu_btn_find_lobby = new Button();
            menu_btn_find_lobby.Location = new Point(50, 250);
            menu_btn_find_lobby.Size = new Size(300, 100);
            menu_btn_find_lobby.Text = "Find Lobby";
            menu_btn_find_lobby.Click += menu_btn_find_lobby_Click;
            this.Controls.Add(menu_btn_find_lobby);

        }
        void destroy_display_menu() {
            this.Controls.Remove(menu_btn_single);
            menu_btn_single.Dispose();
            this.Controls.Remove(menu_btn_create_lobby);
            menu_btn_create_lobby.Dispose();
            this.Controls.Remove(menu_btn_find_lobby);
            menu_btn_find_lobby.Dispose();
        }

        void init_display_waitConn() {
            // retain menu window settings

            waiting_conn = new Label() {
                Location = new Point(0, 200),
                Size = new Size(400, 50),
                Text = "Waiting for a rival...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(waiting_conn);

            cancel_waiting_conn = new Button() {
                Location = new Point(50, 300),
                Size = new Size(300, 50),
                Text = "Cancel"
            };
            cancel_waiting_conn.Click += btn_cancel_wait_conn_Click;
            this.Controls.Add(cancel_waiting_conn);

            var thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            var file = thisExe.GetManifestResourceStream("ReversiClient.w8loader.gif");
            pic_waiting_conn = new PictureBox() {
                Location = new Point(175, 150),
                Size = new Size(50, 50),
                Image = Image.FromStream(file)
            };
            this.Controls.Add(pic_waiting_conn);
        }
        void destroy_display_waitConn() {
            this.Controls.Remove(waiting_conn);
            waiting_conn.Dispose();
            this.Controls.Remove(cancel_waiting_conn);
            cancel_waiting_conn.Dispose();
            this.Controls.Remove(pic_waiting_conn);
            pic_waiting_conn.Dispose();
        }

        void init_display_findLobby() {
            lobby_list = new ListBox() {
                Location = new Point(50, 50),
                Size = new Size(300, 200),
                SelectionMode = SelectionMode.One
            };
            lobby_list_initialize();
            this.Controls.Add(lobby_list);
            gameNet.LobbyList.RaiseListChangeEvent += lobby_list_update_event;

            cancel_finding_lobby = new Button() {
                Location = new Point(50, 300),
                Size = new Size(300, 50),
                Text = "Cancel"
            };
            cancel_finding_lobby.Click += btn_cancel_find_lobby_Click;
            this.Controls.Add(cancel_finding_lobby);

            btn_join_lobby = new Button() {
                Location = new Point(50, 250),
                Size = new Size(300, 50),
                Text = "Join"
            };
            btn_join_lobby.Click += menu_btn_join_lobby_Click;
            this.Controls.Add(btn_join_lobby);
        }
        void destroy_display_findLobby() {
            gameNet.LobbyList.RaiseListChangeEvent -= lobby_list_update_event;
            this.Controls.Remove(lobby_list);
            lobby_list.Dispose();
            this.Controls.Remove(cancel_finding_lobby);
            cancel_finding_lobby.Dispose();
            this.Controls.Remove(btn_join_lobby);
            btn_join_lobby.Dispose();
        }
        void lobby_list_update_event(object source, ListChangeEventArgs e) {
            if (InvokeRequired) {
                Invoke(new Action<object, ListChangeEventArgs>(lobby_list_update_event), source, e);
                return;
            }
            string content = e.ip.Address.ToString();
            if (e.mode == 1) {
                lobby_list.Items.Add(content);
            }
            else if (e.mode == 2) {
                if (lobby_list.Items.Contains(content))
                    lobby_list.Items.Remove(content);
            }
        }
        void lobby_list_initialize() {
            lobby_list.Items.Clear();
            foreach(var item in gameNet.LobbyList.HostList) {
                lobby_list.Items.Add(item.Item1.Address.ToString());
            }
        }

        void init_display_waitConn_srv() {
            // retain menu window settings

            waiting_conn = new Label() {
                Location = new Point(0, 200),
                Size = new Size(400, 50),
                Text = "Connecting to rival...",
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(waiting_conn);

            cancel_waiting_conn = new Button() {
                Location = new Point(50, 300),
                Size = new Size(300, 50),
                Text = "Cancel"
            };
            cancel_waiting_conn.Click += btn_cancel_conn_srv_Click;
            this.Controls.Add(cancel_waiting_conn);

            var thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            var file = thisExe.GetManifestResourceStream("ReversiClient.w8loader.gif");
            pic_waiting_conn = new PictureBox() {
                Location = new Point(175, 150),
                Size = new Size(50, 50),
                Image = Image.FromStream(file)
            };
            this.Controls.Add(pic_waiting_conn);
        }


        void init_display_board() {
            Size size = new Size(margin * 2 + square_size * board.N, margin * 2 + square_size * board.N);
            this.ClientSize = size;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
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
        void destroy_display_board() {
            for (int i = 0; i < board.N; i++) {
                for (int j = 0; j < board.N; j++) {
                    this.Controls.Remove(btn[i, j]);
                    btn[i, j].Dispose();
                }
            }
        }

        void menu_btn_single_Click(object sender, EventArgs e) {
            destroy_display_menu();
            init_display_board();
            board.game_start();
            ready_for_next_play();
        }
        void menu_btn_create_lobby_Click(object sender, EventArgs e) {
            destroy_display_menu();
            gameNet.send_beacon();
            init_display_waitConn();
        }
        void btn_cancel_wait_conn_Click(object sender, EventArgs e) {
            destroy_display_waitConn();
            gameNet.send_beacon(false);
            init_display_menu();
        }

        void menu_btn_find_lobby_Click(object sender, EventArgs e) {
            destroy_display_menu();
            init_display_findLobby();
            gameNet.beacon_receiving();
        }
        void menu_btn_join_lobby_Click(object sender, EventArgs e) {
            if (lobby_list.SelectedItem == null) {
                System.Diagnostics.Debug.WriteLine("No item selected.");
                return;
            }
            // Find ip endpoint in stored list
            bool ip_valid = false;
            foreach(var item in gameNet.LobbyList.HostList) {
                if (item.Item1.Address.ToString().Equals(lobby_list.SelectedItem.ToString())) {
                    ip_to_connect = item.Item1.Address.ToString();
                    ip_valid = true;
                    break;
                }
            }
            if (ip_valid) {
                destroy_display_findLobby();
                init_display_waitConn_srv();
                try {
                    gameNet.game_cli_connect(ip_to_connect);
                }
                catch (TimeoutException) {
                    waiting_conn.Text = "Connection failed.";
                }
            }
        }
        void btn_cancel_find_lobby_Click(object sender, EventArgs e) {
            destroy_display_findLobby();
            gameNet.beacon_receiving_stop();
            init_display_menu();
        }

        void btn_cancel_conn_srv_Click(object sender,EventArgs e) {
            destroy_display_waitConn();
            init_display_findLobby();
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
