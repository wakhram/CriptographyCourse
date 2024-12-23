using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;

namespace Hill_sCipher
{
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        string keyStart = "";
        int m = 26;
        private static readonly Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();

            KeyText.Text = "Enter key...";
            KeyText.Foreground = Brushes.Gray;
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

        public int[,] CreateMatrixFromWord(string word)
        {
            int length = word.Length;

            int matrixSize = (int)Math.Sqrt(length);

            int[,] matrix = new int[matrixSize, matrixSize];

            for (int i = 0; i < length; i++)
            {
                char letter = word[i];
                int letterIndex = char.ToUpper(letter) - 'A';

                int row = i / matrixSize;
                int col = i % matrixSize;

                matrix[row, col] = letterIndex;
            }

            return matrix;
        }

        public int CalculateDeterminant(int[,] matrix)
        {
            int determinant = 0;
            int size = matrix.GetLength(0);

            if (size == 2)
            {
                determinant = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                return determinant < 0 ? (m + determinant % m) % m : determinant % m;
            }

            for (int col = 0; col < size; col++)
            {
                int[,] minor = GetMinor(matrix, 0, col);
                int minorDeterminant = CalculateDeterminant(minor);

                determinant += (col % 2 == 0 ? 1 : -1) * matrix[0, col] * minorDeterminant;
            }

            return determinant < 0 ? (m + determinant % m) % m : determinant % m;
        }

