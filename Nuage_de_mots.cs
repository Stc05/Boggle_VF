using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;


namespace Boggle_VF
{
    class Nuage_de_mots
    {
        private Dictionary<string, int> mots_score;
        public Nuage_de_mots(Dictionary<string, int> mots)
        {
            this.mots_score = mots;
        }

        public Dictionary<string, int> Mots_Score
        {
            get { return this.mots_score; }
            set { this.mots_score = value; }
        }

        /// <summary>
        /// Génère un nuage de mot à partir d'un dictionnaire de mots (key) et du score obtenu gràce au mot (value)
        /// </summary>
        /// <param name="cheminFichier">nom du fichier auquel est enrengistré le nuage de mot du joueur</param>
        public void GenererNuageDeMots(string nom_fichier)
        {
            int largeur = 800;
            int hauteur = 600;

            using (Bitmap image = new Bitmap(largeur, hauteur))
            {
                using (Graphics graphique = Graphics.FromImage(image))
                {
                    if (this.mots_score == null || !this.mots_score.Any())
                    {
                        graphique.Clear(Color.White);
                        string message = "Pas de mots trouvés";
                        using (Font police = new Font("Arial", 50, FontStyle.Bold))
                        {
                            SizeF tailleTexte = graphique.MeasureString(message, police);
                            float x = (largeur - tailleTexte.Width) / 2; 
                            float y = (hauteur - tailleTexte.Height) / 2; 

                            using (Brush pinceau = new SolidBrush(Color.Navy))
                            {
                                graphique.DrawString(message, police, pinceau, new PointF(x, y));
                            }
                        }
                    }
                    else
                    {
                        using (Brush pinceauDégradé = new LinearGradientBrush(new Point(0, 0), new Point(largeur, hauteur), Color.LightBlue, Color.White))
                        {
                            graphique.FillRectangle(pinceauDégradé, 0, 0, largeur, hauteur);
                        }

                        Random aleatoire = new Random();
                        float facteurAmplification = 6.0f; 
                        foreach (var mot in this.Mots_Score)
                        {
                            string texte = mot.Key;
                            int poids = mot.Value+3;

                            float taillePolice = poids * facteurAmplification;

                            using (Font police = new Font("Arial", taillePolice, FontStyle.Bold))
                            {
                                Color couleurMot = Color.FromArgb(aleatoire.Next(256), aleatoire.Next(256), aleatoire.Next(256));
                                using (Brush pinceau = new SolidBrush(couleurMot))
                                {
                                    SizeF tailleTexte = graphique.MeasureString(texte, police);
                                    int x = aleatoire.Next(0, Math.Max(1, largeur - (int)tailleTexte.Width));
                                    int y = aleatoire.Next(0, Math.Max(1, hauteur - (int)tailleTexte.Height));

                                    graphique.DrawString(texte, police, pinceau, new PointF(x, y));
                                }
                            }
                        }
                    }
                }
                image.Save(nom_fichier, ImageFormat.Png);
            }
        }

    }
}



