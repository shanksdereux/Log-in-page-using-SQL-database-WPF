using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Log_in_page_using_SQL_database_WPF
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadGrid();
        }


        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-NV42M9C;Initial Catalog=LoginDB;Integrated Security=True");

        public void LoadGrid()
        {
            string query = "SELECT * FROM tblInfo";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            DataTable dataTable = new DataTable();
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            dataTable.Load(sqlDataReader);
            sqlConnection.Close();
            datagrid.ItemsSource = dataTable.DefaultView;
        }

        public bool IsValid()
        {
            if (txtboxName.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (txtboxAge.Text == string.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (txtboxGender.Text == string.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (txtboxCity.Text == string.Empty)
            {
                MessageBox.Show("City is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public void clearData()
        {
            txtboxName.Clear();
            txtboxAge.Clear();
            txtboxGender.Clear();
            txtboxCity.Clear();
            Id_number.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            try
            {
                if (IsValid())
                {
                    string queryInsert = "INSERT INTO tblInfo VALUES (@Name, @Age, @Gender, @City)";
                    SqlCommand sqlCommandInsert = new SqlCommand(queryInsert, sqlConnection);
                    sqlCommandInsert.CommandType = CommandType.Text;
                    sqlCommandInsert.Parameters.AddWithValue("@Name", txtboxName.Text);
                    sqlCommandInsert.Parameters.AddWithValue("@Age", txtboxAge.Text);
                    sqlCommandInsert.Parameters.AddWithValue("@Gender", txtboxGender.Text);
                    sqlCommandInsert.Parameters.AddWithValue("@City", txtboxCity.Text);
                    sqlConnection.Open();
                    sqlCommandInsert.ExecuteNonQuery();
                    sqlConnection.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully Registered!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string queryDelete = "DELETE FROM tblInfo WHERE ID = " + Id_number.Text;
            SqlCommand sqlCommandDelete = new SqlCommand(queryDelete, sqlConnection);

            try
            {
                sqlCommandDelete.ExecuteNonQuery();
                MessageBox.Show("The record you've selected has been deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                sqlConnection.Close();
                clearData();
                LoadGrid();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not Deleted" + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string queryUpdate = "UPDATE tblInfo set Name = '" + txtboxName.Text + "', Age = '" + txtboxAge.Text + "', Gender = '" + txtboxGender.Text + "', City = '" + txtboxCity.Text + "' WHERE ID = '" + Id_number.Text + "'";
            SqlCommand sqlCommandUpdate = new SqlCommand(queryUpdate, sqlConnection);

            try
            {
                sqlCommandUpdate.ExecuteNonQuery();
                MessageBox.Show("Record has been updated successfully!","Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating the data." + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                clearData();
                LoadGrid();
            }
        }
    }
}
