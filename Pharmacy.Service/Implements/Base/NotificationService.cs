using Elk.Core;
using Elk.Http;
using Pharmacy.Domain;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System;
using Pharmacy.Service.Resource;

namespace Pharmacy.Service
{
    public class NotificationService : INotificationService
    {
        private IConfiguration _configuration { get; }

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<IResponse<bool>> NotifyAsync(NotificationDto notifyDto)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("Token", _configuration["CustomSettings:NotifierToken"]);
                var notify = await http.PostAsync(_configuration["CustomSettings:NotifierUrl"], new StringContent(notifyDto.SerializeToJson(), Encoding.UTF8, "application/json"));
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
