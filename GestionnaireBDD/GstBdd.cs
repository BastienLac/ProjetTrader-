using MySql.Data.MySqlClient;
using System;
using MetierTrader;
using System.Collections.Generic;

namespace GestionnaireBDD
{
    public class GstBdd
    {
        private MySqlConnection cnx;
        private MySqlCommand cmd;
        private MySqlDataReader dr;

        // Constructeur
        public GstBdd()
        {
            string chaine = "Server=localhost;Database=bourse;Uid=root;Pwd=";
            cnx = new MySqlConnection(chaine);
            cnx.Open();
        }

        public List<Trader> getAllTraders()
        {
            List<Trader> mesTraders = new List<Trader>();
            cmd = new MySqlCommand("Select idTrader, nomTrader from trader", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Trader unTrader = new Trader(Convert.ToInt16(dr[0].ToString()), dr[1].ToString());
                mesTraders.Add(unTrader);
            }
            dr.Close();
            return mesTraders;
        }
        public List<ActionPerso> getAllActionsByTrader(int numTrader)
        {
            List<ActionPerso> mesActionsPerso = new List<ActionPerso>();
            cmd = new MySqlCommand("SELECT action.idAction, action.nomAction, acheter.prixAchat, acheter.quantite FROM action INNER JOIN acheter ON action.idAction = acheter.numAction WHERE acheter.numTrader ="+numTrader, cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ActionPerso uneActionPerso = new ActionPerso(Convert.ToInt32(dr[0].ToString()), dr[1].ToString(), Convert.ToDouble(dr[2].ToString()), Convert.ToInt32(dr[3].ToString()));
                mesActionsPerso.Add(uneActionPerso);
            }
            dr.Close();
            return mesActionsPerso;
            
        }

        public List<MetierTrader.Action> getAllActionsNonPossedees(int numTrader)
        {
            List<MetierTrader.Action> mesActionsNonPossedees = new List<MetierTrader.Action>();
            cmd = new MySqlCommand("SELECT action.idAction, action.nomAction FROM action WHERE idAction NOT IN(SELECT action.idAction FROM action INNER JOIN acheter ON action.idAction = acheter.numAction WHERE numTrader =" + numTrader + ")", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                MetierTrader.Action uneAction = new MetierTrader.Action(Convert.ToInt16(dr[0].ToString()), dr[1].ToString());
                mesActionsNonPossedees.Add(uneAction);
            }
            dr.Close();
            return mesActionsNonPossedees;
        }

        public void SupprimerActionAcheter(int numAction, int numTrader)
        {
            cmd = new MySqlCommand("DELETE from acheter WHERE numAction ="+numAction+" AND numTrader="+numTrader, cnx);
            cmd.ExecuteNonQuery();
        }

        public void UpdateQuantite(int numAction, int numTrader, int quantite)
        {
            cmd = new MySqlCommand("UPDATE acheter SET quantite =" + quantite + " WHERE numAction =" + numAction + " AND numTrader =" + numTrader, cnx);
            cmd.ExecuteNonQuery();
        }

        public double getCoursReel(int numAction)
        {
            double coursReel = 0;
            cmd = new MySqlCommand("SELECT coursReel FROM action WHERE idAction ="+numAction, cnx);
            dr = cmd.ExecuteReader();
            dr.Read();
            coursReel = Convert.ToDouble(dr[0].ToString());
            dr.Close();
            return coursReel;
        }
        public void AcheterAction(int numAction, int numTrader, double prix, int quantite)
        {
            cmd = new MySqlCommand("insert into acheter values(" + numAction + " , '" + numTrader + "' , " + prix + " , " + quantite + ")", cnx);
            cmd.ExecuteNonQuery();
        }
        public double getTotalPortefeuille(int numTrader)
        {
            double portefeuille = 0;
            cmd = new MySqlCommand("SELECT SUM(acheter.prixAchat * acheter.quantite) FROM acheter WHERE acheter.numTrader =" + numTrader, cnx);
            dr = cmd.ExecuteReader();
            dr.Read();
            portefeuille = Convert.ToDouble(dr[0].ToString());
            dr.Close();
            return portefeuille;
        }
    }
}
