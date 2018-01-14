using System.Collections.Generic;
using com.miaow.Core.Domain.Entities;

namespace com.BookSpider.Model
{
    public class BookInfo : Entity
    {
        public string BookName { get; set; }
        public string MenuUrl { get; set; }
        public List<MenuItemInfo> MenuList { get; set; }
    }
}