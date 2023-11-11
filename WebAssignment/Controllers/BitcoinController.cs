using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;
using WebAssignment.Models;
using WebAssignment.Response;

namespace WebAssignment.Controllers
{
    [ApiController]
    [Route("api/bitcoin")]
    public class BitcoinController : ControllerBase
    {
        private readonly ApiContext _apiContext;
        public BitcoinController(ApiContext apiContext)
        {
            _apiContext = apiContext;
            if (_apiContext.SourceList.Count() == 0)
            {
                _apiContext.SourceList.Add(new Source { Name = "bitstamp", targetedValue = "last", apiURL = "https://www.bitstamp.net/api/v2/ticker/btcusd/" });
                _apiContext.SourceList.Add(new Source { Name = "bitfinex", targetedValue = "last_price", apiURL = "https://api.bitfinex.com/v1/pubticker/BTCUSD" });

                _apiContext.SaveChanges();
            }
        }

        [HttpGet("GetSourcesDetails")]
        public IActionResult GetSourcesDetails()
        {
            return Ok(_apiContext.SourceList);
        }
        [HttpGet("GetSourcesNames")]
        public IActionResult GetSourcesNames()
        {
            return Ok(_apiContext.SourceList.Select(x=>x.Name));
        }

        [HttpGet("/{sourceName}")]
        public async Task<IActionResult> GetBitcoin(string sourceName)
        {
            decimal price = 0;
            var source = _apiContext.SourceList.FirstOrDefault(x=> x.Name == sourceName.ToLower());

            if(source != null && !string.IsNullOrEmpty(source.apiURL)) { 
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(source.apiURL);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(content);
                        if (data.TryGetValue(source.targetedValue, out var targetValue))
                        {
                            price = Convert.ToDecimal(targetValue);

                            var bitcoinPrice = new Bitcoin
                            {
                                Source = sourceName,
                                Price = Convert.ToDecimal(price.ToString("0.00")),
                                Timestamp = DateTime.Now,
                            };
                            _apiContext.BitcoinList.Add(bitcoinPrice);
                            await _apiContext.SaveChangesAsync();
                            return Ok(new { Source = sourceName, Price = Convert.ToDecimal(price.ToString("0.00")) });
                        }
                        else
                            return BadRequest("no price found");
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet("GetBitcoinPriceHistory")]
        public IActionResult GetBitcoinHistory()
        {
            return Ok(_apiContext.BitcoinList.ToList());
        }

        [HttpPost("AddNewSource")]
        public IActionResult AddNewSource(Source source)
        {
            
              // {"name": "coinpaprika",
              //"targetedValue": "price_usd",
              //"apiURL": "https://api.coinpaprika.com/v1/ticker/btc-bitcoin?quotes=USD"}

            if (_apiContext.SourceList.Any(x=> x.Name == source.Name))
                return BadRequest("Source already exists");

            _apiContext.SourceList.Add(new Source
            {
                Name = source.Name,
                targetedValue = source.targetedValue,
                apiURL = source.apiURL,
            });
            _apiContext.SaveChanges();
            return Ok(source);
        }

        [HttpPost("UpdateSource")]
        public IActionResult UpdateSource(Source source)
        {
            var oldSource = _apiContext.SourceList.FirstOrDefault(x => x.Name == source.Name);
            if (oldSource != null)
            {
                oldSource.targetedValue = source.targetedValue;
                oldSource.apiURL = source.apiURL;
                _apiContext.SaveChanges();
                return Ok(source);
            }
            return BadRequest("Source doesn't exist");
        }

        [HttpPost("DeleteSource/{source}")]
        public IActionResult DeleteSource(string source)
        {
            var oldSource = _apiContext.SourceList.FirstOrDefault(x => x.Name == source);
            if (oldSource != null)
            {
                _apiContext.SourceList.Remove(oldSource);
                _apiContext.SaveChanges();
                return Ok(source);
            }
            return BadRequest("Source doesn't exist");
        }
    }
}
