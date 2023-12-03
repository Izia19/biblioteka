using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using MySqlX.XDevAPI.Relational;

namespace projekt
{
    public class Biblioteka
    {
        public string connectionString = "server=localhost;user id=root;password=;database=biblioteka";
        public MySqlConnection polaczenie;

        public List<Ksiazka> Lista_Ksiazek = new List<Ksiazka>();
        public List<Czytelnik> Lista_Czytelnikow = new List<Czytelnik>();
        public List<Autor> Lista_Autorow = new List<Autor>();
        public List<Wypozyczona_ksiazka> Lista_biblioteka = new List<Wypozyczona_ksiazka>();
        public List<Album> Lista_Albumow = new List<Album>();

        public Biblioteka()
        {
            try
            {
                polaczenie = new MySqlConnection(connectionString);
                polaczenie.Open();

                Console.WriteLine("Połączono z bazą danych.");

                //uzupełnienie listy ksiazek
                string kwerenda = "SELECT ksiazka.Tytul, autor.ID_osoby, osoba.Imie, osoba.Nazwisko " +
                    "FROM ksiazka " +
                    "JOIN autor ON ksiazka.ID_autora = autor.ID " +
                    "JOIN osoba ON autor.ID_osoby = osoba.ID";
                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Autor autor = new Autor(reader["Imie"].ToString(), reader["Nazwisko"].ToString());

                        Ksiazka ksiazka = new Ksiazka(reader["Tytul"].ToString(), autor);
                        Lista_Ksiazek.Add(ksiazka);
                    }
                    reader.Close();
                }

                //uzupełnienie listy czytelników
                string kwerenda2 = "SELECT osoba.Imie, osoba.Nazwisko " +
                    "FROM czytelnik " +
                    "JOIN osoba ON czytelnik.ID_osoby = osoba.ID";
                MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                using (MySqlDataReader reader2 = cmd2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        Czytelnik czytelnik = new Czytelnik(reader2["Imie"].ToString(), reader2["Nazwisko"].ToString());

                        Lista_Czytelnikow.Add(czytelnik);
                    }
                    reader2.Close();
                }

