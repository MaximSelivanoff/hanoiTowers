using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Towers
{
    class Game_menu
    {
        public Game_field game_field;
        Button button1, button2;
        DomainUpDown DomainUpDown;
        System.Windows.Forms.Timer timer;
        public Game_menu(Panel game_panel, Button button1, Button button2, DomainUpDown domainUpDown)
        {
            this.game_field = new Game_field(game_panel);
            this.timer = new System.Windows.Forms.Timer { Interval = 30 };
            timer.Enabled = true;
            this.timer.Tick += delegate (object sender, EventArgs e)
            {
                this.game_field.Generate_frame();
            };

            this.button1 = button1;
            this.button2 = button2;
            DomainUpDown = domainUpDown;
            domainUpDown.TextChanged += Ring_select_change;
            button1.MouseClick += Start;
            button2.MouseClick += Bot_Solve;
            game_field.panel.MouseDown += Ring_mooving;
            game_field.panel.MouseUp += Ring_stop_mooving;
            game_field.panel.MouseUp += Win_check;
        }
        public void Ring_select_change(object sender, EventArgs e) // измененение количества колец для игры
        {
            game_field.Generate_field(int.Parse(DomainUpDown.Text));

        }
        public void Start(object sender, MouseEventArgs e)
        {
            game_field.Generate_field(int.Parse(DomainUpDown.Text));
        }
        public void Ring_mooving(object sender, MouseEventArgs e)
        {
            //определение кольца для перемещения и башни, на которой оно стоит
            foreach (Ring r in game_field.tower0.Rings)
            {
                if (e.X > r.ring_rect.X && e.X < (r.ring_rect.X + r.ring_rect.Width) && e.Y > r.ring_rect.Y && e.Y < r.ring_rect.Y + r.ring_rect.Height)
                {
                    if (game_field.tower0.CanMooveRing(r) == false)
                        return;
                    game_field.panel.MouseMove += Mooving;
                    ring_to_moove = r;
                    x = e.X - r.ring_rect.X;
                    y = e.Y - r.ring_rect.Y;
                    cur_tower = game_field.tower0;
                    return;
                }
            }
            foreach (Ring r in game_field.tower1.Rings)
            {
                if (e.X > r.ring_rect.X && e.X < (r.ring_rect.X + r.ring_rect.Width) && e.Y > r.ring_rect.Y && e.Y < r.ring_rect.Y + r.ring_rect.Height)
                {
                    if (game_field.tower1.CanMooveRing(r) == false)
                        return;
                    game_field.panel.MouseMove += Mooving;
                    ring_to_moove = r;
                    x = e.X - r.ring_rect.X;
                    y = e.Y - r.ring_rect.Y;
                    cur_tower = game_field.tower1;
                    return;
                }
            }
            foreach (Ring r in game_field.tower2.Rings)
            {
                if (e.X > r.ring_rect.X && e.X < (r.ring_rect.X + r.ring_rect.Width) && e.Y > r.ring_rect.Y && e.Y < r.ring_rect.Y + r.ring_rect.Height)
                {
                    if (game_field.tower2.CanMooveRing(r) == false)
                        return;
                    game_field.panel.MouseMove += Mooving;
                    ring_to_moove = r;
                    x = e.X - r.ring_rect.X;
                    y = e.Y - r.ring_rect.Y;
                    cur_tower = game_field.tower2;
                    return;
                }
            }
        }
        Tower cur_tower;
        Ring ring_to_moove;
        int x, y;
        public void Mooving(object sender, MouseEventArgs e)
        {
            ring_to_moove.ring_rect.X = e.X - x;
            ring_to_moove.ring_rect.Y = e.Y - y;
        }
        public void Ring_stop_mooving(object sender, MouseEventArgs e)
        {
            if (ring_to_moove == null)
                return;
            if (ring_to_moove.ring_rect.IntersectsWith(game_field.tower0.tower_rect))
                if (game_field.tower0.AddRing(ring_to_moove))  // если добавили кольцо в новую башню
                    cur_tower.DelRing(ring_to_moove);

            if (ring_to_moove.ring_rect.IntersectsWith(game_field.tower1.tower_rect))
                if (game_field.tower1.AddRing(ring_to_moove))
                    cur_tower.DelRing(ring_to_moove);

            if (ring_to_moove.ring_rect.IntersectsWith(game_field.tower2.tower_rect))
                if (game_field.tower2.AddRing(ring_to_moove))
                    cur_tower.DelRing(ring_to_moove);
            game_field.Update_towers();
            game_field.panel.MouseMove -= Mooving;
        }
        public void Win_check(object sender, MouseEventArgs e)
        {
            if (game_field.tower1.Rings.Count == int.Parse(DomainUpDown.Text.ToString()))
            {
                MessageBox.Show("WiN!!!!");
                game_field.Generate_field(int.Parse(DomainUpDown.Text.ToString()));
            }
            if (game_field.tower2.Rings.Count == int.Parse(DomainUpDown.Text.ToString()))
            {
                MessageBox.Show("WiN!!!!");
                game_field.Generate_field(int.Parse(DomainUpDown.Text.ToString()));
            }
        }
        Tower from, to;
        int[] step;
        public void Bot_Solve(object sender, MouseEventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            DomainUpDown.Enabled = false;
            game_field.Generate_field(int.Parse(DomainUpDown.Text.ToString()));
            Bot_steps = new List<int[]>();
            Bot_solve_generation(int.Parse(DomainUpDown.Text.ToString()), 0, 1);
            timer.Tick += Bot_Ring_mooving;

        }
        bool y_up = true;
        void Bot_Ring_mooving(object sender, EventArgs e)
        {
            step = Bot_steps.First();
            switch (step[0])
            {
                case 0: from = game_field.tower0; break;
                case 1: from = game_field.tower1; break;
                case 2: from = game_field.tower2; break;
            }
            switch (step[1])
            {
                case 0: to = game_field.tower0; break;
                case 1: to = game_field.tower1; break;
                case 2: to = game_field.tower2; break;
            }
            bool step_out = false;
            if (step[0] < step[1])
            {
                if (from.GetTopRing().ring_rect.Y > 50 && y_up) // вверх
                    from.GetTopRing().ring_rect.Y -= 15;
                else
                {
                    if (from.GetTopRing().ring_rect.X + from.GetTopRing().ring_rect.Width / 2 < to.x_centr_pozition)
                        from.GetTopRing().ring_rect.X += 15;
                    else
                    {
                        if (from.GetTopRing().ring_rect.Y < to.GetTopRingYcoord() - 30)
                        {
                            from.GetTopRing().ring_rect.Y += 15;
                            y_up = false;
                        }
                        else
                        {
                            step_out = true;
                            y_up = true;
                        }
                    }

                }
            }
            if (step[0] > step[1]) // справа налево
            {
                if (from.GetTopRing().ring_rect.Y > 50 && y_up) // вверх
                {
                    from.GetTopRing().ring_rect.Y -= 15;
                }
                else
                {
                    if (from.GetTopRing().ring_rect.X + from.GetTopRing().ring_rect.Width / 2 > to.x_centr_pozition)
                        from.GetTopRing().ring_rect.X -= 15;
                    else
                    {
                        if (from.GetTopRing().ring_rect.Y < to.GetTopRingYcoord() - 30)// вниз
                        {
                            y_up = false;
                            from.GetTopRing().ring_rect.Y += 15;
                        }
                        else
                        {
                            step_out = true;
                            y_up = true;
                        }
                    }
                }
            }

            if (step_out)
            {
                to.AddRing(from.GetTopRing());
                from.DelRing(from.GetTopRing());
                Bot_steps.Remove(Bot_steps.First());
                if (Bot_steps.Count == 0)
                {
                    timer.Tick -= Bot_Ring_mooving;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    DomainUpDown.Enabled = true;
                }
            }
        }
        List<int[]> Bot_steps;
        void Bot_solve_generation(int rings0_count, int i, int k)
        {
            if (rings0_count == 1)
            {
                int[] bot_step = new int[2] { i, k };
                Bot_steps.Add(bot_step);
            }
            else
            {
                int tmp = 3 - i - k;
                Bot_solve_generation(rings0_count - 1, i, tmp);
                int[] bot_step = new int[2] { i, k };
                Bot_steps.Add(bot_step);
                Bot_solve_generation(rings0_count - 1, tmp, k);
            }
        }
    }
}