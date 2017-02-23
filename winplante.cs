using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Configuration;
using log4net;
using log4net.Config;
using System.Text.RegularExpressions;


namespace WinPlante
{
    public partial class winplante : Form
    {
        //dataset utilisé pour les plantes test2
        DataSet ds = new DataSet();

        //dataset bouture
        DataSet mondata = new DataSet();
        protected static readonly ILog log = LogManager.GetLogger(typeof(Program));
  



        public winplante()
        {
            InitializeComponent();
        }

        private void formSaisie_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            //mise a vide des label
            lblresultbouture.Text = "";
            lblresultplante.Text = "";

            lblresultmodifplante.Text = "";
            lblresultModifBouture.Text = "";

            lblsupp.Text = "";
            lblsupBout.Text = "";

            lbletatListePlante.Text = "";
            lblEtatBouture.Text = "";

            ListePLante();
            ListeBouture();



            for (int i = 0; i <= 30; i++)
            {
                cbTempsleve.Items.Add(i.ToString());
            }

        }

      

        private void btnenregistrer_Click(object sender, EventArgs e)
        {

        }



        private void ClearTxt()
        {
            txtcommentaire.Text = "";
            txtnomgraine.Text = "";
            saisie_dateSemiGraine.Text = "";
            saisie_DateRepiq.Text = "";


        }

        private void btnquitter_Click(object sender, EventArgs e)
        {
            winplante retaccueil = new winplante();
            retaccueil.Show();

            this.Hide();
        }

        private void btEnregistrerPlante_Click(object sender, EventArgs e)
        {
            //insertion de nouvelle données

            if (InsertPlante())
            {
                Effacerplante();

                listboxplante.Items.Clear();
                ListePLante();

            }
            else
            {
                log.Error("Echec de l'insertion des données");
            }

        }




        private void btnsauverBouture_Click(object sender, EventArgs e)
        {
            //insertion de bouture

            if (InsertBouture())
            {
                Effacerbouture();
            }
            else
            {
                log.Error("Erreur dans l'insertion des données");
            }
        }


        private bool InsertPlante()
        {
            try
            {
                if (!(txtnomgraine.Text == ""))
                {
                    log.Info("-------------------------------");
                    log.Info("*** insertion d une plante *** ");
                    // string connexionstring = "Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;";
                    String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
                    // OdbcConnection maconnexion = new OdbcConnection(connexionstring);


                    OracleConnection maconnexion = new OracleConnection(connexionstring);

                    string typegraine = txtnomgraine.Text;
                    typegraine = txtnomgraine.Text.Replace("'", " ");

                    // DateTime madate = saisie_dateSemiGraine.Value;
                    //  string dateSemiPlante = madate.ToString("dd/MM/yyyy");
                    string dateSemiPlante = saisie_dateSemiGraine.Text;

                    string commPlante = txtcommentaire.Text;
                    commPlante = txtcommentaire.Text.Replace("'", " ");

                    log.Info("commPlante : " + commPlante);
                   string insertsoc = "INSERT INTO plante (idplante,nomplante,datesemi,commentaire) values (SEQPLANTE.nextval, '" + typegraine + "',TO_DATE('" + dateSemiPlante + "','DD-MM-YYYY HH:Mi:SS'),'" + commPlante + "')";

                    log.Info("Requete envoyee : " + insertsoc);
                    maconnexion.Open();
                    // OdbcCommand macom = new OdbcCommand(insertsoc, maconnexion);
                    OracleCommand macom = new OracleCommand(insertsoc, maconnexion);
                    OracleDataReader monreader = macom.ExecuteReader();
                    //  OdbcDataReader monreader = macom.ExecuteReader();

                    maconnexion.Close();
                    maconnexion.Dispose();

                    lblresultplante.ForeColor = Color.Green;
                    lblresultplante.Text = "Insertion Reussi";
                    log.Info("=====> INSERTION DE LA PLANTE REUSSI ");
                    return true;
                }
                else
                {
                    log.Error("Champ txtnomgraine vide : " + txtnomgraine.Text);
                    lblresultplante.ForeColor = Color.Red;
                    lblresultplante.Text = "Merci de remplir tous les champs";

                    return false;
                }

            }
            catch (Exception erreur)
            {
                lblresultplante.ForeColor = Color.Red;
                lblresultplante.Text = "Echec de l\'insertion des données";
                log.Error("Erreur dans l insertion de la plante : " + erreur.ToString());
                return false;
            }
        }


        private void Effacerplante()
        {
            txtnomgraine.Text = "";
            saisie_DateRepiq.Text = "";
            txtcommentaire.Text = "";
        }

