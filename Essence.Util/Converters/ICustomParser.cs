using System;

namespace Essence.Util.Converters
{
    public interface ICustomParser
    {
        bool TryParse(string text, string format, IFormatProvider formatProvider, out object result);
    }
}