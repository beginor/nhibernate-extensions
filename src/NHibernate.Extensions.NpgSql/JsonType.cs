using System;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

    public class JsonType : IUserType {

        public virtual SqlType[] SqlTypes => new SqlType[] {
            new NpgSqlType(DbType.String, NpgsqlDbType.Json)
        };

        public System.Type ReturnedType => typeof(string);

        public bool IsMutable => true;

        public object Assemble(object cached, object owner) {
            var str = cached as string;
            if (string.IsNullOrWhiteSpace(str)) {
                return null;
            }
            return str;
        }

        public object DeepCopy(object value) {
            if (value == null) {
                return null;
            }
            var json = JsonConvert.SerializeObject(value);
            var obj = JsonConvert.DeserializeObject(json, value.GetType());
            return obj;
        }

        public object Disassemble(object value) {
            if (value == null) {
                return null;
            }
            return value;
        }

        public new bool Equals(object x, object y) {
            if (x == null && y == null) {
                return true;
            }
            if (x == null || y == null) {
                return false;
            }
            return ((string)x).Equals((string)y);
        }

        public int GetHashCode(object x) {
            if (x == null) {
                return 0;
            }
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner) {
            if (names.Length != 1) {
                throw new InvalidOperationException("Only expecting one column...");
            }
            var val = rs[names[0]] as string;
            if (!string.IsNullOrWhiteSpace(val)) {
                return val;
            }
            return null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session) {
            var parameter = (NpgsqlParameter)cmd.Parameters[index];
            if (value == null) {
                parameter.Value = DBNull.Value;
            }
            else {
                parameter.Value = value;
            }
        }

        public object Replace(object original, object target, object owner) {
            return original;
        }

    }

}
