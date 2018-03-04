using System;
using System.Collections.Generic;

namespace com.miaow.Core.Domain.Entities
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey))) return true;
            if (typeof(TPrimaryKey) == typeof(int)) return Convert.ToInt32(Id) <= 0;
            if (typeof(TPrimaryKey) == typeof(long)) return Convert.ToInt64(Id) <= 0;
            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TPrimaryKey>)) return false;

            if (ReferenceEquals(this, obj)) return true;

            var other = (Entity<TPrimaryKey>) obj;
            if (IsTransient() && other.IsTransient()) return false;

            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }
            
            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }

    public abstract class Entity : Entity<int>
    {
    }

}