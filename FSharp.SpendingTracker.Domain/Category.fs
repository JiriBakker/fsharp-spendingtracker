namespace FSharp.SpendingTracker.Domain

open System

type Category = { 
        Title               : string;
        DescriptionsToMatch : string list
    }
    