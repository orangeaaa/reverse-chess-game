using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class reversi_board
    {
        internal int N = 8;
        internal int[,] status;
        internal int steps;
        internal int player;
        internal int game_phase; // 0: before start, 1: in game, 2: ended

        // moves is a list of individual moves
        // Each move is a list of individual operations
        // Each operation is a tuple of (x, y, old_status, new_status)
        internal List<List<Tuple<int, int, int, int>>> moves = new List<List<Tuple<int, int, int, int>>>();

        internal List<Tuple<int, int>> cur_valid_places = new List<Tuple<int, int>>();

        internal int[] cnt = new int[2];

        public reversi_board()
        {
            status = new int[N, N];
            game_phase = 0;
        }

        internal void game_start()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    status[i, j] = -1;
                }
            }
            status[3, 4] = status[4, 3] = 0;
            status[3, 3] = status[4, 4] = 1;
            steps = 0;
            player = 0;
            game_phase = 1;
        }

        internal void game_over()
        {
            Array.Clear(cnt, 0, cnt.Length);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (status[i, j] == 0) cnt[0]++;
                    else if (status[i, j] == 1) cnt[1]++;
                }
            }
            game_phase = 2;
        }

        internal void make_move(int x,int y) {
            place(x, y, player, true);

            next_player();

            int pass_count = 0;
            while (find_valid_place(player) == 0 && pass_count < 2) {
                pass_count++;
                next_player();
            }
            if (pass_count == 2)
                game_over();

        }

        internal int find_valid_place(int pl)
        {
            int validCount = 0;
            cur_valid_places.Clear();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (place(i, j, pl))
                    {
                        cur_valid_places.Add(new Tuple<int, int>(i, j));
                        validCount++;
                    }
                }
            }

            return validCount;
        }

        internal bool place(int x, int y, int pl, bool real_move = false)
        {
            if (status[x, y] != -1) return false;

            var pro_move = new List<Tuple<int, int, int, int>>();

            bool isValid = false;
            int validCount;

            validCount = 0;
            for (int i = x + 1; i < N; i++) // R
            {
                if (status[i, y] == pl) { validCount = i - x - 1; break; }
                else if (status[i, y] == -1) { validCount = 0; break; }
            }
            if (validCount > 0)
            {
                isValid = true;
                for (int i = x; i < N; i++)
                {
                    if (status[i, y] == pl) break;
                    else
                    {
                        pro_move.Add(new Tuple<int, int, int, int>(i, y, status[i, y], pl));
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
                for (int i = x + 1, j = y + 1; i < N && j < N; i++, j++)
                {
                    if (status[i, j] == pl) break;
                    else
                    {
                        pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
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
                for (int j = y + 1; j < N; j++)
                {
                    if (status[x, j] == pl) break;
                    else
                    {
                        pro_move.Add(new Tuple<int, int, int, int>(x, j, status[x, j], pl));
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
                for (int i = x - 1, j = y + 1; i >= 0 && j < N; i--, j++)
                {
                    if (status[i, j] == pl) break;
                    else
                    {
                        pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
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
                for (int i = x - 1; i >= 0; i--)
                {
                    if (status[i, y] == pl) break;
                    else
                    {
                        pro_move.Add(new Tuple<int, int, int, int>(i, y, status[i, y], pl));
                    }
                }
            }

            validCount = 0;
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--) // UL
            {
                if (status[i, j] == pl) { validCount = x - i - 1; break; }
                else if (status[i, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0) {
                isValid = true;
                for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--) {
                    if (status[i, j] == pl) break;
                    else {
                        pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
                    }
                }
            }

            validCount = 0;
            for (int j = y - 1; j >= 0; j--) // U
            {
                if (status[x, j] == pl) { validCount = y - j - 1; break; }
                else if (status[x, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0) {
                isValid = true;
                for (int j = y - 1; j >= 0; j--) {
                    if (status[x, j] == pl) break;
                    else {
                        pro_move.Add(new Tuple<int, int, int, int>(x, j, status[x, j], pl));
                    }
                }
            }

            validCount = 0;
            for (int i = x + 1, j = y - 1; i < N && j >= 0; i++, j--) // UR
            {
                if (status[i, j] == pl) { validCount = i - x - 1; break; }
                else if (status[i, j] == -1) { validCount = 0; break; }
            }
            if (validCount > 0) {
                isValid = true;
                for (int i = x + 1, j = y - 1; i < N && j >= 0; i++, j--) {
                    if (status[i, j] == pl) break;
                    else {
                        pro_move.Add(new Tuple<int, int, int, int>(i, j, status[i, j], pl));
                    }
                }
            }

            if (isValid)
            {
                pro_move.Add(new Tuple<int, int, int, int>(x, y, status[x, y], pl));
            }

            if (real_move) {
                moves.Add(pro_move);
                exe_move(pro_move);
            }

            return isValid;
        }

        void exe_move(List<Tuple<int,int,int,int>> which_move, bool forward = true) {
            foreach(var op in which_move) {
                status[op.Item1, op.Item2] = (forward ? op.Item4 : op.Item3);
            }
            if (forward) steps++;
            else steps--;
        }


        void next_player() { player = (player + 1) % 2; }

    }
}
