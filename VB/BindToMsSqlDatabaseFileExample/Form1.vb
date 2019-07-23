Imports DevExpress.DashboardCommon
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql

Namespace BindToMsSqlDatabaseFileExample
	Partial Public Class Form1
		Inherits DevExpress.XtraEditors.XtraForm

		Public Sub New()
			InitializeComponent()
			dashboardDesigner1.CreateRibbon()
			Dim currentDashboard As Dashboard = CreateDashboard()
			BindDataSource(currentDashboard, CreateDataSource())
			dashboardDesigner1.Dashboard = currentDashboard
		End Sub

		Private Sub BindDataSource(ByVal dashboard As Dashboard, ByVal dashboardSqlDataSource As DashboardSqlDataSource)
			dashboard.DataSources.Add(dashboardSqlDataSource)
			For Each item In dashboard.Items
				Dim dataItem As DataDashboardItem = TryCast(item, DataDashboardItem)
				If dataItem IsNot Nothing Then
					dataItem.DataSource = dashboardSqlDataSource
					dataItem.DataMember = dashboardSqlDataSource.Queries(0).Name
				End If
			Next item
		End Sub

		Private Function CreateDataSource() As DashboardSqlDataSource
			Dim connectionParameters As New CustomStringConnectionParameters()
			connectionParameters.ConnectionString = "XpoProvider=MSSqlServer;Data Source=(LocalDB)\MSSQLLocalDB;" & "AttachDbFilename=|DataDirectory|\NW19.mdf;" & "Integrated Security=True"
			Dim sqlDataSource As New DashboardSqlDataSource("NW19 SQL Server Database File", connectionParameters)
			' Comment out two lines to use CustomSqlQuery and SQL string expression.
			Dim selectQuery As SelectQuery = CreateSqlQuery()
			sqlDataSource.Queries.Add(selectQuery)
			' Uncomment two lines to to use CustomSqlQuery and SQL string expression.
			'CustomSqlQuery selectQuery = CreateSqlStringQuery();
			'sqlDataSource.Queries.Add(selectQuery);
			sqlDataSource.CalculatedFields.AddRange(CreateCalculatedFields(selectQuery))
			sqlDataSource.Fill()
			Return sqlDataSource
		End Function

		Private Shared Function CreateCalculatedFields(ByVal selectQuery As SqlQuery) As CalculatedFieldCollection
			Dim fieldSalesPerson As New CalculatedField() With {.Name = "Sales Person", .DataMember = selectQuery.Name, .Expression = "Concat([FirstName], ' ', [LastName])"}
			Dim fieldExtPrice As New CalculatedField() With {.Name = "Extended Price", .DataMember = selectQuery.Name, .Expression = "[Quantity] * [UnitPrice]"}
			Return New CalculatedFieldCollection() From { fieldSalesPerson, fieldExtPrice }
		End Function

		Private Shared Function CreateSqlQuery() As SelectQuery
			Dim selectQuery As New SelectQuery("SalesPersons")
			Dim orders = selectQuery.AddTable("Orders")
			Dim order_details = selectQuery.AddTable("Order Details")
			Dim employees = selectQuery.AddTable("Employees")
			Dim products = selectQuery.AddTable("Products")
			Dim categories = selectQuery.AddTable("Categories")
			selectQuery.AddRelation(order_details, orders, "OrderID")
			selectQuery.AddRelation(orders, employees, "EmployeeID")
			selectQuery.AddRelation(order_details, products, "ProductID")
			selectQuery.AddRelation(products, categories, "CategoryID")
			selectQuery.SelectColumns(orders, New String() { "OrderDate", "ShipCity", "ShipCountry" })
			selectQuery.SelectColumns(order_details, New String() { "UnitPrice", "Quantity" })
			selectQuery.SelectColumns(employees, New String() { "FirstName", "LastName" })
			selectQuery.SelectColumn(products, "ProductName")
			selectQuery.SelectColumn(categories, "CategoryName")
			Return selectQuery
		End Function

		Private Function CreateDashboard() As Dashboard
			Dim dBoard As New Dashboard()
			Dim chart As New ChartDashboardItem()

			chart.Arguments.Add(New Dimension("OrderDate", DateTimeGroupInterval.MonthYear))
			chart.Panes.Add(New ChartPane())
			Dim salesAmountSeries As New SimpleSeries(SimpleSeriesType.SplineArea)
			salesAmountSeries.Value = New Measure("Extended Price")
			chart.Panes(0).Series.Add(salesAmountSeries)
			Dim grid As New GridDashboardItem()
			grid.Columns.Add(New GridDimensionColumn(New Dimension("Sales Person")))
			grid.Columns.Add(New GridMeasureColumn(New Measure("Extended Price")))
			dBoard.Items.AddRange(chart, grid)
			Return dBoard
		End Function

		Private Shared Function CreateSqlStringQuery() As CustomSqlQuery
			Dim customSqlStringQuery As New CustomSqlQuery() With {.Name = "SalesPersons", .Sql = "SELECT Categories.CategoryName, [Order Details].UnitPrice, [Order Details].Quantity, Products.ProductName, Orders.OrderDate, Employees.LastName, Employees.FirstName FROM Orders INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID INNER JOIN Products ON [Order Details].ProductID = Products.ProductID INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID"}
			Return customSqlStringQuery
		End Function
	End Class
End Namespace
