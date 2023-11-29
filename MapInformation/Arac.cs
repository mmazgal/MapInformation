using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapInformation
{
    public class Arac
    {
        private string Plaka;
        private string Tipi;
        private string From;
        private string To;
        private PointLatLng Konum;

        public Arac(string plaka, string tipi, string from, string to, PointLatLng konum)
        {
            Plaka = plaka;
            Tipi = tipi;
            From = from;
            To = to;
            Konum = konum;
        }

        public string Plaka1 { get => Plaka; set => Plaka = value; }
        public string Tipi1 { get => Tipi; set => Tipi = value; }
        public string From1 { get => From; set => From = value; }
        public string To1 { get => To; set => To = value; }

        public PointLatLng Konum1 { get => Konum; set => Konum = value; }

        public override string ToString()
        {
            string str ="\n" + Plaka + "\n" + Tipi + "\n" + From + "\n" + To;
            return str;
        }
    }
}
