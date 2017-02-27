using System;
using System.IO;

namespace Essence.Math
{
    public static class FileUtils
    {
        /// <summary>
        /// Reemplaza la extension del fichero.
        /// </summary>
        /// <param name="fileInfo">Fichero.</param>
        /// <param name="newExt">Nueva extensión sin incluir el '.'.</param>
        /// <returns>Fichero con la extensión modificada.</returns>
        public static FileInfo ReplaceExtension(this FileInfo fileInfo, string newExt)
        {
            return new FileInfo(Path.ChangeExtension(fileInfo.ToString(), newExt));
        }
    }
}