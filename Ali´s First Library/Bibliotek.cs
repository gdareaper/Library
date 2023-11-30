    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using static inlämn.Upp_Library_1._0.Library;
    using static System.Reflection.Metadata.BlobBuilder;

    namespace inlämn.Upp_Library_1._0
    {

        public class Library
        {/// <summary>
        /// förbättringar: 1). skulle vilja komma på ett sätt att kunns uppdatera tillgängliga böcker listan medans programmet körs och inte att programmet 
        /// måste avslutas först för att ändringarna ska visas i den tillagda listan av böcker
        /// </summary>
            
            //mina listor över låntagare och böcker
             List<Låntagare> Borrowers = new List<Låntagare>();
             List<Bok> Books = new List<Bok>();

            //Filepath till de båda json filerna, böcker och låntagare
            static string filePath = "böcker.json";
            static string filepath2 = "låntagare.json";

            //Lägga till nya låntagare
            public void AddNewLenders()
            {
                Console.WriteLine("Give name of lender:");
                string? namn = Console.ReadLine();

                Console.WriteLine("Give lenders personnummer in the format YYYY/mm/dd:");
                string? personmummer = Console.ReadLine();

                Borrowers.Add(new Låntagare { Namn = namn, Personnummer = personmummer});

                Console.WriteLine("Lender has been added.");
            }
            
            //Ladda låntagare från fil
            public void LoadLendersFromFile()
            {
                if (File.Exists(filepath2))
                {
                    string jsonContent = File.ReadAllText(filepath2);
                    Borrowers = JsonConvert.DeserializeObject<List<Låntagare>>(jsonContent) ?? new List<Låntagare>();
                }
            }
            
            //Spara långtagare till fil
            public void SaveLendersToFile()
            {
                string jsonContent = JsonConvert.SerializeObject(Borrowers, Formatting.Indented);
                File.WriteAllText(filepath2, jsonContent);
            }

            //Lägga till ny bok
            public void AddNewBook()
            {
                Console.WriteLine("Give book title:");
                string titel = Console.ReadLine();
                Console.WriteLine("Give name of Author:");
                string författare = Console.ReadLine();

                Books.Add(new Bok { Titel = titel, Författare = författare,  Utlånad = false });

                Console.WriteLine("Book has been added.");
            }

        public bool RemoveBook(string titel)
        {
            //Kontrollerar om Titeln Är Tom eller Enbart Består av Mellanslag
            if (string.IsNullOrWhiteSpace(titel))
            {
                Console.WriteLine("Title can´t be empty.");
                return false;
            }

            //Söker efter Boken
            var bok = Books.FirstOrDefault(b => b.Titel.Equals(titel, StringComparison.OrdinalIgnoreCase));

            //Kontrollerar om Boken Finns
            if (bok == null)
            {
                Console.WriteLine("The book wasn´t found.");
                return false;
            }

            //Tar Bort Boken och Bekräftar
            Books.Remove(bok);
            Console.WriteLine($"Book '{titel}' has been removed.");
            return true;
        }


        //uppdatera bokstatus
        public void UpdateBookStatus()
            {
                Console.WriteLine("What title do you want to update?:");
                string titel = Console.ReadLine();

                //Söker efter Boken
                var book = Books.FirstOrDefault(b => b.Titel.Equals(titel, StringComparison.OrdinalIgnoreCase));
                if (book != null)
                {
                    //Kontrollerar om Boken Finns och Uppdaterar Status
                    book.Utlånad = !book.Utlånad;
                    Console.WriteLine($"The books status has been updated: {(book.Utlånad ? "borrowed" : "not borrowed")}");
                }
                else
                {
                //Hanterar Fall Där Boken Inte Hittas
                Console.WriteLine("Book wasn´t found.");
                }
            }
       
      
            //spara tillagda böcker till fil
            public void SaveBooksToFile()
            {
            // Serialisera listan av böcker till en JSON-sträng
            string jsonContent = JsonConvert.SerializeObject(Books, Formatting.Indented);
            File.WriteAllText(filePath, jsonContent);

            /* using (StreamWriter file = new StreamWriter(filePath, false)) 
             {
                 file.Write(jsonContent);
             }*/ ///<summary>
            //ville experimentera och se om jag kunde få filen att stängas så
            //att de nya böckerna skulle kunna sparas med using, men det funkade inte!</summary>


        }


        //Ladda bok från fil
        public void LoadBookFromFile()
            {
                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);
                    Books = JsonConvert.DeserializeObject<List<Bok>>(jsonContent) ?? new List<Bok>();
                }
            }

            //Låna ut bok
            public void LendOutBook()
            {
                Console.WriteLine("Give lenders name:");
                string låntagareNamn = Console.ReadLine();

                var låntagare = Borrowers.FirstOrDefault(l => l.Namn.Equals(låntagareNamn, StringComparison.OrdinalIgnoreCase));
                if (låntagare == null)
                {
                    Console.WriteLine("Lender is not in the system.");
                    return;
                }

                Console.WriteLine("Name the title you wish to borrow:");
                string titel = Console.ReadLine();

                var bok = Books.FirstOrDefault(b => b.Titel.Equals(titel, StringComparison.OrdinalIgnoreCase));
                if (bok != null && !bok.Utlånad)
                {
                    bok.Utlånad = true;
                    bok.LånadAv = låntagareNamn; // Sätt namnet på låntagaren
                    Console.WriteLine($"{titel} has been lended to {låntagareNamn}.");
                }
                else
                {
                    Console.WriteLine("Book is not available or doesn´t exist.");
                }

                // Spara ändringarna
                SaveBooksToFile();
            }

        //metod för återlämning av bok
        public void ReturnBook(string titel)
        {
            var bok = Books.FirstOrDefault(b => b.Titel.Equals(titel, StringComparison.OrdinalIgnoreCase));
            if (bok != null)
            {
                if (bok.Utlånad)
                {
                    bok.Utlånad = false;
                    bok.LånadAv = null; // Nollställ vem som lånat boken
                    Console.WriteLine($"The book '{titel}' has been returned.");
                    SaveBooksToFile(); // Spara ändringarna till filen
                }
                else
                {
                    Console.WriteLine("Book has already been returned.");
                }
            }
            else
            {
                Console.WriteLine("Book wasn´t found.");
            }
        }


        //visa låntagare och deras böcker
        public void ShowLendersWithBooks()
            {
                foreach (var låntagare in Borrowers)
                {
                    Console.WriteLine($"Name: {låntagare.Namn} -- {låntagare.Personnummer}");
                    var lånadeBöcker = Books.Where(b => b.LånadAv == låntagare.Namn);
                    foreach (var bok in lånadeBöcker)
                    {
                        Console.WriteLine($"\tBorrowed book: {bok.Titel}");
                    }
                }
            }
     


    }
}