        private void Effacerbouture()
        {
            txtnombouture.Text = "";
            txtcomm.Text = "";
        }


        private bool InsertBouture()
        {
            if (!(txtnombouture.Text == ""))
            {
                try
                {
                    log.Info("-------------------------------");
                    log.Info("**** Insertion dune bouture ****");
                    // string connexionstring = "Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;";
                    String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
                    //  OdbcConnection connexionBouture = new OdbcConnection(connexionstring);

                    OracleConnection connexionBouture = new OracleConnection(connexionstring);
                    string typeplante = txtnombouture.Text;
                    typeplante = typeplante.Replace("'", " ");

                    //DateTime madate = saisieDateBouture.Value;
                    //string dateBouture = madate.ToString("dd/MM/yyyy");
                    string dateBouture = saisieDateBouture.Text;
                    string comBouture = txtcomm.Text;
                    comBouture = txtcomm.Text.Replace("'", " ");

                    string insertsoc = "INSERT INTO Bouture (idbouture,nom,datebouture,commentaire) values (SEQBOUTURE.nextval,'" + typeplante + "',TO_DATE('" + dateBouture + "','DD-MM-YYYY hh24:mi:ss'),'" + comBouture + "')";
                    log.Info("Requete envoyée : " + insertsoc);
                    connexionBouture.Open();
                    //  OdbcCommand macom = new OracleCommand(insertsoc, connexionBouture);
                    OracleCommand macom = new OracleCommand(insertsoc, connexionBouture);
                    //  OdbcDataReader monreader = macom.ExecuteReader();
                    OracleDataReader monreader = macom.ExecuteReader();
                    connexionBouture.Close();
                    connexionBouture.Dispose();

                    lblresultbouture.ForeColor = Color.Green;
                    lblresultbouture.Text = "Insertion Reussi";
                    log.Info("=====> INSERTION DE LA BOUTURE REUSSI ");
                    return true;


                }
                catch (Exception erreur)
                {
                    lblresultbouture.ForeColor = Color.Red;
                    lblresultbouture.Text = erreur.Message;
                    // MessageBox.Show(erreur.Message);
                    log.Error("Erreur dans l\'insertion de la bouture ! : " + erreur.ToString());
                    return false;
                }
            }
            else
            {
                log.Info("Champ txtnombouture.Text  vide : " + txtnombouture.Text);
                lblresultbouture.ForeColor = Color.Red;
                lblresultbouture.Text = "Merci de remplir tous les champs.";
                return false;
            }
        }

        private void testerConnexion()
        {
            try
            {
                string connexionstring = "Data Source={Oracle In XE};Server=XE;uid=system;pwd=eternity;";
                //WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
                OdbcConnection connexion = new OdbcConnection(connexionstring);//

                MessageBox.Show(connexion.DataSource.ToString());

                OdbcCommand cmd = new OdbcCommand("select NOMPLANTE, datesemi,dateleve,daterepiquage,commentaire, tempsleve from plante", connexion);
                connexion.Open();

                connexion.Close();
                connexion.Dispose();

                MessageBox.Show("Connexion ok");

            }
            catch (Exception erreur)
            {

                MessageBox.Show("Erreur : " + erreur.Message);


            }
        }

        //affichage de la liste de plante
        private void ListePLante()
        {
            try
            {
                String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;

                log.Info("*******************************************");
                log.Info("*** Affichage de la liste des plantes **** ");
                string requete = "select NOMPLANTE, datesemi from plante";

                OracleConnection connexion = new OracleConnection(connexionstring);
                connexion.Open();
               // OracleCommand cmd = new OracleCommand("select NOMPLANTE, datesemi,dateleve,commentaire, tempsleve from plante", connexion);
                OracleCommand cmd = new OracleCommand(requete, connexion);



                //alimentation de la combobox  ,adresse,cp, ville
                OracleDataReader monreader = cmd.ExecuteReader();

            
      
                while (monreader.Read())
                {
                   
                    
                    listboxplante.Items.Add(monreader.GetValue(0).ToString() + "           " + monreader.GetValue(1).ToString());
                  

                }
             

                //fermeture des connexion
                connexion.Close();
                connexion.Dispose();
                lbletatListePlante.Text = "Etat : Données disponible";
                lbletatListePlante.ForeColor = Color.Green;
                log.Info("=====> AFFICHAGE DE LA LISTE DES PLANTE  REUSSI ");

            }
            catch (Exception erreur)
            {
                log.Error("Erreur dans l'affichage de la liste des plantes : " + erreur.ToString());
                lbletatListePlante.Text = "Etat : Données indisponible ";
                lbletatListePlante.ForeColor = Color.Red;

            }
        }

        //affichage de la liste de plante

