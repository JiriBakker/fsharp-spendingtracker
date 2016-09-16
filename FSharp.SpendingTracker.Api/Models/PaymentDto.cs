using System;
using FSharp.SpendingTracker.Domain;

namespace FSharp.SpendingTracker.Api.Models {

    public class PaymentDto {

        public decimal        Amount    { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }

        public static PaymentDto MapFrom(Payment payment) {
            var paymentDto = new PaymentDto();
            paymentDto.Amount    = payment.Amount;
            paymentDto.Timestamp = payment.Timestamp;
            return paymentDto;
        }

    }

}