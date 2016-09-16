namespace FSharp.SpendingTracker.Tests

open NUnit.Framework
open FsUnit

open FSharp.SpendingTracker.Domain

[<TestFixture>]
type PaymentTests() =
    
    [<Test>]
    member this.payment_can_be_constructed() =
        let payment = Payment()
        true |> should be True
   
