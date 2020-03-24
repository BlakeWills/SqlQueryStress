# SQLQueryStress

![.NET Core](https://github.com/BlakeWills/SqlQueryStress/workflows/.NET%20Core/badge.svg)

This project was inspired by Adam Mechanic's original [SQLQueryStress](https://github.com/ErikEJ/SqlQueryStress).


## Getting Started 

1) Create a database connection
    - Open the connection dropdown from the toolbar and select "Connection Manager"
    - Click the new connection button.
    - Choose a name, database engine and specify a [connection string](https://www.connectionstrings.com/sql-server/).
    - Click Save and close all but the main window.

2) Enter your query in the main query editor.

3) If your query has parameters, set values using the parameters window, which is available by clicking on the parameters button within the toolbar.


## Parameterized Queries

SQLQueryStress currently supports three types of query parameters:
 1) Random numbers within a range
 2) Random dates
 3) Parameter values which come from a database query.

 Query parameters should use the `@@paramName` syntax, e.g.:

 `SELECT * FROM myTable WHERE myColumn = @@myParam`

There is also the concept of "Linked Parameters" for situations where a parameter value is dependant on the value of another parameter, such as the end date in a date range.

As an example, to configure a date range first configure the start date parameter by setting the parameter type to "Random Date Range", then specify a minimum date, maximum date and maximum interval between the dates in the range. The end date can then be configured by setting the paramater type to "Random Date Range" and setting the Linked Parameter to `@StartDate`.

## Screenshots

SQLQueryStress:
![SqlQueryStress](https://user-images.githubusercontent.com/8128694/77238667-4bf2f180-6bca-11ea-86e1-c6bc65b6812f.png)

Execution Details:
![ExecutionDetails](https://user-images.githubusercontent.com/8128694/77238714-abe99800-6bca-11ea-833f-9d9f4efa2364.png)

## Road Map

- Cross Platform GUI
- Command Line Application
- Support for other database engines (I.E Couchbase, PostgreSQL)
- Query Performance Comparison
- Histograms / Live Visualisations.

## Contributing

Pull requests are welcome.
