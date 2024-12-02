using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mastermind2_PE_NielBeynaerts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        private Button selectedButton; // Track the currently selected button

        string[] selectedColors = new string[4];
        string[] randomColorSelection = new string[4];
        SolidColorBrush[,] guessingHistoryFeedback = new SolidColorBrush[10, 4];

        int selectedColorPosition = 0;

        SolidColorBrush[] chosenColors = new SolidColorBrush[4]; // Initialize array to hold 4 colors

        private DispatcherTimer timer = new DispatcherTimer();

        string userName;
        bool userNameEntered = false;
        int attempts = 0;
        int points = 100;
        public MainWindow()
        {
            InitializeComponent();

            CreateRandomColorCombination();
        }

        public void StartGame()
        {
            do
            {
                userName = Interaction.InputBox($"Welkom! Geef uw naam.", "Welkom");
            } while (userName == null || userName == "");
        }
        public void CreateRandomColorCombination()
        {
            StartGame();

            //SolidColorBrush[] colors = { Brushes.Red, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.White, Brushes.Orange };
            string[] colorsName = { "Red", "Yellow", "Green", "Blue", "White", "Orange" };

            int[] randomColors = new int[4];

            for (int i = 0; i < randomColors.Length; i++)
            {
                randomColors[i] = rnd.Next(0, colorsName.Length); // Enters a number into array
            }

            // Display color names in the window title for debugging
            // RandomColors[n] gives a number, this number is entered in 'ColorsName[]', this gives the name of a color and displays it as title
            // this.Title = $"{colorsName[randomColors[0]]}, {colorsName[randomColors[1]]}, {colorsName[randomColors[2]]}, {colorsName[randomColors[3]]}";

            attempts++;
            this.Title = $"Poging {attempts}";

            randomColorSelection[0] = colorsName[randomColors[0]];
            randomColorSelection[1] = colorsName[randomColors[1]];
            randomColorSelection[2] = colorsName[randomColors[2]];
            randomColorSelection[3] = colorsName[randomColors[3]];

            StartCountdown();
        }

        private void color1Button_Click(object sender, RoutedEventArgs e)
        {
            radioButtonsGroupBox.Visibility = Visibility.Visible;
            selectedButton = color1Button;
            selectedColorPosition = 0;
            UncheckRadioButtons();
        }

        private void color2Button_Click(object sender, RoutedEventArgs e)
        {
            radioButtonsGroupBox.Visibility = Visibility.Visible;
            selectedButton = color2Button;
            selectedColorPosition = 1;
            UncheckRadioButtons();
        }

        private void color3Button_Click(object sender, RoutedEventArgs e)
        {
            radioButtonsGroupBox.Visibility = Visibility.Visible;
            selectedButton = color3Button;
            selectedColorPosition = 2;
            UncheckRadioButtons();
        }

        private void color4Button_Click(object sender, RoutedEventArgs e)
        {
            radioButtonsGroupBox.Visibility = Visibility.Visible;
            selectedButton = color4Button;
            selectedColorPosition = 3;
            UncheckRadioButtons();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton colorRadioButton)
            {
                switch (colorRadioButton.Content.ToString())
                {
                    case "Rood":
                        selectedButton.Background = Brushes.Red;
                        selectedColors[selectedColorPosition] = "Red";
                        break;
                    case "Geel":
                        selectedButton.Background = Brushes.Yellow;
                        selectedColors[selectedColorPosition] = "Yellow";
                        break;
                    case "Groen":
                        selectedButton.Background = Brushes.Green;
                        selectedColors[selectedColorPosition] = "Green";
                        break;
                    case "Blauw":
                        selectedButton.Background = Brushes.Blue;
                        selectedColors[selectedColorPosition] = "Blue";
                        break;
                    case "Wit":
                        selectedButton.Background = Brushes.White;
                        selectedColors[selectedColorPosition] = "White";
                        break;
                    case "Oranje":
                        selectedButton.Background = Brushes.Orange;
                        selectedColors[selectedColorPosition] = "Orange";
                        break;
                    default:
                        break;
                }
            }

        }

        private void UncheckRadioButtons()
        {
            redRadioButton.IsChecked = false;
            greenRadioButton.IsChecked = false;
            blueRadioButton.IsChecked = false;
            whiteRadioButton.IsChecked = false;
            orangeRadioButton.IsChecked = false;
            yellowRadioButton.IsChecked = false;
        }


        private void DisplayGuessOnCanvas(string[] guess, int attempt)
        {
            int totalRowWidth = 250;
            double canvasWidth = attemptCanvas.ActualWidth;
            double startingLeft = (canvasWidth - totalRowWidth) / 2;

            int topPosition = attempt * (30 + 10);

            for (int i = 0; i < guess.Length; i++)
            {
                Label label = new Label
                {
                    Width = 50,
                    Height = 30,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(guess[i])),
                    Content = "",
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                };

                Canvas.SetLeft(label, startingLeft + i * (50 + 10));
                Canvas.SetTop(label, topPosition);

                attemptCanvas.Children.Add(label);
            }
        }


        private void validateColorCode_Click(object sender, RoutedEventArgs e)
        {
            Button[] buttons = { color1Button, color2Button, color3Button, color4Button };

            for (int i = 0; i < selectedColors.Length; i++)
            {
                if (selectedColors[i] == randomColorSelection[i])
                {
                    SetButtonStyle(buttons[i], new Thickness(2, 2, 2, 20), Colors.DarkRed);
                }
                else if (randomColorSelection.Contains(selectedColors[i]))
                {
                    SetButtonStyle(buttons[i], new Thickness(2, 2, 2, 20), Colors.Wheat);
                    points -= 1;
                    pointsLabel.Content = points.ToString();
                }
                else
                {
                    points -= 2;
                    pointsLabel.Content = points.ToString();
                }
            }

            // Display the guess on the canvas
            DisplayGuessOnCanvas(selectedColors, attempts);

            if (selectedColors[0] == randomColorSelection[0] && selectedColors[1] == randomColorSelection[1] && selectedColors[2] == randomColorSelection[2] && selectedColors[3] == randomColorSelection[3])
            {
                StopCountdown();

                MessageBoxResult result = MessageBox.Show($"Code is gekraakt in {attempts} pogingen, wil je nog eens?", $"WINNER", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    ResetGame(); // Herstart het spel
                }
                else
                {
                    Close(); // Sluit de applicatie
                }
            }


            attempts++;
            if (attempts > 10)
            {
                MessageBoxResult result = MessageBox.Show($"Je hebt het maximaal aantal pogingen bereikt, wil je nog eens?", $"WINNER", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    ResetGame(); // Herstart het spel
                }
                else
                {
                    Close(); // Sluit de applicatie
                }
            }
            StopCountdown();
            StartCountdown();
            this.Title = $"Poging {attempts}";
        }
        private void ResetGame()
        {
            // Reset alle variabelen
            attempts = 0;
            points = 100;
            selectedColors = new string[4];
            randomColorSelection = new string[4];
            guessingHistoryFeedback = new SolidColorBrush[10, 4];
            selectedColorPosition = 0;

            // Reset Canvas
            attemptCanvas.Children.Clear();
            pointsLabel.Content = points.ToString();
            timerLabel.Content = "0";

            // Genereer een nieuwe kleurcode
            CreateRandomColorCombination();

            // Update de titel
            this.Title = $"Poging {attempts}";
        }


        private void SetButtonStyle(Button button, Thickness thickness, Color color)
        {
            button.BorderThickness = thickness;
            button.BorderBrush = new SolidColorBrush(color);
        }

        private void ToggleDebug()
        {
            randomGeneratedCodeTextBox.Text = $"{randomColorSelection[0]}, {randomColorSelection[1]}, {randomColorSelection[2]}, {randomColorSelection[3]}";
            randomGeneratedCodeTextBox.Visibility = Visibility.Visible;
        }



        ///<summary>
        ///De 'StartCountdown' methode start een timer
        ///die iedere seconde omhoog gaat. Wanneer de timer 10 seconden bereikt zal deze resetten
        ///en de variable 'attempts' verhogen met 1
        ///</summary>

        DateTime startTime;
        TimeSpan elapsedTime;
        private void StartCountdown()
        {
            if (attempts > 10)
            {
                MessageBox.Show("You have reached the maximum amount of guesses");
                Close();
            }
            else
            {
                startTime = DateTime.Now;
                timer.Tick += Timer_Tick; //Event koppelen
                timer.Interval = new TimeSpan(0, 0, 1); //Elke seconde
                timer.Start(); //Timer starten
            }

        }

        ///<summary>
        ///De 'StopCountdown' methode stopt de timer
        ///die eerder werd gestart via de 'StartCountdown' methode
        ///</summary>
        private void StopCountdown()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;
            timerLabel.Content = elapsedTime.ToString("ss");

            if (elapsedTime.TotalSeconds > 10)
            {
                StopCountdown();
                attempts++;
                this.Title = $"Poging {attempts}";
                StartCountdown();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je het spel wilt beëindigen?", "Afsluiten", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                // Annuleer het afsluiten van de applicatie
                e.Cancel = true;
            }
        }
    }
}