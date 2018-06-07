using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        Maze bludiste;
        Compute comp;

        public Form1()
        {
            InitializeComponent();

            Maze.VelikostPole = 15;
            Maze.ZakladPosun = 15; //Vždy musí být větší nebo rovno jak VelikostPole
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bludiste = new Maze(panel1.Width, panel1.Height);
            comp = null;
            panel1.Invalidate(); //Vytvoř nové bludiště, vynuluju cestu a zavolej panel1_paint
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comp = new Compute(Maze.pole);
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (bludiste != null)
                bludiste.DrawMaze(g);

            if(comp != null)
            {
                comp.CalculateWay();
                comp.DrawWay(e.Graphics);
            } //Pokud už není vytvořené (například aby se furt nevytvářelo nové bludiště při resize nebo při startu aplikace
        }
    }
}
