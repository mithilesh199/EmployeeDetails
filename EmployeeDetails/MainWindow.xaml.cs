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

namespace EmployeeDetails
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();

        public MainWindow()
        {
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://gorest.co.in/public/v2/users");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //InitializeComponent();
            //HttpResponseMessage response = client.GetAsync("https://gorest.co.in/public/v2/users").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var employees = response.Content.ReadAsAsync<IEnumerable<Employee>>().Result;
            //    Employee.ItemsSource = employees;

            //}
            //else
            //{
            //    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            //}

            // }
            //public MainWindow()
            //{

            InitializeComponent();
            LoadGrid();

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://gorest.co.in/public/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //InitializeComponent();

        }

        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Empdb;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from FirstTable", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }

        //private async void GetEmployees()
        //{
            

        //    var response = await client.GetStringAsync("https://gorest.co.in/public/v2/users");
        //    //var json = JsonConvert.SerializeObject(response);
        //    var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(response);
        //    //var employees = JsonConvert.DeserializeObject<List<Employee>>(json);
        //    datagrid.DataContext = employees;
        //}


        private void Loaddatabtn_Click(object sender, RoutedEventArgs e)
        {
            //HttpClient client = new HttpClient();

            //client.BaseAddress = new Uri("https://localhost:44324/api/");

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //var id = id_txt.Text.Trim();

            //var url = "/api/Employee/GetAllEmployees" + id;

            //HttpResponseMessage response = client.GetAsync(url).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    var employees = response.Content.ReadAsAsync<Employee>().Result;
            //    MessageBox.Show("Employee Found : " + employees.Id + " " + employees.Name + " " + employees.Age + " " + employees.Gender + " " + employees.Email);
            //}
            //else
            //{
            //    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            //}
            LoadGrid();

            //this.GetEmployees();

        }

        public bool isValid()
        {

            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == string.Empty)
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
            age_txt.Clear();
            gender_txt.Clear();
            email_txt.Clear();
        }

        private void Insertbtn_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44324/api/");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var employee = new Employee();

            employee.Id = int.Parse(id_txt.Text);

            employee.Name = name_txt.Text;

            employee.Age = age_txt.Text;

            employee.Gender = gender_txt.Text;

            employee.Email = email_txt.Text;

            var response = client.PostAsJsonAsync("/api/Employee/AddEmployee", employee).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Employee Added");

                id_txt.Text = "";

                name_txt.Text = "";

                age_txt.Text = "";

                gender_txt.Text = "";               

            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }

        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44324/api/");

            var id = id_txt.Text.Trim();

            var url = "/api/Employee/DeleteEmployee/" + id;

            HttpResponseMessage response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("User Deleted");
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void Updatebtn_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:44324/api/");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var employee = new Employee();

            employee.Id = int.Parse(id_txt.Text);

            employee.Name = name_txt.Text;

            employee.Age = age_txt.Text;

            employee.Gender = gender_txt.Text;

            employee.Email = email_txt.Text;

            var response = client.PutAsJsonAsync("/api/Employee/UpdateEmployee/", employee).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Employee Updated");

                id_txt.Text = "";

                name_txt.Text = "";

                age_txt.Text = "";

                gender_txt.Text = "";               

            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void Clearbtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }
    }
}
