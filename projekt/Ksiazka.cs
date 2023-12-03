using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Ksiazka
    {
        public string tytul;
        public Autor autor;

        public Ksiazka(string tytul, Autor autor)
        {
            this.tytul = tytul;
            this.autor = autor;
        }
    }
}
