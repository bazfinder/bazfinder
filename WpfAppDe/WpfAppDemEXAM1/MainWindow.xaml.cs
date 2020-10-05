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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.IO;



namespace WpfAppDemEXAM1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            CaptchaText.Visibility = Visibility.Hidden;
            CaptchaTB.Visibility = Visibility.Hidden;
            RepeatCaptcha.Visibility = Visibility.Hidden;
            CaptchaBG.Visibility = Visibility.Visible;
        }
        int LogCount = 1;
        string Pass = "", Role = "";
        MySqlConnection Connect = new MySqlConnection("server=localhost;user id=root;password=;database=den;port=3306;persistsecurityinfo=True;sslmode=None"); 

        private void Connection()
        {
 
            MySqlCommand Cmd = new MySqlCommand("SELECT * FROM den.users where Login='" + LoginTB.Text +  "';", Connect);
            try
            {
                Connect.Open();
                MySqlDataReader Data = Cmd.ExecuteReader();
                if (Data.Read())
                {
                    Pass = Data[4].ToString();
                    Role = Data[5].ToString();

                }
                else
                {
                    MessageBox.Show("Нету такакого логина");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД!!!");
            }
            finally { Connect.Close(); }
        }
        private void LogBT_Click(object sender, RoutedEventArgs e)//Система авторизации
        {
            if (LoginTB.Text != "" && PasswordBox.Password != "")
            {
                if (LogCount <= 3)
                {
                    Connection();
                    if (LogCount==3)
                    {
                        Captcha();
                        CaptchaBG.Visibility = Visibility.Visible;
                        CaptchaText.Visibility = Visibility.Visible;
                        CaptchaTB.Visibility = Visibility.Visible;
                        RepeatCaptcha.Visibility = Visibility.Visible;
                    }
                }
               else 
               {   
                    if (CaptchaTB.Text == CaptchaText.Content.ToString())
                    {
                        Connection();
                    }
                    else
                    {
                        MessageBox.Show("Капча не та!!!");
                    }
               }

                if (PasswordBox.Password.Equals(Pass))
                {
                   
                    switch (Role)
                    {
                        case "Администратор":
                            {
                                AdminWindow admin = new AdminWindow();
                                admin.Show();
                                Close();
                                break;
                            }
                        case "Менеджер С":
                            {
                                ManagerSWindow ManagS = new ManagerSWindow();
                                ManagS.Show();
                                Close();
                                break;
                            }
                        case "Менеджер А":
                            {
                                ManagAForm ManagA = new ManagAForm();
                                ManagA.Show();
                                Close();
                                break;
                            }
                        case "Удален":
                            {
                                MessageBox.Show("DELETED");
                                break;
                            }
                    }

                }
                else
                {
                    MessageBox.Show("пароль не правильные");
                }
            }
            else
            {
                MessageBox.Show("данные не заполнены!!!");
            }
            LogCount++;
        }

        private void Captcha() // капча
        {
            string Cap = String.Empty;
            Random Rnd = new Random();
            string Alf = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 6; i++)
            {
                Cap += Alf[Rnd.Next(Alf.Length)];
            }
            CaptchaText.Content = Cap;
            

        }

        private void RepeatCaptcha_Click(object sender, RoutedEventArgs e)
        {
            CaptchaText.Content = "";
            Captcha();
        }
    }
    

}
