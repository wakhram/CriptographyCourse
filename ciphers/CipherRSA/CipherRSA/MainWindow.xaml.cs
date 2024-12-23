using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace CipherRSA
{
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        private static readonly Random random = new Random();

        // RSA параметры
        private BigInteger e; // Открытый ключ
        private BigInteger d; // Приватный ключ
        private BigInteger n; // Модуль

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
        
        // Генерация ключей RSA
        private void GenerateRSAKeys()
        {
            // Простые числа
            BigInteger p = GenerateRandomPrime(1024);
            BigInteger q = GenerateRandomPrime(1024);
            n = p * q;
            BigInteger phi = (p - 1) * (q - 1);

            // Выбор e (1 < e < phi, gcd(e, phi) = 1)
            e = 65537; // Стандартное значение e
            while (GCD(e, phi) != 1)
            {
                e++;
            }

            // Вычисление d (d * e ≡ 1 mod phi)
            d = ModInverse(e, phi);

            //MessageBox.Show($"Keys generated:\n" +
              //              $"Open key: e = {e}, n = {n}\n" +
                //            $"Private key: d = {d}, n = {n}");
        }

        // Функция шифрования
        private string EncryptFunc(string plainText)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(plainText);
            BigInteger[] encryptedBlocks = new BigInteger[messageBytes.Length];

            for (int i = 0; i < messageBytes.Length; i++)
            {
                encryptedBlocks[i] = BigInteger.ModPow(new BigInteger(messageBytes[i]), e, n);
            }

            return string.Join(" ", encryptedBlocks);
        }

        // Функция дешифрования
        private string DecryptFunc(string cipherText)
        {
            string[] blocks = cipherText.Split(' ');
            byte[] decryptedBytes = new byte[blocks.Length];

            for (int i = 0; i < blocks.Length; i++)
            {
                BigInteger encryptedBlock = BigInteger.Parse(blocks[i]);
                decryptedBytes[i] = (byte)BigInteger.ModPow(encryptedBlock, d, n);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        // Обработчик кнопки "Encrypt"
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            GenerateRSAKeys();
            string encryptedText = EncryptFunc(ImportedText.Text);
            ResultText.Text = encryptedText;
            ResultText.Foreground = Brushes.Black;
        }

        // Обработчик кнопки "Decrypt"
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                string decryptedText = DecryptFunc(ImportedText.Text);
                ResultText.Text = decryptedText;
                ResultText.Foreground = Brushes.Black;
            }
            catch
            {
                MessageBox.Show("Error while decrypting. Make sure the input text was encrypted.");
            }
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

        // Генерация случайного простого числа
        private BigInteger GenerateRandomPrime(int bitLength)
        {
            BigInteger prime;
            do
            {
                prime = GenerateRandomNumber(BigInteger.Pow(2, 511), BigInteger.Pow(2, 512));
            } while (!IsPrime(prime, 10));
            return prime;
        }

        // Генерация случайного числа в диапазоне [min, max]
        private BigInteger GenerateRandomNumber(BigInteger min, BigInteger max)
        {
            BigInteger result;
            do
            {
                byte[] bytes = new byte[max.GetByteCount()];
                random.NextBytes(bytes);
                result = new BigInteger(bytes);

            } while (result < min || result >= max); // Убедимся, что число в нужном диапазоне

            return result;
        }


        // Проверка на простоту (тест Миллера-Рабина)
        private bool IsPrime(BigInteger number, int k)
        {
            if (number < 2) return false;
            if (number == 2 || number == 3) return true;
            if (number % 2 == 0) return false;

            BigInteger d = number - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = GenerateRandomNumber(2, number - 2);
                BigInteger x = BigInteger.ModPow(a, d, number);
                if (x == 1 || x == number - 1) continue;

                bool isComposite = true;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1) return false;
                    if (x == number - 1)
                    {
                        isComposite = false;
                        break;
                    }
                }

                if (isComposite) return false;
            }
            return true;
        }

        // Расширенный алгоритм Евклида для нахождения обратного по модулю
        private BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, y = 0, x = 1;

            while (a > 1)
            {
                BigInteger q = a / m;
                (a, m) = (m, a % m);
                (x, y) = (y, x - q * y);
            }

            return x < 0 ? x + m0 : x;
        }

        // НОД (алгоритм Евклида)
        private BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
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
