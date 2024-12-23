using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Numerics;
using System.Xml.Linq;

namespace ElGamal
{
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        private static readonly Random random = new Random();

        // plain keys
        int y, g, p;
        // close key
        int x;
        //temporary key
        int tempKey;
        public MainWindow()
        {
            InitializeComponent();

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

        private static bool IsPrime(int number)
        {
            if (number < 2) return false;
            if (number % 2 == 0) return number == 2;

            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        public int GetSimpleNumber(int min, int max)
        {
            Random random = new Random();
            int number;

            do
            {
                number = random.Next(min, max); // Генерируем случайное число в диапазоне
                if (number % 2 == 0) number += 1; // Делаем нечетным
            } while (!IsPrime(number));

            return number;
        }

        public static int GeneratePrimitiveRoot(int p)
        {
            // Ensure the input is a prime number
            if (!IsPrime(p)) throw new ArgumentException("Input must be a prime number.");

            // Store all prime factors of p-1
            int phi = p - 1;
            var primeFactors = GetPrimeFactors(phi);

            for (int g = 2; g < p; g++)
            {
                bool isPrimitiveRoot = true;

                // Check g^(phi / factor) mod p ≠ 1 for all factors
                foreach (var factor in primeFactors)
                {
                    if (ModularExponentiation(g, phi / factor, p) == 1)
                    {
                        isPrimitiveRoot = false;
                        break;
                    }
                }

                if (isPrimitiveRoot) return g;
            }

            throw new Exception("No primitive root found.");
        }

        // Helper: Modular exponentiation to efficiently calculate (base^exp) % mod
        private static int ModularExponentiation(int baseValue, int exp, int mod)
        {
            int result = 1;
            baseValue %= mod;

            while (exp > 0)
            {
                if ((exp & 1) == 1) // If exp is odd
                {
                    result = (result * baseValue) % mod;
                }

                baseValue = (baseValue * baseValue) % mod;
                exp >>= 1; // Divide exp by 2
            }

            return result;
        }

        // Helper: Find all prime factors of a number
        private static List<int> GetPrimeFactors(int n)
        {
            var factors = new List<int>();
            for (int i = 2; i * i <= n; i++)
            {
                while (n % i == 0)
                {
                    factors.Add(i);
                    n /= i;
                }
            }

            if (n > 1) factors.Add(n);
            return factors;
        }


        public void keyGeneration()
        {
            bool keysGenerated = false; 

            while (!keysGenerated)
            {
                try
                {
                    p = GetSimpleNumber(200, 60000);
                    g = GeneratePrimitiveRoot(p);

                    x = GetSimpleNumber(1, p - 1);
                    y = ModularExponentiation(g, x, p);

                    keysGenerated = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}. Retrying key generation...");
                }
            }
        }

        static int[] ConvertCharArrayToArray(char[] charArray)
        {
            int[] result = new int[charArray.Length];

            for (int i = 0; i < charArray.Length; i++)
            {
                result[i] = (int)charArray[i];
            }

            return result;
        }

        public int GetRandomNumberWithGCD(int lowerBound, int upperBound)
        {
            Random rand = new Random();
            int x;

            do
            {
;                x = rand.Next(lowerBound + 1, upperBound);
            }
            while (GCD(x, upperBound) != 1);

            return x;
        }

        public int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public string EncryptFunc(string plainText)
        {
            char[] text = plainText.ToCharArray();
            int[] textAsNumber = ConvertCharArrayToArray(text);
            keyGeneration();
            StringBuilder encryptedText = new StringBuilder();
            
            tempKey = GetRandomNumberWithGCD(1, p - 1);
            int a = ModularExponentiation(g, tempKey, p);
            foreach (int n in textAsNumber)
            {
                int b = (ModularExponentiation(y, tempKey, p) * n) % p; // Второй компонент шифротекста
                
                encryptedText.Append((char)a);
                encryptedText.Append((char)b);
            }

            return encryptedText.ToString();
        }

        public string DecryptFunc(string encryptedText)
        {
            char[] text = encryptedText.ToCharArray();
            int[] textAsNumber = ConvertCharArrayToArray(text);
            //keyGeneration();
            StringBuilder decryptedText = new StringBuilder();

            for (int i = 0; i < textAsNumber.Length; i += 2)
            {
                int a = encryptedText[i];     // Первый символ
                int b = encryptedText[i + 1]; // Второй символ

                int aInverse = ModularExponentiation(a, p - 1 - x, p); // Обратный элемент
                int m = (aInverse * b) % p; // Исходное сообщение

                decryptedText.Append((char)m);
            }

            return decryptedText.ToString();
        }
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            string encryptedtext = EncryptFunc(ImportedText.Text);
            ResultText.Text = encryptedtext;
            ResultText.Foreground = Brushes.Black;
        }
        
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            string decryptedtext = DecryptFunc(ImportedText.Text);
            ResultText.Text = decryptedtext;
            ResultText.Foreground = Brushes.Black;
        }

        private bool ValidateInputs()
        {
            string textToProcess = ImportedText.Text;
            if (string.IsNullOrEmpty(textToProcess) || textToProcess == "Here will be imported text...")
            {
                MessageBox.Show("Please import or enter text to encrypt/decrypt.");
                return false;
            }

            return true;
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            ImportedText.Text = ResultText.Text;
            ResultText.Text = "";
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

    }
}
