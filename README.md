# APIAssignment
The api could be run by either using "-dotnet run" or by running the solution since swagger is included in it.
This api includes both sources bitstamp and bitfinex APIs so the user can view the latest prices of both bitcoins. 
The user also can add new sources other than the existing ones, where he will specify the name of the source,
its API url, and the targeted value that he wanted to view from the json response.

The api requests included are:
1. GET /api/bitcoin/GetSourcesDetails : to get all sources with their name, api url and targeted viewed value (last_price)
2. GET /api/bitcoin/GetSourcesNames  : to get only the names of the resources
3. GET /api/bitcoin/{sourceName}  : to get the latest price of the specified source, and store it in the in-memory database
4. GET /api/bitcoin/GetBitcoinPriceHistory  : to get all the stored in-memory sources and prices
5. POST /api/bitcoin/AddNewSource  : to add new source including its name, targeted value and the api url
   this could be tested by adding for example this bitcoin api:
     {
       "name": "coinpaprika",
       "targetedValue": "price_usd",
       "apiURL": "https://api.coinpaprika.com/v1/ticker/btc-bitcoin?quotes=USD"
     }
6. POST /api/bitcoin/UpdateSource   : to update the details of an existing source
7. POST /api/bitcoin/DeleteSource/{source}   : to delete an existing source by providing its name
