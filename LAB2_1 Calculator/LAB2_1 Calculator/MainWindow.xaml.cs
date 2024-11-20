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

namespace LAB2_1_Calculator
{
    public partial class MainWindow : Window
    {
        double Result = 0;

        string Operator = "";

        bool FirstOperand = true;
        bool OperatorPressed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string pressed = ((Button)e.Source).Content.ToString();

            switch (pressed)
            {
                case "CE": // Initialize
                    Result = 0;
                    FirstOperand = true;
                    TextBlockScreen.Text = "";
                    break;

                case "+":
                case "-":
                case "×":
                case "÷":
                case "=":

                    if (!FirstOperand) // if the first operand was already entered, do the operation.
                    {
                        if (Operator == "+")
                            Result += double.Parse(TextBlockScreen.Text);

                        else if (Operator == "-")
                            Result -= double.Parse(TextBlockScreen.Text);

                        else if (Operator == "×")
                            Result *= double.Parse(TextBlockScreen.Text);

                        else if (Operator == "÷")
                            Result /= double.Parse(TextBlockScreen.Text);
                    }
                    
                    else // if it is the first operand, just accept the operand.
                    {
                        Result = double.Parse(TextBlockScreen.Text);
                        FirstOperand = false;
                    }

                    TextBlockScreen.Text = Result.ToString();

                    Operator = pressed;
                    OperatorPressed = true;

                    break;

                default: // A number was pressed
                    if (OperatorPressed)
                    {
                        TextBlockScreen.Text = pressed;
                        OperatorPressed = false;
                    }
                    else
                        TextBlockScreen.Text += pressed;

                    break;
            }
        }
    }
}
