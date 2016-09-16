# Spending Tracker application with a C# Web API and an F# Domain layer

(Simple) spending tracker application to get more familiarity with real world F# web application development. Initially the Web API layer will be implemented in C#, but may be ported to F# at a later stage.


###### Database

Excute the SQL scripts in the ~/sql folder consecutively to create the proper database structure. Manually add records to the *Payment* table to be able to retrieve them from the API.


###### connections.config

In order to connect to the database, the FSharp.SpendingTracker.Api project requires a *connections.confg* file containing a connection string to the database. A template file (*connections.config.template*) has been provided, and should be modified and renamed to *connections.config*.