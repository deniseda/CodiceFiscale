using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodiceFiscale
{
    public class CountryRepository
    {
        private readonly string _url;

        public CountryRepository(string url = "https://raw.githubusercontent.com/GCCodes/Codice-Fiscale-API/master/comuni.json")
        {
            _url = url;
        }

        public async Task<List<Country>> LoadCountryFromJson()
        {
            using var client = new HttpClient();
            string jsonString = await client.GetStringAsync(_url);
            var countries = JsonSerializer.Deserialize<List<Country>>(jsonString);
            if(countries == null || countries.Count == 0)
            {
                throw new Exception("Dati comune non disponibili o non caricati correttamente");
            }
            return countries;

        }
// 1) data una stringa restituisce un bool se esiste il comune inserito dall'utente
        public async Task<bool> IsCountryExist(string countryname)
        {
            var countries = await LoadCountryFromJson();
            return countries.Any(c => c.Nome.Equals(countryname, StringComparison.OrdinalIgnoreCase));
        }
        

// 2) data la stringa del comune inserito restituisca il codice catastale 
         
        public async Task<string?> GetCodiceCatastale(string countryname)
        {
            var countries = await LoadCountryFromJson();
            var country = countries.FirstOrDefault(c => c.Nome.Equals(countryname, StringComparison.OrdinalIgnoreCase));
            return country?.CodiceCatastale;
        }


    }
}
