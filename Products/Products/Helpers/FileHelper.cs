namespace Products.Helpers
{
    using System.IO;

    public class FilesHelper
    {
        /// <summary>
        /// Metodo que recibe un objeto Stream y lo conviente en un Array bite[]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}