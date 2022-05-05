using System;
using System.Collections.Generic;
using System.Text;

namespace Starter.Data.Interfaces
{
    public interface IAuditable<T>
    {
        T CreatedById { get; set; }
        DateTimeOffset CreatedOn { get; set; }
        T UpdatedById { get; set; }
        DateTimeOffset UpdatedOn { get; set; }
    }
}
