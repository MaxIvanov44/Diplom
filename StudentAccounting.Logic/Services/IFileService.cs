using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace StudentAccounting.Logic.Services
{
    public interface IFileService
    {
        byte[] ImageToBytes(IFormFile image);
        Image ImageFromBytes(byte[] bytes);
    }
}
