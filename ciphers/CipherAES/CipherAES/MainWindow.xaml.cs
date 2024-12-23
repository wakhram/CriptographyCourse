using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Numerics;
using System.Xml.Linq;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;

namespace CipherAES
{
    public partial class MainWindow : Window
    {
        string fileContent = "";
        string fileName = "";
        private static readonly Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            ImportedText.Text = "Here will be imported text...";
            ImportedText.Foreground = Brushes.Gray;
            ResultText.Text = "Here will be result text...";
            ResultText.Foreground = Brushes.Gray;
            Key.Text = "Enter key...";
            Key.Foreground = Brushes.Gray;
        }

        // Основная таблица S-box для AES
        readonly static byte[] SBox = new byte[256]
        {
            0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76,
            0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0,
            0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15,
            0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75,
            0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84,
            0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF,
            0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8,
            0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2,
            0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73,
            0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB,
            0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79,
            0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08,
            0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A,
            0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E,
            0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF,
            0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16
        };

        // Константы раундов
        readonly static byte[] InvSBox = new byte[256]
        {
            0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB,
            0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB,
            0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E,
            0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25,
            0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92,
            0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84,
            0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06,
            0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B,
            0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73,
            0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E,
            0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B,
            0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4,
            0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F,
            0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF,
            0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D
        };

        private byte[,] ExpandKey(byte[,] keyMatrix)
        {
            // Rcon (раундовая константа)
            byte[] Rcon = new byte[10] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1B, 0x36 };

            // Конвертируем ключ в одномерный массив для удобства работы
            byte[] expandedKey = new byte[176]; // 11 раундов * 16 байт
            for (int i = 0; i < 16; i++)
            {
                expandedKey[i] = keyMatrix[i / 4, i % 4];
            }

            // Генерируем остальные 160 байт
            int bytesGenerated = 16; // Первые 16 байт уже добавлены
            int rconIndex = 0;

            byte[] temp = new byte[4]; // Для хранения текущего слова
            while (bytesGenerated < 176)
            {
                // Получаем предыдущее слово
                for (int i = 0; i < 4; i++)
                {
                    temp[i] = expandedKey[bytesGenerated - 4 + i];
                }

                // Каждое 4-е слово
                if (bytesGenerated % 16 == 0)
                {
                    temp = RotWord(temp);
                    temp = SubWord(temp);

                    // XOR с Rcon
                    temp[0] ^= Rcon[rconIndex];
                    rconIndex++;
                }

                // XOR с соответствующим словом из предыдущего ключа
                for (int i = 0; i < 4; i++)
                {
                    expandedKey[bytesGenerated] = (byte)(expandedKey[bytesGenerated - 16] ^ temp[i]);
                    bytesGenerated++;
                }
            }

