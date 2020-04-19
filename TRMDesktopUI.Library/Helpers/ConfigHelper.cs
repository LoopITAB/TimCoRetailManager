
using System;
using System.Configuration;

namespace TRMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            //double output = 0;

            string taxRateText = ConfigurationManager.AppSettings["taxRate"];

            //output = Double.Parse(taxRateText);
            bool IsValidTaxRate = Decimal.TryParse(taxRateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new InvalidCastException("The tax rate is not set up properly");
            }

            return output;
        }



    }
}