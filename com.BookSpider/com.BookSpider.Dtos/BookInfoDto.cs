using System.Collections.Generic;

namespace com.BookSpider.Dtos
{
    public class BookInfoDto
    {
        public string BookName { get; set; }
        public string MenuUrl { get; set; }
        public List<MenuItemInfoDto> MenuList { get; set; }
    }
}