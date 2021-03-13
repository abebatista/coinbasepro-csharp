using CoinbasePro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinbasePro.Network.Authentication;
using Newtonsoft.Json;

namespace ClientApp.Controllers
{
    public class JsonCandle
    {
        public DateTime Time { get; set; }

        public decimal Low { get; set; }

        public decimal High { get; set; }

        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            var _Authenticator = new Authenticator("4fe22c33f8b65c2546bbace195b4b44d", "pkMkgG6kj79LRoV/Yix2sDqK0ZjjHtMvoAItpdOAdx1/+MzdYZ0U6TAKotEq6qoRZF/VigMex3eo0eWe1c5KEw==", "*Ab3l4rd0*");

            CoinbaseProClient clientSandbox = new CoinbaseProClient(_Authenticator, true);
            
            var market_1min = await clientSandbox.ProductsService.GetHistoricRatesAsync(
                CoinbasePro.Shared.Types.ProductType.BtcUsd, new DateTime(2020, 1, 1)
                , new DateTime(2021,3, 11)
                , CoinbasePro.Services.Products.Types.CandleGranularity.Minutes5);

            market_1min = market_1min.Reverse().ToList();

            List<JsonCandle> _list = new List<JsonCandle>();
            foreach(var market_1min_item in market_1min)
            {
                _list.Add(
                    new JsonCandle
                    {
                        Close = (market_1min_item.Close.HasValue ? market_1min_item.Close.Value:0),
                        High = (market_1min_item.High.HasValue ? market_1min_item.High.Value : 0),
                        Low = (market_1min_item.Low.HasValue ? market_1min_item.Low.Value : 0),
                        Open = (market_1min_item.Open.HasValue ? market_1min_item.Open.Value : 0),
                        Time = market_1min_item.Time,
                        Volume = market_1min_item.Volume
                    }
                );
            }


            string market_1min_str = JsonConvert.SerializeObject(_list);

            string path = @"C:\Users\billy\source\repos\Stock.Indicators\EmaDemaTema\data\historical\BTC-USD-5min.json";

            System.IO.File.WriteAllText(path, market_1min_str);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
