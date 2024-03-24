namespace Domain.Core
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; protected set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public void SetId(TKey id)
        {
            Id = id;
        }

    }


}
