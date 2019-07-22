# How to: Bind a Dashboard to a Microsoft SQL Server Database File at Runtime

This example demonstrates how to bind a Microsoft SQL Server database file (.MDF) at runtime to a dashboard created in code.

To bind the dashboard to the Microsoft SQL Server database file, create a [DashboardSqlDataSource](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardSqlDataSource) instance and provide connection parameters. 

> The [MsSqlConnectionParameters](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters) object is not suitable for the SQL database file. You should use the [CustomStringConnectionParameters](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.ConnectionParameters.CustomStringConnectionParameters) instead.

The following connection string is used for .MDF file in this example:

```language
XpoProvider=MSSqlServer;Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\NW19.mdf;Integrated Security=True
```

![screenshot](/images/screenshot.png)

The [SelectQuery](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SelectQuery) object is used to build a SQL query in code. 

> The [SelectQueryFluentBuilder](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SelectQueryFluentBuilder) instance in not appropriate for this query because it does not support multiple JOINs.

The dashboard uses [calculated fields](https://docs.devexpress.com/Dashboard/16134). Create them in code and add to the [DashboardSqlDataSource.CalculatedFields ](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardSqlDataSource.CalculatedFields) collection.


To bind dashboard items and calculated fields to the [SQL Data Source](https://docs.devexpress.com/Dashboard/16151), you should specify the Data Member setting. It is the name of the created SelectQuery.

See also:

* [Connecting to a Data Source](https://docs.devexpress.com/Dashboard/116879)
* [Providing Data](https://docs.devexpress.com/Dashboard/12146)
* [Dashboard.ConfigureDataConnection](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.Dashboard.ConfigureDataConnection) event