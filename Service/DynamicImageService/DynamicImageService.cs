using QRCodeGeneratorAPI.Models;
using System.Drawing;
using System.Net;
using System.Drawing.Imaging;
using Interface.IService;
using Model.DynamicImage;
using System.Globalization;

namespace Service.DynamicImageService
{
    public class DynamicImageService : IDynamicImageService
    {
        private readonly IQRService _qRService;
        private readonly IImageUploadService _imageUploadService;
        public DynamicImageService(IQRService qRService, IImageUploadService imageUploadService)
        {
            _qRService = qRService;
            _imageUploadService = imageUploadService;

        }
        public string CreateDynamicImage(DynamicImageModel dynamicImage)
        {
            try
            {
                dynamicImage = SetDefaultValuesForDynamicImage(dynamicImage);

                // Background image
                Image backgroundImage = LoadImageFromUrl(dynamicImage.BackgroundImageUrl);

                // Logo image
                Image topLogoImage = LoadImageFromUrl(dynamicImage.LeftTopLogoUrl);

                // bottom Logo image
                Image logoImage = LoadImageFromUrl(dynamicImage.LeftBottomLogoUrl);

                // Logo image
                Image qrImage = LoadImageFromUrl(dynamicImage.RightBottomImageUrl);

                // Text
                string text = dynamicImage.JobTitle;
                Font font = new Font("Arial", dynamicImage.JobTitleTextSize, FontStyle.Bold);
                Color textColor = ColorTranslator.FromHtml("#3A47B0"); // Convert hex color to Color object
                Brush textBrush = new SolidBrush(textColor);

                int totalWidth = dynamicImage.TotalImageWidth;
                int totalHeight = dynamicImage.TotalImageHeight;

                var qrImageHeight = dynamicImage.RightBottomImageHeight;
                var qrImageWidth = dynamicImage.RightBottomImageWidth;

                var leftTopImageHeight = dynamicImage.LeftTopLogoHeight;
                var leftTopImageWidth = dynamicImage.LeftTopLogoWidth;

                var leftBottomImageHeight = dynamicImage.LeftBottomLogoHeight;
                var leftBottomImageWidth = dynamicImage.LeftBottomLogoWidth;

                // Create a new image with the same dimensions as the background
                using (Bitmap resultImage = new Bitmap(totalWidth, totalHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.Clear(Color.White);

                        var padding = dynamicImage.Padding;

                        // Draw the background image to cover the entire canvas
                        using (ImageAttributes attributes = new ImageAttributes())
                        {
                            // Set the opacity (0.5 for 50% transparency, adjust as needed)
                            attributes.SetColorMatrix(new ColorMatrix { Matrix33 = 1.0f });
                            graphics.DrawImage(backgroundImage, new Rectangle(0, 0, totalWidth, totalHeight),
                                0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, attributes);
                        }

                        // Draw the logo in the left top corner
                        graphics.DrawImage(topLogoImage, new Rectangle(0 + padding, 0 + padding, leftTopImageWidth, leftTopImageHeight));

                        // Draw the image in the left bottom corner
                        //graphics.DrawImage(logoImage, new Rectangle(0 + padding, totalHeight - leftBottomImageHeight - padding, leftBottomImageWidth, leftBottomImageHeight));

                        // Draw the image in the right bottom corner
                        graphics.DrawImage(qrImage, new Rectangle(totalWidth - qrImageWidth - padding, totalHeight - qrImageHeight - padding, qrImageWidth, qrImageHeight));

                        // Draw the text in the right top corner
                        SizeF textSize = graphics.MeasureString(text, font);
                        float textX = resultImage.Width - textSize.Width - padding;
                        float textY = 100; //Top Padding
                        graphics.DrawString(text, font, textBrush, new PointF(textX, textY));
                    }

                    string base64String = ConvertImageToBase64(resultImage);
                    return base64String;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private DynamicImageModel SetDefaultValuesForDynamicImage(DynamicImageModel dynamicImage)
        {
            dynamicImage.TotalImageWidth = dynamicImage.TotalImageWidth == 0 ? 1200 : dynamicImage.TotalImageWidth;
            dynamicImage.TotalImageHeight = dynamicImage.TotalImageHeight == 0 ? 630 : dynamicImage.TotalImageHeight;
            dynamicImage.LeftTopLogoWidth = dynamicImage.LeftTopLogoWidth == 0 ? 410 : dynamicImage.LeftTopLogoWidth;
            dynamicImage.LeftTopLogoHeight = dynamicImage.LeftTopLogoHeight == 0 ? 270 : dynamicImage.LeftTopLogoHeight;

            dynamicImage.LeftBottomLogoWidth = dynamicImage.LeftBottomLogoWidth == 0 ? 250 : dynamicImage.LeftBottomLogoWidth; //not using
            dynamicImage.LeftBottomLogoHeight = dynamicImage.LeftBottomLogoHeight == 0 ? 100 : dynamicImage.LeftBottomLogoHeight; //not using

            dynamicImage.RightBottomImageWidth = dynamicImage.RightBottomImageWidth == 0 ? 190 : dynamicImage.RightBottomImageWidth; //QR
            dynamicImage.RightBottomImageHeight = dynamicImage.RightBottomImageHeight == 0 ? 225 : dynamicImage.RightBottomImageHeight; //QR

            dynamicImage.JobTitleTextSize = dynamicImage.JobTitleTextSize == 0 ? 30 : dynamicImage.JobTitleTextSize;
            dynamicImage.Padding = dynamicImage.Padding == 0 ? 60 : dynamicImage.Padding;

            string defaultBackgroundImageUrl;
            if (dynamicImage.isWithLinkedInText)
            {
                defaultBackgroundImageUrl = "https://static.ajkerdeal.com/ImageGeneration/test/BG-1200X630.jpg";
            }
            else
            {
                defaultBackgroundImageUrl = "https://static.ajkerdeal.com/ImageGeneration/test/BG-1200X630-B.jpg";
            }
            
            var defaultToplogoImageUrl = "https://static.ajkerdeal.com/ImageGeneration/test/BDJobs_details_logo.jpg";
            var defaultleftBottomLogoImageUrl = "https://bdjobs.com/images/logo.png";
            var defaultQRImageUrl = "https://cdn2.hubspot.net/hubfs/477837/Imported_Blog_Media/QR_Code-1.jpg";

            // Background image
            dynamicImage.BackgroundImageUrl = dynamicImage.BackgroundImageUrl == "" 
                                                    || dynamicImage.BackgroundImageUrl == "string"
                                                    || dynamicImage.BackgroundImageUrl == null 
                                                    ? defaultBackgroundImageUrl : dynamicImage.BackgroundImageUrl;

            // Logo image
            dynamicImage.LeftTopLogoUrl = dynamicImage.LeftTopLogoUrl == "" 
                                                    || dynamicImage.LeftTopLogoUrl == "string"
                                                    || dynamicImage.LeftTopLogoUrl == null
                                                    ? defaultToplogoImageUrl : dynamicImage.LeftTopLogoUrl;

            // Logo image
            dynamicImage.LeftBottomLogoUrl = dynamicImage.LeftBottomLogoUrl == "" 
                                                    || dynamicImage.LeftBottomLogoUrl == "string" 
                                                    || dynamicImage.LeftBottomLogoUrl == null 
                                                    ? defaultleftBottomLogoImageUrl : dynamicImage.LeftBottomLogoUrl;

            // Logo image
            dynamicImage.RightBottomImageUrl = dynamicImage.RightBottomImageUrl == "" 
                                                    || dynamicImage.RightBottomImageUrl == "string" 
                                                    || dynamicImage.RightBottomImageUrl == null
                                                    ? defaultQRImageUrl : dynamicImage.RightBottomImageUrl;

            return dynamicImage;
        }

        private Image LoadImageFromUrl(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        private string ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Save the image to the memory stream
                image.Save(ms, ImageFormat.Jpeg);

                // Convert the byte array to Base64
                byte[] imageBytes = ms.ToArray();

                return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));
            }
        }

