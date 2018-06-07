using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze
{
    public enum Obsah
    {
        volno,
        zed,
        zaklad,
        start,
        end

    };

    public class Maze
    {
        public static int VelikostPole { get; set; }
        public static int ZakladPosun
        {
            get { return zakladPosun; }
            set
            {
                zakladPosun = (value >= VelikostPole) ? zakladPosun = VelikostPole + value - (value % VelikostPole) : VelikostPole; //Podmínka se splní, ale dál to blbne. Nevím proč
            }
        }

        public static Obsah[,] pole;
        int height, width;
        Random rand;
        List<Point> zaklady;
        static int zakladPosun;
      
        public Maze(int width,int height)
        {
            this.width = width - (width % VelikostPole);
            this.height = height - (height % VelikostPole); // výpočet aby přestalo vykreslovat pokud už se to nevejde

            pole = new Obsah[width, height];
            rand = new Random();
            zaklady = new List<Point>();

            BasicMaze();
            GenerateMaze();
            GenerateStartEnd();
        }

        private void BasicMaze()
        {
            #region StěnaOkolo
            for (int x = 0; x < width; x++) 
            {
                pole[x, 0] = Obsah.zed;
                pole[x, height - VelikostPole] = Obsah.zed;
            } //stěna na x souřadnicích
            for (int y = 0; y < height; y++)
            {
                pole[0, y] = Obsah.zed;
                pole[width - VelikostPole, y] = Obsah.zed;
            } //stěna na y souřadnicích
            #endregion

            #region Zaklad
            for (int x = zakladPosun; x < width - zakladPosun; x += zakladPosun)  //Bude se posunovat o 15 (nastavíme v properties)
            {
                for (int y = zakladPosun; y < height - zakladPosun; y += zakladPosun)
                {
                    pole[x, y] = Obsah.zaklad;
                    zaklady.Add(new Point(x,y)); // uloží základ point do fronty základů
                }

            }
            #endregion
        }
        private void GenerateMaze()
        {
            while (zaklady.Count != 0)
            {
                int nahodZaklad = rand.Next(zaklady.Count);
                Point nahodnyZaklad = zaklady[nahodZaklad];
                zaklady.RemoveAt(nahodZaklad); //vybere náhodný základ a poté ho smaže z fronty

                int smer = rand.Next(1, 5);  // 4směry  left,right,top,bottom
                switch (smer) //Mění pole na stěnu dokud nenarazí na stěnu
                {
                    case 1: //levo
                        while (pole[nahodnyZaklad.X, nahodnyZaklad.Y] != Obsah.zed)
                        {
                            if (pole[nahodnyZaklad.X, nahodnyZaklad.Y] == Obsah.zaklad)
                            {
                                zaklady.Remove(new Point(nahodnyZaklad.X,nahodnyZaklad.Y));
                            }

                            pole[nahodnyZaklad.X, nahodnyZaklad.Y] = Obsah.zed;
                            nahodnyZaklad.X--;
                        }
                        break;
                    case 2: //pravo
                        while (pole[nahodnyZaklad.X, nahodnyZaklad.Y] != Obsah.zed)
                        {
                            if (pole[nahodnyZaklad.X, nahodnyZaklad.Y] == Obsah.zaklad)
                            {
                                zaklady.Remove(new Point(nahodnyZaklad.X, nahodnyZaklad.Y));
                            }

                            pole[nahodnyZaklad.X, nahodnyZaklad.Y] = Obsah.zed;
                            nahodnyZaklad.X++;
                        }
                        break;
                    case 3: //nahoru
                        while (pole[nahodnyZaklad.X, nahodnyZaklad.Y] != Obsah.zed)
                        {
                            if (pole[nahodnyZaklad.X, nahodnyZaklad.Y] == Obsah.zaklad)
                            {
                                zaklady.Remove(new Point(nahodnyZaklad.X, nahodnyZaklad.Y));
                            }

                            pole[nahodnyZaklad.X, nahodnyZaklad.Y] = Obsah.zed;
                            nahodnyZaklad.Y--;
                        }
                        break;
                    case 4://dolu
                        while (pole[nahodnyZaklad.X, nahodnyZaklad.Y] != Obsah.zed)
                        {
                            if (pole[nahodnyZaklad.X, nahodnyZaklad.Y] == Obsah.zaklad)
                            {
                                zaklady.Remove(new Point(nahodnyZaklad.X, nahodnyZaklad.Y));
                            }

                            pole[nahodnyZaklad.X, nahodnyZaklad.Y] = Obsah.zed;
                            nahodnyZaklad.Y++;
                        }
                        break;
                }
            }
        }
        private void GenerateStartEnd()
        {
            int volba =rand.Next(1, 5);
            switch (volba)
            {
                case 1:
                    pole[VelikostPole, VelikostPole] = Obsah.start; //levo hore
                    break;
                case 2:
                    pole[width - VelikostPole - VelikostPole, VelikostPole] = Obsah.start; //pravo hore
                    break;
                case 3:
                    pole[VelikostPole, height - VelikostPole - VelikostPole] = Obsah.start; //levo dole
                    break;
                case 4:
                    pole[width - VelikostPole - VelikostPole, height - VelikostPole - VelikostPole] = Obsah.start; // pravo dole
                    break;

            }//Vybere roh pro start

            volba = rand.Next(1, 5);
            switch (volba)
            {
                case 1: // levej horní roh
                    if (pole[VelikostPole, VelikostPole] != Obsah.start)
                        pole[VelikostPole, VelikostPole] = Obsah.end;
                    else //jinak pravej horní
                        pole[width - VelikostPole - VelikostPole, VelikostPole] = Obsah.end; 
                    break;
                case 2: //pravej horní
                    if (pole[width - VelikostPole - VelikostPole, VelikostPole] != Obsah.start)
                        pole[width - VelikostPole - VelikostPole, VelikostPole] = Obsah.end;
                    else //jinak levej horní
                        pole[VelikostPole, VelikostPole] = Obsah.end;
                    break;
                case 3: //levo dole
                    if (pole[VelikostPole, height - VelikostPole - VelikostPole] != Obsah.start)
                        pole[VelikostPole, height - VelikostPole - VelikostPole] = Obsah.end;
                    else //jinak pravo dole
                        pole[width - VelikostPole - VelikostPole, height - VelikostPole - VelikostPole] = Obsah.end; 
                    break;
                case 4: //pravo dole
                    if (pole[width - VelikostPole - VelikostPole, height - VelikostPole - VelikostPole] != Obsah.start)
                        pole[width - VelikostPole - VelikostPole, height - VelikostPole - VelikostPole] = Obsah.end;
                    else //jinak levo dole
                        pole[VelikostPole, height - VelikostPole - VelikostPole] = Obsah.end;
                    break;
            } //Pozice pro End
        }
        public void DrawMaze(Graphics g)
        {
            for (int x = 0; x <= width - VelikostPole; x++)
            {
                for (int y = 0; y <= height - VelikostPole; y++)
                {
                    if (pole[x, y] == Obsah.zed)
                    {
                        Rectangle rec = new Rectangle(new Point(x, y), new Size(VelikostPole,VelikostPole));
                        g.FillRectangle(new SolidBrush(Color.Black), rec);

                    }

                    /*   if (pole[x, y] == Obsah.zaklad)
                       {
                           Rectangle rec = new Rectangle(new Point(x, y), new Size(velikostPole, velikostPole));
                           g.FillEllipse(new SolidBrush(Color.Red), rec);
                       } */

                    if (pole[x, y] == Obsah.start)
                    {
                        Rectangle rec = new Rectangle(new Point(x, y), new Size(VelikostPole, VelikostPole));
                        g.FillEllipse(new SolidBrush(Color.Green), rec);
                    }
                    if (pole[x, y] == Obsah.end)
                    {
                        Rectangle rec = new Rectangle(new Point(x, y), new Size(VelikostPole, VelikostPole));
                        g.FillEllipse(new SolidBrush(Color.Red), rec);
                    }

                } //Vykreslí příslušné tvary
            }
        }
    }
}
