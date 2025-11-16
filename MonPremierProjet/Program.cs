using System;
using System.Collections.Generic;
using System.IO;

namespace BanqueApp
{
    // Classe abstraite : base commune pour tous les comptes
    abstract class Compte
    {
        public string NumeroCompte { get; set; }
        public decimal Solde { get; protected set; }
        public List<string> Historique { get; private set; }

        public Compte(string numero)
        {
            NumeroCompte = numero;
            Solde = 0;
            Historique = new List<string>();
            Historique.Add($"Compte créé : {numero}");
        }

        public virtual void Depot(decimal montant)
        {
            Solde += montant;
            Historique.Add($"Dépôt : +{montant} | Nouveau solde : {Solde}");
        }

        public virtual void Retrait(decimal montant)
        {
            if (montant > Solde)
            {
                Console.WriteLine("Solde insuffisant !");
                Historique.Add($"Retrait échoué : -{montant} (solde insuffisant)");
                return;
            }

            Solde -= montant;
            Historique.Add($"Retrait : -{montant} | Nouveau solde : {Solde}");
        }

        public void AfficherInfos()
        {
            Console.WriteLine($"\n--- Informations compte {NumeroCompte} ---");
            Console.WriteLine($"Type : {this.GetType().Name}");
            Console.WriteLine($"Solde : {Solde} €");
            Console.WriteLine("-----------------------------------------");
        }

        public void SauvegarderDansFichier()
        {
            string nomFichier = $"{NumeroCompte}.txt";
            File.WriteAllLines(nomFichier, Historique);
        }
    }

    // Compte courant
    class CompteCourant : Compte
    {
        public CompteCourant(string numero) : base(numero) { }
    }

    // Compte épargne
    class CompteEpargne : Compte
    {
        public CompteEpargne(string numero) : base(numero) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans votre application bancaire !");
            Console.WriteLine("Création de vos comptes...");

            Compte courant = new CompteCourant("CC001");
            Compte epargne = new CompteEpargne("CE001");

            bool quitter = false;

            while (!quitter)
            {
                Console.WriteLine("\n===== MENU BANCAIRE =====");
                Console.WriteLine("1 - Voir informations comptes");
                Console.WriteLine("2 - Déposer");
                Console.WriteLine("3 - Retirer");
                Console.WriteLine("4 - Quitter");
                Console.Write("Votre choix : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        courant.AfficherInfos();
                        epargne.AfficherInfos();
                        break;

                    case "2":
                        EffectuerOperation(courant, epargne, true);
                        break;

                    case "3":
                        EffectuerOperation(courant, epargne, false);
                        break;

                    case "4":
                        quitter = true;
                        break;

                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }

            // Sauvegarde finale
            courant.SauvegarderDansFichier();
            epargne.SauvegarderDansFichier();

            Console.WriteLine("\nFichiers de transactions générés !");
            Console.WriteLine("Merci d'avoir utilisé l'application bancaire.");
        }

        static void EffectuerOperation(Compte courant, Compte epargne, bool depot)
        {
            Console.WriteLine("\nSur quel compte ?");
            Console.WriteLine("1 - Compte courant");
            Console.WriteLine("2 - Compte épargne");
            Console.Write("Choix : ");
            string cp = Console.ReadLine();

            Compte compteChoisi = (cp == "1") ? courant : (cp == "2") ? epargne : null;

            if (compteChoisi == null)
            {
                Console.WriteLine("Compte inconnu.");
                return;
            }

            Console.Write("Montant : ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal montant))
            {
                Console.WriteLine("Montant invalide.");
                return;
            }

            if (depot)
                compteChoisi.Depot(montant);
            else
                compteChoisi.Retrait(montant);
        }
    }
}
