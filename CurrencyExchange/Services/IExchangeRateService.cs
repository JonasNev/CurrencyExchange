using CurrencyExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Services
{
    public interface IExchangeRateService
    {
        ExchangeRate GetExchangeRate(CurrencyPair currency);
    }
}
