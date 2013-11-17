using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Halo.Utilities
{
    public class ExportManager
    {
        // <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="gv"></param>
        public static void ExportGridviewToExcel(string fileName, GridView gridview)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a table to contain the grid
                    Table table = new Table();

                    //  include the gridline settings
                    table.GridLines = gridview.GridLines;

                    //  add the header row to the table
                    if (gridview.HeaderRow != null)
                    {
                        ExportManager.PrepareControlForExport(gridview.HeaderRow);
                        table.Rows.Add(gridview.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gridview.Rows)
                    {
                        ExportManager.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gridview.FooterRow != null)
                    {
                        ExportManager.PrepareControlForExport(gridview.FooterRow);
                        table.Rows.Add(gridview.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];

                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    ExportManager.PrepareControlForExport(current);
                }
            }
        }

        /// <summary>
        /// Exports the data table to excel.
        /// </summary>
        /// <param name="table">The table.</param>
        public static void ExportDataTableToExcel(string fileName, DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            string tab = string.Empty;

            foreach (DataColumn dataColumn in table.Columns)
            {
                HttpContext.Current.Response.Write(tab + dataColumn.ColumnName);
                tab = "\t";
            }

            HttpContext.Current.Response.Write("\n");

            foreach (DataRow dataRow in table.Rows)
            {
                tab = string.Empty;

                // gates 10/1/2013 fixed bug that was outputting header instead of row info. 
                foreach (object item in dataRow.ItemArray)
                {
                    HttpContext.Current.Response.Write(tab + item.ToString());
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }

            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Exports the data table to CSV.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="table">The table.</param>
        public static void ExportDataTableToCSV(string fileName, DataTable table)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in table.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(fileName + ".csv", sb.ToString());
        }
    }  /// End of Class
}
