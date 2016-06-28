using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using NPOI.SS.Formula.Eval;
using System.Web.UI.WebControls;

namespace BLL
{
    /// <summary>
    /// 使用NPOI组件
    /// 需引入ICSharpCode.SharpZipLib.dll/NPOI.dll/NPOI.OOXML.dll/NPOI.OpenXml4Net.dll/NPOI.OpenXmlFormats.dll
    /// office2007
    /// </summary>
    public class NPOIExcelHelper
    {
        /// <summary>
        /// 将Excel文件中的数据读出到DataTable中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable Excel2DataTable(string file)
        {
            DataTable dt = new DataTable();
            IWorkbook workbook = null;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                //office2003 HSSFWorkbook
                workbook = new HSSFWorkbook(fs);
                // XSSFWorkbook 
            }
            ISheet sheet = workbook.GetSheetAt(0);
            dt = Export2DataTable(sheet, 0, true);
            return dt;

        }
        /// <summary>
        /// 将指定sheet中的数据导入到datatable中
        /// </summary>
        /// <param name="sheet">指定需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在的行号，-1没有列头</param>
        /// <param name="needHeader"></param>
        /// <returns></returns>
        private static DataTable Export2DataTable(ISheet sheet, int HeaderRowIndex, bool needHeader)
        {
            DataTable dt = new DataTable();
            XSSFRow headerRow = null;
            int cellCount;
            try
            {
                if (HeaderRowIndex < 0 || !needHeader)
                {
                    headerRow = sheet.GetRow(0) as XSSFRow;
                    cellCount = headerRow.LastCellNum;
                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        dt.Columns.Add(column);
                    }
                }
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex) as XSSFRow;
                    cellCount = headerRow.LastCellNum;
                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        ICell cell = headerRow.GetCell(i);
                        if (cell == null)
                        {
                            break;//到最后 跳出循环
                        }
                        else
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                            dt.Columns.Add(column);
                        }

                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = HeaderRowIndex + 1; i <= sheet.LastRowNum; i++)
                {
                    XSSFRow row = null;
                    if (sheet.GetRow(i) == null)
                    {
                        row = sheet.CreateRow(i) as XSSFRow;
                    }
                    else
                    {
                        row = sheet.GetRow(i) as XSSFRow;
                    }
                    DataRow dtRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j <= cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            switch (row.GetCell(j).CellType)
                            {
                                case CellType.Boolean:
                                    dtRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                    break;
                                case CellType.Error:
                                    dtRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (row.GetCell(j).CachedFormulaResultType)
                                    {

                                        case CellType.Boolean:
                                            dtRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);

                                            break;
                                        case CellType.Error:
                                            dtRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);

                                            break;
                                        case CellType.Numeric:
                                            dtRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);

                                            break;
                                        case CellType.String:
                                            string strFORMULA = row.GetCell(j).StringCellValue;
                                            if (strFORMULA != null && strFORMULA.Length > 0)
                                            {
                                                dtRow[j] = strFORMULA.ToString();
                                            }
                                            else
                                            {
                                                dtRow[j] = null;
                                            }
                                            break;
                                        default:
                                            dtRow[j] = "";
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                    {
                                        dtRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                    }
                                    else
                                    {
                                        dtRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                    }
                                    break;
                                case CellType.String:
                                    string str = row.GetCell(j).StringCellValue;
                                    if (!string.IsNullOrEmpty(str))
                                    {

                                        dtRow[j] = Convert.ToString(str);


                                    }
                                    else
                                    {
                                        dtRow[j] = null;
                                    }
                                    break;
                                default:
                                    dtRow[j] = "";
                                    break;
                            }

                        }
                    }
                    dt.Rows.Add(dtRow);
                }

            }
            catch (Exception)
            {

                return null;
            }
            return dt;
        }
        /// <summary>
        /// 将DataTable中的数据导入Excel文件中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public static void DataTable2Excel(DataTable dt, string file, string sheetName)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow header = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }
            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            byte[] buffer = stream.ToArray();
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }
        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueType(XSSFCell cell)
        {
            if (cell == null)
            {
                return null;
            }
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;

                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                default:
                    return "=" + cell.StringCellValue;
            }
        }


        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="fullPath">文件全路径</param>
        /// <param name="extension">扩展名</param>
        /// <returns></returns>
        public static DataSet GetDataFromExcel(string fullPath, string extension)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            { dt = getDataTableFromExcel2003(fullPath); }
            catch (Exception ex)
            {
                dt = getDataTableFromExcel2007(fullPath);
            }
            //if (extension.Equals(".xls"))
            //{
            //    dt = getDataTableFromExcel2003(fullPath);
            //}
            //else if (extension.Equals(".xlsx"))
            //{
            //    dt = getDataTableFromExcel2007(fullPath);
            //}

            ds.Tables.Add(dt);
            return ds;
        }

        public static DataTable getDataTableFromExcel2007(string fullpath)
        {
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            XSSFWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0);
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum + 1;
            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum + 1; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                table.Rows.Add(dataRow);
            }
            stream.Close();
            File.Delete(fullpath);
            return table;
        }
        public static DataTable getDataTableFromExcel2003(string fullpath)
        {
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);

            HSSFWorkbook workbook = new HSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheetAt(0);
            DataTable table = new DataTable();
            //获取sheet的首行
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            try
            {
                int rowCount = sheet.LastRowNum + 1;
                for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum + 1; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    table.Rows.Add(dataRow);
                }
                //stream.Close();
                //File.Delete(fullpath);


            }
            catch (Exception ex)
            { }
            finally
            {
                stream.Dispose();
                stream.Close();
                File.Delete(fullpath);
            }

            return table;
        }
        //public static DataTable ReadExcel(FileUpload fuUpload)
        //{
        //    if (fuUpload.HasFile)
        //    {

        //        //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
        //        XSSFWorkbook workbook = new XSSFWorkbook(fuUpload.FileContent);

        //        //获取excel的第一个sheet
        //        ISheet sheet = workbook.GetSheetAt(0);

        //        DataTable table = new DataTable();
        //        //获取sheet的首行
        //        IRow headerRow = sheet.GetRow(0);

        //        //一行最后一个方格的编号 即总的列数
        //        int cellCount = headerRow.LastCellNum;

        //        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
        //        {
        //            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
        //            table.Columns.Add(column);
        //        }
        //        //最后一列的标号  即总的行数
        //        int rowCount = sheet.LastRowNum;

        //        for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
        //        {
        //            IRow row = sheet.GetRow(i);
        //            DataRow dataRow = table.NewRow();

        //            for (int j = row.FirstCellNum; j < cellCount; j++)
        //            {
        //                if (row.GetCell(j) != null)
        //                    dataRow[j] = row.GetCell(j).ToString();
        //            }

        //            table.Rows.Add(dataRow);
        //        }

        //        workbook = null;
        //        sheet = null;
        //        return table;
        //    }
        //    return null;
        //}
    }
}

