using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace com.miaow.Core.Domain.Entities
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public abstract TPrimaryKey Id { get; set; }
    }

    public abstract class Entity : Entity<int>
    {
    }

}