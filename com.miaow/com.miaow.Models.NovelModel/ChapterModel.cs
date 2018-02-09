using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Models.NovelModel
{
    public class ChapterModel:Entity<int>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(10000)]
        public string Content { get; set; }

        public int SortId { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        [Required]
        public int NovelId { get; set; }

        public virtual NovelModel Novel { get; set; }
    }
}
