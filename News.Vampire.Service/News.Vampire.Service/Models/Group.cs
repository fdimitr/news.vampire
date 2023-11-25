using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Vampire.Service.Models
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(64)]
        [Required]
         public string Name { get; set; }

        public virtual ICollection<Source>? Sources { get; set; }

        [DefaultValue(true)]
        [Required]
        public bool isActive { get; set; }
    }
}
