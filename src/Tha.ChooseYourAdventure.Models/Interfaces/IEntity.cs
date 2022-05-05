using System;
using System.Collections.Generic;
using System.Text;

namespace Tha.ChooseYourAdventure.Data.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
