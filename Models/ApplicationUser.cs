using Microsoft.AspNet.Identity.EntityFramework;

namespace DemoWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        // Các properties Email và UserName đã có sẵn từ IdentityUser
        // Bạn có thể thêm properties tùy chỉnh ở đây nếu cần
    }
}