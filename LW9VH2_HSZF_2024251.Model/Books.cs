using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace LW9VH2_HSZF_2024251.Model
{
  public class Books{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        //public int AuthorID { get; set; } ;   
        public Authors Author { get; set; } = null!;

  
        //public  int CategoryID { get; set; } 
        public Categories Category { get; set; } = null!;
        public double Price { get; set; }
        [Required]
        public int Publication_Year { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        public int Pages { get; set; }

        public Books()
        {
            Author = new Authors();
            Category = new Categories();
        }

            public Books( string title,  double price, int publication_Year, string isbn, int pages, Authors autors, Categories categoryes ) 
        {
         
            Title = title;
            Author = autors;
            Category = categoryes;
            Price = price;
            Publication_Year = publication_Year;
            Isbn = isbn;
            Pages = pages;
        }
    }
}
