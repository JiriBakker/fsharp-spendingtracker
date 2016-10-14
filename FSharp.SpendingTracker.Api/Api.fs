namespace FSharp.SpendingTracker.Api

module Api =

    open System
    open System.IO
    open System.Collections.Generic
    open System.Linq
    
    open FSharp.Configuration
    open FSharp.Data

    open Suave
    open Suave.Filters
    open Suave.Operators
    open Suave.Successful
    open Suave.RequestErrors
    open Suave.ServerErrors
    open Newtonsoft.Json

    open FSharp.SpendingTracker.Data.Repositories
    open FSharp.SpendingTracker.Domain

    type private Settings = AppSettings<"App.config">

    type private History = CsvProvider<Sample = "Timestamp;Description;Amount", Schema = "date,string,decimal", Separators = ";">

    let private app =

        // Config
        let connectionString = Settings.ConnectionStrings.FsHarpSpendingTracker
    
        // Helpers
        let toResult status result =
            Writers.setMimeType "application/json; charset=utf-8"
            >=> Writers.addHeader "Access-Control-Allow-Origin" "*"
            >=> status (JsonConvert.SerializeObject result) 
         
        let executeAction action httpContext =
            action() httpContext

        // TODO place in modules?

        let processHistoryFile (filePath:string) =
            let csvData = History.Load(filePath)
            [
                String.Format("Success! Found {0} rows", csvData.Rows.Count()) ;
                String.Format("Found {0} headers: {1}", csvData.Headers.Value.Count(), String.Join(" - ", csvData.Headers.Value)) ;
                "Rows:"
            ] @ (
                csvData.Rows
                |> List.ofSeq
                |> List.map (fun row -> String.Format("Description: {0} - Amount: {1} - Timestamp: {2}", row.Description, row.Amount, row.Timestamp))
            )


        // Actions
        let getRecentHistory () = 
            let oneWeekAgo = Some (DateTimeOffset.Now.AddDays(-7.0))
            PaymentRepository.getPayments oneWeekAgo None connectionString
            |> toResult OK

        let getAllHistory () = 
            PaymentRepository.getPayments None None connectionString
            |> toResult OK    
        
        let uploadHistory () httpContext =
            let files = httpContext.request.files

            match files with
            | []    -> "No history file uploaded"        |> toResult BAD_REQUEST
            | x::xs -> processHistoryFile x.tempFilePath |> toResult OK
        
            <| httpContext    

        // Routing
        choose [ 
            path "/history/recent" >=> GET  >=> executeAction getRecentHistory ;
            path "/history/all"    >=> GET  >=> executeAction getAllHistory    ;
            path "/history"        >=> POST >=> executeAction uploadHistory    ;
            NOT_FOUND "Oops! Resource not found..."
        ]



    [<EntryPoint>]
    let main _ =
        let config = defaultConfig
        let config =
            match IISHelpers.httpPlatformPort with
            | Some port ->
                { config with
                    bindings = [ HttpBinding.mkSimple HTTP "127.0.0.1" port ] }
            | None -> config
        startWebServer config app
        0