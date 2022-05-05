using System;
using System.Collections.Generic;
using System.Text;

namespace Tha.ChooseYourAdventure.Library.Extensions
{
    public static class IStringExtensions
    {
        public static bool HasValue(this string value)
        {
            if (string.IsNullOrEmpty(value) ||
                string.IsNullOrWhiteSpace(value)
                ) { return false; }

            return true;
        }
    }
}
