using MySql.Data.MySqlClient;
using System;
using System.Reflection;

namespace projekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Biblioteka biblioteka = new Biblioteka();
            Console.Title = "BIBLIOTEKA";
            string connectionString = "server=localhost;user id=root;password=;database=biblioteka";
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.WriteLine("          ______     __   ______     __      __   _______   ________   _______   __   ___   _________ ");
                Console.WriteLine("         |  __  |   |  | |  __  |   |  |    |  | |  ___  | |__    __| |   ____| |  | /  /  |   ___   |");
                Console.WriteLine("         | |__| |_  |  | | |__| |_  |  |    |  | | |   | |    |  |    |  |__    |  |/  /   |  |   |  |");
                Console.WriteLine("         |  ____  | |  | |  ____  | |  |    |  | | |   | |    |  |    |   __|   |     <    |  |___|  |");
                Console.WriteLine(@"         | |____| | |  | | |____| | |  |__  |  | | |___| |    |  |    |  |____  |  |\  \   |   ___   |");
                Console.WriteLine(@"         |________| |__| |________| |_____| |__| |_______|    |__|    |_______| |__| \__\  |__|   |__|");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(7, 7);
                Console.WriteLine("\t\t\t\t\tŁADOWANIE ZASOBÓW, PROSZĘ CZEKAĆ");
                Console.WriteLine("\t\t\t\t ___________________________________________________");
                Console.WriteLine("\t\t\t\t|___________________________________________________|");
                Console.SetCursorPosition(33, 9);
                for (int i = 0; i < 51; i++)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("_");
                    Thread.Sleep(50);
                }
                ConsoleKeyInfo keyInfo;
                Console.ResetColor();
                int wybor = 1;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;

                do
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    
                    Console.WriteLine("\n");
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("        +-----------------------++----------------------++----------------------++----------------------+");
                    Console.BackgroundColor = wybor == 1 ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.Write("\t|\tCZYTELNICY\t|");

                    Console.BackgroundColor = wybor == 2 ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.Write("|\tAUTORZY\t\t|");

                    Console.BackgroundColor = wybor == 3 ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.Write("|\t KSIĄŻKI\t|");

                    Console.BackgroundColor = wybor == 4 ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.WriteLine("|\t BIBLIOTEKA\t|");

                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("        +-----------------------++----------------------++----------------------++----------------------+");
                    keyInfo = Console.ReadKey();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            wybor = wybor == 1 ? 4 : wybor - 1;
                            break;
                        case ConsoleKey.RightArrow:
                            wybor = wybor == 4 ? 1 : wybor + 1;
                            break;
                        case ConsoleKey.Enter:
                            switch (wybor)
                            {
                                case 1:
                                    {
                                        int wybor2 = 1;
                                        Console.BackgroundColor = ConsoleColor.Gray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Clear();

                                        ConsoleKeyInfo keyInfo2;

                                        do
                                        {
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            
                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.WriteLine("|                 CZYTELNICY                 |");
                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.BackgroundColor = wybor2 == 1 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor2 == 1 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 1. DODAJ CZYTELNIKA                        |");

                                            Console.BackgroundColor = wybor2 == 2 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor2 == 2 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 2. WYSWIETL CZYTELNIKOW                    |");

                                            Console.BackgroundColor = wybor2 == 3 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor2 == 3 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 3. EDYTUJ CZYTELNIKA                       |");

                                            Console.BackgroundColor = wybor2 == 4 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor2 == 4 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 4. USUŃ CZYTELNIKA                         |");

                                            Console.ResetColor();
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.WriteLine("+--------------------------------------------+");

                                            keyInfo2 = Console.ReadKey();

                                            switch (keyInfo2.Key)
                                            {
                                                case ConsoleKey.UpArrow:
                                                    wybor2 = wybor2 == 1 ? 4 : wybor2 - 1;
                                                    break;
                                                case ConsoleKey.DownArrow:
                                                    wybor2 = wybor2 == 4 ? 1 : wybor2 + 1;
                                                    break;
                                                case ConsoleKey.Enter:
                                                    switch (wybor2)
                                                    {
                                                        case 1:
                                                            {
                                                                // Logika dla opcji 1 - DODAJ CZYTELNIKA
                                                                Console.WriteLine("Podaj imie czytelnika: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko czytelnika: ");
                                                                string nazwisko = Console.ReadLine();

                                                                biblioteka.Dodaj_czytelnika(imie, nazwisko);
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                // Logika dla opcji 2 - WYSWIETL CZYTELNIKOW
                                                                biblioteka.Wyswietl_czytelnikow();
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                // Logika dla opcji 3 - EDYTUJ CZYTELNIKA
                                                                Console.WriteLine("Podaj imie czytelnika ktorego chcesz edytowac: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko czytelnika ktorego chcesz edytowac: ");
                                                                string nazwisko = Console.ReadLine();

                                                                Console.WriteLine("Podaj nowe imie: ");
                                                                string nowe_imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nowe nazwisko: ");
                                                                string nowe_nazwisko = Console.ReadLine();

                                                                biblioteka.Edytuj_czytelnika(imie, nazwisko, nowe_imie, nowe_nazwisko);         
                                                                break;
                                                            }
                                                        case 4:
                                                            {
                                                                // Logika dla opcji 3 - USUŃ CZYTELNIKA
                                                                Console.WriteLine("Podaj imie czytelnika: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko czytelnika: ");
                                                                string nazwisko = Console.ReadLine();

                                                                biblioteka.Usun_czytelnika(imie, nazwisko);                                                     
                                                                break;
                                                            }
                                                    }
                                                    Console.WriteLine(" Naciśnij dowolny klawisz, by wrócić... ");
                                                    Console.ReadKey();
                                                    break;
                                            }
                                        }
                                        while (keyInfo2.Key != ConsoleKey.Escape);

                                        break;
                                    }
                                case 2:
                                    {
                                        int wybor3 = 1;
                                        Console.BackgroundColor = ConsoleColor.Gray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Clear();
                                        ConsoleKeyInfo keyInfo3;
                                        do
                                        {
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.Clear();

                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.WriteLine("|                  AUTORZY                   |");
                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.BackgroundColor = wybor3 == 1 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor3 == 1 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 1. DODAJ AUTORA                            |");

                                            Console.BackgroundColor = wybor3 == 2 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor3 == 2 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 2. WYŚWIETL AUTOROW                        |");

                                            Console.BackgroundColor = wybor3 == 3 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor3 == 3 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 3. EDYTUJ AUTORA                           |");
                                            

                                            Console.BackgroundColor = wybor3 == 4 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor3 == 4 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 4. USUŃ AUTORA                             |");

                                            Console.ResetColor();
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.WriteLine("+--------------------------------------------+");
                                            keyInfo3 = Console.ReadKey();

                                            switch (keyInfo3.Key)
                                            {
                                                case ConsoleKey.UpArrow:
                                                    wybor3 = wybor3 == 1 ? 4 : wybor3 - 1;
                                                    break;
                                                case ConsoleKey.DownArrow:
                                                    wybor3 = wybor3 == 4 ? 1 : wybor3 + 1;
                                                    break;
                                                case ConsoleKey.Enter:
                                                    switch (wybor3)
                                                    {
                                                        case 1:
                                                            {
                                                                // Logika dla opcji 1 - DODAJ AUTORA
                                                                Console.WriteLine("Podaj imie autora: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko autora: ");
                                                                string nazwisko = Console.ReadLine();

                                                                biblioteka.Dodaj_autora(imie, nazwisko);
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                // Logika dla opcji 2 - WYŚWIETL AUTOROW
                                                                biblioteka.Wyswietl_autorow();
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                // Logika dla opcji 3 - EDYTUJ AUTORA
                                                                Console.WriteLine("Podaj imie autora ktorego chcesz edytowac: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko autora ktorego chcesz edytowac: ");
                                                                string nazwisko = Console.ReadLine();

                                                                Console.WriteLine("Podaj nowe imie: ");
                                                                string nowe_imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nowe nazwisko: ");
                                                                string nowe_nazwisko = Console.ReadLine();

                                                                biblioteka.Edytuj_autora(imie, nazwisko, nowe_imie, nowe_nazwisko);
                                                                break;
                                                                
                                                            }
                                                        case 4:
                                                            {
                                                                // Logika dla opcji 4 - USUŃ AUTORA
                                                                Console.WriteLine("Podaj imie autora: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko autora: ");
                                                                string nazwisko = Console.ReadLine();

                                                                biblioteka.Usun_autora(imie, nazwisko);                                                            
                                                                break;
                                                            }
                                                    }
                                                    Console.WriteLine(" Naciśnij dowolny klawisz, by wrócić... ");
                                                    Console.ReadKey();
                                                    break;
                                            }

                                        }
                                        while (keyInfo3.Key != ConsoleKey.Escape);

                                        break;
                                    }
                                case 3:
                                    {
                                        int wybor4 = 1;
                                        Console.BackgroundColor = ConsoleColor.Gray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Clear();
                                        ConsoleKeyInfo keyInfo4;
                                        do
                                        {
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.Clear();

                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.WriteLine("|                  KSIĄŻKI                   |");
                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.BackgroundColor = wybor4 == 1 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor4 == 1 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 1. DODAJ KSIĄŻKĘ                           |");

                                            Console.BackgroundColor = wybor4 == 2 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor4 == 2 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 2. EDYTUJ KSIĄŻKĘ                          |");

                                            Console.BackgroundColor = wybor4 == 3 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor4 == 3 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 3. USUŃ KSIĄŻKĘ                            |");

                                            Console.ResetColor();
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.WriteLine("+--------------------------------------------+");
                                            keyInfo4 = Console.ReadKey();

                                            switch (keyInfo4.Key)
                                            {
                                                case ConsoleKey.UpArrow:
                                                    wybor4 = wybor4 == 1 ? 3 : wybor4 - 1;
                                                    break;
                                                case ConsoleKey.DownArrow:
                                                    wybor4 = wybor4 == 3 ? 1 : wybor4 + 1;
                                                    break;
                                                case ConsoleKey.Enter:
                                                    switch (wybor4)
                                                    {
                                                        case 1:
                                                            {
                                                                // Logika dla opcji 1 - DODAJ KSIĄŻKE
                                                                Console.WriteLine("Podaj tytul: ");
                                                                string tytul = Console.ReadLine();

                                                                Console.WriteLine("Podaj imie autora: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko autora: ");
                                                                string nazwisko = Console.ReadLine();

                                                                biblioteka.Dodaj_ksiazke(imie, nazwisko, tytul);
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                // Logika dla opcji 2 - EDYTUJ KSIAZKI
                                                                biblioteka.Wyswietl_ksiazki();

                                                                Console.WriteLine("Podaj tytul ksiazki która chcesz edytowac: ");
                                                                string stary_tytul = Console.ReadLine();

                                                                Console.WriteLine("Podaj nowy tytul: ");
                                                                string nowy_tytul = Console.ReadLine();

                                                                biblioteka.Edytuj_ksiazke(stary_tytul, nowy_tytul);
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                // Logika dla opcji 3 - USUŃ KSIAZKI
                                                                Console.WriteLine("Podaj tytul: ");
                                                                string tytul = Console.ReadLine();

                                                                biblioteka.Usun_ksiazke(tytul);
                                                                break;
                                                            }
                                                    }
                                                    Console.WriteLine(" Naciśnij dowolny klawisz, by wrócić... ");
                                                    Console.ReadKey();
                                                    break;
                                            }

                                        }
                                        while (keyInfo4.Key != ConsoleKey.Escape);

                                        break;
                                    }
                                case 4:
                                    {
                                        int wybor5 = 1;
                                        Console.BackgroundColor = ConsoleColor.Gray;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Clear();
                                        ConsoleKeyInfo keyInfo5;
                                        do
                                        {
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.Clear();

                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.WriteLine("|                 BIBLIOTEKA                 |");
                                            Console.WriteLine("+--------------------------------------------+");
                                            Console.BackgroundColor = wybor5 == 1 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor5 == 1 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 1. WYŚWIETL WSZYSTKIE KSIĄŻKI              |");

                                            Console.BackgroundColor = wybor5 == 2 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor5 == 2 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 2. WYŚWIETL KSIĄŻKI CZYTELNIKA             |");

                                            Console.BackgroundColor = wybor5 == 3 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor5 == 3 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 3. WYPOŻYCZ KSIĄŻKĘ                        |");

                                            Console.BackgroundColor = wybor5 == 4 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor5 == 4 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 4. WYŚWIETL DŁUŻNIKÓW                      |");


                                            Console.BackgroundColor = wybor5 == 5 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
                                            Console.ForegroundColor = wybor5 == 5 ? ConsoleColor.White : ConsoleColor.Black;
                                            Console.WriteLine("| 5. ODDAJ KSIĄŻKĘ                           |");

                                            Console.ResetColor();
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.BackgroundColor = ConsoleColor.Gray;
                                            Console.WriteLine("+--------------------------------------------+");
                                            keyInfo5 = Console.ReadKey();

                                            switch (keyInfo5.Key)
                                            {
                                                case ConsoleKey.UpArrow:
                                                    wybor5 = wybor5 == 1 ? 5 : wybor5 - 1;
                                                    break;
                                                case ConsoleKey.DownArrow:
                                                    wybor5 = wybor5 == 5 ? 1 : wybor5 + 1;
                                                    break;
                                                case ConsoleKey.Enter:
                                                    switch (wybor5)
                                                    {
                                                        case 1:
                                                            {
                                                                // Logika dla opcji 1 - WYŚWIETL WSZYSTKIE KSIĄŻKI
                                                                biblioteka.Wyswietl_ksiazki();
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                // Logika dla opcji 2 - WYŚWIETL KSIĄŻKI CZYTELNIKA   
                                                                Console.WriteLine("Podaj imie: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko: ");
                                                                string nazwisko = Console.ReadLine();

                                                                biblioteka.Wyswietl_ksiazki_czytelnika(imie, nazwisko);
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                // Logika dla opcji 3 - WYPOŻYCZ KSIĄŻKĘ 
                                                                Console.WriteLine("Podaj imie: ");
                                                                string imie = Console.ReadLine();

                                                                Console.WriteLine("Podaj nazwisko: ");
                                                                string nazwisko = Console.ReadLine();

                                                                Console.WriteLine("Podaj tytul: ");
                                                                string tytul = Console.ReadLine();

                                                                biblioteka.Wypozycz_ksiazke(imie, nazwisko, tytul);
    
                                                                break;
                                                            }
                                                        case 4:
                                                            {
                                                                // Logika dla opcji 4 - WYŚWIETL DŁUŻNIKÓW
                                                                biblioteka.Wyswietl_dluznikow();
                                                                break;
                                                            }
                                                        case 5:
                                                            {
                                                                // Logika dla opcji 5 - ODDAJ KSIĄŻKĘ 
                                                                Console.WriteLine("Podaj tytul: ");
                                                                string tytul = Console.ReadLine();

                                                                biblioteka.Oddaj_ksiazke(tytul);
                                                                break;
                                                            }
                                                    }
                                                    Console.WriteLine("Naciśnij dowolny klawisz, by wrócić... ");
                                                    Console.ReadKey();
                                                    break;
                                            }

                                        }
                                        while (keyInfo5.Key != ConsoleKey.Escape);

                                        break;
                                    }
                            }
                            Console.ReadKey();
                            break;
                    }


                }
                while (keyInfo.Key != ConsoleKey.Escape);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}