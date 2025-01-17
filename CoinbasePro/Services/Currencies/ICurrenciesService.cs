﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinbasePro.Services.Currencies
{
    public interface ICurrenciesService
    {
        Task<IEnumerable<Models.Currency>> GetAllCurrenciesAsync();

        Task<Models.Currency> GetCurrencyByIdAsync(Shared.Types.Currency currency);
    }
}
