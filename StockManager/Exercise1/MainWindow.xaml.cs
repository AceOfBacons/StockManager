using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Microsoft.VisualBasic;

//Pablo Saldarriaga 
//ID: 30109276
//Centennial college winter 2021
namespace Exercise1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Stock> stocks = new List<Stock>();
        public MainWindow()
        {
            InitializeComponent();
            stockGrid.ItemsSource = readStocks(); //Initialize grid to the populated stock list
        }

        private static List<Stock> readStocks()
        {
            //using io reader in the source file
            using(var reader = new System.IO.StreamReader(@"D:\Centennial semester 4\Programming 3\Lab#3\Lab#3\stockData.csv"))
            {
                //iterate until end of file
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var invalidChars = new[] { "\"", "(", ")" }; //set of characters to ignore

                    //if elements of the list contain the invalid characters, then do nothing
                    if(invalidChars.Any(values[2].Contains) || invalidChars.Any(values[3].Contains) || invalidChars.Any(values[4].Contains) || invalidChars.Any(values[5].Contains))
                    {

                    }
                    else
                    {
                        //else add a new stock
                        stocks.Add(new Stock
                        {
                            symbol = values[0],
                            date = DateTime.Parse(values[1]),
                            //remove the currency character for doubles
                            open = double.Parse(values[2].Remove(0, 1)), 
                            high = double.Parse(values[3].Remove(0, 1)),
                            low = double.Parse(values[4].Remove(0, 1)),
                            close = double.Parse(values[5].Remove(0, 1))
                        });
                    }

                }
            }
            return stocks;
        }

        //Factorial calculation logic
        public void calculateFactorial()
        {
            long numberToFactorial = 1;
            long result = 1;

            //updating ui elements from another thread 
            this.Dispatcher.Invoke(() =>
            {
                numberToFactorial = long.Parse(factorialTxtBox.Text);

            });
           
            //iterating through number and reducing it by one each time
            for (long i = numberToFactorial; i >= 1; i--)
            {
                result *= i; //keep on multiplicating
            }

            Thread.Sleep(5000); // wait for 5 seconds

            this.Dispatcher.Invoke(() =>
            {
                resultLabel.Content = "Result is: " + result.ToString();
            });
            
        }

        private async void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(calculateFactorial); // start task 
            task.Start();
            resultLabel.Content = "Calculating...";
            await task;

        }


        //Searching logic
        static List<Stock> stocksFiltered = new List<Stock>();
        private void Button_Click(object sender, RoutedEventArgs e) //search button onClick
        {
            stocksFiltered.Clear(); //Empty out the grid

            if (searchTxtBox.Text.Equals(""))
            {
                //nothing on the text box, so display all stocks
                stockGrid.ItemsSource = readStocks();
                stocksFiltered.AddRange(stocks);
            }
            else
            {
                foreach (Stock stock in stocks)
                {
                    //if text value matches the stock symbol then add it to the grid
                    if (stock.symbol.Contains(searchTxtBox.Text))
                    {
                        stocksFiltered.Add(stock);
                    }
                }
            }

            //set new grid with filtered stocks
            stockGrid.ItemsSource = stocksFiltered.OrderByDescending(s => s.date).ToList();
        }
        


        private void searchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
