using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodiceFiscale
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            // Richiamo il metodo CreatePerson.Create per creare un'istanza di Person
            Person person = await PersonFactory.Create();

            Console.WriteLine(person.ToString());
            Console.WriteLine($"Codice Fiscale: {await person.CodiceFiscale()}");
        }
    }
 }

