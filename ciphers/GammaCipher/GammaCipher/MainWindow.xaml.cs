using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;

namespace GammaCipher
{
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        string keyStart = "";
        string polinom = "";
        public MainWindow()
        {
            InitializeComponent();

            KeyText.Text = "Enter key...";
            KeyText.Foreground = Brushes.Gray;
            PolinomText.Text = "Enter polinom...";
            PolinomText.Foreground = Brushes.Gray;
            ImportedText.Text = "Here will be imported text...";
            ImportedText.Foreground = Brushes.Gray;
            ResultText.Text = "Here will be result text...";
            ResultText.Foreground = Brushes.Gray;
        }

        // Открытие и чтение файла
        private void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";

            bool? result = ofd.ShowDialog();
            if (result == true)
            {
                fileContent = File.ReadAllText(ofd.FileName);
                fileName = ofd.SafeFileName;
                FilePathTextBlock1.Text = ofd.FileName;
                ImportedText.Text = fileContent;
                ImportedText.Foreground = Brushes.Black;
            }
        }

        // Сохранение зашифрованного текста в файл
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            fileName = fileName.Replace(".txt", "");
            string savedFileName = fileName + "_encrypted.txt";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, savedFileName);

            try
            {
                File.WriteAllText(filePath, ResultText.Text);
                FilePathTextBlock2.Text = $"File saved: {filePath}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Метод генерации ключа с помощью линейно-конгруэнтного генератора
        //КОНГРУЭНТНЫЙ
        /*private string GenerateKey(int length)
        {
            var key = new StringBuilder();
            int a = 1664525;    
            int c = 1013904223; 
            int m = (int)Math.Pow(2, 31); 
            int current = int.Parse(polinom);

            for (int i = 0; i < length; i++)
            {
                current = (a * current + c) % m;
                key.Append((char)(current % 256));
            }
            return key.ToString();
        }*/
        private string GenerateKey()
        {
            int[] startPos = keyStart.Select(c => (int)char.GetNumericValue(c)).ToArray();
            int[] polinomArray = polinom.Select(c => (int)char.GetNumericValue(c)).ToArray();
            List<int> finalKey = new List<int>();
            string original = string.Join("", startPos);
            string current = original;

            do
            {
                finalKey.Add(startPos[^1]);

                int pos1 = polinomArray[0] - 1;
                int pos2 = polinomArray[1] - 1;
                int xorResult = startPos[pos1] ^ startPos[pos2];

                int[] newArray = new int[startPos.Length];
                newArray[0] = xorResult;
                Array.Copy(startPos, 0, newArray, 1, startPos.Length - 1);

                startPos = newArray;
                current = string.Join("", startPos);

            } while (current != original);

            string finalKeyString = string.Join("", finalKey);
             FilePathTextBlock2.Text = finalKeyString;
            return finalKeyString;
        }

        private string ExpandKeyToMatchTextLength(string baseKey, int textLength)
        {
            StringBuilder expandedKey = new StringBuilder();

            while (expandedKey.Length < textLength)
            {
                expandedKey.Append(baseKey);
            }

            return expandedKey.ToString().Substring(0, textLength);
        }

        private string TextToBinary(string text)
        {
            return string.Join("", text.Select(c => Convert.ToString(c, 2).PadLeft(8, '0')));
        }

        private string BinaryToText(string binary)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < binary.Length; i += 8)
            {
                string byteString = binary.Substring(i, 8);
                char character = (char)Convert.ToInt32(byteString, 2);
                text.Append(character);
            }
            return text.ToString();
        }

        private string ProcessTextWithXOR(string text)
        {
            string binaryText = TextToBinary(text);
            string baseKey = GenerateKey();
            string expandedKey = ExpandKeyToMatchTextLength(baseKey, binaryText.Length);

            StringBuilder encryptedBinary = new StringBuilder();
            for (int i = 0; i < binaryText.Length; i++)
            {
                char xorResult = (char)((binaryText[i] - '0') ^ (expandedKey[i] - '0') + '0');
                encryptedBinary.Append(xorResult);
            }

            return BinaryToText(encryptedBinary.ToString());
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            string encryptedText = ProcessTextWithXOR(ImportedText.Text);
            ResultText.Text = encryptedText;
            ResultText.Foreground = Brushes.Black;
        }

        // Метод проверки входных данных (ключ и текст)
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(polinom) || string.IsNullOrEmpty(keyStart) || keyStart == "Enter key..." || polinom == "Enter polinom...")
            {
                MessageBox.Show("Please enter a key for encryption/decryption.");
                return false;
            } 

            string textToProcess = ImportedText.Text;
            if (string.IsNullOrEmpty(textToProcess) || textToProcess == "Here will be imported text...")
            {
                MessageBox.Show("Please import or enter text to encrypt/decrypt.");
                return false;
            }

            return true;
        }

        // Функция для передачи текста из результата в поле ввода
        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            ImportedText.Text = ResultText.Text;
            ResultText.Text = "";
        }

        // События фокуса для текстовых полей (для удобства пользователя)
        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (KeyText.Text == "Enter key...")
            {
                KeyText.Text = "";
                KeyText.Foreground = Brushes.Black;
            }
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(KeyText.Text))
            {
                KeyText.Text = "Enter key...";
                KeyText.Foreground = Brushes.Gray;
            }
        }

        private void InputTextBox_GotFocus2(object sender, RoutedEventArgs e)
        {
            if (ResultText.Text == "Here will be result text...")
            {
                ResultText.Text = "";
                ResultText.Foreground = Brushes.Black;
            }
        }

        private void InputTextBox_LostFocus2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ResultText.Text))
            {
                ResultText.Text = "Here will be result text...";
                ResultText.Foreground = Brushes.Gray;
            }
        }

        private void InputTextBox_GotFocus3(object sender, RoutedEventArgs e)
        {
            if (ImportedText.Text == "Here will be imported text...")
            {
                ImportedText.Text = "";
                ImportedText.Foreground = Brushes.Black;
            }
        }

        private void InputTextBox_LostFocus3(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ImportedText.Text))
            {
                ImportedText.Text = "Here will be imported text...";
                ImportedText.Foreground = Brushes.Gray;
            }
        }

        private void InputTextBox_GotFocus4(object sender, RoutedEventArgs e)
        {
            if (PolinomText.Text == "Enter polinom...")
            {
                PolinomText.Text = "";
                PolinomText.Foreground = Brushes.Black;
            }
        }

        private void InputTextBox_LostFocus4(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PolinomText.Text))
            {
                PolinomText.Text = "Enter polinom...";
                PolinomText.Foreground = Brushes.Gray;
            }
        }

        private void KeyText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            keyStart = KeyText.Text;
        }

        private void PolinomText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            polinom = PolinomText.Text;
        }
    }
}