                //uzupełnienie listy autorów
                string kwerenda3 = "SELECT osoba.Imie, osoba.Nazwisko " +
                    "FROM autor " +
                    "JOIN osoba ON autor.ID_osoby = osoba.ID";
                MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie);

                using (MySqlDataReader reader3 = cmd3.ExecuteReader())
                {
                    while (reader3.Read())
                    {
                        Autor autor = new Autor(reader3["Imie"].ToString(), reader3["Nazwisko"].ToString());

                        Lista_Autorow.Add(autor);
                    }
                    reader3.Close();
                }

                //uzupełnienie listy biblioteka
                string kwerenda4 = "SELECT ksiazka.Tytul, oa.Imie AS AutorImie, oa.Nazwisko AS AutorNazwisko, oc.Imie AS CzytelnikImie, oc.Nazwisko AS CzytelnikNazwisko, biblioteka.Data_wypo, biblioteka.Data_zwro " +
                    "FROM biblioteka " +
                    "JOIN ksiazka ON biblioteka.ID_ksiazki = ksiazka.ID " +
                    "JOIN autor ON ksiazka.ID_autora = autor.ID " +
                    "JOIN osoba AS oa ON autor.ID_osoby = oa.ID " +
                    "JOIN czytelnik ON biblioteka.ID_czytelnika = czytelnik.ID " +
                    "JOIN osoba AS oc ON czytelnik.ID_osoby = oc.ID;";
                MySqlCommand cmd4 = new MySqlCommand(kwerenda4, polaczenie);

                using (MySqlDataReader reader4 = cmd4.ExecuteReader())
                {
                    while (reader4.Read())
                    { 
                        Autor autor2 = new Autor(reader4["AutorImie"].ToString(), reader4["AutorNazwisko"].ToString());
                        Ksiazka ksiazka2 = new Ksiazka(reader4["Tytul"].ToString(), autor2);
                        Czytelnik czytelnik2 = new Czytelnik(reader4["CzytelnikImie"].ToString(), reader4["CzytelnikNazwisko"].ToString());
                        DateTime dataWypozyczenia = Convert.ToDateTime(reader4["Data_wypo"]);
                        DateTime dataZwrotu = Convert.ToDateTime(reader4["Data_zwro"]);
                        Wypozyczona_ksiazka wypozyczona_Ksiazka = new Wypozyczona_ksiazka(ksiazka2, czytelnik2, dataWypozyczenia, dataZwrotu);
                        Lista_biblioteka.Add(wypozyczona_Ksiazka);
                    }
                    reader4.Close();
                }

                //uzupelnienie listy albumow
                string kwerenda5 = "SELECT Tytul, Imie, Nazwisko, Kartki " +
                    "FROM osoba " +
                    "JOIN autor ON osoba.ID = autor.ID_osoby " +
                    "JOIN ksiazka ON autor.ID = ksiazka.ID " +
                    "JOIN album ON ksiazka.ID = album.ID_ksiazki";

                MySqlCommand cmd5 = new MySqlCommand(kwerenda5, polaczenie);

                using (MySqlDataReader reader5 = cmd5.ExecuteReader())
                {
                    while (reader5.Read())
                    {
                        Autor autor = new Autor(reader5["Imie"].ToString(), reader5["Nazwisko"].ToString());
                        Album album = new Album(reader5["Kartki"].ToString(), reader5["Tytul"].ToString(), autor);
                        Lista_Albumow.Add(album);
                    }
                    reader5.Close();
                }
                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } 


        //ksiazki
        public void Dodaj_ksiazke(string imie, string nazwisko, string tytul)
        {
            foreach (Ksiazka k in Lista_Ksiazek)
            {
                if (k.tytul == tytul) //sprawdza czy jest taka ksiazka
                {
                    Console.WriteLine("Taka ksiazka juz istnieje");
                    return; // przerwij działanie funkcji
                }
            }
            try
            {
                Autor autor = new Autor(imie, nazwisko);
                Ksiazka ksiazka = new Ksiazka(tytul, autor);
                Lista_Ksiazek.Add(ksiazka);

                polaczenie.Open();
                int ile_ID = 0;

                //select sprawdzajacy czy dany autor juz istnieje
                string kwerenda = "SELECT COUNT(osoba.ID) FROM osoba INNER JOIN autor ON autor.ID_osoby = osoba.ID WHERE Imie = @imie AND Nazwisko = @nazwisko";
                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie); //wywolanie kwerendy

                cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                cmd.Parameters.AddWithValue("@nazwisko", nazwisko);

                ile_ID = Convert.ToInt32(cmd.ExecuteScalar());


                //jesli istnieje to:
                if (ile_ID == 1)
                {
                    int ID_autora = 0; //ID autora o poadanym imieniu i nazwisku 

                    //
                    string kwerenda2 = "SELECT autor.ID FROM osoba JOIN autor ON autor.ID_osoby = osoba.ID WHERE Imie = @imie AND Nazwisko = @nazwisko";
                    MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie); //wywolanie kwerendy

                    cmd2.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                    cmd2.Parameters.AddWithValue("@nazwisko", nazwisko);

                    ID_autora = Convert.ToInt32(cmd2.ExecuteScalar());
                  

                    string kwerenda3 = "INSERT INTO ksiazka (Tytul,ID_autora) VALUES (@tytul,@ID_autora)";
                    MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie); //wywolanie kwerendy

                    cmd3.Parameters.AddWithValue("@tytul", tytul);  //przypisanie zmiennych 
                    cmd3.Parameters.AddWithValue("@ID_autora", ID_autora);
                    cmd3.ExecuteNonQuery();
                }
                else //jesli nie istnieje 
                {
                    Console.WriteLine("Pomyślnie dodano nowego autora");
                    Lista_Autorow.Add(autor);

                    int ID_osoby = 0; //ID osoby o poadanym imieniu i nazwisku 
                    int ID_autora = 0; //ID autora o poadanym imieniu i nazwisku 

                    //dodawanie nowej osoby
                    string kwerenda1 = "INSERT INTO osoba (Imie,Nazwisko) VALUES (@imie, @nazwisko)";
                    MySqlCommand cmd1 = new MySqlCommand(kwerenda1, polaczenie); //wywolanie kwerendy

                    cmd1.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                    cmd1.Parameters.AddWithValue("@nazwisko", nazwisko);
                    cmd1.ExecuteNonQuery();

                    //wyszukiwanie ID dodanej osoby
                    string kwerenda2 = "SELECT osoba.ID FROM osoba WHERE Imie = @imie AND Nazwisko = @nazwisko ";
                    MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie); //wywolanie kwerendy

                    cmd2.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                    cmd2.Parameters.AddWithValue("@nazwisko", nazwisko);

                    ID_osoby = Convert.ToInt32(cmd2.ExecuteScalar());

                    //dodawanie autora
                    string kwerenda3 = "INSERT INTO autor (ID_osoby) VALUES (@ID_osoby)";
                    MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie); //wywolanie kwerendy

                    cmd3.Parameters.AddWithValue("@ID_osoby", ID_osoby);
                    cmd3.ExecuteNonQuery();


                    //wyszukiwanie ID dodanego autora
                    string kwerenda4 = "SELECT ID FROM autor WHERE autor.ID_osoby = @ID_osoby";
                    MySqlCommand cmd4 = new MySqlCommand(kwerenda4, polaczenie); //wywolanie kwerendy

                    cmd4.Parameters.AddWithValue("@ID_osoby", ID_osoby);

                    ID_autora = Convert.ToInt32(cmd4.ExecuteScalar());

                    //dodawanie ksiazki
                    string kwerenda5 = "INSERT INTO ksiazka (Tytul,ID_autora) VALUES (@tytul,@ID_autora)";
                    MySqlCommand cmd5 = new MySqlCommand(kwerenda5, polaczenie); //wywolanie kwerendy

                    cmd5.Parameters.AddWithValue("@tytul", tytul);  //przypisanie zmiennych 
                    cmd5.Parameters.AddWithValue("@ID_autora", ID_autora);
                    cmd5.ExecuteNonQuery();
                }

                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                polaczenie.Close();
            }
        } //dziala

        public void Wyswietl_ksiazki()
        {
            try
            {
                polaczenie.Open();

                int dostepnosc = 0;
                Console.WriteLine("Lista wszystkich książek w bibliotece: ");
                Console.WriteLine("+--------------------------------------------+");
                foreach (Ksiazka k in Lista_Ksiazek)
                {
                    string tytul = k.tytul;
                    string kwerenda = "SELECT COUNT(biblioteka.ID) FROM biblioteka JOIN ksiazka ON biblioteka.ID_ksiazki = ksiazka.ID WHERE Tytul = @tytul";
                    MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie); //wywolanie kwarendy

                    cmd.Parameters.AddWithValue("@tytul", tytul); //przypisanie zmiennych 
                    dostepnosc = Convert.ToInt32(cmd.ExecuteScalar());

                    if (dostepnosc == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Tytuł: {0} ", k.tytul);
                        Console.WriteLine("Autor: {0} {1}", k.autor.imie, k.autor.nazwisko);
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.WriteLine("+--------------------------------------------+");
                    }
                    else
                    {
                        Console.WriteLine("Tytuł: {0} ", k.tytul);
                        Console.WriteLine("Autor: {0} {1}", k.autor.imie, k.autor.nazwisko);
                        Console.WriteLine("+--------------------------------------------+");
                    }
                    Thread.Sleep(50);

                }

                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        } //działa

        public void Edytuj_ksiazke(string stary_tytul, string nowy_tytul)
        {
            int index = Lista_Ksiazek.FindIndex(p => p.tytul == stary_tytul); //pobiera indeks ksiazki o starym tytule 
            if (index != -1)
            {
                Ksiazka ksiazka_o_indeksie = Lista_Ksiazek[index]; //pobiera kisazke o podanym indeksie
                string imie = ksiazka_o_indeksie.autor.imie; //pobiera konkretne wartosci z tej pobranej wczesniej ksiazki 
                string nazwisko = ksiazka_o_indeksie.autor.nazwisko;

                Autor autor = new Autor(imie, nazwisko);
                Ksiazka ksiazka = new Ksiazka(nowy_tytul, autor);
                Lista_Ksiazek[index] = ksiazka;
                try
                {
                    polaczenie.Open();

                    string kwerenda = "UPDATE ksiazka SET Tytul = @nowy_tytul  WHERE Tytul = @stary_tytul";
                    MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie); //wywolanie kwarendy

                    cmd.Parameters.AddWithValue("@nowy_tytul", nowy_tytul); //przypisanie zmiennych 
                    cmd.Parameters.AddWithValue("@stary_tytul", stary_tytul);
                    cmd.ExecuteNonQuery();

                    polaczenie.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    polaczenie.Close();
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono ksiazki o tytule: " + stary_tytul);
            }
        } //dziala

        public void Usun_ksiazke(string tytul)
        {
            int index = Lista_Ksiazek.FindIndex(p => p.tytul == tytul);
            if (index != -1)
            {
                Lista_Ksiazek.RemoveAt(index);
                try
                {
                    polaczenie.Open();

                    string kwerenda = "DELETE FROM ksiazka WHERE Tytul = @tytul";
                    MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie); //wywolanie kwarendy

                    cmd.Parameters.AddWithValue("@tytul", tytul); //przypisanie zmiennych 
                    cmd.ExecuteNonQuery();

                    polaczenie.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    polaczenie.Close();
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono ksiazki o tytule: " + tytul);
            }
        } //dziala

        public void Wypozycz_ksiazke(string imie, string nazwisko, string tytul)
        {
            try
            {
                string imie_autora;
                string nazwisko_autora;
                bool jest_czytelnik = false;
                bool jest_ksiazka = false;

                foreach (Wypozyczona_ksiazka w in Lista_biblioteka)
                {
                    if (w.ksiazka.tytul == tytul) //sprawdza czy na liscie wypozyczonych ksiazek znajduje sie ten tytul 
                    {
                        Console.WriteLine("Ta ksiazka jest juz wypozyczona");
                        return;
                    }
                }
                foreach(Czytelnik c in Lista_Czytelnikow)
                {
                    if(c.imie == imie && c.nazwisko == nazwisko)
                    {
                        jest_czytelnik = true;
                        foreach (Ksiazka k in Lista_Ksiazek)
                        {
                            if (k.tytul == tytul) //sprawdza czy na liscie znajduje sie taki tytul
                            {
                                jest_ksiazka = true;
                                imie_autora = k.autor.imie;
                                nazwisko_autora = k.autor.nazwisko; //podpisuje imie i nazwisko autora podanej ksiazki

                                Autor autor = new Autor(imie_autora, nazwisko_autora);
                                Ksiazka ksiazka = new Ksiazka(tytul, autor);
                                Czytelnik czytelnik = new Czytelnik(imie, nazwisko);

                                DateTime data_wypo = DateTime.Today;

                                DateTime data_zwro = data_wypo.AddDays(14);

                                Wypozyczona_ksiazka wypozyczona_Ksiazka = new Wypozyczona_ksiazka(ksiazka, czytelnik, data_wypo, data_zwro);

                                polaczenie.Open();

                                int ID_czytelnika = 0;
                                int ID_ksiazki = 0;

                                //wyszukiwanie ID osoby o podanym imieniu i nazwisku
                                string kwerenda = "SELECT czytelnik.ID FROM osoba JOIN czytelnik ON Imie = @imie AND Nazwisko = @nazwisko AND czytelnik.ID_osoby = osoba.ID";
                                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie); //wywolanie kwerendy

                                cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                                cmd.Parameters.AddWithValue("@nazwisko", nazwisko);

                                ID_czytelnika = Convert.ToInt32(cmd.ExecuteScalar());

                                //wyszukiwanie ID ksiazki o podanym tytule
                                string kwerenda3 = "SELECT ksiazka.ID FROM ksiazka WHERE Tytul = @tytul";
                                MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie); //wywolanie kwerendy

                                cmd3.Parameters.AddWithValue("@tytul", tytul);  //przypisanie zmiennych 

                                ID_ksiazki = Convert.ToInt32(cmd3.ExecuteScalar());

                                string data_wypo2 = data_wypo.ToString("yyyy-MM-dd");
                                string data_zwro2 = data_zwro.ToString("yyyy-MM-dd");

                                //dodaje do biblioteki wypozyczona ksiazke
                                string kwerenda4 = "INSERT INTO biblioteka (ID_ksiazki, ID_czytelnika, Data_wypo, Data_zwro) VALUES (@ID_ksiazki, @ID_czytelnika, @data_wypo, @data_zwro)";
                                MySqlCommand cmd4 = new MySqlCommand(kwerenda4, polaczenie); //wywolanie kwerendy

                                cmd4.Parameters.AddWithValue("@ID_ksiazki", ID_ksiazki);  //przypisanie zmiennych 
                                cmd4.Parameters.AddWithValue("@ID_czytelnika", ID_czytelnika);
                                cmd4.Parameters.AddWithValue("@data_wypo", data_wypo);
                                cmd4.Parameters.AddWithValue("@data_zwro", data_zwro);

                                cmd4.ExecuteNonQuery();
                                Wypozyczona_ksiazka wypo_ks = new Wypozyczona_ksiazka(ksiazka, czytelnik, data_wypo, data_zwro);
                                Lista_biblioteka.Add(wypo_ks);
                                polaczenie.Close();
                            }
                        }
                    }
                }
                if (jest_czytelnik == false)
                {
                    Console.WriteLine("Nie znaleziono czytelnika o podanych imieniu i nazwisku");
                }
                if (jest_ksiazka == false)
                {
                    Console.WriteLine("Nie znaleziono ksiazki o podanym tytule");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                polaczenie.Close();
            }
        } //dziala

        public void Wyswietl_ksiazki_czytelnika(string imie, string nazwisko) //działa
        {
            int index = Lista_biblioteka.FindIndex(p => p.czytelnik.imie == imie && p.czytelnik.nazwisko == nazwisko);
            if (index != -1)
            {
                Console.WriteLine("\n+--------------------------------------------+");
                Console.WriteLine("Czytelnik: {0} {1} ", imie, nazwisko);
                Console.WriteLine("Lista wypożyczonych książek: ");
                Console.WriteLine("+--------------------------------------------+");

                foreach (Wypozyczona_ksiazka w in Lista_biblioteka)
                {
                    if (w.czytelnik.imie == imie && w.czytelnik.nazwisko == nazwisko)
                    {
                        Console.WriteLine("Tytul: {0} ", w.ksiazka.tytul);
                        Console.WriteLine("Autor: {0} {1}", w.ksiazka.autor.imie, w.ksiazka.autor.nazwisko);
                        Console.WriteLine("Data zwrotu: {0}", w.Data_zwro.ToString("dd-MM-yyyy"));
                        Console.WriteLine("+--------------------------------------------+");
                    }
                }
            }
            else
            {
                Console.WriteLine("Ten czytelnik nie ma wypozyczonych ksiazek lub nie istnieje");
            }         
        }

        public void Oddaj_ksiazke(string tytul)
        {
            for (int i = 0; i < Lista_biblioteka.Count; i++)
            {
                if (Lista_biblioteka[i].ksiazka.tytul.Equals(tytul, StringComparison.OrdinalIgnoreCase))
                {
                    Lista_biblioteka.RemoveAt(i);
                    Console.WriteLine("Oddano książkę o tytule: {0}", tytul);
                    break;
                }
            }
            try
            {
                polaczenie.Open();

                int ID = 0;

                //wyszukiwanie ID wypozyczenia dla ksiazki o podanym ID
                string kwerenda = "SELECT biblioteka.ID FROM biblioteka JOIN ksiazka ON biblioteka.ID_ksiazki = ksiazka.ID WHERE Tytul = @tytul";
                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie); //wywolanie kwerendy

                cmd.Parameters.AddWithValue("@tytul", tytul);  //przypisanie zmiennych 
                ID = Convert.ToInt32(cmd.ExecuteScalar());

                if (ID != 0)
                {
                    //usuwa wypozyczenie ksiazki o podanym ID
                    string kwerenda2 = "DELETE FROM biblioteka WHERE ID = @ID";
                    MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie); //wywolanie kwerendy

                    cmd2.Parameters.AddWithValue("@ID", ID);  //przypisanie zmiennych
                    cmd2.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine("Ta ksiazka nie jest wypozyczona lub nie ma takiej ksiazki");
                }
                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                polaczenie.Close();
            }
        } //działa


        //autorzy
        public void Wyswietl_autorow() //dziala
        {
            Console.WriteLine("Lista autorów: ");
            Console.WriteLine("+--------------------------------------------+");
            foreach (Autor a in Lista_Autorow)
            {
                Console.WriteLine("Imie: {0} ", a.imie);
                Console.WriteLine("Nazwisko: {0}", a.nazwisko);
                Console.WriteLine("+--------------------------------------------+");
                Thread.Sleep(50);
            }
        }

        public void Dodaj_autora(string imie, string nazwisko)
        {
            foreach (Autor a in Lista_Autorow)
            {
                if (a.imie == imie && a.nazwisko == nazwisko) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    Console.WriteLine("Taki autor juz istnieje");
                    return; // przerwij działanie funkcji
                }
            }
            try
            {
                polaczenie.Open();

                Autor autor = new Autor(imie, nazwisko);
                Lista_Autorow.Add(autor);

                int ID_osoby = 0;
                //dodawanie osoby 
                string kwerenda = "INSERT INTO osoba (Imie,Nazwisko) VALUES (@imie,@nazwisko)";
                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                cmd.ExecuteNonQuery();

                //wyszukiwanie ID dodanej wyzej osoby
                string kwerenda2 = "SELECT ID FROM osoba WHERE Imie = @imie AND Nazwisko = @nazwisko";
                MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                cmd2.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                cmd2.Parameters.AddWithValue("@nazwisko", nazwisko);

                ID_osoby = Convert.ToInt32(cmd2.ExecuteScalar());

                //dodawanie autora
                string kwerenda3 = "INSERT INTO autor (ID_osoby) VALUES (@ID_osoby)";
                MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie);

                cmd3.Parameters.AddWithValue("@ID_osoby", ID_osoby);  //przypisanie zmiennych
                cmd3.ExecuteNonQuery();

                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        } //dziala

        public void Edytuj_autora(string imie, string nazwisko, string nowe_imie, string nowe_nazwisko)
        {
            bool znaleziono = false;
            List<Autor> kopia_lista_autorow = new List<Autor>(Lista_Autorow); // tworzenie kopii listy
            foreach (Autor a in kopia_lista_autorow)
            {
                if (a.imie == imie && a.nazwisko == nazwisko) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    try
                    {
                        int indeks = Lista_Autorow.IndexOf(a); //sprawdza indeks elementu o takich danych 
                        znaleziono = true;

                        Autor nowy_autor = new Autor(nowe_imie, nowe_nazwisko);
                        Lista_Autorow[indeks] = nowy_autor;

                        polaczenie.Open();

                        //edytowanie starego imienia i nazwiska na nowo podane
                        string kwerenda = "UPDATE osoba SET imie = @nowe_imie, nazwisko = @nowe_nazwisko WHERE imie = @imie AND nazwisko = @nazwisko";
                        MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                        cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                        cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                        cmd.Parameters.AddWithValue("@nowe_imie", nowe_imie);
                        cmd.Parameters.AddWithValue("@nowe_nazwisko", nowe_nazwisko);
                        cmd.ExecuteNonQuery();

                        polaczenie.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (znaleziono == false) //jesli nie ma takiej osoby na liscie 
            {
                Console.WriteLine("Nie ma takiego autora");
            }
        } //dziala

        public void Usun_autora(string imie, string nazwisko)
        {

            bool znaleziono = false;
            foreach (Autor a in Lista_Autorow)
            {
                if (a.imie == imie && a.nazwisko == nazwisko) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    try
                    {
                        int indeks = Lista_Autorow.IndexOf(a); //sprawdza indeks elementu o takich danych 
                        znaleziono = true;
                        Lista_Autorow.RemoveAt(indeks); //usuwa ten element z listy

                        polaczenie.Open();

                        int ID_osoby = 0;
                        //wyszukiwanie ID osoby o podanym imieniu i nazwisku
                        string kwerenda = "SELECT ID FROM osoba WHERE Imie = @imie AND Nazwisko = @nazwisko";
                        MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                        cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                        cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                        ID_osoby = Convert.ToInt32(cmd.ExecuteScalar());

                        //usuwanie autora o pobranym ID_osoby
                        string kwerenda2 = "DELETE FROM autor WHERE ID_osoby = @ID_osoby";
                        MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                        cmd2.Parameters.AddWithValue("@ID_osoby", ID_osoby);  //przypisanie zmiennych
                        cmd2.ExecuteNonQuery();

                        //usuwanie osoby o podanym imieniu i nazwisku 
                        string kwerenda3 = "DELETE FROM osoba WHERE ID = @ID_osoby";
                        MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie);

                        cmd3.Parameters.AddWithValue("@ID_osoby", ID_osoby);  //przypisanie zmiennych
                        cmd3.ExecuteNonQuery();

                        polaczenie.Close();
                        break; // jeśli już znaleziono obiekt, można przerwać pętlę
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (znaleziono == false) //jesli nie ma takiej osoby na liscie 
            {
                Console.WriteLine("Nie ma takiego autora");
            }
        } //dziala


        //czytelnicy
        public void Wyswietl_czytelnikow()
        {
            Console.WriteLine("Lista czytelników: ");
            Console.WriteLine("+--------------------------------------------+");
            foreach (Osoba c in Lista_Czytelnikow)
            {
                Console.WriteLine("Imie: {0} ", c.imie);
                Console.WriteLine("Nazwisko: {0}", c.nazwisko);
                Console.WriteLine("+--------------------------------------------+");
                Thread.Sleep(50);
            }
        } //dziala

        public void Dodaj_czytelnika(string imie, string nazwisko) //dziala
        {
            foreach (Czytelnik c in Lista_Czytelnikow)
            {
                if (c.imie == imie && c.nazwisko == nazwisko) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    Console.WriteLine("Taki czytelnik juz istnieje");
                    return; // przerwij działanie funkcji
                }
            }
            try
            {
                Czytelnik czytelnik = new Czytelnik(imie, nazwisko);
                Lista_Czytelnikow.Add(czytelnik);

                polaczenie.Open();

                int ID_osoby = 0;
                //dodawanie osoby 
                string kwerenda = "INSERT INTO osoba (Imie,Nazwisko) VALUES (@imie,@nazwisko)";
                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                cmd.ExecuteNonQuery();

                //wyszukiwanie ID dodanej wyzej osoby
                string kwerenda2 = "SELECT ID FROM osoba WHERE Imie = @imie AND Nazwisko = @nazwisko";
                MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                cmd2.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                cmd2.Parameters.AddWithValue("@nazwisko", nazwisko);

                ID_osoby = Convert.ToInt32(cmd2.ExecuteScalar());

                //dodawanie czytelnika
                string kwerenda3 = "INSERT INTO czytelnik (ID_osoby) VALUES (@ID_osoby)";
                MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie);

                cmd3.Parameters.AddWithValue("@ID_osoby", ID_osoby);  //przypisanie zmiennych
                cmd3.ExecuteNonQuery();

                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Usun_czytelnika(string imie, string nazwisko)
        {
            bool znaleziono = false;
            foreach (Czytelnik c in Lista_Czytelnikow)
            {
                if (c.imie == imie && c.nazwisko == nazwisko) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    try
                    {
                        int indeks = Lista_Czytelnikow.IndexOf(c); //sprawdza indeks elementu o takich danych 
                        znaleziono = true;
                        Lista_Czytelnikow.RemoveAt(indeks); //usuwa ten element z listy

                        polaczenie.Open();

                        int ID_osoby = 0;
                        //wyszukiwanie ID osoby o podanym imieniu i nazwisku
                        string kwerenda = "SELECT ID FROM osoba WHERE Imie = @imie AND Nazwisko = @nazwisko";
                        MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                        cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                        cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                        ID_osoby = Convert.ToInt32(cmd.ExecuteScalar());

                        //usuwanie czytelnika o pobranym ID_osoby
                        string kwerenda2 = "DELETE FROM czytelnik WHERE ID_osoby = @ID_osoby";
                        MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                        cmd2.Parameters.AddWithValue("@ID_osoby", ID_osoby);  //przypisanie zmiennych
                        cmd2.ExecuteNonQuery();

                        //usuwanie osoby o podanym imieniu i nazwisku 
                        string kwerenda3 = "DELETE FROM osoba WHERE ID = @ID_osoby";
                        MySqlCommand cmd3 = new MySqlCommand(kwerenda3, polaczenie);

                        cmd3.Parameters.AddWithValue("@ID_osoby", ID_osoby);  //przypisanie zmiennych
                        cmd3.ExecuteNonQuery();

                        polaczenie.Close();
                        break; // jeśli już znaleziono obiekt, można przerwać pętlę
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                
            }
            if(znaleziono == false) //jesli nie ma takiej osoby na liscie 
            {
                Console.WriteLine("Nie ma takiego czytelnika");
            }
        } //dziala

        public void Edytuj_czytelnika(string imie, string nazwisko, string nowe_imie, string nowe_nazwisko)
        {
            bool znaleziono = false;
            List<Czytelnik> kopia_lista_czytelnikow = new List<Czytelnik>(Lista_Czytelnikow); // tworzenie kopii listy
            foreach (Czytelnik c in kopia_lista_czytelnikow)
            {
                if (c.imie == imie && c.nazwisko == nazwisko) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    try
                    {
                        int indeks = Lista_Czytelnikow.IndexOf(c); //sprawdza indeks elementu o takich danych 
                        znaleziono = true;

                        Czytelnik nowy_czytelnik = new Czytelnik(nowe_imie, nowe_nazwisko);
                        Lista_Czytelnikow[indeks] = nowy_czytelnik;

                        polaczenie.Open();

                        //edytowanie starego imienia i nazwiska na nowo podane
                        string kwerenda = "UPDATE osoba SET imie = @nowe_imie, nazwisko = @nowe_nazwisko WHERE imie = @imie AND nazwisko = @nazwisko";
                        MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                        cmd.Parameters.AddWithValue("@imie", imie);  //przypisanie zmiennych 
                        cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
                        cmd.Parameters.AddWithValue("@nowe_imie", nowe_imie);
                        cmd.Parameters.AddWithValue("@nowe_nazwisko", nowe_nazwisko);
                        cmd.ExecuteNonQuery();

                        polaczenie.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if(znaleziono == false) //jesli nie ma takiej osoby na liscie 
            {
                Console.WriteLine("Nie ma takiego czytelnika");
            }
        } //dziala


        //album same metody bez wywolywania

        public void Wyswietl_albumy()
        {
            Console.WriteLine("Lista albumów: ");
            Console.WriteLine("+--------------------------------------------+");
            foreach (Album a in Lista_Albumow)
            {
                Console.WriteLine("Tytyl: {0}", a.tytul);
                Console.WriteLine("Imie i nazwisko autora: {0} {1} ", a.autor.imie, a.autor.nazwisko);
                Console.WriteLine("Rodzaj kartek: {0} ", a.kartki);
                Console.WriteLine("+--------------------------------------------+");
                Thread.Sleep(50);
            }
        }

        public void Dodaj_album(string tytul, string kartki)
        {
            bool czy_znaleziono = false;
            foreach (Album a in Lista_Albumow)
            {
                if (a.tytul == tytul) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    Console.WriteLine("Taki album juz istnieje");
                    return; // przerwij działanie funkcji
                }
            }
            foreach(Ksiazka k in Lista_Ksiazek)
            {
                if(k.tytul == tytul)
                {
                    czy_znaleziono = true;
                    Autor autor = new Autor(k.autor.imie, k.autor.nazwisko);
                    Album album = new Album(kartki, tytul, autor);
                    Lista_Albumow.Add(album);
                }
            }
            if(czy_znaleziono == false)
            {
                Console.WriteLine("Taki tytul nie istnieje");
            }
            try
            {
                polaczenie.Open();

                int ID_ksiazki = 0;

                //wyszukiwanie ID ksiazki
                string kwerenda = "SELECT ID FROM ksiazka WHERE Tytul = @tytul";
                MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                cmd.Parameters.AddWithValue("@tytul", tytul);  //przypisanie zmiennych 

                ID_ksiazki = Convert.ToInt32(cmd.ExecuteScalar());


                //dodawanie albumu
                string kwerenda2 = "INSERT INTO album (ID_ksiazki, kartki) VALUES (@ID_ksiazki,@kartki)";
                MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                cmd2.Parameters.AddWithValue("@ID_ksiazki", ID_ksiazki);  //przypisanie zmiennych 
                cmd2.Parameters.AddWithValue("@kartki", kartki);
                cmd2.ExecuteNonQuery();

                polaczenie.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Usun_album(string tytul)
        {
            bool czy_znaleziono = false;
            foreach (Album a in Lista_Albumow)
            {
                if (a.tytul == tytul) //sprawdza czy osoba o danym imieniu i nazwisku znajduje sie na liscie
                {
                    try
                    {
                        int indeks = Lista_Albumow.IndexOf(a); //sprawdza indeks elementu o takich danych 
                        czy_znaleziono = true;
                        Lista_Albumow.RemoveAt(indeks); //usuwa ten element z listy

                        polaczenie.Open();

                        int ID_albumu = 0;
                        //wyszukiwanie ID albumu o podany tytule
                        string kwerenda = "SELECT album.ID FROM album JOIN ksiazka ON ksiazka.ID = album.ID_ksiazki WHERE Tytul = @tytul";
                        MySqlCommand cmd = new MySqlCommand(kwerenda, polaczenie);

                        cmd.Parameters.AddWithValue("@tytul", tytul);  //przypisanie zmiennych 
                        ID_albumu = Convert.ToInt32(cmd.ExecuteScalar());

                        //usuwanie albumu o pobranym wyzej ID
                        string kwerenda2 = "DELETE FROM album WHERE ID = @ID_albumu";
                        MySqlCommand cmd2 = new MySqlCommand(kwerenda2, polaczenie);

                        cmd2.Parameters.AddWithValue("@ID_albumu", ID_albumu);  //przypisanie zmiennych
                        cmd2.ExecuteNonQuery();

                        polaczenie.Close();
                        break; // jeśli już znaleziono obiekt, można przerwać pętlę
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (czy_znaleziono == false) //jesli nie ma takiej osoby na liscie 
            {
                Console.WriteLine("Nie ma takiego albumu");
            }
        }


        public void Wyswietl_dluznikow() //działa z naliczaniem opłat
        {
            //na listach
            DateTime obecna_data = DateTime.Today;
            Console.WriteLine("Lista dłużników: ");
            Console.WriteLine("+--------------------------------------------+");
            foreach(Wypozyczona_ksiazka w in Lista_biblioteka)
            {
                if(obecna_data > w.Data_zwro)
                {
                    TimeSpan roznica = obecna_data.Subtract(w.Data_zwro);
                    int dni = (int)(roznica.TotalSeconds / (24 * 60 * 60));
                    string data_zwrotu = w.Data_zwro.ToString("yyyy-MM-dd");

                    Console.WriteLine("Czytelnik: {0} {1} ", w.czytelnik.imie, w.czytelnik.nazwisko);
                    Console.WriteLine("Tytul: {0}", w.ksiazka.tytul);
                    Console.WriteLine("Data zwrotu: {0} ", data_zwrotu);
                    Console.WriteLine("Naliczona opłata: {0}zł", dni);
                    Console.WriteLine("+--------------------------------------------+");
                    Thread.Sleep(50);
                }
            }
        }    
    }
}
