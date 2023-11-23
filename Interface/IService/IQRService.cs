using QRCodeGeneratorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IService
{
    public interface IQRService
    {
        string CreateQRCode(QRCodeModel qRCode);
    }
}
