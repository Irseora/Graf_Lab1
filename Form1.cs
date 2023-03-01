using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen p;
        int n = 0; //nr de noduri
        ArrayList noduri = new ArrayList(); //nodurile grafului
        bool gata_noduri = false;
        int arc1 = -1, arc2 = -1; //retine extremitatile unui arc (muchie)
        int[,] a; //matricea de adiacenta
        float raza = 20;

        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        //distanta intre doua puncte
        private double dist(PointF x, PointF y)
        {
            return Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));
        }

        //gaseste un nod al grafului
        private int find(PointF x)
        {
            for (int i = 0; i < n; i++)
                if (dist(x, (PointF)noduri[i]) <= raza)
                    return i;
            return -1;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            p = new Pen(Color.Black, 3);
            PointF aux = this.PointToClient(new Point(Form1.MousePosition.X,
            Form1.MousePosition.Y));
            if (gata_noduri == false)
            {
                n++;
                noduri.Add(aux);
                g.DrawEllipse(p, ((PointF)noduri[n - 1]).X - raza / 2, ((PointF)noduri[n -
                1]).Y - raza / 2, raza, raza);
                PointF x = new PointF();
                x.X = ((PointF)noduri[n - 1]).X - raza / 3;
                x.Y = ((PointF)noduri[n - 1]).Y - raza / 3;
                g.DrawString(n.ToString(), new Font(FontFamily.GenericSansSerif, 10), new
                SolidBrush(Color.Black), x);
            }
            else
            {
                int poz = find(aux);
                if (poz == -1)
                    return;
                if (arc1 == -1)
                {
                    arc1 = poz;
                }
                else
                {
                    arc2 = poz;
                    a[arc1, arc2] = 1;

                    //graf neorientat
                    if (this.radioButton2.Checked == true)
                    {
                        a[arc2, arc1] = 1;
                        p.EndCap = LineCap.NoAnchor;
                    }
                    else
                        p.EndCap = LineCap.ArrowAnchor;
                    //bucla
                    if (arc1 == arc2)
                        g.DrawArc(p, new Rectangle((int)((PointF)noduri[arc1]).X,
                        (int)(((PointF)noduri[arc1]).Y - raza), (int)raza, (int)raza), 180, 270);
                    else
                        g.DrawLine(p, ((PointF)noduri[arc1]).X + raza / 2,
                        ((PointF)noduri[arc1]).Y, ((PointF)noduri[arc2]).X + raza / 2,
                        ((PointF)noduri[arc2]).Y);
                    arc1 = arc2 = -1;

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (n == 0)
                this.label2.Text = "Indicati nodurile grafului!";
            else
            {
                gata_noduri = true;
                a = new int[n, n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        a[i, j] = 0;
                this.label2.Text = "Indicati arcele / muchiile!";
            }
        }
    }
}
