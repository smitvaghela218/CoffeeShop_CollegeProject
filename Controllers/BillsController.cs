using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using MailKit.Search;

namespace CoffeeShop.Controllers
{
    public class BillsController : Controller
    {
        // Dependency Injection
        private IConfiguration _configuration;

        public BillsController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public IActionResult BillEdit(int BillID)
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
                     
            #region BillID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectByPK";
            command.Parameters.AddWithValue("@BillID", BillID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            BillsModel billsModel = new BillsModel();

            foreach (DataRow dataRow in table.Rows)
            {
                billsModel.BillID= Convert.ToInt32(@dataRow["BillID"]);
                billsModel.BillNumber = Convert.ToString(@dataRow["BillNumber"]);
                billsModel.BillDate = Convert.ToDateTime(@dataRow["BillDate"]);
                billsModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                billsModel.TotalAmount = Convert.ToDouble(@dataRow["TotalAmount"]);
                billsModel.Discount = Convert.ToInt32(@dataRow["Discount"]);
                billsModel.NetAmount = Convert.ToInt32(@dataRow["NetAmount"]);
                billsModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                // billsModel.OrderDate = Convert.ToDateTime(@dataRow["Amount"]);

            }

            #endregion

            return View("BillAddEdit", billsModel);
        }



        public IActionResult BillSave(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (billsModel.BillID == null)
                {
                    command.CommandText = "PR_Bills_Insert";
                }
                else
                {
                    command.CommandText = "PR_Bills_UpdateByPK";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = billsModel.BillID;
                }
                command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billsModel.BillNumber;
                command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billsModel.OrderID;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = billsModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("BillsList");

            }
            return View("BillsAddEdit", billsModel);
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

        public IActionResult BillsList()
        {
            // Connect SQL
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }


        public IActionResult BillDelete(int BillID)
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
                command.CommandText = "PR_Bills_DeleteByPK";

                // Add parameters to procedure
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;

                // ExecuteNonQuery() : used to execute a SQL query that does not return any data, such as INSERT, UPDATE, or DELETE statements.
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Redirect to View - ProductList
            return RedirectToAction("BillsList");
        }


        public IActionResult BillAddEdit()
        {
            ViewBag.UserList = GetUserDropDown();
            ViewBag.OrderList = GetOrderDropDown();
            return View();
        }


        public IActionResult Save(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("BillsList");
            }
            else
            {
                return View("BillAddEdit", billsModel);
            }
        }
    }
}
