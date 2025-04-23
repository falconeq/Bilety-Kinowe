/*
Zadanie zaliczeniowe z c#
Imię i nazwisko ucznia: Oliwier Wałdoch
Data wykonania zadania: 03.03.2025
Treść zadania: Bilety Kinowe
Opis funkcjonalności aplikacji: W aplikacji, po wybraniu seansu, zostaje nam przedstawiona możliwość wyboru miejsc oraz zniżki jak i późniejszej opcji zakupu odpowiednie dobranych biletów.
*/


using System;
using System.Collections.Generic;
using System.Windows;

namespace Bilety_Kinowe
{
    public partial class Cart : Window
    {
        // Lista przechowująca miejsca dodane do koszyka
        public static List<Tuple<string, string>> miejscaKoszyk = new List<Tuple<string, string>>();

        public Cart()
        {
            InitializeComponent();
            zaladujMiejsca();
        }

        // Funkcja ładująca miejsca do listy widoku koszyka
        public void zaladujMiejsca()
        {
            lbCart.Items.Clear();
            // Iteracja przez wszystkie miejsca w koszyku
            foreach (var miejsce in miejscaKoszyk)
            {
                // Utworzenie tekstu miejsca z numerem i rodzajem
                string miejsceText = miejsce.Item1 + " (" + miejsce.Item2 + ")";

                // Dodanie tekstu miejsca do listy lbCart
                lbCart.Items.Add(miejsceText);
            }

            // Aktualizacja sumy do zapłaty
            aktuSuma();
        }


        // Funkcja aktualizująca sumę do zapłaty
        private void aktuSuma()
        {
            int suma = 0;
            foreach (var miejsce in miejscaKoszyk)
            {
                if (miejsce.Item2 == "Normalny")
                {
                    suma += 20;
                }
                else
                {
                    suma += 15;
                }
            }
            txtSuma.Text = "Suma: " + suma + " PLN";
        }

        // Funkcja obsługująca kliknięcie przycisku Kup Teraz
        private void kupTeraz_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy koszyk nie jest pusty
            if (miejscaKoszyk.Count == 0)
            {
                MessageBox.Show("Aby przejść do płatności, proszę wybrać miejsca!", "Wybierz miejsca", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obliczenie sumy do zapłaty
            int suma = 0;
            foreach (var miejsce in miejscaKoszyk)
            {
                if (miejsce.Item2 == "Normalny")
                {
                    suma += 20;
                }
                else
                {
                    suma += 15;
                }
            }

            // Otwarcie okna płatności
            Payment oknoPlatnosci = new Payment(suma, miejscaKoszyk);
            oknoPlatnosci.Show();
        }

        // Funkcja obsługująca opróżnianie koszyka
        private void oproznijKoszyk(object sender, RoutedEventArgs e)
        {
            lbCart.Items.Clear();
            miejscaKoszyk.Clear();
            txtSuma.Text = "Suma: 0 PLN";
        }
    }
}