using System;
using System.Collections.Generic;
using System.Text;

namespace MetierTrader
{
    public class ActionPerso
    {
        private int numAction;
        private string nomAction;
        private double prixAchat;
        private int quantite;
        private double total;

        public ActionPerso(int unNum, string unNom, double unPrix, int uneQuantite)
        {
            NumAction = unNum;
            NomAction = unNom;
            PrixAchat = unPrix;
            Quantite = uneQuantite;
            Total = (PrixAchat * Quantite);

        }
        public ActionPerso(int unNum, string unNom)
        {
            NumAction = unNum;
            NomAction = unNom;
        }


        public int NumAction { get => numAction; set => numAction = value; }
        public string NomAction { get => nomAction; set => nomAction = value; }
        public double PrixAchat { get => prixAchat; set => prixAchat = value; }
        public int Quantite { get => quantite; set => quantite = value; }
        public double Total { get => total; set => total = value; }
    }
}
