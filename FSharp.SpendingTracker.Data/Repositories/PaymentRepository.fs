namespace FSharp.SpendingTracker.Data.Repositories

module PaymentRepository =    

    open System
    open System.Data.SqlClient
    open FSharp.SpendingTracker.Data.Dapper.DapperBase
    open FSharp.SpendingTracker.Data.Records

    let getPayments (dateFrom:DateTimeOffset option) (dateUntil:DateTimeOffset option) connectionString =        

        let timestampClauseIfSome timestampOption clauseSuffix =
            match timestampOption with
            | None            -> ""
            | Some(timestamp) -> " AND [Timestamp] " + clauseSuffix

        let dateFromClause  = timestampClauseIfSome dateFrom  ">= @dateFrom"
        let dateUntilClause = timestampClauseIfSome dateUntil "<= @dateUntil"           
        
        let dateRangeParameters =
            [("dateFrom", dateFrom); ("dateUntil", dateUntil)]
            |> List.fold (fun acc param -> match param with | (_,None) -> acc | (k,Some(p)) -> (k,p :> obj) :: acc) []

        use connection = new SqlConnection(connectionString)
        connection
        |> dapperMapParameterizedQuery<PaymentRecord> ("SELECT * FROM [Payment] WHERE (1=1) " + dateFromClause + dateUntilClause) (Map dateRangeParameters)
        |> List.ofSeq
        |> List.map (fun paymentRecord -> paymentRecord.MapTo)
    