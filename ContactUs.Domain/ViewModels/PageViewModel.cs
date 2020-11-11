using System;

namespace ConnectUs.Domain.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel(int itemsCount, int pageNumer, int pageSize)
        {
            PageNumber = pageNumer;
            TotalPages = (int) Math.Ceiling(itemsCount / (double) pageSize);
        }

        public int PageNumber { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}