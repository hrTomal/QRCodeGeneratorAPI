using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Interface.IService
{
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(string base64image, string exactFileName, string imageFolder);
    }
}
