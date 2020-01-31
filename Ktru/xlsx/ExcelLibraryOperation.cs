using System.Collections.Generic;
using ExcelLibrary.SpreadSheet;
using Ktru.infrastructure;
using Ktru.model;

namespace Ktru.xlsx
{
    class ExcelLibraryOperation : IXlsxOperation
    {
        public void SaveKtruFile(string filePath, IEnumerable<KtruItem> ktrus)
        {
            var wb = new Workbook();
            Worksheet ws = null;
            int i = 0;
            int wsCount = 1;
            foreach (KtruItem k in ktrus)
            {
                if (i == 0)
                {
                    ws = new Worksheet("КТРУ" + wsCount);
                    wb.Worksheets.Add(ws);
                    ws.Cells[i, 0] = new Cell("Код");
                    ws.Cells[i, 1] = new Cell("Наименование");
                    ws.Cells[i, 2] = new Cell("Единицы измерения");
                    ws.Cells[i, 3] = new Cell("Дата применения");
                    ws.Cells[i, 4] = new Cell("Версия");
                    ws.Cells[i, 5] = new Cell("Статус");
                    wsCount++;
                }
                else
                {
                    ws.Cells[i, 0] = new Cell(k.Code);
                    ws.Cells[i, 1] = new Cell(k.Name);
                    ws.Cells[i, 2] = new Cell(string.Join(", ", k.Units));
                    ws.Cells[i, 3] = new Cell(k.StartDate.ToString("dd.MM.yyyy"));
                    ws.Cells[i, 4] = new Cell(k.Version);
                    ws.Cells[i, 5] = new Cell(k.Actual ? "Включено в КТРУ" : "Недействительно");
                }
                i++;
                if (i == 65000)
                {
                    i = 0;
                }
            }
            wb.Save(filePath);
        }
    }
}
