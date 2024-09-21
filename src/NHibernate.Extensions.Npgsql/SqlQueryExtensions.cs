using System;
using NHibernate.UserTypes;

namespace NHibernate.Extensions.Npgsql;

public static class SqlQueryExtensions {

    [Obsolete]
    public static ISQLQuery AddCustomScalar<TUserType>(
        this ISQLQuery query,
        string columnAlias
    ) where TUserType : IUserType {
        query.AddScalar(
            columnAlias,
            NHibernateUtil.Custom(typeof(TUserType))
        );
        return query;
    }

    [Obsolete]
    public static ISQLQuery SetCustomParameter<TUserType>(
        this ISQLQuery query,
        string name,
        object val
    ) where TUserType : IUserType {
        query.SetParameter(
            name,
            val,
            NHibernateUtil.Custom(typeof(TUserType))
        );
        return query;
    }

    [Obsolete]
    public static ISQLQuery SetCustomParameter<TUserType>(
        this ISQLQuery query,
        int position,
        object val
    ) where TUserType : IUserType {
        query.SetParameter(
            position,
            val,
            NHibernateUtil.Custom(typeof(TUserType))
        );
        return query;
    }

}
