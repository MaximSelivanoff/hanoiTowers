using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanoi_Towers
{
    class Tower
    {

        int number;
        public List<Ring> Rings;
        public Point pozition;
        public int x_centr_pozition;
        public int width = 20;
        int rings_count;
        public Rectangle tower_rect;
        static Random random = new Random();
        public Tower(int number, int rings_count, Point pozition)
        {
            Rings = new List<Ring>();
            this.pozition = pozition;
            this.number = number;
            this.rings_count = rings_count;
            x_centr_pozition = pozition.X + width / 2; // центр башни по оси Х
            tower_rect = new Rectangle(pozition.X, 100, width, 1000);
            Generate_Rings();
        }
        public void Generate_Rings()
        {
            for (int i = rings_count - 1; i >= 0; i--)
            {
                Size r_size = new Size((i + 1) * 40, 30);
                Point r_pozition = new Point(x_centr_pozition - r_size.Width / 2, pozition.Y - (rings_count - i) * 30);
                Ring r = new Ring(r_pozition, r_size, Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)));
                Rings.Add(r);
            }
        }
        public void Update()
        {
            int i = Rings.Count;
            foreach(Ring r in Rings)
            {
                i--;
                r.pozition = new Point(x_centr_pozition - r.size.Width / 2, pozition.Y - (Rings.Count - i) * 30);
                r.ring_rect.Location = r.pozition;
            }
        }
        public void Clear()
        {
            Rings = null;
        }
        public bool AddRing(Ring ring)
        {
            if (Rings.Count != 0 && Rings.Last().size.Width < ring.size.Width)
                return false;
            else
            {
                Rings.Add(ring);
                Update();
            }
            return true;
        }
        public bool CanMooveRing(Ring ring)
        {
            if (ring.pozition.Y < pozition.Y - (Rings.Count - 1) * ring.size.Height)
                return true;
            else 
                return false;
        }
        public Ring GetTopRing()
        {
            if (Rings.Count != 0)
                return Rings.Last();
            else return null;
        }
        public int GetTopRingYcoord()
        {
            if (Rings.Count != 0)
                return Rings.Last().ring_rect.Y;
            else return pozition.Y;
        }
        public void DelRing(Ring ring)
        {
            Rings.Remove(ring);
            Update();
        }
    }
}
