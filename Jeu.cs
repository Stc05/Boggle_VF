using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Diagnostics;

namespace Boggle_VF
{
    public class Jeu
    {
        private Joueur[] joueurs;
        private Dictionnaire dico;
        private int nb_joueurs;
        private Dictionary<Joueur, int> tableau_score;
        private float duree_jeu;
        

        #region propriétés
        public Joueur[] Joueurs
        {
            get { return this.joueurs; }
            set { this.joueurs = value; }
        }
        public Dictionnaire Dico
        {
            get { return this.dico; }
            set { this.dico = value; }
        }
        public int NbJoueurs
        {
            get { return this.nb_joueurs; }
            set { this.nb_joueurs = value; }
        }
        public Dictionary<Joueur, int> Tableau_score
        {
            get { return this.tableau_score; }
            set { this.tableau_score = value; }
        }

        public float Duree_jeu
        {
            get { return this.duree_jeu; }
            set { this.duree_jeu = value; }
        }
        #endregion

        public Jeu(Dictionnaire dico, Dictionary<Joueur, int> tableau_score)
        {
            this.Dico = dico;
            this.Tableau_score = tableau_score;
        }

        /// <summary>
        ///  Demande le nombre des joueurs et demande le nom de chaque joueur totu en créant des instance de la classe joueur
        /// </summary>
        public void CreerJoueurs()
        {

            Console.WriteLine("\nEntrez le nombre de joueurs : ");
            int nb_joueur;
            bool nombre_valide;
            do
            {
                string entrée = Console.ReadLine();
                nombre_valide = int.TryParse(entrée, out nb_joueur) && nb_joueur >= 2;

                if (!nombre_valide)
                {
                    Console.WriteLine("Erreur : Vous devez entrer un nombre de joueur supérieur ou égale à 2");
                }
            } while (!nombre_valide);
            this.NbJoueurs = nb_joueur;
            Joueur[] joueurs = new Joueur[this.NbJoueurs];
            for (int i = 0; i < this.NbJoueurs; i++)
            {
                Console.WriteLine("\nEntrez le nom du joueur " + (i + 1) + " : ");
                List<string> mot = new List<string>();
                Plateau plateau = new Plateau();
                joueurs[i] = new Joueur(Console.ReadLine(), 0, mot, plateau);
            }
            this.Joueurs = joueurs;
        }

        /// <summary>
        /// Demande aux joueur la durée d'un tour
        /// </summary>
        /// <returns>le nombre de minute pour un tour</returns>
        public void DemanderDureeJeu()
        {
            Console.WriteLine("\nEntrez la durée d'un tour (en secondes) : ");
            int duree_jeu;
            bool nombre_valide;
            do
            {
                string entrée = Console.ReadLine();
                nombre_valide = int.TryParse(entrée, out duree_jeu);

                if (!nombre_valide)
                {
                    Console.WriteLine("Vous devez entrer un entier");
                }
            } while (!nombre_valide);
            this.Duree_jeu = duree_jeu;
        }

        /// <summary>
        /// Créer un plateau à partir de la taille voulu par les joueurs
        /// </summary>
        public void CreerPlateau(int i)
        {
            DE.CreerListeLettres();
            this.Joueurs[i].Plateau.CreerTableauDés();
            this.Joueurs[i].Plateau.CreerFacesSuperieures();
        }

        /// <summary>
        /// Créer le dictionnaire de mots à partir de la langue que les  joueurs ont décidé de jouer et créer le dictionnaire de mots trié
        /// </summary>
        public void CreerDico()
        {
            this.Dico.DemanderLangue();
            this.Dico.DicoTrié();
        }

        /// <summary>
        ///  Créer un tableau avec tout les scores des joueurs
        /// </summary>
        public void CreerTableauScore()
        {
            for (int i = 0; i < this.NbJoueurs; i++)
            {
                this.Tableau_score[this.Joueurs[i]] = this.Joueurs[i].Score;
            }
        }

