using System;
using System.Collections.Generic;
using System.IO;
using Ktru.infrastructure;
using Ktru.model;
using OfficeOpenXml;

namespace Ktru.xlsx
{
    class EPPlusOperation : IXlsxOperation
    {
        public void SaveKtruFile(string filePath, IEnumerable<KtruItem> ktrus)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "Ktru.exe";
                excelPackage.Workbook.Properties.Title = "КТРУ";
                excelPackage.Workbook.Properties.Subject = "Каталог товаров, работ, услуг для обеспечения государственных и муниципальных нужд";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("КТРУ");

                //Add some text to cell A1
                worksheet.Cells["A1"].Value = "Код";
                worksheet.Cells["B1"].Value = "Наименование";
                worksheet.Cells["C1"].Value = "Единицы измерения";
                worksheet.Cells["D1"].Value = "Дата применения";
                worksheet.Cells["E1"].Value = "Версия";
                worksheet.Cells["F1"].Value = "Статус";

                //You could also use [line, column] notation:
                int i = 2;
                foreach (KtruItem k in ktrus)
                {
                    worksheet.Cells[i, 1].Value = k.Code;
                    worksheet.Cells[i, 2].Value = k.Name;
                    worksheet.Cells[i, 3].Value = string.Join(", ", k.Units);
                    worksheet.Cells[i, 4].Value = k.StartDate.ToString("dd.MM.yyyy");
                    worksheet.Cells[i, 5].Value = k.Version;
                    worksheet.Cells[i, 6].Value = k.Actual ? "Включено в КТРУ" : "Недействительно";
                    i++;
                }

                //Save your file
                FileInfo fi = new FileInfo(filePath);
                excelPackage.SaveAs(fi);
            }
        }
    }
}
