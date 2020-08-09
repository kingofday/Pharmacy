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
using Microsoft.Extensions.Options;

namespace Pharmacy.Service
{
    public class NotificationService : INotificationService
    {
        private IOptions<CustomSetting> _settings { get; }

        public NotificationService(IOptions<CustomSetting> settings)
        {
            _settings = settings;
        }


        public async Task<IResponse<bool>> NotifyAsync(NotificationDto notifyDto)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("Token", _settings.Value.NotifierToken);
                var notify = await http.PostAsync(_settings.Value.NotifierUrl, new StringContent(notifyDto.SerializeToJson(), Encoding.UTF8, "application/json"));
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
