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
    public partial class Graf_Laborator : Form
    {
        Graphics g;
        Pen p;
        int n = 0; // Numar de noduri
        ArrayList noduri = new ArrayList(); // Nodurile grafului
        bool gata_noduri = false;
        int arc1 = -1, arc2 = -1; // Eetine extremitatile unui arc (muchie)
        int[,] a; // Matricea de adiacenta
        float raza = 20;

        public Graf_Laborator()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        // Distanta intre doua puncte
        private double dist(PointF x, PointF y)
        {
            return Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));
        }

        // Gaseste un nod al grafului
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
            PointF aux = this.PointToClient(new Point(Graf_Laborator.MousePosition.X, Graf_Laborator.MousePosition.Y));
            if (gata_noduri == false)
            {
                n++;
                noduri.Add(aux);
                g.DrawEllipse(p, ((PointF)noduri[n - 1]).X - raza / 2, ((PointF)noduri[n - 1]).Y - raza / 2, raza, raza);
                PointF x = new PointF();
                x.X = ((PointF)noduri[n - 1]).X - raza / 3;
                x.Y = ((PointF)noduri[n - 1]).Y - raza / 3;
                g.DrawString(n.ToString(), new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(Color.Black), x);
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

                    // Graf neorientat
                    if (this.radioButton2.Checked == true)
                    {
                        a[arc2, arc1] = 1;
                        p.EndCap = LineCap.NoAnchor;
                    }
                    else
                        p.EndCap = LineCap.ArrowAnchor;
                    // Bucla
                    if (arc1 == arc2)
                        g.DrawArc(p, new Rectangle((int)((PointF)noduri[arc1]).X, (int)(((PointF)noduri[arc1]).Y - raza), (int)raza, (int)raza), 180, 270);
                    else
                        g.DrawLine(p, ((PointF)noduri[arc1]).X + raza / 2, ((PointF)noduri[arc1]).Y, ((PointF)noduri[arc2]).X + raza / 2, ((PointF)noduri[arc2]).Y);

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

        // LAB 1
        // ----------------------------------------------------------------------
        // LAB 2

        int[] mark;
        int prim, ultim;
        string s = "";

        // Backtracking - Init
        void init(int[] st, int k)
        {
            st[k] = -1;
        }

        // Backtracking - Succesor
        bool succesor(int[] st, int k)
        {
            for (int i = st[k] + 1; i < n; i++)
                if (mark[i] == k)
                {
                    st[k] = i;
                    return true;
                }
            return false;
        }

        // Backtracking - Valid
        bool valid(int[] st, int k)
        {
            if ((k == 0) || (k > 0 && a[st[k - 1], st[k]] == 1))
                return true;
            return false;
        }

        // Backtracking - Solutie
        bool solutie(int k, int n)
        {
            if (k == n)
                return true;
            return false;
        }

        // Backtracking - Afisare solutie
        void afisare(int[] st, int k)
        {
            if (!(st[0] == prim && st[k - 1] == ultim))
                return;
            if (s != "")
                s += ";";
            for (int i = 0; i < k; i++)
                s += " " + (st[i] + 1).ToString();
            this.label6.Text = s;
            p = new Pen(Color.Red, 1);
            p.DashStyle = DashStyle.Dot;
            for (int i = 0; i < k - 1; i++)
                g.DrawLine(p, (PointF)noduri[st[i]], (PointF)noduri[st[i + 1]]);
        }

        // Backtracking - Rutina recursiva
        void bt(int n, int[] st, int k)
        {
            if (solutie(k, n))
                afisare(st, k);
            else
            {
                init(st, k);
                while (succesor(st, k))
                    if (valid(st, k))
                        bt(n, st, k + 1);
            }
        }

        // Determinare drum minim
        private void button1_Click(object sender, EventArgs e)
        {
            prim = Convert.ToInt32(this.textBox1.Text) - 1;
            ultim = Convert.ToInt32(this.textBox2.Text) - 1;
            if (prim == ultim)
            {
                if (a[prim, ultim] == 1)
                    this.label5.Text = "Drumul minim de la varful " + (prim + 1).ToString() + " la varful " + (ultim + 1).ToString() + " este de lungime 1.";
                else
                    this.label5.Text = "Nu exista drum de la varful " + (prim + 1).ToString() + "la varful " + (ultim + 1).ToString() + "!";
            }
            else
            {
                // Marcare noduri
                mark = new int[n];
                for (int i = 0; i < n; i++)
                    mark[i] = 0;
                for (int i = 0; i < n; i++)
                    if (a[prim, i] == 1)
                        mark[i] = 1;
                int k = 2;
                while (k < n)
                {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (mark[i] == k - 1 && a[i, j] == 1 && (mark[j] == 0 || mark[j] > k))
                                mark[j] = k;
                    k++;
                }

                if (mark[ultim] != 0)
                {
                    this.label5.Text = "Drumul minim de la varful " + (prim + 1).ToString() + " la varful " + (ultim + 1).ToString() + " este de lungime " + mark[ultim] + ".";
                    // Reconstituire drum
                    int[] st = new int[mark[ultim] + 1];
                    bt(mark[ultim] + 1, st, 0);
                }
                else
                    this.label5.Text = "Nu exista drum de la varful " + (prim + 1).ToString() + "la varful " + (ultim + 1).ToString() + "!";
            }
        }
    }
}
