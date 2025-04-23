/*
Zadanie zaliczeniowe z c#
Imię i nazwisko ucznia: Oliwier Wałdoch
Data wykonania zadania: 03.03.2025
Treść zadania: Bilety Kinowe
Opis funkcjonalności aplikacji: W aplikacji, po wybraniu seansu, zostaje nam przedstawiona możliwość wyboru miejsc oraz zniżki jak i późniejszej opcji zakupu odpowiednie dobranych biletów.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bilety_Kinowe
{
    public partial class Front : Window
    {
        // Ceny biletów normalnych i ulgowych
        private const int cenaNormalny = 20;
        private const int cenaUlgowy = 15;

        // Listy przechowujące wybrane miejsca i zajęte miejsca
        private List<Tuple<string, string>> wybraneMiejsca = new List<Tuple<string, string>>();
        private List<string> zajeteMiejsca = new List<string>();

        public Front()
        {
            InitializeComponent();
            zaladujZajete();
        }

        // Funkcja obsługująca kliknięcie przycisku miejsca
        private void miejsce_clicked(object sender, RoutedEventArgs e)
        {
            // Przekształcenie sender na Button
            Button btn = (Button)sender;

            // Sprawdzenie czy miejsce jest zajęte
            if (btn.Background == Brushes.Black) return;

            // Ustalanie rodzaju miejsca (zniżka)
            string rodzajMiejsca = rbNormalny.IsChecked == true ? "Normalny" : "Ulgowy";
            Tuple<string, string> miejsce = new Tuple<string, string>(btn.Content.ToString(), rodzajMiejsca);

            // Sprawdzanie czy miejsce jest już wybrane
            if (btn.Background == Brushes.Green)
            {
                // Usunięcie miejsca z listy wybranych miejsc i zmiana koloru na szary
                btn.Background = Brushes.LightGray;
                for (int i = 0; i < wybraneMiejsca.Count; i++)
                {
                    if (wybraneMiejsca[i].Item1 == btn.Content.ToString())
                    {
                        wybraneMiejsca.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                // Dodanie miejsca do listy wybranych i zmiana koloru na zielony
                btn.Background = Brushes.Green;
                // Usunięcie miejsca z listy wybranych jeśli już tam jest
                for (int i = 0; i < wybraneMiejsca.Count; i++)
                {
                    if (wybraneMiejsca[i].Item1 == btn.Content.ToString())
                    {
                        wybraneMiejsca.RemoveAt(i);
                        break;
                    }
                }

                wybraneMiejsca.Add(miejsce);
            }
            aktuInterfejs();
        }

        // Funkcja aktualizująca interfejs
        private void aktuInterfejs()
        {
            // Aktualizuje listę wybranych miejsc
            if (wybraneMiejsca.Count > 0)
            {
                // Tworzy liste tekstową miejsc
                List<string> miejscaStringList = new List<string>();
                foreach (var miejsce in wybraneMiejsca)
                {
                    string miejsceString = miejsce.Item1 + " (" + miejsce.Item2 + ")";
                    miejscaStringList.Add(miejsceString);
                }
                // Ustawia tekst wybranych miejsc
                txtMiejsca.Text = string.Join(", ", miejscaStringList);
            }
            else
            {
                txtMiejsca.Text = "Brak";
            }


            // Obliczenie sumy do zapłaty
            int suma = 0;
            foreach (var miejsce in wybraneMiejsca)
            {
                if (miejsce.Item2 == "Normalny")
                {
                    suma += cenaNormalny;
                }
                else
                {
                    suma += cenaUlgowy;
                }
            }
            txtSuma.Text = "Suma: " + suma + " PLN";
        }


        // Funkcja obsługująca dodawanie wybranych miejsc do koszyka
        private void dodajKoszyk(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy wybrano miejsca
            if (wybraneMiejsca.Count == 0)
            {
                MessageBox.Show("Aby dodać do koszyka, proszę wybrać miejsca!", "Wybierz miejsca", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Iteracja przez wybrane miejsca
            foreach (var miejsce in wybraneMiejsca)
            {
                bool miejsceJuzDodane = false;

                // Sprawdzenie czy miejsce jest już w koszyku
                for (int i = 0; i < Cart.miejscaKoszyk.Count; i++)
                {
                    if (Cart.miejscaKoszyk[i].Item1 == miejsce.Item1)
                    {
                        miejsceJuzDodane = true;
                        break;
                    }
                }

                // Dodanie miejsca do koszyka jeśli nie było wcześniej dodane
                if (!miejsceJuzDodane)
                {
                    resetMiejsca(miejsce.Item1);
                    Cart.miejscaKoszyk.Add(miejsce);
                }
            }
            wybraneMiejsca.Clear();
            aktuInterfejs();
        }

        // Funkcja otwierająca okno koszyka
        private void cart_okno(object sender, RoutedEventArgs e)
        {
            Cart cartOkno = new Cart();
            cartOkno.Show();
        }

        // Funkcja obsługująca przejście do okna płatności
        private void payment_okno(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy wybrano miejsca
            if (wybraneMiejsca.Count == 0)
            {
                MessageBox.Show("Aby przejść do płatności, proszę wybrać miejsca!", "Wybierz miejsca", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obliczenie sumy
            int suma = 0;
            foreach (var miejsce in wybraneMiejsca)
            {
                if (miejsce.Item2 == "Normalny")
                {
                    suma += cenaNormalny;
                }
                else
                {
                    suma += cenaUlgowy;
                }
            }

            // Otwarcie okna płatności
            Payment paymentOkno = new Payment(suma, wybraneMiejsca);
            paymentOkno.Show();

            // Reset wybranych miejsc i aktualizacja interfejsu
            foreach (var miejsce in wybraneMiejsca)
            {
                resetMiejsca(miejsce.Item1);
            }
            wybraneMiejsca.Clear();
            aktuInterfejs();
        }

        // Funckja Zaznacza zajęte miejsca
        public void zaznaczZajete(List<string> miejsca)
        {
            // Szuka przycisków odpowiadających miejscom
            foreach (var panel in ((Panel)Content).Children.OfType<StackPanel>())
            {
                foreach (var child in panel.Children.OfType<Button>())
                {
                    // Ustawia kolor tła na czarny jeśli zajęte
                    if (miejsca.Contains(child.Content.ToString()))
                    {
                        child.Background = Brushes.Black;

                        // Dodaje do listy zajętych miejsc
                        if (!zajeteMiejsca.Contains(child.Content.ToString()))
                        {
                            zajeteMiejsca.Add(child.Content.ToString());
                        }
                    }
                }
            }
            // Zapisuje zajęte miejsca
            zapiszZajete();
        }


        // Resetuje kolor miejsca na szary
        private void resetMiejsca(string miejsce)
        {
            // Szuka przycisku odpowiadającego miejscu
            foreach (var panel in ((Panel)Content).Children.OfType<StackPanel>())
            {
                foreach (var child in panel.Children.OfType<Button>())
                {
                    // Jeśli znajdzie zmienia kolor tła na szary
                    if (child.Content.ToString() == miejsce)
                    {
                        child.Background = Brushes.LightGray;
                    }
                }
            }
        }


        // Funkcja zapisująca zajęte miejsca do ustawień aplikacji
        private void zapiszZajete()
        {
            Properties.Settings.Default.zajeteKupno = string.Join(",", zajeteMiejsca);
            Properties.Settings.Default.Save();
        }

        // Ładuje zajęte miejsca z ustawień aplikacji
        private void zaladujZajete()
        {
            // Pobiera zajęte miejsca zapisane w ustawieniach
            string zajeteMiejscaStr = Properties.Settings.Default.zajeteKupno;

            // Sprawdza czy ciąg nie jest pusty
            if (!string.IsNullOrEmpty(zajeteMiejscaStr))
            {
                // Konwertuje ciąg na listę i zaznacza miejsca jako zajęte
                zajeteMiejsca = zajeteMiejscaStr.Split(',').ToList();
                zaznaczZajete(zajeteMiejsca);
            }
        }
    }
}