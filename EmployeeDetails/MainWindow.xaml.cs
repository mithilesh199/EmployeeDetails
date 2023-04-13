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
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using EmployeeDetails.Models;
using System.Net;
using System.IO;
using RestSharp;
using System.Web;
using Newtonsoft.Json;

//using Microsoft.Extensions.Logging;
//using System.Text.Json.Nodes;

namespace EmployeeDetails
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string token = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

        public MainWindow()
        {
            InitializeComponent();
            GetEmployees();
        }

        private async void GetEmployees()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gorest.co.in/public-api/users");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer" + token);
            lblMessage.Content = " ";

            var response = await client.GetStringAsync("https://gorest.co.in/public/v2/users");

            var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(response);

            datagrid.DataContext = employees;
        }

        private async void SaveEmployee(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gorest.co.in/public-api/users");
            client.Timeout = TimeSpan.FromSeconds(900);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + token);
            HttpContent httpContent = null;
            var ms = new MemoryStream();
            //SerializeJsonIntoStream(content, ms);
            using (var sw = new StreamWriter(ms, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, employee);
                jtw.Flush();
            }
            ms.Seek(0, SeekOrigin.Begin);
            httpContent = new StreamContent(ms);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("https://gorest.co.in/public-api/users", httpContent);

            //await client.PostAsJsonAsync("/public/v2/users", httpContent);


            //this.GetEmployees();
        }

        private async void UpdateEmployee(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gorest.co.in/public-api/users");
            client.Timeout = TimeSpan.FromSeconds(900);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + token);
            HttpContent httpContent = null;
            var ms = new MemoryStream();
            //SerializeJsonIntoStream(content, ms);
            using (var sw = new StreamWriter(ms, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, employee);
                jtw.Flush();
            }
            ms.Seek(0, SeekOrigin.Begin);
            httpContent = new StreamContent(ms);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PutAsync("https://gorest.co.in/public-api/users/" + employee.id, httpContent);
        }

        private async void DeleteEmployee(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gorest.co.in/public-api/users");
            //client.Timeout = TimeSpan.FromSeconds(900);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + token);
            var response = await client.DeleteAsync("https://gorest.co.in/public-api/users/" + id);
            //await client.DeleteAsync("https://gorest.co.in/public-api/users" + Id);
        }


        public bool isValid()
        {

            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (status_txt.Text == string.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == string.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (email_txt.Text == string.Empty)
            {
                MessageBox.Show("Email is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public void clearData()
        {
            id_txt.Clear();
            name_txt.Clear();
            status_txt.Clear();
            gender_txt.Clear();
            email_txt.Clear();
        }

        private void Clearbtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void Loaddatabtn_Click(object sender, RoutedEventArgs e)
        {

            this.GetEmployees();
        }

        private void Insertbtn_Click(object sender, RoutedEventArgs e)
        {
            var employee = new Employee()
            {
                id = Convert.ToInt32(id_txt.Text),
                name = name_txt.Text,
                status = status_txt.Text,
                gender = gender_txt.Text,
                email = email_txt.Text
            };

            if (employee.id == 0)
            {
                this.SaveEmployee(employee);
                lblMessage.Content = "Employee Saved";
            }
            else
            {
                //this.UpdateEmployee(employee);
                lblMessage.Content = "Employee not saved";
            }
            id_txt.Text = "".ToString();
            name_txt.Text = " ";
            status_txt.Text = " ";
            gender_txt.Text = " ";
            email_txt.Text = " ";

        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee();
            this.DeleteEmployee(employee.id);
        }

        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {

            var employee = new Employee()
            {
                id = Convert.ToInt32(id_txt.Text),
                name = name_txt.Text,
                status = status_txt.Text,
                gender = gender_txt.Text,
                email = email_txt.Text
            };

            if (employee.id != 0)
            {
                this.UpdateEmployee(employee);
                lblMessage.Content = "Employee Updated";
            }
            else
            {
                //this.UpdateEmployee(employee);
                lblMessage.Content = "Employee not updated";
            }
            id_txt.Text = 0.ToString();
            name_txt.Text = " ";
            status_txt.Text = " ";
            gender_txt.Text = " ";
            email_txt.Text = " ";
        }

        
    }
}
