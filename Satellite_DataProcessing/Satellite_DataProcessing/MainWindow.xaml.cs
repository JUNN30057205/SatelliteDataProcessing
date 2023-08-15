using Galileo6;
using Microsoft.Windows.Themes;
using System;
using System.Collections;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
//using Galileo6;
//Name: Jun Sumida
//ID :30057205
//C# Assessment One

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
            TextBlock statusMS = StatusMS;
            statusMS.Text = "Status Message";

        }
              
        #region Global Methods 
        //4.1 Create two data structure using the LinkedList<T>class.
        //The two LinkedLists of type double are to be declared as global within the "public partial class".
        private LinkedList<double> SensorAList = new LinkedList<double>();
        private LinkedList<double> SensorBList = new LinkedList<double>();
        
        const int dataSize = 400;

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

            for (int i = 0; i < dataSize; i++)
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
            for (int i = 0; i < dataSize; i++)
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

            StatusMS.Text = "Sensor Data successfully Loaded.";

            //Button Control
            Button_InsertionSortA.IsEnabled = true;
            Button_InsertionSortB.IsEnabled = true;
            Button_SelectionSortA.IsEnabled = true;
            Button_SelectionSortB.IsEnabled = true;

            //TextBoxes & ListBox Clear
            TextBox_SensorA.Clear();
            TextBox_SensorB.Clear();
            TextBox_SelectSsA.Clear();
            TextBox_SelectSsB.Clear();
            TextBox_InsertSsA.Clear();
            TextBox_InsertSsB.Clear();
            TextBox_SelectSsA.Clear();
            TextBox_SelectSsB.Clear();
            TextBoxSsA_SearchIte.Clear();
            TextBoxSsB_SearchIte.Clear();
            TextBoxSsA_SearchRec.Clear();
            TextBoxSsB_SearchRec.Clear();
            ListBoxSensorA.Items.Clear();
            ListBoxSensorB.Items.Clear();          
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
            /*Selection Sort
                integer min => 0
                integer max => numberOfNodes(list)
                for ( i = 0 to max - 1 )
                min => i
                for ( j = i + 1 to max )
                if (list element(j) < list element(min))
                min => j
                END for
                // Supplied C# code
                LinkedListNode<double> currentMin = list.Find(list.ElementAt(min))
                LinkedListNode<double> currentI = list.Find(list.ElementAt(i))
                // End of supplied C# code
                var temp = currentMin.Value
                currentMin.Value = currentI.Value
                currentI.Value = temp
                END for
             */
            int min = 0;
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
                currentMin.Value = currentI.Value;
                currentI.Value = temp;                
                
            }
            return true;
        }

        //4.8	Create a method called “InsertionSort” which has a single parameter of type LinkedList,
        //  while the calling code argument is the linkedlist name. The method code must follow the pseudo code supplied below in the Appendix.
        //  The return type is Boolean.
        private bool InsertionSort(LinkedList<double> linkedList)
        {
            /*Insertion Sort
                integer max = numberOfNodes(list)
                for ( i = 0 to max – 1 )
                for ( j = i + 1 to j > 0, j-- )
                if (list element(j - 1) > list element(j))
                // Supplied C# code
                LinkedListNode<double> current = list.Find(list.ElementAt(j))
                // End of supplied C# code
                // Add Swap code here by swapping
                // previous value with current value.
                END if
                END for
                END for
             */
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
            return true;
        }
        #endregion
        #region Bianry Search (Iterative and Recursive)
        //4.9	Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        //  This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        //  The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        //  The method code must follow the pseudo code supplied below in the Appendix.
        private int BinarySearchIterative(LinkedList<double> linkedList, int searchValue, int min, int max)
        {
            /*Binary Search Iterative  
                while (minimum <= maximum - 1)
                integer middle = minimum + maximum / 2
                if (search value = list element(middle))
                return ++middle
                else if (search value < list element(middle))
                maximum => middle - 1
                else
                minimum => middle + 1
                END while
                return minimum 
             */                

            while (min <= max - 1)
            {
                int mid = (min + max) / 2;

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
            /*Binary Search Recursive
                if (minimum <= maximum - 1)
                integer middle = minimum + maximum / 2
                if (search value = list element(middle))
                return middle
                else if (search value < list element(middle))
                return binarySearchRecursive(list, search value, minimum, middle - 1)
                else
                return binarySearchRecursive(list, search value, middle + 1, maximum)
                END if
                return minimum
            */
            if (min <= max - 1)
            {
                int mid = (min + max) / 2;

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
                    return BinarySearchRecursive(linkedList, searchValue, mid + 1, max);
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
            TextBoxSsA_SearchIte.Clear();
           
            if (!string.IsNullOrEmpty(TextBox_SensorA.Text))
            {                
                if (SearchRange(TextBox_SensorA, SensorAList) && InsertionSort(SensorAList) || SelectionSort(SensorAList))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int found = BinarySearchIterative(SensorAList, int.Parse(TextBox_SensorA.Text), 0, NumberOfNodes(SensorAList));
                    sw.Stop();
                    long numTickes = sw.ElapsedTicks;
                    TextBoxSsA_SearchIte.Text = numTickes.ToString() + " ticks";
                    DisplayListboxData(SensorAList, ListBoxSensorA);
                    HighlightItems(found, ListBoxSensorA);
                    Button_BinSearchIterativeA.IsEnabled = false;
                }
                else
                {
                    StatusMS.Text = "Search Value is out of range or sensorAList is Unsorted";
                }
            }
            else
            {                
                StatusMS.Text = "Search Input is Empty, Enter the Value"; 
                TextBox_SensorA.Focus();
            }     
        }

        private void ButtonClickB_BinSearchIterative(object sender, RoutedEventArgs e)
        {
            TextBoxSsB_SearchIte.Clear();
            if (!string.IsNullOrEmpty (TextBox_SensorB.Text))
            {
                if (SearchRange(TextBox_SensorB, SensorBList) && SelectionSort(SensorBList) || InsertionSort(SensorBList))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int found = BinarySearchIterative(SensorBList, int.Parse(TextBox_SensorB.Text), 0, NumberOfNodes(SensorBList));
                    sw.Stop();
                    long numTickes = sw.ElapsedTicks;
                    TextBoxSsB_SearchIte.Text = numTickes.ToString() + " ticks";
                    DisplayListboxData(SensorBList, ListBoxSensorB);
                    HighlightItems(found, ListBoxSensorB);
                    Button_BinSearchIterativeB.IsEnabled = false;
                    
                }
                else
                {
                    StatusMS.Text = "Search Value is out of range or sensorBList is Unsorted";
                }
            }
            else
            {
                StatusMS.Text = "Search Input is Empty, Enter the Value";
                TextBox_SensorB.Focus();
            }
        }

        private void ButtonClickA_BinSearchRecursive(object sender, RoutedEventArgs e)
        {
            TextBoxSsA_SearchRec.Clear();

            if (!string.IsNullOrEmpty(TextBox_SensorA.Text))
            {
                if (SearchRange(TextBox_SensorA, SensorAList) && SelectionSort(SensorAList) || InsertionSort(SensorAList))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int found = BinarySearchRecursive(SensorAList, int.Parse(TextBox_SensorA.Text), 0, NumberOfNodes(SensorAList));
                    sw.Stop();
                    long numTickes = sw.ElapsedTicks;
                    TextBoxSsA_SearchRec.Text = numTickes.ToString() + " ticks";
                    DisplayListboxData(SensorAList, ListBoxSensorA);
                    HighlightItems(found, ListBoxSensorA);
                    Button_BinSearchRecursiveA.IsEnabled = false;
                }
                else
                {
                    StatusMS.Text = "Search Value is out of range or sensorAList is Unsorted";
                }
            }
            else
            {
                StatusMS.Text = "Search Input is Empty, Enter the Value";
                TextBox_SensorA.Focus();
            }            
        }

        private void ButtonClickB_BinSearchRecursive(object sender, RoutedEventArgs e)
        {
            TextBoxSsB_SearchRec.Clear();
            if (!string.IsNullOrEmpty (TextBox_SensorB.Text))
            {
                if (SearchRange(TextBox_SensorB, SensorBList) && SelectionSort(SensorBList) || InsertionSort(SensorBList))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    int found = BinarySearchRecursive(SensorBList, int.Parse(TextBox_SensorB.Text), 0, NumberOfNodes(SensorBList));
                    sw.Stop();
                    long numTickes = sw.ElapsedTicks;
                    TextBoxSsB_SearchRec.Text = numTickes.ToString() + " ticks";
                    DisplayListboxData(SensorBList, ListBoxSensorB);
                    HighlightItems(found, ListBoxSensorB);
                    Button_BinSearchRecursiveB.IsEnabled = false;
                }
                else
                {
                    StatusMS.Text = "Search Value is out of range or sensorBList is Unsorted";
                }
            }
            else
            {
                StatusMS.Text = "Search Input is Empty, Enter the Value";
                TextBox_SensorB.Focus();
            }            
        }
        //Highlight searchValue and two numbers on each side. (searchIndex, -2, +2) 
        private void HighlightItems(int found, ListBox listBox)
        {
            //listBox.SelectionMode = SelectionMode.Multiple;

            //int startIndex = Math.Max(0, found - 2);
            //int endIndex = Math.Min(listBox.Items.Count - 1, found + 2);
            //for (int i = startIndex; i < endIndex; i++)
            //{
            //    listBox.SelectedItems.Add(listBox.Items[i]);
            //}

            if (found >= 0 && found <= 2)
            {
                for (int x = 0; x <= 3; x++)
                {
                    listBox.SelectedItems.Add(listBox.Items.GetItemAt(x));
                }
            }
            else if (found >= 397 && found <= 399)
            {
                for (int x = 0; x <= 397; x++)
                {
                    listBox.SelectedItems.Add(listBox.Items.GetItemAt(x));
                }
            }
            else
            {
                for (int x = found - 2; x <= found + 2; x++)
                {
                    listBox.SelectedItems.Add(listBox.Items.GetItemAt(x));  //highlight 5 elements
                }
            }
        }
        #endregion

        #region Button Sort
        //4.12	Create four button click methods that will sort the LinkedList using the Selection and Insertion methods.
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

            //Button control here(SelectionSort:Disable)
            Button_SelectionSortA.IsEnabled = false;           
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
            Button_SelectionSortB.IsEnabled = false;           
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

            //Button control
            Button_InsertionSortA.IsEnabled = false;
            Button_BinSearchIterativeA.IsEnabled = true;
            Button_BinSearchRecursiveA.IsEnabled = true;
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
            //Button control
            Button_InsertionSortB.IsEnabled = false;
            Button_BinSearchIterativeB.IsEnabled = true;
            Button_BinSearchRecursiveB.IsEnabled = true;
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
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+$");                        
        }
        private void TextBoxB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+$");
        }

        private bool SearchRange(TextBox textbox, LinkedList<double> linkedlist)
        {
            if(!string.IsNullOrEmpty(textbox.Text))
            {
                int searchValue = int.Parse(textbox.Text);
                //Check if searche value is within the range
                if (searchValue > MinValue(linkedlist) && (searchValue < MaxValue(linkedlist)))
                {
                    return true;
                }
                else
                {
                    textbox.Clear();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private int MinValue(LinkedList<double> linkedlist)
        {
            return (int)Math.Floor(linkedlist.ElementAt(0)); //Returns the largest integer value less than/equal to the specified number.
        }
        private int MaxValue(LinkedList<double> linkedlist)
        {
            return (int)Math.Ceiling(linkedlist.ElementAt(399)); //Returns the smallest integer value greater than/equal to the specified number.
        }
              
        #endregion



        //private bool SearchRange(LinkedList<double> linkedlist, int mid, int searchValue)
        //{

        //}

        //private int GetNearestValue(LinkedList<double> linkedList, int mid, int searchValue)
        //{
                //int start = Math.Abs()
                //end =
        //}

    }

}
