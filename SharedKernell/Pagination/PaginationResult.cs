﻿namespace SharedKernell.Pagination
{
    public class PaginationResult<T> where T : class
    {
        public int Count { get; set; }
        public IQueryable<T> Entities { get; set; }
    }
}