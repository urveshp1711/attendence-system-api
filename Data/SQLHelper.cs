using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace UAS.Data
{
    public interface IMySQLHelper
    {
        bool GetDBConnectionStatus(bool prevConnState);
        DataSet GetDataTableByID(string storedProcedureName, string parameterName, int parmeterValue);
        long BeginTrans();
        object ExecuteNonQuery(MySqlParameter[] paras, string cmdText);
        void CommitTrans(long transID);
        void RollBackTrans(long transID);
        DataSet ReturnWithDataSet(MySqlParameter para, string cmdText);
        DataSet ReturnWithDataSet(MySqlParameter[] paras, string cmdText);
        DataTable ReturnWithDataTable(MySqlParameter[] paras, string cmdText);
        int Return_CUD_flag(MySqlParameter[] parameters, string procedure);
        int ExecuteNonQuery(string query, ref bool executionSucceeded);
        object ExecuteScalar(string query);
        MySqlConnection? CloseConnection();
        MySqlCommand CreateCommandObject(MySqlParameter[] paras, string cmdText);
        MySqlCommand CreateCommandObject(MySqlParameter para, string cmdText);
        string GetProcedureName(string table, Enums.OperationType operation);
        DataSet return_DataSet(string str_SqlCommand);
        DataTable return_DataTable(string str_SqlCommand);
        object ReturnScalarValue(MySqlParameter[] parameters, string cmdText);
    }

    public class MySQLHelper : IMySQLHelper
    {
        public readonly string? _dbConnString;

        public MySqlConnection? _connection;
        public bool _isInTransaction;
        public MySqlTransaction? _sqlTransaction;
        public long _sqlTransactionID;
        private MySqlCommand? _sqlCommand;

        private DateTime _startTime;
        private DateTime _endTime;


        public MySQLHelper(string? dbConnString)
        {
            _dbConnString = dbConnString;
        }

        public bool GetDBConnectionStatus(bool prevConnState)
        {
            if (string.IsNullOrEmpty(_dbConnString))
            {
                return false;
            }
            using (MySqlConnection conn = new MySqlConnection(_dbConnString))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    throw ;
                }
            }
        }

        public long BeginTrans()
        {
            long l_lng_BeginTrans = 0;
            using (_connection = new MySqlConnection(_dbConnString))
            {
                if (_connection != null && (_connection.ConnectionString == "" || string.IsNullOrEmpty(_connection.ConnectionString)))
                {
                    _connection.ConnectionString = _dbConnString;
                    if (_isInTransaction == false)
                    {
                        //Get Start time of Transaction
                        _startTime = DateTime.Now;

                        _sqlTransactionID = _sqlTransactionID + 1;
                        _isInTransaction = true;

                        _connection.Open();
                        _sqlTransaction = _connection?.BeginTransaction();

                        l_lng_BeginTrans = _sqlTransactionID;
                    }
                }
            }
            return l_lng_BeginTrans;
        }

        public void CommitTrans(long transID)
        {
            try
            {
                if (_isInTransaction == true)
                {
                    if (_sqlTransactionID == transID)
                    {
                        _sqlTransaction.Commit();
                        _isInTransaction = false;

                        _connection?.Close();
                        //Get End Time When Main Transaction is Done
                        _endTime = DateTime.Now;
                        //Get end time
                        //get transaction time in miliseconds
                        //reset start time and end time
                        _startTime = DateTime.Now;
                        _endTime = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RollBackTrans(long transID)
        {
            try
            {
                if (_isInTransaction == true)
                {
                    if (_sqlTransactionID == transID)
                    {
                        _sqlTransaction.Rollback();
                        _isInTransaction = false;
                        _connection?.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataTableByID(string storedProcedureName, string MySqlParameterName, int sqlParmeterValue)
        {

            try
            {
                using (_connection = new MySqlConnection())
                {
                    _connection.ConnectionString = _dbConnString;
                    _connection?.Open();
                    DataTable tableTemp = new DataTable();
                    DataSet ds = new DataSet();

                    using (_sqlCommand = new MySqlCommand())
                    {
                        _sqlCommand.Connection = _connection;
                        _sqlCommand.CommandText = storedProcedureName;
                        _sqlCommand.CommandType = CommandType.StoredProcedure;
                        _sqlCommand.Parameters.Add(
                            new MySqlParameter(string.Format("@{0}", MySqlParameterName), SqlDbType.Int)
                            {
                                Value = sqlParmeterValue
                            });
                        MySqlDataAdapter myAdapter = new MySqlDataAdapter(_sqlCommand);
                        myAdapter.Fill(ds);

                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlCommand.Connection = CloseConnection();
            }
        }

        public DataSet ReturnWithDataSet(MySqlParameter para, string cmdText)
        {
            DataSet l_ds_Data = new DataSet();
            MySqlCommand l_sql_Cmd = new MySqlCommand();
            MySqlDataAdapter l_sql_da = new MySqlDataAdapter();
            try
            {
                l_sql_Cmd = CreateCommandObject(para, cmdText);
                l_sql_da.SelectCommand = l_sql_Cmd;
                l_sql_da.Fill(l_ds_Data);
            }
            catch (Exception ex)
            {
                l_ds_Data = null;
                throw ex;
            }
            finally
            {
                if (_isInTransaction == false)
                {
                    _connection?.Close();
                }
            }
            return (l_ds_Data);
        }

        public DataSet ReturnWithDataSet(MySqlParameter[] paras, string cmdText)
        {
            if (_dbConnString == "")
            {

                _connection = new MySqlConnection(_dbConnString);
            }
            DataSet l_ds_Data = new DataSet();
            MySqlCommand l_sql_Cmd = new MySqlCommand();
            MySqlDataAdapter l_sql_da = new MySqlDataAdapter();
            try
            {
                l_sql_Cmd = CreateCommandObject(paras, cmdText);
                l_sql_da.SelectCommand = l_sql_Cmd;
                l_sql_da.Fill(l_ds_Data);
            }
            catch (Exception ex)
            {
                l_ds_Data = null;
                throw ex;
            }
            finally
            {
                if (_isInTransaction == false)
                {
                    _connection?.Close();
                }
            }
            return (l_ds_Data);
        }

        public DataSet return_DataSet(string str_SqlCommand)
        {
            try
            {
                using (_connection = new MySqlConnection())
                {
                    _connection.ConnectionString = _dbConnString;
                    _connection?.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter())
                    {
                        using (_sqlCommand = new MySqlCommand())
                        {
                            _sqlCommand.Connection = _connection;
                            _sqlCommand.CommandText = str_SqlCommand;
                            _sqlCommand.CommandType = CommandType.Text;
                            da.SelectCommand = _sqlCommand;

                            using (DataSet ds = new DataSet())
                            {
                                da.Fill(ds, "table1");
                                return ds;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _sqlCommand.Connection = CloseConnection();
            }
        }

        public DataTable return_DataTable(string str_SqlCommand)
        {
            try
            {
                using (_connection = new MySqlConnection())
                {
                    _connection.ConnectionString = _dbConnString;
                    _connection?.Open();
                    MySqlDataReader reader_Temp;
                    using (_sqlCommand = new MySqlCommand())
                    {
                        DataTable Table_Temp = new DataTable();
                        _sqlCommand.Connection = _connection;
                        _sqlCommand.CommandText = str_SqlCommand;
                        _sqlCommand.CommandType = CommandType.Text;
                        reader_Temp = _sqlCommand.ExecuteReader();
                        if (reader_Temp.HasRows == true)
                        {
                            Table_Temp.Load(reader_Temp);
                            reader_Temp.Close();
                            return Table_Temp;
                        }
                        else
                        {
                            reader_Temp.Close();
                            return Table_Temp;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _sqlCommand.Connection = CloseConnection();
            }
        }

        public DataTable ReturnWithDataTable(MySqlParameter[] paras, string cmdText)
        {
            if (_dbConnString == "")
            {

                _connection = new MySqlConnection(_dbConnString);
            }
            DataTable l_ds_Data = new DataTable();
            MySqlCommand l_sql_Cmd = new MySqlCommand();
            MySqlDataAdapter l_sql_da = new MySqlDataAdapter();
            try
            {
                l_sql_Cmd = CreateCommandObject(paras, cmdText);
                l_sql_da.SelectCommand = l_sql_Cmd;
                l_sql_da.Fill(l_ds_Data);
            }
            catch (Exception ex)
            {
                l_ds_Data = null;
                throw ex;
            }
            finally
            {
                if (_isInTransaction == false)
                {
                    _connection?.Close();
                }
            }
            return (l_ds_Data);
        }

        public int Return_CUD_flag(MySqlParameter[] parameters, string procedure)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd = CreateCommandObject(parameters, procedure);
            int returnFlag = cmd.ExecuteNonQuery();
            return returnFlag;
        }

        public int ExecuteNonQuery(string query, ref bool executionSucceeded)
        {
            int i = -1;

            try
            {
                using (_connection = new MySqlConnection())
                {
                    _connection.ConnectionString = _dbConnString;
                    _connection?.Open();

                    MySqlCommand objCommand = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = _connection
                    };
                    i = objCommand.ExecuteNonQuery();
                    executionSucceeded = true;
                }
            }
            catch
            {
                executionSucceeded = false;
                throw;
            }
            return i;
        }

        public object ExecuteScalar(string query)
        {

            try
            {
                using (_connection = new MySqlConnection())
                {
                    _connection.ConnectionString = _dbConnString;
                    _connection?.Open();

                    MySqlCommand objCommand = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = _connection
                    };
                    var retval = objCommand.ExecuteScalar();
                    return retval;
                }
            }
            catch
            {
                throw;
            }
        }

        public object ReturnScalarValue(MySqlParameter[] parameters, string cmdText)
        {
            try
            {
                using (_connection = new MySqlConnection())
                {
                    _connection.ConnectionString = _dbConnString;
                    _connection?.Open();

                    using (MySqlCommand cmd = new MySqlCommand(cmdText, _connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public object ExecuteNonQuery(MySqlParameter[] paras, string cmdText)
        {

            _connection = new MySqlConnection(_dbConnString);
            int ReturnID = 0;
            MySqlCommand l_sql_Cmd = new MySqlCommand();

            try
            {
                l_sql_Cmd = CreateCommandObject(paras, cmdText);
                ReturnID = l_sql_Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_isInTransaction == false)
                {
                    _connection?.Close();
                }
            }

            return (ReturnID);
        }

        public MySqlConnection? CloseConnection()
        {
            if (_connection?.State == ConnectionState.Open)
            {
                _connection?.Close();
            }
            return _connection;
        }

        public MySqlCommand CreateCommandObject(MySqlParameter[] paras, string cmdText)
        {

            _connection = new MySqlConnection(_dbConnString);
            if (_connection != null && (_connection.ConnectionString == "" || string.IsNullOrEmpty(_connection.ConnectionString)))
            {
                _connection.ConnectionString = _dbConnString;
            }

            MySqlCommand l_sql_Cmd = new MySqlCommand();
            try
            {
                l_sql_Cmd.CommandType = CommandType.StoredProcedure;
                l_sql_Cmd.CommandText = cmdText;

                if (_connection?.State == ConnectionState.Closed)
                {
                    _connection?.Open();
                }

                l_sql_Cmd.Connection = _connection;

                ////if (m_bln_IsInTransaction == true)
                ////{
                ////    l_sql_Cmd.Transaction = m_trn_Transaction;
                ////}

                if (paras == null)
                {
                    return (l_sql_Cmd);
                }

                foreach (MySqlParameter para in paras)
                {
                    l_sql_Cmd.Parameters.Add(para);
                }
            }
            catch
            {
                throw;
            }

            return (l_sql_Cmd);
        }

        public MySqlCommand CreateCommandObject(MySqlParameter para, string cmdText)
        {

            _connection = new MySqlConnection(_dbConnString);
            if (_connection != null && (_connection.ConnectionString == "" || string.IsNullOrEmpty(_connection.ConnectionString)))
            {
                _connection.ConnectionString = _dbConnString;
            }
            MySqlCommand l_sql_Cmd = new MySqlCommand();
            try
            {
                l_sql_Cmd.CommandType = CommandType.StoredProcedure;
                l_sql_Cmd.CommandText = cmdText;

                if (_connection?.State == ConnectionState.Closed)
                {
                    _connection?.Open();
                }

                l_sql_Cmd.Connection = _connection;

                //if (m_bln_IsInTransaction == true)
                //{
                //    l_sql_Cmd.Transaction = m_trn_Transaction;
                //}

                if (para == null)
                {
                    return (l_sql_Cmd);
                }

                l_sql_Cmd.Parameters.Add(para);
            }
            catch
            {
                throw;
            }

            return (l_sql_Cmd);
        }

        public string GetProcedureName(string table, Enums.OperationType operation)
        {
            var procedureName = "proc" + "_" + table + "_" + operation;
            return procedureName;
        }
    }

    public class Enums
    {
        public enum DefaultText
        {
            [Description("--Select--")]
            DropDownDefaultText = 0,

            [Description("Please select any instance")]
            SelectInstance,
        }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public enum OperationType
        {
            Insert = 1,
            Update = 2,
            Delete = 3,
            GetAll = 4
        }
    }
}
