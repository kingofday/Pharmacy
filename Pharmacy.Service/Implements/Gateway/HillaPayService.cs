using Elk.Core;
using Newtonsoft.Json;
using Pharmacy.Domain;
using System.Net.Http;
using Pharmacy.Service.Resource;
using System.Threading.Tasks;
using Pharmacy.DataAccess.Ef;
using System;

namespace Pharmacy.Service
{
    public class HillaPayService : IGatewayService
    {
        readonly IPaymentService _paymentSrv;
        readonly IPaymentRepo _paymentRepo;
        readonly AppUnitOfWork _appUow;
        public HillaPayService(AppUnitOfWork appUow, IPaymentService paymentSrv)
        {
            _paymentSrv = paymentSrv;
            _paymentRepo = appUow.PaymentRepo;
            _appUow = appUow;
        }
        public async Task<IResponse<CreateTransactionReponse>> CreateTransaction(CreateTransactionRequest model, object[] args)
        {
            try
            {
                var payment = new Payment()
                {
                    PaymentGatewayId = model.GatewayId,
                    Type = model.PaymentType,
                    Price = model.Amount,
                    OrderId = model.OrderId,
                    PaymentStatus = PaymentStatus.Canceled
                };
                var addPayment = await _paymentSrv.Add(payment);
                if (!addPayment.IsSuccessful) return new Response<CreateTransactionReponse> { Message = addPayment.Message };
                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("api-key", model.ApiKey);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        amount = model.Amount,
                        mobile = model.MobileNumber,
                        description = model.Description,
                        order_id = addPayment.Result.PaymentId,
                        callback = model.CallbackUrl
                    }));
                    var callRep = await http.PostAsync(model.Url, content);
                    if (callRep.IsSuccessStatusCode)
                    {
                        var strRep = await callRep.Content.ReadAsStringAsync();
                        var rep = JsonConvert.DeserializeObject<HillaPayGetTransResponse>(strRep);
                        if (rep.status.status == 200)
                        {
                            payment.TransactionId = rep.result_transaction_send.transaction_id;
                            _paymentRepo.Update(payment);
                            var update = await _appUow.ElkSaveChangesAsync();
                            return new Response<CreateTransactionReponse>
                            {
                                IsSuccessful = update.IsSuccessful,
                                Message = update.Message,
                                Result = new CreateTransactionReponse
                                {
                                    TransactionId = rep.result_transaction_send.transaction_id,
                                    GatewayUrl = rep.result_transaction_send.transaction_url,
                                }
                            };
                        }
                        else return new Response<CreateTransactionReponse> { Message = ServiceMessage.CreateTransactionFailed };
                    }
                    else return new Response<CreateTransactionReponse> { Message = ServiceMessage.CreateTransactionFailed };
                }
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<CreateTransactionReponse> { Message = ServiceMessage.CreateTransactionFailed };
            }

        }

        public async Task<IResponse<string>> VerifyTransaction(VerifyRequest model, object[] args)
        {
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Add("api-key", model.ApiKey);
                var contentModel = new
                {
                    order_id = model.OrderId,
                    transaction_id = model.TransactionId,
                    rrn = args[0].ToString()
                };
                var content = new StringContent(JsonConvert.SerializeObject(contentModel));
                var callRep = await http.PostAsync(model.Url, content);
                if (callRep.IsSuccessStatusCode)
                {
                    var strRep = await callRep.Content.ReadAsStringAsync();
                    var rep = JsonConvert.DeserializeObject<HillaPayVerifyReponse>(strRep);

                    if (rep.status.status == 500)
                        return new Response<string>
                        {
                            IsSuccessful = true,
                            Message = ServiceMessage.VerifyTransactionFailed,
                            Result = rep.result_transaction_verify.transaction_id
                        };

                    else return new Response<string>
                    {
                        Message = ServiceMessage.VerifyTransactionFailed,
                        Result = rep.result_transaction_verify.transaction_id
                    };

                }
                return new Response<string> { Message = ServiceMessage.VerifyTransactionFailed };
            }
        }
    }
}
