using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Entities;

namespace com.BookSpider.Model
{
    [Serializable]
    public class MenuItemInfo : Entity,ICreateDateTime,IUpdateDateTime
    {
        public MenuItemInfo()
        {
            CreatedDateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        [StringLength(200)]
        public string Url { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        public int BookId { get; set; }
        public virtual BookInfo Book { get; set; }
        public int SortId { get; set; }
        [MaxLength]
        public string Context { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
