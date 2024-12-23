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

namespace TranspositionCipher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        RandomLetterGenerator generator = new RandomLetterGenerator();
        private int[] key;
        
        public MainWindow()
        {
            InitializeComponent();
            myTxtbx.GotFocus += RemoveText;

            for (int i = 4; i <= 10; i++)
            {
                KeyLength.Items.Add(i);
            }



        }

        private string ChangingCapitalLetters(string text)
        {
            char[] tempResult = text.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsUpper(text[i]))
                {
                    tempResult[i] = Char.ToLower(text[i]);
                }
            }

            string result = new string(tempResult);
            return result.Replace(" ", "");
        }


        static int[] ShuffleArray(int[] array)
        {
            Random random = new Random();
            int n = array.Length;

            // Алгоритм Фишера-Йетса
            for (int i = n - 1; i > 0; i--)
            {
                // Выбираем случайный индекс от 0 до i
                int j = random.Next(0, i + 1);

                // Меняем местами элементы array[i] и array[j]
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return array;
        }

        private string EncryptFunc(string plainText, int kL)
        {

            StringBuilder cipherText = new StringBuilder();

            for (int i = 0; i < plainText.Length; i += kL)
            {
                string chunk = plainText.Substring(i, kL);

                char[] encryptedChunk = new char[kL];

                for (int j = 0; j < chunk.Length; j++)
                {
                    int newPosition = key[j]; // Convert 1-based index to 0-based
                    encryptedChunk[newPosition] = chunk[j];
                }

                cipherText.Append(new string(encryptedChunk));
            }

            return cipherText.ToString();
        }

        private string DecryptFunc(string cipherText, int kL)
        {
            // Создаём обратный ключ
            int[] reverseKey = new int[kL];
            for (int i = 0; i < kL; i++)
            {
                reverseKey[key[i]] = i;
            }

            StringBuilder plainText = new StringBuilder();

            // Расшифровываем текст
            for (int i = 0; i < cipherText.Length; i += kL)
            {
                string chunk = cipherText.Substring(i, kL);
                char[] decryptedChunk = new char[kL];

                for (int j = 0; j < chunk.Length; j++)
                {
                    int originalPosition = reverseKey[j];
                    decryptedChunk[originalPosition] = chunk[j];
                }

                plainText.Append(new string(decryptedChunk));
            }

            return plainText.ToString();
        }

        private void EncryptClick(object sender, RoutedEventArgs e)
        {
            if (myTxtbx.Text == "" || myTxtbx.Text == "Enter text..." || myTxtbx.Text.Length < 3)
            {
                MessageBox.Show("Writing text with at least 3 letters is required!");
            }
            else if (KeyLength.SelectedIndex == -1)
            {
                MessageBox.Show("Selecting key length is required!");
            }
            else
            {
                string plainText = ChangingCapitalLetters(myTxtbx.Text);
                int keyLength = (int)KeyLength.SelectedItem;
                string cipherText = "";
                if (plainText.Length % keyLength == 0)
                {
                    cipherText = EncryptFunc(plainText, keyLength);
                }
                else
                {
                    int n = plainText.Length % keyLength;
                    for (int i = 0; i < (keyLength - n); i++)
                    {
                        plainText += generator.RandomLetter();
                    }

                    cipherText = EncryptFunc(plainText, keyLength);
                }

                ResultTextLabel.Content = cipherText;
            }
        }

        private void DecryptClick(object sender, RoutedEventArgs e)
        {
            if (myTxtbx.Text == "" || myTxtbx.Text == "Enter text..." || myTxtbx.Text.Length < 3)
            {
                MessageBox.Show("Writing text with at least 3 letters is required!");
            }
            else if (KeyLength.SelectedIndex == -1)
            {
                MessageBox.Show("Selecting key length is required!");
            }
            else
            {
                int keyLength = (int)KeyLength.SelectedItem;
                string plainText = DecryptFunc(myTxtbx.Text, keyLength);
                ResultTextLabel.Content = plainText;
            }
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (myTxtbx.Text == "Enter text...")
            {
                myTxtbx.Text = "";
            }
        }

        private void KeyLength_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int keyLength = (int)KeyLength.SelectedItem;
            key = new int[keyLength];
            
            for (int i = 0; i < keyLength; i++)
                key[i] = i;

            key = ShuffleArray(key);

            for (int i = 0; i < keyLength; i++)
                Console.WriteLine(key[i]);
        }

        private void TransportClick(object sender, RoutedEventArgs e)
        {
            myTxtbx.Text = ResultTextLabel.Content.ToString();
            ResultTextLabel.Content = "";
        }
    }
}

public class RandomLetterGenerator
{
    private static Random random = new Random();

    public string RandomLetter()
    {
        int randomNumber = random.Next(0, 26); // 26 is exclusive
        char randomLetter = (char)('a' + randomNumber);
        return randomLetter.ToString();
    }
}
