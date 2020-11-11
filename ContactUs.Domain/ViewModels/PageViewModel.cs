using System;

namespace ConnectUs.Domain.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public PageViewModel(int itemsCount, int pageNumer, int pageSize)
        {
            PageNumber = pageNumer;
            TotalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }
}
