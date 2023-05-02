using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Category = MyShopProject.DTO.Category;

namespace MyShopProject.BUS
{
    public class ImportData_BUS
    {
        private Product_BUS product_BUS;
        private Category_BUS category_BUS;
        public ImportData_BUS() { 
            product_BUS= new Product_BUS();
            category_BUS= new Category_BUS();
        }
        public string GetCellValue(string fileName, string sheetName, string addressName)
        {
            try
            {
                string value = null;
                using (SpreadsheetDocument document =
                    SpreadsheetDocument.Open(fileName, false))
                {
                    WorkbookPart wbPart = document.WorkbookPart;
                    Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                      Where(s => s.Name == sheetName).FirstOrDefault();

                    if (theSheet == null)
                    {
                        throw new ArgumentException("sheetName");
                    }
                    WorksheetPart wsPart =
                        (WorksheetPart)(wbPart.GetPartById(theSheet.Id));
                    Cell theCell = wsPart.Worksheet.Descendants<Cell>().
                      Where(c => c.CellReference == addressName).FirstOrDefault();

                    if (theCell.InnerText.Length > 0)
                    {
                        value = theCell.InnerText;
                        if (theCell.DataType != null)
                        {
                            switch (theCell.DataType.Value)
                            {
                                case CellValues.SharedString:
                                    var stringTable =
                                        wbPart.GetPartsOfType<SharedStringTablePart>()
                                        .FirstOrDefault();
                                    if (stringTable != null)
                                    {
                                        value =
                                            stringTable.SharedStringTable
                                            .ElementAt(int.Parse(value)).InnerText;
                                    }
                                    break;

                                case CellValues.Boolean:
                                    switch (value)
                                    {
                                        case "0":
                                            value = "FALSE";
                                            break;
                                        default:
                                            value = "TRUE";
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
        public async Task<bool> getProductFromExcelFile(string filename, string sheetName)
        {
            try
            {
                var document = SpreadsheetDocument.Open(filename, false);
                var wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.FirstOrDefault(s => s.Name == sheetName);
                var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                var rows = wsPart.Worksheet.Descendants<Row>();
                int numRows = rows.Count();
                foreach (var row in rows)
                {
                    int cellcount = row.Descendants<Cell>().Count();
                    if (cellcount != 11)
                    {
                        MessageBox.Show("File's format is not correct please check the file again!!");
                        return false;
                    }
                }
                for (int i = 2; i <= numRows; i++)
                {
                    var book = new Book();
                    book.Name = GetCellValue(filename, sheetName, $"A{i}");
                    book.ImageBase64 = GetCellValue(filename, sheetName, $"B{i}");
                    book.PurchasePrice = Convert.ToInt32(GetCellValue(filename, sheetName, $"C{i}"));
                    book.SellingPrice = Convert.ToInt32(GetCellValue(filename, sheetName, $"D{i}"));
                    book.Author = GetCellValue(filename, sheetName, $"E{i}");
                    book.PublishedYear = Convert.ToInt32(GetCellValue(filename, sheetName, $"F{i}"));
                    book.QuantityStock = Convert.ToInt32(GetCellValue(filename, sheetName, $"G{i}"));
                    book.QuantityOrder = Convert.ToInt32(GetCellValue(filename, sheetName, $"H{i}"));
                    book.CatID = GetCellValue(filename, sheetName, $"I{i}");
                    book.Description = GetCellValue(filename, sheetName, $"J{i}");
                    book.IsOnStock = Convert.ToBoolean(GetCellValue(filename, sheetName, $"K{i}"));
                    await product_BUS.AddProduct(book);
                }
                
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        public async Task<ObservableCollection<Category>> GetCategoryFromExcelFile(string filename,string sheetName)
        {
            try
            {
                var document = SpreadsheetDocument.Open(filename, false);
                var wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.FirstOrDefault(s => s.Name == sheetName);
                var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                var rows = wsPart.Worksheet.Descendants<Row>();
                int numRows = rows.Count();
                
                foreach( var row in rows )
                {
                    int cellcount = row.Descendants<Cell>().Count();
                    Debug.WriteLine($"Row {row.RowIndex} has {cellcount} cells.");
                    if (cellcount != 2)
                    {
                        MessageBox.Show("File's format is not correct please check the file again!!");
                        return null;
                        
                    }
                }
                for (int i = 2; i <= numRows; i++)
                {
                    var cat = new Category();
                    cat.Name = GetCellValue(filename, sheetName, $"A{i}");
                    cat.Description = GetCellValue(filename, sheetName, $"B{i}");

                    var result = await category_BUS.addCategory(cat);
                    
                    
                }
                var allcat = await category_BUS.getAllCategory();
                
                return allcat;

            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
