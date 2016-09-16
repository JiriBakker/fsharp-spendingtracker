namespace FSharp.SpendingTracker.Domain

open System

type Payment = { 
        Amount    : decimal;
        Timestamp : DateTimeOffset 
    }
    