using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Maze
{
    class Compute
    {
        Obsah[,] pole;
        int[,] computePole;
        Point start, end;

        public Compute(Obsah[,] pole)
        {
            this.pole = pole;
            computePole = new Int32[pole.GetLength(0), pole.GetLength(1)]; //Vytvoří int pole pro výpočty cesty (stejně velké jako pole bludiště)
            FindStartEnd();
        }
        private void FindStartEnd() //Projede všechny pozice a najde start | end
        {
            for(int x=0;x<pole.GetLength(0);x++)
            {
                for(int y = 0;y<pole.GetLength(1);y++)
                {
                    if (pole[x, y] == Obsah.start)
                        start = new Point(x, y);
                    if (pole[x, y] == Obsah.end)
                        end = new Point(x, y);
                }
            }
        }

        public void CalculateWay()
        {
            List<Point> fronta = new List<Point>() { start };
            List<Point> tempLst = new List<Point>();

            computePole[start.X, start.Y] = 1;
            while (true)
            {
                foreach (Point bod in fronta)
                {

                    //  if (pole[bod.x + 1, bod.y] != Obsah.stena || pole[bod.x - 1, bod.y] != Obsah.stena || pole[bod.x, bod.y + 1] != Obsah.stena || pole[bod.x, bod.y - 1] != Obsah.stena)
                    //    computePole[bod.x, bod.y]++;
                    //////

                    if (pole[bod.X + Maze.VelikostPole, bod.Y] != Obsah.zed && computePole[bod.X + Maze.VelikostPole, bod.Y] == 0)
                    {
                        computePole[bod.X + Maze.VelikostPole, bod.Y] = computePole[bod.X, bod.Y] + 1;
                        tempLst.Add(new Point(bod.X + Maze.VelikostPole, bod.Y));
                    } //Doprava je volno
                    if (pole[bod.X - Maze.VelikostPole, bod.Y] != Obsah.zed && computePole[bod.X - Maze.VelikostPole, bod.Y] == 0)
                    {
                        computePole[bod.X - Maze.VelikostPole, bod.Y] = computePole[bod.X, bod.Y] + 1;
                        tempLst.Add(new Point(bod.X - Maze.VelikostPole, bod.Y));
                    } //Doleva je volno
                    if (pole[bod.X, bod.Y + Maze.VelikostPole] != Obsah.zed && computePole[bod.X, bod.Y + Maze.VelikostPole] == 0)
                    {
                        computePole[bod.X, bod.Y + Maze.VelikostPole] = computePole[bod.X, bod.Y] + 1;
                        tempLst.Add(new Point(bod.X, bod.Y + Maze.VelikostPole));
                    } //Dolů je volno
                    if (pole[bod.X, bod.Y - Maze.VelikostPole] != Obsah.zed && computePole[bod.X, bod.Y - Maze.VelikostPole] == 0)
                    {
                        computePole[bod.X, bod.Y - Maze.VelikostPole] = computePole[bod.X, bod.Y] + 1;
                        tempLst.Add(new Point(bod.X, bod.Y - Maze.VelikostPole));
                    } //Nahoru je volno
                }
                fronta = new List<Point>(tempLst);
                tempLst.Clear();
                if (fronta.Count == 0) return; //Algoritmus VLNA - Itnetwork - algoritmy - Vlna (hledá všechny možné cesty)
            }
        }
        public void DrawWay(Graphics g)
        {
            bool repeat = true;
            List<Point> vysledek = new List<Point>();
            Point bod = end;
            while (repeat)
            {
                
                    if (computePole[bod.X + Maze.VelikostPole, bod.Y] == computePole[bod.X,bod.Y] - 1)
                    {
                        bod = new Point(bod.X + Maze.VelikostPole, bod.Y);
                    } //Vpravo je o jedno menší
                    else if (computePole[bod.X - Maze.VelikostPole, bod.Y] == computePole[bod.X, bod.Y] - 1)
                    {
                        bod = new Point(bod.X-+ Maze.VelikostPole, bod.Y);
                    } //Vlevo je o jedno menší
                    else if (computePole[bod.X, bod.Y + Maze.VelikostPole] == computePole[bod.X, bod.Y] - 1)
                    {
                        bod = new Point(bod.X, bod.Y + Maze.VelikostPole);
                    } //Dole o jedno menší
                    else if (computePole[bod.X, bod.Y - Maze.VelikostPole] == computePole[bod.X, bod.Y] - 1)
                    {
                        bod = new Point(bod.X, bod.Y - Maze.VelikostPole);
                    } //Hore o jedno menšé
                if (bod == start)
                    repeat = false;
                else
                vysledek.Add(bod);
            } //Najde pozice první cesty


            foreach (Point pozice in vysledek)
            {
                Rectangle rec = new Rectangle(pozice, new Size(Maze.VelikostPole, Maze.VelikostPole));
                g.FillEllipse(new SolidBrush(Color.Blue), rec);
            } //Vykreslí
        }
    }
}
