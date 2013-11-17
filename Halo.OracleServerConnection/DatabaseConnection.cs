using Oracle.DataAccess.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Halo.OracleServerConnection
{
    public class DatabaseConnection
    {
        private OracleConnection connection;
        private string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseConnection"/> class.
        /// </summary>
        public DatabaseConnection()
        {
            List<string> listConnectionStrings = new List<string>();

            foreach (ConnectionStringSettings _connection in ConfigurationManager.ConnectionStrings)
            {
                if (_connection.Name.ToLower() != "localsqlserver"
                        && !string.IsNullOrEmpty(_connection.ConnectionString.Trim()))
                {
                    listConnectionStrings.Add(_connection.ConnectionString);
                    break;
                }
            }

            if (listConnectionStrings.Count > 0)
            {
                connectionString = listConnectionStrings[0];
            }

            try
            {
                connection = new OracleConnection(connectionString);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseConnection"/> class.
        /// </summary>
        /// <param name="oracleConnectionString">The oracle connection string.</param>
        public DatabaseConnection(string oracleConnectionString)
        {
            connectionString = oracleConnectionString;

            connection = new OracleConnection(connectionString);
        }

        /// <summary>
        /// Executes the reader.
        /// Best Used for select packages
        /// </summary>
        /// <param name="storedProcedureName">The command text.</param>
        /// <returns>Returns a datatable</returns>
        public DataTable Execute(string storedProcedureName)
        {
            DataTable dataTable = new DataTable();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();

            try
            {
                dataAdapter.SelectCommand = connection.CreateCommand();
                dataAdapter.SelectCommand.CommandText = storedProcedureName;
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                connection.Open();
                //dataAdapter.SelectCommand.ExecuteReader();
                dataAdapter.Fill(dataTable);
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// Executes the reader.
        /// Best Used for select packages
        /// </summary>
        /// <param name="storedProcedureName">The command text.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns a Datatable.</returns>
        public DataTable Execute(string storedProcedureName, List<OracleParameter> parameter)
        {
            DataTable dataTable = new DataTable();
            OracleDataAdapter dataAdapter = new OracleDataAdapter();

            try
            {
                dataAdapter.SelectCommand = connection.CreateCommand();
                dataAdapter.SelectCommand.CommandText = storedProcedureName;
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddRange(parameter.ToArray());
                connection.Open();
                //dataAdapter.SelectCommand.ExecuteReader();
                dataAdapter.Fill(dataTable);
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// Executes the non query.
        /// Example use is for Insert, Update, or Delete Stored Procedures
        /// </summary>
        /// <param name="storedProcedureName">The command text.</param>
        /// <returns>Returns the int of affected rows or the last inserted row. (You must tell the package to return it.)</returns>
        public int ExecuteStoredProcedure(string storedProcedureName)
        {
            try
            {
                OracleCommand oracleCommand = connection.CreateCommand();
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.CommandText = storedProcedureName;
                connection.Open();
                return oracleCommand.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the non query.
        /// Example use is for Insert, Update, or Delete Stored Procedures
        /// </summary>
        /// <param name="storedProcedureName">The command text.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns the int of affected rows or the last inserted row. (You must tell the package to return it.)</returns>
        public int ExecuteStoredProcedure(string storedProcedureName, List<OracleParameter> parameter)
        {
            try
            {
                OracleCommand oracleCommand = connection.CreateCommand();
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.CommandText = storedProcedureName;
                oracleCommand.Parameters.AddRange(parameter.ToArray());
                connection.Open();
                return oracleCommand.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes the scaler.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>Returns an Object.</returns>
        public object ExecuteScaler(string commandText)
        {
            object returnedObject;

            try
            {
                OracleCommand oracleCommand = connection.CreateCommand();
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.CommandText = commandText;
                connection.Open();
                returnedObject = oracleCommand.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return returnedObject;
        }

        /// <summary>
        /// Executes the scaler.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns an object.</returns>
        public object ExecuteScaler(string commandText, List<OracleParameter> parameter)
        {
            object returnedObject;

            try
            {
                OracleCommand oracleCommand = connection.CreateCommand();
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.CommandText = commandText;
                oracleCommand.Parameters.AddRange(parameter.ToArray());
                connection.Open();
                returnedObject = oracleCommand.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return returnedObject;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>The Connection Object</returns>
        public static DatabaseConnection GetConnection()
        {
            return new DatabaseConnection();
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The Connection Object.</returns>
        public static DatabaseConnection GetConnection(string connectionString)
        {
            return new DatabaseConnection(connectionString);
        }
    }  /// End of Class
}
