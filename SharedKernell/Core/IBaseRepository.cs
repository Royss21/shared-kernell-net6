
namespace SharedKernell.Core
{
    using FluentValidation;
    using FluentValidation.Results;
    using SharedKernell.Pagination;
    using System.Linq.Expressions;

    public interface IBaseRepository<TEntity, in TId> where TEntity : class
    {
        Task<TEntity> GetAsync(TId id);

        IQueryable<TEntity> All(bool @readonly = true);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = true);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void RemoveRange(IEnumerable<TEntity> entities);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task<ValidationResult> AddAsync(TEntity entity, params IValidator<TEntity>[] validaciones);

        Task<ValidationResult> AddAsync(TEntity entity, IValidator<TEntity> validation);

        Task<ValidationResult> AddRangeAsync(IEnumerable<TEntity> entities, IValidator<TEntity> validation);

        Task<ValidationResult> UpdateAsync(TEntity entity, params IValidator<TEntity>[] validaciones);

        Task<TEntity> AddAsync(TEntity entity);

        void Update(TEntity entity);

        Task<ValidationResult> UpdateAsync(TEntity entity, IValidator<TEntity> validation);

        Task<ValidationResult> DeleteAsync(TEntity entity, params IValidator<TEntity>[] validaciones);

        Task<ValidationResult> DeleteAsync(TEntity entity, IValidator<TEntity> validation);

        Task<ValidationResult> ValidateEntityAsync(TEntity entity, IValidator<TEntity> validation);

        Task<ValidationResult> ValidateEntityAsync(TEntity entity, IEnumerable<IValidator<TEntity>> validations);

        Task<IEnumerable<TDto>> RunSqlQuery<TDto>(string storeProcedure, object parameters = null);

        Task<ValidationResult> AddEntityAsync(TEntity entity, ValidationResult validationResultEntity);

        Task BulkDeleteAsync(IEnumerable<TEntity> entities, List<string> updateByProperties = null,
            bool preserveInsertOrder = false, bool setOutputIdentity = false);

        Task BulkInsertAsync(IEnumerable<TEntity> entities, List<string> updateByProperties = null,
            bool preserveInsertOrder = false, bool setOutputIdentity = false);

        Task BulkInsertOrUpdateAsync(IEnumerable<TEntity> entities, List<string> updateByProperties = null,
            bool preserveInsertOrder = false, bool setOutputIdentity = false);

        Task BulkInsertOrUpdateOrDeleteAsync(IEnumerable<TEntity> entities, List<string> updateByProperties = null,
            bool preserveInsertOrder = false, bool setOutputIdentity = false);

        Task<PaginationResult<TEntity>> FindAllPagingAsync(PaginationParameters<TEntity> parameters,
            bool @readonly = true);

    }
}