        public async Task<IEnumerable<DynamicImageWithQRResponseModel>> CreateDynamicImageWithQR(DynamicImageWIthQRModel dynamicImage)
        {
            try
            {
                dynamicImage = SetDefaultValuesForDynamicImageWithQR(dynamicImage);

                var multipleImageInfo = dynamicImage.ImageInformation;
                List<DynamicImageWithQRResponseModel> imageWithQRResponseModels = new List<DynamicImageWithQRResponseModel>();
                QRCodeModel qRCodeModel = new QRCodeModel();
                Image qrImage;
                string base64QR;
                string uploadedQRImageUrl;
                string uploadQRFileName;
                string fileNameEndingText;
                string text;
                Font font;
                Color textColor;
                Color textColorDeadline;
                Brush textBrush;
                Brush textBrushScanToApply;
                Brush textBrushDeadline;
                float deadlinePadFromTitle ;
                foreach (var imageItem in multipleImageInfo)
                {
                    if (imageItem.LinkType.ToLower() == "LinkedIn".ToLower()) { fileNameEndingText = "LA"; }
                    else if (imageItem.LinkType.ToLower() == "RegularApply".ToLower()) { fileNameEndingText = "BA"; }
                    else if (imageItem.LinkType.ToLower() == "JobDetails".ToLower()) { fileNameEndingText = "JD"; }
                    else { return null; }

                    // Background image
                    Image backgroundImage = LoadImageFromUrl(imageItem.BackgroundImageUrl);
                    // Logo image
                    Image topLogoImage = LoadImageFromUrl(dynamicImage.CompanyLogoUrl);
                    // bottom Logo image
                    //Image logoImage = LoadImageFromUrl(imageItem.LeftBottomLogoUrl);
                    // QR image
                    //Image qrImage = LoadImageFromUrl(imageItem.RightBottomImageUrl);

                    //QRCodeModel qRCodeModel = new QRCodeModel();
                    qRCodeModel.QRCodeText = imageItem.JobURL;
                    base64QR = _qRService.CreateQRCode(qRCodeModel);
                    qrImage = ConvertBase64ToImage(base64QR);

                    uploadQRFileName = $"Q_{dynamicImage.JobID}_{fileNameEndingText}";
                    uploadedQRImageUrl = await _imageUploadService.UploadImageAsync(base64QR, uploadQRFileName, "QR");

                    // Text
                    text = dynamicImage.JobTitle;
                    font = new Font("Arial", imageItem.JobTitleTextSize, FontStyle.Bold);
                    textColor = ColorTranslator.FromHtml("#3A47B0"); // Convert hex color to Color object
                    textBrush = new SolidBrush(textColor);
                    textBrushScanToApply = new SolidBrush(Color.Black);
                    
                    // Create a new image with the same dimensions as the background
                    using (Bitmap resultImage = new Bitmap(imageItem.TotalImageWidth, imageItem.TotalImageHeight))
                    {
                        using (Graphics graphics = Graphics.FromImage(resultImage))
                        {
                            graphics.Clear(Color.White);

                            var padding = imageItem.Padding;

                            using (ImageAttributes attributes = new ImageAttributes())
                            {
                                attributes.SetColorMatrix(new ColorMatrix { Matrix33 = 1.0f });
                                graphics.DrawImage(backgroundImage, new Rectangle(0, 0, imageItem.TotalImageWidth, imageItem.TotalImageHeight),
                                    0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, attributes);
                            }

                            // Draw the logo in the left top corner
                            graphics.DrawImage(topLogoImage, new Rectangle(0 + padding, 0 + padding, imageItem.CompanyLogoWidth, imageItem.CompanyLogoHeight));

                            // Draw the image in the left bottom corner
                            //graphics.DrawImage(logoImage, new Rectangle(0 + padding, dynamicImage.TotalImageHeight - dynamicImage.LeftBottomLogoHeight - padding, dynamicImage.LeftBottomLogoWidth, leftBottomImageHeight));

                            // Draw the image in the right bottom corner QR
                            Rectangle qrRectangle = new Rectangle(
                                imageItem.TotalImageWidth - imageItem.RightBottomImageWidth - padding - 10,
                                imageItem.TotalImageHeight - imageItem.RightBottomImageHeight - padding,
                                imageItem.RightBottomImageWidth,
                                imageItem.RightBottomImageHeight - 35);

                            graphics.DrawImage(qrImage, qrRectangle); //QR Image

                            // Draw the text "Scan To Apply" under qrImage
                            string scanToApplyText = "Scan To Apply";
                            Font scanToApplyFont = new Font("Arial", 17, FontStyle.Bold); // Adjust font size as needed
                            SizeF scanToApplyTextSize = graphics.MeasureString(scanToApplyText, scanToApplyFont);
                            float scanToApplyTextX = imageItem.TotalImageWidth - padding - imageItem.RightBottomImageWidth + 12 - 10;
                            float scanToApplyTextY = imageItem.TotalImageHeight - padding - 37; // Adjust the vertical position
                            graphics.DrawString(scanToApplyText, scanToApplyFont, textBrushScanToApply, new PointF(scanToApplyTextX, scanToApplyTextY));

                            //For QR Image and text "Scan To Apply" Border
                            Rectangle combinedRectangle = new Rectangle(
                                imageItem.TotalImageWidth - imageItem.RightBottomImageWidth - padding - 10,
                                imageItem.TotalImageHeight - imageItem.RightBottomImageHeight - padding,
                                imageItem.RightBottomImageWidth,
                                imageItem.RightBottomImageHeight);

                            // Draw a rectangle border around the QR code
                            Pen borderPen = new Pen(Color.Gray, 2);
                            graphics.DrawRectangle(borderPen, combinedRectangle);


                            float maxJobTitleWidth = 620;
                            float maxJobTitleHeight = 270;

                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Far; // Right align
                            format.LineAlignment = StringAlignment.Near; // Top align

                            // Draw the text in the right top corner
                            SizeF jobTitleTextSize = graphics.MeasureString(text, font);
                            if (jobTitleTextSize.Width > 620) //For 2 line text
                            {
                                RectangleF rectF1 = new RectangleF(imageItem.TotalImageWidth - padding - maxJobTitleWidth, padding + (padding/2),
                                                                        maxJobTitleWidth, maxJobTitleHeight);
                                
                                graphics.DrawString(text, font, textBrush, rectF1, format);

                            }
                            else //for 1 line text
                            {
                                float textX = resultImage.Width - jobTitleTextSize.Width - padding;
                                float textY = padding * 2; //Top Padding
                                graphics.DrawString(text, font, textBrush, new PointF(textX, textY));

                            }

                            if (!string.IsNullOrEmpty(imageItem.Deadline))
                            {

                                // Convert the deadline string to DateTime
                                DateTime deadlineDate = DateTime.ParseExact(imageItem.Deadline, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                                // Format the deadlineDate to the desired format
                                string formattedDeadline = deadlineDate.ToString("dd MMM yyyy");

                                // Draw the text "Scan To Apply" under qrImage
                                textColorDeadline = ColorTranslator.FromHtml("#D33E27");
                                textBrushDeadline = new SolidBrush(textColorDeadline);
                                string deadlineText = $"Deadline : {formattedDeadline}";
                                Font deadlineFont = new Font("Arial", 20, FontStyle.Bold); // Adjust font size as needed
                                SizeF deadlineTextSize = graphics.MeasureString(deadlineText, deadlineFont);
                                float deadlineTextX = imageItem.TotalImageWidth - padding - deadlineTextSize.Width - 15;
                                float deadlineTextY = 220; // Adjust the vertical position
                                graphics.DrawString(deadlineText, deadlineFont, textBrushDeadline, new PointF(deadlineTextX, deadlineTextY));
                            }
                        }

                        string base64String = ConvertImageToBase64(resultImage);
                        //upload Image
                        var uploadImageFileName = $"S_{dynamicImage.JobID}_{fileNameEndingText}";
                        var uploadedDynamicImageUrl = await _imageUploadService.UploadImageAsync(base64String, uploadImageFileName, "SharedImage");

                        DynamicImageWithQRResponseModel responseModel = new DynamicImageWithQRResponseModel();
                        responseModel.QRImageUrl = uploadedQRImageUrl;
                        responseModel.DynamicImageUrl = uploadedDynamicImageUrl;

                        imageWithQRResponseModels.Add(responseModel);
                    }
                }
                return imageWithQRResponseModels;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private DynamicImageWIthQRModel SetDefaultValuesForDynamicImageWithQR(DynamicImageWIthQRModel dynamicImage)
        {
            var imageInfo = dynamicImage.ImageInformation;

            foreach(var imageItem in imageInfo)
            {
                imageItem.TotalImageWidth = imageItem.TotalImageWidth == 0 ? 1200 : imageItem.TotalImageWidth;
                imageItem.TotalImageHeight = imageItem.TotalImageHeight == 0 ? 630 : imageItem.TotalImageHeight;
                imageItem.CompanyLogoWidth = imageItem.CompanyLogoWidth == 0 ? 410 : imageItem.CompanyLogoWidth;
                imageItem.CompanyLogoHeight = imageItem.CompanyLogoHeight == 0 ? 270 : imageItem.CompanyLogoHeight;

                imageItem.LeftBottomLogoWidth = imageItem.LeftBottomLogoWidth == 0 ? 250 : imageItem.LeftBottomLogoWidth; //not using
                imageItem.LeftBottomLogoHeight = imageItem.LeftBottomLogoHeight == 0 ? 100 : imageItem.LeftBottomLogoHeight; //not using

                imageItem.RightBottomImageWidth = imageItem.RightBottomImageWidth == 0 ? 190 : imageItem.RightBottomImageWidth; //QR
                imageItem.RightBottomImageHeight = imageItem.RightBottomImageHeight == 0 ? 225 : imageItem.RightBottomImageHeight; //QR

                imageItem.JobTitleTextSize = imageItem.JobTitleTextSize == 0 ? 32 : imageItem.JobTitleTextSize;
                imageItem.Padding = imageItem.Padding == 0 ? 60 : imageItem.Padding;

                string defaultBackgroundImageUrl;
                if (imageItem.isWithLinkedInText)
                {
                    defaultBackgroundImageUrl = "https://static.ajkerdeal.com/ImageGeneration/test/BG-1200X630.jpg";
                }
                else
                {
                    defaultBackgroundImageUrl = "https://static.ajkerdeal.com/ImageGeneration/test/BG-1200X630-B.jpg";
                }

                var defaultToplogoImageUrl = "https://static.ajkerdeal.com/ImageGeneration/test/BDJobs_details_logo.jpg";
                var defaultleftBottomLogoImageUrl = "https://bdjobs.com/images/logo.png";
                var defaultQRImageUrl = "https://cdn2.hubspot.net/hubfs/477837/Imported_Blog_Media/QR_Code-1.jpg";


                // Background image
                imageItem.BackgroundImageUrl = imageItem.BackgroundImageUrl == ""
                                                        || imageItem.BackgroundImageUrl == "string"
                                                        || imageItem.BackgroundImageUrl == null
                                                        ? defaultBackgroundImageUrl : imageItem.BackgroundImageUrl;

                // Logo image
                dynamicImage.CompanyLogoUrl = dynamicImage.CompanyLogoUrl == ""
                                                        || dynamicImage.CompanyLogoUrl == "string"
                                                        || dynamicImage.CompanyLogoUrl == null
                                                        ? defaultToplogoImageUrl : dynamicImage.CompanyLogoUrl;

                // Logo image
                //dynamicImage.LeftBottomLogoUrl = dynamicImage.LeftBottomLogoUrl == ""
                //                                        || dynamicImage.LeftBottomLogoUrl == "string"
                //                                        || dynamicImage.LeftBottomLogoUrl == null
                //                                        ? defaultleftBottomLogoImageUrl : dynamicImage.LeftBottomLogoUrl;

                // Logo image
                //dynamicImage.ImageInformation.RightBottomImageUrl = dynamicImage.RightBottomImageUrl == ""
                //                                        || dynamicImage.RightBottomImageUrl == "string"
                //                                        || dynamicImage.RightBottomImageUrl == null
                //                                        ? defaultQRImageUrl : dynamicImage.RightBottomImageUrl;

            }
            dynamicImage.ImageInformation = imageInfo;
            return dynamicImage;
        }

        static Image ConvertBase64ToImage(string base64String)
        {
            try
            {
                // Extract the base64 data from the string (remove the "data:image/png;base64," part)
                string base64Data = base64String.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting base64 to Image: " + ex.Message);
                return null; // or throw an exception depending on your use case
            }
        }



    }
}
