using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAS.Dependancies.Database
{
    public interface IMySQLHelper
    {
        bool GetDBConnectionStatus(bool prevConnState);
        DataSet GetDataTableByID(string storedProcedureName, string sqlParameterName, int sqlParmeterValue);
        long BeginTrans();
        object ExecuteNonQuery(string cmdText, List<MySqlParameter> paras);
        void CommitTrans(long transID);
        void RollBackTrans(long transID);
        DataSet ReturnWithDataSet(MySqlParameter para, string cmdText);
        DataSet ReturnWithDataSet(MySqlParameter[] paras, string cmdText);
        DataTable ReturnWithDataTable(string cmdText, List<MySqlParameter> parameters);
        int Return_CUD_flag(MySqlParameter[] parameters, string procedure);
        int ExecuteNonQuery(string query, ref bool executionSucceeded);
        object ExecuteScalar(string query);
        MySqlConnection CloseConnection();
        MySqlCommand CreateCommandObject(MySqlParameter[] paras, string cmdText);
        MySqlCommand CreateCommandObject(MySqlParameter para, string cmdText);
        //string GetProcedureName(string table, Enums.OperationType operation);
        DataSet return_DataSet(string str_SqlCommand);
        DataTable return_DataTable(string str_SqlCommand);
        object ExecuteScalar(string cmdText, List<MySqlParameter> parameters);

        void AddMySQLParameter(string pName, dynamic? pValue);
    }
}
