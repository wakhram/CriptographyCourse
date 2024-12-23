using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CryptoMaster
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FirstPage());
        }

        private void TranspositionButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\TranspositionCipher\TranspositionCipher\bin\Debug\TranspositionCipher.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void VertTransButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\VerticalTransportCipher\VerticalTransportCipher\bin\Debug\VerticalTransportCipher.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void CaeserButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\CaeserCipher\CaeserCipher\bin\Debug\CaeserCipher.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void BreakCaeserButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\BreakingTheCaeserCipher\BreakingTheCaeserCipher\bin\Debug\net8.0-windows\BreakingTheCaeserCipher.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void VigenereButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\VigenereCryptoSystem\VigenereCryptoSystem\bin\Debug\net8.0-windows\VigenereCryptoSystem.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void GammaButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\GammaCipher\GammaCipher\bin\Debug\net8.0-windows\GammaCipher.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void HillButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\Hill'sCipher\Hill'sCipher\bin\Debug\net8.0-windows\Hill'sCipher.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void ElGamalButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\ElGamal\ElGamal\bin\Debug\net8.0-windows\ElGamal.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void ElGamalDSButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\ElGamalDigSignature\ElGamalDigSignature\bin\Debug\net8.0-windows\ElGamalDigSignature.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void CipherRSAButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\CipherRSA\CipherRSA\bin\Debug\net8.0-windows\CipherRSA.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

        private void AESButton_Click(object sender, RoutedEventArgs e)
        {
            string solutionPath = @"F:\bachelor\cryptography and information security\labs\CipherAES\CipherAES\bin\Debug\net8.0-windows\CipherAES.exe";

            try
            {
                // Запуск файла .sln
                Process.Start(solutionPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open solution: {ex.Message}");
            }
        }

    }
}
