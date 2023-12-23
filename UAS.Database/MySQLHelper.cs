using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using UAS.Dependancies.Database;

namespace UAS.Database
{
    public class MySQLHelper : IMySQLHelper
    {
        public readonly string _dbConnString = "Server=localhost;Database=urja_system;Uid=root;Pwd=admin123;";

        public MySqlConnection? _sqlConnection;
        public bool _isInTransaction;
        public MySqlTransaction? _sqlTransaction;
        public long _sqlTransactionID;
        private DateTime _startTime;
        private DateTime _endTime;
        private MySqlCommand? _sqlCommand;
        private List<MySqlParameter> _sqlParameterList = new List<MySqlParameter>();

        public MySQLHelper()
        {
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
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public long BeginTrans()
        {
            long l_lng_BeginTrans = 0;

            _sqlConnection = new MySqlConnection(_dbConnString);

            try
            {
                if (_sqlConnection != null && (_sqlConnection.ConnectionString == "" || string.IsNullOrEmpty(_sqlConnection.ConnectionString)))
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                }
                if (_isInTransaction == false)
                {
                    //Get Start time of Transaction
                    _startTime = DateTime.Now;

                    _sqlTransactionID = _sqlTransactionID + 1;
                    _isInTransaction = true;

                    //_sqlConnection.Open();
                    //_sqlTransaction = _sqlConnection.BeginTransaction();

                    l_lng_BeginTrans = _sqlTransactionID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

                        _sqlConnection.Close();
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
                        _sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlCommand? Get_sqlCommand()
        {
            return _sqlCommand;
        }

        public DataSet GetDataTableByID(string storedProcedureName, string sqlParameterName, int sqlParmeterValue, MySqlCommand? _sqlCommand)
        {

            try
            {
                using (_sqlConnection = new MySqlConnection())
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                    _sqlConnection.Open();
                    DataTable tableTemp = new DataTable();
                    DataSet ds = new DataSet();

                    using (_sqlCommand = new MySqlCommand())
                    {
                        _sqlCommand.Connection = _sqlConnection;
                        _sqlCommand.CommandText = storedProcedureName;
                        _sqlCommand.CommandType = CommandType.StoredProcedure;
                        _sqlCommand.Parameters.Add(
                            new MySqlParameter(string.Format("@{0}", sqlParameterName), SqlDbType.Int)
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
                    _sqlConnection.Close();
                }
            }
            return (l_ds_Data);
        }

        public DataSet ReturnWithDataSet(MySqlParameter[] paras, string cmdText)
        {
            if (_dbConnString == "")
            {

                _sqlConnection = new MySqlConnection(_dbConnString);
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
                    _sqlConnection.Close();
                }
            }
            return (l_ds_Data);
        }

        public DataSet return_DataSet(string str_SqlCommand)
        {
            try
            {
                using (_sqlConnection = new MySqlConnection())
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                    _sqlConnection.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter())
                    {
                        using (_sqlCommand = new MySqlCommand())
                        {
                            _sqlCommand.Connection = _sqlConnection;
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
            catch (MySqlException e)
            {
                throw e;
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
                using (_sqlConnection = new MySqlConnection())
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                    _sqlConnection.Open();
                    MySqlDataReader reader_Temp;
                    using (_sqlCommand = new MySqlCommand())
                    {
                        DataTable Table_Temp = new DataTable();
                        _sqlCommand.Connection = _sqlConnection;
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
            catch (MySqlException e)
            {
                throw e;
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

                _sqlConnection = new MySqlConnection(_dbConnString);
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
                    _sqlConnection.Close();
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
                using (_sqlConnection = new MySqlConnection())
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                    _sqlConnection.Open();

                    MySqlCommand objCommand = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = _sqlConnection
                    };
                    i = objCommand.ExecuteNonQuery();
                    executionSucceeded = true;
                }
            }
            catch (Exception ex)
            {
                executionSucceeded = false;
                throw ex;
            }
            return i;
        }

        public object ExecuteScalar(string query)
        {

            try
            {
                using (_sqlConnection = new MySqlConnection())
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                    _sqlConnection.Open();

                    MySqlCommand objCommand = new MySqlCommand
                    {
                        CommandType = CommandType.Text,
                        CommandText = query,
                        Connection = _sqlConnection
                    };
                    var retval = objCommand.ExecuteScalar();
                    return retval;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar( string cmdText, List<MySqlParameter> parameters)
        {
            try
            {
                if (parameters.Count == 0) parameters = _sqlParameterList;

                using (_sqlConnection = new MySqlConnection())
                {
                    _sqlConnection.ConnectionString = _dbConnString;
                    _sqlConnection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(cmdText, _sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters.ToArray());
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteNonQuery(string cmdText, List<MySqlParameter> paras)
        {
            if (paras.Count == 0) paras = _sqlParameterList;

            _sqlConnection = new MySqlConnection(_dbConnString);
            int ReturnID = 0;
            MySqlCommand l_sql_Cmd = new MySqlCommand();

            try
            {
                l_sql_Cmd = CreateCommandObject(paras.ToArray(), cmdText);
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
                    _sqlConnection.Close();
                }
            }

            return (ReturnID);
        }

        public MySqlConnection CloseConnection()
        {
            if (_sqlConnection.State == ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
            return _sqlConnection;
        }

        public MySqlCommand CreateCommandObject(MySqlParameter[] paras, string cmdText)
        {

            _sqlConnection = new MySqlConnection(_dbConnString);
            if (_sqlConnection != null && (_sqlConnection.ConnectionString == "" || string.IsNullOrEmpty(_sqlConnection.ConnectionString)))
            {
                _sqlConnection.ConnectionString = _dbConnString;
            }

            MySqlCommand l_sql_Cmd = new MySqlCommand();
            try
            {
                l_sql_Cmd.CommandType = CommandType.StoredProcedure;
                l_sql_Cmd.CommandText = cmdText;

                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                l_sql_Cmd.Connection = _sqlConnection;

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
            catch (Exception ex)
            {
                throw ex;
            }

            return (l_sql_Cmd);
        }

        public MySqlCommand CreateCommandObject(MySqlParameter para, string cmdText)
        {

            _sqlConnection = new MySqlConnection(_dbConnString);
            if (_sqlConnection != null && (_sqlConnection.ConnectionString == "" || string.IsNullOrEmpty(_sqlConnection.ConnectionString)))
            {
                _sqlConnection.ConnectionString = _dbConnString;
            }
            MySqlCommand l_sql_Cmd = new MySqlCommand();
            try
            {
                l_sql_Cmd.CommandType = CommandType.StoredProcedure;
                l_sql_Cmd.CommandText = cmdText;

                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                l_sql_Cmd.Connection = _sqlConnection;

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
            catch (Exception ex)
            {
                throw ex;
            }

            return (l_sql_Cmd);
        }

        public string GetProcedureName(string table, Enums.OperationType operation)
        {
            var procedureName = "proc" + "_" + table + "_" + operation;
            return procedureName;
        }

        public DataSet GetDataTableByID(string storedProcedureName, string sqlParameterName, int sqlParmeterValue)
        {
            throw new NotImplementedException();
        }

        public void AddMySQLParameter(string pName, string pValue)
        {
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = pName;
            param.Value = pValue;

            _sqlParameterList.Add(new MySqlParameter(pName, pValue));
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
            FieldInfo fi = value.GetType().GetField(value.ToString());

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
