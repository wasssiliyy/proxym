using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

namespace proxym
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    interface IBase
    {
        void SetValue(string value);
        List<string> GetValue(string value);
    }

    class DataBase : IBase
    {

        public List<string> Db { get; set; }


        public DataBase()
        {
            Db = new List<string>();
            var txt = File.ReadAllText(@"-/../../../Database\Words.txt");
            var db = string.Join("", txt.Split('\r')).Split('\n');
            Db = db.ToList();
        }


        public void SetValue(string value)
        {
            Db.Add(value);
        }

        public List<string> GetValue(string value)
        {
            if (Db.Contains(value))
            {
                return Db.FindAll(d => d.StartsWith(value));
            }
            else
            {
                Db.Add(value);
                return Db.FindAll(d => d.StartsWith(value));
            }
        }


    }


    class ProxyDataBase : IBase
    {


        public DataBase Db { get; set; }
        public List<string> AllWords { get; set; }


        public ProxyDataBase()
        {
            Db = new DataBase();
            var txt = File.ReadAllText(@"-/../../../Database\Words.txt");
            var aw = string.Join("", txt.Split('\r')).Split('\n');
            AllWords = aw.ToList();

        }

        public List<string> GetValue(string value)
        {
            return AllWords.FindAll(aw => aw.StartsWith(value));
        }



        public void SetValue(string value)
        {
            var word = Db.Db.Find(d => d == value);
            if (word != null)
            {
                if (AllWords.Contains(word) == false)
                {
                    AllWords.Add(word);
                }
                else
                {
                    MessageBox.Show($"{value} databazada movcuddur");
                }
            }
            else
            {
                MessageBox.Show($"{value} databazaya elave olundu");
                Db.Db.Add(value);
            }
        }
    }



    public partial class MainWindow : Window
    {

        DataBase dataBase = new DataBase();

        ProxyDataBase proxy = new ProxyDataBase();

        public List<string> ListboxAddedWords { get; set; }
        public List<string> ListboxWords { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            ListboxAddedWords = new List<string>();
            ListboxWords = new List<string>();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            ListboxAddedWords.Add(txtb.Text);

            TextB.ItemsSource = ListboxAddedWords;

            proxy.SetValue(txtb.Text);
        }

        private void txtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtb.Text.Length > 0)
            {
                ListboxWords = proxy.GetValue(txtb.Text);
                ListB.ItemsSource = ListboxWords;
            }
            else
            {
                ListB.ItemsSource = null;
            }
        }
    }
}
