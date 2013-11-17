using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Halo.Utilities
{
    public class UI
    {
        public static string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }

        public static void GridViewPageIndexChanging(GridView gridView, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex; List<string> test;

        }

        public static void GridViewSorting(GridView gridView, GridViewSortEventArgs e)
        {
            DataTable dataTable = gridView.DataSource as DataTable;

            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + UI.ConvertSortDirectionToSql(e.SortDirection);
            }
        }
    }   /// End of Class
}
