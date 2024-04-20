﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace M03.UF5.AC3
{
    public class ConsumAigua
    {
        [Index(0)]
        public int Anys { get; set; }
        [Index(1)]
        public int Codi_comarca { get; set; }
        [Index(2)]
        public string? Comarca { get; set; }
        [Index(3)]
        public int Poblacio { get; set; }
        [Index(4)]
        public double Domestic_xarxa { get; set; }
        [Index(5)]
        public double Activitats_economiques_i_fonts_propies { get; set; }
        [Index(6)]
        public double Total { get; set; }
        [Index(7)]
        public double Consum_domestic_per_capital { get; set; }
    }
}
