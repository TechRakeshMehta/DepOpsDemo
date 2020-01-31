using INTSOF.Utils;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Entity
{
    public class CustomPagingArgs
    {

        /// For Grid Custom paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyEntity"></param>
        /// <param name="customPagingExpression"></param>
        /// <param name="defaultSortField"></param>
        /// <returns>IQueryable</returns>
        public IQueryable<T> ApplyFilterOrSort<T>(IQueryable<T> anyEntity, CustomPagingArgsContract customPagingExpression)
        {
            //if no sort expression set then apply sorting on default field
            if (customPagingExpression.SortExpression.IsNullOrEmpty())
            {
                customPagingExpression.SortExpression = customPagingExpression.DefaultSortExpression;
            }

            //Apply filter on query
            if (!customPagingExpression.FilterColumns.IsNull())
            {
                if (customPagingExpression.FilterColumns.Count > 0)
                {
                    //CustomPagingArgs customPagingArg = new CustomPagingArgs();
                    anyEntity = anyEntity.Where(ApplyFiltering(customPagingExpression), customPagingExpression.FilterValues.ToArray());
                }
            }

            //apply sorting
            if (customPagingExpression.SortDirectionDescending)
            {
                anyEntity = anyEntity.OrderBy(customPagingExpression.SortExpression + " desc" + customPagingExpression.SecondarySortExpression);
            }
            else
            {
                anyEntity = anyEntity.OrderBy(customPagingExpression.SortExpression + customPagingExpression.SecondarySortExpression);
            }

            //apply sorting
            //anyEntity = anyEntity.OrderBy(customPagingExpression.SortExpression, !customPagingExpression.SortDirectionDescending);
            if (customPagingExpression.PageSize == AppConsts.NONE)
            {
                return anyEntity;
            }
            Int32 totalRecords = anyEntity.Count();
            customPagingExpression.VirtualPageCount = totalRecords;

            //If filtering is resulting in lesser records than the requested page number then change the page number and make it last page.
            if (customPagingExpression.CurrentPageIndex * customPagingExpression.PageSize > totalRecords)
            {
                customPagingExpression.CurrentPageIndex = (totalRecords / customPagingExpression.PageSize) + 1;
            }

            var rows = (((customPagingExpression.CurrentPageIndex - 1) * customPagingExpression.PageSize) < 0) ? 0 : (customPagingExpression.CurrentPageIndex - 1) * customPagingExpression.PageSize;
            return anyEntity.Skip(rows).Take(customPagingExpression.PageSize);
        }

        #region methods

        public String ApplyFiltering(CustomPagingArgsContract customPagingExpression)
        {
            String filterQuery = String.Empty;
            for (Int32 i = 0; i < customPagingExpression.FilterColumns.Count; i++)
            {

                if (customPagingExpression.FilterValues.Count > AppConsts.NONE && String.IsNullOrWhiteSpace(Convert.ToString(customPagingExpression.FilterValues[i])))
                    customPagingExpression.FilterOperators[i] = "NULL";
                if (i > 0)
                {
                    filterQuery += " And ";
                }

                switch (Convert.ToString(customPagingExpression.FilterOperators[i]))
                {
                    case "EqualTo":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower() == @" + i;
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + " == @" + i;
                        break;

                    case "NotEqualTo":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower() != @" + i;
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + " != @" + i;
                        break;

                    case "Contains":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower().Contains(@" + i + ")";
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + ".Contains(@" + i + ")";
                        break;

                    case "DoesNotContain":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += "!" + customPagingExpression.FilterColumns[i] + ".ToLower().Contains(@" + i + ")";
                        else
                            filterQuery += "!" + customPagingExpression.FilterColumns[i] + ".Contains(@" + i + ")";
                        break;

                    case "StartsWith":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower().StartsWith(@" + i + ")";
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + ".StartsWith(@" + i + ")";
                        break;

                    case "EndsWith":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower().EndsWith(@" + i + ")";
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + ".EndsWith(@" + i + ")";
                        break;

                    case "LessThan":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower() < @" + i;
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + " < @" + i;
                        break;

                    case "GreaterThan":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower() > @" + i;
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + " > @" + i;
                        break;

                    case "LessThanOrEqualTo":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower() <= @" + i;
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + " <= @" + i;
                        break;

                    case "GreaterThanOrEqualTo":
                        if (customPagingExpression.FilterTypes[i].Equals("System.String"))
                            filterQuery += customPagingExpression.FilterColumns[i] + ".ToLower() >= @" + i;
                        else
                            filterQuery += customPagingExpression.FilterColumns[i] + " >= @" + i;
                        break;

                    case "IsNull":
                    case "NULL":
                    case "IsEmpty":
                        filterQuery += " ( " + customPagingExpression.FilterColumns[i] + ".Equals(NULL) or " + customPagingExpression.FilterColumns[i] + ".Equals(\"\")) ";
                        break;

                    case "NullOtherThanString":
                        filterQuery += customPagingExpression.FilterColumns[i] + "== NULL";
                        break;

                    default:
                        break;
                }
            }
            return filterQuery;
        }

        #endregion

    }

}