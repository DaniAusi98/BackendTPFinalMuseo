
namespace Core.Domain.Entities
{
    public abstract class DomainEntity<TKey>
    {
        public TKey Id { get; protected set; }

        protected DomainEntity()
        {
        }

        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default!);
        }
    }

    public abstract class DomainEntity : DomainEntity<string>
    {
    }
}
