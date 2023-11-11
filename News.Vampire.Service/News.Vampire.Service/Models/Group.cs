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
         public required string Name { get; set; }

        public virtual ICollection<Source>? Sources { get; set; }

        [DefaultValue(true)]
        public required bool isActive { get; set; }
    }
}
