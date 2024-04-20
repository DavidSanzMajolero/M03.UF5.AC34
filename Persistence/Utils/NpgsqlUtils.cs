using Microsoft.Extensions.Configuration;
using Npgsql;
using dao.DTOs;
using System.Diagnostics.Metrics;

namespace dao.Persistence.Utils
{
    public class NpgsqlUtils
    {
        public static string OpenConnection()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\david\Downloads\AC4\M03.UF5.AC3-master\M03.UF5.AC3-master\appsettings.json", optional: false, reloadOnChange: true) //ESTA RUTA ES ABSOLUTA, CAMBIALA POR LA TUYA PARA QUE FUNCIONE!!!!
                .Build();
            return config.GetConnectionString("MyPostgresConn");
        }

        public static ConsumAiguaDTO GetConsumAigua(NpgsqlDataReader reader)
        {
            ConsumAiguaDTO c = new ConsumAiguaDTO
            {
                Anys = reader.GetInt32(0),
                Codi_comarca = reader.GetInt32(1),
                Comarca = reader.IsDBNull(2) ? null : reader.GetString(2),
                Poblacio = reader.GetInt32(3),
                Domestic_xarxa = reader.GetDouble(4),
                Activitats_economiques_i_fonts_propies = reader.GetDouble(5),
                Total = reader.GetDouble(6),
                Consum_domestic_per_capital = reader.GetDouble(7)
            };
            return c;
        }
    }
}