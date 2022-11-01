namespace SharedKernell.BaseEntity
{
    public abstract class BaseEntity<TId>
    {
        public virtual TId Id { get; set; }
    }
}