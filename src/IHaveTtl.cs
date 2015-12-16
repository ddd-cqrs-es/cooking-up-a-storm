using System;

namespace Restaurant
{
    public interface IHaveTtl
    {
        DateTimeOffset expiry { get; set; }
    }
}