        //chargement de la liste ds boutures
        private void ListeBouture()
        {
            try
            {
                log.Info("*******************************************");
                log.Info("**** Affichage de la liste des boutures *** ");

                // String connexionstring = "Data Source=XE;Persist Security Info=True;User ID=system;Password=eternity;Unicode=True";
                String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;

                //WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;

                OracleConnection connexion = new OracleConnection(connexionstring);
                connexion.Open();
                OracleCommand cmd = new OracleCommand("select nom, datebouture,dateracine,commentaire from Bouture", connexion);


                OracleDataReader monreader = cmd.ExecuteReader();

                OracleDataAdapter monad = new OracleDataAdapter();
                monad.SelectCommand = cmd;
                monad.Fill(mondata);
                GridviewBouture.DataSource = mondata.Tables[0];


                //fermeture des connexion
                connexion.Close();
                connexion.Dispose();

                lblEtatBouture.Text = "Etat : Données disponible";
                lblEtatBouture.ForeColor = Color.Green;
                log.Info("=====> AFFICHAGE DES BOUTURES REUSSI ");


            }
            catch (Exception erreur)
            {
                log.Error("Erreur dans l'affichage de la liste de boutures : " + erreur.ToString());
                lblEtatBouture.Text = "Etat : Données indisponible ";
                lblEtatBouture.ForeColor = Color.Red;


            }

        }


        private void BtmodifPlante_Click(object sender, EventArgs e)
        {
            if (ModifPLante())
            {
                EffacerPlanteModif();
            }
            else
            {
                log.Error("Erreur dans le modification de la plante. ");
            }
        }

        private void EffacerPlanteModif()
        {
            plante_txtNomplante.Text = "";
            plante_DatetimeSemi.Text = "";
            plante_txtComm.Text = "";
            cbTempsleve.Text = "";
        }

        private bool ModifPLante()
        {
            log.Info("Modification de plante  - valeur envoyé : ");
            log.Info("plante_txtNomplante.Text : " + plante_txtNomplante.Text);
            log.Info("plante_DatetimeSemi.Text : " + plante_DatetimeSemi.Text);
            string majplante = "";
            string nomplante = plante_txtNomplante.Text;
            string datesemiplante = plante_DatetimeSemi.Text;
            string comPlante = plante_txtComm.Text;
            string tempslevePlante = cbTempsleve.Text;
            string datelevePLante = plante_DatetimeLeve.Text;

            if (!(nomplante == "") && !(datesemiplante == ""))
            {
                log.Info("           -------------------------------");
                log.Info(" *** Lancement de la procedure de modification d'une plante *** ");


                comPlante = plante_txtComm.Text.Replace("'", " ");


                try
                {
                    // DataSet data = new DataSet();

                    if (comPlante != "")  //zone commentaire non vide
                    {
                        if (datesemiplante != "") // & zone temps non vide
                        {
                            majplante = "update plante set dateleve=TO_DATE('" + datelevePLante + "','DD-MM-YYYY'), commentaire='" + comPlante + "', tempsleve='" + tempslevePlante
                                + "' where nomplante='" + nomplante + "' and datesemi='" + datesemiplante + "' ";
                        }
                        else //  & zone temps vide
                        {
                            majplante = "update plante set dateleve=TO_DATE('" + datelevePLante + "','DD-MM-YYYY'), commentaire='" + comPlante + "' where nomplante='" + nomplante + "' and datesemi='" + datesemiplante + "' ";

                        }

                    }
                    else //zone comm vide 
                    {
                        if (tempslevePlante != "") // & zone temps non vide
                        {
                            majplante = "update plante set dateleve=TO_DATE('" + datelevePLante + "','DD-MM-YYYY'), tempsleve='" + tempslevePlante + "' where nomplante='" + nomplante + "' and datesemi='" + datesemiplante + "' ";
                        }
                        else  //& zone temps vide
                        {
                            majplante = "update plante set dateleve=TO_DATE('" + datelevePLante + "','DD-MM-YYYY') where nomplante='" + nomplante + "' and datesemi='" + datesemiplante + "' ";

                        }

                    }

                    log.Info("Requet envoyée : " + majplante);
                    // string connexionstring = "Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;";
                    String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;

                    OracleConnection databaseConnection = new OracleConnection(connexionstring);
                    databaseConnection.Open();



                    OracleCommand mycommandligne = new OracleCommand(majplante, databaseConnection);
                    mycommandligne.ExecuteScalar();

                    databaseConnection.Close();
                    databaseConnection.Dispose();
                    //selection du nb denregistrement 

                    ///DatagridPlante.DataSource = ds.Tables[0];


                    //mise a jour de la gridview
                   // ds.Tables[0].Clear();
                    listboxplante.Items.Clear();
                    ListePLante();


                    lblresultmodifplante.ForeColor = Color.Green;
                    lblresultmodifplante.Text = "Modification Réussi";


                    log.Info("=====> MISE A JOUR  DE LA PLANTE REUSSI ");
                    return true;

                }
                catch (Exception erreur)
                {
                    //MessageBox.Show(erreur.ToString());
                    log.Error("Erreur dans la modification de la plante : " + erreur.ToString());
                    lblresultmodifplante.ForeColor = Color.Red;
                    lblresultmodifplante.Text = erreur.Message;
                    return false;
                }
            }
            else
            {
                lblresultmodifplante.ForeColor = Color.Red;
                lblresultmodifplante.Text = "Merci de remplir tous les champs";
                return false;
            }

        }

