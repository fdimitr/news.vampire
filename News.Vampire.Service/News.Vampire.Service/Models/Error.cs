using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace News.Vampire.Service.Models
{
    public class Error
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(512)]
        [Column(TypeName = "jsonb")]
        public required string Values { get; set; }
        public DateTime Created { get; set; }
    }
}
