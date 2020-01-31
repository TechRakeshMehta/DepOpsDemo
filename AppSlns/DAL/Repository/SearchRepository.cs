using DAL.Interfaces;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DAL.Repository
{
    public partial class SearchRepository : BaseSearchRepository, ISearchRepository
    {
        public IQueryable<T> QuickSearch<T>(List<string> fieldNames, string searchKeys, string orderByFieldName, bool exactMatch) where T : EntityObject
        {

            //To retrieve the EntitySet from Context
            String entitySetName =
                   base.Context.MetadataWorkspace.GetEntityContainer(base.Context.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                             .Where(bes => bes.ElementType.Name.Equals(typeof(T).Name)).FirstOrDefault().Name;


            return (new ObjectQuery<T>(entitySetName, base.Context, MergeOption.NoTracking)
                                      .FullTextSearch(fieldNames, searchKeys, exactMatch).OrderBy(orderByFieldName, true));
        }
    }


    public static class SearchRepositoryExtension
    {
        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> queryable, List<String> fieldNames, String searchKey, Boolean exactMatch)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "c");
            MethodInfo containsStringMethod = typeof(String).GetMethod("Contains", new Type[] { typeof(String) });

            var publicProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Expression orExpressions = null;

            IEnumerable<PropertyInfo> stringProperties = publicProperties.ToList().Where(condition => fieldNames.Contains(condition.Name) && condition.PropertyType.Equals(typeof(String)));
            IEnumerable<PropertyInfo> otherProperties = publicProperties.ToList().Where(condition => fieldNames.Contains(condition.Name) && !condition.PropertyType.Equals(typeof(String)));

            String[] searchKeyParts;
            if (exactMatch)
            {
                searchKeyParts = new[] { searchKey };
            }
            else
            {
                searchKeyParts = searchKey.Split(' ');
            }

            Boolean flag = false;

            // create expression for all values
            foreach (var property in stringProperties)
            {
                flag = true;
                Expression nameProperty = Expression.Property(parameter, property);
                foreach (var searchKeyPart in searchKeyParts)
                {
                    Expression searchKeyExpression = Expression.Constant(searchKeyPart);
                    Expression callContainsMethod = Expression.Call(nameProperty, containsStringMethod, searchKeyExpression);
                    if (orExpressions == null)
                    {
                        orExpressions = callContainsMethod;
                    }
                    else
                    {
                        orExpressions = Expression.Or(orExpressions, callContainsMethod);
                    }
                }
            }

            // create expression for non string values only.
            foreach (var property in otherProperties)
            {

                Expression nameProperty = Expression.Property(parameter, property);
                foreach (var searchKeyPart in searchKeyParts)
                {
                    dynamic propertyValue = GetValidPropertyValue(nameProperty.Type, searchKeyPart);
                    if (propertyValue != null)
                    {
                        flag = true;
                        Expression searchKeyExpression = Expression.Convert(Expression.Constant(propertyValue), nameProperty.Type);
                        Expression callEqualsMethod = Expression.Equal(nameProperty, searchKeyExpression);

                        if (orExpressions == null)
                        {
                            orExpressions = callEqualsMethod;
                        }
                        else
                        {
                            orExpressions = Expression.Or(orExpressions, callEqualsMethod);
                        }
                    }
                }
            }

            // Create final expression
            if (flag)
            {
                MethodCallExpression finalCallExpression = Expression.Call(typeof(Queryable), "Where",
                                                                           new Type[] { queryable.ElementType },
                                                                           queryable.Expression,
                                                                           Expression.Lambda<Func<T, Boolean>>(orExpressions, new ParameterExpression[] { parameter }));

                return (queryable.Provider.CreateQuery<T>(finalCallExpression));
            }
            return null;
        }

        /// <summary>  
        /// Searches in all String properties for the specified search key.  
        /// It is also able to search for several words. If the searchKey is for example 'John Travolta' then 
        /// with exactMatch set to false all records which contain either 'John' or 'Travolta' in some String property  
        /// are returned.  
        /// </summary>  
        public static IQueryable<T> AdvanceTextSearch<T>(this IQueryable<T> queryable, IDictionary<String, String> searchCriteria, Boolean ignoreCase = true)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "c");
            MethodInfo containsStringMethod = typeof(String).GetMethod("Contains", new Type[] { typeof(String) });
            MethodInfo equalsStringMethod = typeof(String).GetMethod("Equals", new Type[] { typeof(String) });
            MethodInfo dateMethod = typeof(System.Data.Entity.Core.Objects.EntityFunctions).GetMethod("TruncateTime", new Type[] { typeof(DateTime) });
            MethodInfo convertToLowerCaseMethod = typeof(string).GetMethod("ToLower", new Type[] { });
            var publicProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Expression andExpressions = null;

            IEnumerable<PropertyInfo> stringProperties = publicProperties.ToList().Where(condition => searchCriteria.ContainsKey(condition.Name) && condition.PropertyType.Equals(typeof(String)));
            IEnumerable<PropertyInfo> otherProperties = publicProperties.ToList().Where(condition => searchCriteria.ContainsKey(condition.Name) && !condition.PropertyType.Equals(typeof(String)));

            Boolean flag = false;
            //Boolean isRelatedPropertySet = false;

            // Create expression for string columns
            foreach (var property in stringProperties)
            {
                flag = true;
                //isRelatedPropertySet = false;
                Expression nameProperty = Expression.Property(parameter, property);
                List<Expression> relatedPropertyList = new List<Expression>();
                List<Expression> modifiedPropertyList = new List<Expression>();
                relatedPropertyList.Add(nameProperty);
                List<String> getRelatedPropertyValue;

                if (RelatedProperties.RelatedPropertyCollection.ContainsKey(property.Name))
                {
                    getRelatedPropertyValue = new List<String>();
                    getRelatedPropertyValue = RelatedProperties.RelatedPropertyCollection.GetValue(property.Name);
                    foreach (var value in getRelatedPropertyValue)
                    {
                        relatedPropertyList.Add(Expression.Property(parameter,value.ToString()));
                    }
                }                

                //String[] str = searchCriteria[property.Name].Split(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR);
                String[] str = searchCriteria[property.Name].IsNotNull() ? searchCriteria[property.Name].Split(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR) : new String[0];
                if (str.Length <= AppConsts.ONE)
                {
                    Expression searchKeyExpression = Expression.Constant(searchCriteria[property.Name]);
                    //added for converting searchKeyExpression and nameProperty to lower case
                    if (searchCriteria[property.Name].IsNotNull())
                    {
                        if (ignoreCase == false)
                        {
                            searchKeyExpression = Expression.Call(searchKeyExpression, convertToLowerCaseMethod);
                            modifiedPropertyList = new List<Expression>();
                            foreach (var expressionProperty in relatedPropertyList)
                            {
                                modifiedPropertyList.Add(Expression.Call(expressionProperty, convertToLowerCaseMethod));
                            }
                            relatedPropertyList = modifiedPropertyList;
                        }
                        Expression callContainsMethod = null;
                        MethodInfo comparisonMethod = null;
                        if (EqualSearchFields.EqualSearchCollection.Contains(property.Name))
                        {
                            comparisonMethod = equalsStringMethod;//Expression.Call(nameProperty, equalsStringMethod, searchKeyExpression);
                        }
                        else
                        {
                            comparisonMethod = containsStringMethod;
                        }


                        foreach (var exp in relatedPropertyList)
                        {
                            if (callContainsMethod.IsNull())
                            {
                                callContainsMethod = Expression.Call(exp, comparisonMethod, searchKeyExpression);
                            }
                            else
                            {
                                callContainsMethod = Expression.Or(callContainsMethod, Expression.Call(exp, comparisonMethod, searchKeyExpression));
                            }

                        }
                        //orExpressions = Expression.Or(orExpressions, Expression.Call(nameProperty, containsStringMethod, searchKeyExpression));
                        if (andExpressions.IsNull())
                        {
                            andExpressions = callContainsMethod;
                        }
                        else
                        {
                            andExpressions = Expression.And(andExpressions, callContainsMethod);
                        }
                    }
                }
                else
                {
                    Expression orExpressions = null;

                    foreach (String item in str)
                    {
                        if (item.Trim().Length > 0)
                        {
                            Expression searchKeyExpression = Expression.Constant(item.Trim());
                            //added for converting searchKeyExpression and nameProperty to lower case
                            if (ignoreCase == false)
                            {
                                searchKeyExpression = Expression.Call(searchKeyExpression, convertToLowerCaseMethod);
                                modifiedPropertyList = new List<Expression>();
                                foreach (var expressionProperty in relatedPropertyList)
                                {
                                    modifiedPropertyList.Add(Expression.Call(expressionProperty, convertToLowerCaseMethod));
                                }
                                relatedPropertyList = modifiedPropertyList;
                            }

                            foreach (var exp in relatedPropertyList)
                            {
                                if (orExpressions.IsNull())
                                {
                                    orExpressions = Expression.Call(exp, containsStringMethod, searchKeyExpression);
                                }
                                else
                                {
                                    orExpressions = Expression.Or(orExpressions, Expression.Call(exp, containsStringMethod, searchKeyExpression));
                                }
                            }
                        }
                    }

                    // Add all Or expression to And expression for final call
                    if (!orExpressions.IsNull())
                    {
                        if (andExpressions.IsNull())
                        {
                            andExpressions = orExpressions;
                        }
                        else
                        {
                            andExpressions = Expression.And(andExpressions, orExpressions);
                        }
                    }
                }
            }

            // create expression for non string values only.
            foreach (var property in otherProperties)
            {
                if (searchCriteria[property.Name].IsNotNull())
                {
                    Expression nameProperty = Expression.Property(parameter, property);
                    dynamic propertyValue = GetValidPropertyValue(nameProperty.Type, searchCriteria[property.Name]);
                    TypeCode typeCode = GetTypeCode(nameProperty.Type);
                    Boolean isListType = searchCriteria[property.Name].Contains(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString());

                    if (typeCode == TypeCode.DateTime && isListType == false)
                    {
                        nameProperty = Expression.Call(dateMethod, Expression.Convert(nameProperty, typeof(DateTime?)));
                    }

                    if (isListType == true && propertyValue.Count > 1)
                    {
                        flag = true;
                        Expression orExpressions = null;
                        if (typeCode == TypeCode.DateTime)
                        {
                            Expression dateFrom = Expression.Call(dateMethod, Expression.Convert(Expression.Constant(propertyValue[0]), typeof(DateTime?)));
                            Expression dateTo = Expression.Call(dateMethod, Expression.Convert(Expression.Constant(propertyValue[1]), typeof(DateTime?)));
                            orExpressions = DatesBetween(nameProperty, dateFrom, dateTo);
                        }
                        else
                        {
                            foreach (var value in propertyValue)
                            {
                                Expression searchKeyExpression = Expression.Convert(Expression.Constant(value), nameProperty.Type);
                                Expression callExpressionMethod = Expression.Equal(nameProperty, searchKeyExpression);

                                if (orExpressions == null)
                                {
                                    orExpressions = callExpressionMethod;
                                }
                                else
                                {
                                    orExpressions = Expression.Or(orExpressions, callExpressionMethod);
                                }
                            }
                        }
                        if (andExpressions == null)
                        {
                            andExpressions = orExpressions;
                        }
                        else
                        {
                            andExpressions = Expression.And(andExpressions, orExpressions);
                        }
                    }
                    else if (isListType == false && propertyValue != null && searchCriteria[property.Name] != AppConsts.ZERO)
                    {
                        Expression searchKeyExpression = Expression.Convert(Expression.Constant(propertyValue), nameProperty.Type);
                        Expression callExpressionMethod = Expression.Equal(nameProperty, searchKeyExpression);

                        if (andExpressions == null)
                        {
                            andExpressions = callExpressionMethod;
                        }
                        else
                        {
                            andExpressions = Expression.And(andExpressions, callExpressionMethod);
                        }
                    }
                }
            }


            if (flag && andExpressions.IsNotNull())
            {
                // Create final expression if criteria matched
                MethodCallExpression finalCallExpression = Expression.Call(typeof(Queryable), "Where",
                                                                           new Type[] { queryable.ElementType },
                                                                           queryable.Expression,
                                                                           Expression.Lambda<Func<T, Boolean>>(andExpressions, new ParameterExpression[] { parameter }));

                // Return only matched records.
                return queryable.Provider.CreateQuery<T>(finalCallExpression);
            }
            else
            {
                // Return all record while no criteria selected.
                if (searchCriteria.Count.Equals(AppConsts.NONE))
                {
                    return queryable;
                }

                // Return empty record set while invalid search criteria.
                return (new List<T>().AsQueryable());

                //Raise error when all search criteria are invalid. -- to be discuss
                //throw new SysXException("Invalid search criteria, please contact to system admin.");
            }
        }

        /// <summary>
        /// Apply Order by to any entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">Entity as IQueryable.</param>
        /// <param name="orderByFieldName">String field name which to arrange.</param>
        /// <param name="IsAscending">True, if record arrange in ascending order.</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, String orderByFieldName, Boolean IsAscending)
        {
            if (queryable == null)
            {
                return null;
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "c");
            var orderByProperty = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                            .FirstOrDefault(condition => condition.Name.Equals(orderByFieldName));

            // Create OrderBy Expression if property exist else no effect.
            if (orderByProperty != null)
            {
                Expression orderProperty = Expression.Property(parameter, orderByProperty);

                String OrderByMethod = "OrderBy";
                if (!IsAscending)
                {
                    OrderByMethod = "OrderByDescending";
                }
                MethodCallExpression finalCallExpression = Expression.Call(
                                                           typeof(Queryable), OrderByMethod,
                                                           new Type[] { queryable.ElementType, orderProperty.Type },
                                                           queryable.Expression,
                                                           Expression.Quote(Expression.Lambda(orderProperty, parameter)));
                return (queryable.Provider.CreateQuery<T>(finalCallExpression));
            }
            return queryable;
        }

        private static dynamic GetValidPropertyValue(Type type, String value)
        {
            TypeCode typeCode = GetTypeCode(type);
            Boolean isMultipleValues = value.IsNotNull() && (value.Contains(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR.ToString()) == true);

            // Convert to specific type if allow.
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    Boolean boolValue;
                    if (Boolean.TryParse(value, out boolValue))
                    {
                        return boolValue;
                    }
                    break;
                case TypeCode.DateTime:
                    DateTime dateValue;

                    if (isMultipleValues)
                    {
                        return Array.ConvertAll(value.Split(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR), DateTime.Parse).ToList<DateTime>();
                    }
                    if (DateTime.TryParse(value, out dateValue))
                    {
                        return dateValue;
                    }
                    break;
                case TypeCode.Decimal:
                    Decimal decValue;
                    if (Decimal.TryParse(value, out decValue))
                    {
                        return decValue;
                    }
                    break;
                case TypeCode.Double:
                    Double doubleValue;
                    if (Double.TryParse(value, out doubleValue))
                    {
                        return doubleValue;
                    }
                    break;
                case TypeCode.Int16:
                    Int16 int16Value;

                    if (isMultipleValues)
                    {
                        return Array.ConvertAll(value.Split(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR), Int16.Parse).ToList<Int16>();
                    }
                    if (Int16.TryParse(value, out int16Value))
                    {
                        return int16Value;
                    }
                    break;
                case TypeCode.Int32:
                    Int32 integerValue;

                    if (isMultipleValues)
                    {
                        return Array.ConvertAll(value.Split(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR), Int32.Parse).ToList<Int32>();
                    }
                    else if (Int32.TryParse(value, out integerValue))
                    {
                        return integerValue;
                    }
                    break;
                case TypeCode.Int64:
                    Int64 int64Value;

                    if (isMultipleValues)
                    {
                        return Array.ConvertAll(value.Split(SysXSearchConsts.SEARCH_MULTIVALUE_SEPERATOR), Int64.Parse).ToList<Int64>();
                    }
                    else if (Int64.TryParse(value, out int64Value))
                    {
                        return int64Value;
                    }
                    break;
            }
            return null;
        }

        private static TypeCode GetTypeCode(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);

            // Check Type is Nullable and base TypeCode.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeCode = Type.GetTypeCode(type.GetGenericArguments()[0]);
            }
            return typeCode;
        }

        private static Expression DatesBetween(Expression nameProperty, Expression dateFrom, Expression dateTo)
        {
            return Expression.And(Expression.GreaterThanOrEqual(nameProperty, dateFrom), Expression.LessThanOrEqual(nameProperty, dateTo));
        }
    }
}
