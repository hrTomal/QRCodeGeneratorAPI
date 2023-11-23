using Microsoft.AspNetCore.Mvc;
using QRCodeGeneratorAPI.Models;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using Interface.IService;

namespace QRCodeGeneratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRService _qRService;
        public QRCodeController(IQRService qRService) 
        {
            _qRService = qRService;
        }

        [HttpPost(Name = "CreateQRCode")]
        public IActionResult CreateQRCode(QRCodeModel qRCode)
        {
            try
            {
                var qrResult = _qRService.CreateQRCode(qRCode);
                var returnData = new
                {
                    Image = qrResult,
                };
                return Ok(new ApiResponse<object>(returnData));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
            
            
        }
    }
}
