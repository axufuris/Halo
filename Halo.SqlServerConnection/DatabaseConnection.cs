using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Halo.SqlServerConnection
{
    public class DatabaseConnection : BaseConnection, IDisposable
    {
        private SqlConnection connection;
        private string connectionString;
        private bool keepConnection;

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
        /// </summary>
        public DatabaseConnection()
        {
            string encryptedConnection = connectionString;

            try
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
                    encryptedConnection = listConnectionStrings[0];
                }
            }
            catch
            {
                encryptedConnection = connectionString;
            }

            String connectionSt = encryptedConnection;
            connection = new SqlConnection(connectionSt);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public DatabaseConnection(string connectionStringName)
        {
            string encryptedConnection = connectionString;

            encryptedConnection = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            encryptedConnection = connectionString;

            String connectionSt = encryptedConnection;
            connection = new SqlConnection(connectionSt);
        }

        private bool GetIsConnected()
        {
            return (connection.State == ConnectionState.Open);
        }

        public bool Open()
        {
            connection.ConnectionString = connectionString;
            connection.Open();
            return (connection.State == ConnectionState.Open);
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public SqlConnection Connection
        {
            get
            {
                return connection;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DatabaseConnection"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected
        {
            get
            {
                return GetIsConnected();
            }
            set
            {
                SetIsConnected(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [keep connection].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [keep connection]; otherwise, <c>false</c>.
        /// </value>
        public bool KeepConnection
        {
            get
            {
                return keepConnection;
            }
            set
            {
                keepConnection = value;
            }
        }

        private void SetIsConnected(bool value)
        {
            if (value)
            {
                if (!(GetIsConnected()))
                {
                    Open();
                }
            }
            else
            {
                if (GetIsConnected())
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// This function returns a populated DataTable based on the 
        /// passed sql stored procedure and list of parameters.
        /// Good implementations of this Method is for Select statements.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure.</param>
        /// <returns>Populated Datatable based on the passed parameterized sql select command.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public DataTable Execute(string storedProcedureName)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                try
                {
                    //Does the stored procedure have an owner?  If not add dbo.
                    if (!(storedProcedureName.Contains(".")))
                    {
                        storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                    }

                    connection.Open();
                    da = new SqlDataAdapter(storedProcedureName, connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.Fill(dt);
                    connection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// This function returns a populated DataTable based on the 
        /// passed sql stored procedure and list of parameters. 
        /// Good implementations of this Method is for Select statements.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Populated Datatable based on the passed parameterized sql select command.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public DataTable Execute(string storedProcedureName, SqlParameter[] parameters)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                try
                {
                    //Does the stored procedure have an owner?  If not add dbo.
                    if (!(storedProcedureName.Contains(".")))
                    {
                        storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                    }

                    connection.Open();
                    da = new SqlDataAdapter(storedProcedureName, connection);

                    if (parameters != null)
                    {
                        da.SelectCommand.Parameters.AddRange(parameters);
                    }

                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.Fill(dt);
                    connection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// This function returns a populated DataTable based on the 
        /// passed sql stored procedure and list of parameters.   
        /// Good implementations of this Method is for Select statements.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Populated Datatable based on the passed parameterized sql select command.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public DataTable Execute(string storedProcedureName, List<SqlParameter> parameters)
        {
            DataTable dt = null;

            if ((parameters != null) && (parameters.Count > 0))
            {
                dt = Execute(storedProcedureName, parameters.ToArray());
            }
            else
            {
                dt = Execute(storedProcedureName);
            }

            return dt;
        }

        /// <summary>
        /// This function returns a populated DataTable based on the 
        /// passed sql select statement/command and list of parameters.  
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>Populated Datatable based on the passed parameterized sql select command.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public DataTable ExecuteSQL(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            if (!(String.IsNullOrEmpty(sql)))
            {
                try
                {
                    connection.Open();
                    da = new SqlDataAdapter(sql, connection);
                    da.Fill(dt);
                    connection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// This function returns a populated DataTable based on the 
        /// passed sql select statement/command and list of parameters.  
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Populated Datatable based on the passed parameterized sql select command.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public DataTable ExecuteSQL(string sql, SqlParameter[] parameters)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            if (!(String.IsNullOrEmpty(sql)))
            {
                try
                {
                    connection.Open();
                    da = new SqlDataAdapter(sql, connection);

                    if (parameters != null)
                    {
                        da.SelectCommand.Parameters.AddRange(parameters);
                    }

                    da.Fill(dt);
                    connection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// This function returns a populated DataTable based on the 
        /// passed sql select statement/command and list of parameters.   
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Populated Datatable based on the passed parameterized sql select command.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public DataTable ExecuteSQL(string sql, List<SqlParameter> parameters)
        {
            DataTable dt = null;

            if ((parameters != null) && (parameters.Count > 0))
            {
                dt = ExecuteSQL(sql, parameters.ToArray());
            }
            else
            {
                dt = ExecuteSQL(sql);
            }

            return dt;
        }

        /// <summary>
        /// This procedure executes the passed sql stored procedure
        /// and sends along the list of parameters.
        /// </summary>
        /// <param name="storedProcedureName">The stored procedure.</param>
        /// <returns>Returns the amount of rows affected</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public void Execute(string storedProcedureName, ref int rowsAffected)
        {
            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                try
                {
                    //Does the stored procedure have an owner?  If not add dbo.
                    if (!(storedProcedureName.Contains(".")))
                    {
                        storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                    }

                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    //if (!(command.Parameters.Contains("Result")))
                    //{
                    //    command.Parameters.Add("Result", SqlDbType.Int);
                    //    command.Parameters["Result"].Direction = ParameterDirection.ReturnValue;
                    //}

                    rowsAffected = command.ExecuteNonQuery();
                    //rowsAffected = Int32.Parse(command.Parameters["Result"].Value.ToString());
                    connection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// This procedure executes the passed sql stored procedure, 
        /// sends along the list of parameters and returns the 
        /// amount of rows that were affected.
        /// Use this method for executing data modification 
        /// scripts (Update/Insert/Delete) and need/want to determine
        /// how many records were affected.  
        /// </summary>
        /// <param name="sql">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public void Execute(string storedProcedureName, SqlParameter[] parameters, ref int rowsAffected)
        {
            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                try
                {
                    //Does the stored procedure have an owner?  If not add dbo.
                    if (!(storedProcedureName.Contains(".")))
                    {
                        storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                    }

                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    //if (!(command.Parameters.Contains("Result")))
                    //{
                    //    command.Parameters.Add("Result", SqlDbType.Int);
                    //    command.Parameters["Result"].Direction = ParameterDirection.ReturnValue;
                    //}

                    rowsAffected = command.ExecuteNonQuery();
                    //rowsAffected = Int32.Parse(command.Parameters["Result"].Value.ToString());
                    connection.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        ///// <summary>
        ///// This procedure executes the passed sql command text
        ///// and sends along the list of parameters.
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <returns>Returns the amount of rows affected</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public void ExecuteSQL(string sql, ref int rowsAffected)
        //{
        //    if (!(String.IsNullOrEmpty(sql)))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            SqlCommand command = new SqlCommand(sql, connection);
        //            rowsAffected = command.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (connection.State == ConnectionState.Open)
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// This procedure executes the passed sql command text, 
        ///// sends along the list of parameters and returns the 
        ///// amount of rows that were affected.
        ///// Use this method for executing data modification 
        ///// scripts (Update/Insert/Delete) and need/want to determine
        ///// how many records were affected.  
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns></returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public void ExecuteSQL(string sql, SqlParameter[] parameters, ref int rowsAffected)
        //{
        //    if (!(String.IsNullOrEmpty(sql)))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            SqlCommand command = new SqlCommand(sql, connection);
        //            command.Parameters.AddRange(parameters);
        //            rowsAffected = command.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (connection.State == ConnectionState.Open)
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// This function executes the passed sql stored procedure 
        /// and sends along the list of parameters returning 
        /// a populated SqlDataReader class.  
        /// Warning, you can only have one active SqlDataReader per connection.
        /// When you are done with the reader, be sure to call Close().
        /// </summary>
        /// <param name="storedProcedureName">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public SqlDataReader ExecuteDataReader(string storedProcedureName, SqlParameter[] parameters)
        {
            SqlDataReader reader = null;

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                try
                {
                    //Does the stored procedure have an owner?  If not add dbo.
                    if (!(storedProcedureName.Contains(".")))
                    {
                        storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                    }

                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    reader = command.ExecuteReader();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return reader;
        }

        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns></returns>
        public SqlDataReader ExecuteDataReader(string storedProcedureName)
        {
            SqlDataReader reader = null;

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                try
                {
                    //Does the stored procedure have an owner?  If not add dbo.
                    if (!(storedProcedureName.Contains(".")))
                    {
                        storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                    }

                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    reader = command.ExecuteReader();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return reader;
        }

        /// <summary>
        /// This function executes the passed sql command text 
        /// and sends along the list of parameters returning 
        /// a populated SqlDataReader class.  
        /// Warning, you can only have one active SqlDataReader per connection.
        /// When you are done with the reader, be sure to call Close().
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public SqlDataReader ExecuteDataReaderSQL(string sql, SqlParameter[] parameters)
        {
            SqlDataReader reader = null;

            if (!(String.IsNullOrEmpty(sql)))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddRange(parameters);
                    reader = command.ExecuteReader();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return reader;
        }

        public SqlDataReader ExecuteDataReaderSQL(string sql)
        {
            SqlDataReader reader = null;

            if (!(String.IsNullOrEmpty(sql)))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return reader;
        }

        ///// <summary>
        ///// This function returns the rows affected from executing the
        ///// sql stored procedure that returns no recordset (such as an insert
        ///// or delete statement).    
        ///// Good implementation for this Procedure is for Update Methods.
        ///// In stored procedure put "RETURN @@ROWCOUNT;" to return affected rows.
        ///// </summary>
        ///// <param name="storedProcedureName">The stored procedure.</param>
        ///// <returns>Integer result of the rows effected message from executing the SQL.</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public int ExecuteCommand(string storedProcedureName)
        //{
        //    int rowsAffected = 0;
        //    Execute(storedProcedureName, ref rowsAffected);

        //    return rowsAffected;
        //}

        ///// <summary>
        ///// This function returns the rows affected from executing the
        ///// sql stored procedure and parameters that returns no recordset 
        ///// (such as an insertor delete statement).   
        ///// Good implementation for this Procedure is for Update Methods.
        ///// In stored procedure put "RETURN @@ROWCOUNT;" to return affected rows.
        ///// </summary>
        ///// <param name="storedProcedureName">The stored procedure.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns>Integer result of the rows effected message from executing the SQL.</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public int ExecuteCommand(string storedProcedureName, SqlParameter[] parameters)
        //{
        //    int rowsAffected = 0;
        //    Execute(storedProcedureName, parameters, ref rowsAffected);

        //    return rowsAffected;
        //}

        ///// <summary>
        ///// This function returns the rows affected from executing the
        ///// sql stored procedure and parameters that returns no recordset 
        ///// (such as an insertor delete statement).   
        ///// Good implementation for this Procedure is for Update Methods.
        ///// In stored procedure put "RETURN @@ROWCOUNT;" to return affected rows.
        ///// </summary>
        ///// <param name="storedProcedureName">The stored procedure.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns>Integer result of the rows effected message from executing the SQL.</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public int ExecuteCommand(string storedProcedureName, List<SqlParameter> parameters)
        //{
        //    int rowsAffected = 0;

        //    Execute(storedProcedureName, parameters.ToArray(), ref rowsAffected);

        //    return rowsAffected;
        //}

        ///// <summary>
        ///// This function returns the rows affected from executing the
        ///// sql statement that returns no recordset (such as an insert
        ///// or delete statement).    
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <returns>Integer result of the rows effected message from executing the SQL.</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public int ExecuteCommandSQL(string sql)
        //{
        //    int rowsAffected = 0;
        //    ExecuteSQL(sql, ref rowsAffected);

        //    return rowsAffected;
        //}

        ///// <summary>
        ///// This function returns the rows affected from executing the
        ///// sql statement and parameters that returns no recordset 
        ///// (such as an insertor delete statement).   
        ///// 
        ///// This function returns the rows affected from executing the
        ///// sql statement that returns no recordset (such as an insert
        ///// or delete statement).
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns>Integer result of the rows effected message from executing the SQL.</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public int ExecuteCommandSQL(string sql, SqlParameter[] parameters)
        //{
        //    int rowsAffected = 0;
        //    ExecuteSQL(sql, parameters, ref rowsAffected);

        //    return rowsAffected;
        //}

        ///// <summary>
        ///// This function returns the rows affected from executing the
        ///// sql statement and parameters that returns no recordset 
        ///// (such as an insertor delete statement).   
        ///// 
        ///// This function returns the rows affected from executing the
        ///// sql statement that returns no recordset (such as an insert
        ///// or delete statement).
        ///// </summary>
        ///// <param name="sql">The SQL.</param>
        ///// <param name="parameters">The parameters.</param>
        ///// <returns>Integer result of the rows effected message from executing the SQL.</returns>
        ///// <author>xufurisa</author>
        ///// <createdate>4/4/2011</createdate>
        //public int ExecuteCommandSQL(string sql, List<SqlParameter> parameters)
        //{
        //    int rowsAffected = 0;

        //    ExecuteSQL(sql, parameters.ToArray(), ref rowsAffected);

        //    return rowsAffected;
        //}

        /// <summary>
        /// This function returns the result from executing the
        /// passed stored procedure name and optional list of parameters.   
        /// Good implementation for this method is for Create Statements.
        /// To return the int use "return SCOPE_IDENTITY()" in stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Integer result from executing stored procedure.  If the stored procedure doesn't
        /// explicitly return a value, the default is zero.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public int ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters)
        {
            int result = 0;

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                //Does the stored procedure have an owner?  If not add dbo.
                if (!(storedProcedureName.Contains(".")))
                {
                    storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                }

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    if (!(command.Parameters.Contains("Result")))
                    {
                        command.Parameters.Add("Result", SqlDbType.Int);
                        command.Parameters["Result"].Direction = ParameterDirection.ReturnValue;
                    }

                    command.ExecuteNonQuery();
                    result = Int32.Parse(command.Parameters["Result"].ToString());
                    connection.Close();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This function returns the result from executing the
        /// passed stored procedure name and optional list of parameters.
        /// Good implementation for this method is for Create Statements.
        /// To return the int use "return SCOPE_IDENTITY()" in stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Integer result from executing stored procedure.  If the stored procedure doesn't
        /// explicitly return a value, the default is zero.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public int ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {
            int result = 0;

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                //Does the stored procedure have an owner?  If not add dbo.
                if (!(storedProcedureName.Contains(".")))
                {
                    storedProcedureName = String.Format("dbo.{0}", storedProcedureName);
                }

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    if (!(command.Parameters.Contains("Result")))
                    {
                        command.Parameters.Add("Result", SqlDbType.Int);
                        command.Parameters["Result"].Direction = ParameterDirection.ReturnValue;
                    }

                    command.ExecuteScalar();
                    result = Convert.ToInt32(command.Parameters["Result"].Value.ToString());
                    connection.Close();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This function returns the result from executing the
        /// passed stored procedure name and optional list of parameters.  
        /// Good implementation for this method is for Create Statements.
        /// To return the int use "return SCOPE_IDENTITY()" in stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>Integer result from executing stored procedure.  If the stored procedure doesn't
        /// explicitly return a value, the default is zero.</returns>
        /// <author>xufurisa</author>
        /// <createdate>4/4/2011</createdate>
        public int ExecuteStoredProcedure(string storedProcedureName)
        {
            int result = 0;

            if (!(String.IsNullOrEmpty(storedProcedureName)))
            {
                //Does the stored procedure have an owner?  If not add dbo.
                if (!(storedProcedureName.Contains(".")))
                {
                    storedProcedureName = String.Format(CultureInfo.CurrentCulture, "dbo.{0}", storedProcedureName);
                }

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(storedProcedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    if (!(command.Parameters.Contains("Result")))
                    {
                        command.Parameters.Add("Result", SqlDbType.Int);
                        command.Parameters["Result"].Direction = ParameterDirection.ReturnValue;
                    }

                    command.ExecuteScalar();
                    result = Convert.ToInt32(command.Parameters["Result"].Value.ToString(), CultureInfo.CurrentCulture);
                    connection.Close();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        #region IDisposable Implementation

        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release disposable objects used by this instance here.

                    if (connection != null)
                        connection.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }  /// End of Class
}
