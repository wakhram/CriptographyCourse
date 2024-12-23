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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;


namespace CaeserCipher
{
    public partial class MainWindow : Window
    {
        public string fileContent;
        public string fileName = "example";
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 3; i <= 23; i++)
                keyLength.Items.Add(i);

        }

        private void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

            // Show open file dialog box 
            bool? result = openFileDialog.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                fileContent = File.ReadAllText(openFileDialog.FileName);
                // Get the selected file path
                fileName = openFileDialog.SafeFileName;
                string filePath = openFileDialog.FileName;
                FilePathTextBlock.Text = filePath;  // Display the file path in the TextBlock
            }
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock.Text == "No file attached." || keyLength.SelectedItem == null)
            {
                MessageBox.Show("You have to choose file or length of key!");
            } 
            else
            {
                string encryptedText = CaesarCipherEncrypt((int)keyLength.SelectedItem);

                // Specify the file name and path
                fileName = fileName.Replace(".txt", "");
                string savedFileName = fileName + "_encrypted.txt"; // You can customize the file name

                // Combine Desktop path and the full file name
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = System.IO.Path.Combine(desktopPath, savedFileName);

                // Create and write to the file
                try
                {
                    File.WriteAllText(filePath, encryptedText);

                    // Update the TextBlock to show the saved file path
                    FilePathTextBlock2.Text = $"{filePath}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock.Text == "No file attached." || keyLength.SelectedItem == null)
            {
                MessageBox.Show("You have to choose file or length of key!");
            }
            else
            {
                string decryptedText = CaesarCipherDecrypt((int)keyLength.SelectedItem);

                // Specify the file name and path
                fileName = fileName.Replace(".txt", "");
                string savedFileName = fileName + "_decrypted.txt"; // You can customize the file name
                
                // Combine Desktop path and the full file name
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = System.IO.Path.Combine(desktopPath, savedFileName);

                // Create and write to the file
                try
                {
                    File.WriteAllText(filePath, decryptedText);

                    // Update the TextBlock to show the saved file path
                    FilePathTextBlock2.Text = $"{filePath}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        public string CaesarCipherEncrypt(int shift)
        {
            // Variable to store the encrypted text
            StringBuilder encryptedText = new StringBuilder();

            // Iterate over each character in the input text
            foreach (char c in fileContent)
            {
                if (char.IsLetter(c)) // Only process alphabetic characters
                {
                    char offset;
                    // Determine if the letter is uppercase or lowercase
                    if (char.IsUpper(c))  // If the character is uppercase
                        offset = 'A';
                    else  // If the character is lowercase
                        offset = 'a';

                    // Encrypt the character and wrap it around using modulo
                    char encryptedChar = (char)(((c + shift - offset) % 26) + offset);

                    // Append the encrypted character to the result
                    encryptedText.Append(encryptedChar);
                }
                else
                {
                    // If the character is not a letter, add it unchanged
                    encryptedText.Append(c);
                }
            }

            // Return the encrypted string
            return encryptedText.ToString();
        }

        public string CaesarCipherDecrypt(int shift)
        {
            // Variable to store the decrypted text
            StringBuilder decryptedText = new StringBuilder();

            // Iterate over each character in the input text
            foreach (char c in fileContent)
            {
                if (char.IsLetter(c)) // Only process alphabetic characters
                {
                    // Determine if the letter is uppercase or lowercase
                    char offset;

                    if (char.IsUpper(c))  // If the character is uppercase
                        offset = 'A';
                    else  // If the character is lowercase
                        offset = 'a';

                    // Decrypt the character by reversing the shift
                    char decryptedChar = (char)(((c - shift - offset + 26) % 26) + offset);

                    // Append the decrypted character to the result
                    decryptedText.Append(decryptedChar);
                }
                else
                {
                    // If the character is not a letter, add it unchanged
                    decryptedText.Append(c);
                }
            }

            // Return the decrypted string
            return decryptedText.ToString();
        }

        private void OpenFileButton1_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock.Text == "No file attached.")
            {
                MessageBox.Show("You have to import file");
            }
            else
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "notepad.exe",   // Указываем приложение (Блокнот)
                    Arguments = FilePathTextBlock.Text,       // Указываем файл для открытия
                    UseShellExecute = true      // Включаем использование оболочки Windows
                };

                Process.Start(psi);

                //Process.Start("notepad.exe", FilePathTextBlock.Text);
            }
        }

        private void OpenFileButton2_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBlock2.Text == "No file saved.")
            {
                MessageBox.Show("You have to import file");
            }
            else
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "notepad.exe",   // Указываем приложение (Блокнот)
                    Arguments = FilePathTextBlock2.Text,       // Указываем файл для открытия
                    UseShellExecute = true      // Включаем использование оболочки Windows
                };

                Process.Start(psi);

                //Process.Start("notepad.exe", FilePathTextBlock.Text);
            }
        }
    }
}