        private void btnmodifBouture_Click(object sender, EventArgs e)
        {
            if (modifBouture())
            {
                EffacerBoutureModif();
            }
            else
            {
                log.Info("------------------------------------");
                log.Error("Erreur dans la modification  des données");
            }
        }

        private void EffacerBoutureModif()
        {
            bout_txtnomplante.Text = "";
            bout_txtdatesemi.Text = "";
        }

        private bool modifBouture()
        {

            log.Info("Lancement de la procedure de modification d'une bouture");
            //DateTime madate = bout_txtdateracine.Value;
            //string dateRacineBouture = madate.ToString("dd/MM/yyyy");

            string nomBouture = bout_txtnomplante.Text;
            string dateBouture = bout_txtdatesemi.Text;
            string dateRacineBouture = bout_txtdateracine.Text;

            if (!(bout_txtnomplante.Text == "") && !(bout_txtdatesemi.Text == ""))
            {
                try
                {
                    string majplante = "update BOUTURE set DATERACINE=TO_DATE('" + dateRacineBouture + "','DD-MM-YYYY') where nom='" + nomBouture + "' and datebouture='" + dateBouture + "' ";

                    log.Info("Requet ede mise a jour de la bouture : " + majplante);
                    String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
                    // string connexionstring = "Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;";
                    // OdbcConnection databaseConnection = new OdbcConnection(connexionstring);
                    // OdbcConnection databaseConnection = new OdbcConnection(connexionstring);
                    // databaseConnection.Open();

                    OracleConnection databaseConnection = new OracleConnection(connexionstring);
                    databaseConnection.Open();
                    OracleCommand mycommandligne = new OracleCommand(majplante, databaseConnection);
                    mycommandligne.ExecuteScalar();



                    //efface la liste puis on reaffiche la liste 
                    GridviewBouture.DataSource = mondata.Tables[0];
                    mondata.Tables[0].Clear();
                    ListeBouture();

                    lblresultModifBouture.ForeColor = Color.Green;
                    lblresultModifBouture.Text = "Modification Réussi";

                    log.Info("=====> MISE A JOUR  DE LA BOUTURE REUSSI ");
                    return true;

                }
                catch (Exception erreur)
                {
                    log.Error("Erreur dans la mise a jour de la bouture : " + erreur.ToString());
                    lblresultModifBouture.ForeColor = Color.Red;
                    lblresultModifBouture.Text = "Echec Modification";
                    return false;


                }
            }
            else
            {
                lblresultModifBouture.ForeColor = Color.Red;
                lblresultModifBouture.Text = "Merci de remplir tous les champs.";
                return false;
            }
        }

        private void winplante_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnsupp_Click(object sender, EventArgs e)
        {
            //suppresion de l'enregistrement

            if (SuppPLante())
            {
                EffacerPlanteSup();

            }
            else
            {
                log.Info("------------------------------------");
                log.Error("Echec de la suppression de la plante");
            }
        }

        private void EffacerPlanteSup()
        {
            sup_txtnom.Text = "";
            sup_txtDateSemi.Text = "";
        }

