using System;

namespace com.miaow.Core.Domain.Entities
{
    public interface IUpdateDateTime
    {
        DateTime LastUpdateTime { get; set; }
    }
}