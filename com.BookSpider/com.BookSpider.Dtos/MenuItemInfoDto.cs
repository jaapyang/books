﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.BookSpider.Dtos
{
    public class MenuItemInfoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int SortId { get; set; }
        public string Content { get; set; }
    }
}
