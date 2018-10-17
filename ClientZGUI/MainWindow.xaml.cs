using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private  static Random rand = new Random();

        private  ClientZGUI.GeneticService.Point[] points;
        
    
        private  int startPoint = 2;
        private  List<Host> hosts = new List<Host>();


        private  int countOfPoints;
        private  int countOfGenerations;
        private  int lengthOfPopulation;
        private  List<string> addresesOfHosts = new List<string>();


        private  int partsToSend;
        private  int lengthOfPart;

        private  bool runing = false;

        private List<int> bestResults = new List<int>();

        private int lastCountOfPoints = 0;
        private List<StreamWriter> logFiles = new List<StreamWriter>();
       

        private void addHostButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = textBox.Text;
            listBox.Items.Add(ip);
            textBox.Text = "";
        }
     

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            
            sayIsProccesingAndChangeOpacityOfStartButton();
            await Task.Run(() => { Thread.Sleep(10); });

            stopwatch.Start();
            go();
            stopwatch.Stop();

            sayIsEndOfProcesing();
            timeOfGetingResponseLabel.Content = "Czas oczekiwania: " + stopwatch.ElapsedMilliseconds + " ms";
            findAndShowBestResult();
            bestResults.Clear();
        }

        private void sayIsProccesingAndChangeOpacityOfStartButton()
        {
            processingInfoLabel.Content = "Przetwarzam...";
            startButton.Opacity = 0.5f;
        }

        private void findAndShowBestResult()
        {
            int best = bestResults[0];
            for (int i = 1; i < bestResults.Count; i++)
            {
                if (best > bestResults[i])
                {               
                    best = bestResults[i];
                }
            }

            bestResultLabel.Content = "Najkrótsza ścieżka: " + best;
        }


        private void go()
        {
            if (runing)
            {
                cleaerLogs();
            }
            getDataFromUser();
            putAddressesFromListBoxIntoAddresesList();

            createLogsFiles();
            makeTabsForEachHost();
         
            if ( ( runing == false ) || (lastCountOfPoints != countOfPoints) )
            {                
                initializeAndRandPoints();
               
                runing = true;
            }

            
            firstSheduleAndStartConnections();
            sheduler();
           
            resultTabControl.Visibility = Visibility.Visible;
            hosts.Clear();
            closeAllLogFiles();
            logFiles.Clear();
        }

      
     

        private void cleaerLogs()
        {
            for (int i = 0; i < resultTabControl.Items.Count; i++)
            {
                TabItem tab = (TabItem)resultTabControl.Items[i];
                StackPanel stp = (StackPanel)((ScrollViewer)tab.Content).Content;

                stp.Children.Clear();
                              
                RichTextBox log = new RichTextBox();
                log.Document.LineHeight = 1d;
                log.IsReadOnly = true;
                                
                stp.Children.Add(log);
            }
        }
       


        private void putAddressesFromListBoxIntoAddresesList()
        {
            if (addresesOfHosts.Count > 0)
            {
                addresesOfHosts.Clear();
            }
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                string address = listBox.Items.GetItemAt(i).ToString();
                addresesOfHosts.Add(address);               
            }
        }

        private void createLogsFiles()
        {            
            for (int idHost = 0; idHost < addresesOfHosts.Count; idHost++)
            {                    
                logFiles.Add( File.CreateText(addresesOfHosts[idHost] + ".txt") );
            }
        }

        private void closeAllLogFiles()
        {
            for (int idFile = 0; idFile < logFiles.Count; idFile++)
            {
                logFiles[idFile].Close();
            }
        }

        private void makeTabsForEachHost()
        {
            if (resultTabControl.Items.Count > 0)
                resultTabControl.Items.Clear();

            for (int idHost = 0; idHost < addresesOfHosts.Count; idHost++)
            {
                StackPanel stackPanel = new StackPanel();
                TabItem tab = new TabItem();

                ScrollViewer scroll = new ScrollViewer();
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

                RichTextBox log = new RichTextBox();
                log.Document.LineHeight = 1d;
                log.IsReadOnly = true;
                stackPanel.Children.Add(log);
                scroll.Content = stackPanel;
                stackPanel.Background = Brushes.White;


                tab.Header = addresesOfHosts[idHost];
                tab.Content = scroll;
                resultTabControl.Items.Add(tab);
            }
        }
      
        private void getDataFromUser()
        {
            lastCountOfPoints = countOfPoints;
            countOfPoints = int.Parse(countOfPointsTextBox.Text);
            
            countOfGenerations = int.Parse(numberOfGenerationsTextBox.Text);
            lengthOfPopulation = int.Parse(populationLengthTextBox.Text);

            partsToSend = lengthOfPopulation;
            lengthOfPart = int.Parse(packageLenghtTextBox.Text);
        }

        private void initializeAndRandPoints()
        {
            points = new ClientZGUI.GeneticService.Point[countOfPoints];
            for (int i = 0; i < points.Length; i++)
            {
                int x = rand.Next(0, 150);
                int y = rand.Next(0, 150);

                points[i] = new ClientZGUI.GeneticService.Point();
                points[i].x = x;
                points[i].y = y;
            }
        }

        private void firstSheduleAndStartConnections()
        {            
            for (int i = 0; i < addresesOfHosts.Count; i++)
            {               
                if (partsToSend < lengthOfPart)
                {
                    lengthOfPart = partsToSend;
                    if (lengthOfPart < 4)
                    {
                        lengthOfPart = 4;
                    }
                    partsToSend = 0;
                }

                lengthOfPopulation = lengthOfPart;              
                makeNewConnection(addresesOfHosts[i]);
                partsToSend -= lengthOfPart;
                if (partsToSend < 0) break;
            }
        }


        private void makeNewConnection(string ipOfHost)
        {
            Host host = new Host(ipOfHost, points, startPoint, lengthOfPopulation, countOfGenerations);

            host.start();

            hosts.Add(host);
        }

        private void sheduler()
        {
            int counter = 0;
            while (counter != addresesOfHosts.Count)
            {
                for (int currentHost = 0; currentHost < addresesOfHosts.Count; currentHost++)
                {
                    if (hosts[currentHost].finished)
                    {
                        if (hosts[currentHost].readed == false)
                        {
                            counter++;
                            showResultOnWindowAndWriteToLogFile(currentHost);
                            hosts[currentHost].readed = true;

                            if (tryToSendNextPartOfData(currentHost))
                                counter--;
                        }
                    }
                }
            }
        }


        private bool tryToSendNextPartOfData(int host)
        {
            if (partsToSend > 0)
            {
                if ((partsToSend - lengthOfPart) >= 0)
                {
                    hosts[host].lengthOfPopulation = lengthOfPart;
                    hosts[host].startWithPopulation();
                    partsToSend -= lengthOfPart;
                    return true;
                }
                else
                {
                    if (partsToSend < lengthOfPart)
                    {
                        if (partsToSend < 4)
                        {
                            partsToSend = 4;
                        }

                        hosts[host].lengthOfPopulation = partsToSend;
                        hosts[host].startWithPopulation();
                        partsToSend = 0;
                        return true;
                    }
                }
            }
            return false;
        }


        private void showResultOnWindowAndWriteToLogFile(int idHost)
        {           
            string msg;
            string msgForFile = "";

            msg = "L.p.: " + hosts[idHost].counter + "\n";
            msgForFile = hosts[idHost].counter.ToString();

            msg += "Czas oczekiwania klienta: " + hosts[idHost].singleTimeOfWork + " ms\n";
            msgForFile += "\t" + hosts[idHost].singleTimeOfWork;

            msg += "Czas obliczen: " + hosts[idHost].result.timeOfCounting + " ms\n";
            msgForFile += "\t" + hosts[idHost].result.timeOfCounting;

            msg += "Czas podrozy: " + (hosts[idHost].singleTimeOfWork - hosts[idHost].result.timeOfCounting) + " ms\n";
            msgForFile += "\t" + (hosts[idHost].singleTimeOfWork - hosts[idHost].result.timeOfCounting);

            msg += "Całkowity czas: " + hosts[idHost].allTimeOfWork + " ms";
            msgForFile += "\t" + hosts[idHost].allTimeOfWork;

            msg += "\nNajlepszy wynik: " + hosts[idHost].result.ways[0];
            msgForFile += "\t" + hosts[idHost].result.ways[0];

            msg += "\nWielkosc paczki: " + hosts[idHost].lengthOfPopulation + "\n\n";
            msgForFile += "\t" + hosts[idHost].lengthOfPopulation + "\n";

           

            logFiles[idHost].WriteLine(msgForFile);
            

            TabItem tab = (TabItem)resultTabControl.Items[idHost];
            tab.Header = addresesOfHosts[idHost] + " (" + hosts[idHost].counter + ")"; 
            StackPanel stp = (StackPanel)((ScrollViewer)tab.Content).Content;
                      
           
            RichTextBox log = (RichTextBox) stp.Children[0];
            log.Background = Brushes.White;            
            log.AppendText(msg);
            
          
            bestResults.Add(hosts[idHost].result.ways[0]);   
        }


        private void sayIsEndOfProcesing()
        {
            processingInfoLabel.Content = "Policzone!";
            startButton.Opacity = 1f;
        }    
    }
}
