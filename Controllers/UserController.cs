using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CoffeeShop.Controllers
{
    public class UserController : Controller
    {
        // Dependency Injection
        private IConfiguration _configuration;

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult UserRegister(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Register";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                    sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
                    sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userRegisterModel.Address;
                    sqlCommand.ExecuteNonQuery();
                    return RedirectToAction("Login", "User");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }
            return RedirectToAction("Register");
        }
        public UserController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        //public IActionResult UserLogin(UserModel userModel)
        //{
        //    string connectionString = this._configuration.GetConnectionString("ConnectionString"); // From appsettings.json file
        //    SqlConnection connection = new SqlConnection(connectionString);

        //    // Open connection
        //    connection.Open();

        //    // Create command
        //    SqlCommand command = connection.CreateCommand();

        //    // Define command type - here, StoredProcedure
        //    command.CommandType = CommandType.StoredProcedure;

        //    // Define command text - here, ProcedureName
        //    command.CommandText = "PR_User_Login";

        //    // ExecuteReader() : To read data from database
        //    SqlDataReader reader = command.ExecuteReader();

        //    // Create table and load(fill) data in table
        //    DataTable table = new DataTable();
        //    table.Load(reader);

        //}
            
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public IActionResult Login() { 
            return View();
        }
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }

                        return RedirectToAction("ProductList", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }

        public IActionResult UserEdit(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");


            #region UserID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectByPK";
            command.Parameters.AddWithValue("@UserID", UserID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            UserModel userModel = new UserModel();

            foreach (DataRow dataRow in table.Rows)
            {
                userModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                userModel.UserName = @dataRow["UserName"].ToString();
                userModel.Email = @dataRow["Email"].ToString();
                userModel.Password= Convert.ToString(@dataRow["Password"]);

                userModel.MobileNo = @dataRow["MobileNo"].ToString();
                userModel.Address = Convert.ToString(@dataRow["Address"]);
                userModel.IsActive = Convert.ToBoolean(dataRow["IsActive"]);

            }

            #endregion

            return View("UserAddEdit", userModel);
        }


        public IActionResult UserList()
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
            command.CommandText = "PR_User_SelectAll";

            //command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = ;

            // ExecuteReader() : To read data from database
            SqlDataReader reader = command.ExecuteReader();

            // Create table and load(fill) data in table
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }

        public IActionResult UserAddEdit()
        {
            return View();
        }

        public IActionResult UserDelete(int UserID)
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
                command.CommandText = "PR_User_DeleteByPK";

                // Add parameters to procedure
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

                // ExecuteNonQuery() : used to execute a SQL query that does not return any data, such as INSERT, UPDATE, or DELETE statements.
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Redirect to View - ProductList
            return RedirectToAction("UserList");
        }

        public IActionResult UserSave(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (userModel.UserID == null)
                {
                    command.CommandText = "PR_User_Insert";
                }
                else
                {
                    command.CommandText = "PR_User_UpdateByPK";
                    command.Parameters.Add("@UserID",SqlDbType.Int).Value = userModel.UserID;
                }
                command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                command.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
                command.ExecuteNonQuery();
                return RedirectToAction("UserList");   

            }
            return View("UserAddEdit", userModel);
        }
        
    }
}
