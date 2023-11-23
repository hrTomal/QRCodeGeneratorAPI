using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace QRCodeGeneratorAPI.Models
{
    public class QRCodeModel
    {
        [Display(Name = "Enter QRCode Text")]
        public string QRCodeText { get; set; }
    }
}
