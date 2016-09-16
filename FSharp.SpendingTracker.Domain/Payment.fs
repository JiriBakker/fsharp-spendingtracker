namespace FSharp.SpendingTracker.Domain

open System

type Payment() = 
    member this.Amount = 123M
    member this.Timestamp = DateTimeOffset.UtcNow
