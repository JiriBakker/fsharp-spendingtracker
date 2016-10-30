namespace FSharp.SpendingTracker.Data.Repositories

module CategoryRepository =    

    open System
    open System.Collections.Generic
    open System.Data.SqlClient
    open FSharp.SpendingTracker.Domain
    open FSharp.SpendingTracker.Data.Dapper.DapperBase
    open FSharp.SpendingTracker.Data.Records

    type private CategoryRecordIntermediatePoging1 = { CategoryId : int; Title : string }
    type private CategoryRecordIntermediatePoging2 = { CategoryId : int; Title : string ; CategoryDescriptionId : int; Description : string }


    let getCategoriesPoging3 connectionString =      
        let query =
            "SELECT c.[CategoryId], c.[Title], cd.[CategoryDescriptionId], cd.[Description] 
            FROM [Category] AS c
            LEFT JOIN [CategoryDescription] AS cd ON cd.CategoryId = c.CategoryId 
            WHERE (1=1) 
           "
        
        let lookup = Dictionary<int, CategoryRecord>()
        use connection = new SqlConnection(connectionString)
        connection
        |> dapperQuery<CategoryRecordIntermediatePoging2> query 
        |> Seq.groupBy (fun intermediate -> intermediate.CategoryId)
        |> Seq.map (fun (key, group) -> 
                { 
                    CategoryId = key; 
                    Title = Seq.head group |> (fun g -> g.Title); 
                    DescriptionsToMatch = (Seq.map (fun row -> { CategoryDescriptionId = row.CategoryDescriptionId; Description = row.Description } : CategoryDescriptionRecord) group) |> List.ofSeq
                } : CategoryRecord
            )                

        |> Seq.map (fun categoryRecord -> categoryRecord.mapTo)

    let getCategoriesPoging2 connectionString =      
        let query =
            "SELECT c.[CategoryId], c.[Title], cd.[CategoryDescriptionId], cd.[Description] 
             FROM [Category] AS c
             LEFT JOIN [CategoryDescription] AS cd ON cd.CategoryId = c.CategoryId"
        
        use connection = new SqlConnection(connectionString)
        connection
        |> dapperQuery<CategoryRecordIntermediatePoging2> query 
        |> Seq.groupBy (fun intermediate -> intermediate.CategoryId)
        |> Seq.map (fun (key, group) -> 
                { 
                    CategoryId = key; 
                    Title = Seq.head group |> (fun g -> g.Title); 
                    DescriptionsToMatch = (Seq.map (fun row -> { CategoryDescriptionId = row.CategoryDescriptionId; Description = row.Description } : CategoryDescriptionRecord) group) |> List.ofSeq
                } : CategoryRecord
            )                

        |> Seq.map (fun categoryRecord -> categoryRecord.mapTo)

    let getCategoriesPoging1 connectionString =      
        let query =
            "SELECT c.[CategoryId], c.[Title], cd.[CategoryDescriptionId], cd.[Description] 
             FROM [Category] AS c
             LEFT JOIN [CategoryDescription] AS cd ON cd.CategoryId = c.CategoryId"
        
        let lookup = Dictionary<int, CategoryRecord>()
        use connection = new SqlConnection(connectionString)
        connection
        |> dapperQuery1Join<CategoryRecordIntermediatePoging1, CategoryDescriptionRecord, CategoryRecord> 
                query 
                (fun c cd ->
                    if lookup.ContainsKey(c.CategoryId) then
                        let existingCategoryRecord = lookup.[c.CategoryId]
                        let updatedCategoryRecord = { existingCategoryRecord with DescriptionsToMatch = cd :: existingCategoryRecord.DescriptionsToMatch }
                        lookup.[c.CategoryId] = updatedCategoryRecord |> ignore
                        updatedCategoryRecord
                    else
                        let newCategoryRecord = { DescriptionsToMatch = [cd]; CategoryId = c.CategoryId; Title = c.Title }
                        lookup.Add(c.CategoryId, newCategoryRecord)
                        newCategoryRecord                    
                )
                "CategoryDescriptionId"

        |> Seq.map (fun categoryRecord -> categoryRecord.mapTo)
    
   