using System.Globalization;
using System;
using System.Xml.Linq;
using CsvHelper.Configuration;
using CsvHelper;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using dao.Persistence.Utils;
using dao.Persistence.DAO;
using dao.Persistence.Mapping;
using dao.DTOs;

namespace M03.UF5.AC3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CargarDatosCSV();
            FillComarca();

        }
        private void CargarDatosCSV()
        {
            string rutaArchivo = @"..\..\..\Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv";

            dataGridView1.DataSource = XMLHelper.LeerCSV(rutaArchivo);
            dataGridView1.Columns[1].Visible = false;
        }

        private void FillComarca()
        {
            string xmlFilePath = "AguasXML.xml";
            XDocument xmlDoc = XDocument.Load(xmlFilePath);

            var rows = from row in xmlDoc.Descendants("row")
                       select new ConsumAigua
                       {
                           Anys = (int)row.Element("Any"),
                           Codi_comarca = (int)row.Element("Codi_comarca"),
                           Comarca = (string)row.Element("Comarca"),
                           Poblacio = (int)row.Element("Població"),
                           Domestic_xarxa = (double)row.Element("Domèstic_xarxa"),
                           Activitats_economiques_i_fonts_propies = (double)row.Element("Activitats_econòmiques_i_fonts_pròpies"),
                           Total = (double)row.Element("Total"),
                           Consum_domestic_per_capital = (double)row.Element("Consum_domèstic_per_càpita")
                       };
            foreach (var row in rows)
            {
                comarcaBox.Items.Add(row.Comarca);
            }
        }


        private void netejar_Click(object sender, EventArgs e)
        {
            anyBox.Text = "";
            comarcaBox.Text = "";
            poblacioBox.Text = "";
            domesticBox.Text = "";
            activitatsBox.Text = "";
            consumBox.Text = "";
            totalBox.Text = "";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int result = Int32.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
            if (result > 20000) poblacioLabel.Text = "SI";
            else poblacioLabel.Text = "NO";

            string nomComarca = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            int conXarxa = Int32.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            int poblacio = Int32.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
            double consum = conXarxa / poblacio;
            consumLabel.Text = consum.ToString();


            XDocument xDocument = XDocument.Load("AguasXML.xml");
            var comarcaAlt = (from comarca in xDocument.Descendants("row")
                              where comarca.Element("Comarca").Value == nomComarca
                              orderby Convert.ToDouble(comarca.Element("Consum_domèstic_per_càpita").Value) descending
                              select comarca.Element("Consum_domèstic_per_càpita").Value).FirstOrDefault();

            if (double.Parse(comarcaAlt) == double.Parse(dataGridView1.CurrentRow.Cells[7].Value.ToString()))
            {
                domesticAltLabel.Text = "SI";
            }
            else
            {
                domesticAltLabel.Text = "NO";
            }


            var comarcaBaix = (from comarca in xDocument.Descendants("row")
                               where comarca.Element("Comarca").Value == nomComarca
                               orderby Convert.ToDouble(comarca.Element("Consum_domèstic_per_càpita").Value)
                               select comarca.Element("Consum_domèstic_per_càpita").Value).FirstOrDefault();

            if (double.Parse(comarcaBaix) == double.Parse(dataGridView1.CurrentRow.Cells[7].Value.ToString()))
            {
                domesticBaixLabel.Text = "SI";
            }
            else
            {
                domesticBaixLabel.Text = "NO";
            }

        }

        Dictionary<string, int> comarcaToCodiComarca = new Dictionary<string, int>()
        {
            { "ALT CAMP, L'", 1 },
            { "ALT EMPORDÀ, L'", 2 },
            { "ALT PENEDÈS, L'", 3 },
            { "ALT URGELL, L'", 4 },
            { "ALTA RIBAGORÇA, L'", 5 },
            { "ANOIA, L'", 6 },
            { "BAGES, EL", 7 },
            { "BAIX CAMP, EL", 8 },
            { "BAIX EBRE, EL", 9 },
            { "BAIX EMPORDÀ, EL", 10 },
            { "BAIX LLOBREGAT, EL", 11 },
            { "BAIX PENEDÈS, EL", 12 },
            { "BARCELONÈS, EL", 13 },
            { "BERGUEDÀ, EL", 14 },
            { "CERDANYA, LA", 15 },
            { "CONCA DE BARBERÀ, LA", 16 },
            { "GARRAF, EL", 17 },
            { "GARRIGUES, LES", 18 },
            { "GARROTXA, LA", 19 },
            { "GIRONÈS, EL", 20 },
            { "MARESME, EL", 21 },
            { "MONTSIÀ, EL", 22 },
            { "NOGUERA, LA", 23 },
            { "OSONA", 24 },
            { "PALLARS JUSSÀ, EL", 25 },
            { "PALLARS SOBIRÀ, EL", 26 },
            { "PLA D'URGELL, EL", 27 },
            { "PLA DE L'ESTANY, EL", 28 },
            { "PRIORAT, EL", 29 },
            { "RIBERA D'EBRE, LA", 30 },
            { "RIPOLLÈS, EL", 31 },
            { "SEGARRA, LA", 32 },
            { "SEGRIÀ, EL", 33 },
            { "SELVA, LA", 34 },
            { "SOLSONÈS, EL", 35 },
            { "TARRAGONÈS, EL", 36 },
            { "TERRA ALTA", 37 },
            { "URGELL, L'", 38 },
            { "VAL D'ARAN, LA", 39 },
            { "VALLÈS OCCIDENTAL, EL", 40 },
            { "VALLÈS ORIENTAL, EL", 41 },
            { "Moianès", 42 }
        };

        private void guardar_Click(object sender, EventArgs e)
        {

            int year, poblacion, domesticXarxa2, activities, total2;
            double consumPerCapita2;
            string comarca1;

            errorProvider1.Clear();

            try
            {
                year = int.Parse(anyBox.Text);
                if (year < 2012 || year > 2050) throw new Exception();
            }
            catch (Exception)
            {
                errorProvider1.SetError(anyBox, "Ingresa el año correctamente");
                return;
            }

            try
            {
                comarca1 = comarcaBox.Text;
                if (comarca1 == string.Empty) throw new Exception();

            }
            catch (Exception)
            {
                errorProvider1.SetError(comarcaBox, "Ingresa la comarca correctamente.");
                return;
            }

            try
            {
                poblacion = int.Parse(poblacioBox.Text);
                if (poblacion < 0) throw new Exception();
                errorProvider1.Clear();
            }
            catch (Exception)
            {
                errorProvider1.SetError(poblacioBox, "La poblacion ha de ser un numero entero");
                return;
            }

            try
            {
                domesticXarxa2 = int.Parse(domesticBox.Text);
                if (domesticXarxa2 < 0) throw new Exception();
                errorProvider1.Clear();
            }
            catch (Exception)
            {
                errorProvider1.SetError(domesticBox, "'Domèstic xarxa' ha de ser un numero entero");
                return;
            }

            try
            {
                activities = int.Parse(activitatsBox.Text);
                if (activities < 0)
                    throw new Exception();
                errorProvider1.Clear();
            }
            catch (Exception)
            {
                errorProvider1.SetError(activitatsBox, "'Gestió de dades demogràfiques de gestió' ha de ser un numero entero");
                return;
            }

            try
            {
                consumPerCapita2 = double.Parse(consumBox.Text);
                if (consumPerCapita2 < 0) throw new Exception();
                errorProvider1.Clear();
            }
            catch (Exception)
            {
                errorProvider1.SetError(consumBox, "'Consum domèstic per capità'  ha de ser un numero entero ");
                return;
            }

            try
            {
                total2 = int.Parse(totalBox.Text);
                if (total2 < 0)
                    throw new Exception();
                errorProvider1.Clear();
            }
            catch (Exception)
            {
                errorProvider1.SetError(totalBox, "'Total' ha de ser un numero entero");
                return;
            }


            string any = anyBox.Text;
            string comarca = comarcaBox.Text;
            string poblacio = poblacioBox.Text;
            string domesticXarxa = domesticBox.Text;
            string activitats = activitatsBox.Text;
            string total = totalBox.Text;
            string consumPerCapita = consumBox.Text;

            int codiComarca;
            if (comarcaToCodiComarca.TryGetValue(comarca, out codiComarca))
            {
                var records = new List<ConsumAigua>
                {
                    new ConsumAigua
                    {
                        Anys = int.Parse(any),
                        Codi_comarca = codiComarca,
                        Comarca = comarca,
                        Poblacio = int.Parse(poblacio),
                        Domestic_xarxa = double.Parse(domesticXarxa),
                        Activitats_economiques_i_fonts_propies = double.Parse(activitats),
                        Total = double.Parse(total),
                        Consum_domestic_per_capital = double.Parse(consumPerCapita)
                    }
                };

                AppendCsv(records);

                MessageBox.Show("Datos guardados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("La comarca ingresada no tiene un código de comarca asociado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private static void AppendCsv(List<ConsumAigua> records)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using var stream = File.Open(@"..\..\..\Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv", FileMode.Append);
            using var writer = new StreamWriter(stream);
            using var csvWriter = new CsvWriter(writer, config);

            csvWriter.WriteRecords(records);
        }

        private void persistir_Click(object sender, EventArgs ee)
        {
            // Crear una instància del DAO
            IConsumAiguaDAO consumaiguaDao = new ConsumAiguaDAO(NpgsqlUtils.OpenConnection());

            // Exemple d'ús del DAO
            ConsumAiguaDTO newContact = new ConsumAiguaDTO
            {
                Anys = 2021,
                Codi_comarca = 1,
                Comarca = "ALT CAMP, L'",
                Poblacio = 20000,
                Domestic_xarxa = 1000,
                Activitats_economiques_i_fonts_propies = 2000,
                Total = 3000,
                Consum_domestic_per_capital = 4000
            };

            // Afegir un nou contacte
            try
            {
                consumaiguaDao.AddConsumAiguaDTO(newContact);
                MessageBox.Show("Datos guardados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        /*    //Recuperar el contacte per id
            try
            {
                var contact = consumaiguaDao.GetConsumAiguaDTOById(3);
                Console.WriteLine($"ID: {contact.Codi_comarca}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/
/*
            //Recuperar tots els contactes
            var contacts = consumaiguaDao.GetAllConsumAiguaDTO();
            Console.WriteLine($"ID | Nom  \t| Cognom");
            foreach (var row in contacts)
            {
                Console.WriteLine($"{row.Codi_comarca}");
            }

            // Actualitzar un contacte   
            try
            {
                ConsumAiguaDTO contact = new ConsumAiguaDTO
                {

                };
                consumaiguaDao.UpdateConsumAiguaDTO(contact);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Eliminar un contacte

            try
            {
                consumaiguaDao.DeleteConsumAiguaDTO(8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/
        }
    }
}
