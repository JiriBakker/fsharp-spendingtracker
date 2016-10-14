namespace FSharp.SpendingTracker.Api

module IISHelpers =
    // https://blog.vbfox.net/2016/03/31/Suave-in-IIS-hello-world.html

    open System

    /// Port specified by IIS HttpPlatformHandler
    let httpPlatformPort =
        match Environment.GetEnvironmentVariable("HTTP_PLATFORM_PORT") with
        | null -> None
        | value ->
            match Int32.TryParse(value) with
            | true, value -> Some value
            | false, _ -> None