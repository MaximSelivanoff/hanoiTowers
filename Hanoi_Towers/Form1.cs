using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hanoi_Towers
{
    public partial class Form1 : Form
    {
        Game_menu menu;
        public Form1()
        {
            InitializeComponent();
            this.menu = new Game_menu(panel2, button1, button2, domainUpDown1);
        }
    }
}
