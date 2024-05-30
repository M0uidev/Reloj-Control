using Reloj_Control.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Reloj_Control.Services
{
    public class DataRelojDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool RegisterCheckIn(RelojControlCheckInModel model)
        {
            string checkSqlStatement = "SELECT COUNT(*) FROM dbo.CheckIn WHERE id = @id AND Joined = 0";
            string sqlStatement = "INSERT INTO dbo.CheckIn (id, fecha, HoraDeEntrada) VALUES (@id, @fecha, @horaDeEntrada)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand(checkSqlStatement, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id", model.Id);
                    int existingCount = (int)checkCommand.ExecuteScalar();

                    // If there is an existing checkIn that has not been combined with a checkOut, return false.
                    if (existingCount > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand command = new SqlCommand(sqlStatement, connection))
                {
                    // Extract the date and time from HoraEntrada.
                    string fecha = $"{model.HoraEntrada:dd-MM-yyyy}";
                    string horaDeEntrada = $"{model.HoraEntrada:HH:mm}";

                    command.Parameters.AddWithValue("@id", model.Id);
                    command.Parameters.AddWithValue("@fecha", DateTime.Parse(fecha));
                    command.Parameters.AddWithValue("@horaDeEntrada", TimeSpan.Parse(horaDeEntrada));

                    int rowsAffected = command.ExecuteNonQuery();

                    // If a row was inserted return true, otherwise return false.
                    return rowsAffected > 0;
                }
            }
        }

        public bool RegisterCheckOut(RelojControlCheckOutModel model)
        {
            string sqlStatement = "INSERT INTO dbo.CheckOut (id, fecha, HoraDeSalida) VALUES (@id, @fecha, @horaDeSalida)";
            string combinedDataSqlStatement = "INSERT INTO dbo.CombinedData (Id, Fecha, HoraEntrada, HoraSalida, HorasTotales) VALUES (@id, @fecha, @horaDeEntrada, @horaDeSalida, @horasTotales)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlStatement, connection))
                {
                    // Extract the date and time from HoraSalida.
                    string fecha = $"{model.HoraSalida:dd-MM-yyyy}";
                    string horaDeSalida = $"{model.HoraSalida:HH:mm}";

                    command.Parameters.AddWithValue("@id", model.Id);
                    command.Parameters.AddWithValue("@fecha", DateTime.Parse(fecha));
                    command.Parameters.AddWithValue("@horaDeSalida", TimeSpan.Parse(horaDeSalida));

                    int rowsAffected = command.ExecuteNonQuery();

                    // If a row was inserted, insert into CombinedData and return true, otherwise return false.
                    if (rowsAffected > 0)
                    {
                        string selectStatement = "SELECT TOP 1 HoraDeEntrada FROM dbo.CheckIn WHERE Id = @id AND Fecha = @fecha AND Joined = 0";
                        string updateStatement = "UPDATE dbo.CheckIn SET Joined = 1 WHERE Id = @id AND Fecha = @fecha AND Joined = 0";

                        TimeSpan horaDeEntrada;

                        using (SqlCommand selectCommand = new SqlCommand(selectStatement, connection))
                        {
                            selectCommand.Parameters.AddWithValue("@id", model.Id);
                            selectCommand.Parameters.AddWithValue("@fecha", DateTime.Parse(fecha));

                            object result = selectCommand.ExecuteScalar();

                            if (result != null)
                            {
                                horaDeEntrada = (TimeSpan)result;

                                using (SqlCommand updateCommand = new SqlCommand(updateStatement, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@id", model.Id);
                                    updateCommand.Parameters.AddWithValue("@fecha", DateTime.Parse(fecha));

                                    updateCommand.ExecuteNonQuery();
                                }

                                // Calculate HorasTotales
                                TimeSpan HorasTotales = TimeSpan.Parse(horaDeSalida) - horaDeEntrada;

                                using (SqlCommand combinedDataCommand = new SqlCommand(combinedDataSqlStatement, connection))
                                {
                                    combinedDataCommand.Parameters.AddWithValue("@id", model.Id);
                                    combinedDataCommand.Parameters.AddWithValue("@fecha", DateTime.Parse(fecha));
                                    combinedDataCommand.Parameters.AddWithValue("@horaDeEntrada", horaDeEntrada);
                                    combinedDataCommand.Parameters.AddWithValue("@horaDeSalida", TimeSpan.Parse(horaDeSalida));
                                    combinedDataCommand.Parameters.AddWithValue("@horasTotales", HorasTotales);

                                    combinedDataCommand.ExecuteNonQuery();
                                }

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    return false;
                }
            }
        }


    }
}
