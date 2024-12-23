using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Windows.Controls;
using System.Numerics;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace ElGamalDigSignature
{
    public partial class MainWindow : Window
    {
        private static readonly Random random = new Random();

        // plain keys
        long y, g, p;
        // close key
        long x;
        //temporary key
        long tempKey;

        //signature
        long S1, S2;
        
        // verify
        double V1, V2;

        // hash message
        long m;
        public MainWindow()
        {
            InitializeComponent();

        }

        private static bool IsPrime(long number)
        {
            if (number < 2) return false;
            if (number % 2 == 0) return number == 2;

            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        public long GetSimpleNumber(long min, long max)
        {
            Random random = new Random();
            long number;

            do
            {
                number = random.NextInt64(min, max); // Генерируем случайное число в диапазоне
                if (number % 2 == 0) number += 1; // Делаем нечетным
            } while (!IsPrime(number));

            return number;
        }

        public long GetRandNumber(long min, long max)
        {
            Random random = new Random();
            return random.NextInt64(min, max);
        }

        public static long GeneratePrimitiveRoot(long p)
        {
            // Ensure the input is a prime number
            if (!IsPrime(p)) throw new ArgumentException("Input must be a prime number.");

            // Store all prime factors of p-1
            long phi = p - 1;
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
        private static long ModularExponentiation(long baseValue, long exp, long mod)
        {
            long result = 1;
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

        private long Exponentiation(long baseValue, long exp)
        {
            long result = 1;
            long baseLong = baseValue; // Чтобы избежать переполнения при больших значениях

            while (exp > 0)
            {
                // Если текущая степень нечётная, умножаем результат на основание
                if ((exp & 1) == 1)
                {
                    result *= baseLong;
                }

                // Увеличиваем базу за счёт её возведения в квадрат
                baseLong *= baseLong;
                exp >>= 1; // Делим exp на 2 (сдвиг вправо)
            }

            return result;
        }


        // Helper: Find all prime factors of a number
        private static List<long> GetPrimeFactors(long n)
        {
            var factors = new List<long>();
            for (long i = 2; i * i <= n; i++)
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
                    p = 30803;
                    g = 2;
                    //GetSimpleNumber(200, 60000);
                    //GeneratePrimitiveRoot(p);

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

        public long GetRandomNumberWithGCD(long lowerBound, long upperBound)
        {
            Random rand = new Random();
            long x;

            do
            {
                ; x = rand.NextInt64(lowerBound + 1, upperBound);
            }
            while (GCD(x, upperBound) != 1);

            return x;
        }

        private void GenerateKeysButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                keyGeneration(); // Generate keys with 256-bit prime
                PublicKeyTextBox.Text = $"p: {p}, g: {g}, y: {y}";
                VerificationResultTextBox.Text = "Keys generated successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating keys: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Convert message to BigInteger (hash simulation)
                string message = MessageTextBox.Text;

                SignFunc(message);
                SignatureTextBox.Text = $"S1: {S1}, S2: {S2}, m: {m}";
                VerificationResultTextBox.Text = "Message signed successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error signing message: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Verify Signature
        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse input
                string message = MessageTextBox.Text;

                // Verify signature
                bool isValid = VerifyFunc(message);
                VerificationTextBox.Text = $"V1: {V1}, V2: {V2}";
                VerificationResultTextBox.Text = isValid ? "Signature is valid." : "Signature is invalid.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error verifying signature: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public long HashToLong(string input)
        {
            // Use SHA256 to compute the hash
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert input string to byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the first 8 bytes of the hash to a long
                long hashLong = BitConverter.ToInt64(hashBytes, 0);

                return Math.Abs(hashLong); // Ensure a positive long number
            }
        }

        public void SignFunc(string plainText)
        {
            keyGeneration();
            m = HashToLong(plainText);  // Хеширование сообщения (например, SHA-256)

            tempKey = GetRandomNumberWithGCD(1, p - 1);
            S1 = ModularExponentiation(g, tempKey, p);
            long kInversed = ModularExponentiation(tempKey, -1, p - 1);
            S2 = ModularExponentiation((kInversed * (m - x * S1)) % (p - 1), 1, p - 1);
        }

        public bool VerifyFunc(string plainText)
        {
            m = HashToLong(plainText);  // Хеширование сообщения для верификации

            V1 = ModularExponentiation(g, m, p);

            long tempV2_1 = ModularExponentiation(y, S1, p);
            long tempV2_2 = ModularExponentiation(g, S2, p);
            V2 = (tempV2_1 * tempV2_2) % p;

            return V1 == V2;
        }


    }
}
