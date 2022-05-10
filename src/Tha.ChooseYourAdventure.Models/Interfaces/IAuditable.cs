using System;
using System.Collections.Generic;
using System.Text;

namespace Tha.ChooseYourAdventure.Data.Interfaces
{
    public interface IAuditable<T> : IAuditable
    {
        T CreatedById { get; set; }
        T UpdatedById { get; set; }
    }

    public interface IAuditable
    {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset UpdatedOn { get; set; }
    }
}
