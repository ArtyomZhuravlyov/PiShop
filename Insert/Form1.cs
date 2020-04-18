using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Insert
{
    public partial class Form1 : Form
    {
        string connectionString;

        public Form1()
        {
            InitializeComponent();
            // получаем строку подключения
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            string[] files = Directory.GetFiles(path);
            int ind = 0;
            for (int i = 0; i < files.Length; i++)
            {
                //поиск индекса последнего слеша
                ind = files[i].LastIndexOf('\\'); //{textStart}
                string name = files[i].Substring(ind + 1);
                String NewName = "Short" + name;
                string NewPath = files[i].Replace(name, NewName);
                //переименование
                File.Move(files[i], NewPath);
            }

  

            MessageBox.Show("Готово!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            string[] files = Directory.GetFiles(path);
            int ind = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                for (int i = 0; i < files.Length; i++)
                {
                    //поиск индекса последнего слеша
                    ind = files[i].LastIndexOf('\\'); //{textStart}
                    string name = files[i].Substring(ind + 1);
                    
                    string Name = $"'Пижама { name.Replace(".jpg", "").Replace(".png", "").Replace("Short", "").ToUpper() }'";
                    string Address = $"'Обычный топ/{name}'";
                    string Address2 = $"'Обычный топ/{name}2'";
                    string category = "'обычный топ'";
                    int Price = 1590;
                    int WithOutPrice = 1590;
                    int Sale = 0;
                    string sqlExpression = $"INSERT INTO Products (Name, Description, Address,Price, Category, ImageData, ImageMimeType, PriceWithoutSales, Weight, Favourite ,Size, Image2Address ,Sale ) VALUES ({Name}, null, {Address}, {Price}, {category}, null, null, {WithOutPrice}, null, 0 ,null, {Address2} , {Sale})";

                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();


                    //using (SqlCommand command = new SqlCommand("DELETE FROM " + Products, connection)) //очищаем таблицу
                    //{
                    //    command.ExecuteNonQuery();
                    //}

                    //INSERT INTO Product VALUES('${}', 1157, 'PC');
                }
            }
                //String NewName = "Short" + name;
                //string NewPath = files[i].Replace(name, NewName);
                //переименование
                //  File.Move(files[i], NewPath);
            

            //смотрим есть ли файлы в текущей папке, если нет, то спускаемся в папки ниже
            //if ((Directory.GetFiles(_NameFolder)).Count() > 0)
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            string[] files = Directory.GetFiles(path);
            int ind = 0;
            for (int i = 0; i < files.Length; i++)
            {
                //поиск индекса последнего слеша
                ind = files[i].LastIndexOf('\\'); //{textStart}
                string name = files[i].Substring(ind + 1);
                String NewName = "S" + name; //name.Substring(6);
                string NewPath = files[i].Replace(name, NewName);
                //переименование
                File.Move(files[i], NewPath);
            }



            MessageBox.Show("Готово!");
        }
    }
}
