using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Globalization;

namespace CodiceFiscale
{

    public enum Genre
    {
        Male, 
        Female 
    }

    public class Person
    {
        public string Name { get; }
        public string Surname { get; }
        public DateOnly DateBirth { get; }
        public Genre Genre { get; protected set; } //enum
        public string BirthdayCountry { get; }


        public Person(string name, string surname, DateOnly datebirth, Genre genre, string birthcountry)
        {
            Name = name; 
            Surname = surname;
            DateBirth = datebirth;
            Genre = genre;
            BirthdayCountry = birthcountry;

        }


        public override string ToString()
        {
            // enum per il genere  
            return $"Nome: {Name}, Cognome: {Surname}, Data di nascita: {DateBirth}, Genere: {(Genre == Genre.Female ? "Femminile" : "Maschile")}, Paese di nascita: {BirthdayCountry}";
        }

        public async Task<string> CodiceFiscale()
        {


            string codeSurname = ExtractCode(Surname);

            string codeName = ExtractCode(Name, isName: true); 

            string year = DateBirth.Year.ToString().Substring(2);

            string month = ComputeMonth(DateBirth.Month);

            string day = (Genre == Genre.Female ? DateBirth.Day + 40 : DateBirth.Day).ToString("D2");

            string codeCountry = await ComputeCodeCountry(BirthdayCountry);

            var partialFiscalCode = $"{codeSurname}{codeName}{year}{month}{day}{codeCountry}";

            string controlCharacter = ComputeControlCharacter(partialFiscalCode);

            return $"{partialFiscalCode}{controlCharacter}";
        }



        // considera le prime le 3 consonanti, in assenza prende le vocali. 
        // se il nome fosse corto e non ci fossero sufficienti caratteri inserisce una X 

        private string ExtractCode(string input, bool isName = false)
        {
            // rimuove eventuali caratteri speciali
            input = new string(input.Where(char.IsLetter).ToArray());

            string consonants = new string(input.Where(c => !"AEIOUÀÈÌÒÙÉ".Contains(char.ToUpper(c))).ToArray());
            string vowels = new string(input.Where(c => "AEIOUÀÈÌÒÙÉ".Contains(char.ToUpper(c))).ToArray());

            if (isName && consonants.Length > 3)
            {
                // Regola del nome: la prima, la terza e la quarta consonante.
                consonants = $"{consonants[0]}{consonants[2]}{consonants[3]}";
            }

            string code = consonants + vowels;

            // Completa con 'X' se meno di 3 caratteri
            return code.Length >= 3 ? code.Substring(0, 3).ToUpper() : code.PadRight(3, 'X').ToUpper();
        }

        private string ComputeMonth(int month)
        {
            return "ABCDEHLMPRST"[month - 1].ToString();
        }


        //private static string ComputeCodeCouty(string nomeComune, List<Country> comuni)
        //{
        //    var comune = comuni.FirstOrDefault(c => c.Nome.Equals(nomeComune, StringComparison.OrdinalIgnoreCase));
        //    return comune != null ? comune.CodiceCatastale : "XXX"; // "XXX" nel caso in cui il comune non venga trovato
        //}


        private static async Task<string> ComputeCodeCountry(string nomeComune)
        {
            var countryRepository = new CountryRepository();
            var codiceCatastale = await countryRepository.GetCodiceCatastale(nomeComune);
            return codiceCatastale!;
        }


        // sola lettura 
        private static readonly int[] Dispari = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
        private static readonly int[] Pari = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        private static readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string ComputeControlCharacter(string partialFiscalCode)
        {
            int sum = 0;

            // Ciclo su ogni carattere del codice fiscale
            for (int i = 0; i < partialFiscalCode.Length; i++)
            {
                char c = partialFiscalCode[i];

                if (char.IsDigit(c))
                {
                    // Conversione da char a numero
                    int digit = c - '0';
                    sum += (i % 2 == 0) ? Dispari[digit] : Pari[digit];
                }
                else if (char.IsLetter(c))
                {
                    // Conversione da char a indice alfabetico
                    int index = c - 'A';
                    sum += (i % 2 == 0) ? Dispari[index] : Pari[index];
                }
                else
                {
                    throw new ArgumentException($"Carattere non valido nel codice fiscale: {c}");
                }
            }

            // Calcolo del resto modulo 26
            int remainder = sum % 26;

            // Restituzione del carattere di controllo
            return Alphabet[remainder].ToString();
        }
         

    }


}

