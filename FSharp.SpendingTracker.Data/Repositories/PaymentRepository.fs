namespace FSharp.SpendingTracker.Data.Repositories

module PaymentRepository =    

    open System
    open System.Data.SqlClient
    open FSharp.SpendingTracker.Domain
    open FSharp.SpendingTracker.Data.Dapper.DapperBase
    open FSharp.SpendingTracker.Data.Records

    let getPayments category (dateFrom:DateTimeOffset option) (dateUntil:DateTimeOffset option) limit connectionString =      
        let addClauseIfSome optional clauseSuffix query =
            match optional with
            | None            -> query
            | Some(timestamp) -> query + " AND " + clauseSuffix

        let append appendString query =
            query + appendString

        let query =
            "SELECT * FROM [Payment] WHERE (1=1) "
            |> addClauseIfSome category  "('@category' = '@category')"
            |> addClauseIfSome dateFrom  "[Timestamp] >= @dateFrom"
            |> addClauseIfSome dateUntil "[Timestamp] <= @dateUntil"
            |> append " ORDER BY [Timestamp] DESC "
            |> append " OFFSET 0 ROWS FETCH NEXT @limit ROWS ONLY "
        
        let addParam key optional list =
            match optional with
            | Some value -> (key, value :> obj) :: list
            | _ -> list

        let queryParameters = 
            []
            |> addParam "category" category
            |> addParam "dateFrom" dateFrom
            |> addParam "dateUntil" dateUntil
            |> addParam "limit" (Some limit)

        use connection = new SqlConnection(connectionString)
        connection
        |> dapperMapParameterizedQuery<PaymentRecord> query (Map queryParameters)

        |> Seq.map (fun paymentRecord -> paymentRecord.mapTo)
    
    let add (payment:Payment) connectionString =
        let paymentRecord = PaymentRecord.mapFrom payment
        
        use connection = new SqlConnection(connectionString)
        connection
        |> dapperMapParameterizedInsert 
            "INSERT INTO [Payment] ([Amount], [Timestamp], [Description], [CreatedAtUtc]) VALUES (@amount, @timestamp, @description, SYSDATETIME());" 
            (Map [("amount", paymentRecord.Amount :> obj); ("timestamp", paymentRecord.Timestamp :> obj); ("description", paymentRecord.Description :> obj)])