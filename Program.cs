using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Drawing.Charts;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace AposeWordDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //InsertChart();
            //FileBookMark();
            NPOIReadTables("template/testdata.docx");
            //AposeReadTables();
            Console.Read();
        }

        static void AposeReadTables()
        {
            //https://blog.csdn.net/knqiufan/article/details/77847370
            //打开word文档，fileName是路径地址，需要扩展名
            var doc = new Aspose.Words.Document("template/testdata2.docx");
            var nodeCollection = doc.GetChildNodes(NodeType.Table, true);
            for (int t = 0; t < nodeCollection.Count; t++)
            {
                var table = nodeCollection[t] as Aspose.Words.Tables.Table;
                ReadOneTable(table);
                Console.WriteLine("----------------------------");
            }
        }

        static void NPOIReadTables(string filePath)
        {
            XWPFDocument document = null;
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    document = new XWPFDocument(file);
                    foreach (var table in document.Tables)
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var rowCells = table.Rows[i].GetTableCells();
                            for (int j = 0; j < rowCells.Count; j++)
                            {
                                Console.WriteLine(rowCells[j].GetText());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("文件{0}打开失败，错误：{1}", new string[] { filePath, e.ToString() }));
            }
        }

        static void ReadOneTable(Aspose.Words.Tables.Table table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                {
                    Console.WriteLine(table.Rows[i].Cells[j].GetText());
                }
            }
        }

        static void FileBookMark()
        {
            Aspose.Words.Document doc = new Aspose.Words.Document("template/测试合同.docx");
            var dic = new Dictionary<string, string>();
            dic.Add("合同编号", "TY2018036");
            dic.Add("项目名称", "宝安区数字化城管系统和三个一系统相关功能拓展升级项目");
            dic.Add("委托方", "深圳市宝安区城市管理局");
            dic.Add("受托方", "深圳市图元科技有限公司");
            dic.Add("签订时间", DateTime.Now.ToShortDateString());
            dic.Add("签订地点", "深圳市");
            dic.Add("有效期限", "一年");

            DocumentBuilder builder = new DocumentBuilder(doc);
            //书签替换
            foreach (var key in dic.Keys)
            {
                builder.MoveToBookmark(key); //移动到标签位置
                builder.Write(dic[key]);
            }

            //在对应书签位置插入word文档
            //Document srcDoc = new Document("TestInsertChartColumn.docx");

            builder.MoveToBookmark("合同正文");

            //builder.InsertDocument(srcDoc, ImportFormatMode.KeepDifferentStyles);

            doc.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "合同正文.doc"));

            //转换为pdf文档
            doc.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "合同正文.pdf"), Aspose.Words.SaveFormat.Pdf);

        }
        /// <summary>
        /// 插入图表
        /// </summary>
        static void InsertChart()
        {
            Aspose.Words.Document doc = new Aspose.Words.Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            // Add chart with default data. You can specify different chart types and sizes.
            Shape shape = builder.InsertChart(ChartType.Line, 432, 252);

            // Chart property of Shape contains all chart related options.
            Chart chart = shape.Chart;

            // Get chart series collection.
            ChartSeriesCollection seriesColl = chart.Series;

            // Delete default generated series.
            seriesColl.Clear();

            // Create category names array, in this example we have two categories.
            string[] categories = new string[] { "AW Category 1", "AW Category 1" };

            // Adding new series. Please note, data arrays must not be empty and arrays must be the same size.
            seriesColl.Add("AW Series 1", categories, new double[] { 1, 2 });
            seriesColl.Add("AW Series 2", categories, new double[] { 3, 4 });
            seriesColl.Add("AW Series 3", categories, new double[] { 5, 6 });
            seriesColl.Add("AW Series 4", categories, new double[] { 7, 8 });
            seriesColl.Add("AW Series 5", categories, new double[] { 9, 10 });

            doc.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestInsertChartColumn.docx"));
        }
    }
}
