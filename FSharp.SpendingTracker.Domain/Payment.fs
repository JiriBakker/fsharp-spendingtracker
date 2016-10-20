namespace FSharp.SpendingTracker.Domain

open System

type Payment = { 
        PaymentId   : int option;
        Amount      : decimal;
        Timestamp   : DateTimeOffset;
        Description : string
    }
    