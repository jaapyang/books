namespace com.miaow.Core.Domain.Entities
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }

    public abstract class Entity : Entity<int> { }

}