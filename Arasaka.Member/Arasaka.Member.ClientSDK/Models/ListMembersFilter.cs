﻿namespace Arasaka.Member.ClientSDK.Models
{
    public class ListMembersFilter
    {
        const int maxPageSize = 30;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
