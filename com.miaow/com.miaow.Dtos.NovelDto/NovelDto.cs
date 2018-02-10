using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.miaow.Dtos.NovelDto
{
    public class NovelDto
    {
        public int Id { get; set; }
        
        public string NovelName { get; set; }
        
        public string MenuUrl { get; set; }

        public int MaxChapterIndex { get; set; }
    }
}
