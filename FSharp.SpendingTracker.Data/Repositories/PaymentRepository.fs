﻿namespace FSharp.SpendingTracker.Data.Repositories

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

        connection
        |> dapperQuery<PaymentRecord> "SELECT * FROM [Payment] WHERE (1=1) "
        |> List.ofSeq
        |> List.map (fun paymentRecord -> paymentRecord.MapTo)
    