using System.Data;
using Dapper;

namespace Candidate.Core.Widgets.Dapper;

public interface IDapperWidget
{
    List<T> CallProcedure<T>(string procedureName, DynamicParameters parameters, string serverName = "CandidateLogDB");

    Task<List<T>> CallProcedureAsync<T>(string procedureName, DynamicParameters parameters, string serverName = "CandidateLogDB");

    List<T> CallProcedure<T>(string procedureName, string connectionString, DynamicParameters parameters);

    Task<List<T>> CallProcedureAsync<T>(string procedureName, string connectionString, DynamicParameters parameters);

    DataTable CallProcedure(string procedureName, DynamicParameters parameters, string serverName = "CandidateLogDB");

    DataTable CallProcedure(string procedureName, string connectionString, DynamicParameters parameters);

    List<T> RunQuery<T>(string query, DynamicParameters parameters, string serverName = "CandidateLogDB");

    List<T> RunQuery<T>(string query, string connectionString, DynamicParameters parameters);

}