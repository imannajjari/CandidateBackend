using System.Data;
using Candidate.Core.Widgets.Config;
using Candidate.Core.Widgets.Convertor;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Candidate.Core.Widgets.Dapper;

public class DapperWidget : IDapperWidget
{
    private readonly List<ConnectionStringViewModel> _connectionStrings;
    public DapperWidget()
    {
        var connectionStrings = new List<ConnectionStringViewModel>
        {
            new()
            {
                ServerName = "CandidateLogDB",
                ConnectionString = ConfigWidget.GetConfigValue<string>("ConnectionStrings:CandidateLogDBConnectionString")
            },
            new()
            {
                ServerName = "CandidateDB",
                ConnectionString = ConfigWidget.GetConfigValue<string>("ConnectionStrings:CandidateDBConnectionString")
            },

        };

        _connectionStrings = connectionStrings;
    }


    // =====================================================================================
    // =====================================================================================
    // =====================================================================================
    // ===================================================================================== Procedure

    public List<T> CallProcedure<T>(string procedureName, DynamicParameters parameters, string serverName)
    {
        var server = _connectionStrings.FirstOrDefault(x => x.ServerName.ToLower() == serverName.ToLower());
        if (server != null)
        {
            var result = CallProcedure<T>(procedureName, server.ConnectionString, parameters);
            return result;
        }
        else
        {
            return new List<T>();
        }
    }


    public async Task< List<T>> CallProcedureAsync<T>(string procedureName, DynamicParameters parameters, string serverName)
    {
        var server = _connectionStrings.FirstOrDefault(x => x.ServerName.ToLower() == serverName.ToLower());
        if (server != null)
        {
            var result =await CallProcedureAsync<T>(procedureName, server.ConnectionString, parameters);
            return result;
        }
        else
        {
            return new List<T>();
        }
    }


    public async Task<List<T>> CallProcedureAsync<T>(string procedureName, string connectionString, DynamicParameters parameters)
    {

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var result = await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 600);
            return result.ToList();
        }
    }


    public List<T> CallProcedure<T>(string procedureName, string connectionString, DynamicParameters parameters)
    {

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection.Query<T>(procedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 600).ToList();
    }


    public DataTable CallProcedure(string procedureName, DynamicParameters parameters, string serverName)
    {
        var list = CallProcedure<dynamic>(procedureName, parameters, serverName);
        var dt = ConvertorWidget.ToDataTable(procedureName, list);
        return dt;
    }

    public DataTable CallProcedure(string procedureName, string connectionString, DynamicParameters parameters)
    {
        var list = CallProcedure<dynamic>(procedureName, connectionString, parameters);
        var dt = ConvertorWidget.ToDataTable(procedureName, list);
        return dt;
    }


    // =====================================================================================
    // =====================================================================================
    // =====================================================================================
    // ===================================================================================== Query

    public List<T> RunQuery<T>(string query, DynamicParameters parameters, string serverName)
    {

        var server = _connectionStrings.SingleOrDefault(x => x.ServerName.ToLower() == serverName.ToLower());
        if (server != null)
        {
            var result = RunQuery<T>(query, server.ConnectionString, parameters);
            return result;
        }
        else
        {
            return new List<T>();
        }
    }

    public List<T> RunQuery<T>(string query, string connectionString, DynamicParameters parameters)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        return connection.Query<T>(query, parameters, commandType: CommandType.Text, commandTimeout: 600).ToList();
    }


}