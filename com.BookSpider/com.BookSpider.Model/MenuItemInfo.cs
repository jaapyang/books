using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Entities;

namespace com.BookSpider.Model
{
    public class MenuItemInfo:Entity
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
        public virtual  BookInfo Book { get; set; }
        public int SortId { get; set; }
    }
}