        private bool SuppPLante()
        {
            log.Info("           -------------------------------");
            log.Info(" ****** Lancement de la procedure de suppression d'une plante *****  ");

            if (!(sup_txtDateSemi.Text == "") && !(sup_txtnom.Text == ""))
            {

                string delplante = "";
                try
                {

                    delplante = "delete from plante where nomplante='" + sup_txtnom.Text + "' and datesemi='" + sup_txtDateSemi.Text + "' ";
                    log.Info("Requete envoyée pour la suppresion : " + delplante);

                    // string connexionstring = "Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;";
                    String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
                    OracleConnection databaseConnection = new OracleConnection(connexionstring);

                    databaseConnection.Open();



                    //selection du nb denregistrement 
                    OracleCommand mycommandligne = new OracleCommand(delplante, databaseConnection);


                    mycommandligne.ExecuteScalar();


                    databaseConnection.Close();
                    databaseConnection.Dispose();
                    //DatagridPlante.DataSource = ds.Tables[0];
                  //  ds.Tables[0].Clear();
                    listboxplante.Items.Clear();
                    ListePLante();


                    lblsupp.ForeColor = Color.Green;
                    lblsupp.Text = "Suppression Réussi";
                    log.Info("Suppression de la plante reussite");
                    return true;
                }
                catch (Exception erreur)
                {
                    log.Error("Erreur dans la suppression de la plante : " + erreur.ToString());
                    lblsupp.ForeColor = Color.Red;
                    lblsupp.Text = erreur.Message;
                    return false;
                }
            }
            else
            {
                lblsupp.ForeColor = Color.Red;
                lblsupp.Text = "Merci de remplir tous les champs";
                return false;
            }
        }

        private void btnsuppBouture_Click(object sender, EventArgs e)
        {
            if (SUppBouture())
            {
                EffacerBoutureSup();
            }
            else
            {
                log.Info("------------------------------------");
                log.Error("Erreur dans la suppresion des données");
            }
        }

        private void EffacerBoutureSup()
        {
            sup_txtnomBout.Text = "";
            sup_txtdateSemiBout.Text = "";

        }

        private bool SUppBouture()
        {
            log.Info(" ****** Lancement de la procedure de suppression de la bouture");

            if (!(sup_txtnomBout.Text == "") && !(sup_txtdateSemiBout.Text == ""))
            {


                string delBouture = "";
                string nombouture = sup_txtnomBout.Text;
                string dateBoutureSupp = sup_txtdateSemiBout.Text;


                try
                {



                    delBouture = "delete from bouture where nom='" + nombouture + "' and datebouture='" + dateBoutureSupp + "' ";

                    log.Info("Requete envoyé pour supprimer la bouture : " + delBouture);

                    //string connexionstring = "Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;";
                    String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
                    OracleConnection databaseConnection = new OracleConnection(connexionstring);
                    databaseConnection.Open();



                    //selection du nb denregistrement 
                    OracleCommand mycommandligne = new OracleCommand(delBouture, databaseConnection);


                    mycommandligne.ExecuteScalar();



                    databaseConnection.Close();
                    databaseConnection.Dispose();

                    GridviewBouture.DataSource = mondata.Tables[0];
                    mondata.Tables[0].Clear();
                    ListeBouture();


                    lblsupBout.ForeColor = Color.Green;
                    lblsupBout.Text = "Suppression Réussi";
                    log.Info("SUppresion de la bouture reussite");
                    return true;
                }
                catch (Exception erreur)
                {
                    log.Error("Erreur dans la suppression de la bouture : " + erreur.ToString());
                    lblsupBout.ForeColor = Color.Red;
                    lblsupBout.Text ="Erreur dans la suppression";
                    return false;
                }

            }
            else
            {
                lblsupBout.ForeColor = Color.Red;
                lblsupBout.Text = "Merci de remplir tous les champs ";
                return false;
            }
        }

        private void tabplantes_SelectedIndexChanged(object sender, EventArgs e)
        {




        }

        //rafraishissement de la liste des plantes
        private void btnupdatePlante_Click(object sender, EventArgs e)
        {
            log.Info("------- RAFRAICHISSEMENT DE LA LISTE DES PLANTES *------ ");
            listboxplante.Items.Clear();
            ListePLante();
        }

        private void btnupdatebouture_Click(object sender, EventArgs e)
        {
            log.Info("**** RAFRAICHISSEMENT DE LA LISTE DES BOUTURES **** ");
            GridviewBouture.DataSource = mondata.Tables[0];
            mondata.Tables[0].Clear();
            ListeBouture();
        }



        private void btnsauvegarder_Click(object sender, EventArgs e)
        {

            //création des fichiers sql et enregistrement des tables
            if (ecritureSauvegarde())
            {
                lblresultSauvegarde.ForeColor = Color.Green;
                lblresultSauvegarde.Text = "Ok";
            }
            else
            {
                lblresultSauvegarde.ForeColor = Color.Red;
                lblresultSauvegarde.Text = "Echec de la sauvegarde";
            }




        }



