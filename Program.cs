using Boggle_VF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Boggle_VF
{
    public class Program
    {
        static void Main(string[] args)
        {

            Dictionnaire dico1 = new Dictionnaire();
            Dictionary<Joueur, int> tableauscore1 = new Dictionary<Joueur, int>();
            Jeu jeu1 = new Jeu(dico1, tableauscore1);
            jeu1.Lancerlejeu();
        }
    }
}
