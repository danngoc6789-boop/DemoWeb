using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWeb.Models
{
    [Table("Products")]
    public class Product
    {
       
        
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(200)]
            public string Name { get; set; }

            [StringLength(500)]
            public string Description { get; set; }

            [Required]
            public decimal Price { get; set; }

            [StringLength(500)]
            public string Image { get; set; }

            // Thêm trường để lưu nhiều ảnh (phân cách bằng dấu ;)
            [StringLength(2000)]
            public string Images { get; set; }

        public string Gender { get; set; }

        [NotMapped]
            public string ImagePath
            {
                get { return MainImage; }
            }


        [StringLength(50)]
            public string Type { get; set; }

            [StringLength(100)]
            public string Category { get; set; }

            public DateTime CreatedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public List<string> ImageList
        {
            get
            {
                if (string.IsNullOrEmpty(Images))
                    return new List<string>();

                return Images
                    .Replace(";~", ";")
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(img => img.Trim())
                    .Where(img => !string.IsNullOrWhiteSpace(img))
                    .Select(img => img.StartsWith("~") ? img : "~" + img)
                    .ToList();
            }
        }
        
        [NotMapped]
        public string MainImage
        {
            get
            {
                // 1. Ưu tiên cột Image (ảnh chính do admin chọn)
                if (!string.IsNullOrEmpty(Image))
                {
                    return Image.StartsWith("~") ? Image : "~" + Image;
                }

                // 2. Nếu không có Image, lấy ảnh đầu từ ImageList
                var firstImage = ImageList.FirstOrDefault();
                if (!string.IsNullOrEmpty(firstImage))
                    return firstImage;

                // 3. Cuối cùng dùng no-image
                return "~/Content/Images/no-image.jpg";
            }
        }

        // ← THÊM: Lấy tất cả ảnh (Image + ImageList) để hiện thumbnails
        [NotMapped]
        public List<string> AllImages
        {
            get
            {
                var allImages = new List<string>();

                // Thêm ảnh chính vào đầu
                if (!string.IsNullOrEmpty(Image))
                {
                    var mainImg = Image.StartsWith("~") ? Image : "~" + Image;
                    allImages.Add(mainImg);
                }

                // Thêm các ảnh phụ từ ImageList (không trùng với Image)
                var additionalImages = ImageList.Where(img => img != Image && img != "~" + Image);
                allImages.AddRange(additionalImages);

                return allImages;
            }
        }

    }
}