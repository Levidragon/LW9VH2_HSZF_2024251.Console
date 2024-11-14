// See https://aka.ms/new-console-template for more information
using ConsoleTools;
using LW9VH2_HSZF_2024251.Application;
using LW9VH2_HSZF_2024251.Model;
using LW9VH2_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.ComponentModel.Design;
 
// csinálj tobb lehetöséget egy konyv hozzáadásához  ( csak a könyv  vagy könyv, szerzö és kategoria )
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, service) =>
    {
        service.AddTransient<BooksDBContext>();
        service.AddSingleton<IBooksDataProvider,BooksDateaProvider>();
        service.AddSingleton<ILibriService,LibriService>();
    }).Build();
await host.StartAsync();

using IServiceScope serviceScope= host.Services.CreateScope();
IServiceProvider serviceProvider = serviceScope.ServiceProvider;

var btx = serviceProvider.GetService<BooksDBContext>();
var Adat = serviceProvider.GetService<ILibriService>();

SeedData(btx);

Adat.AuthorsMappa();
var menu = new ConsoleMenu()
           .Add(" All the books ", () => Adat.GetBooks(1))
           .Add(" Books published in a given year", () => Adat.BooksPpublishedYear())
           .Add(" Most popular category ", () => Adat.MostPopularCategory())
           .Add(" Books by year of publication ", () => Adat.BooksByYearOfPublication())
           .Add(" Books circulation by categoryes ", () => Adat.BookCirculationByCategory())
           .Add(" Price-value ratio of the books ", () => Adat.Pageandprice())
           .Add(" Add Book ", () => Adat.AddBooks())
           .Add(" Kilépés", ConsoleMenu.Close)
           .Configure(config =>
           {
               config.Selector = ">> ";
               config.Title = "Könyvtár menü";
               config.EnableWriteTitle = true;
               config.EnableBreadcrumb = true;
               config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
           });


menu.Show();


Logo();







//Console.WriteLine("feladat 1");
//Adat.Pageandprice();
//Console.WriteLine("feladat 2");
//Adat.BooksPpublishedYear();
//Console.WriteLine("feladat 3");
//Adat.BooksByYearOfPublication();
//Console.WriteLine("feladat 4");
//Adat.GetBookById(1);
//Console.WriteLine("feladat 5");
//Adat.GetBooks();
//Console.WriteLine(" 2 db");
//Adat.GetBooks(2);

//Console.WriteLine(" feladat 6 ");
////Adat.AddBooks(new Books("addTest", 25.3, 1995, "2341325", 45));
////Adat.GetBooks();
////Console.WriteLine(" feladat 7 ");
////Adat.UpdateBook(2, new Books("addTest", 45, 1995, "2341325", 450));
////Adat.GetBooks();
//Console.WriteLine(" feladat 8 ");
//Adat.BookCirculationByCategory();
//Console.WriteLine(" feladat 9 ");
//Adat.MostPopularCategory();
//Console.WriteLine(" feladat 10");
//Adat.GetBooks();
//Adat.RemoveBook(1);
//Console.WriteLine(" kivette ");
//Adat.GetBooks();

Adat.RemoveAll();













;

static void SeedData(BooksDBContext btx)
{
    var authors = new Authors[] {
    new Authors ("J.R.R Tolkien",1981,"Britt"),
    new Authors ("J.K Rowling",1982,"Britt"),
    new Authors ("Fekete Istvan",1980,"magyar")
};
    btx.AuthorsTable.AddRange(authors);
    btx.SaveChanges();


    var categories = new Categories[]
    {
    new Categories("Horror","wefpwoekfpwkefpkpwekf"),
    new Categories("Fantsy","owejfjwoe weoi jfoweijf "),
    new Categories("Krimi","werifuon iweun wne in")
    };

    btx.CategoriesTable.AddRange(categories);
    btx.SaveChanges();
    var books = new Books[] {
    new Books("cim1",2.3, 1990,"2341415",233 ,authors[0],categories[0]),
    new Books("cim2",5.0, 1991,"2325515",223,authors[1],categories[1]),
    new Books("cim3",1.2, 1992,"1567247",243,authors[2],categories[2])
};

    btx.BooksTable.AddRange(books);
    btx.SaveChanges();

    authors[0].Books.Add(books[0]);
    authors[1].Books.Add(books[1]);
    authors[2].Books.Add(books[2]);
    categories[0].Books.Add(books[0]);
    categories[1].Books.Add(books[1]);
    categories[2].Books.Add(books[2]);


    btx.SaveChanges();

    ;
}

void Logo()
{
    string logo = @" 
  _       _  _            _        ____                 _              _____        _          _               
 | |     (_)| |          (_)      |  _ \               | |            / ____|      | |        | |              
 | |      _ | |__   _ __  _       | |_) |  ___    ___  | | __        | |      __ _ | |_  __ _ | |  ___    __ _ 
 | |     | || '_ \ | '__|| |      |  _ <  / _ \  / _ \ | |/ /        | |     / _` || __|/ _` || | / _ \  / _` |
 | |____ | || |_) || |   | |      | |_) || (_) || (_) ||   <         | |____| (_| || |_| (_| || || (_) || (_| |
 |______||_||_.__/ |_|   |_|      |____/  \___/  \___/ |_|\_\         \_____|\__,_| \__|\__,_||_| \___/  \__, |
                                                                                                          __/ |
                                                                                                         |___/ ";
    Console.WriteLine(logo);
}