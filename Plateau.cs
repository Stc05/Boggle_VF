using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace Boggle_VF
{
    public class Plateau
    {
        private DE[] tableau_dés;
        private string[,] faces_supérieures;
        private int taille;

        #region propriétés
        public string[,] Faces_Superieures
        {
            get { return this.faces_supérieures; }
            set { this.faces_supérieures = value; }
        }
        public DE[] Tableau_Dés
        {
            get { return this.tableau_dés; }
            set { this.tableau_dés = value; }
        }
        public int Taille
        {
            get { return taille; }
            set { taille = value; }
        }
        #endregion


        /// <summary>
        /// Attribut à l'attribut de classe tableau_dés de l'instance courrante un tableau de dé qui va composer le plateau
        /// </summary>
        public void CreerTableauDés()
        {
            DE[] tab = new DE[this.Taille * this.Taille];
            for (int i = 0; i < tab.Length; i++)
            {
                DE de = new DE();
                de.TableauFaces();
                de.LancerDés();
                tab[i] = de;
            }
            this.Tableau_Dés = tab;
        }

        /// <summary>
        /// Attribut à l'attribut de classe faces_supérieures de l'instance courrante un tableau des faces supérieures qui va composé le plateau
        /// </summary>
        public void CreerFacesSuperieures()
        {
            string[,] faces_superieures = new string[this.Taille, this.Taille];
            this.Faces_Superieures = faces_superieures;
            int compteur = 0;
            for (int i = 0; i < this.Taille; i++)
            {
                for (int j = 0; j < this.Taille; j++)
                {
                    this.Faces_Superieures[i, j] = this.Tableau_Dés[compteur].Face_visible;
                    compteur++;
                }
            }
        }

        /// <summary>
        /// Affiche le plateau avec toutes les lettres des phases visibles des dés
        /// </summary>
        public void AfffichagePlateau()
        {
            string chaine = "";
            for (int i = 0; i < this.Faces_Superieures.GetLength(0); i++)
            {
                for (int j = 0; j < this.Faces_Superieures.GetLength(1); j++)
                {
                    chaine += this.Faces_Superieures[i, j] + " ";
                }
                chaine += "\n";
            }
            Console.WriteLine(chaine);
        }

        /// <summary>
        /// Retourne une chaine de caractère décrivant le plateau (taille et faces visibles)
        /// </summary>
        /// <returns> chaine de caractère</returns>
        public override string ToString()
        {
            string chaine = "Le plateau est de taille " + this.Taille + "*" + this.Taille + " et les faces visibles sont : ";
            for (int i = 0; i < this.Faces_Superieures.GetLength(0); i++)
            {
                for (int j = 0; j < this.Faces_Superieures.GetLength(1); j++)
                {
                    chaine += this.Faces_Superieures[i, j];
                    if ((i < this.Faces_Superieures.GetLength(0) - 1) && (j < this.Faces_Superieures.GetLength(1) - 1))
                    {
                        chaine += ", ";
                    }
                }
            }
            return chaine;
        }


        /// <summary>
        /// Vérifie si le mots inséré par utilisateur est éligible c'est à dire que le mot se forme bien sur le plateau (à la verrticale, à l'horizontal ou en diagonal)
        /// </summary>
        /// <param name="mot">mots insérré par le joueur</param>
        /// <returns>booleen</returns>
        public bool EligibilitéMotPlateau(string mot)
        {
            if (mot.Length > this.Taille * this.Taille)
            {
                return false;
            }

            int lignes = this.Faces_Superieures.GetLength(0);
            int colonnes = this.Faces_Superieures.GetLength(1);

            for (int ligne = 0; ligne < lignes; ligne++)
            {
                for (int colonne = 0; colonne < colonnes; colonne++)
                {
                    if (this.Faces_Superieures[ligne, colonne] == mot[0].ToString())
                    {
                        bool[,] cases_visitées = new bool[lignes, colonnes];
                        if (RechercheAdjacente(mot, ligne, colonne, cases_visitées, 0, this.Faces_Superieures) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Vérifie si le mot peut etre formé sur le plateau et vérfie aussi si on ne passe pas deux fois sur la meme lettre
        /// </summary>
        /// <param name="mot">mot inséré par l'utilisateur</param>
        /// <param name="ligne">lettre à la ième ligne du plateau à vérifié s'il compose le mot à chercher</param>
        /// <param name="colonne">lettre à la ième colonne du plateau à vérifié s'il compose le mot à chercher</param>
        /// <param name="cases_visitées">tableau de bool qui met true si on est déjà passé par la case</param>
        /// <param name="indice">compteur</param>
        /// <param name="faces_supérieures">plateau</param>
        /// <returns></returns>
        public static bool RechercheAdjacente(string mot, int ligne, int colonne, bool[,] cases_visitées, int indice, string[,] faces_supérieures)
        {
            if (indice == mot.Length)
            {
                return true;
            }
            if (ligne < 0 || ligne >= faces_supérieures.GetLength(0) || colonne < 0 || colonne >= faces_supérieures.GetLength(1) || cases_visitées[ligne, colonne] || Convert.ToChar(faces_supérieures[ligne, colonne]) != mot[indice])
            {
                return false;
            }
            cases_visitées[ligne, colonne] = true;
            int[] deplacementsLignes = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] deplacementsColonnes = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int dir = 0; dir < 8; dir++)
            {
                if (RechercheAdjacente(mot, ligne + deplacementsLignes[dir], colonne + deplacementsColonnes[dir], cases_visitées, indice + 1, faces_supérieures))
                {
                    return true;
                }
            }

            cases_visitées[ligne, colonne] = false;

            return false;
        }

    }
}


