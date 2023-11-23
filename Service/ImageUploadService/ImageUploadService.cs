using Interface.IService;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Model.Utility;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Drawing.Drawing2D;


namespace Service.ImageUploadService
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly GoogleCredential googleCredential;
        private readonly StorageClient storageClient;
        private readonly string bucketName;
        private readonly GoogleBucket googleBucket;

        public ImageUploadService()
        {
            googleBucket = new GoogleBucket();
            var credentialJson = new
            {
                type = googleBucket.Type,
                project_id = googleBucket.ProjectId,
                private_key_id = googleBucket.PrivateKeyId,
                private_key = googleBucket.PrivateKey,
                client_email = googleBucket.ClientEmail,
                client_id = googleBucket.ClientId,
                auth_uri = googleBucket.AuthUri,
                token_uri = googleBucket.TokenUri,
                auth_provider_x509_cert_url = googleBucket.AuthProviderCertUrl,
                client_x509_cert_url = googleBucket.ClientCertUrl,
                universe_domain = googleBucket.UniverseDomain
            };
            var googleCredential = GoogleCredential.FromJson(JsonConvert.SerializeObject(credentialJson));
            storageClient = StorageClient.Create(googleCredential);
            bucketName = "bdjobs";
        }


        public async Task<string> UploadImageAsync(string base64image, string exactFileName, string imageFolder)
        {
            if (base64image == null)
            {
                return "Image File cannot be null.";
            }

            try
            {
                Image image = ConvertBase64ToImage(base64image);
                using (MemoryStream memStream = new MemoryStream())
                {
                    Image scaledImage = new Bitmap(image, image.Width, image.Height);
                    scaledImage.Save(memStream, ImageFormat.Jpeg);

                    //string exactFileName = $"S_{jobId}_JD";
                    string fileName = $"corporate/{imageFolder}/jobs/{exactFileName}.jpeg"; //SharedImage

                    var dataObject = await storageClient.UploadObjectAsync(bucketName, fileName, null, memStream);
                    string str = dataObject.MediaLink;
                    var strUrl = "https://storage.googleapis.com/bdjobs/" + fileName;
                    return strUrl;
                }
            }
            catch (Exception ex)
            {
                return $"Error uploading image: {ex.Message}";
            }
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
                return null; 
            }
        }


        private Image ScaleByPercent(Image save_Image)
        {
            try
            {
                var resized = new Bitmap(save_Image, new Size(256, 256));
                int sourceWidth = save_Image.Width;
                int sourceHeight = save_Image.Height;
                int sourceX = 0;
                int sourceY = 0;
                int bestX = 0;
                int bestY = 0;
                int bestWidth = 180;
                int bestHeight = 200;

                double width;
                double height;
                double xPos;
                double yPos;

                Bitmap bmPhoto = new Bitmap(bestWidth, bestHeight, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(save_Image.HorizontalResolution, save_Image.VerticalResolution);
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(save_Image,
                    new Rectangle(bestX, bestY, bestWidth, bestHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
                grPhoto.Dispose();
                return bmPhoto;
            }
            catch
            {
                throw;
            }
        }

    }
}
