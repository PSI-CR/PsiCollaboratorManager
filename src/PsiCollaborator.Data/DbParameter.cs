using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data
{
    public class DbParameter
    {
        public DbParameter(string paramName, ParameterDirection paramDirection, object paramValue)
        {
            Name = paramName;
            Direction = paramDirection;
            /*if(paramValue == null)
                Value = DBNull.Value;
            else*/
            Value = paramValue;
        }

        public DbParameter(string name, ParameterDirection direction, object value, DbType dbType)
        {
            Name = name;
            Direction = direction;
            Value = value;
            DbType = dbType;
        }

        public string Name { get; set; }

        public ParameterDirection Direction { get; set; }

        public object Value { get; set; }

        public DbType DbType { get; set; }
    }
}
