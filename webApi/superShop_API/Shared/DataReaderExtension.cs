using System.Data.SqlClient;
using System.Linq.Expressions;
using Npgsql;

namespace superShop_API.Shared;

public static class NpgsqlDataReaderExtension
{

    public static List<T> ToList<T>(this NpgsqlDataReader reader)
    {
        var result = new List<T>();
        var readRow = reader.GetReader<T>();

        while (reader.Read())
            result.Add(readRow(reader));

        return result;
    }

    private static Func<NpgsqlDataReader, T> GetReader<T>(this NpgsqlDataReader reader)
    {
        var readerColums = new List<string>();

        for (int i = 0; i < reader.FieldCount; i++)
            readerColums.Add(reader.GetName(i));

        // determine the information about the reader
        var readerParam = Expression.Parameter(typeof(NpgsqlDataReader), "reader");
        var readerGetValue = typeof(NpgsqlDataReader).GetMethod("GetValue");

        // create a Constant expression of DBNull.Value to compare values to in reader
        var dbNullValue = typeof(DBNull).GetField("Value");
        var dbNullExp = Expression.Field(Expression.Parameter(typeof(DBNull), "DBNull"), dbNullValue);

        var memberBindings = new List<MemberBinding>();
        foreach (var property in typeof(T).GetProperties())
        {
            // determine the default value of the property
            object defaultValue = null;
            if (property.PropertyType.IsValueType)
                defaultValue = Activator.CreateInstance(property.PropertyType);
            else if (property.PropertyType.Name.ToLower().Equals("string"))
                defaultValue = string.Empty;

            if (readerColums.Contains(property.Name))
            {
                // build the Call expression to retrieve the data value from the reader
                var indexExpression = Expression.Constant(reader.GetOrdinal(property.Name));
                var getValueExp = Expression.Call(readerParam, readerGetValue, new Expression[] { indexExpression });

                // create the conditional expression to make sure the reader value != DBNull.Value
                var testExp = Expression.NotEqual(dbNullExp, getValueExp);
                var ifTrue = Expression.Convert(getValueExp, property.PropertyType);
                var ifFalse = Expression.Convert(Expression.Constant(defaultValue), property.PropertyType);

                // create the actual Bind expression to bind the value from the reader to the property value
                var memberInfo = typeof(T).GetMember(property.Name)[0];
                var memberBinding = Expression.Bind(memberInfo, Expression.Condition(testExp, ifTrue, ifFalse));
                memberBindings.Add(memberBinding);
            }
        }

        // create a MemberInit expression for the item with the member bindings
        var newItem = Expression.New(typeof(T));
        var memberInit = Expression.MemberInit(newItem, memberBindings);

        var lambda = Expression.Lambda<Func<NpgsqlDataReader, T>>(memberInit, new ParameterExpression[] { readerParam });
        Delegate? @delegate = lambda.Compile();
        return (Func<NpgsqlDataReader, T>)@delegate;
    }
}

public static class SqlDataReaderExtension
{

    public static List<T> ToList<T>(this SqlDataReader reader)
    {
        var result = new List<T>();
        var readRow = reader.GetReader<T>();

        while (reader.Read())
            result.Add(readRow(reader));

        return result;
    }

    private static Func<SqlDataReader, T> GetReader<T>(this SqlDataReader reader)
    {
        var readerColums = new List<string>();

        for (int i = 0; i < reader.FieldCount; i++)
            readerColums.Add(reader.GetName(i));

        // determine the information about the reader
        var readerParam = Expression.Parameter(typeof(NpgsqlDataReader), "reader");
        var readerGetValue = typeof(NpgsqlDataReader).GetMethod("GetValue");

        // create a Constant expression of DBNull.Value to compare values to in reader
        var dbNullValue = typeof(DBNull).GetField("Value");
        var dbNullExp = Expression.Field(Expression.Parameter(typeof(DBNull), "DBNull"), dbNullValue);

        var memberBindings = new List<MemberBinding>();
        foreach (var property in typeof(T).GetProperties())
        {
            // determine the default value of the property
            object defaultValue = null;
            if (property.PropertyType.IsValueType)
                defaultValue = Activator.CreateInstance(property.PropertyType);
            else if (property.PropertyType.Name.ToLower().Equals("string"))
                defaultValue = string.Empty;

            if (readerColums.Contains(property.Name))
            {
                // build the Call expression to retrieve the data value from the reader
                var indexExpression = Expression.Constant(reader.GetOrdinal(property.Name));
                var getValueExp = Expression.Call(readerParam, readerGetValue, new Expression[] { indexExpression });

                // create the conditional expression to make sure the reader value != DBNull.Value
                var testExp = Expression.NotEqual(dbNullExp, getValueExp);
                var ifTrue = Expression.Convert(getValueExp, property.PropertyType);
                var ifFalse = Expression.Convert(Expression.Constant(defaultValue), property.PropertyType);

                // create the actual Bind expression to bind the value from the reader to the property value
                var memberInfo = typeof(T).GetMember(property.Name)[0];
                var memberBinding = Expression.Bind(memberInfo, Expression.Condition(testExp, ifTrue, ifFalse));
                memberBindings.Add(memberBinding);
            }
        }

        // create a MemberInit expression for the item with the member bindings
        var newItem = Expression.New(typeof(T));
        var memberInit = Expression.MemberInit(newItem, memberBindings);

        var lambda = Expression.Lambda<Func<NpgsqlDataReader, T>>(memberInit, new ParameterExpression[] { readerParam });
        Delegate? @delegate = lambda.Compile();
        return (Func<SqlDataReader, T>)@delegate;
    }
}