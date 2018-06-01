using System.Collections.Generic;

namespace Cource
{
    class CourcesCB
    {
        public string Date { get; set; }
        public string PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public string Timestamp { get; set; }
        public List<Currency> Valute { get; set; }

        public CourcesCB()
        {
            this.Valute = new List<Currency>();
        }
    }
}
