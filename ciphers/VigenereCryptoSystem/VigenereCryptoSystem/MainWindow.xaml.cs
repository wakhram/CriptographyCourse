using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;

namespace VigenereCryptoSystem
{
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        string key = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        public string Encrypt(string plainText)
        {
            string result = "";
            int keyIndex = 0;

            foreach (char symbol in plainText)
            {
                if (char.IsLetter(symbol))
                {
                    char symbolUpper = char.ToUpper(symbol);
                    char keyUpper = char.ToUpper(key[keyIndex % key.Length]);

                    // Определение позиции символа и ключа в таблице Виженера
                    int row = keyUpper - 'A';  // Позиция в левом столбце (ключ)
                    int column = symbolUpper - 'A';  // Позиция в верхней строке (символ текста)

                    // Найдём зашифрованный символ в таблице (пересечение строки и столбца)
                    char encryptedChar = (char)('A' + (row + column) % 26);

                    // Сохранение регистра (если исходный символ был в нижнем регистре, сохраняем его)
                    result += char.IsUpper(symbol) ? encryptedChar : char.ToLower(encryptedChar);

                    keyIndex++;
                }
                else
                {
                    result += symbol;  // Пропуск не буквенных символов
                }
            }
            return result;
        }

        public string Decrypt(string cipherText)
        {
            string result = "";
            int keyIndex = 0;

            foreach (char symbol in cipherText)
            {
                if (char.IsLetter(symbol))
                {
                    char symbolUpper = char.ToUpper(symbol);
                    char keyUpper = char.ToUpper(key[keyIndex % key.Length]);

                    // Определение позиции ключа в левом столбце
                    int row = keyUpper - 'A';

                    // Поиск исходной позиции в верхней строке
                    int decryptedPos = (symbolUpper - 'A' - row + 26) % 26;
                    char decryptedChar = (char)('A' + decryptedPos);

                    // Сохранение регистра
                    result += char.IsUpper(symbol) ? decryptedChar : char.ToLower(decryptedChar);

                    keyIndex++;
                }
                else
                {
                    result += symbol;  // Пропуск не буквенных символов
                }
            }
            return result;
        }

        private void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt";

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                fileContent = File.ReadAllText(ofd.FileName);
                fileName = ofd.SafeFileName;
                string filePath = ofd.FileName;
                FilePathTextBlock1.Text = filePath;
                ImportedText.Text = fileContent;
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock1.Text == "No file attached." || KeyTextBox.Text == "")
            {
                MessageBox.Show("Choose file and enter key!");
            }
            else
            {
                ChangedText.Text = Decrypt(ImportedText.Text);
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock1.Text == "No file attached." || KeyTextBox.Text == "")
            {
                MessageBox.Show("Choose file and enter key!");
            }
            else
            {
                ChangedText.Text = Encrypt(ImportedText.Text);
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            fileName = fileName.Replace(".txt", "");
            string savedFileName = fileName + "changed.txt";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(desktopPath, savedFileName);

            try
            {
                File.WriteAllText(filePath, ChangedText.Text);
                FilePathTextBlock2.Text = $"File saved: {filePath}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void TransformButton_Click(object sender, RoutedEventArgs e)
        {
            ImportedText.Text = ChangedText.Text;
            ChangedText.Text = "";
        }

        private void KeyTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            key = KeyTextBox.Text;
        }
    }
}
