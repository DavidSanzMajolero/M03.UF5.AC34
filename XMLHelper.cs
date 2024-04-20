using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;

namespace M03.UF5.AC3
{
    public class XMLHelper
    {
        public static void CreateXMLFileWithLINQ(List<ConsumAigua> list)
        {
            XDocument xmlDoc = new XDocument(
                new XElement("data",
                    from item in list
                    select new XElement("row",
                        new XElement("Any", item.Anys),
                        new XElement("Codi_comarca", item.Codi_comarca),
                        new XElement("Comarca", item.Comarca),
                        new XElement("Població", item.Poblacio),
                        new XElement("Domèstic_xarxa", item.Domestic_xarxa),
                        new XElement("Activitats_econòmiques_i_fonts_pròpies", item.Activitats_economiques_i_fonts_propies),
                        new XElement("Total", item.Total),
                        new XElement("Consum_domèstic_per_càpita", item.Consum_domestic_per_capital)
                    )
                )
            );

            string xmlFilePath = "AguasXML.xml";
            xmlDoc.Save(xmlFilePath);
            Console.WriteLine($"XML file saved: {xmlFilePath}");
        }
        public static DataTable LeerCSV(string rutaArchivo)
        {
            DataTable dataTable = new DataTable();
            string[] encabezados;
            using (TextFieldParser parser = new TextFieldParser(rutaArchivo))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                if (!parser.EndOfData)
                {
                    encabezados = parser.ReadFields();
                }
                else
                {
                    return null; // Si el archivo está vacío, retorna null
                }
            }

            foreach (string encabezado in encabezados)
            {
                dataTable.Columns.Add(encabezado);
            }

            using (TextFieldParser parser = new TextFieldParser(rutaArchivo))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] campos = parser.ReadFields();

                    dataTable.Rows.Add(campos);
                }
            }

            return dataTable;
        }

        public static List<ConsumAigua> ReadCsv()
        {
            List<ConsumAigua> consumAiguas = new List<ConsumAigua>();
            using (var reader = new StreamReader(@"..\..\..\Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                consumAiguas = csv.GetRecords<ConsumAigua>().ToList();
            }
            return consumAiguas;
        }
    }
}
