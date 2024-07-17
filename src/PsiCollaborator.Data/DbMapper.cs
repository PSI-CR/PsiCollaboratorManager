using PsiCollaborator.Data.DataBaseConnection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data
{
    public class DbMapper
    {
        private DbCommand _command { get; set; }
        private DbConnection _connection { get; set; }
        protected string UserAccountID { get; set; }
        private void open()
        {
            try
            {
                _connection = DataBaseConfigurator.CreateDbConnection();
                _connection.Open();
            }
            catch (Exception ex)
            {
                close();
                throw new InvalidOperationException(string.Format("Could not connect to the database. {0}", ex.Message), ex);
            }
        }

        private void close()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        private object executeProcedure(string procedureName, ExecuteType executeType, List<DbParameter> parameters, CommandType commandType)
        {
            object returnObject = null;
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _command = _connection.CreateCommand();
                    _command.CommandText = procedureName.ToLower();
                    _command.CommandType = commandType;
                    _command.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        _command.Parameters.Clear();
                        foreach (var dbParameter in parameters)
                        {
                            var parameter = _command.CreateParameter();
                            parameter.ParameterName = "@" + dbParameter.Name.ToLower();
                            parameter.Direction = dbParameter.Direction;
                            parameter.Value = dbParameter.Value;
                            if (!dbParameter.DbType.Equals(DbType.AnsiString))
                                parameter.DbType = dbParameter.DbType;
                            _command.Parameters.Add(parameter);
                        }
                    }
                    switch (executeType)
                    {
                        case ExecuteType.ExecuteReader:

                            returnObject = _command.ExecuteReader();
                            break;
                        case ExecuteType.ExecuteNonQuery:
                            returnObject = _command.ExecuteNonQuery();
                            break;
                        case ExecuteType.ExecuteScalar:
                            returnObject = _command.ExecuteScalar();
                            break;
                        default:
                            break;
                    }
                }
            }
            return returnObject;
        }

        private void updateOutParameters()
        {
            if (_command.Parameters.Count > 0)
            {
                OutParameters = new List<DbParameter>();
                OutParameters.Clear();
                for (int i = 0; i < _command.Parameters.Count; i++)
                {
                    var parameter = _command.Parameters[i] as IDbDataParameter;
                    if (parameter.Direction == ParameterDirection.Output)
                        OutParameters.Add(new DbParameter(parameter.ParameterName, ParameterDirection.Output, parameter.Value));
                }
            }
        }

        private int executeSqlMapObject<T>(string procedureName, T entity, List<DbParameter> listParameters, params string[] ignoreParameters)
        {
            if (string.IsNullOrEmpty(procedureName))
                throw new ArgumentException("procedureName is null or empty.", "procedureName");
            if (entity == null)
                throw new ArgumentNullException("entity", "entity is null.");
            var parameters = new List<DbParameter>();
            foreach (var prop in entity.GetType().GetProperties())
            {
                if (ignoreParameters.Contains(prop.Name))
                    continue;
                var name = "param_" + prop.Name;
                var value = prop.GetValue(entity);
                if (value != null)
                    parameters.Add(new DbParameter(name, ParameterDirection.Input, value));
                else
                    parameters.Add(new DbParameter(name, ParameterDirection.Input, DBNull.Value));
            }
            parameters.AddRange(listParameters);
            return ExecuteSql(procedureName, parameters);
        }

        protected enum ExecuteType
        {
            ExecuteReader,
            ExecuteNonQuery,
            ExecuteScalar
        };

        protected List<DbParameter> OutParameters { get; private set; }

        protected int ExecuteSql(string procedureName, List<DbParameter> parameters, ExecuteType executeType = ExecuteType.ExecuteNonQuery, CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(procedureName))
                throw new ArgumentException("procedureName is null or empty.", "procedureName");
            int returnValue;
            open();
            returnValue = (int)executeProcedure(procedureName, executeType, parameters, commandType);
            updateOutParameters();
            close();
            return returnValue;
        }

        protected int ExecuteSqlMapObject<T>(string procedureName, T entity)
        {
            return executeSqlMapObject(procedureName, entity, new List<DbParameter>());
        }

        protected int ExecuteSqlMapObject<T>(string procedureName, T entity, params string[] ignoreParameters)
        {
            return executeSqlMapObject(procedureName, entity, new List<DbParameter>(), ignoreParameters);
        }

        protected int ExecuteSqlMapObject<T>(string procedureName, T entity, DbParameter outputParameter, params string[] ignoreParameters)
        {
            return executeSqlMapObject(procedureName, entity, new List<DbParameter>() { outputParameter }, ignoreParameters);
        }
        protected int ExecuteSqlMapObject<T>(string procedureName, T entity, List<DbParameter> parameters)
        {
            return executeSqlMapObject(procedureName, entity, parameters);
        }


        protected IEnumerable<T> ExecuteList<T>(string procedureName) where T : new()
        {
            if (string.IsNullOrEmpty(procedureName))
                throw new ArgumentException("procedureName is null or empty.", "procedureName");
            return ExecuteListWithParameters<T>(procedureName, null);
        }

        protected IEnumerable<T> ExecuteListWithParameters<T>(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure) where T : new()
        {
            if (string.IsNullOrEmpty(procedureName))
                throw new ArgumentException("procedureName is null or empty.", "procedureName");
            open();
            IDataReader reader = (IDataReader)executeProcedure(procedureName, ExecuteType.ExecuteReader, parameters, commandType);
            var listObjects = new List<T>();
            while (reader.Read())
            {
                T tempObject = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.GetValue(i);
                    if (value != DBNull.Value)
                    {
                        var name = reader.GetName(i);
                        var propertyInfo = typeof(T).GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null)
                        {
                            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                            propertyInfo.SetValue(tempObject, (value == null) ? null : Convert.ChangeType(value, type), null);
                        }
                    }
                }
                listObjects.Add(tempObject);
            }
            reader.Close();
            updateOutParameters();
            close();
            return listObjects;
        }

        protected T ExecuteSingle<T>(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure) where T : new()
        {
            if (string.IsNullOrEmpty(procedureName))
                throw new ArgumentException("procedureName is null or empty.", "procedureName");
            open();
            IDataReader reader = (IDataReader)executeProcedure(procedureName, ExecuteType.ExecuteReader, parameters, commandType);
            T tempObject = default(T);
            if (reader.Read())
            {
                tempObject = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.GetValue(i);
                    if (value != DBNull.Value)
                    {
                        var name = reader.GetName(i);
                        var propertyInfo = typeof(T).GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null)
                        {
                            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                            propertyInfo.SetValue(tempObject, (value == null) ? null : Convert.ChangeType(value, type), null);
                        }
                    }
                }
            }
            reader.Close();
            updateOutParameters();
            close();
            return tempObject;
        }
    }
}
