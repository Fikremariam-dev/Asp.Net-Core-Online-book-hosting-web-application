using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public int  userId { set; get; }
        [Required]
        public int BookID { set; get; }
        [Required]
        public int value { set; get; }
    }
}