        /// <summary>
        /// Demande la taille du plateau aux joueurs
        /// </summary>
        public void DemanderTaillePlateau()
        {
            int taille_plateau =4;
            Console.WriteLine("\nDe quelle taille sera la plateau ?  ");
            bool nombre_valide;
            do
            {
                string entrée = Console.ReadLine();
                nombre_valide = int.TryParse(entrée, out taille_plateau) && taille_plateau >= 3 && taille_plateau <= 8;

                if (!nombre_valide)
                {
                    Console.WriteLine("Vous devez entrer une taille de plateau comprise entre 3 et 8.");
                }
            } while (!nombre_valide);
            for (int i = 0; i < this.NbJoueurs; i++)
            {
                this.Joueurs[i].Plateau.Taille = taille_plateau;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Détermine le gangant c'est à dire celui qui a le score le plus haut (l'égalité est aussi prise en compte)
        /// </summary>
        /// <param name="tableau_score"></param>
        /// <returns>Liste de joueurs ayant gagnés</returns>
        public List<Joueur> Gagnant(Dictionary<Joueur, int> tableau_score)
        {
            List<Joueur> gagnants = new List<Joueur>();
            int max_score = tableau_score.Values.Max();
            foreach (var joueur in tableau_score)
            {
                if (joueur.Value == max_score)
                {
                    gagnants.Add(joueur.Key);
                }
            }
            return gagnants;
        }
        /// <summary>
        /// Vérifie si le mot tapé par le joueur existe dans la langue et s'il la disposition des dés permet de former ce mot
        /// </summary>
        /// <param name="mot">mot tapé par l'utilisateur</param>
        /// <returns>si le mot peut être comptabilisé</returns>
        public bool VerifierMot(string mot, int indice)
        {
            if (this.Dico == null || this.Joueurs[indice].Plateau == null)
            {
                Console.WriteLine("Erreur : Dico ou Plateau non initialisé.");
                return false;
            }
            bool t = true;
            if (this.Joueurs[indice].Plateau.EligibilitéMotPlateau(mot) == false || this.Dico.RechDicoRecursif(mot, 0, -2) == false)
            {
                t = false;
            }
            return t;
        }

        /// <summary>
        /// Bonus : Calcule le pourcentage de mots que le joueur à trouvé par rapport aux mots possibles qu'il aurait pu trouvé dans son plateau de lettres
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public float PourcentagesMotsTrouvés(int indice)
        {
            List<string> liste_mots_possibles = new List<string>();
            foreach (string mot in this.Dico.Dico_trié)
            {
                if (this.VerifierMot(mot, indice) == true)
                {
                    liste_mots_possibles.Add(mot);
                }
            }
            float nb_mots_possibles = liste_mots_possibles.Count;
            float nb_mots_trouvés = this.Joueurs[indice].Mots_trouvés.Count;
            float pourcentage = (float)Math.Round((nb_mots_trouvés * 100 / nb_mots_possibles), 2);
            this.Joueurs[indice].Mot_possibles = liste_mots_possibles;
            return pourcentage;
        }


        /// <summary>
        /// Affiche tous les mots possibles du plateau
        /// </summary>
        /// <param name="indice">correspond au plateau du (i+1)ème joueur du jeu</param>
        public void AfficherMotsPossibles(int indice)
        {
            Console.WriteLine("Tous les mots possibles sont : \n");
            string[] tab = this.Joueurs[indice].Mot_possibles.ToArray();
            for (int i=0; i<tab.Length; i++)
            {
                Console.Write(tab[i]);
                if(i<tab.Length-1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Génère le nuage de mots d'un joueur à partir du dictionnaire de mots trouvé (Key) et du scrore (Value) obtenue grace à ce mot
        /// </summary>
        public void NuageMots()
        {
            for (int i = 0; i < this.Joueurs.Length; i++) 
            {
                Nuage_de_mots nuage = new Nuage_de_mots(Joueurs[i].DictionnaireMotsScores());
                string nom_fichier = "Nuage_de_mots_" + Joueurs[i].Nom + ".png";
                nuage.GenererNuageDeMots(nom_fichier);
                Console.WriteLine("Le nuage de mots de " + Joueurs[i].Nom + " est enrengistré sous le nom : " + nom_fichier);
            }
        }

        /// <summary>
        /// Ouvre directement le nuage de mot après avoir fini le jeu 
        /// </summary>
        /// <param name="lien_fichier">lien du nuage de mot correspondant au joueur</param>
        public void AfficherNuage()
        {
            for (int i = 0; i < this.Joueurs.Length; i++)
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "Nuage_de_mots_" + Joueurs[i].Nom + ".png",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }

        /// <summary>
        /// Imbrique toutes les classes et leur fonctions ensemble ce qui permet d'executer tout le jeu
        /// </summary>
        public void LancerJeu()
        {
            bool rejouer = true;
            while (rejouer == true)
            {
                Console.Clear();
                Console.WriteLine("                                                        BOOGLE");
                this.CreerDico();
                this.CreerJoueurs();
                this.DemanderDureeJeu();
                this.DemanderTaillePlateau();
                for (int i = 0; i < this.NbJoueurs; i++)
                {
                    int compteur = 0;
                    Console.WriteLine("C'est au tour de " + this.Joueurs[i].Nom + ".");
                    Console.WriteLine("Pressez une touche pour lancer votre tour.");
                    Console.ReadKey();
                    Console.Clear();
                    this.CreerPlateau(i);
                    Console.Clear();
                    Console.WriteLine("                                                        BOOGLE");
                    this.Joueurs[i].Plateau.AfffichagePlateau();
                    Console.Write("Entrez un mot : ");
                    DateTime temps_debut = DateTime.Now;
                    TimeSpan duree = TimeSpan.FromSeconds(this.Duree_jeu);
                    this.Joueurs[i].CalculScore();
                    Console.Write("                                     Score : " + this.Joueurs[i].Score + "\n");
                    TimeSpan temps_restant = duree - (DateTime.Now - temps_debut);
                    Console.WriteLine("                                                     Temps restant : " + temps_restant.Seconds + " secondes.");
                    while (DateTime.Now - temps_debut < duree)
                    {
                        if(compteur >0)
                        {
                            compteur = 0;
                            this.Joueurs[i].CalculScore();
                            Console.Write("Entrez un mot : ");
                            Console.Write("                                     Score : " + this.Joueurs[i].Score + "\n");
                            temps_restant = duree - (DateTime.Now - temps_debut);
                            Console.WriteLine("                                                     Temps restant : " + temps_restant.Seconds + " secondes.");
                        }
                        if (Console.KeyAvailable == true)
                        {
                            string mot = Console.ReadLine();
                            Console.Clear();
                            Console.WriteLine("                                                        BOOGLE");
                            this.Joueurs[i].Plateau.AfffichagePlateau();
                            compteur++;
                            if (string.IsNullOrWhiteSpace(mot))
                            {
                                Console.WriteLine("Veuillez saisir un mot valide.\n");
                            }
                            else
                            {
                                mot = mot.Trim().ToUpper();
                                bool verif = this.VerifierMot(mot, i);
                                if (this.Joueurs[i].Contain(mot) == true)
                                {
                                    Console.WriteLine("\"" + mot + "\" a déja été entré.\n");
                                }
                                else if (verif == true)
                                {
                                    Console.WriteLine("\"" + mot + "\" est valide\n");
                                    this.Joueurs[i].AddMot(mot);
                                }
                                else
                                {
                                    Console.WriteLine("\"" + mot + "\" est invalide\n");
                                }
                            }
                        }
                    }
                    this.Joueurs[i].CalculScore();
                    Console.Clear();
                    Console.WriteLine("\nVotre tour est terminé.\n");
                    float pourcentage = this.PourcentagesMotsTrouvés(i);
                    Console.WriteLine("Vous avez trouvé " + pourcentage + " % des mots possibles sur ce plateau\n\n");
                    this.AfficherMotsPossibles(i);
                    Console.WriteLine();
                }
                Console.WriteLine("\nPressez une touche pour voir les résultats.\n");
                Console.ReadKey();
                this.CreerTableauScore();
                Console.Clear();
                Console.WriteLine("                                                        BOOGLE\n");
                int nb_gagagnants = this.Gagnant(this.Tableau_score).Count();
                if (nb_gagagnants > 1)
                {
                    Console.Write("EGALITÉ ENTRE : ");
                    for (int i = 0; i < nb_gagagnants; i++)
                    {
                        Console.Write(this.Joueurs[i].Nom);
                        if (i < nb_gagagnants - 1)
                        {
                            Console.Write(" et ");
                        }
                        else
                        {
                            Console.WriteLine(".\n");
                        }
                    }
                }
                else
                {
                    foreach (Joueur i in this.Gagnant(this.Tableau_score))
                    {
                        Console.Write("\nLE VAINQUEUR DE CETTE PARTIE AVEC UN SCORE DE " + i.Score + " EST...");
                        Thread.Sleep(2000);
                        Console.WriteLine("   " + i.Nom.ToUpper() + " !!!\n");
                    }
                }
                Console.WriteLine("\nTableau des scores : \n");
                for (int i = 0; i < NbJoueurs; i++)
                {
                    Console.WriteLine(this.Joueurs[i].ToString() + "\n");
                }
                Console.WriteLine();
                this.NuageMots();
                Console.WriteLine();
                Thread.Sleep(5500);
                this.AfficherNuage();
                string oui_non;
                do
                {
                    Console.WriteLine("\nVoulez vous rejouer ?\n");
                    oui_non = Console.ReadLine().Trim().ToUpper();

                } while (oui_non != "OUI" && oui_non != "NON");
                if (oui_non == "NON")
                {
                    rejouer = false;
                }
            }
        }
    }
}

