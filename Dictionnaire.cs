using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using Boggle_VF;

namespace Boggle_VF
{
    public class Dictionnaire
    {
        private string fichier_dico_brut;
        private string langue;
        private string[] dico_trié;


        #region propriétés

        public string Langue
        {
            get { return this.langue; }
            set { this.langue = value; }
        }
        public string Fichier_dico_brut
        {
            get { return this.fichier_dico_brut; }
            set { this.fichier_dico_brut = value; }
        }
        public string[] Dico_trié
        {
            get { return this.dico_trié; }
            set { this.dico_trié = value; }
        }
        #endregion

        /// <summary>
        /// Attribue le bon fichier contenant tous les mots de la langue sélectionnée
        /// </summary>
        public void DemanderLangue()
        {
            Console.WriteLine("\nVeuillez choisir une langue (francais/anglais) : ");
            string choix = Console.ReadLine().Trim().ToLower();
            while (choix != "francais" && choix != "anglais")
            {
                Console.WriteLine("\nChoix invalide. Veuillez entrer 'francais' ou 'anglais' : ");
                choix = Console.ReadLine().Trim().ToLower();
            }
            this.Langue = choix;
            if (this.Langue == "francais")
            {
                this.Fichier_dico_brut = "MotsPossiblesFR.txt";
            }
            else
            {
                this.Fichier_dico_brut = "MotsPossiblesEN.txt";
            }
        }

        /// <summary>
        /// Créer un tableau à partir du fichier_brut (MotsPossibles) fourni par la prof qui attribue à chaque mot (séparé d'un espace) une case du tableau
        /// </summary>
        /// <returns>tableau de tous les mots du dictionnaire attibué à une langue</returns>
        public string[] SeparationMotDico()
        {
            string[] mots_dico=null;
            try
            {
                string[] separation = File.ReadAllText(this.Fichier_dico_brut).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                mots_dico =new string[separation.Length];
                Array.Copy(separation,mots_dico, mots_dico.Length);
            }
            catch(FileNotFoundException f)
            {
                Console.WriteLine(f.Message);
            }
            return mots_dico;
        }

        /// <summary>
        /// Trie fusion
        /// </summary>
        /// <param name="tableau">Tableau des mots du dictionnaire</param>
        /// <returns>tableau de mots trié</returns>
        public static string[] TrieDico(string[] tableau)
        {
            if (tableau.Length <= 1)
            {
                return tableau;
            }
            int milieu = tableau.Length / 2;
            string[] gauche = new string[milieu];
            string[] droite = new string[tableau.Length - milieu];

            Array.Copy(tableau, 0, gauche, 0, milieu);
            Array.Copy(tableau, milieu, droite, 0, tableau.Length - milieu);

            gauche = TrieDico(gauche);
            droite = TrieDico(droite);

            return Fusionner(gauche, droite);
        }

        /// <summary>
        /// Fusionne les tableaux pour le tri fusion
        /// </summary>
        /// <param name="gauche">tableau de mots de la partie gauche</param>
        /// <param name="droite">tableau de mots de la partie droite</param>
        /// <returns>tableau fusionnée des deux tableaux (droite et gauche)</returns>
        public static string[] Fusionner(string[] gauche, string[] droite)
        {
            string[] fusion_tableaux = new string[gauche.Length + droite.Length];
            int indexGauche = 0;
            int indexDroite = 0;
            int index_tableaux_fusionés = 0;

            while (indexGauche < gauche.Length && indexDroite < droite.Length)
            {
                if (string.Compare(gauche[indexGauche], droite[indexDroite]) <= 0)
                {
                    fusion_tableaux[index_tableaux_fusionés] = gauche[indexGauche];
                    indexGauche++;
                }
                else
                {
                    fusion_tableaux[index_tableaux_fusionés] = droite[indexDroite];
                    indexDroite++;
                }
                index_tableaux_fusionés++;
            }
            while (indexGauche < gauche.Length)
            {
                fusion_tableaux[index_tableaux_fusionés] = gauche[indexGauche];
                indexGauche++;
                index_tableaux_fusionés++;
            }
            while (indexDroite < droite.Length)
            {
                fusion_tableaux[index_tableaux_fusionés] = droite[indexDroite];
                indexDroite++;
                index_tableaux_fusionés++;
            }
            return fusion_tableaux;
        }

        /// <summary>
        /// Sauvegarde le tableau de tous les mots du dictionnaire trié dans un fichier pour pouvoir être réutilisé sans avoir besoin de repasser par la methode SeparationMotDico et TrieDico pour optimiser la complexité temporelle
        /// </summary>
        public void EcritureDicoTrié()
        {
            if (this.Langue == "anglais")
            {
                File.AppendAllLines("Dico_anglais_trie.txt", this.Dico_trié);
            }
            else
            {
                File.AppendAllLines("Dico_francais_trie.txt", this.Dico_trié);

            }
        }

