using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Boggle_VF
{
    public class DE
    {
        private string[] faces;
        private string face_visible;
        public static Dictionary<string, int[]> dico_lettres_poids_occurence = new Dictionary<string, int[]>();
        public static List<string> lettres = new List<string>();
        public static Random random = new Random();

        #region propriétés
        public string[] Faces
        {
            get { return this.faces; }
            set { this.faces = value; }
        }
        public string Face_visible
        {
            get { return this.face_visible; }
            set { this.face_visible = value; }
        }
        #endregion

        /// <summary>
        /// Créer un dictionnaire à partir du fichier "lettres" qui va attribuer à chaque lettres (la clée) les valeurs d'occurences et le poids qui leur correspondent sous forme d'un tableau d'entiers
        /// </summary>
        /// <param name="lienfichier">fichier lettres</param>
        public static void CreerDico(string lienfichier)
        {
            try
            {
                foreach (string ligne in File.ReadLines(lienfichier))
                {
                    string[] separe = ligne.Split(';');
                    if (separe.Length >= 3)
                    {
                        string cle = separe[0].Trim().ToUpper();
                        int[] valeurs = new int[2];


                        int valeur1 = Convert.ToInt32(separe[1].Trim());
                        int valeur2 = Convert.ToInt32(separe[2].Trim());

                        valeurs[0] = valeur1;
                        valeurs[1] = valeur2;
                        dico_lettres_poids_occurence[cle] = valeurs;
                    }
                    else
                    {
                        Console.WriteLine($"La ligne ne contient pas suffisamment de données : {ligne}");
                    }
                }
            }
            catch (FileNotFoundException f)
            {
                Console.WriteLine("Le fichier n'existe pas");
                Console.WriteLine(f.Message);
            }
        }


        /// <summary>
        /// Ajoute à la liste "lettre" x fois la lettre en fonction de la fréquence d'occurence qui à été attribué à la lettre (qui correspond à la deuxième valeur du tableau d'entier de chaque clé)
        /// </summary>
        public static void CreerListeLettres()
        {
            try
            {
                CreerDico("Lettres.txt");
                foreach (KeyValuePair<string, int[]> indice in dico_lettres_poids_occurence)
                {
                    for (int i = 0; i < indice.Value[1]; i++)
                    {
                        lettres.Add(indice.Key);
                    }
                }
            }
            catch (FileNotFoundException f)
            {
                Console.WriteLine("Le fichier n'existe pas");
                Console.WriteLine(f.Message);
            }
        }

        /// <summary>
        /// Attribue un tableau de lettre correspondant aux faces d'un dé à l'attribut de classe "faces" de l'instance courrante
        /// </summary>
        public void TableauFaces()
        {
            string[] tableau = new string[6];
            for (int i = 0; i < 6; i++)
            {
                int h = random.Next(0, lettres.Count - 1);
                tableau[i] = lettres[h];
            }
            this.Faces = tableau;
        }

        /// <summary>
        /// attribue la lettre correspondant à la face visible à l'attribut de classe "visible" de l'instance courrante
        /// </summary>
        /// <param name="rand"></param>
        public void LancerDés()
        {
            this.Face_visible = this.Faces[random.Next(0, 5)];
        }

        /// <summary>
        /// Retourne une chaine de caractère qui décrit un dé (les faces du dé et la face visibles)
        /// </summary>
        /// <returns>chaine de caractère</returns>
        public override string ToString()
        {
            string S = "";
            for (int i = 0; i < this.Faces.Length; i++)
            {
                S = S + this.Faces[i];
                if (i < this.Faces.Length - 1)
                {
                    S = S + ", ";
                }
            }
            return ("Les faces du dé sont " + S + ".\nLa face visible est" + this.Face_visible);
        }
    }
}
