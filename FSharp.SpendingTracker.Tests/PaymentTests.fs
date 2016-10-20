namespace FSharp.SpendingTracker.Tests

open NUnit.Framework
open FsUnit
open System

open FSharp.SpendingTracker.Domain

[<TestFixture>]
type PaymentTests() =
    
    [<Test>]
    member this.payment_can_be_constructed() =
        let payment = { Amount = 123M; Timestamp = DateTimeOffset.UtcNow; Description = "test"; PaymentId = None }
        true |> should be True
   
