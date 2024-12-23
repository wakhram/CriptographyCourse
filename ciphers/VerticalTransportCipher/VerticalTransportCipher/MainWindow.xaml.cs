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

namespace VerticalTransportCipher
{
    public partial class MainWindow : Window
    {
        RandomLetterGenerator generator = new RandomLetterGenerator();
        private int[] key;
        private char[,] matrix = null;
        private int keyLength;

        public MainWindow()
        {
            InitializeComponent();
            myTxtbx.GotFocus += RemoveText;
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

            for (int i = 0; i < kL; i++)
                for (int j = 0; j < kL; j++)
                    cipherText.Append((matrix[j, key[i]]));

            return cipherText.ToString();
        }

        private string DecryptFunc(string cipherText, int kL)
        {
            StringBuilder plainText = new StringBuilder();

            for (int i = 0; i < kL; i++)
                for (int j = 0; j < kL; j++)
                    plainText.Append((matrix[i, j]));

            return plainText.ToString();
        }

        private void EncryptClick(object sender, RoutedEventArgs e)
        {
            if (myTxtbx.Text == "" || myTxtbx.Text == "Enter text..." || myTxtbx.Text.Length < 3)
            {
                MessageBox.Show("Writing text with at least 3 letters is required!");
            }
            else
            {
                string plainText = ChangingCapitalLetters(myTxtbx.Text);
                AddTextToTable(plainText);
                string cipherText = "";
                cipherText = EncryptFunc(plainText, keyLength);
                ResultTextLabel.Content = cipherText;
            }
        }

        private void DecryptClick(object sender, RoutedEventArgs e)
        {
            if (myTxtbx.Text == "" || myTxtbx.Text == "Enter text..." || myTxtbx.Text.Length < 3)
            {
                MessageBox.Show("Writing text with at least 3 letters is required!");
            }
            else
            {
                string plainText = DecryptFunc(myTxtbx.Text, keyLength);
                ResultTextLabel.Content = plainText;
            }
        }

        private void AddTextToTable(string text)
        {
            int length = text.Length;
            int size = (int)Math.Ceiling(Math.Sqrt(length));

            keyLength = size;
            key = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
                key[i] = i;

            key = ShuffleArray(key);

            for (int i = 0; i < keyLength; i++)
                Console.WriteLine(key[i]);

            matrix = new char[size, size];

            int charIndex = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (charIndex < text.Length)
                    {
                        matrix[i, j] = text[charIndex];
                        charIndex++;
                    }
                    else
                    {
                        matrix[i, j] = generator.RandomLetter(); // Заполняем пробелами, если символов не хватает
                    }
                }
            }
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (myTxtbx.Text == "Enter text...")
            {
                myTxtbx.Text = "";
            }
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

    public char RandomLetter()
    {
        int randomNumber = random.Next(0, 26); // 26 is exclusive
        char randomLetter = (char)('a' + randomNumber);
        return randomLetter;
    }
}
