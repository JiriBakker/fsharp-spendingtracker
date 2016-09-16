namespace FSharp.SpendingTracker.Data.Repositories

open System
open FSharp.SpendingTracker.Data.Dapper.DapperBase
open FSharp.SpendingTracker.Data.Records

module PaymentRepository =    

    let getPayments (dateFrom:DateTimeOffset option) (dateUntil:DateTimeOffset option) connection =        

        let timestampClauseIfSome timestampOption clauseSuffix =
            match timestampOption with
            | None            -> ""
            | Some(timestamp) -> " AND [Timestamp] " + clauseSuffix

        let dateFromClause  = timestampClauseIfSome dateFrom  ">= @dateFrom"
        let dateUntilClause = timestampClauseIfSome dateUntil "<= @dateUntil"           
        
        let dateRangeParameters =
            [("dateFrom", dateFrom); ("dateUntil", dateUntil)]
            |> List.fold (fun acc param -> match param with | (_,None) -> acc | (k,Some(p)) -> (k,p :> obj) :: acc) []

        connection
        |> dapperMapParametrizedQuery<PaymentRecord> ("SELECT * FROM [Payment] WHERE (1=1) " + dateFromClause + dateUntilClause) (Map dateRangeParameters)
        |> List.ofSeq
        |> List.map (fun paymentRecord -> paymentRecord.MapTo)
    