        /// <summary>
        /// Permet de générer le tableau trié de mots à partir du fichier brut ou du fichier traité (dictionnaire trié) et l'attribuer à l'attribut dico_trié de l'instance courrante
        /// </summary>
        public void DicoTrié()
        {
            if (this.Langue == "anglais")
            {
                try
                {
                    this.Dico_trié = File.ReadAllLines("Dico_anglais_trie.txt");
                }
                catch (FileNotFoundException f)
                {
                    this.Dico_trié = TrieDico(this.SeparationMotDico());
                    this.EcritureDicoTrié();
                }
            }
            else
            {
                try
                {
                    this.Dico_trié = File.ReadAllLines("Dico_francais_trie.txt");

                }
                catch (FileNotFoundException f)
                {
                    this.Dico_trié = TrieDico(this.SeparationMotDico());
                    this.EcritureDicoTrié();
                }
            }
        }

        /// <summary>
        /// Rechercher un mot dans le dicitonnaire de facon recursive
        /// </summary>
        /// <param name="mot">mot à chercher</param>
        /// <param name="debut">valeur initiale du tableau à parcourir</param>
        /// <param name="fin">valeur finale du tableau à parcourir</param>
        /// <returns> retourne true si le mot appartient au dictionnaire, sinon false</returns>
        public bool RechDicoRecursif(string mot, int debut = 0, int fin = -2)
        {
            if (fin == -2)
            {
                fin = this.Dico_trié.Length - 1;
            }
            if (debut > fin)
            {
                return false;

            }
            int milieu = (debut + fin) / 2;
            if (this.Dico_trié[milieu] == mot)
            {
                return true;
            }
            else
            {
                if (String.Compare(this.Dico_trié[milieu], mot) < 0)
                {
                    return RechDicoRecursif(mot, milieu + 1, fin);
                }
                else
                {
                    return RechDicoRecursif(mot, debut, milieu - 1);
                }
            }
        }

        /// <summary>
        /// Compte pour chaque lettre le nombre de mots du dictionnaire commencant par celles ci
        /// </summary>
        /// <returns>renvoi un tableau de taille 26 qui contient le nombre pour de mots pour chaque lettres</returns>
        public int[] NombreMotsParLettre()
        {
            int[] nombres_Mots_Par_Lettre = new int[26];
            for (int i = 0; i < 26; i++)
            {
                int compteur = 0;
                for (int j = 0; j < this.Dico_trié.Length; j++)
                {
                    if (this.Dico_trié[j][0] == (char)('A' + i))
                    {
                        compteur++;
                    }
                }
                nombres_Mots_Par_Lettre[i] = compteur;
            }
            return nombres_Mots_Par_Lettre;
        }


        /// <summary>
        /// Determine la taille du mot le plus long du dictionnaire
        /// </summary>
        /// <returns>la tailledu mot le plus long</returns>
        public int TailleMotPlusLong()
        {
            int taille = this.Dico_trié[0].Length;
            for (int i = 1; i < this.Dico_trié.Length; i++)
            {
                if (taille < this.Dico_trié[i].Length)
                {
                    taille = this.Dico_trié[i].Length;
                }
            }
            return taille;
        }



        /// <summary>
        /// Calcule le nombre de mot dans le dictionnaire qui a une certaine longueur
        /// </summary>
        /// <returns>tableau de nombre de mot correspondant aux differentes tailles par ordre croissant</returns>
        public int[] NombreMotParLongueur()
        {
            int[] nombre_Mot_Par_Longueur = new int[this.TailleMotPlusLong()];
            for (int i = 0; i < this.Dico_trié.Length; i++)
            {
                nombre_Mot_Par_Longueur[this.Dico_trié[i].Length - 1]++;
            }
            return nombre_Mot_Par_Longueur;
        }

        /// <summary>
        /// Créer une chaine de caradctère qui contient les informations sur le nombre de mots par longueur et par lettre, mais aussi sur la langue du dictionnaire
        /// </summary>
        /// <returns>retourner la chaine de caracctère</returns>
        public override string ToString()
        {
            string chaine = "Nombre de mots par longueur :";
            for (int i = 0; i < this.NombreMotParLongueur().Length; i++)
            {

                chaine += "\nil y a " + this.NombreMotParLongueur()[i] + " mots qui ont une longuer de " + (i + 1) + "lettres";

            }
            chaine += "\n\nNombre de mots par lettre";
            for (int j = 0; j < this.NombreMotsParLettre().Length; j++)
            {
                chaine += "\nil y a " + this.NombreMotsParLettre()[j] + " qui commence par la lettre " + (char)(j + 'A');
            }
            chaine += "\n\nLangue: " + this.Langue;
            return chaine;
        }
    }
}
