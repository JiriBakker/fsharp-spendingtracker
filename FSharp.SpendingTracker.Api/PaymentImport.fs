namespace FSharp.SpendingTracker.Api

module PaymentImport =
    
    open System
    open System.IO
    open System.Collections.Generic
    open System.Linq

    open FSharp.SpendingTracker.Domain
    open FSharp.SpendingTracker.Data.Repositories
    open FSharp.Data
    open FSharp.Configuration 

    type private Settings = AppSettings<"App.config">
    
    type private History = CsvProvider<Sample = "Timestamp;Description;Amount", Schema = "date,string,decimal", Separators = ";">

    let private toDateTimeOffset dateTime =
        DateTimeOffset(dateTime) // TODO JB set timezone offset?

    let processCsvFile (filePath:string) =
        let csvData = History.Load(filePath)
        csvData.Rows
        |> Seq.map (fun row -> { Amount = row.Amount; Timestamp = (toDateTimeOffset row.Timestamp); Description = row.Description; PaymentId = None })
        |> Seq.map (fun payment -> PaymentRepository.add payment Settings.ConnectionStrings.FsHarpSpendingTracker)

