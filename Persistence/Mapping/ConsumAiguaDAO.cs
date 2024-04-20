using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dao.Persistence.DAO;
using dao.DTOs;
using Npgsql;
using Npgsql.Util;
using dao.Persistence.Utils;
using M03.UF5.AC3;

namespace dao.Persistence.Mapping
{
    public class ConsumAiguaDAO : IConsumAiguaDAO
    {
        private readonly string connectionString;

        public ConsumAiguaDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ConsumAiguaDTO GetConsumAiguaDTOById(int id)
        {
            ConsumAiguaDTO contact = null;

            using (NpgsqlConnection connection = new NpgsqlConnection(NpgsqlUtils.OpenConnection()))
            {
                string query = "SELECT Id, Anys, Codi_Comarca, Comarca, Poblacio, Domestic_xarxa, Activitats_economiques_i_fonts_propies, Total, Consum_domestic_per_capital FROM ConsumAigua WHERE Id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // ORM: [--,--,--] -----> ContactDTO
                    contact = NpgsqlUtils.GetConsumAigua(reader);
                }
            }

            return contact;
        }

        public void AddConsumAiguaDTO(ConsumAiguaDTO consumAigua)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO ConsumAigua (Anys, Codi_Comarca, Comarca, Poblacio, Domestic_xarxa, Activitats_economiques_i_fonts_propies, Total, Consum_domestic_per_capital) VALUES (@Anys, @Codi_Comarca, @Comarca, @Poblacio, @Domestic_xarxa, @Activitats_economiques_i_fonts_propies, @Total, @Consum_domestic_per_capital)";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Anys", consumAigua.Anys);
                command.Parameters.AddWithValue("@Codi_Comarca", consumAigua.Codi_comarca);
                command.Parameters.AddWithValue("@Comarca", consumAigua.Comarca);
                command.Parameters.AddWithValue("@Poblacio", consumAigua.Poblacio);
                command.Parameters.AddWithValue("@Domestic_xarxa", consumAigua.Domestic_xarxa);
                command.Parameters.AddWithValue("@Activitats_economiques_i_fonts_propies", consumAigua.Activitats_economiques_i_fonts_propies);
                command.Parameters.AddWithValue("@Total", consumAigua.Total);
                command.Parameters.AddWithValue("@Consum_domestic_per_capital", consumAigua.Consum_domestic_per_capital);
                connection.Open();
                command.ExecuteNonQuery();
            }

        }
        public void UpdateConsumAiguaDTO(ConsumAiguaDTO consumAigua)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "UPDATE ConsumAigua SET Anys = @Anys, Codi_Comarca = @Codi_Comarca, Comarca = @Comarca, Poblacio = @Poblacio, Domestic_xarxa = @Domestic_xarxa, Activitats_economiques_i_fonts_propies = @Activitats_economiques_i_fonts_propies, Total = @Total, Consum_domestic_per_capital = @Consum_domestic_per_capital WHERE Id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Anys", consumAigua.Anys);
                command.Parameters.AddWithValue("@Codi_Comarca", consumAigua.Codi_comarca);
                command.Parameters.AddWithValue("@Comarca", consumAigua.Comarca);
                command.Parameters.AddWithValue("@Poblacio", consumAigua.Poblacio);
                command.Parameters.AddWithValue("@Domestic_xarxa", consumAigua.Domestic_xarxa);
                command.Parameters.AddWithValue("@Activitats_economiques_i_fonts_propies", consumAigua.Activitats_economiques_i_fonts_propies);
                command.Parameters.AddWithValue("@Total", consumAigua.Total);
                command.Parameters.AddWithValue("@Consum_domestic_per_capital", consumAigua.Consum_domestic_per_capital);
                connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public void DeleteConsumAiguaDTO(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "DELETE FROM ConsumAigua WHERE Id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ConsumAiguaDTO> GetAllConsumAiguaDTO()
        {
            List<ConsumAiguaDTO> contacts = new List<ConsumAiguaDTO>();

            using (NpgsqlConnection connection = new NpgsqlConnection(NpgsqlUtils.OpenConnection()))
            {
                string query = "SELECT Id, Anys, Codi_Comarca, Comarca, Poblacio, Domestic_xarxa, Activitats_economiques_i_fonts_propies, Total, Consum_domestic_per_capital FROM ConsumAigua";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // ORM: [--,--,--] -----> ContactDTO                  
                    ConsumAiguaDTO contact = NpgsqlUtils.GetConsumAigua(reader);
                    contacts.Add(contact);
                }
            }
            return contacts;
        }
    }
}