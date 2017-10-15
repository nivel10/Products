﻿namespace Products.API.Helpers
{
    using System.IO;
    using System.Web;

    /// <summary>
    /// Metodo que lamacena una imagen en la carpeta /Content/Images/
    /// </summary>
    public class FilesHelper
    {
        public static bool UploadPhoto(MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
                var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}