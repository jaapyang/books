using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using com.miaow.Core.Domain.Entities;

namespace com.BookSpider.Model
{
    public class BookInfo : Entity, IRootEntity
    {
        [StringLength(200)]
        public string BookName { get; set; }
        [StringLength(200)]
        public string MenuUrl { get; set; }
        public List<MenuItemInfo> MenuList { get; set; }
    }
}