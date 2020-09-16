using Elk.Core;
using Elk.Http;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System;
using Pharmacy.Service.Resource;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.Service
{
    public class NotificationService : INotificationService
    {
        private IConfiguration _config { get; }

        public NotificationService(IConfiguration config)
        {
            _config = config;
        }


        public async Task<IResponse<bool>> NotifyAsync(NotificationDto notifyDto)
        {
            try
            {
                using var http = new HttpClient();
                var token = _config["CustomSettings:NotifierToken"];
                var url = _config["CustomSettings:NotifierUrl"];
                http.DefaultRequestHeaders.Add("Token", token);
                var notify = await http.PostAsync(url, new StringContent(notifyDto.SerializeToJson(), Encoding.UTF8, "application/json"));
                if (!notify.IsSuccessStatusCode) return new Response<bool> {Message = ServiceMessage.Error };
                return (await notify.Content.ReadAsStringAsync()).DeSerializeJson<Response<bool>>();
            }
            catch(Exception e)
            {
                FileLoger.Error(e);
                return new Response<bool> { Message = ServiceMessage.Error };
            }

        }

    }
}
