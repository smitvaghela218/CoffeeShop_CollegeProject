using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CoffeeShop.Controllers
{
    public class OrderDetailController : Controller
    {
        // Dependency Injection
        private IConfiguration _configuration;

        public OrderDetailController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public IActionResult OrderDetailEdit(int OrderDetailID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            #region User Drop-Down

            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            connection1.Close();

            List<UserDropDownModel> users = new List<UserDropDownModel>();

            foreach (DataRow dataRow in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(dataRow["UserID"]);
                userDropDownModel.UserName = dataRow["UserName"].ToString();
                users.Add(userDropDownModel);
            }

            ViewBag.UserList = users;

            #endregion

            #region Order-Date Drop-Down

            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_Order_DropDown";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            connection2.Close();

            List<OrderDropDownModel> orders = new List<OrderDropDownModel>();

            foreach (DataRow dataRow in dataTable2.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(dataRow["OrderID"]);
                orderDropDownModel.OrderDate = Convert.ToDateTime(dataRow["OrderDate"]);
                orders.Add(orderDropDownModel);
            }

            ViewBag.OrderList = orders;

            #endregion 

            #region Product Drop-Down

            SqlConnection connection3 = new SqlConnection(connectionString);
            connection3.Open();
            SqlCommand command3 = connection3.CreateCommand();
            command3.CommandType = System.Data.CommandType.StoredProcedure;
            command3.CommandText = "PR_Product_DropDown";
            SqlDataReader reader3 = command3.ExecuteReader();
            DataTable dataTable3 = new DataTable();
            dataTable3.Load(reader3);
            connection3.Close();

            List<ProductDropDownModel> products = new List<ProductDropDownModel>();

            foreach (DataRow dataRow in dataTable3.Rows)
            {
                ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                productDropDownModel.ProductID = Convert.ToInt32(dataRow["ProductID"]);
                productDropDownModel.ProductName = dataRow["ProductName"].ToString();
                products.Add(productDropDownModel);
            }

            ViewBag.ProductList = products;

            #endregion 

            #region OrderDetailID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectByPK";
            command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderDetailModel orderDetailModel = new OrderDetailModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderDetailModel.OrderDetailID = Convert.ToInt32(@dataRow["OrderDetailID"]);
                orderDetailModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderDetailModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                orderDetailModel.Quantity = Convert.ToInt32(@dataRow["Quantity"]);
                orderDetailModel.Amount = Convert.ToDouble(@dataRow["Amount"]);
                orderDetailModel.TotalAmount = Convert.ToDouble(@dataRow["TotalAmount"]);
                orderDetailModel.UserID = Convert.ToInt32(@dataRow["UserID"]);

            }

            #endregion

            return View("OrderDetailAddEdit", orderDetailModel);
        }


        public IActionResult OrderDetailSave(OrderDetailModel orderDetailModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderDetailModel.OrderDetailID == null)
                {
                    command.CommandText = "PR_OrderDetail_Insert";
                }
                else
                {
                    command.CommandText = "PR_OrderDetail_UpdateByPK";
                    command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderDetailModel.OrderDetailID;
                }
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderDetailModel.OrderID;
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderDetailModel.ProductID;
                command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderDetailModel.Quantity;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = orderDetailModel.Amount;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderDetailModel.TotalAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderDetailModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderDetailList");

            }
            return View("OrderDetailAddEdit", orderDetailModel);
        }
        public List<UserDropDownModel> GetUserDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);

            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";

            SqlDataReader reader1 = command1.ExecuteReader();

            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            // Create a list of userDropDownModel
            List<UserDropDownModel> userList = new List<UserDropDownModel>();

            // Input new data in userDropDownModel object and add into List
            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                userList.Add(userDropDownModel);
            }

            return userList;
        }

        public List<OrderDropDownModel> GetOrderDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);

            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";

            SqlDataReader reader1 = command1.ExecuteReader();

            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            // Create a list of userDropDownModel
            List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();

            // Input new data in userDropDownModel object and add into List
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDownModel.OrderDate = Convert.ToDateTime(data["OrderDate"]);
                orderList.Add(orderDropDownModel);
            }

            return orderList;
        }

        public List<ProductDropDownModel> GetProductDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);

            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Product_DropDown";

            SqlDataReader reader1 = command1.ExecuteReader();

            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            // Create a list of userDropDownModel
            List<ProductDropDownModel> productList = new List<ProductDropDownModel>();

            // Input new data in userDropDownModel object and add into List
            foreach (DataRow data in dataTable1.Rows)
            {
                ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                productDropDownModel.ProductID = Convert.ToInt32(data["ProductID"]);
                productDropDownModel.ProductName = data["ProductName"].ToString();
                productList.Add(productDropDownModel);
            }

            return productList;
        }

        public IActionResult OrderDetailList()
        {
            // Connect SQL
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }



        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
            try
            {
                // Connect SQL
                string connectionString = this._configuration.GetConnectionString("ConnectionString"); // From appsettings.json file
                SqlConnection connection = new SqlConnection(connectionString);

                // Open connection
                connection.Open();

                // Create command
                SqlCommand command = connection.CreateCommand();

                // Define command type - here, StoredProcedure
                command.CommandType = CommandType.StoredProcedure;

                // Define command text - here, ProcedureName
                command.CommandText = "PR_OrderDetail_DeleteByPK";

                // Add parameters to procedure
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;

                // ExecuteNonQuery() : used to execute a SQL query that does not return any data, such as INSERT, UPDATE, or DELETE statements.
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Redirect to View - ProductList
            return RedirectToAction("OrderDetailList");
        }


        public IActionResult OrderDetailAddEdit()
        {
            ViewBag.UserList = GetUserDropDown();
            ViewBag.ProductList = GetProductDropDown();
            ViewBag.OrderList = GetOrderDropDown();
            return View();
        }


        public IActionResult Save(OrderDetailModel orderDetailModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderDetailList");
            }
            else
            {
                return View("OrderDetailAddEdit", orderDetailModel);
            }
        }
    }
}
