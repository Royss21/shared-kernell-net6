
namespace SharedKernell.Pagination
{
    using SharedKernell.Enum;
    using Microsoft.EntityFrameworkCore.Query;
    using System.Linq.Expressions;

    public class PaginationParameters<T> where T : class
    {
        public LambdaExpression OrderColumn { get; set; }
        public SortTypeEnum SortType { get; set; }
        public int Start { get; set; }
        public int RowsCount { get; set; }
        public bool PaginationIgnore { get; set; }
        public Expression<Func<T, bool>> FilterWhere { get; set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Include { get; set; }
    }
}