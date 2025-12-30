using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalverwaltung
{
    internal class Mitarbeiter
    {
       
        public string Name { get; set; }
        public string Nachname { get; set; }
        public string Funktion { get; set; }
        public int Urlaubstage { get; set; } = 26;
        public List<DateOnly> Krankheitstage { get; set; }
        
        
        public Mitarbeiter(string name, string nachname, string funktion)
        {
            Name = name;
            Nachname = nachname;
            Funktion = funktion;
            Krankheitstage = new List<DateOnly>();
        }
       
    }
}
