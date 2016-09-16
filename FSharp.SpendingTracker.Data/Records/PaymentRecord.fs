namespace FSharp.SpendingTracker.Data.Records

open System
open FSharp.SpendingTracker.Domain

type PaymentRecord = { PaymentId:int; Amount:decimal; Timestamp:DateTimeOffset } with
    member this.MapTo : Payment =
        { Amount = this.Amount; Timestamp = this.Timestamp }
