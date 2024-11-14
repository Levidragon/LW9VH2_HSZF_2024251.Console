using LW9VH2_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LW9VH2_HSZF_2024251.Persistence.MsSql
{
    public interface IBooksDataProvider
    {
        // metodusok amiket az IBooksDataProvider keresytul eg lehet hivni kivulrol ajd 
        // ebbol lehet ajd tobbet is csinalni ha asik entitasok kernek le infot innen
        // eddig jo 
        Books GetBookById(int id);

        // eddig jo 
        void UpdateBook(int id, Books NewBooks);

        void RemoveBook(int id);

        // eddig jo 
        void AddBooks(Books Book);
        public void AddAuthor(Authors author);
        public void AddCategory(Categories category);

        // eddig jo 
        List<string> Pageandprice();

        // eddig jo 
        IEnumerable<Books> GetBooks();

        //Adott évben kiadott könyvek
        IEnumerable<Books> BooksPpublishedYear(int year);

        //Legnépszerűbb kategória
        string MostPopularCategory();

        //Kiadási év szerinti könyvek riportja
        Dictionary<int, List<Books>> BooksByYearOfPublication();

        //Kategóriánkénti könyvforgalom
        List<string> BookCirculationByCategory();
        public void RemoveAll();
        

        public Categories AlreadyCategory(string name);
        public Books AlreadyBook(string title);
        public Authors AlreadyAuthor(string name);
        public Dictionary<string , List<Books>> AuthorsAndBooks();







    }
    public class BooksDateaProvider : IBooksDataProvider
    {
        private readonly BooksDBContext context;
        // AppDBContext az inyektalasa
        public BooksDateaProvider(BooksDBContext context)
        {
            this.context = context;
        }

        // nem jo
        public void RemoveBook(int id)
        {
            var item = context.BooksTable.FirstOrDefault(x => x.Id == id);
            if (item == null) throw new NullReferenceException();
            context.BooksTable.Remove(item);
            context.SaveChanges();

        }

        // eddig jo 
        // You can update the price page and the puplication date
        public void UpdateBook(int id, Books NewBooks)
        {
            if (NewBooks == null) throw new NullReferenceException();
            var item = context.BooksTable.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                AddBooks(NewBooks);
            }
            else
            {
                item.Category = NewBooks.Category;
                item.Title = NewBooks.Title;
                item.Author = NewBooks.Author;
                item.Price = NewBooks.Price;
                item.Pages = NewBooks.Pages;
                item.Publication_Year = NewBooks.Publication_Year;
            }
        }

        // eddig jo 
        public void AddBooks(Books Book)
        {
            context.BooksTable.Add(Book);
            context.SaveChanges();
        }
        public void AddAuthor(Authors author)
        {
            context.AuthorsTable.Add(author);
            context.SaveChanges();
        }
        public void AddCategory(Categories category)
        {
            context.CategoriesTable.Add(category);
            context.SaveChanges();
        }

        // eddig jo 
        public Dictionary<int, List<Books>> BooksByYearOfPublication()
        {
            Dictionary<int, List<Books>> dictionary = new Dictionary<int, List<Books>>();

            var table = context.BooksTable.Include(x => x.Category).Include(c=>c.Author);
            var booksByYear = table
            .GroupBy(b => b.Publication_Year)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                Year = g.Key,
                Books = g.ToList()
            })
           ;
            foreach (var group in booksByYear)
            {
                dictionary[group.Year] = group.Books;
            }

            return dictionary;

        }

        // eddig jo 
        public IEnumerable<Books> BooksPpublishedYear(int year)
        {
            var bookandauthors = context.BooksTable.Include(b => b.Author).Include(c => c.Category);

            var books = bookandauthors.Where(x => x.Publication_Year == year);

            if (books == null) throw new NullReferenceException();

            return books;
        }

        //eddig jo 
        public Books GetBookById(int id)
        {
            var book = context.BooksTable.FirstOrDefault(x => x.Id == id);
            if (book == null) throw new NullReferenceException("Contains no value");
            return book;

        }

        //eddig jo 
        public IEnumerable<Books> GetBooks()
        {
            return context.BooksTable.Include(x => x.Category).Include(z => z.Author);
        }

        // eddig jo
        public string MostPopularCategory()
        {
            var table = context.BooksTable.Include(x => x.Category);
            var BestCategori = table
                .GroupBy(x => x.Category)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    BookCount = group.Count()
                }).OrderByDescending(x => x.BookCount).First();

            return $"{BestCategori.CategoryId.Name} : {BestCategori.BookCount} piece ";

        }

        //edig jo 
        public List<string> BookCirculationByCategory()
        {

            List<string> list = new List<string>();

            var grup = context.BooksTable.Include(k => k.Category).AsEnumerable().GroupBy(x => x.Category);
            var selected = grup.Select(g => new
            {
                Category = g.Key,
                BookCount = g.Count(),
                AvgPrice = g.Average(p => p.Price),
                AvgPage = g.Average(o => o.Pages)
            }).ToList();

            foreach (var item in selected)
            {
                list.Add($" Category : {item.Category.Name}  Books Count : {item.BookCount} Avrage Price : {item.AvgPrice} Avrage Page : {item.AvgPage}");

            }

            return list;
        }

        // edig jo 
        public List<string> Pageandprice()
        {
            List<string> list = new List<string>();
            var nemtudom = context.BooksTable.Select(t => new
            {
                Id = t.Id,
                BookTitle = t.Title,
                PriceValueRatio = t.Pages / t.Price,
            }).ToList();
            foreach (var item in nemtudom)
            {
                list.Add($"Id : {item.Id}  Titkle : {item.BookTitle}  Price value ratio : {item.PriceValueRatio} ");
            }
            return list;



        }

        public void RemoveAll()
        {
            context.Database.EnsureDeleted();
        }

        public Dictionary<string, List<Books>> AuthorsAndBooks()
        {
            Dictionary<string, List<Books>> dictionary = new Dictionary<string, List<Books>>();

            var table = context.BooksTable.Include(x => x.Category).Include(c => c.Author);
            var booksByAuthors = table
            .GroupBy(b => b.Author)
            .Select(g => new
            {
                Name = g.Key.Name,
                Books = g.ToList()
            })
           ;
            foreach (var group in booksByAuthors)
            {
                dictionary[group.Name] = group.Books;
            }

            return dictionary;

        }

        public Categories AlreadyCategory(string name)
        {
            Categories category = context.CategoriesTable.FirstOrDefault(x => x.Name== name);
            return category;
        }

        public Books AlreadyBook(string title)
        {
            Books book = context.BooksTable.FirstOrDefault(x => x.Title == title);
            return book;
        }

        public Authors AlreadyAuthor(string name)
        {
            Authors author = context.AuthorsTable.FirstOrDefault(x => x.Name == name);
            return author;
        }






    }
}