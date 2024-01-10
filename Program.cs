/* Pegar os dados de temperatura em sabara MG e salvar no Banco */

using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class ObjetoURL
{
    public string city { get; set; }
    public string url { get; set; }
}

namespace WebScraperClima
{
   class Program 
   {
        static void Main(String[] args)
        {
            //Send get request to Climatempo            
            // string urlSabara = "https://www.climatempo.com.br/previsao-do-tempo/cidade/186/sabara-mg";
            // string urlCaete = "https://www.climatempo.com.br/previsao-do-tempo/cidade/116/caete-mg";
            // string urlRavena = "https://www.climatempo.com.br/previsao-do-tempo/cidade/5882/ravena-mg";
            // string urlTaquaracu = "https://www.climatempo.com.br/previsao-do-tempo/cidade/4068/taquaracudeminas-mg";

            var httpClient = new HttpClient();
            
            List<ObjetoURL> listaUrl = new List<ObjetoURL>
            {
                new ObjetoURL { city = "Sabara", url = "https://www.climatempo.com.br/previsao-do-tempo/cidade/186/sabara-mg" },
                new ObjetoURL { city = "Caete", url = "https://www.climatempo.com.br/previsao-do-tempo/cidade/116/caete-mg" },
                new ObjetoURL { city = "Ravena", url = "https://www.climatempo.com.br/previsao-do-tempo/cidade/5882/ravena-mg"},
                new ObjetoURL { city = "Taquaracu", url = "https://www.climatempo.com.br/previsao-do-tempo/cidade/4068/taquaracudeminas-mg"},                
            };

            foreach (var objeto in listaUrl)
            {
                Console.WriteLine($"ID: {objeto.city}, Nome: {objeto.url}");                
                var html = httpClient.GetStringAsync(objeto.url).Result;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                //Get the temperature Min
                var temperatureElement = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='min-temp-1']");
                var temperatureMin = temperatureElement.InnerText.Trim();
                Console.WriteLine("Temperatura Min:"+temperatureMin);

                //Get the temperature Max
                var temperatureElementMax = htmlDocument.DocumentNode.SelectSingleNode("//span[@id='max-temp-1']");
                var temperatureMax = temperatureElementMax.InnerText.Trim();
                Console.WriteLine("Temperatura Max:"+temperatureMax);

                //Get the temperature Rain
                var temperatureElementRain = htmlDocument.DocumentNode.SelectSingleNode("//span[@class='_margin-l-5']");
                var temperatureRain = temperatureElementRain.InnerText.Trim();
                Console.WriteLine("Chuva:"+temperatureRain.Replace(Environment.NewLine, ""));

                //Get the temperature wind
                var speedElementWind = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='mainContent']/div[5]/div[4]/div[1]/div[2]/div[2]/div[2]/div[1]/ul/li[3]/div");
                var speedwind = speedElementWind.InnerHtml.Trim();
                string pattern = @"<span class=""arrow _margin-r-10"" style=""transform: rotate\(247\.5deg\);""\></span>";
                Console.WriteLine("Velocidade Vento: " + (Regex.Replace(speedwind, pattern, "")).Replace(Environment.NewLine, ""));            

                //Get the temperature moisture
                var elementMoisture = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='mainContent']/div[5]/div[4]/div[1]/div[2]/div[2]/div[2]/div[1]/ul/li[4]/div/p/span[1]");
                var moisture = elementMoisture.InnerText.Trim();
                Console.WriteLine("Umidade:"+moisture);

                //Get the Houre Sun
                var elementSun = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='mainContent']/div[5]/div[4]/div[1]/div[2]/div[2]/div[2]/div[1]/ul/li[5]/span[2]");
                var houreSun = elementSun.InnerText.Trim();
                Console.WriteLine("Horas de SOL:"+houreSun.Replace(Environment.NewLine, ""));
            }
        }
    }

}