        private bool ecritureSauvegarde()
        {
            DateTime madate = DateTime.Now;
            string date = madate.ToString("dd--MM--yyyy-HH-mm-ss"); //
            string nomfichier = date + "_export.sql";
            string uriFichier = "D:\\vstudio 2013\\Projects\\backupProject\\winplante\\" + nomfichier;
            String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;
            //C:\\Users\\Vincent\\Desktop\\


            string insertTablebouture = "";
            string creationTable =
             "   --------------------------------------------------------  \n " +
             "   --  DDL for Table PLANTE  \n " +
              "  --------------------------------------------------------  \n " +
                //plante
              "    CREATE TABLE \"SYSTEM\".\"PLANTE\"   \n " +
             "      (	\"IDPLANTE\" NUMBER(30,0),   \n " +
             "       \"NOMPLANTE\" VARCHAR2(40 BYTE),   \n " +
              "      \"DATESEMI\" VARCHAR2(20 BYTE),   \n " +
               "     \"DATELEVE\" VARCHAR2(20 BYTE),  \n " +
              "      \"COMMENTAIRE\" VARCHAR2(70 BYTE),  \n " +
              "      \"DATEREPIQUAGE\" VARCHAR2(20 BYTE),  \n " +
              "      \"TEMPSLEVE\" VARCHAR2(20 BYTE) \n " +
              "     ) PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING  \n " +
               "   STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645  \n " +
               "   PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)  \n " +
               "   TABLESPACE \"SYSTEM\" ;  \n " +
              "  REM INSERTING into SYSTEM.PLANTE  \n " +
             "   SET DEFINE OFF;  \n " +
             "   --------------------------------------------------------   \n " +
            "    --  DDL for Index PLANTE_PK  \n " +
             "   --------------------------------------------------------   \n " +

              "    CREATE UNIQUE INDEX \"SYSTEM\".\"PLANTE_PK\" ON \"SYSTEM\".\"PLANTE\" (\"IDPLANTE\")   \n " +
             "     PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS   \n " +
             "     STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645  \n " +
             "     PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)   \n " +
             "     TABLESPACE \"SYSTEM\" ;  \n " +
            "    --------------------------------------------------------  \n " +
            "    --  Constraints for Table PLANTE   \n " +
             "   --------------------------------------------------------  \n " +

              "    ALTER TABLE \"SYSTEM\".\"PLANTE\" ADD CONSTRAINT \"PLANTE_PK\" PRIMARY KEY (\"IDPLANTE\")   \n " +
             "     USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS   \n " +
             "     STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645  \n " +
             "     PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)   \n " +
             "     TABLESPACE \"SYSTEM\"  ENABLE;   \n " +
              "    ALTER TABLE \"SYSTEM\".\"PLANTE\" MODIFY (\"IDPLANTE\" NOT NULL ENABLE);   \n " +
                //bouture
             "    --------------------------------------------------------   \n " +
             "   --  DDL for Table BOUTURE  \n " +
            "    --------------------------------------------------------   \n " +

              "    CREATE TABLE \"SYSTEM\".\"BOUTURE\"     \n " +
              "     (	\"IDBOUTURE\" NUMBER(30,0),   \n " +
             "       \"NOM\" VARCHAR2(30 BYTE),   \n " +
              "      \"DATEBOUTURE\" VARCHAR2(20 BYTE),   \n " +
              "      \"DATERACINE\" VARCHAR2(20 BYTE),   \n " +
              "      \"COMMENTAIRE\" VARCHAR2(70 BYTE)  \n " +
              "     ) PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING  \n " +
               "   STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645  \n " +
              "    PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)   \n " +
             "     TABLESPACE \"SYSTEM\" ;   \n " +
             "   REM INSERTING into SYSTEM.BOUTURE  \n " +
             "   SET DEFINE OFF; \n " +
             "   --------------------------------------------------------   \n " +
            "    --  DDL for Index BOUTURE_PK \n " +
             "   -------------------------------------------------------- \n " +

            "      CREATE UNIQUE INDEX \"SYSTEM\".\"BOUTURE_PK\" ON \"SYSTEM\".\"BOUTURE\" (\"IDBOUTURE\")   \n " +
             "     PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS   \n " +
             "     STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645  \n " +
             "     PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)  \n " +
             "     TABLESPACE \"SYSTEM\" ;  \n " +
             "   -------------------------------------------------------- \n " +
             "   --  Constraints for Table BOUTURE \n " +
             "   -------------------------------------------------------- \n " +

             "     ALTER TABLE \"SYSTEM\".\"BOUTURE\" ADD CONSTRAINT \"BOUTURE_PK\" PRIMARY KEY (\"IDBOUTURE\")   \n " +
             "     USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS  \n " +
             "     STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645 \n " +
             "     PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)   \n " +
             "     TABLESPACE \"SYSTEM\"  ENABLE;   \n " +
                //sequence
            " CREATE SEQUENCE  \"SYSTEM\".\"SEQPLANTE\"  MINVALUE 1 MAXVALUE 999999 INCREMENT BY 1 START WITH 21 CACHE 20 NOORDER  NOCYCLE ; \n " +
            " CREATE SEQUENCE  \"SYSTEM\".\"SEQBOUTURE\"  MINVALUE 1 MAXVALUE 999999 INCREMENT BY 1 START WITH 21 CACHE 20 NOORDER  NOCYCLE ;  \n ";


            //selection des données des boutures
            #region donneesBoutures
            try
            {


                string Donnees = "select IDBOUTURE,NOM,DATEBOUTURE,DATERACINE,COMMENTAIRE from bouture";
                OracleConnection connexion = new OracleConnection(connexionstring);
                OracleCommand cmbbouture = new OracleCommand(Donnees, connexion);

                connexion.Open();
                cmbbouture.CommandType = CommandType.Text;

                OracleDataReader monreader = cmbbouture.ExecuteReader();

                while (monreader.Read())
                {
                    log.Info(monreader.GetValue(0).ToString());
                    log.Info(monreader.GetValue(1).ToString());
                    log.Info(monreader.GetValue(2).ToString());
                    log.Info(monreader.GetValue(3).ToString());
                    log.Info(monreader.GetValue(4).ToString());
                    insertTablebouture = insertTablebouture + " Insert into BOUTURE (IDBOUTURE,NOM,DATEBOUTURE,DATERACINE,COMMENTAIRE) values ('" + monreader.GetValue(0).ToString() + "','" + monreader.GetValue(1).ToString() + "','" + monreader.GetValue(2).ToString() + "','" + monreader.GetValue(3).ToString() + "','" + monreader.GetValue(4).ToString() + "'); \n";

                }//fin while

                connexion.Close();
                connexion.Dispose();
                monreader.Close();
                monreader.Dispose();
            }
            catch (Exception erreur)
            {
                log.Error("Erreur dans l'enregistrement des données de bouture : " + erreur.ToString());
            }
            #endregion




            //Selection des données des plantes
            #region donneesPlantes
            string insertplante = "";
            try
            {
                string Donnees2 = "select IDPLANTE, NOMPLANTE,DATESEMI,DATELEVE,COMMENTAIRE, DATEREPIQUAGE,TEMPSLEVE from plante";
                // OracleConnection connexion2 = new OracleConnection("Driver={Microsoft ODBC for Oracle};Server=XE;Uid=system;Pwd=eternity;");
                OracleConnection connexion2 = new OracleConnection(connexionstring);

                OracleCommand cmd2 = new OracleCommand(Donnees2, connexion2);

                connexion2.Open();
                cmd2.CommandType = CommandType.Text;

                OracleDataReader monreader2 = cmd2.ExecuteReader();

                while (monreader2.Read())
                {
                    log.Info(monreader2.GetInt32(0).ToString());  //id
                    log.Info(monreader2.GetValue(1).ToString());  //nom
                    log.Info(monreader2.GetValue(2).ToString());  //datesemi
                    log.Info(monreader2.GetValue(3).ToString());  //dateleve
                    log.Info(monreader2.GetValue(4).ToString());  //commentaire
                    log.Info(monreader2.GetValue(5).ToString()); //daterepiquage
                    log.Info(monreader2.GetValue(6).ToString()); //tempsleve

                    insertplante = insertplante + "  Insert into PLANTE (IDPLANTE,NOMPLANTE,DATESEMI,DATELEVE,DATEREPIQUAGE,COMMENTAIRE,TEMPSLEVE) values ('" + monreader2.GetInt32(0).ToString() + "','" + monreader2.GetValue(1).ToString() + "','" + monreader2.GetValue(2).ToString() + "','" + monreader2.GetValue(3).ToString() + "','" + monreader2.GetValue(4).ToString() + "','" + monreader2.GetValue(5).ToString() + "','" + monreader2.GetValue(6).ToString() + "'); \n";


                }//fin while

                connexion2.Close();
                connexion2.Dispose();
                monreader2.Close();
                monreader2.Dispose();
            }
            catch (Exception Erreur)
            {
                log.Error("Erreur dans l'enregistrement des données de bouture : " + Erreur.ToString());


            }
            #endregion

            string varfinal = creationTable + insertTablebouture + insertplante;



            try
            {
                ecriturefichier(varfinal, uriFichier);

                lblcheminfichier.Text = uriFichier;
                string dossierSauvegarde = @"D:\vstudio 2013\Projects\backupProject\winplante\";
                Process.Start(dossierSauvegarde);

            }
            catch (Exception erreur)
            {
                MessageBox.Show(erreur.ToString());
                return false;
            }
            return true;



        }//fin procedure

