namespace Traffic.Data.Interfaces
{
    public abstract class DomainEntity<T>
    {
        public T Id { get; set; }
    }
}
