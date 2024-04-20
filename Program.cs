using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using dao.DTOs;
using dao.Persistence.DAO;

namespace M03.UF5.AC3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            List<ConsumAigua> consumAiguas = XMLHelper.ReadCsv();
            if (consumAiguas != null && consumAiguas.Any())
            {
                XMLHelper.CreateXMLFileWithLINQ(consumAiguas);
            }
            else
            {
                Console.WriteLine("No data found in CSV file.");
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
