using System.ComponentModel.DataAnnotations;

namespace QRCodeGeneratorAPI.Models
{
    public class DynamicImageWIthQRModel
    {
        public string JobID { get; set; }
        public string JobTitle { get; set; }
        public List<ImageInformation> ImageInformation { get; set; }
    }

    public class ImageInformation
    {
        public string JobURL { get; set; }
        public string LinkType { get; set; }
        public string? Deadline { get; set; }
        public bool isWithLinkedInText { get; set; }
        public string? CompanyLogoUrl { get; set; }
        //public string? LeftBottomLogoUrl { get; set; }
        public string? BackgroundImageUrl { get; set; }
        //public string? RightBottomImageUrl { get; set; }
        public int TotalImageWidth { get; set; }
        public int TotalImageHeight { get; set; }
        public int CompanyLogoWidth { get; set; }
        public int CompanyLogoHeight { get; set; }
        public int LeftBottomLogoWidth { get; set; }
        public int LeftBottomLogoHeight { get; set; }
        public int RightBottomImageWidth { get; set; }
        public int RightBottomImageHeight { get; set; }
        public int JobTitleTextSize { get; set; }
        public int Padding { get; set; }
    }
}
