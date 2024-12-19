using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Boggle_VF
{
    public class Joueur
    {
        private string nom;
        private int score;
        private List<string> mots_trouvés;
        private Plateau plateau;
        private List<string> mot_possibles;
        public Joueur(string nom, int score, List<string> mots_trouvés, Plateau plateau)
        {
            this.nom = nom;
            this.score = 0;
            this.mots_trouvés = mots_trouvés;
            this.plateau = plateau;
        }
        #region propriétés
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }
        public List<string> Mots_trouvés
        {
            get { return this.mots_trouvés; }
            set { this.mots_trouvés = value; }
        }
        public Plateau Plateau
        {
            get { return this.plateau; }
            set { this.plateau = value; }
        }
        public List<string> Mot_possibles
        {
            get { return this.mot_possibles; }
            set { this.mot_possibles = value; }
        }
        #endregion

        /// <summary>
        /// Verifie si le mot tapé appartient déja aux mots trouvés par le joueur pendant la partie
        /// </summary>
        /// <param name="mot">mot à vérifier</param>
        /// <returns>bool</returns>
        public bool Contain(string mot)
        {
            if (this.Mots_trouvés.Contains(mot) == true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ajoute le mot trouvé dans la liste des mots du joueur 
        /// </summary>
        /// <param name="mot"></param>
        public void AddMot(string mot)
        {
            this.Mots_trouvés.Add(mot);
        }

        /// <summary>
        /// Retourne une chaine de caractère qui décrit le joueur (nom, score, mots trouvés)
        /// </summary>
        /// <returns>chaine de caractère</returns>
        public override string ToString()
        {
            string chaine = "";
            foreach (string mot in this.Mots_trouvés)
            {
                chaine = chaine + mot + ", ";
            }
            if (chaine != "")
            {
                chaine = chaine.Substring(0, chaine.Length - 2);
            }
            return ("Nom : " + this.Nom + "\nScore : " + this.Score + "\nMots trouvés : " + chaine);
        }

        /// <summary>
        /// Calcule le score obtenue par le joueur en fonction de la longueur du mot et des lettres qui les contiennent
        /// </summary>
        public void CalculScore()
        {
            this.Score = 0;
            if (this.Mots_trouvés.Count != 0)
            {
                foreach (string indice in Mots_trouvés)
                {
                    foreach (char caractere in indice)
                    {
                        this.Score = this.Score + DE.dico_lettres_poids_occurence[Convert.ToString(caractere)][0];
                    }
                }
            }
            else
            {
                this.Score = 0;
            }
        }

        /// <summary>
        /// Créer un dictionnaire qui prend en Key le mot trouvé par le joueur et en Value le score que rapporte le mot
        /// </summary>
        /// <returns>dictionnaire</returns>
        public Dictionary<string, int> DictionnaireMotsScores()
        {
            Dictionary<string, int> dico_mots_scores = new Dictionary<string, int>();
            if (this.Mots_trouvés != null && this.Mots_trouvés.Count > 0)
            {
                for (int i = 0; i < this.Mots_trouvés.Count; i++)
                {
                    string mot = this.Mots_trouvés[i];
                    int score = 0;
                    foreach (char caractères in mot)
                    {
                        score += DE.dico_lettres_poids_occurence[Convert.ToString(caractères)][0];
                    }
                    dico_mots_scores[mot] = score;
                }
            }
            return dico_mots_scores;
        }
    }
}

