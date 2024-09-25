using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using MimeKit.Encodings;

namespace CoffeeShop.Controllers
{
    public class CustomerController : Controller
    {
        // Dependency Injection
        private IConfiguration _configuration;

        public CustomerController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public IActionResult CustomerEdit(int CustomerID)
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


            #region CustomerID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_SelectByPK";
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CustomerModel customerModel = new CustomerModel();

            foreach (DataRow dataRow in table.Rows)
            {
                customerModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                customerModel.CustomerName = @dataRow["CustomerName"].ToString();
                customerModel.HomeAddress = @dataRow["HomeAddress"].ToString();
                customerModel.Email = Convert.ToString(@dataRow["Email"]);
                customerModel.MobileNo = @dataRow["MobileNo"].ToString();
                customerModel.GST_NO = Convert.ToString(@dataRow["GST_NO"]);
                customerModel.CityName = Convert.ToString(dataRow["CityName"]);
                customerModel.PinCode = Convert.ToString(dataRow["PinCode"]);
                customerModel.UserID = Convert.ToInt32(dataRow["UserID"]);
            }
            #endregion

            return View("CustomerAddEdit", customerModel);
        }


        public IActionResult CustomerSave(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (customerModel.CustomerID == null)
                {
                    command.CommandText = "PR_Customer_Insert";
                }
                else
                {
                    command.CommandText = "PR_Customer_UpdateByPK";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerModel.CustomerID;
                }
                command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerModel.CustomerName;
                command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = customerModel.HomeAddress;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = customerModel.Email;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = customerModel.MobileNo;
                command.Parameters.Add("@GST_NO", SqlDbType.VarChar).Value = customerModel.GST_NO;
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = customerModel.CityName;
                command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = customerModel.PinCode;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customerModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = customerModel.UserID;

                command.ExecuteNonQuery();
                return RedirectToAction("CustomerList");

            }
            return View("CustomerAddEdit", customerModel);
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


        public IActionResult CustomerList()
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
            command.CommandText = "PR_Customer_SelectAll";

            // ExecuteReader() : To read data from database
            SqlDataReader reader = command.ExecuteReader();

            // Create table and load(fill) data in table
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }


        public IActionResult CustomerDelete(int CustomerID)
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
                command.CommandText = "PR_Customer_DeleteByPK";

                // Add parameters to procedure
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;

                // ExecuteNonQuery() : used to execute a SQL query that does not return any data, such as INSERT, UPDATE, or DELETE statements.
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Redirect to View - ProductList
            return RedirectToAction("CustomerList");
        }


        public IActionResult CustomerAddEdit()
        {
            ViewBag.UserList = GetUserDropDown();
            return View();
        }


        public IActionResult Save(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("CustomerList");
            }
            else
            {
                return View("CustomerAddEdit", customerModel);
            }
        }
    }
}
