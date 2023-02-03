using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Towers
{
    class Ring
    {
        public Size size;
        public Point pozition;
        public Color color;
        public Rectangle ring_rect;

        public Ring(Point pozition, Size size, Color color)
        {
            this.size = size;
            this.pozition = pozition;
            this.color = color;
            ring_rect = new Rectangle(pozition, size);
        }
    }
}
