
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LW9VH2_HSZF_2024251.Model
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; } = String.Empty;
        [StringLength(500)]
        [Required]
        public string  Description { get; set; } = String.Empty;
        [Required]
        public virtual ICollection<Books> Books { get; set; }



        public Categories()
        {
                Books = new HashSet<Books>();
        }

        public Categories( string name, string description)
        {
        
            Name = name;
            Description = description;
        }
    }
}
