using System.ComponentModel.DataAnnotations;

namespace QRCodeGeneratorAPI.Models
{
    public class DynamicImageModel
    {
        
        public string JobTitle { get; set; }
        public bool isWithLinkedInText { get; set; }
        public string? LeftTopLogoUrl { get; set; }
        public string? LeftBottomLogoUrl { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? RightBottomImageUrl { get; set; }
        public int TotalImageWidth { get; set; }
        public int TotalImageHeight { get; set; }
        public int LeftTopLogoWidth { get; set; }
        public int LeftTopLogoHeight { get; set; }
        public int LeftBottomLogoWidth { get; set; }
        public int LeftBottomLogoHeight { get; set; }
        public int RightBottomImageWidth { get; set; }
        public int RightBottomImageHeight { get; set; }
        public int JobTitleTextSize { get; set; }
        public int Padding { get; set; }
    }
}
