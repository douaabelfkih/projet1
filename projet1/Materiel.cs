using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace projet1
{
    public partial class Materiel : Form
    {
        SqlConnection cnx;
        SqlCommand cmd;
        SqlDataAdapter adap;
        SqlCommandBuilder cb;
        DataSet dset;

        int indice;
        public Materiel()
        {
            InitializeComponent();

        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEns_Click(object sender, EventArgs e)
        {
            Enseignant cl = new Enseignant(); cl.Show();
            this.Hide();
        }

        private void btnDEm_Click(object sender, EventArgs e)
        {
            Demande cl = new Demande(); cl.Show();
            this.Hide();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            Chat cl = new Chat(); cl.Show();
            this.Hide();
        }

        private void btnR_Click(object sender, EventArgs e)
        {
            Form1 cl = new Form1(); cl.Show();
            this.Hide();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Materiel_Load(object sender, EventArgs e)
        {

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=monprojet;Integrated Security=True";

            using (SqlConnection connection1 = new SqlConnection(connectionString))
            using (SqlConnection connection2 = new SqlConnection(connectionString))
            {
                connection1.Open();
                connection2.Open();

                // Commande pour les types
                SqlCommand cmdTypes = new SqlCommand("SELECT DISTINCT CAST(Type AS NVARCHAR(MAX)) AS Type FROM Materiel", connection1);
                SqlDataReader readerTypes = cmdTypes.ExecuteReader();

                while (readerTypes.Read())
                {
                    string typeMateriel = readerTypes["Type"].ToString();
                    TreeNode typeNode = new TreeNode(typeMateriel);

                    // Commande pour les références
                    SqlCommand cmdReferences = new SqlCommand("SELECT Reference FROM Materiel WHERE CAST(Type AS NVARCHAR(MAX)) = @type", connection2);
                    cmdReferences.Parameters.AddWithValue("@type", typeMateriel);

                    SqlDataReader readerReferences = cmdReferences.ExecuteReader();
                    while (readerReferences.Read())
                    {
                        string reference = readerReferences["Reference"].ToString();
                        TreeNode referenceNode = new TreeNode(reference);
                        typeNode.Nodes.Add(referenceNode);
                    }
                    readerReferences.Close();

                    treeView1.Nodes.Add(typeNode);
                }

                readerTypes.Close();
            }
            using (SqlConnection connection3 = new SqlConnection(connectionString))
            {
                connection3.Open();

                string query = @"
                    SELECT 
                        M.Reference, 
                        M.Type, 
                        CASE 
                            WHEN M.EnseignantId IS NULL THEN 'Libre' 
                            ELSE E.Nom 
                        END AS Affectation
                    FROM Materiel M
                    LEFT JOIN Enseignant E ON M.EnseignantId = E.Id";


                // Initialiser correctement l'adap
                SqlDataAdapter adap = new SqlDataAdapter(query, connection3);

                // Créer un DataTable pour stocker les résultats
                DataTable dataTable = new DataTable();

                // Remplir le DataTable avec les résultats de la requête
                adap.Fill(dataTable);

                // Associer le DataTable au DataGridView pour afficher les résultats
                dataGridView1.DataSource = dataTable;  // Afficher les données dans le DataGridView
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string queryMateriel = "SELECT Reference, Type FROM Materiel WHERE EnseignantId IS NULL";  // Matériels libres
                SqlCommand cmdMateriel = new SqlCommand(queryMateriel, connection);
                SqlDataReader readerMateriel = cmdMateriel.ExecuteReader();

                while (readerMateriel.Read())
                {
                    string reference = readerMateriel["Reference"].ToString();
                    string type = readerMateriel["Type"].ToString();
                    cmbMateriel.Items.Add(new { Reference = reference, Type = type });
                }
                readerMateriel.Close();
            }



        }
        private void RafraichirMateriels()
        {// Rechargez les matériels affectés et libérés, selon vos besoins
            cmbMateriel.Items.Clear();
            cmbMaterielL.Items.Clear();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=monprojet;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Charger les matériels affectés dans cmbMaterielL (affichage des matériels affectés)
                string query = "SELECT Reference, Type FROM Materiel WHERE EnseignantId IS NOT NULL";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string reference = reader["Reference"].ToString();
                    string type = reader["Type"].ToString();
                    cmbMaterielL.Items.Add(reference + " - " + type);
                }
                reader.Close();

                // Charger les matériels disponibles dans cmbMateriel (affichage des matériels disponibles)
                string queryLibre = "SELECT Reference, Type FROM Materiel WHERE EnseignantId IS NULL";
                SqlCommand cmdLibre = new SqlCommand(queryLibre, connection);
                SqlDataReader readerLibre = cmdLibre.ExecuteReader();
                while (readerLibre.Read())
                {
                    string reference = readerLibre["Reference"].ToString();
                    string type = readerLibre["Type"].ToString();
                    cmbMateriel.Items.Add(reference + " - " + type);
                }
                readerLibre.Close();
            }
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private string selectedReference = string.Empty;  // Variable pour stocker la référence sélectionnée
        private int? selectedEnseignantId = null;  // Variable pour stocker l'ID de l'enseignant
        private void btnAffecter_Click(object sender, EventArgs e)
        {
            if (cmbMateriel.SelectedItem != null && cmbEnseignants.SelectedItem != null)
            {
                // Récupérer la référence sélectionnée
                string selectedItem = cmbMateriel.SelectedItem.ToString();
                selectedReference = selectedItem.Split(' ')[0]; // Extraire la référence

                // Récupérer l'ID de l'enseignant sélectionné
                string enseignantName = cmbEnseignants.SelectedItem.ToString();
                string enseignantId = enseignantName.Split(' ')[0];  // Extraire l'ID de l'enseignant
                selectedEnseignantId = int.Parse(enseignantId);

                // Afficher un message pour confirmer que l'affectation a bien été sélectionnée
                MessageBox.Show("Le matériel a été sélectionné pour affectation.");
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel et un enseignant.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbMaterielL.SelectedItem != null)
            {
                // Récupérer la référence sélectionnée pour la libération
                string selectedItem = cmbMaterielL.SelectedItem.ToString();
                selectedReference = selectedItem.Split(' ')[0]; // Extraire la référence
                selectedEnseignantId = null;  // Aucun enseignant associé lors de la libération

                // Afficher un message pour confirmer que la libération a bien été sélectionnée
                MessageBox.Show("Le matériel a été sélectionné pour libération.");
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un matériel à libérer.");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedReference))
            {
                try
                {
                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=monprojet;Integrated Security=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Mise à jour de l'EnseignantId ou suppression (libération)
                        string query = selectedEnseignantId.HasValue
                            ? "UPDATE Materiel SET EnseignantId = @EnseignantId WHERE Reference = @Reference"
                            : "UPDATE Materiel SET EnseignantId = NULL WHERE Reference = @Reference";

                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Reference", selectedReference);
                        if (selectedEnseignantId.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@EnseignantId", selectedEnseignantId.Value);
                        }

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Mise à jour réussie !");
                            // Vous pouvez également recharger les données pour afficher les changements
                            RafraichirMateriels();
                        }
                        else
                        {
                            MessageBox.Show("Aucun matériel trouvé avec cette référence.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la mise à jour du matériel : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Aucun matériel sélectionné pour la mise à jour.");
            }
        }
    }
}
