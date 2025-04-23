/*
Zadanie zaliczeniowe z c#
Imię i nazwisko ucznia: Oliwier Wałdoch
Data wykonania zadania: 03.03.2025
Treść zadania: Bilety Kinowe
Opis funkcjonalności aplikacji: W aplikacji, po wybraniu seansu, zostaje nam przedstawiona możliwość wyboru miejsc oraz zniżki jak i późniejszej opcji zakupu odpowiednie dobranych biletów.
*/

using System.ComponentModel;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace Bilety_Kinowe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Otwieranie nowego okna z wyborem miejsc na seans
        private void front_okno(object sender, RoutedEventArgs e)
        {
            Front frontOkno = new Front();
            frontOkno.Show();
        }

        // Zapytanie przy zamknięciu aplikacji
        protected override void OnClosing(CancelEventArgs e)
        {
            var yesno = MessageBox.Show("Czy chcesz zamknąć aplikację?", "Zamykanie aplikacji", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (yesno != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}
