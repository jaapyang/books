namespace com.miaow.Core.Domain.Entities
{
    public interface IEntity { }

    public interface IEntity<TPrimaryKey> : IEntity
    {
        TPrimaryKey Id { get; set; }
    }
}