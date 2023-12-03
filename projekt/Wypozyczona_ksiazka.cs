using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Wypozyczona_ksiazka
    {
        public Ksiazka ksiazka;
        public Czytelnik czytelnik;
        public DateTime Data_wypo;
        public DateTime Data_zwro;

        public Wypozyczona_ksiazka(Ksiazka ksiazka, Czytelnik czytelnik, DateTime data_wypo, DateTime data_zwro)
        {
            this.ksiazka = ksiazka;
            this.czytelnik = czytelnik;
            Data_wypo = data_wypo;
            Data_zwro = data_zwro;
        }
    }
}
    