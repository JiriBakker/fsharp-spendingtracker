namespace FSharp.SpendingTracker.Data.Records

open System
open FSharp.SpendingTracker.Domain

type PaymentRecord = { PaymentId:int; Amount:decimal; Timestamp:DateTimeOffset; Description:string } with
    member this.mapTo : Payment =
        { Amount = this.Amount; Timestamp = this.Timestamp; Description = this.Description; PaymentId = Some(this.PaymentId) }
    static member mapFrom (payment:Payment) : PaymentRecord =
        { Amount = payment.Amount; Timestamp = payment.Timestamp; Description = payment.Description; PaymentId = (defaultArg payment.PaymentId 0) }