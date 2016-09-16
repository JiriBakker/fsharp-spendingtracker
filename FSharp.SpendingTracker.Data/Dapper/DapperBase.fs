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
    
    let dapperParameterizedQuery<'Result> (query:string) (param:obj) (connection:SqlConnection) : 'Result seq =
        connection.Query<'Result>(query, param)
    
    let dapperMapParameterizedQuery<'Result> (query:string) (param : Map<string,_>) (connection:SqlConnection) : 'Result seq =
        let expando = ExpandoObject()
        let expandoDictionary = expando :> IDictionary<string,obj>
        for paramValue in param do
            expandoDictionary.Add(paramValue.Key, paramValue.Value :> obj)
    
        connection |> dapperParameterizedQuery query expando


    