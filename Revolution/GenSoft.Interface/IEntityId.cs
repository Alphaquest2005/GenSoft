using System;

namespace GenSoft.Interfaces
{
    
    public interface IEntityId
    {
        int Id { get; set; }
        DateTime EntryDateTime { get; }
    }
}