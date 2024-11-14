using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LW9VH2_HSZF_2024251.Model
{
     public class Authors
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public int Birth_year { get; set; }
        [Required]
        public virtual ICollection<Books> Books { get; set; }
        [Required]
        public string Nationality { get; set; }= String.Empty;

        public Authors()
        {
            Name = "NaN";
            Books = new HashSet<Books>();
        }

        public Authors( string name, int birth_year,  string nationality)
        {
           
            Name = name;
            Birth_year = birth_year;
            Books = new HashSet<Books>();
            Nationality = nationality;
        }
    }
}
