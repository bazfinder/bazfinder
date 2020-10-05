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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class PavilionsWindow : Window
    {
        public PavilionsWindow()
        {
            InitializeComponent();
            Load(new MySqlCommand("SELECT * FROM qwer.pavilion",Connect));
        }
        
        MySqlConnection Connect = new MySqlConnection("server=localhost;user id=root;password=;database=qwer;port=3306;persistsecurityinfo=True;sslmode=None");

        ////- SORTIROVKA

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

                MessageBox.Show("Ошибка  бд");
            }
            finally { Connect.Close(); }

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FloorFilterBT_Click(object sender, RoutedEventArgs e)//сортировка
        {
            Load(new MySqlCommand("SELECT * FROM qwer.pavilion WHERE level='" + FilterTB.Text + "';", Connect));
        }

        private void StatusFilterBT_Click(object sender, RoutedEventArgs e)
        {
            Load(new MySqlCommand("SELECT * FROM qwer.pavilion WHERE status='" + FilterTB.Text + "';", Connect));
        }

        private void CanelFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Load(new MySqlCommand("SELECT * FROM qwer.pavilion", Connect));
        }

        double Min, Max;

        private void InsertBT_Click(object sender, RoutedEventArgs e)//Добавление в бд
        {
            Insert(new MySqlCommand("INSERT INTO `pavilion`(`NameTC`, `PavilionN`, `level`, `status`, `square`, `price`, `KoeDopPrice`) VALUES ('" + NameTB.Text+"', '"+NomerPavilionTB.Text+"','"+LeveLT.Text+"','"+StatusT.Text+"','"+SquareT.Text+"','"+PriceTB.Text+"','"+CoefficentTB.Text+"')",Connect));
        }

        void Insert(MySqlCommand Cmd)
        {
            if (NameTB.Text !="" && NomerPavilionTB.Text !="" && LeveLT.Text != "" && StatusT.Text != "" && SquareT.Text != "" && PriceTB.Text !="" && CoefficentTB.Text != "" &&
                NameTB.Text != " " && NomerPavilionTB.Text != " " && LeveLT.Text != " " && StatusT.Text != " " && SquareT.Text != " " && PriceTB.Text != " " && CoefficentTB.Text != " ")
            {
                try
                {
                    Connect.Open();
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка с бд  \n\n\n\n\nПодробнее:\n" + ex.ToString());
                }
                finally
                {
                    Connect.Close();
                    Load(new MySqlCommand("SELECT * FROM qwer.pavilion", Connect));
                }
            }
            else
            {
                MessageBox.Show("Для изменения  заполните все поля.");
            }
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
                MySqlCommand Cmd = new MySqlCommand("UPDATE qwer.pavilion SET `status`='Удален' WHERE `NameTC`='" + text + "'", Connect);
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
            Load(new MySqlCommand("SELECT * FROM qwer.pavilion", Connect));
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
                MySqlCommand Cmd = new MySqlCommand("UPDATE `pavilion` SET `NameTC`='" + NameTB.Text + "',`PavilionN`='" + NomerPavilionTB.Text + "',`level`='" + LeveLT.Text + "',`status`='" + StatusT.Text + "',`square`='" + SquareT.Text + "',`price`='" + PriceTB.Text + "',`KoeDopPrice`='" + CoefficentTB.Text + "' WHERE `NameTC`='" + text + "'", Connect);
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
            Load(new MySqlCommand("SELECT * FROM qwer.pavilion", Connect));
        }

        private void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void SelectedRow(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (DataRowView Row in DataGrid.SelectedItems)
            {
                NameTB.Text = Row.Row.ItemArray[0].ToString();
                NomerPavilionTB.Text = Row.Row.ItemArray[1].ToString();
                LeveLT.Text = Row.Row.ItemArray[2].ToString();
                StatusT.Text = Row.Row.ItemArray[3].ToString();
                SquareT.Text = Row.Row.ItemArray[4].ToString();
                PriceTB.Text = Row.Row.ItemArray[5].ToString();
                CoefficentTB.Text = Row.Row.ItemArray[6].ToString();
            }
        }

        private void ToSC_Click(object sender, RoutedEventArgs e)
        {
            ManagerSWindow S = new ManagerSWindow();
            S.Show();
            Close();
           
        }

        private void MinSquareSortTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SquareFilterBT_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Min = Convert.ToDouble(MinSquareSortTB.Text);
                Max = Convert.ToDouble(MaxSquareSortTB.Text);
                Load(new MySqlCommand("SELECT * FROM `qwer`.`pavilion` WHERE `square` BETWEEN " + Min + " AND " + Max + "", Connect));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Введите данные");
            }

        }
    }
}
