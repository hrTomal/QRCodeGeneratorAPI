using Interface.IService;
using Microsoft.AspNetCore.Mvc;
using QRCodeGeneratorAPI.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QRCodeGeneratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DynamicImageController : ControllerBase
    {
        private readonly IDynamicImageService _dynamicImageService;
        public DynamicImageController(IDynamicImageService dynamicImageService)
        {
            _dynamicImageService = dynamicImageService;
        }


        //[HttpPost]
        //[Route("CreateDynamicImage")]
        //public IActionResult CreateDynamicImage(DynamicImageModel dynamicImage)
        //{
        //    try
        //    {
        //        var returnData = _dynamicImageService.CreateDynamicImage(dynamicImage);
        //        return Ok(new ApiResponse<object>(returnData));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse<string>(ex.Message));
        //    }
        //}
        
        [HttpPost]
        [Route("CreateDynamicImageWithQR")]
        public async Task<IActionResult> CreateDynamicImageWithQR(DynamicImageWIthQRModel dynamicImage)
        {
            try
            {
                var returnData = await _dynamicImageService.CreateDynamicImageWithQR(dynamicImage);
                if(returnData != null)
                {
                    return Ok(new ApiResponse<object>(returnData));
                }
                return BadRequest(new ApiResponse<string>("Check The Link Types, It must be LinkedIn or RegularApply or JobDetails ")); ;                
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }
    }
}
