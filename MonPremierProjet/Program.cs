using System;
using System.Collections.Generic;
using System.IO;

namespace BanqueApp
{
    // Classe de base pour tous les comptes
    abstract class Compte
    {
        public string NumeroCompte { get; private set; }
        public string Titulaire { get; private set; }
        public decimal Solde { get; protected set; }
        public List<string> Historique { get; private set; }

        protected Compte(string numero, string titulaire)
        {
            NumeroCompte = numero;
            Titulaire = titulaire;
            Solde = 0;
            Historique = new List<string>();
            Historique.Add($"Compte créé : {numero} | Titulaire : {titulaire}");
        }

        public virtual void Depot(decimal montant)
        {
            Solde += montant;
            Historique.Add($"Dépôt : +{montant} | Nouveau solde : {Solde}");
            Console.WriteLine($"Vous avez déposé : {montant} €.");
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
            Console.WriteLine($"Vous avez retiré : {montant} €.");
        }

        public void AfficherInfos()
        {
            Console.WriteLine($"\n--- Informations compte {NumeroCompte} ---");
            Console.WriteLine($"Titulaire : {Titulaire}");
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

    class CompteCourant : Compte
    {
        public CompteCourant(string numero, string titulaire) : base(numero, titulaire) { }
    }

    class CompteEpargne : Compte
    {
        public CompteEpargne(string numero, string titulaire) : base(numero, titulaire) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans votre application bancaire !");
            Console.WriteLine("Appuyez sur Entrée pour afficher le menu.");
            Console.ReadLine();

            // Création des comptes
            Compte courant = new CompteCourant("CC001", "Mabou Kevin");
            Compte epargne = new CompteEpargne("CE001", "Mabou Kevin");

            bool quitter = false;

            while (!quitter)
            {
                AfficherMenu();
                string? choix = Console.ReadLine()?.ToUpper();

                switch (choix)
                {
                    case "I":
                        courant.AfficherInfos();
                        epargne.AfficherInfos();
                        break;
                    case "CS":
                        Console.WriteLine($"Solde du compte courant : {courant.Solde} €");
                        break;
                    case "CD":
                        EffectuerDepot(courant);
                        break;
                    case "CR":
                        EffectuerRetrait(courant);
                        break;
                    case "ES":
                        Console.WriteLine($"Solde du compte épargne : {epargne.Solde} €");
                        break;
                    case "ED":
                        EffectuerDepot(epargne);
                        break;
                    case "ER":
                        EffectuerRetrait(epargne);
                        break;
                    case "X":
                        quitter = true;
                        break;
                    default:
                        Console.WriteLine("Option invalide !");
                        break;
                }

                if (!quitter)
                {
                    Console.WriteLine("\nAppuyez sur Entrée pour afficher le menu.");
                    Console.ReadLine();
                }
            }

            // Sauvegarde des historiques
            courant.SauvegarderDansFichier();
            epargne.SauvegarderDansFichier();

            Console.WriteLine("Transactions enregistrées dans les fichiers texte.");
            Console.WriteLine("Merci d'avoir utilisé l'application bancaire !");
        }

        static void AfficherMenu()
        {
            Console.WriteLine("\nVeuillez sélectionner une option ci-dessous :");
            Console.WriteLine("[I]  Voir les informations sur le titulaire du compte");
            Console.WriteLine("[CS] Compte courant - Consulter le solde");
            Console.WriteLine("[CD] Compte courant - Déposer des fonds");
            Console.WriteLine("[CR] Compte courant - Retirer des fonds");
            Console.WriteLine("[ES] Compte épargne - Consulter le solde");
            Console.WriteLine("[ED] Compte épargne - Déposer des fonds");
            Console.WriteLine("[ER] Compte épargne - Retirer des fonds");
            Console.WriteLine("[X]  Quitter");
            Console.Write("Votre choix : ");
        }

        static void EffectuerDepot(Compte compte)
        {
            Console.Write("Quel montant souhaitez-vous déposer ? ");
            if (decimal.TryParse(Console.ReadLine(), out decimal montant) && montant > 0)
            {
                compte.Depot(montant);
            }
            else
            {
                Console.WriteLine("Montant invalide !");
            }
        }

        static void EffectuerRetrait(Compte compte)
        {
            Console.Write("Quel montant souhaitez-vous retirer ? ");
            if (decimal.TryParse(Console.ReadLine(), out decimal montant) && montant > 0)
            {
                compte.Retrait(montant);
            }
            else
            {
                Console.WriteLine("Montant invalide !");
            }
        }
    }
}
