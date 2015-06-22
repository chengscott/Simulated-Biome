using System;
using System.Drawing;
using System.Xml;

namespace SB
{
    sealed class Tree : Plants
    {
        public Tree() {}
        public Tree(int x, int y, bool sex)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("SB.xml");
            // br_ = 2200; span_ = 60; cap_ = 30; appt_ = 10; full_ = 150; rspan_ = 5; rcap_ = 5; radius_ = 1;
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@br").Value, out br_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@span").Value, out span_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@cap").Value, out cap_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@appt").Value, out appt_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@full").Value, out full_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@rspan").Value, out rspan_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@rcap").Value, out rcap_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/plants/tree/@radius").Value, out radius_);
            x_ = x; y_ = y; sex_ = sex;
            brush_.Add(Brushes.YellowGreen);
            brush_.Add(Brushes.LimeGreen);
            brush_.Add(Brushes.Green);
            brush_.Add(Brushes.DarkGreen);
        }
    }

    sealed class Giraffe : Animals
    {
        public Giraffe() {}
        public Giraffe(int x, int y, bool sex)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("SB.xml");
            // br_ = 1100; span_ = 22; cap_ = 5; appt_ = 40; full_ = 120; rspan_ = 5; rcap_ = 35; mr_ = 15; vore_ = vore.herbi_;
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@br").Value, out br_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@span").Value, out span_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@cap").Value, out cap_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@appt").Value, out appt_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@full").Value, out full_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@rspan").Value, out rspan_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@rcap").Value, out rcap_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@radius").Value, out radius_);
            Enum.TryParse<vore>(xml.SelectSingleNode("/Dot/creatures/animals/giraffe/@vore").Value, out vore_);
            x_ = x; y_ = y; sex_ = sex;
            brush_.Add(Brushes.Yellow);
            brush_.Add(Brushes.Gold);
            brush_.Add(Brushes.Goldenrod);
            brush_.Add(Brushes.Peru);
        }
    }

    sealed class Lion : Animals
    {
        public Lion() { }
        public Lion(int x, int y, bool sex)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("SB.xml");
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@br").Value, out br_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@span").Value, out span_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@cap").Value, out cap_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@appt").Value, out appt_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@full").Value, out full_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@rspan").Value, out rspan_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@rcap").Value, out rcap_);
            Int32.TryParse(xml.SelectSingleNode("/Dot/creatures/animals/lion/@radius").Value, out radius_);
            Enum.TryParse<vore>(xml.SelectSingleNode("/Dot/creatures/animals/lion/@vore").Value, out vore_);
            x_ = x; y_ = y; sex_ = sex;
            brush_.Add(Brushes.SaddleBrown);
            brush_.Add(Brushes.Firebrick);
            brush_.Add(Brushes.Brown);
            brush_.Add(Brushes.Maroon);
        }
    }
}
