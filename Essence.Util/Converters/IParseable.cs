using System;

namespace Essence.Util.Converters
{
    public interface IParseable
    {
        bool TryParse(string text, string format, IFormatProvider formatProvider);
    }
}