            // Конвертируем в массив 4x44 (44 столбца по 4 байта)
            byte[,] expandedKeyMatrix = new byte[4, 44];
            for (int col = 0; col < 44; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    expandedKeyMatrix[row, col] = expandedKey[col * 4 + row];
                }
            }

            return expandedKeyMatrix;
        }

        // Вспомогательные функции
        private byte[] RotWord(byte[] word)
        {
            return new byte[4] { word[1], word[2], word[3], word[0] };
        }

        private byte[] SubWord(byte[] word)
        {
            for (int i = 0; i < 4; i++)
            {
                int index = (word[i] >> 4) * 16 + (word[i] & 0x0F);
                word[i] = SBox[index];
            }
            return word;
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

        public List<byte[,]> PrepareTextToEncrypt(byte[] bytes)
        {
            // Проверяем, делится ли длина на 16
            int remainder = bytes.Length % 16;
            if (remainder != 0)
            {
                // Если нет, дополняем массив до ближайшего числа, кратного 16
                int paddingLength = 16 - remainder;
                Array.Resize(ref bytes, bytes.Length + paddingLength);

                // Заполняем оставшиеся байты, например, нулями
                for (int i = bytes.Length - paddingLength; i < bytes.Length; i++)
                {
                    bytes[i] = 0x00; // Здесь можно использовать другой символ, например, 0x20 (пробел)
                }
            }

            // Создаем список для хранения матриц 4x4
            List<byte[,]> blocks = new List<byte[,]>();

            // Разбиваем байты на блоки по 16 и преобразуем в матрицы
            for (int i = 0; i < bytes.Length; i += 16)
            {
                byte[,] matrix = new byte[4, 4];
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        matrix[row, col] = bytes[i + row * 4 + col];
                    }
                }
                blocks.Add(matrix);
            }

            // Возвращаем список матриц
            return blocks;
        }

        public byte[,] ConvertKeyToMatrix(byte[] keyBytes)
        {
            if (keyBytes.Length < 16)
            {
                byte[] paddedKey = new byte[16];
                Array.Copy(keyBytes, paddedKey, keyBytes.Length);
                keyBytes = paddedKey;
            }
            else if (keyBytes.Length > 16)
            {
                throw new ArgumentException("Key must not be longer than 16 bytes.");
            }


            // Создаем матрицу 4x4 для ключа
            byte[,] keyMatrix = new byte[4, 4];
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    keyMatrix[row, col] = keyBytes[row * 4 + col];
                }
            }

            return keyMatrix;
        }

        private void AddRoundKey(byte[,] state, byte[,] expandedKey, int round)
        {
            // Каждый раунд использует 4 столбца (16 байт) из расширенного ключа
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] ^= expandedKey[row, round * 4 + col]; // Применяем XOR с соответствующими байтами из раундового ключа
                }
            }
        }

        private void SubBytes(byte[,] state)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    byte value = state[row, col];
                    int index = (value >> 4) * 16 + (value & 0x0F); // Вычисление индекса для SBox
                    state[row, col] = SBox[index];
                }
            }
        }


        private void ShiftRows(byte[,] state)
        {
            for (int row = 1; row < 4; row++)
            {
                byte[] temp = new byte[4];
                for (int col = 0; col < 4; col++)
                {
                    temp[col] = state[row, (col + row) % 4];
                }
                // Копируем обратно в двумерный массив, указывая правильные индексы
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = temp[col];
                }
            }
        }

        private void MixColumns(byte[,] state)
        {
            for (int col = 0; col < 4; col++)
            {
                byte a0 = state[0, col], a1 = state[1, col];
                byte a2 = state[2, col], a3 = state[3, col];

                state[0, col] = (byte)(GFMultiply(a0, 2) ^ GFMultiply(a1, 3) ^ a2 ^ a3);
                state[1, col] = (byte)(a0 ^ GFMultiply(a1, 2) ^ GFMultiply(a2, 3) ^ a3);
                state[2, col] = (byte)(a0 ^ a1 ^ GFMultiply(a2, 2) ^ GFMultiply(a3, 3));
                state[3, col] = (byte)(GFMultiply(a0, 3) ^ a1 ^ a2 ^ GFMultiply(a3, 2));
            }
        }

        // Умножение в поле Галуа
        private byte GFMultiply(byte a, byte b)
        {
            byte result = 0;
            byte temp = a;

            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) == 1) result ^= temp;
                bool highBit = (temp & 0x80) != 0;
                temp <<= 1;
                if (highBit) temp ^= 0x1B; // Полином x^8 + x^4 + x^3 + x + 1
                b >>= 1;
            }

            return result;
        }

        public string ConvertResultToText(List<byte[,]> result)
        {
            // Определяем общий список для хранения всех байтов
            List<byte> allBytes = new List<byte>();

            // Обрабатываем каждый блок 4x4
            foreach (var block in result)
            {
                // Конвертируем блок 4x4 в одномерный массив
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        allBytes.Add(block[row, col]);
                    }
                }
            }

            // Преобразуем список байтов в массив
            byte[] byteArray = allBytes.ToArray();

            // Преобразуем байты в строку
            return Encoding.UTF8.GetString(byteArray); // Убираем добавленные нули, если они есть
        }

        private byte[,] EncryptBlock(byte[,] state, byte[,] expandedKey)
        {
            // Начальный раунд
            AddRoundKey(state, expandedKey, 0);

            // Основные раунды
            for (int round = 1; round <= 9; round++)
            {
                SubBytes(state);
                ShiftRows(state);
                MixColumns(state);
                AddRoundKey(state, expandedKey, round);
            }

            // Финальный раунд
            SubBytes(state);
            ShiftRows(state);
            AddRoundKey(state, expandedKey, 10);

            return state;
        }

        private bool IsValidBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string ToBase64(string input)
        {
            if (IsValidBase64(input))
                return input;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        private string EncryptText(string plainText, string key)
        {
            // Преобразуем текст и ключ в Base64
            string plainTextBase64 = ToBase64(plainText);
            string keyBase64 = ToBase64(key);

            // Декодируем из Base64
            byte[] plainTextBytes = Convert.FromBase64String(plainTextBase64);
            byte[] keyBytes = Convert.FromBase64String(keyBase64);

            // Преобразуем ключ в матрицу
            byte[,] keyMatrix = ConvertKeyToMatrix(keyBytes);

            // Подготавливаем текст для шифрования
            List<byte[,]> blocks = PrepareTextToEncrypt(plainTextBytes);

            // Расширяем ключ
            byte[,] expandedKeys = ExpandKey(keyMatrix);

            // Шифруем блоки
            List<byte> encryptedBytes = new List<byte>();
            foreach (var block in blocks)
            {
                byte[,] state = EncryptBlock(block, expandedKeys);
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        encryptedBytes.Add(state[row, col]);
                    }
                }
            }

            // Конвертируем зашифрованные байты обратно в Base64
            return Convert.ToBase64String(encryptedBytes.ToArray());
        }

        public List<byte[,]> PrepareTextToDecrypt(string encryptedText)
        {
            MessageBox.Show("Hello", "!");

            // Преобразуем зашифрованный текст в массив байтов
            byte[] bytes = Encoding.UTF8.GetBytes(encryptedText);

            // Создаем список для хранения матриц 4x4
            List<byte[,]> blocks = new List<byte[,]>();

            // Разбиваем байты на блоки по 16 и преобразуем в матрицы
            for (int i = 0; i < bytes.Length; i += 16)
            {
                byte[,] matrix = new byte[4, 4];
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        matrix[row, col] = bytes[i + row * 4 + col];
                    }
                }
                blocks.Add(matrix);
            }

            // Возвращаем список матриц
            return blocks;
        }

        private void InvSubBytes(byte[,] state)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    byte value = state[row, col];
                    int index = (value >> 4) * 16 + (value & 0x0F); // Используем инверсный SBox
                    state[row, col] = InvSBox[index];
                }
            }
        }

        private void InvShiftRows(byte[,] state)
        {
            // В отличие от обычного сдвига, здесь сдвигаем влево
            for (int row = 1; row < 4; row++)
            {
                byte[] temp = new byte[4];
                for (int col = 0; col < 4; col++)
                {
                    temp[col] = state[row, (col - row + 4) % 4]; // Сдвигаем влево
                }
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = temp[col];
                }
            }
        }

        private void InvMixColumns(byte[,] state)
        {
            for (int col = 0; col < 4; col++)
            {
                byte a0 = state[0, col], a1 = state[1, col];
                byte a2 = state[2, col], a3 = state[3, col];

                state[0, col] = (byte)(GFMultiply(a0, 0x0E) ^ GFMultiply(a1, 0x0B) ^ GFMultiply(a2, 0x0D) ^ GFMultiply(a3, 0x09));
                state[1, col] = (byte)(GFMultiply(a0, 0x09) ^ GFMultiply(a1, 0x0E) ^ GFMultiply(a2, 0x0B) ^ GFMultiply(a3, 0x0D));
                state[2, col] = (byte)(GFMultiply(a0, 0x0D) ^ GFMultiply(a1, 0x09) ^ GFMultiply(a2, 0x0E) ^ GFMultiply(a3, 0x0B));
                state[3, col] = (byte)(GFMultiply(a0, 0x0B) ^ GFMultiply(a1, 0x0D) ^ GFMultiply(a2, 0x09) ^ GFMultiply(a3, 0x0E));
            }
        }

        private byte[,] DecryptBlock(byte[,] state, byte[,] expandedKey)
        {
            // Начальный раунд: применяем AddRoundKey с ключом для последнего раунда
            AddRoundKey(state, expandedKey, 10);
            InvShiftRows(state);
            InvSubBytes(state);

            // Основные раунды (обратные операции)
            for (int round = 9; round >= 1; round--)
            {
                AddRoundKey(state, expandedKey, round);
                InvMixColumns(state);
                InvShiftRows(state);
                InvSubBytes(state);
            }

            // Финальный раунд
            AddRoundKey(state, expandedKey, 0);

            return state;
        }

        private string DecryptText(string encryptedText, string key)
        {
            // Преобразуем ключ в Base64
            string keyBase64 = ToBase64(key);

            // Проверяем зашифрованный текст
            if (!IsValidBase64(encryptedText))
                throw new ArgumentException("The encrypted text is not a valid Base64 string.");

            // Декодируем из Base64
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Convert.FromBase64String(keyBase64);

            // Преобразуем ключ в матрицу
            byte[,] keyMatrix = ConvertKeyToMatrix(keyBytes);

            // Расширяем ключ
            byte[,] expandedKeys = ExpandKey(keyMatrix);

            // Разбиваем зашифрованные байты на блоки
            List<byte[,]> blocks = new List<byte[,]>();
            for (int i = 0; i < encryptedBytes.Length; i += 16)
            {
                byte[,] block = new byte[4, 4];
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        block[row, col] = encryptedBytes[i + col + row * 4];
                    }
                }
                blocks.Add(block);
            }

            // Дешифруем блоки
            List<byte> decryptedBytes = new List<byte>();
            foreach (var block in blocks)
            {
                byte[,] state = DecryptBlock(block, expandedKeys);
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        decryptedBytes.Add(state[row, col]);
                    }
                }
            }

            // Преобразуем расшифрованные байты в строку и убираем padding
            return Encoding.UTF8.GetString(decryptedBytes.ToArray()).TrimEnd('\0');
        }

        // Событие для кнопки Encrypt
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckKey(Key.Text))
                {
                    MessageBox.Show("Please enter key with 16 symbols.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string textToEncrypt = ImportedText.Text; // Текст для шифрования
                if (string.IsNullOrWhiteSpace(textToEncrypt))
                {
                    MessageBox.Show("Please enter text to encrypt.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string encryptedText = EncryptText(textToEncrypt, Key.Text);
                ResultText.Text = encryptedText;
                ResultText.Foreground = Brushes.Black;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during encryption: {ex.Message}", "Encryption Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Событие для кнопки Decrypt
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckKey(Key.Text))
                {
                    MessageBox.Show("Please enter key with 16 symbols.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string textToDecrypt = ImportedText.Text; // Текст для дешифрования
                if (string.IsNullOrWhiteSpace(textToDecrypt))
                {
                    MessageBox.Show("Please enter encrypted text to decrypt.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string decryptedText = DecryptText(textToDecrypt, Key.Text);
                ResultText.Text = decryptedText;
                ResultText.Foreground = Brushes.Black;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during decryption: {ex.Message}", "Decryption Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CheckKey(string k)
        {
            if (k.Length == 16)
                return true;
            return false;
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            ImportedText.Text = ResultText.Text;
            ResultText.Text = "";
        }

        private void InputTextBox_GotFocus1(object sender, RoutedEventArgs e)
        {
            if (Key.Text == "Enter key...")
            {
                Key.Text = "";
                Key.Foreground = Brushes.Black;
            }
        }

        private void InputTextBox_LostFocus1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Key.Text))
            {
                Key.Text = "Enter key...";
                Key.Foreground = Brushes.Gray;
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

    }
}