namespace FSharp.SpendingTracker.Api

module Api =

    open System
    open System.IO
    open System.Collections.Generic
    open System.Linq
    
    open FSharp.Configuration    

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
            | []    -> "No history file uploaded"                  |> toResult BAD_REQUEST
            | x::xs -> PaymentImport.processCsvFile x.tempFilePath |> toResult OK
        
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