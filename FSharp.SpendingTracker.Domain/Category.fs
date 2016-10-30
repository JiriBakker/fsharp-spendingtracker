namespace FSharp.SpendingTracker.Domain

open System

type Category = { 
        CategoryId          : int;
        Title               : string;
        DescriptionsToMatch : string list
    }
    