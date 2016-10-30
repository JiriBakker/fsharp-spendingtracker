namespace FSharp.SpendingTracker.Tests

open NUnit.Framework
open FsUnit
open System

open FSharp.SpendingTracker.Api
open FSharp.SpendingTracker.Domain

[<TestFixture>]
type CategoryMatcherTests() =
    
    [<Test>]
    member this.identical_descriptions_are_matched() =
        let categories = [
            { Title = "MyCategory";    DescriptionsToMatch = [ "category-to-match" ] };
            { Title = "OtherCategory"; DescriptionsToMatch = [ "this-category-should-not-be-matched" ] }      
        ] 

        let orderedMatches =
            categories
            |> CategoryMatcher.getPotentialCategoryMatches "category-to-match"

        orderedMatches.Head
        |> (fun (category,_,_) -> category.Title)
        |> should equal "MyCategory"

    [<Test>]
    member this.similar_descriptions_are_matched() =
        let categories = [
            { Title = "MyCategory";    DescriptionsToMatch = [ "category-to-match" ] };
            { Title = "OtherCategory"; DescriptionsToMatch = [ "this-category-should-not-be-matched" ] }      
        ] 

        let orderedMatches =
            categories
            |> CategoryMatcher.getPotentialCategoryMatches "similar-category-to-match"

        orderedMatches.Head
        |> (fun (category,_,_) -> category.Title)
        |> should equal "MyCategory"

    [<Test>]
    member this.matching_is_case_insensitive() =
        let categories = [
            { Title = "MyCategory";    DescriptionsToMatch = [ "category to match" ] };
            { Title = "OtherCategory"; DescriptionsToMatch = [ "Category-To-Match" ] }      
        ] 

        let orderedMatches =
            categories
            |> CategoryMatcher.getPotentialCategoryMatches "CATEGORY TO MATCH"

        orderedMatches.Head
        |> (fun (category,_,_) -> category.Title)
        |> should equal "MyCategory"

   
