using Pharmacy.Domain;
using System.Threading.Tasks;
using Pharmacy.Notifier.Service;
using Pharmacy.Notifier.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Notifier.Controllers
{
    public class NotificationController : Controller
    {
        public INotificationService _notificationService { get; }

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpGet]
        public IActionResult Index()
            => Ok("WellCome To Pharmacy.Notifier Api ...");

        [HttpPost, AuthenticationFilter]
        public async Task<IActionResult> AddAsync([FromBody] NotificationDto notifyDto, Application application)
            => Ok(await _notificationService.AddAsync(notifyDto, application.ApplicationId));

    }
}