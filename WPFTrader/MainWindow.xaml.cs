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
using GestionnaireBDD;
using MetierTrader;

namespace WPFTrader
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GstBdd gstBDD;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gstBDD = new GstBdd();
            lstTraders.ItemsSource = gstBDD.getAllTraders();
        }

        private void lstTraders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstActions.ItemsSource = gstBDD.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
            txtTotalPortefeuille.Text = gstBDD.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
            lstActionsNonPossedees.ItemsSource = gstBDD.getAllActionsNonPossedees((lstTraders.SelectedItem as Trader).NumTrader);
        }

        private void lstActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstActions.SelectedItem != null)
            {
                double coursReel = Convert.ToDouble(gstBDD.getCoursReel((lstActions.SelectedItem as ActionPerso).NumAction));
                double prixAchat = (lstActions.SelectedItem as ActionPerso).PrixAchat;
                if (prixAchat == coursReel)
                {
                    imgAction.Source = new BitmapImage(new Uri("/Images/Moyen.png", UriKind.RelativeOrAbsolute));
                }
                if (prixAchat < coursReel)
                {
                    imgAction.Source = new BitmapImage(new Uri("/Images/Haut.png", UriKind.RelativeOrAbsolute));
                }
                if (prixAchat > coursReel)
                {
                    imgAction.Source = new BitmapImage(new Uri("/Images/Bas.png", UriKind.RelativeOrAbsolute));
                }
            }
           
        }

        private void btnVendre_Click(object sender, RoutedEventArgs e)
        {
            if(lstActions.SelectedItem == null)
            {
                MessageBox.Show("Selectionner une Action", "Votre choix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(txtQuantiteVendue.Text == "")
                {
                    MessageBox.Show("Saisir une quantité", "Votre choix", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (Convert.ToInt32(txtQuantiteVendue.Text) > (lstActions.SelectedItem as ActionPerso).Quantite)
                    {
                        MessageBox.Show("Impossible de vendre plus que ce que vous possédez", "Votre choix", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if(Convert.ToInt32(txtQuantiteVendue.Text) < (lstActions.SelectedItem as ActionPerso).Quantite)
                        {
                            int nouvelleQuantite = (lstActions.SelectedItem as ActionPerso).Quantite - Convert.ToInt32(txtQuantiteVendue.Text);
                            gstBDD.UpdateQuantite((lstActions.SelectedItem as ActionPerso).NumAction, (lstTraders.SelectedItem as Trader).NumTrader, nouvelleQuantite);
                            lstActions.ItemsSource = null;
                            lstActions.ItemsSource = gstBDD.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
                            txtTotalPortefeuille.Text = gstBDD.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
                        }
                        else
                        {
                            gstBDD.SupprimerActionAcheter((lstActions.SelectedItem as ActionPerso).NumAction, (lstTraders.SelectedItem as Trader).NumTrader);
                            lstActions.ItemsSource = null;
                            lstActions.ItemsSource = gstBDD.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
                            txtTotalPortefeuille.Text = gstBDD.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
                            lstActionsNonPossedees.ItemsSource = null;
                            lstActionsNonPossedees.ItemsSource = gstBDD.getAllActionsNonPossedees((lstTraders.SelectedItem as Trader).NumTrader);
                        }
                    }
                }
            }
           
        }

        private void btnAcheter_Click(object sender, RoutedEventArgs e)
        {
            if(lstActionsNonPossedees.SelectedItem == null)
            {
                MessageBox.Show("Saisir une Action", "Votre choix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(txtPrixAchat.Text == "")
                {
                    MessageBox.Show("Saisir un prix", "Votre choix", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if(txtQuantiteAchetee.Text == "")
                    {
                        MessageBox.Show("Saisir une quantité", "Votre choix", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        gstBDD.AcheterAction((lstActionsNonPossedees.SelectedItem as MetierTrader.Action).NumAction, (lstTraders.SelectedItem as Trader).NumTrader, Convert.ToDouble(txtPrixAchat.Text), Convert.ToInt32(txtQuantiteAchetee.Text));
                        MessageBox.Show("Action enregistrée", "Votre achat", MessageBoxButton.OK, MessageBoxImage.Information);
                        lstActions.ItemsSource = null;
                        lstActions.ItemsSource = gstBDD.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
                        lstActionsNonPossedees.ItemsSource = null;
                        lstActionsNonPossedees.ItemsSource = gstBDD.getAllActionsNonPossedees((lstTraders.SelectedItem as Trader).NumTrader);
                        txtTotalPortefeuille.Text = gstBDD.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
                    }
                }
            }
        }
    }
}
