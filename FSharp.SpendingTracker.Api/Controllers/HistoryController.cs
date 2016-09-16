using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FSharp.SpendingTracker.Api.Models;
using FSharp.SpendingTracker.Data.Repositories;
using Microsoft.FSharp.Core;

namespace FSharp.SpendingTracker.Api.Controllers {

    [RoutePrefix("history")]
    public class HistoryController : BaseApiController {

        [Route("recent")]
        public IEnumerable<PaymentDto> GetRecentPayments() {
            return this.ExecuteInSqlConnection(sqlConnection => {
                var payments = 
                    PaymentRepository.getPayments(
                        dateFrom:   new FSharpOption<DateTimeOffset>(DateTimeOffset.Now.AddDays(-7)), 
                        dateUntil:  FSharpOption<DateTimeOffset>.None, 
                        connection: sqlConnection);

                return payments.Select(PaymentDto.MapFrom).ToList();
            });
        }

        [Route("all")]
        public IEnumerable<PaymentDto> GetAllPayments() {
            return this.ExecuteInSqlConnection(sqlConnection => {
                var payments = 
                    PaymentRepository.getPayments(
                        dateFrom:   FSharpOption<DateTimeOffset>.None, 
                        dateUntil:  FSharpOption<DateTimeOffset>.None, 
                        connection: sqlConnection);

                return payments.Select(PaymentDto.MapFrom).ToList();
            });
        }

    }

}
