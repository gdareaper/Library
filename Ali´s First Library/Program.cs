using System.Collections.Generic;
using static inlämn.Upp_Library_1._0.Library;
using Newtonsoft.Json;
using System;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net.Http.Json;

namespace inlämn.Upp_Library_1._0
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.Title = "Los Bibletekas";
            Console.WriteLine($"{Console.Title,50}");

            Library library = new Library();

            ///<summary>hade problem med att syncronisera min
            ///json böcker lista med den listan som nytillagda
            ///böcker spara i, som är samma lista. Väldigt konstigt</summary>
            //List<Bok> Böcker = new List<Bok>();


            Bok bok1 = new Bok();
            Låntagare låntagare1 = new Låntagare();

          


            string filePath = "böcker.json";
            string filepath2 = "låntagare.json";

            // Läs in hela JSON-filen som en sträng.
            string json = File.ReadAllText(filePath);
            string json2 = File.ReadAllText(filepath2);

            // Deserialisera JSON-strängen till en lista av Bok-objekt.
            List<Bok> Böcker = JsonConvert.DeserializeObject<List<Bok>>(json);
            List<Låntagare> låntagare = JsonConvert.DeserializeObject<List<Låntagare>>(json2);

            library.LoadBookFromFile();
            library.LoadLendersFromFile();

         


            string menuChoice = "0";

            while (menuChoice != "3")
            {

                Console.WriteLine("\nHere is the menu, Please make a choice\n");

                Console.WriteLine("1. Add New books to library");
                Console.WriteLine("2. Borrow book");
                Console.WriteLine("3. Return book");
                Console.WriteLine("4. Show available books");
                Console.WriteLine("5. Show lenders");
                Console.WriteLine("6. Show lenders and their books");
                Console.WriteLine("7. Add lender");
                Console.WriteLine("8. Update book Status");
                Console.WriteLine("9. Remove a book");
                Console.WriteLine("0. Quit Program");






                menuChoice = Console.ReadLine();

                Console.WriteLine();

                switch (menuChoice)
                {
                    case "1":


                        library.AddNewBook();

                        library.SaveBooksToFile();

                        break;

                    case "2":

                        library.LendOutBook();

                        break;

                    case "3":
                        Console.WriteLine("Write the name of the title that you wish to return:");
                        string title = Console.ReadLine();

                        library.ReturnBook(title);
                        //library.SparaBöckerTillFil();


                        break;


                    case "4":

                        //lista av böcker som finns i biblioteket                    
                        foreach (var bok in Böcker)
                        {
                            library.LoadBookFromFile();
                            Console.WriteLine($"Author: {bok.Författare}");
                            Console.WriteLine($"Title: {bok.Titel}");
                            Console.WriteLine($"Borrowed: {(bok.Utlånad ? "Yes" : "No")}");
                            Console.WriteLine(new string('-', 20)); // Skapar en avdelare mellan böckerna                            

                        }

                        break;

                    case "5":

                        //lista av låntagare som finns i biblioteket
                        foreach (var borrower in låntagare)
                        {
                            Console.WriteLine($"Name: {borrower.Namn}");
                            Console.WriteLine($"Personnummer: {borrower.Personnummer}");
                            Console.WriteLine(new string('-', 20)); // Skapar en avdelare mellan låntagarna
                        }
                        break;

                    case "6":
                        library.ShowLendersWithBooks();

                        break;

                    case "7":

                        library.AddNewLenders();
                        library.SaveLendersToFile();
                        break;

                    case "8":
                        //uppdatera en boks status från utlånad till tillgänglig
                        library.UpdateBookStatus();
                        break;

                    case "9":
                        //Ta bort en bok
                        Console.WriteLine("Write the title that you wish to remove:");
                        string tabortBok = Console.ReadLine();

                        bool borttagen = library.RemoveBook(tabortBok);
                        if (borttagen)
                        {
                            library.SaveBooksToFile();
                        }
                        break;
                    case "0":
                        Console.WriteLine("Program Shutdown.");
                        Console.WriteLine();
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;
                    default: Console.WriteLine("Your choice is not available on the menu");
                        break;
                } 
                    
                
            }

            




            library.SaveBooksToFile();
        }
    }
    
}