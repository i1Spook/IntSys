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

namespace IntSysA2WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid myGrid;
        Button btn;
        Slider sld1;
        TextBox txt1;
        
        public MainWindow()
        {
            /*Beim Drücken des Reset‐Buttons soll der Schieberegler auf den Wert 0 gesetzt werden und das links 
            stehende  Textfeld  eine  0  anzeigen.  Bei  der  Interaktion  mit  dem  Schieberegler  soll  ein  Wert  von 
            minimal 0 und maximal 100 im links stehenden Textfeld angezeigt werden. Der Schieberegler soll als 
            Schrittweite „1“ verwenden. */

            InitializeComponent();   

            sld1 = new Slider();
            sld1.HorizontalAlignment = HorizontalAlignment.Left;
            sld1.VerticalAlignment = VerticalAlignment.Top;
            sld1.Height = 36;
            sld1.Width = 227;
            sld1.Margin = new Thickness(148, 52, 0, 0);
            sld1.Minimum = 0;
            sld1.Maximum = 100;
            sld1.Name = "slValue";
            sld1.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            sld1.TickFrequency = 1;
            sld1.IsSnapToTickEnabled = true;
            sld1.ValueChanged += sliderChanged;

            btn = new Button();
            btn.Content = "Reset";
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Width = 111;
            btn.Height = 22;
            btn.Margin = new Thickness(343, 113, 0, 0);
            btn.Name = "btn1";
            btn.Click += meinHandler;

            txt1 = new TextBox();
            txt1.Name = "textbox";
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.VerticalAlignment = VerticalAlignment.Top;
            txt1.Width = 120;
            txt1.Height = 23;
            txt1.Margin = new Thickness(10, 52, 0, 0);
            txt1.TextWrapping = TextWrapping.Wrap;

            myGrid = new Grid();
            myGrid.Children.Add(sld1);
            myGrid.Children.Add(btn);
            myGrid.Children.Add(txt1);
            AddChild(myGrid);

            //btn1.Click += btn1_Click;
        }

        private void sliderChanged(object sender, RoutedEventArgs e)
        {
            txt1.Text = sld1.Value.ToString();
        }
        private void meinHandler(object sender, RoutedEventArgs e)
        {
            sld1.Value = 0;
        }
        //private void btn1_Click(object sender, RoutedEventArgs e)
        //{
        //    slValue.Value = 0;
            
            
        //}
        //private void TextBox_Changed(object sender, RoutedEventArgs e)
        //{
        //    textbox.Text = slValue.Value.ToString();
        //}
    }
}
