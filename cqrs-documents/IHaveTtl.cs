using System;

namespace cqrs_documents
{
    public interface IHaveTtl
    {
        DateTimeOffset expiry { get; set; }
    }
}