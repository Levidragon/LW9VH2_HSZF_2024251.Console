using LW9VH2_HSZF_2024251.Model;
using LW9VH2_HSZF_2024251.Persistence.MsSql;
using System.Collections.Generic;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.IO;
using System;

namespace LW9VH2_HSZF_2024251.Application
{



    // a könnyebb kiriás miatt testeld hogy a categori táblát nem tombként hanem sima vátozoként rakod bele a book táblába 
    // KI KELL DERITENI HOGYAN LEHET PL AZ IRÓ NEVÁT IS MEGJELENITENI (toStringbooks metodusban ird ar)
    public interface ILibriService
    {
        void GetBookById(int id);
        void UpdateBook(int id, Books NewBooks);
        void RemoveBook(int id);
        void AddBooks();
        //A könyvek és oldalszám viszonya
        void GetBooks();
        void GetBooks(int BooksNumber);
        //Adott évben kiadott könyvek
        void BooksPpublishedYear();
        //Legnépszerűbb kategória
        void MostPopularCategory();
        //Kiadási év szerinti könyvek riportja
        void BooksByYearOfPublication();
        //Kategóriánkénti könyvforgalom
        void BookCirculationByCategory();
        void Pageandprice();
        public void RemoveAll();
        public void  AuthorsMappa();

      
    }
    public class LibriService: ILibriService
    {
        private readonly IBooksDataProvider Querys;

        public LibriService(IBooksDataProvider iterface )
        {
            this.Querys = iterface;
        }

        // eddig jo 
        public void GetBookById(int id)
        {
            Console.WriteLine(ToStringBooks(Querys.GetBookById(id)));
        }

        // eddig jo 
        public void UpdateBook(int id, Books NewBooks)
        {
            Querys.UpdateBook(id, NewBooks);
        }

        // eddig jo 
        public void RemoveBook(int id)
        {
            Querys.RemoveBook(id);
        }

        // eddig jo 
        public void AddBooks()
        {
            Console.Write("Titel:");
            string Title=Console.ReadLine();

            Console.WriteLine();
            Console.Write("Price:");
            double Price  = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine();
            Console.Write("Publication year:");
            int PublcationYear = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.Write("Isbn:");
            string Isbn = Console.ReadLine();

            Console.WriteLine();
            Console.Write("Page number :");
            int Page  = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.Write("Author name :");
            string name= Console.ReadLine();
            Authors author = Querys.AlreadyAuthor(name);

            if(author == null)
            {
                Console.WriteLine("\n");
                Console.WriteLine("There is no such author in the database, please enter the author's details.");

                Console.WriteLine("\n");
                Console.Write("Birth year:");
                int birthyear = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("\n");
                Console.Write("Nationality:");
                string nationality = Console.ReadLine();

                author = new Authors(name, birthyear, nationality);
                Querys.AddAuthor(author);


            }

            Console.WriteLine();
            Console.Write("Category:");
            string categoryname = Console.ReadLine();

            Categories category = Querys.AlreadyCategory(categoryname);
            if (category== null)
            {
                Console.WriteLine();
                Console.WriteLine("There is no such category in the database, please enter the category's details.");
               
                Console.WriteLine();
                Console.Write("Description:");
                string description = Console.ReadLine();
                category= new Categories(categoryname,description);

                Querys.AddCategory(category);

            }

            Books NewBook = new Books(Title,Price,PublcationYear,Isbn,Page, author, category);

            Querys.AddBooks(NewBook);
            EndOfTheReport();
        }

        // eddog jo 
        public void GetBooks()
        {


            foreach (var item in Querys.GetBooks())
            {
                Console.WriteLine(ToStringBooks(item));
            }

            EndOfTheReport();
        }

        // eddig jo
        public void GetBooks(int BooksNumber)
        {
            var books = Querys.GetBooks().ToList();

            int i = 0;

            do
            {
                Console.Clear();
                for ( int j = i;  j < i+ BooksNumber ;  j++)
                {
                    Console.WriteLine(ToStringBooks(books[j]));
                }

                Console.WriteLine(" Previous page > [A]   ");
                Console.WriteLine(" Next page > [D]   ");
                ConsoleKeyInfo key;
                
                key = Console.ReadKey();

                if (key.KeyChar=='a')
                {
                    if (i<0 || i-BooksNumber<0)
                    {
                        i = 0;
                    }
                    else
                    {
                        i-=BooksNumber;
                    }
                }
                else
                {
                    if (key.KeyChar == 'd')
                    {
                        i += BooksNumber;
                    }
                }
            }
            while (i<books.Count());
            Console.Clear();
            Console.WriteLine("End of the list");
            EndOfTheReport();

        }

        // eddig jo     
        public void BooksPpublishedYear()
        {
            Console.WriteLine("Searched Year");
            int year = Convert.ToInt32(Console.ReadLine());

            foreach (var item in Querys.BooksPpublishedYear(year))
            {
                Console.WriteLine($"{ToStringBooks(item)}");
            }
            EndOfTheReport();
        }

        // eddig jo
        public void MostPopularCategory()
        {
            Console.WriteLine($"The most popular category: {Querys.MostPopularCategory()}");
            EndOfTheReport();
        }

        // valamiért nem irja ki az iro nevét 
        public void BooksByYearOfPublication()
        {
            foreach (var item in Querys.BooksByYearOfPublication())
            {
                Console.WriteLine($"Year : {item.Key}  ");
                foreach (var item1 in item.Value)
                {
                    Console.Write($" \t - {ToStringBooks(item1)} \n");
                }
            }

            EndOfTheReport();
        }

        // eddig jo 
        public void BookCirculationByCategory()
        {

            foreach (var item in Querys.BookCirculationByCategory())
            {
                Console.WriteLine(item);
            }
            EndOfTheReport();
        }

        // eddig jo 
        public void Pageandprice()
        {
            foreach (var item in Querys.Pageandprice())
            {
                Console.WriteLine(item);
            }
            EndOfTheReport();
        }

        // eddig jo
        public void RemoveAll()
        {
           Querys.RemoveAll();
        }
        // nem jo
        public void AuthorsMappa()
        {
            string path= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Directory.CreateDirectory(Path.Combine(path, $"{DateTime.Now.ToLongDateString()}") );
            var authors  = Querys.AuthorsAndBooks();
            foreach (var item in authors)
            {
                StreamWriter sw = File.CreateText(Path.Combine(path,$"{DateTime.Now.ToLongDateString()}", $"{item.Key}.txt"));

                foreach (var item1 in item.Value)
                {
                    sw.WriteLine(ToStringBooks(item1));
                }
                sw.Close();
            }
        }

        // eddig jo
        private string ToStringBooks(Books be)
        {
            return $"Id : {be.Id},  Title : {be.Title},  Isbn : {be.Isbn},  Categori : {be.Category.Name} Author : {be.Author.Name} Price : {be.Price},  Page : {be.Pages} ";
        }

        private void EndOfTheReport ()
        {

            Console.WriteLine("Press ESC to return");
            ConsoleKeyInfo key;
            do
            {
                 key = Console.ReadKey();
                
            }
            while (key.Key != ConsoleKey.Escape);
        }


    }
}
