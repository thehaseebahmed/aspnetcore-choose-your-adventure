using System;
using System.Collections.Generic;
using System.Text;

namespace Starter.Data.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
