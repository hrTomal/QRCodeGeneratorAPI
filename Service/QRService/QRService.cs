using QRCodeGeneratorAPI.Models;
using System.Drawing.Imaging;
using System.Drawing;
using QRCoder;
using Interface.IService;

namespace Service.QRService
{
    public class QRService : IQRService
    {
        public string CreateQRCode(QRCodeModel qRCode)
        {
            try
            {
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.QRCodeText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = qrCode.GetGraphic(60);

                byte[] BitmapArray;
                using (MemoryStream ms = new MemoryStream())
                {
                    QrBitmap.Save(ms, ImageFormat.Png);
                    BitmapArray = ms.ToArray();
                }

                string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                return QrUri;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
