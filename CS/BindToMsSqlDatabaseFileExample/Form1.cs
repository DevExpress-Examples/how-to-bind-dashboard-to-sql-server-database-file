using DevExpress.DashboardCommon;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;

namespace BindToMsSqlDatabaseFileExample
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            dashboardDesigner1.CreateRibbon();
            Dashboard currentDashboard = CreateDashboard();
            BindDataSource(currentDashboard, CreateDataSource());
            dashboardDesigner1.Dashboard = currentDashboard;
        }

        private void BindDataSource(Dashboard dashboard, DashboardSqlDataSource dashboardSqlDataSource)
        {
            dashboard.DataSources.Add(dashboardSqlDataSource);
            foreach (var item in dashboard.Items)
            {
                DataDashboardItem dataItem = item as DataDashboardItem;
                if (dataItem != null)
                {
                    dataItem.DataSource = dashboardSqlDataSource;
                    dataItem.DataMember = dashboardSqlDataSource.Queries[0].Name;
                }
            }
        }

        private DashboardSqlDataSource CreateDataSource()
        {
            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters();
            connectionParameters.ConnectionString =
                @"XpoProvider=MSSqlServer;Data Source=(LocalDB)\MSSQLLocalDB;" +
                @"AttachDbFilename=|DataDirectory|\NW19.mdf;" +
                @"Integrated Security=True";
            DashboardSqlDataSource sqlDataSource =
                new DashboardSqlDataSource("NW19 SQL Server Database File", connectionParameters);
            // Comment out two lines to use CustomSqlQuery and SQL string expression.
            SelectQuery selectQuery = CreateSqlQuery();
            sqlDataSource.Queries.Add(selectQuery);
            // Uncomment two lines to to use CustomSqlQuery and SQL string expression.
            //CustomSqlQuery selectQuery = CreateSqlStringQuery();
            //sqlDataSource.Queries.Add(selectQuery);
            sqlDataSource.CalculatedFields.AddRange(CreateCalculatedFields(selectQuery));
            sqlDataSource.Fill();
            return sqlDataSource;
        }

        private static CalculatedFieldCollection CreateCalculatedFields(SqlQuery selectQuery)
        {
            CalculatedField fieldSalesPerson = new CalculatedField()
            {
                Name = "Sales Person",
                DataMember = selectQuery.Name,
                Expression = "Concat([FirstName], ' ', [LastName])"
            };
            CalculatedField fieldExtPrice = new CalculatedField()
            {
                Name = "Extended Price",
                DataMember = selectQuery.Name,
                Expression = "[Quantity] * [UnitPrice]",
            };
            return new CalculatedFieldCollection() { fieldSalesPerson, fieldExtPrice };
        }

        private static SelectQuery CreateSqlQuery()
        {
            SelectQuery selectQuery = new SelectQuery("SalesPersons");
            var orders = selectQuery.AddTable("Orders");
            var order_details = selectQuery.AddTable("Order Details");
            var employees = selectQuery.AddTable("Employees");
            var products = selectQuery.AddTable("Products");
            var categories = selectQuery.AddTable("Categories");
            selectQuery.AddRelation(order_details, orders, "OrderID");
            selectQuery.AddRelation(orders, employees, "EmployeeID");
            selectQuery.AddRelation(order_details, products, "ProductID");
            selectQuery.AddRelation(products, categories, "CategoryID");
            selectQuery.SelectColumns(orders, new string[] { "OrderDate", "ShipCity", "ShipCountry" });
            selectQuery.SelectColumns(order_details, new string[] { "UnitPrice", "Quantity" });
            selectQuery.SelectColumns(employees, new string[] { "FirstName", "LastName" });
            selectQuery.SelectColumn(products, "ProductName");
            selectQuery.SelectColumn(categories, "CategoryName");
            return selectQuery;
        }

        private Dashboard CreateDashboard()
        {
            Dashboard dBoard = new Dashboard();
            ChartDashboardItem chart = new ChartDashboardItem();
            chart.Arguments.Add(new Dimension("OrderDate", DateTimeGroupInterval.MonthYear));
            chart.Panes.Add(new ChartPane());
            SimpleSeries salesAmountSeries = new SimpleSeries(SimpleSeriesType.SplineArea);
            salesAmountSeries.Value = new Measure("Extended Price");
            chart.Panes[0].Series.Add(salesAmountSeries);
            GridDashboardItem grid = new GridDashboardItem();
            grid.Columns.Add(new GridDimensionColumn(new Dimension("Sales Person")));
            grid.Columns.Add(new GridMeasureColumn(new Measure("Extended Price")));
            dBoard.Items.AddRange(chart, grid);
            return dBoard;
        }

        private static CustomSqlQuery CreateSqlStringQuery()
        {
            CustomSqlQuery customSqlStringQuery = new CustomSqlQuery()
            {
                Name = "SalesPersons",
                Sql = @"SELECT Categories.CategoryName, [Order Details].UnitPrice, [Order Details].Quantity, 
                               Products.ProductName, Orders.OrderDate, Employees.LastName, Employees.FirstName
                        FROM Orders INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID 
                                    INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID 
                                    INNER JOIN Products ON [Order Details].ProductID = Products.ProductID 
                                    INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID"
            };
            return customSqlStringQuery;
        }
    }
}
