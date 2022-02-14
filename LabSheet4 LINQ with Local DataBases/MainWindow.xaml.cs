/*
 * Program: Using LINQ with local database
 * Date: 14/02/2022
 * Author : Rudgery Lopes
 * 
 * 
 * 
 */

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
using System.IO;

namespace LabSheet4_LINQ_with_Local_DataBases
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //declaring the local database 
        NORTHWNDEntities db = new NORTHWNDEntities();



        public MainWindow()
        {
            InitializeComponent();
        }


        //Exercise 1
        private void btnQueryEx1_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers
                        select c.CompanyName;

            
            lbxCustomersEx1.ItemsSource = query.ToList();

        }


        //Exercise 2
        private void btnQueryEx2_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers
                        select c;

            Ex2DataGrid.ItemsSource = query.ToList();
        }

        //Exercise 3 - Code to retrieve information
        private void btnQueryEx3_Click(object sender, RoutedEventArgs e)
        {
            var query = from o in db.Orders
                        where o.Customer.City.Equals("London")
                        || o.Customer.City.Equals("Paris")
                        || o.Customer.Country.Equals("USA")
                        orderby o.Customer.CompanyName
                        select new
                        {
                            CustomerName = o.Customer.CompanyName,
                            City = o.Customer.City,
                            Address = o.ShipAddress
                            
                            
                        };

            Ex3DataGrid.ItemsSource = query.ToList().Distinct();
        }


        //Exercise 4 - Code to display product information
        private void btnQueryEx4_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        where p.Category.CategoryName.Equals("Beverages")
                        orderby p.ProductID descending
                        select new
                        {
                            p.ProductID,
                            p.ProductName,
                            p.Category.CategoryName,
                            p.UnitPrice
                        };

            Ex4DataGrid.ItemsSource = query.ToList();
        }


        // Exercise 5: Code to insert a product
        private void btnQueryEx5_Click(object sender, RoutedEventArgs e)
        {
            Product p = new Product()
            {
                ProductName = "Kickapoo Jungle Joy Juice",
                UnitPrice = 12.49m,
                CategoryID = 1
            };

            db.Products.Add(p);
            db.SaveChanges();

            ShowProducts(Ex5DataGrid);
            
        }

        private void ShowProducts(DataGrid currentGrid)
        {
            var query = from p in db.Products
                        where p.Category.CategoryName.Equals("Beverages")
                        orderby p.ProductID descending
                        select new
                        {
                            p.ProductID,
                            p.ProductName,
                            p.Category.CategoryName,
                            p.UnitPrice

                        };
            currentGrid.ItemsSource = query.ToList();
        }

        //Exercise 6: Update product information
        private void btnQueryEx6_Click(object sender, RoutedEventArgs e)
        {
            Product p1 = (db.Products
                .Where(p => p.ProductName.StartsWith("Kick"))
                .Select(p => p)).First();

            p1.UnitPrice = 100m;

                db.SaveChanges();
            ShowProducts(Ex6DataGrid);

        }


        //Exercise 7  : Multiple update
        private void btnQueryEx7_Click(object sender, RoutedEventArgs e)
        {
            var products = from p in db.Products
                           where p.ProductName.StartsWith("Kick")
                           select p;

            foreach (var item in products)
            {
                item.UnitPrice = 100m;
            }

            db.SaveChanges();
            ShowProducts(Ex7DataGrid);

        }


        //exercise  8 - delete
        private void btnQueryEx8_Click(object sender, RoutedEventArgs e)
        {
            var products = from p in db.Products
                           where p.ProductName.StartsWith("Kick")
                           select p;

            db.Products.RemoveRange(products);
            db.SaveChanges();
            ShowProducts(Ex8DataGrid);

        }

        //Exercise 9 : Using a Stored Procedure
        private void btnQueryEx9_Click(object sender, RoutedEventArgs e)
        {
            var query = db.Customers_By_City("London");
            Ex9DataGrid.ItemsSource = query.ToList();
        }
    }
}
