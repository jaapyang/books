using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.BookSpider.Model;

namespace com.BookSpider.DomainService
{
    public class BookDomainService:BookRepositoryBase<int,BookInfo>
    {
    }

    public class MenuItemDomainService : BookRepositoryBase<int, MenuItemInfo>
    {
    }
}
