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
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class ManagAForm : Window
    {
        public ManagAForm()
        {
            InitializeComponent();
            Load(new MySqlCommand("SELECT `Name`, `TelephoneNumber`, `Adress` , `Status`FROM `renters` ",Connect));
        }
        MySqlConnection Connect = new MySqlConnection("server=localhost;user id=root;password=;database=den;port=3306;persistsecurityinfo=True;sslmode=None");


        private void SelectedRow(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    NameTB.Text = Row.Row.ItemArray[0].ToString();
                    PhoneTB.Text = Row.Row.ItemArray[1].ToString();
                    AdressTB.Text = Row.Row.ItemArray[2].ToString();
                    Statustb.Text = Row.Row.ItemArray[3].ToString();
                }
            }
            catch { }
        }

        void Load(MySqlCommand Cmd)
        {
            try
            {
                Connect.Open();
                DataTable DT = new DataTable();
                DT.Load(Cmd.ExecuteReader());
                DataGrid.DataContext = DT;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ошибка подключения к бд");
            }
            finally { Connect.Close(); }

        }
        void Delete()
        {
            try
            {
                string text= String.Empty;
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    text = Row.Row.ItemArray[0].ToString();
                }
                MySqlCommand Cmd = new MySqlCommand("UPDATE `den`.`renters` SET `Status`='удален' WHERE `Name`='"+text+"'", Connect);
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
            Load(new MySqlCommand("SELECT `Name`, `TelephoneNumber`, `Adress` ,`Status` FROM `renters` ' ", Connect));
        }
        void Insert(MySqlCommand Cmd)
        {
            if (NameTB.Text!=""&&PhoneTB.Text!=""&&AdressTB.Text!=""&& NameTB.Text != " " && PhoneTB.Text != " " && AdressTB.Text != " " && AdressTB.Text != " ")
            {
                try
                {
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
                    Load(new MySqlCommand("SELECT `Name`, `TelephoneNumber`, `Adress` FROM `renters` ", Connect));
                }
            }
            else
            {
                MessageBox.Show("Для изменения данных учетной записи заполните все необходимые поля.");
            }
        }
        private void InsertBT_Click(object sender, RoutedEventArgs e)
        {
            Insert(new MySqlCommand("INSERT INTO `den`.`renters`(`Name`, `TelephoneNumber`, `Adress` , `Status`) VALUES ('" + NameTB.Text+"','"+PhoneTB.Text+"','"+AdressTB.Text+ "','" + Statustb.Text + "')", Connect));
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void UpdateBT_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
        void Update()//Редактирование данных
        {
            try
            {
                string text = String.Empty;
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    text = Row.Row.ItemArray[0].ToString();
                }
                MySqlCommand Cmd = new MySqlCommand("UPDATE `renters` SET `Name`='" + NameTB.Text + "',`TelephoneNumber`='" + PhoneTB.Text + "',`Adress`='" + AdressTB.Text +  "' WHERE `Name`='" + text + "'", Connect);
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
            Load(new MySqlCommand("SELECT * FROM den.renters", Connect));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }

        private void Statustb_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PhoneKD(object sender, KeyEventArgs e)
        {

        }
    }
}
