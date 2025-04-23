/*
Zadanie zaliczeniowe z c#
Imię i nazwisko ucznia: Oliwier Wałdoch
Data wykonania zadania: 03.03.2025
Treść zadania: Bilety Kinowe
Opis funkcjonalności aplikacji: W aplikacji, po wybraniu seansu, zostaje nam przedstawiona możliwość wyboru miejsc oraz zniżki jak i późniejszej opcji zakupu odpowiednie dobranych biletów.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Bilety_Kinowe
{
    public partial class Payment : Window
    {
        // Zmienna przechowująca sumę do zapłaty
        private int suma;

        // Lista przechowująca wybrane miejsca
        private List<Tuple<string, string>> wybraneMiejsca;

        // Czy powiodła się płatność
        private bool czyUdana = false;

        public Payment(int suma, List<Tuple<string, string>> wybraneMiejsca)
        {
            InitializeComponent();
            this.suma = suma;
            this.wybraneMiejsca = wybraneMiejsca;
            txtSuma.Text = "Suma: " + suma + " PLN";
            txtMiejsca.Text = stworzListe();
        }

        // Tworzy listę wybranych miejsc w formie string
        private string stworzListe()
        {
            // Inicjalizacja listy miejsc
            List<string> miejscaLista = new List<string>();

            // Dodaje miejsca do listy
            foreach (var miejsce in wybraneMiejsca)
            {
                string miejsceString = miejsce.Item1 + " (" + miejsce.Item2 + ")";
                miejscaLista.Add(miejsceString);
            }

            // Łączy listę w jeden string
            return string.Join(", ", miejscaLista);
        }


        // Przycisk przerwania płatności
        private void przerwijPlatnosc_Click(object sender, RoutedEventArgs e)
        {
            if (sprawdzRegex())
            {
                czyUdana = true;
                MessageBox.Show("Płatność zakończona sukcesem!", "Udana płatność", MessageBoxButton.OK, MessageBoxImage.Information);

                // Tworzy listę zajętych miejsc
                List<string> zajeteMiejsca = new List<string>();
                foreach (var miejsce in wybraneMiejsca)
                {
                    zajeteMiejsca.Add(miejsce.Item1);
                }

                // Pobiera okno Front i zaznacza zajęte miejsca
                Front oknoFront = Application.Current.Windows.OfType<Front>().FirstOrDefault();
                if (oknoFront != null)
                {
                    oknoFront.zaznaczZajete(zajeteMiejsca);
                }

                // Usuwa zajęte miejsca z koszyka
                for (int i = Cart.miejscaKoszyk.Count - 1; i >= 0; i--)
                {
                    if (zajeteMiejsca.Contains(Cart.miejscaKoszyk[i].Item1))
                    {
                        Cart.miejscaKoszyk.RemoveAt(i);
                    }
                }

                // Pobiera okno Cart i odświeża widok koszyka
                Cart oknoCart = Application.Current.Windows.OfType<Cart>().FirstOrDefault();
                if (oknoCart != null)
                {
                    oknoCart.zaladujMiejsca();
                }

                // Zamknięcie Payment
                Close();
            }
            else
            {
                MessageBox.Show("Proszę wypełnić wszystkie pola poprawnie.", "Wypełnij pola", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Funkcja sprawdzająca poprawność wprowadzonych danych (Regexy)
        private bool sprawdzRegex()
        {
            bool czyPoprawne = true;

            if (string.IsNullOrWhiteSpace(txtImie.Text))
            {
                czyPoprawne = false;
            }
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                czyPoprawne = false;
            }
            if (!Regex.IsMatch(txtTel.Text, @"^\+?\d{9,15}$"))
            {
                czyPoprawne = false;
            }
            if (!Regex.IsMatch(txtNum.Text, @"^\d{16}$"))
            {
                czyPoprawne = false;
            }
            if (!Regex.IsMatch(txtData.Text, @"^(0[1-9]|1[0-2])\/([0-9]{2})$"))
            {
                czyPoprawne = false;
            }
            if (!Regex.IsMatch(txtCVV.Text, @"^\d{3,4}$"))
            {
                czyPoprawne = false;
            }

            return czyPoprawne;
        }

        // Funkcja obsługująca zamykanie okna
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!czyUdana)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz przerwać płatność?", "Przerywanie płatności", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }
    }
}
