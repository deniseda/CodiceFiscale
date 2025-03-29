using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace CodiceFiscale
{
    public class RecordCatastale
    {
        [JsonPropertyName("codice")]
        public string Codice { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }


    }


    public class Country 
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("codice")]
        public string Codice { get; set; }

        [JsonPropertyName("zona")]
        public RecordCatastale Zona {  get; set; }

        [JsonPropertyName("regione")]
        public RecordCatastale Regione { get; set; }

        [JsonPropertyName("provincia")]
        public RecordCatastale Provincia { get; set; }

        [JsonPropertyName("sigla")]
        public string Sigla { get; set; }

        [JsonPropertyName("codiceCatastale")]
        public string CodiceCatastale { get; set; }

        [JsonPropertyName("cap")]
        public IList<string> Cap { get; set; }

        [JsonPropertyName("popolazione")]
        public int Popolazione { get; set; }
    }
}
