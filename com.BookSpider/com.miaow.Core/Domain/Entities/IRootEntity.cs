namespace com.miaow.Core.Domain.Entities
{
    public interface IRootEntity<TPrimaryKey> : IEntity<TPrimaryKey> { }
    public interface IRootEntity : IRootEntity<int> { }
}