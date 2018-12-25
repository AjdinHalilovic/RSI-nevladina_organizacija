using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public static class DbConnectionExtension
    {
        #region Query

        public static T QueryFirstOrDefault<T>(this DbConnection connection, string sql, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return connection.QueryFirstOrDefault<T>(sql, dynamicParameters, commandType: CommandType.Text);
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this DbConnection connection, string sql, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return await connection.QueryFirstOrDefaultAsync<T>(sql, dynamicParameters, commandType: CommandType.Text);
        }

        public static IEnumerable<T> Query<T>(this DbConnection connection, string sql, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return connection.Query<T>(sql, dynamicParameters, commandType: CommandType.Text);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this DbConnection connection, string sql, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return await connection.QueryAsync<T>(sql, dynamicParameters, commandType: CommandType.Text);
        }

        #endregion

        #region Query function

        public static T QueryFunctionFirstOrDefault<T>(this DbConnection connection, string functionName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return connection.QueryFirstOrDefault<T>(functionName, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        public static async Task<T> QueryFunctionFirstOrDefaultAsync<T>(this DbConnection connection, string functionName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return await connection.QueryFirstOrDefaultAsync<T>(functionName, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        public static IEnumerable<T> QueryFunction<T>(this DbConnection connection, string functionName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return connection.Query<T>(functionName, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        public static async Task<IEnumerable<T>> QueryFunctionAsync<T>(this DbConnection connection, string functionName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            return await connection.QueryAsync<T>(functionName, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Execute function

        public static void ExecuteFunction(this DbConnection connection, string functionName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            connection.Execute(functionName, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        public static async Task ExecuteFunctionAsync(this DbConnection connection, string functionName, object parameters)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (var parameter in parameters.GetType().GetProperties())
                dynamicParameters.Add(parameter.Name, parameter.GetValue(parameters));

            await connection.ExecuteAsync(functionName, dynamicParameters, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
