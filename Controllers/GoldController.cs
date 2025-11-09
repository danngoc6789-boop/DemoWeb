using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DemoWeb.Models;
using DemoWeb.Services;

namespace DemoWeb.Controllers
{
    public class GoldController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public async Task<ActionResult> UpdatePrices()
        {
            var service = new GoldBtmcService();
            var goldList = await service.FetchAsync();

            // Lưu vào DB
            db.GoldPrices.AddRange(goldList);
            db.SaveChanges();

            return Content("Cập nhật giá vàng thành công!");
        }

        public ActionResult Index()
        {
            var list = db.GoldPrices.OrderByDescending(g => g.Timestamp).ToList();
            return View(list);
        }
    }
}
