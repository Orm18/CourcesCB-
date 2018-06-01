using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cource
{
    class Program
    {
        private static CourcesCB cources;

        static void Main(string[] args)
        {
            var userCommand = string.Empty;

            do
            {
                var code = string.Empty;
                if (string.IsNullOrWhiteSpace(userCommand))
                {
                    Console.WriteLine("Options: currency char code or num code, list.");
                    code = Console.ReadLine(); //что то читаем
                    // Другой комментарий
                }
                else
                {
                    code = userCommand;
                }

                if (string.IsNullOrWhiteSpace(code))
                    continue;

                LoadCources();

                if (string.Equals(code, "list", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine(string.Join(", ", cources.Valute.Select(x => x.CharCode)));
                    userCommand = Console.ReadLine();
                    continue;
                }

                var currency = cources.Valute.FirstOrDefault(x => string.Equals(x.CharCode, code, StringComparison.InvariantCultureIgnoreCase) ||
                                                                  string.Equals(x.NumCode, code, StringComparison.InvariantCultureIgnoreCase));

                if (currency == null)
                {
                    Console.WriteLine("Have not that currency.");
                    userCommand = Console.ReadLine();
                    continue;
                }

                Console.WriteLine($"Cource value = {currency.Value}. Cource date: {cources.Date}");
                userCommand = Console.ReadLine();
            } while (!string.IsNullOrWhiteSpace(userCommand));
            
            
        }

        private static void LoadCources()
        {
            var json = string.Empty;
            using (var webClient = new System.Net.WebClient())
                json = webClient.DownloadString("https://www.cbr-xml-daily.ru/daily_json.js");

            var data = (JObject)JsonConvert.DeserializeObject(json);

            cources = new CourcesCB();
            cources.Date = data["Date"].Value<string>();
            cources.PreviousDate = data["PreviousDate"].Value<string>();
            cources.PreviousURL = data["PreviousURL"].Value<string>();
            cources.Timestamp = data["Timestamp"].Value<string>();

            var valutes = data["Valute"].Children();
            foreach (var valute in valutes)
            {
                var props = new Dictionary<string, JProperty>();
                foreach (var child in ((JProperty)valute).Children().Children())
                    props.Add(((JProperty)child).Name, (JProperty)child);
                var currency = new Currency();
                currency.ID = props["ID"].Value.Value<string>();
                currency.NumCode = props["NumCode"].Value.Value<string>();
                currency.CharCode = props["CharCode"].Value.Value<string>();
                currency.Nominal = props["Nominal"].Value.Value<int>();
                currency.Name = props["Name"].Value.Value<string>();
                currency.Value = props["Value"].Value.Value<double>();
                currency.Previous = props["Previous"].Value.Value<double>();
                cources.Valute.Add(currency);
            }
        }
    }
}
