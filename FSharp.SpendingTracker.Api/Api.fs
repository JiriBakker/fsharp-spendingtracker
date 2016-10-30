namespace FSharp.SpendingTracker.Api

module Api =

    open System
    open System.IO
    open System.Collections.Generic
    open System.Linq
    open System.Globalization
    
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
        let getCategories () =
            CategoryRepository.getCategoriesPoging2 connectionString
            |> toResult OK

        let getHistory () httpContext = 
            let getParam key = 
                match (httpContext.request.queryParam key) with
                | Choice1Of2 value -> Some value
                | _                -> None

            let parseDate dateStringOption =
                match dateStringOption with
                | Some dateString -> Some (DateTimeOffset.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                | _               -> None

            let parseIntOrDefault intStringOption defaultInt =
                match intStringOption with
                | Some intString -> Int32.Parse intString
                | _              -> defaultInt

            PaymentRepository.getPayments 
                (getParam "category") 
                (parseDate (getParam "dateFrom")) 
                (parseDate (getParam "dateUntil")) 
                (parseIntOrDefault (getParam "limit") 20)
                connectionString
            |> toResult OK    

            <| httpContext
        
        let uploadHistory () httpContext =
            let files = httpContext.request.files

            match files with
            | []    -> "No history file uploaded"                  |> toResult BAD_REQUEST
            | x::xs -> PaymentImport.processCsvFile x.tempFilePath |> toResult OK
        
            <| httpContext    

        // Routing
        choose [ 
            GET  >=> path "/category" >=> executeAction getCategories ;
            GET  >=> path "/history"  >=> executeAction getHistory    ;
            POST >=> path "/history"  >=> executeAction uploadHistory ;
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