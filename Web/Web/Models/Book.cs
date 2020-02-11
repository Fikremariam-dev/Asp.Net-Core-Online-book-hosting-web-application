using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Book
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public string Tags { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string PhoneNo { get; set; }
    }
}
