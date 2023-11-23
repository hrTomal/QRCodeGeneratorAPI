﻿using Model.DynamicImage;
using QRCodeGeneratorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.IService
{
    public interface IDynamicImageService
    {
        string CreateDynamicImage(DynamicImageModel dynamicImageModel);
        Task<IEnumerable<DynamicImageWithQRResponseModel>> CreateDynamicImageWithQR(DynamicImageWIthQRModel dynamicImageModel);
    }
}
