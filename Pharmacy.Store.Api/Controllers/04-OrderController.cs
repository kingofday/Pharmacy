using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.API.Resources;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.API.Controllers
{
    public class OrderController : Controller
    {
        readonly IUserService _userService;
        readonly IOrderService _orderService;
        readonly IPaymentService _paymentService;
        readonly IGatewayFactory _gatewayFectory;
        readonly IConfiguration _configuration;
        public OrderController(IUserService userService, IOrderService orderService, IPaymentService paymentService, IGatewayFactory gatewayFactory, IConfiguration configuration)
        {
            _userService = userService;
            _orderService = orderService;
            _paymentService = paymentService;
            _gatewayFectory = gatewayFactory;
            _configuration = configuration;
        }

    }
}