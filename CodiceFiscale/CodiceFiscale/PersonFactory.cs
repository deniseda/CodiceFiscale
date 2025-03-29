using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodiceFiscale
{
    public class PersonFactory
    {
        //List<Country> comuni
        public static async Task<Person> Create()
        {
            var countryRepository = new CountryRepository();
            List<Country> comuni = await countryRepository.LoadCountryFromJson();

            Console.WriteLine("Inserisci il nome:");
            string nome = GetStringFromUser();

            Console.WriteLine("Inserisci il cognome:");
            string cognome = GetStringFromUser();

            Console.WriteLine("Inserisci la data di nascita (AAAA-MM-GG):");
            DateOnly dataNascita;
            while (!DateOnly.TryParse(Console.ReadLine(), out dataNascita))
            {
                Console.WriteLine("Data non valida. Riprova (AAAA-MM-GG):");
            }

            Console.WriteLine("Inserisci il genere (Male/Female):");
            Genre genere;
            while (!Enum.TryParse(Console.ReadLine(), true, out genere))
            {
                Console.WriteLine("Genere non valido. Riprova (Male/Female):");
            }

            Console.WriteLine("Inserisci la città:");
            string città = GetStringFromUser();

            // Controlla che la città esista nella lista dei comuni
            if (await countryRepository.IsCountryExist(città) == false)
            {
                Console.WriteLine($"Attenzione: La città '{città}' non è valida!");
            }

            return new Person(nome, cognome, dataNascita, genere, città);
        }

        public static string GetStringFromUser()
        {
            bool exit = false;
            string? userInput = null;
            while (exit == false)
            {
                userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Il valore non può essere vuoto!");
                }
                else
                {
                    exit = true;
                    break;
                }
            }
            return userInput!;
        }
    }
}
