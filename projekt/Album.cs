using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Album : Ksiazka
    {
        public string kartki;

        public Album(string kartki,string tytul, Autor autor) : base(tytul, autor)
        {
            this.kartki = kartki;
            this.tytul = tytul;
            this.autor = autor;
        }
    }
}
