using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace SB
{
    public partial class Main : Form
    {
        private Graphics wndGraphics, backGraphics;
        private Bitmap backBmp;
        private static Trend trend = new Trend();
        private static int steps;
        private static Map[] map = new Map[CST.kW * CST.kH];
        private enum species { Tree, Giraffe, Lion } // New: Derived Class

        public Main()
        {
            InitializeComponent();
            trend.Size = new Size(CST.kViewW, CST.kViewH / 3);
            trend.Location = new Point(0, CST.kViewH + 70);
            foreach (string name in Enum.GetNames(typeof(species)))
            {
                trend.chart.Series.Add(name);
                trend.chart.Legends.Add(new Legend("Legend_" + name));
                trend.chart.Series[name].BorderWidth = 3;
                trend.chart.Series[name].ChartType = SeriesChartType.Line;
                trend.chart.Series[name].Name = name;
            }
            // NEW: Color
            trend.chart.Series[0].Color = Color.Green;
            trend.chart.Series[1].Color = Color.Yellow;
            trend.chart.Series[2].Color = Color.Brown;
            trend.Show();
            this.Size = new Size(CST.kViewW + 50, CST.kViewH + 70);
            Random rnd = new Random();
            wndGraphics = CreateGraphics();
            backBmp = new Bitmap(CST.kViewW + 20, CST.kViewH + 20);
            backGraphics = Graphics.FromImage(backBmp);
            for (int v = 0; v < CST.kH * CST.kW; ++v)
            {
                map[v] = new Map();
                int i = v % CST.kW, j = v / CST.kW;
                XmlDocument xml = new XmlDocument();
                xml.Load("SB.xml");
                // NEW: Derived class
                int brate = Convert.ToInt32(xml.SelectSingleNode("/Dot/creatures/plants/tree/@ibr").Value);
                if (rnd.Next(10001) <= brate) { map[v].creature = new Tree(i, j, rnd.Next(2) == 1); continue; }
                Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@ibr").Value, out brate);
                if (rnd.Next(10001) <= brate) { map[v].creature = new Giraffe(i, j, rnd.Next(2) == 1); continue; }
                Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@ibr").Value, out brate);
                if (rnd.Next(10001) <= brate) { map[v].creature = new Lion(i, j, rnd.Next(2) == 1); continue; }
            }
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int[] values = Enumerable.Range(0, CST.kW * CST.kH - 1).OrderBy(n => rnd.Next()).ToArray();
            foreach (int v in values)
            {
                if (map[v].creature != null)
                {
                    if (map[v].creature.Dead()) { map[v].creature = null; continue; }
                    Type type = map[v].creature.GetType().BaseType;
                    // NEW: Base class
                    if (type == typeof(Plants))
                    {
                        Plants plant = (Plants)map[v].creature;
                        plant.Grow();
                        plant.Reproduce(ref map);
                    }
                    else if (type == typeof(Animals))
                    {
                        Animals animal = (Animals)map[v].creature;
                        animal.Move(ref map);
                    }
                }
            }
            DrawGame();
        }

        private void DrawGame()
        {
            backGraphics.FillRectangle(Brushes.Ivory, 0, 0, CST.kViewW + 20, CST.kViewH + 20);
            Dictionary<string, int> cnt = new Dictionary<string, int>() { { "Map", 0 } };
            foreach (string name in Enum.GetNames(typeof(species))) cnt.Add(name, 0);
            for (int v = 0; v < CST.kW * CST.kH; ++v)
                if (map[v].creature != null)
                {
                    int i = v / CST.kW, j = v % CST.kW;
                    backGraphics.FillRectangle(map[v].creature.Colour(), j * CST.kSize, i * CST.kSize, CST.kSize, CST.kSize);
                    ++cnt[map[v].creature.GetType().Name];
                }
            int t = 0;
            foreach (string name in Enum.GetNames(typeof(species)))
            {
                backGraphics.DrawString("Number of " + name + "s: " + cnt[name], new Font("Arial", 12.0f), Brushes.Black, 200 * (t++), CST.kViewH);
                trend.chart.Series[name].Points.AddXY(steps, cnt[name]);
            }
            backGraphics.DrawString("The world has spawned: " + steps++ + " years", new Font("Arial", 12.0f), Brushes.Black, 200 * t, CST.kViewH);
            wndGraphics.DrawImageUnscaled(backBmp, 0, 0);
            trend.Refresh();
        }
    }

    public static class CST
    {
        public const int kViewW = 1440;
        public const int kViewH = (900 - 110) * 3 / 4;
        public const int kW = kViewW / kSize;
        public const int kH = kViewH / kSize;
        public const int kSize = 10;
    }
}
