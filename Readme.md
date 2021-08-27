<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/198263101/19.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T828589)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Form1.cs](./CS/BindToMsSqlDatabaseFileExample/Form1.cs) (VB: [Form1.vb](./VB/BindToMsSqlDatabaseFileExample/Form1.vb))
<!-- default file list end -->

# Dashboard for WinForms - How to bind a dashboard to a Microsoft SQL Server database file at runtime

This example demonstrates how to bind a Microsoft SQL Server database file (.MDF) at runtime to a dashboard created in code.

To bind the dashboard to the Microsoft SQL Server database file, create a [DashboardSqlDataSource](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardSqlDataSource) instance and provide connection parameters. 

> The [MsSqlConnectionParameters](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters) object is not suitable for the SQL database file. You should use the [CustomStringConnectionParameters](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.ConnectionParameters.CustomStringConnectionParameters) instead.

The following connection string is used for .MDF file in this example:

```code
XpoProvider=MSSqlServer;Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\NW19.mdf;Integrated Security=True
```

Database and log files NW19.mdf and NW19.ldf are not incuded in the project. They are copied to the working directory with the following post-build event command:

```sh
xcopy "$(ProjectDir)NW19*.*" $(TargetDir) /Y
```

![screenshot](/images/screenshot.png)

The [SelectQuery](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SelectQuery) object is used to build a SQL query in code. You can also use the [CustomSqlQuery](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.CustomSqlQuery) to specify a SQL expression string.

> The [SelectQueryFluentBuilder](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SelectQueryFluentBuilder) instance in not appropriate for this query because it does not support multiple JOINs.

The dashboard uses [calculated fields](https://docs.devexpress.com/Dashboard/16134). Create them in code and add to the [DashboardSqlDataSource.CalculatedFields ](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardSqlDataSource.CalculatedFields) collection.


To bind dashboard items and calculated fields to the [SQL Data Source](https://docs.devexpress.com/Dashboard/16151), you should specify the Data Member setting. It is the name of the created SelectQuery.

## Documentation

* [Connecting to a Data Source](https://docs.devexpress.com/Dashboard/116879)
* [Providing Data](https://docs.devexpress.com/Dashboard/12146)
* [Dashboard.ConfigureDataConnection](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.Dashboard.ConfigureDataConnection) event

## More Examples

* [Dashboard for WinForms - How to Bind a Dashboard to an SQL Data Source](https://github.com/DevExpress-Examples/how-to-bind-a-dashboard-to-a-sql-database-using-dashboardsqldatasource-e5107)
* [Dashboard for WinForms - How to Replace the Dashboard Sql Data Source with the Dashboard Object Data Source](https://github.com/DevExpress-Examples/how-to-replace-dashboardsqldatasource-with-dashboardobjectdatasource-with-filtered-data-t556647)
* [Dashboard for WPF - How to bind a dashboard to a Microsoft SQL Server database file](https://github.com/DevExpress-Examples/wpf-dashboard-how-to-bind-to-sql-database-file)
