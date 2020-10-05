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
using System.Collections.ObjectModel;
using System.Data;

namespace WpfAppDemEXAM1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ManagerSWindow : Window
    {
        public ManagerSWindow()
        {
            InitializeComponent();
            Load(new MySqlCommand("SELECT * FROM den.tc", Connect));
        }

        // MySqlCommand Cmd;  
        void Load(MySqlCommand Cmd)//Подключение к бд
        {
            try
            {
                Connect.Open();// выгружаем из бд таблицу
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

        MySqlConnection Connect = new MySqlConnection("server=localhost;user id=root;password=;database=den;port=3306;persistsecurityinfo=True;sslmode=None");

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }


        private void CityFilterBT_Click_1(object sender, RoutedEventArgs e)// Сортировки 
        {
            Load(new MySqlCommand("SELECT * FROM den.tc WHERE `City`='" + FilterTB.Text + "';", Connect));
        }

        private void StatusFilterBT_Copy_Click(object sender, RoutedEventArgs e)
        {
            Load(new MySqlCommand("SELECT * FROM den.tc WHERE `Status`='" + FilterTB.Text + "';", Connect));
        }

        private void CanelFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Load(new MySqlCommand("SELECT * FROM den.tc", Connect));

        }

        private void InsertBT_Click(object sender, RoutedEventArgs e)//Добавление в базу данных
        {
            try
            {
                Insert(new MySqlCommand("INSERT INTO `den`.`tc` (`Name`, `Status`, `Amount`, `City`, `Price`, `COEF`, `Level`) " +
                                "VALUES ('" + NameTB.Text + "', '" + Plan.Text + "', '" + AmountTB.Text + "', '" + Gorod.Text + "', '" + Price.Text + "', '" + COEF.Text + "', '" + Level.Text + "');", Connect));
            }
            catch { }
           
            
        }
        void Insert(MySqlCommand Cmd)
        {
            if (NameTB.Text != "" && Plan.Text != "" && AmountTB.Text != "" && Gorod.Text != "" && Price.Text != "" && COEF.Text != "" && Level.Text != "" &&
                NameTB.Text != " " && Plan.Text != " " && AmountTB.Text != " " && Gorod.Text != " " && Price.Text != " " && COEF.Text != " " && Level.Text != " ")
            {
                try
                {
                    Connect.Open();
                    Cmd.ExecuteNonQuery();



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка  с бд. \n\n\n\n\nПодробнее:\n" + ex.ToString());
                }
                finally
                {
                    Connect.Close();
                    Load(new MySqlCommand("SELECT * FROM den.tc", Connect));
                }
            }
            else
            {
                MessageBox.Show("Для изменения данных учетной записи заполните все необходимые поля.");
            }
        }
        void Del()//Помечение на удаление 
        {
            try
            {
                string text = String.Empty;
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    text = Row.Row.ItemArray[0].ToString();
                }
                MySqlCommand Cmd = new MySqlCommand("Update den.tc Set `Status`='Удален'  Where `Name`='" + text + "' ", Connect);
                Connect.Open();
                Cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Одинаковые поля города и Названия");
            }
            finally
            {
                Connect.Close();
            }
            Load(new MySqlCommand("SELECT * FROM den.tc", Connect));
        }

        void Update() // Редактирование
        {
            try
            {
                string text = String.Empty;
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    text = Row.Row.ItemArray[0].ToString(); 
                }
                MySqlCommand Cmd = new MySqlCommand ("UPDATE `den`.`tc` SET `Name`= '" + NameTB.Text + "',`Status`= '" + Plan.Text + "',`Amount`= '" + AmountTB.Text + "',`City`= '" + Gorod.Text + "',`Price`= '" + Price.Text + "',`COEF`= '" + COEF.Text + "',`Level`= '" + Level.Text + "' WHERE `Name`= '" + text + "' and `City`='"+ Gorod.Text +"';", Connect);
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
            Load(new MySqlCommand("SELECT * FROM den.tc", Connect));
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            Del();
        }

        private void UpdateBT_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void ToPavilions_Click(object sender, RoutedEventArgs e)//переход на павильоны
        {
            PavilionsWindow Pavilion = new PavilionsWindow();
            Pavilion.Show();
            Close();
        }

        
        private void SelectedRow(object sender, SelectedCellsChangedEventArgs e)// выгрузка бд данных
        {
            try
            {
                foreach (DataRowView Row in DataGrid.SelectedItems)
                {
                    NameTB.Text = Row.Row.ItemArray[0].ToString();
                    Plan.Text = Row.Row.ItemArray[1].ToString();
                    AmountTB.Text = Row.Row.ItemArray[2].ToString();
                    Gorod.Text = Row.Row.ItemArray[3].ToString();
                    Price.Text = Row.Row.ItemArray[4].ToString();
                    COEF.Text = Row.Row.ItemArray[5].ToString();
                    Level.Text = Row.Row.ItemArray[6].ToString();
                }
            }
            catch { }
        }

        private void FilterTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PriceKD(object sender, KeyEventArgs e)
        {
            if(e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
