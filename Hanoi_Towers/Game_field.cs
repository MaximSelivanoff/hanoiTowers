using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Towers
{
    class Game_field
    {
        public Panel panel;
        public Tower tower0, tower1, tower2;

        Graphics g;
        BufferedGraphics buf;
        BufferedGraphicsContext contex;

        public Game_field(Panel panel)
        {
            this.panel = panel;
            //this.tower_size = new Size(panel.Width / 3, panel.Height - panel.Height / 4);
            this.contex = BufferedGraphicsManager.Current;
            this.buf = contex.Allocate(this.panel.CreateGraphics(), this.panel.DisplayRectangle);
            this.g = this.buf.Graphics;
            this.tower0 = new Tower(0, 0, new Point(panel.Width / 6 * 1, panel.Height));
            this.tower1 = new Tower(1, 0, new Point(panel.Width / 6 * 3, panel.Height));
            this.tower2 = new Tower(2, 0, new Point(panel.Width / 6 * 5, panel.Height));

        }
        public void Generate_frame() // прорисовка поля игры
        {
            g.Clear(Color.LightGray);
            SolidBrush brush = new SolidBrush(Color.DodgerBlue);
            this.g.FillRectangle(brush, tower0.tower_rect);
            this.g.FillRectangle(brush, tower2.tower_rect);
            this.g.FillRectangle(brush, tower1.tower_rect);
            if (tower0.Rings != null)
                for (int i = 0; i < tower0.Rings.Count; i++)
                    this.g.FillRectangle(new SolidBrush(tower0.Rings[i].color), tower0.Rings[i].ring_rect);

            if (tower1.Rings != null)
            {
                for (int i = 0; i < tower1.Rings.Count; i++)
                {
                    this.g.FillRectangle(new SolidBrush(tower1.Rings[i].color), tower1.Rings[i].ring_rect);
                }
            }
            if (tower2.Rings != null)
            {
                for (int i = 0; i < tower2.Rings.Count; i++)
                {
                    this.g.FillRectangle(new SolidBrush(tower2.Rings[i].color), tower2.Rings[i].ring_rect);
                }
            }

            this.buf.Render();

        }
        void Clear() // очистка поля
        {
            if (tower0 != null && tower1 != null & tower2 != null)
            {
                tower0.Clear();
                tower1.Clear();
                tower2.Clear();
            }
        }
        public void Update_towers()
        {
            tower0.Update();
            tower1.Update();
            tower2.Update();
        }
        public void Generate_field(int rings_count) // генерация поля с заданным количеством колец
        {
            Clear();
            this.tower0 = new Tower(0, rings_count, new Point(panel.Width / 6 * 1, panel.Height));
            this.tower1 = new Tower(1, 0, new Point(panel.Width / 6 * 3, panel.Height));
            this.tower2 = new Tower(2, 0, new Point(panel.Width / 6 * 5, panel.Height));
        }
    }
}
