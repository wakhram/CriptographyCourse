using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;


namespace BreakingTheCaeserCipher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        char[] lettersByFrequency = ['E', 'T', 'A', 'O', 'I', 'N', 'S', 'H', 'R', 'D', 'L', 'C', 'U', 'M', 'W',
                                       'F', 'G', 'Y', 'P', 'B', 'V', 'K', 'J', 'X', 'Q', 'Z'];
        Dictionary<char, int> LettersFrequencyDictionary = new Dictionary<char, int>();

        int index = -1;
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 26; i++)
            {
                LettersFrequencyDictionary.Add((char)('A' + i), 0);
            }
        }

        private void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (char key in LettersFrequencyDictionary.Keys)
                LettersFrequencyDictionary[key] =  0;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                fileContent = File.ReadAllText(ofd.FileName);
                fileName = ofd.SafeFileName;
                string filePath = ofd.FileName;
                FilePathTextBlock1.Text = filePath;
                EcnryptedInputText.Text = fileContent;
                CountLettersFrequency();
                index = 0;
                BrokenText.Text = BreakTheCaeserCipher();
                AmountOfAttemtps.Text = "Amount of attempts: " + (index + 1).ToString();
            }
        }

        private void CountLettersFrequency()
        {
            foreach(char letter in fileContent)
            {
                if (LettersFrequencyDictionary.ContainsKey(letter))
                {
                    LettersFrequencyDictionary[letter] += 1;
                }
                else if (LettersFrequencyDictionary.ContainsKey(char.ToUpper(letter)))
                {
                    LettersFrequencyDictionary[char.ToUpper(letter)] += 1;
                }
                else
                {
                    continue;
                }
            }
        }

        char MaxValue()
        {
            int max = LettersFrequencyDictionary['A'];
            char l = 'A';
            foreach(char letter in LettersFrequencyDictionary.Keys)
            {
                if (LettersFrequencyDictionary[letter] > max)
                {
                    max = LettersFrequencyDictionary[letter];
                    l = letter;
                }
            }

            return l;
        }

        private string BreakTheCaeserCipher()
        {
            int shift = MaxValue() - lettersByFrequency[index];
            KeyLengthTextBlock.Text = (shift).ToString();
            MostFrequencyLetter.Text = "Letter: " + MaxValue().ToString(); 


            StringBuilder decryptedText = new StringBuilder();

            foreach (char c in fileContent)
            {
                if (char.IsLetter(c))
                {
                    char offset;

                    if (char.IsUpper(c))
                        offset = 'A';
                    else  
                        offset = 'a';

                    char decryptedChar = (char)(((c - shift - offset + 26) % 26) + offset);

                    decryptedText.Append(decryptedChar);
                }
                else
                {
                    decryptedText.Append(c);
                }
            }

            return decryptedText.ToString();
        }

        private void NextLetterFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock1.Text == "No file attached.") 
            {
                MessageBox.Show("First, you have to choose file.");
            }
            else
            {
                EcnryptedInputText.Text = fileContent;
                index++;
                BrokenText.Text = BreakTheCaeserCipher();
                AmountOfAttemtps.Text = "Amount of attempts: " + (index + 1).ToString();
            }
        }

        private void PreviousLetterFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (index == 0)
            {
                MessageBox.Show("First, you have to try at least once.");
            }
            else
            {
                EcnryptedInputText.Text = fileContent;
                index--;
                BrokenText.Text = BreakTheCaeserCipher();
                AmountOfAttemtps.Text = "Amount of attempts: " + (index - 1).ToString();
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            fileName = fileName.Replace(".txt", "");
            string savedFileName = fileName + "_broken.txt";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(desktopPath, savedFileName);

            try
            {
                File.WriteAllText(filePath, BrokenText.Text);
                FilePathTextBlock2.Text = $"File saved: {filePath}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}