using System;
using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace FittsExercise
{
    /// <summary>
    /// Experiment addressing Fitt's law.
    /// 
    /// Measures time to reach a randomly placed button ("target").
    /// 
    /// Involves precuing: The target is displayed before a measurement starts. 
    /// 
    /// Important elements (created via XAML):
    /// 
    /// bnStart: a button, which makes the target visible, and which starts a measurement
    /// eTarget: the circle, which serves as target for the task; clicking this button stops a measurement
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random myRandomizer;
        private int counter = 0;
        private int[] distanceX;
        private int[] distanceY;
        private int[] targetW;
        private int[] targetH;
        private int[] error;
        private DateTime[] startTime;
        private DateTime[] endTime;

        private int experimentId = 1;
        // number of measurements per experiment
        private int nbrOfTasks = 20;
        // mouse pointer automatically set to start button once target is clicked
        private bool resetMousePos = false;
        public bool ResetMousePos
        {
          get{ return resetMousePos; }
          set{ resetMousePos = value; }
        }
        // display new target once current target is clicked 
        private bool precuing = true;
        public bool Precuing
        {
          get { return precuing; }
          set { precuing = value; }
        }

        // flag, indicates if the start button was pressed (...and the hit button should be released)
        private bool doAcquireTarget = false;
        // "click me!" brush
        Brush brClickMe = Brushes.GreenYellow;
        Brush brDontClickMe = Brushes.White;

        // number of errors across all tasks of this experiment session
        private int sessionErrorCount = 0;
        // number of errors observed during the current target acquisition task
        private int taskErrorCount = 0;

        /// <summary>
        /// Construct the main window. Initializes all UI elements.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.myRandomizer = new Random();

            // hide debug info (might obscure small targets)
            this.tbTimings.Visibility = Visibility.Hidden;

            // highlight the start button
            this.bnStart.Background = brClickMe;

            // new size and position of target
            this.Target.Width = this.Target.Height = myRandomizer.Next(5, 100);
            this.Target.Margin = new Thickness(myRandomizer.Next(0, 500), myRandomizer.Next(0, 500), 0, 0);
            if (!this.precuing) this.Target.Visibility = Visibility.Hidden;

            //ToDo
            //Open Setup Window modally
            SetupWindow setup = new SetupWindow();
            bool? setupResult = setup.ShowDialog();
            

            // Build dialog for setting up the experiment
            // Instanciate UserWindow and show it as modal dialog
            // Transfer user input into the following variables
            //      experimentId
            //      nbrOfTasks
            //      resetMousePos
            //      precuing

            // prepare arrays for recording data
            this.distanceX = new int[nbrOfTasks];
            this.distanceY = new int[nbrOfTasks];
            this.targetW = new int[nbrOfTasks];
            this.targetH = new int[nbrOfTasks];
            this.error = new int[nbrOfTasks];
            this.startTime = new DateTime[nbrOfTasks];
            this.endTime = new DateTime[nbrOfTasks];
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.doAcquireTarget)
            {
                // start was clicked, but the user should have clicked target
                this.taskErrorCount++;
            }
            else
            {
                // start counting errors
                this.taskErrorCount = 0;
                // start measurement of time
                this.startTime[counter] = DateTime.Now;
                // record mouse coordinates, calucation happens in HitButtonClicked
                this.distanceX[counter] = (int)Mouse.GetPosition(this).X;
                this.distanceY[counter] = (int)Mouse.GetPosition(this).Y;
                // record width and height
                this.targetW[counter] = (int)this.Target.Width;
                this.targetH[counter] = (int)this.Target.Height;
                // unlock the target button
                this.doAcquireTarget = true;
                this.bnStart.Background = this.brDontClickMe;
                this.Target.Fill = this.brClickMe;
                this.Target.Visibility = Visibility.Visible;
            }
            // prevent error count to be increased by window mouseLeftButtonDown handler
            e.Handled = true;
        }

        private void TargetClicked(object sender, MouseButtonEventArgs e)
        {
            if (this.doAcquireTarget)
            {
                // enforce need to click start button
                this.doAcquireTarget = false;
                // calculate distance
                this.distanceX[counter] = (int)(Mouse.GetPosition(this).X - this.distanceX[counter]);
                this.distanceY[counter] = (int)(Mouse.GetPosition(this).Y - this.distanceY[counter]);
                // store current time
                this.endTime[counter] = DateTime.Now;
                // store number errors counted so far
                this.error[counter] = this.taskErrorCount;
                this.sessionErrorCount += this.taskErrorCount;

                // decide on what to do in the next iteration
                this.counter++;
                if (this.counter < this.nbrOfTasks)
                {
                    // new size of target
                    this.Target.Width = this.Target.Height = this.myRandomizer.Next(5, 100);
                    // use dummies to check for intersections with the start button
                    Rect rStartButton = new Rect(this.bnStart.Margin.Left, this.bnStart.Margin.Top, this.bnStart.ActualWidth, this.bnStart.ActualHeight);
                    Rect rTarget = new Rect(0, 0, this.Target.Width, this.Target.Width);
                    do
                    {
                        rTarget.X = this.myRandomizer.Next(0, 500);
                        rTarget.Y = this.myRandomizer.Next(0, 500);
                    } while (rTarget.IntersectsWith(rStartButton));
                    this.Target.Margin = new Thickness(rTarget.X, rTarget.Y, 0, 0);

                    // highlight the button to be clicked next
                    bnStart.Background = brClickMe;
                    if (precuing) Target.Fill = brDontClickMe;
                    else Target.Visibility = Visibility.Hidden; // hide the target button
                    if (resetMousePos) setMouseCursorPos(MainWindowInstance.Width / 2, MainWindowInstance.Height / 2);
                }
                else
                {
                    // experiment accomplished
                    this.EndExperiment();
                }
            }
            // prevent error count to be increased by window mouseLeftButtonDown handler
            e.Handled = true;
        }

        private void WindowClicked(object sender, MouseButtonEventArgs e)
        {
            // accidentially clicked on window instead of target
            if (this.doAcquireTarget)
            {
                this.taskErrorCount++;
            }
        }

        // DLL für API-Funktion importieren; externe Methode SetCursorPos deklarieren; eigene Methode setMouseCursorPos definieren; 
        [DllImport("user32.dll")]
        extern static int SetCursorPos(int x, int y);
        void setMouseCursorPos(double x, double y)
        {
            Point pUpperLeftCorner = new Point(this.Left + x, this.Top + y);
            // Punkt von geräteunabhängigen Koordinaten konvertieren in Pixel-Koordinaten
            Point pPixelCoordinates = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.Transform(pUpperLeftCorner);

            SetCursorPos((int)pPixelCoordinates.X, (int)pPixelCoordinates.Y);
        }

        protected void EndExperiment()
        {
            this.bnStart.Visibility = Visibility.Hidden;
            this.Target.Visibility = Visibility.Hidden;

            //#toDo
            // open a message box, which displays the number of errors occurred in this experiment
            string message = "Number of errors";
            string caption = "Errors";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            ////DialogResult result;
            //result = MessageBox.Show(message, caption, buttons);

            //if (result == System.Windows.Forms.DialogResult.Yes)
            //{

            //}

      // record this session's data in a file (name includes session id)
      string fileName = "testResults_" + resetMousePos + "_" + precuing + "_" + experimentId;
            string fileNameExt = ".csv";

            //#toDo
            // open a save dialog, which allows for confirming or editing the filename

            //#toDo delete this line once the filename dialog is ready to use
            fileName += fileNameExt;

            // save document
            StreamWriter file = new StreamWriter(fileName);

            tbTimings.Text = "";
            // iterate measurements
            for (int i = 0; i < nbrOfTasks; i++)
            {
                // calculate milliseconds of the current measurement
                int millisNeeded = (endTime[i] - startTime[i]).Seconds * 1000 + (endTime[i] - startTime[i]).Milliseconds;
                // some debug on the screen
                tbTimings.Text += "Timing " + i + ": " + (millisNeeded) + " errors: " + error[i] + '\n';
                // write configuration data
                file.Write(experimentId + ";" + resetMousePos + ";" + precuing);
                // add measurements
                file.WriteLine(";" + millisNeeded + ";" + distanceX[i] + ";" + distanceY[i] + ";" + targetW[i] + ";" + targetH[i] + ";" + error[i].ToString());
            }
            file.Close();

            // Close the application
            Close();
        }
    }
}