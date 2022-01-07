using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace MVCBlog.Web.Infrastructure.Excel;

public static class GenericExcelGenerator
{
    private const int MaximumColumnWidth = 255 * 256;

    public static Column<T> CreateColumn<T>(this IEnumerable<T> data, string header, Func<T, object> value)
    {
        return new Column<T>(header, value);
    }

    public static Column<T> CreateColumn<T>(this Func<IEnumerable<T>> getNextElements, string header, Func<T, object> value)
    {
        return new Column<T>(header, value);
    }

    public static Sheet<T> CreateSheet<T>(this IEnumerable<T> data, string name, IEnumerable<Column<T>> columns)
    {
        return new Sheet<T>(name, columns, data);
    }

    public static QueryableSheet<T> CreateSheet<T>(this Func<IEnumerable<T>> getNextElements, string name, IEnumerable<Column<T>> columns)
    {
        return new QueryableSheet<T>(name, columns, getNextElements);
    }

    public static byte[] GenerateDocument<T1, T2>(bool setColumnWidth, Sheet<T1> sheet1, Sheet<T2> sheet2)
    {
        var workbook = new XSSFWorkbook();

        AddSheet(workbook, setColumnWidth, sheet1);
        AddSheet(workbook, setColumnWidth, sheet2);

        using (var output = new MemoryStream())
        {
            workbook.Write(output);
            return output.ToArray();
        }
    }

    public static byte[] GenerateDocument<T>(bool setColumnWidth, params Sheet<T>[] sheets)
    {
        var workbook = new XSSFWorkbook();

        foreach (var sheetElement in sheets)
        {
            AddSheet(workbook, setColumnWidth, sheetElement);
        }

        using (var output = new MemoryStream())
        {
            workbook.Write(output);
            return output.ToArray();
        }
    }

    private static void AddSheet<T>(XSSFWorkbook workbook, bool setColumnWidth, Sheet<T> sheetElement)
    {
        var sheet = workbook.CreateSheet(WorkbookUtil.CreateSafeSheetName(sheetElement.Name));

        XSSFFont font = (XSSFFont)workbook.CreateFont();
        font.Boldweight = (short)FontBoldWeight.Bold;
        var boldStyle = workbook.CreateCellStyle();
        boldStyle.SetFont(font);

        var numericStyle = workbook.CreateCellStyle();
        numericStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0");

        int rowIndex = 0;
        int columnIndex = 0;
        var row = sheet.CreateRow(rowIndex++);

        foreach (var column in sheetElement.Columns)
        {
            var cell = row.CreateCell(columnIndex++);
            cell.SetCellValue(column.Header);
            cell.CellStyle = boldStyle;
        }

        foreach (var item in sheetElement.Data)
        {
            columnIndex = 0;
            row = sheet.CreateRow(rowIndex++);

            foreach (var column in sheetElement.Columns)
            {
                var cell = row.CreateCell(columnIndex++);

                object value = column.Value(item);

                if (value == null)
                {
                    continue;
                }

                if (value is decimal)
                {
                    cell.SetCellValue((double)(decimal)value);
                    cell.CellStyle = numericStyle;
                }
                else if (value is int)
                {
                    cell.SetCellValue((double)(int)value);
                    cell.CellStyle = numericStyle;
                }
                else if (value is double)
                {
                    cell.SetCellValue((double)value);
                    cell.CellStyle = numericStyle;
                }
                else if (value is long)
                {
                    cell.SetCellValue((double)(long)value);
                    cell.CellStyle = numericStyle;
                }
                else
                {
                    cell.SetCellValue(value.ToString());
                }
            }
        }

        if (setColumnWidth)
        {
            for (int i = 0; i < sheetElement.Columns.Count(); i++)
            {
                if (sheet.LastRowNum < 1000)
                {
                    sheet.AutoSizeColumn(i); // Use only for sheets with few rows!
                    sheet.SetColumnWidth(i, Math.Min(MaximumColumnWidth, (int)(1.2 * sheet.GetColumnWidth(i))));
                }
                else
                {
                    sheet.SetColumnWidth(i, 6000);
                }
            }
        }
    }
}