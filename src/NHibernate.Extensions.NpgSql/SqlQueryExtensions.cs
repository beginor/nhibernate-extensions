using System;
using NHibernate;
using NHibernate.UserTypes;
using NHibernate.Type;

namespace NHibernate.Extensions.NpgSql;

public static class SqlQueryExtensions {

    public static ISQLQuery AddCustomScalar<TUserType>(
        this ISQLQuery query,
        string columnAlias
    ) where TUserType : IUserType {
        query.AddScalar(
            columnAlias,
            new CustomType(typeof(TUserType), null)
        );
        return query;
    }

    public static ISQLQuery SetCustomParameter<TUserType>(
        this ISQLQuery query,
        string name,
        object val
    ) where TUserType : IUserType {
        query.SetParameter(
            name,
            val,
            new CustomType(typeof(TUserType), null)
        );
        return query;
    }

    public static ISQLQuery SetCustomParameter<TUserType>(
        this ISQLQuery query,
        int position,
        object val
    ) where TUserType : IUserType {
        query.SetParameter(
            position,
            val,
            new CustomType(typeof(TUserType), null)
        );
        return query;
    }

}