        private void ecriturefichier(string texte, string chemin)
        {
            StreamWriter sw = null;
            if (!File.Exists(chemin))
            {

                FileStream fs = File.Create(chemin);
                fs.Close();

                // Le fichier n'existe pas. On le crée.
                sw = File.AppendText(chemin);
                sw.WriteLine(texte);
                sw.Close();
                sw = null;
                // Remarque : On peut utiliser sw = File.AppendText(NomFichier) pour ajouter
                // du texte à un fichier existant
                // return true;
            }
            else
            {

                // Le fichier existe 
                sw = File.AppendText(chemin);
                sw.WriteLine(texte);

                sw.Close();
                sw = null;
                // Remarque : On peut utiliser sw = File.AppendText(NomFichier) pour ajouter
                // du texte à un fichier existant

                //  return true;
            }
        }

        private void btnparcourir_Click(object sender, EventArgs e)
        {

            openFileDialog.ShowDialog();


            string cheminImage = openFileDialog.FileName;
            FileInfo moninfo = new FileInfo(cheminImage);


            txtcheminImage.Text = cheminImage;

        }

        private void btnenregistrerImage_Click(object sender, EventArgs e)
        {
            //enregistre l'image vers le dossier des plantes         
            string cheminDossier = @"D:\vstudio 2013\Projects\WinPlante\WinPlante\image\";
            string cheminImage = txtcheminImage.Text;
            FileInfo finfo = new FileInfo(cheminImage);


            DateTime dateT = DateTime.Now;
            string nomdossier = listplante.Text + "_" + dateT.ToString("dd-MM-yyyy");
            string repertoireDest = cheminDossier + nomdossier;

            string nomFichier = repertoireDest + "\\" + finfo.Name;
            //creation du dossier
            Directory.CreateDirectory(cheminDossier + nomdossier);
            log.Info(cheminDossier);
            log.Info(cheminImage);
            log.Info(nomdossier);
            log.Info(nomFichier);

            if (!(File.Exists(nomFichier)))
            {

                //copie le  fichier
                File.Copy(cheminImage, nomFichier);
            }






        }

