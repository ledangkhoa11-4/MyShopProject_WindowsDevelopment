using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MyShopProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public async void getProductFromExcelFile(string filename)
        {
            try
            {
                var document = SpreadsheetDocument.Open(filename, false);
                var wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.FirstOrDefault(s => s.Name == "Sheet1");
                var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                var rows = wsPart.Worksheet.Descendants<Row>();
                int numRows = rows.Count();

                for (int i = 2; i <= numRows; i++)
                {
                    var book = new Book();
                    book.Name = GetCellValue(filename, "Sheet1", $"A{i}");
                    book.ImageBase64 = GetCellValue(filename, "Sheet1", $"B{i}");
                    book.PurchasePrice = Convert.ToInt32(GetCellValue(filename, "Sheet1", $"C{i}"));
                    book.SellingPrice = Convert.ToInt32(GetCellValue(filename, "Sheet1", $"D{i}"));
                    book.Author = GetCellValue(filename, "Sheet1", $"E{i}");
                    book.PublishedYear = Convert.ToInt32(GetCellValue(filename, "Sheet1", $"F{i}"));
                    book.QuantityStock = Convert.ToInt32(GetCellValue(filename, "Sheet1", $"G{i}"));
                    book.QuantityOrder = Convert.ToInt32(GetCellValue(filename, "Sheet1", $"H{i}"));
                    book.CatID = GetCellValue(filename, "Sheet1", $"I{i}");
                    book.Description = GetCellValue(filename, "Sheet1", $"J{i}");
                    book.IsOnStock = Convert.ToBoolean(GetCellValue(filename, "Sheet1", $"K{i}"));
                    await product_BUS.AddProduct(book);
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }
        public async void getCategoryFromExcelFile(string filename)
        {
            try
            {
                var document = SpreadsheetDocument.Open(filename, false);
                var wbPart = document.WorkbookPart;
                var sheets = wbPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.FirstOrDefault(s => s.Name == "Sheet2");
                var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                var rows = wsPart.Worksheet.Descendants<Row>();
                int numRows = rows.Count();

                for (int i = 2; i <= numRows; i++)
                {
                    var cat = new Category();
                    cat.Name = GetCellValue(filename, "Sheet2", $"A{i}");
                    cat.Description = GetCellValue(filename, "Sheet2", $"B{i}");

                    await category_BUS.addCategory(cat);
                    MessageBox.Show($"{cat.Name} {cat.Description}");
                }

            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
