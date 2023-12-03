using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Czytelnik : Osoba
    {
        public Czytelnik(string imie, string nazwisko) : base(imie, nazwisko)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;           
        }
    }
}
