namespace FSharp.SpendingTracker.Data.Records

open System
open FSharp.SpendingTracker.Domain

type CategoryRecord = { CategoryId:int; Title:string; DescriptionsToMatch:CategoryDescriptionRecord list } with
    member this.mapTo : Category =
        { CategoryId = this.CategoryId; Title = this.Title; DescriptionsToMatch = this.DescriptionsToMatch |> List.map (fun d -> d.Description) }