        private int[,] GetMinor(int[,] matrix, int rowToRemove, int colToRemove)
        {
            int size = matrix.GetLength(0);
            int[,] minor = new int[size - 1, size - 1];

            int minorRow = 0, minorCol;

            for (int i = 0; i < size; i++)
            {
                if (i == rowToRemove) continue;

                minorCol = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j == colToRemove) continue;

                    minor[minorRow, minorCol] = matrix[i, j];
                    minorCol++;
                }
                minorRow++;
            }

            return minor;
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

        public string[,] ProcessText(string text, int keyLength)
        {
            text = text.ToUpper();  
            int rootLength = (int)Math.Sqrt(keyLength);

            int remainder = text.Length % rootLength;
            if (remainder != 0)
            {
                text = PadTextToMultiple(text, rootLength - remainder);
            }

            int rows = text.Length / rootLength;
            string[,] matrix = new string[rows, rootLength];

            int textIndex = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < rootLength; col++)
                {
                    matrix[row, col] = text[textIndex].ToString();
                    textIndex++;
                }
            }

            return matrix;
        }

        private string PadTextToMultiple(string text, int paddingLength)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < paddingLength; i++)
            {
                char randomChar = alphabet[random.Next(alphabet.Length)];
                text += randomChar;
            }
            return text;
        }

        public string EncryptText(string[,] textMatrix, int[,] keyMatrix)
        {
            int blockSize = keyMatrix.GetLength(0);
            StringBuilder encryptedText = new StringBuilder();

            for (int row = 0; row < textMatrix.GetLength(0); row++)
            {
                int[] messageBlock = new int[blockSize];

                for (int col = 0; col < blockSize; col++)
                {
                    messageBlock[col] = CharToIndex(textMatrix[row, col]);
                }

                int[] encryptedBlock = EncryptBlock(messageBlock, keyMatrix);

                foreach (int index in encryptedBlock)
                {
                    encryptedText.Append(IndexToChar(index));
                }
            }

            return encryptedText.ToString();
        }

        private int[] EncryptBlock(int[] messageBlock, int[,] keyMatrix)
        {
            int blockSize = keyMatrix.GetLength(0);
            int[] encryptedBlock = new int[blockSize];

            for (int i = 0; i < blockSize; i++)
            {
                encryptedBlock[i] = 0;
                for (int j = 0; j < blockSize; j++)
                {
                    encryptedBlock[i] += keyMatrix[i, j] * messageBlock[j];
                }
                encryptedBlock[i] %= m;
            }

            return encryptedBlock;
        }

        private int CharToIndex(string character)
        {
            return char.ToUpper(character[0]) - 'A';  
        }

        private char IndexToChar(int index)
        {
            return (char)('A' + index);
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;
            if (!ValidateKey()) return;

            int[,] key = CreateMatrixFromWord(keyStart);

            int determinant = CalculateDeterminant(key);
            FilePathTextBlock2.Text = determinant.ToString();
            if (GCD(determinant, m) != 1)
            {
                MessageBox.Show("The key matrix determinant is not invertible under modulo 26. Please use a different key.");
                return;
            }

            string[,] text = ProcessText(ImportedText.Text, keyStart.Length);

            string encrypted = EncryptText(text, key);
            ResultText.Text = encrypted;
            ResultText.Foreground = Brushes.Black;
        }



        public int[,] InverseMatrixModulo(int[,] matrix, int modulo)
        {
            int size = matrix.GetLength(0);
            int determinant = CalculateDeterminant(matrix);
            int inverseDeterminant = ModularMultiplicativeInverse(determinant, modulo);

            if (inverseDeterminant == -1)
            {
                throw new InvalidOperationException("The inverse matrix does not exist because the determinant has no modular inverse.");
            }

            int[,] adjugateMatrix = new int[size, size];

            if (size == 2)
            {
                adjugateMatrix[0, 0] = matrix[1, 1] * inverseDeterminant % modulo;
                adjugateMatrix[0, 1] = -matrix[0, 1] * inverseDeterminant % modulo;
                adjugateMatrix[1, 0] = -matrix[1, 0] * inverseDeterminant % modulo;
                adjugateMatrix[1, 1] = matrix[0, 0] * inverseDeterminant % modulo;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (adjugateMatrix[i, j] < 0)
                            adjugateMatrix[i, j] += modulo;
                    }
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        int[,] minor = GetMinor(matrix, i, j);
                        int minorDeterminant = CalculateDeterminant(minor);
                        int cofactor = ((i + j) % 2 == 0 ? 1 : -1) * minorDeterminant;
                        adjugateMatrix[j, i] = (cofactor * inverseDeterminant) % modulo;

                        if (adjugateMatrix[j, i] < 0)
                            adjugateMatrix[j, i] += modulo;
                    }
                }
            }

            return adjugateMatrix;
        }

        public int ModularMultiplicativeInverse(int a, int modulo)
        {
            int t = 0, newT = 1;
            int r = modulo, newR = a < 0 ? modulo + (a % modulo) : a % modulo;

            while (newR != 0)
            {
                int quotient = r / newR;
                (t, newT) = (newT, t - quotient * newT);
                (r, newR) = (newR, r - quotient * newR);
            }

            if (r > 1) return -1;
            if (t < 0) t += modulo;

            return t;
        }

        public string DecryptText(string[,] encryptedMatrix, int[,] keyMatrix)
        {
            int[,] inverseKeyMatrix = InverseMatrixModulo(keyMatrix, m);
            int blockSize = keyMatrix.GetLength(0);
            StringBuilder decryptedText = new StringBuilder();

            for (int row = 0; row < encryptedMatrix.GetLength(0); row++)
            {
                int[] encryptedBlock = new int[blockSize];

                for (int col = 0; col < blockSize; col++)
                {
                    encryptedBlock[col] = CharToIndex(encryptedMatrix[row, col]);
                }

                int[] decryptedBlock = DecryptBlock(encryptedBlock, inverseKeyMatrix);

                foreach (int index in decryptedBlock)
                {
                    decryptedText.Append(IndexToChar(index));
                }
            }

            return decryptedText.ToString();
        }

        private int[] DecryptBlock(int[] encryptedBlock, int[,] inverseKeyMatrix)
        {
            int blockSize = inverseKeyMatrix.GetLength(0);
            int[] decryptedBlock = new int[blockSize];

            for (int i = 0; i < blockSize; i++)
            {
                decryptedBlock[i] = 0;
                for (int j = 0; j < blockSize; j++)
                {
                    decryptedBlock[i] += inverseKeyMatrix[i, j] * encryptedBlock[j];
                }
                decryptedBlock[i] %= m;
                if (decryptedBlock[i] < 0) decryptedBlock[i] += m;
            }

            return decryptedBlock;
        }
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;
            if (!ValidateKey()) return;

            int[,] key = CreateMatrixFromWord(keyStart);

            int determinant = CalculateDeterminant(key);
            if (GCD(determinant, m) != 1)
            {
                MessageBox.Show("The key matrix determinant is not invertible under modulo 26. Cannot decrypt.");
                return;
            }

            string[,] encryptedText = ProcessText(ImportedText.Text, keyStart.Length);

            string decrypted = DecryptText(encryptedText, key);
            ResultText.Text = decrypted;
            ResultText.Foreground = Brushes.Black;
        }

        private bool ValidateKey()
        {
            if (keyStart.Length == 4 || keyStart.Length == 9 || keyStart.Length == 16)
            {
                return true;
            }
            MessageBox.Show("Length of key should be 4/9/16.");
            return false;
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(keyStart) || keyStart == "Enter key...")
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

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            ImportedText.Text = ResultText.Text;
            ResultText.Text = "";
        }

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

        private void KeyText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            keyStart = KeyText.Text;
        }

    }
}
