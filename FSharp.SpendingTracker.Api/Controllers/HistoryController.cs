using System;
using System.Collections.Generic;
using System.Web.Http;
using FSharp.SpendingTracker.Api.Models;
using FSharp.SpendingTracker.Domain;

namespace FSharp.SpendingTracker.Api.Controllers {

    [RoutePrefix("history")]
    public class HistoryController : ApiController {

        [Route("recent")]
        public IEnumerable<PaymentDto> GetRecentPayments() {
            return new [] { PaymentDto.MapFrom(new Payment()), PaymentDto.MapFrom(new Payment()) };
        }

    }

}
