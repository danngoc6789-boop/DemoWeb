using System;
using System.Threading;
using System.Threading.Tasks;
using DemoWeb.Services;

namespace DemoWeb.Services
{
    public class GoldUpdater
    {
        private static Timer _timer;

        public static void Start()
        {
            _timer = new Timer(async _ =>
            {
                try
                {
                    var service = new GoldBtmcService();
                    var goldList = await service.FetchAsync(); // <-- KHÔNG truyền apiUrl

                    // TODO: Xử lý goldList và lưu vào DB
                    Console.WriteLine("Đã cập nhật giá vàng: " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi cập nhật giá vàng: " + ex.Message);
                }
            }, null, 0, 5 * 60 * 1000); // chạy 5 phút/lần
        }


        public static void Stop()
        {
            _timer?.Dispose();
        }
    }
}
