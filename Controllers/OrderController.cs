using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Models;
using System.Data.SqlClient;
using System.Data;

namespace CoffeeShop.Controllers
{
    public class OrderController : Controller
    {
        // Dependency Injection
        private IConfiguration _configuration;

        public OrderController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public IActionResult OrderEdit(int OrderID)
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

            #region Customer Drop-Down

            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_Customer_DropDown";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            connection2.Close();

            List<CustomerDropDownModel> customers = new List<CustomerDropDownModel>();

            foreach (DataRow dataRow in dataTable2.Rows)
            {
                CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                customerDropDownModel.CustomerID = Convert.ToInt32(dataRow["CustomerID"]);
                customerDropDownModel.CustomerName = dataRow["CustomerName"].ToString();
                customers.Add(customerDropDownModel);
            }

            ViewBag.CustomerList = customers;

            #endregion 

            #region UserID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectByPK";
            command.Parameters.AddWithValue("@OrderID", OrderID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderModel orderModel = new OrderModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderModel.OrderDate = Convert.ToDateTime(@dataRow["OrderDate"]);
                orderModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                orderModel.PaymentMode = @dataRow["PaymentMode"].ToString();
                orderModel.TotalAmount = Convert.ToDouble(@dataRow["TotalAmount"]);
                orderModel.ShippingAddress = Convert.ToString(dataRow["ShippingAddress"]);
                orderModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }
            #endregion

            return View("OrderAddEdit", orderModel);
        }



        public IActionResult OrderSave(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderModel.OrderID == null)
                {
                    command.CommandText = "PR_Order_Insert";
                }
                else
                {
                    command.CommandText = "PR_Order_UpdateByPK";
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderModel.OrderID;
                }
                command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderModel.OrderDate;
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = orderModel.CustomerID;
                command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = orderModel.PaymentMode;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderModel.TotalAmount;
                command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = orderModel.ShippingAddress;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderList");

            }
            return View("OrderAddEdit", orderModel);
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


        public List<CustomerDropDownModel> GetCustomerDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);

            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Customer_DropDown";

            SqlDataReader reader1 = command1.ExecuteReader();

            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            // Create a list of customerDropDownModel
            List<CustomerDropDownModel> customerList = new List<CustomerDropDownModel>();

            // Input new data in customerDropDownModel object and add into List
            foreach (DataRow data in dataTable1.Rows)
            {
                CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                customerDropDownModel.CustomerID = Convert.ToInt32(data["CustomerID"]);
                customerDropDownModel.CustomerName = data["CustomerName"].ToString();
                customerList.Add(customerDropDownModel);
            }

            return customerList;
        }


        public IActionResult OrderList()
        {
            // Connect SQL
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }


        public IActionResult OrderDelete(int OrderID)
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
                command.CommandText = "PR_Order_DeleteByPK";

                // Add parameters to procedure
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;

                // ExecuteNonQuery() : used to execute a SQL query that does not return any data, such as INSERT, UPDATE, or DELETE statements.
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Redirect to View - ProductList
            return RedirectToAction("OrderList");
        }


        public IActionResult OrderAddEdit()
        {
            ViewBag.UserList = GetUserDropDown();
            ViewBag.CustomerList = GetCustomerDropDown();
            return View();
        }


        public IActionResult Save(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderList");
            }
            else
            {
                return View("OrderAddEdit", orderModel);
            }
        }
    }
}
