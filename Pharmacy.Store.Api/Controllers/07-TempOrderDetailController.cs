using System;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Store.Api.Controllers
{
    public class TempOrderDetailController : Controller
    {
        private readonly ITempOrderDetailService _tempOrderDetailSrv;
        public TempOrderDetailController(ITempOrderDetailService tempOrderDetailSrv)
        {
            _tempOrderDetailSrv = tempOrderDetailSrv;
        }

        [HttpGet]
        public IActionResult Get(Guid basketId) => Json(_tempOrderDetailSrv.Get(basketId));
    }
}