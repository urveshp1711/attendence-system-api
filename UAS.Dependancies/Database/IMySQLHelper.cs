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
        object ExecuteNonQuery(MySqlParameter[] paras, string cmdText);
        void CommitTrans(long transID);
        void RollBackTrans(long transID);
        int UpdateObjectLite(object objBo, string tableName);
        DataSet ReturnWithDataSet(MySqlParameter para, string cmdText);
        DataSet ReturnWithDataSet(MySqlParameter[] paras, string cmdText);
        DataTable ReturnWithDataTable(MySqlParameter[] paras, string cmdText);
        int Return_CUD_flag(MySqlParameter[] parameters, string procedure);
        int ExecuteNonQuery(string query, ref bool executionSucceeded);
        object ExecuteScalar(string query);
        MySqlConnection CloseConnection();
        MySqlCommand CreateCommandObject(MySqlParameter[] paras, string cmdText);
        MySqlCommand CreateCommandObject(MySqlParameter para, string cmdText);
        //string GetProcedureName(string table, Enums.OperationType operation);
        DataSet return_DataSet(string str_SqlCommand);
        DataTable return_DataTable(string str_SqlCommand);
        object ReturnScalarValue(MySqlParameter[] parameters, string cmdText);
    }
}
