using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;


namespace Log_in_page_using_SQL_database_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string connectString = "Data Source=DESKTOP-NV42M9C;Initial Catalog=LoginDB;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectString);

            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                    string query = "SELECT COUNT (1) FROM tblUser WHERE @Username = Username AND @Password = Password";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Username", txtUserName.Text);
                    sqlCommand.Parameters.AddWithValue("@Password", txtPassword.Password);
                    int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    if (count == 1)
                    {
                        Dashboard dashboard = new Dashboard();
                        dashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password is Incorrect! ");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
