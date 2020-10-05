using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;


namespace WpfAppDemEXAM1
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        DataTable DT = new DataTable();
        string Zaharlox;
        public AdminWindow()
        {
            InitializeComponent();
            Load(new MySqlCommand("SELECT * FROM den.users", Connect));
        }
       
            MySqlConnection Connect = new MySqlConnection("server=localhost;userid=root;password=;database=den;port=3306;persistsecurityinfo=True;sslmode=None");

        void Load(MySqlCommand Cmd)
        {
            try
            {
                Connect.Open();
              
                DT.Load(Cmd.ExecuteReader());
                DataGrid.DataContext = DT;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ошибка подключения к бд");
            }
            finally { Connect.Close(); }

        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
       
        }
        void Insert(MySqlCommand Cmd)
        {
           



            if (FIOTB.Text != "" && LoginT.Text != "" && PasswordT.Text != "" && Role.Text != "" && PhoneT.Text != "" && pol.Text != "" && 
                FIOTB.Text != " " && LoginT.Text != " " && PasswordT.Text != " " && Role.Text != " " && PhoneT.Text != " " && pol.Text != " ")
            {
                try
                {
                    Connect.Open();
                    Cmd.ExecuteNonQuery();



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Одиниковое поле значений" );
                }
                finally
                {
                    Connect.Close();
                    Load(new MySqlCommand("SELECT * FROM den.users", Connect));
                }
            }
            else
            {
                MessageBox.Show("Для изменения данных учетной записи заполните все необходимые поля.");
            }
        }


        private void InsertBT_Click(object sender, RoutedEventArgs e)
        {
           
            Insert(new MySqlCommand("INSERT INTO `den`.`users` (`FIO`,`Surname`, `Midlname`, `Login`, `Password`, `Pole`, `Nomer`, `Gender`) " +
                   "VALUES ('" + FIOTB.Text + "', '" + sur.Text + "', '" + mid.Text + "', '" + LoginT.Text + "', '" + PasswordT.Text + "', '" + Role.Text + "', '" + PhoneT.Text + "', '" + pol.Text + "');", Connect));
        }
        

        private void UpdateBT_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        void Update()
        {
            try
            {
                string text = String.Empty;
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    text = Row.Row.ItemArray[0].ToString();
                }
                MySqlCommand Cmd = new MySqlCommand("UPDATE `den`.`users` SET `FIO`='" + FIOTB.Text+ "', `Surname`='" + sur.Text + "', `Midlname`='" + mid.Text + "', `Login`='" + LoginT.Text+"',`Password`='"+PasswordT.Text+"',`Pole`='"+Role.Text+"',`Nomer`='"+PhoneT.Text+"',`Gender`='"+pol.Text+"' WHERE `FIO`='" + Zaharlox + "';", Connect);
                Connect.Open();
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка соединения с базой данных. \n\n\n\n\nПодробнее:\n" + ex.ToString());
            }
            finally
            {
                Connect.Close();
            }
            Load(new MySqlCommand("SELECT * FROM den.users", Connect));
        }

        void Delete()
        {
            try
            {
                string text = String.Empty;
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    text = Row.Row.ItemArray[0].ToString();
                }
                MySqlCommand Cmd = new MySqlCommand("UPDATE den.users SET `Pole`='Удален' WHERE `FIO`='" + text + "'", Connect);
                Connect.Open();
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка соединения с базой данных. \n\n\n\n\nПодробнее:\n" + ex.ToString());
            }
            finally
            {
                Connect.Close();
            }
            Load(new MySqlCommand("SELECT * FROM den.users", Connect));
        }
        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void SelectedRow(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (DataRowView Row in DataGrid.SelectedItems)
            {
               FIOTB.Text = Row.Row.ItemArray[0].ToString();
                sur.Text = Row.Row.ItemArray[1].ToString();
                mid.Text = Row.Row.ItemArray[2].ToString();
                LoginT.Text = Row.Row.ItemArray[3].ToString();
               PasswordT.Text = Row.Row.ItemArray[4].ToString();
               Role.Text = Row.Row.ItemArray[5].ToString();
               PhoneT.Text= Row.Row.ItemArray[6].ToString();
               pol.Text = Row.Row.ItemArray[7].ToString();
                Zaharlox = FIOTB.Text;
            }
        }

        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {
            Load(new MySqlCommand("Select From den.users Where `FIO`='" + pois.Text+"'",Connect));

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }

        private void Pois_TextChanged(object sender, TextChangedEventArgs e)
        {
            Load(new MySqlCommand("SELECT * FROM den.users", Connect));
            string search = String.Empty;
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                search = DT.Rows[i].ItemArray[0].ToString().ToLower();
                if (!DT.Rows[i].ItemArray[0].ToString().ToLower().Contains(pois.Text))
                {
                    DT.Rows.RemoveAt(i);
                    i--;
                }

            }
        }
    }
}
