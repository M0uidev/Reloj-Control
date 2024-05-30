// This class is responsible for interacting with the database to perform operations related to users.
using Reloj_Control.Models;
using System.Data.SqlClient;

public class UsersDAO
{
    // Connection string to connect to the database.
    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    // This method checks if a user with the provided username and password exists in the database.
    public bool FindUserByNameAndPassword(UserModel user)
    {
        // SQL query to find a user with the provided username and password.
        string sqlStatement = "SELECT * FROM dbo.Users WHERE username = @username AND password = @password";

        // Using a SqlConnection to connect to the database.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Creating a SqlCommand to execute the SQL query.
            SqlCommand command = new SqlCommand(sqlStatement, connection);

            // Adding parameters to the command.
            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = user.UserName;
            command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 40).Value = user.Password;

            try
            {
                // Opening the connection and executing the command.
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // If the reader has rows, it means a user with the provided username and password exists.
                if (reader.HasRows)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                // Logging any exceptions that occur.
                Console.WriteLine(e.Message);
            }
        }

        // If no user was found, return false.
        return false;
    }

    // This method registers a new user in the database.
    public bool Register(UserModel user)
    {
        // SQL query to check if a user with the provided username already exists.
        string sqlStatement = "SELECT COUNT(*) FROM dbo.Users WHERE username = @username";

        // Using a SqlConnection to connect to the database.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Creating a SqlCommand to execute the SQL query.
            SqlCommand command = new SqlCommand(sqlStatement, connection);

            // Adding a parameter to the command.
            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = user.UserName;

            try
            {
                // Opening the connection and executing the command.
                connection.Open();
                int existingUsers = (int)command.ExecuteScalar();

                // If a user with the provided username already exists, throw an exception.
                if (existingUsers > 0)
                {
                    throw new Exception("Username already exists.");
                }
            }
            catch (Exception e)
            {
                // Logging any exceptions that occur.
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // SQL query to insert a new user into the database.
        sqlStatement = "INSERT INTO dbo.Users (username, password) VALUES (@username, @password)";

        // Using a SqlConnection to connect to the database.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Creating a SqlCommand to execute the SQL query.
            SqlCommand command = new SqlCommand(sqlStatement, connection);

            // Adding parameters to the command.
            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = user.UserName;
            command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 40).Value = user.Password;

            try
            {
                // Opening the connection and executing the command.
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                // If the command affected any rows, it means the user was successfully registered.
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                // Logging any exceptions that occur.
                Console.WriteLine(e.Message);
            }
        }

        // If the user was not successfully registered, return false.
        return false;
    }
}
