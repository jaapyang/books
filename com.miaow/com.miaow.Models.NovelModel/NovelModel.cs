using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Models.NovelModel
{
    public class NovelModel:Entity<int>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [StringLength(50)]
        public string NovelName { get; set; }

        [StringLength(200)]
        public string MenuUrl { get; set; }
        
        public int MaxChapterIndex { get; set; }

        public virtual List<ChapterModel> Chapters { get; set; }

    }
}