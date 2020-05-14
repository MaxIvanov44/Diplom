using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace StudentAccounting.Logic.Services.Implementations
{
    public class FileService : IFileService
    {
        public Image ImageFromBytes(byte[] bytes)
        {
            if (bytes.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    var image = Image.FromStream(ms);
                    return image;
                }
            }
            return null;
        }

        public byte[] ImageToBytes(IFormFile image)
        {
            if (image.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    return ms.ToArray();
                }   
            }
            return null;
        }

    }
}
