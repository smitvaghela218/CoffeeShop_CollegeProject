using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CoffeeShop.Controllers
{
    public class ProductController : Controller
    {
        // Dependency Injection
        private IConfiguration _configuration;

        public ProductController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }
        public IActionResult ProductSave(ProductModel productModel)
        {
            if (productModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (productModel.ProductID == null)
                {
                    command.CommandText = "PR_Product_Insert";
                }
                else
                {
                    command.CommandText = "PR_Product_UpdateByPK";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
                }
                command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("ProductList");
            }

            return View("ProductAddEdit", productModel);
        }


        public IActionResult ProductEdit(int ProductID)
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

            #region ProductByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Product_SelectByPK";
            command.Parameters.AddWithValue("@ProductID", ProductID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            ProductModel productModel = new ProductModel();

            foreach (DataRow dataRow in table.Rows)
            {
                productModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                productModel.ProductName = @dataRow["ProductName"].ToString();
                productModel.ProductCode = @dataRow["ProductCode"].ToString();
                productModel.ProductPrice = Convert.ToDouble(@dataRow["ProductPrice"]);
                productModel.Description = @dataRow["Description"].ToString();
                productModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("ProductAddEdit", productModel);
        }

        public IActionResult ProductDelete(int ProductID)
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
                command.CommandText = "PR_Product_DeleteByPK";

                // Add parameters to procedure
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;

                // ExecuteNonQuery() : used to execute a SQL query that does not return any data, such as INSERT, UPDATE, or DELETE statements.
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Redirect to View - ProductList
            return RedirectToAction("ProductList");
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


        public IActionResult ProductList()
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
            command.CommandText = "PR_Product_SelectAll";

            // ExecuteReader() : To read data from database
            SqlDataReader reader = command.ExecuteReader();

            // Create table and load(fill) data in table
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }

        public IActionResult ProductAddEdit()
        {
            ViewBag.UserList = GetUserDropDown();
            return View();
        }


       
       }
}
