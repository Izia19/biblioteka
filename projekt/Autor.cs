using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Autor : Osoba
    {
        public Autor(string imie, string nazwisko) : base(imie, nazwisko)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
        }
    }
}