        private void Infoplante(string nom, string date)
        {
            try
            {
                String connexionstring = WebConfigurationManager.ConnectionStrings["ConnexionstringOracle"].ConnectionString;

                log.Info("*******************************************");
                log.Info("*** Affichage de la liste des plantes **** ");

                log.Info("select NOMPLANTE, datesemi,dateleve,commentaire, tempsleve from plante where nomplante='" + nom + "' and datesemi='"+date+"') "); //TO_DATE('" + date + "','DD-MM-YYYY  HH:mm:ss'

                OracleConnection connexion = new OracleConnection(connexionstring);
                connexion.Open();
                // OracleCommand cmd = new OracleCommand("select NOMPLANTE, datesemi,dateleve,commentaire, tempsleve from plante", connexion);
                OracleCommand cmd = new OracleCommand("select NOMPLANTE, datesemi,dateleve,commentaire, tempsleve from plante where nomplante='" + nom + "' and datesemi='" + date + "' ", connexion);



                //alimentation de la combobox  ,adresse,cp, ville
                OracleDataReader monreader = cmd.ExecuteReader();
                monreader.Read();
                lblplante.Text=monreader.GetValue(0).ToString() ;             
                lbldate.Text= monreader.GetValue(1).ToString();
                lbltemps.Text =monreader.GetValue(4).ToString() ;
                lblcom.Text = monreader.GetValue(3).ToString();
                
                    
                
            

                //fermeture des connexion
                connexion.Close();
                connexion.Dispose();
                lbletatListePlante.Text = "Etat : Données disponible";
                lbletatListePlante.ForeColor = Color.Green;
                log.Info("=====> AFFICHAGE DE LA LISTE DES PLANTE  REUSSI ");

            }
            catch (Exception erreur)
            {
                log.Error("Erreur dans l'affichage de la liste des plantes : " + erreur.ToString());
                lbletatListePlante.Text = "Etat : Données indisponible - " + erreur.Message;
                lbletatListePlante.ForeColor = Color.Red;

            }
        }


        private void listboxplante_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valeur = listboxplante.SelectedItem.ToString();
            string[] recupInfo = Regex.Split(valeur, "           ");

            //recuperation des données : nom et date semi
            log.Info(recupInfo[0]);
            log.Info(recupInfo[1]);

            DateTime madate = Convert.ToDateTime(recupInfo[1]);
            string madatetime = madate.ToString("dd/MM/yyyy"); ;


            Infoplante(recupInfo[0], madatetime);
            //selection es info en fonction du nom et de la date


        }

   

   

    }

}





