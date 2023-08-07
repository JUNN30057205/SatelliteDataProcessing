using Galileo6;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Galileo6;

namespace SatelliteDataProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
            FillComboBoxes();
        }

        #region Global Methods 
        //4.1 Create two data structure using the LinkedList<T>class.
        //The two LinkedLists of type double are to be declared as global within the "public partial class".
        private LinkedList<double> SensorAList = new LinkedList<double>();
        private LinkedList<double> SensorBList = new LinkedList<double>();

        const int dataSize = 400;
       
        //private void IntConsoleTraceListener()
        //{
        //    ConsoleTraceListener listener = new ConsoleTraceListener();
        //    Trace.Listeners.Add(listener);
        //}             


        //4.2 Create a method called “LoadData” which will populate both LinkedLists.
        //Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList;
        //the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
        //The LinkedList size will be hardcoded inside the method and must be equal to 400.
        //The input parameters are empty, and the return type is void.
        private void LoadData()
        {
            
            ReadData readData = new ReadData();            
            SensorAList.Clear();
            SensorBList.Clear();                       

            for (int i =0; i < dataSize; i++)
            {
                SensorAList.AddLast(readData.SensorA(double.Parse(ComboBoxMu.SelectedValue.ToString()), double.Parse(ComboBoxSigma.SelectedValue.ToString())));
                SensorBList.AddLast(readData.SensorB(double.Parse(ComboBoxMu.SelectedValue.ToString()), double.Parse(ComboBoxSigma.SelectedValue.ToString())));
            }
        }                

        //4.3 Create a custom method called "ShowAllSensorData" which will display both LinkedLists in a ListView.
        // Add column titles "SensorA" and "SensorB"  to the ListView. 
        // The input parameters are empty, and the return type is void
        private void ShowAllSensorData()
        {            
            ListView.Items.Clear();
            for(int i = 0; i < dataSize; i++)
            {
                ListView.Items.Add(new
                {
                    SensorA = SensorAList.ElementAt(i).ToString(), //Column SensorA 
                    SensorB = SensorBList.ElementAt(i).ToString()  //Column SensorB
                });
                
            }
            
        }

        //4.4 Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
        // The input parameters are empty, and the return type is void
        private void ButtonClick_LoadData(object sender, RoutedEventArgs e)
        {
            LoadData();            
            ShowAllSensorData();
        }

        #endregion
        #region Utility Methods
        //4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
        //  The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
        
        private int NumberOfNodes(LinkedList<double> linkedList)
        {
            return linkedList.Count;
        }

        //4.6	Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
        //  The method signature will have two input parameters; a LinkedList, and the ListBox name.
        //  The calling code argument is the linkedlist name and the listbox name.
        private void DisplayListboxData(LinkedList<double> linkedList, ListBox listBox)
        {
            listBox.Items.Clear();
            foreach (double data in linkedList)
            {
                listBox.Items.Add(data);
            }

        }

        #endregion
        #region Sort and Search Methods
        #region Selection and Insertion Sort
        //4.7	Create a method called “SelectionSort” which has a single input parameter of type LinkedList,
        //  while the calling code argument is the linkedlist name. The method code must follow the pseudo code supplied below in the Appendix.
        //  The return type is Boolean.
        private bool SelectionSort(LinkedList<double> linkedList)
        {
            int min;
            int max = NumberOfNodes(linkedList);

            for (int i = 0; i < max - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                {
                    if (linkedList.ElementAt(j) < (linkedList.ElementAt(min)))
                    {
                        min = j;
                    }
                }
                                
                LinkedListNode<double> currentMin = linkedList.Find(linkedList.ElementAt(min));
                LinkedListNode<double> currentI = linkedList.Find(linkedList.ElementAt(i));
                //swap;
                double temp = currentMin.Value;
                currentMin.Next.Value = currentI.Value;
                currentI.Value = temp;               
                
            }
            return true;            
        }

        //4.8	Create a method called “InsertionSort” which has a single parameter of type LinkedList,
        //  while the calling code argument is the linkedlist name. The method code must follow the pseudo code supplied below in the Appendix.
        //  The return type is Boolean.
        private bool InsertionSort(LinkedList<double> linkedList)
        {
            int max = NumberOfNodes(linkedList);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (linkedList.ElementAt(j - 1) > (linkedList.ElementAt(j)))
                    {
                        LinkedListNode<double> current = linkedList.Find(linkedList.ElementAt(j));
                        double temp = current.Previous.Value;
                        current.Previous.Value = current.Value;
                        current.Value = temp; 
                    }
                }
            }
            return false;
        }
        #endregion
        #region Bianry Search (Iterative and Recursive)
        //4.9	Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //  This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //  The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        //  The method code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchInterative(LinkedList<double> linkedList, int searchValue, int min, int max)
        {

            while (min  <= max -1)
            {
                int mid = min + max / 2;

                if (searchValue == linkedList.ElementAt(mid))
                {
                    return ++mid;
                }
                else if (searchValue < linkedList.ElementAt(mid))
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            return min;
        }

        //4.10	Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //  This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //  The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        //  The method code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchRecursive(LinkedList<double> linkedList, int searchValue, int min, int max)
        {            
            if (min <= max - 1)
            {
                int mid = min + max / 2;

                if (searchValue == linkedList.ElementAt(mid))
                {
                    return mid;
                }
                else if (searchValue < linkedList.ElementAt(mid))
                {
                    return BinarySearchRecursive(linkedList, searchValue, min, mid - 1);
                }
                else
                {
                    return BinarySearchRecursive(linkedList, searchValue, mid + 1 , max);
                }               
            }
            return min;
        }
        
        #endregion
        #endregion

        #region UI Button Methods
        #region Button Search
        //4.11	Create four button click methods that will search the LinkedList for an integer value entered into a textbox on the form.
        //The four methods are:
        //1.	Method for Sensor A and Binary Search Iterative
        //2.	Method for Sensor A and Binary Search Recursive
        //3.	Method for Sensor B and Binary Search Iterative
        //4.	Method for Sensor B and Binary Search Recursive
        //The search code must check to ensure the data is sorted, then start a stopwatch before calling the search method.
        //Once the search is complete the stopwatch will stop, and the number of ticks will be displayed in a read only textbox.
        //Finally, the code/method will call the “DisplayListboxData” method and highlight the search target number and two values on each side.
        private void ButtonClickA_BinSearchIterative(object sender, RoutedEventArgs e)
        {
            //if()
            TextBoxSsA_SearchIte.Clear();
            InsertionSort(SensorAList);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //BinarySearchInterative(SensorAList, searchValue, min, max);
            sw.Stop();
            long numTickes = sw.ElapsedTicks;
            TextBoxSsA_SearchIte.Text = sw.ElapsedTicks.ToString() + " ticks";
            DisplayListboxData(SensorAList, ListBoxSensorA);

        }
        private void ButtonClickB_BinSearchIterative(object sender, RoutedEventArgs e)
        {
            TextBoxSsB_SearchIte.Clear();
            InsertionSort(SensorBList);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //BinarySearchInterative();
            sw.Stop();
            long numTickes = sw.ElapsedTicks;
            TextBoxSsB_SearchIte.Text = sw.ElapsedTicks.ToString() + " ticks";
            DisplayListboxData(SensorBList, ListBoxSensorB);
        }

        private void ButtonClickA_BinSearchRecursive(object sender, RoutedEventArgs e)
        {
            TextBoxSsA_SearchRec.Clear();
            InsertionSort(SensorAList);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //BinarySearchRecursive();
            sw.Stop();
            long numTickes = sw.ElapsedTicks;
            TextBoxSsA_SearchRec.Text = sw.ElapsedTicks.ToString() + " ticks";
            DisplayListboxData(SensorAList, ListBoxSensorA);
        }

        private void ButtonClickB_BinSearchRecursive(object sender, RoutedEventArgs e)
        {
            TextBoxSsB_SearchRec.Clear();
            InsertionSort(SensorBList);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //BinarySearchRecursive();
            sw.Stop();
            long numTickes = sw.ElapsedTicks;
            TextBoxSsB_SearchRec.Text = sw.ElapsedTicks.ToString() + " ticks";
            DisplayListboxData(SensorBList, ListBoxSensorB);
        }
        #endregion
        private void Highlight (int found, TextBox textBox)
        {

        }
        #region Button Sort
        //4.12	4.12	Create four button click methods that will sort the LinkedList using the Selection and Insertion methods.
        //The four methods are:
        //1.	Method for Sensor A and Selection Sort
        //2.	Method for Sensor A and Insertion Sort
        //3.	Method for Sensor B and Selection Sort
        //4.	Method for Sensor B and Insertion Sort
        //The button method must start a stopwatch before calling the sort method.
        //Once the sort is complete the stopwatch will stop, and the number of milliseconds will be displayed in a read only textbox.
        //Finally, the code/method will call the “ShowAllSensorData” method and “DisplayListboxData” for the appropriate sensor.
        private void ButtonClickA_SelectionSort(object sender, RoutedEventArgs e)
        {
            TextBox_SelectSsA.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SelectionSort(SensorAList);
            sw.Stop();
            long ElapseMilliseconds = sw.ElapsedMilliseconds;
            TextBox_SelectSsA.Text = sw.ElapsedMilliseconds.ToString() + " millisec";
            DisplayListboxData(SensorAList, ListBoxSensorA);            
        }

        private void ButtonClickB_SelectionSort(object sender, RoutedEventArgs e)
        {
            TextBox_SelectSsB.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SelectionSort(SensorBList);
            sw.Stop();
            long ElapseMilliseconds = sw.ElapsedMilliseconds;
            TextBox_SelectSsB.Text = sw.ElapsedMilliseconds.ToString() + " millisec";
            DisplayListboxData(SensorBList, ListBoxSensorB);
        }

        private void ButtonClickA_InsertionSort(object sender, RoutedEventArgs e)
        {
            TextBox_InsertSsA.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InsertionSort(SensorAList);
            sw.Stop();
            long ElapseMilliseconds = sw.ElapsedMilliseconds;
            TextBox_InsertSsA.Text = sw.ElapsedMilliseconds.ToString() + " millisec";
            DisplayListboxData(SensorAList, ListBoxSensorA);
        }        

        private void ButtonClickB_InsertionSort(object sender, RoutedEventArgs e)
        {
            TextBox_InsertSsB.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InsertionSort(SensorBList);
            sw.Stop();
            long EllapseMilliseconds = sw.ElapsedMilliseconds;
            TextBox_InsertSsB.Text = sw.ElapsedMilliseconds.ToString() + " millisec";
            DisplayListboxData(SensorBList, ListBoxSensorB);
        }
        #endregion

        //4.13	Add two numeric input controls for Sigma and Mu. The value for Sigma must be limited with a minimum of 10 and a maximum of 20.
        //  Set the default value to 10. The value for Mu must be limited with a minimum of 35 and a maximum of 75. Set the default value to 50.
        private void FillComboBoxes()
        {
            //Sigma combobox
            //int default_sigma = 10;
            //int min_sigma = 10;
            //int max_sigma = 20;

            for (int i = 10; i <= 20; i++)
            {
                ComboBoxSigma.Items.Add(i);
            }
            ComboBoxSigma.SelectedIndex = 0; //default

            //Mu combobox
            for (int i = 35; i <= 75; i++)
            {
                ComboBoxMu.Items.Add(i);
            }
            ComboBoxMu.SelectedIndex = 15; //default

        }

        //4.14	Add two textboxes for the search value; one for each sensor, ensure only numeric integer values can be entered.
        private void TextBoxA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new(@"^\d+$");
            
        }

        private void TextBoxB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        #endregion

       
    }
    
}
