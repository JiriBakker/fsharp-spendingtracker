namespace FSharp.SpendingTracker.Data.Dapper

module internal DapperBase =

    // https://gist.github.com/vbfox/1e9f42f6dcdd9efd6660

    open System.Data.SqlClient
    open System.Dynamic
    open System.Collections.Generic
    open System
    open Dapper    

    let dapperQuery<'Result> (query:string) (connection:SqlConnection) =
        connection.Query<'Result>(query)

    let dapperQuery1Join<'Result1, 'Result2, 'Result3> (query:string) (mapFunc:'Result1 -> 'Result2 -> 'Result3) (splitOn:string) (connection:SqlConnection) =
        connection.Query<'Result1, 'Result2, 'Result3>(query, Func<'Result1, 'Result2, 'Result3>(mapFunc), splitOn = splitOn)        
    
    let dapperParameterizedQuery<'Result> (query:string) (param:obj) (connection:SqlConnection) : 'Result seq =
        connection.Query<'Result>(query, param)
    
    let dapperMapParameterizedQuery<'Result> (query:string) (param : Map<string,_>) (connection:SqlConnection) : 'Result seq =
        let expando = ExpandoObject()
        let expandoDictionary = expando :> IDictionary<string,obj>
        for paramValue in param do
            expandoDictionary.Add(paramValue.Key, paramValue.Value :> obj)
    
        connection |> dapperParameterizedQuery query expando

    let dapperMapParameterizedInsert (query:string) (param : Map<string,_>) (connection:SqlConnection) : int =
        dapperMapParameterizedQuery<int> ("SET NOCOUNT ON; " + query + "; SELECT CAST(COALESCE(SCOPE_IDENTITY(), 0) AS INT);") param connection
        |> Seq.head


    