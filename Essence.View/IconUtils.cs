using System.Drawing;

namespace Essence.View
{
    public static class IconUtils
    {
        public static Image ToImage(this Icon icon, Size iconSize)
        {
            return null;
        }

        public static Image ToImage(this System.Drawing.Icon icon, Size iconSize)
        {
            return icon.ToBitmap();
        }
    }
}