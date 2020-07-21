using System;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Store.Api.Controllers
{
    public class TempBasketItemController : Controller
    {
        private readonly ITempBasketItemService _TempBasketItemSrv;
        public TempBasketItemController(ITempBasketItemService TempBasketItemSrv)
        {
            _TempBasketItemSrv = TempBasketItemSrv;
        }

        [HttpGet]
        public IActionResult Get(Guid basketId) => Json(_TempBasketItemSrv.Get(basketId));
    }
}