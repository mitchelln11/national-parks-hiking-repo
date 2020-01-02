using NationalParksHiking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace NationalParksHiking.HelperClass
{
    public class parkHelper
    {
        //// ------------------ Get Lat Long -----------------------------
        //public async Task GetLatLong(Park park, Datum parkInfo)
        //{
        //    //string comboLatLong = parkInfo.data[0].latLong; // Grabs entire lat long string.
        //    var comboLatLong = parkInfo.latLong;
        //    var latLongArray = comboLatLong.Split().ToArray(); // Splits based on comma, set to an array
        //    string isolatedLatitude = latLongArray[0].TrimEnd(','); // New lat variable for the 0 index, trim trailing comma
        //    string isolatedLongtitude = latLongArray[1].TrimEnd(','); // New lng variable for the 1 index trim trailing comma
        //    string latitude = isolatedLatitude.Substring(4, isolatedLatitude.Length - 4); // Remove beginning lat: text
        //    string longitude = isolatedLongtitude.Substring(5, isolatedLongtitude.Length - 5); // Remove beginning lng: text
        //    park.ParkLat = latitude;
        //    park.ParkLng = longitude;
        //    await Park.SaveChangesAsync();
        //}

        //// ------------------ Get Park Description --------------------
        //public async Task GetParkDescription(Park park, Datum parkInfo)
        //{
        //    string parkDescription = parkInfo.description;
        //    park.ParkDescription = parkDescription;
        //    await db.SaveChangesAsync();
        //}

        //// ------------------ Get Park Name -----------------------------
        //public async Task GetFullParkName(Park park, Datum parkInfo)
        //{
        //    string fullParkName = parkInfo.fullName;
        //    park.ParkName = fullParkName;
        //    await db.SaveChangesAsync();
        //}

        //// ------------------ Get State ----------------------------------
        //public async Task GetParkState(Park park, Datum parkInfo)
        //{
        //    string ParkState = parkInfo.states;
        //    park.ParkState = ParkState;
        //    await db.SaveChangesAsync(); // Issues with saving the database???????
        //}


        //async Task RunJsonClient(int? id, Park park, ApiKeys apiKeys)
        //{
        //    string parkKey = apiKeys.NpsKey;
        //    int apiLimit = 150;
        //    string url = $"https://developer.nps.gov/api/v1/parks?q=National%20Park&limit={apiLimit}&api_key={parkKey}";
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync(url);
        //    string jsonresult = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        NpsJsonInfo npsJsonInfo = JsonConvert.DeserializeObject<NpsJsonInfo>(jsonresult); // Run through entire JSON file
        //        Datum parkInfo = npsJsonInfo.data.Where(p => p.parkCode == park.ParkCode).Single(); // Look into individual park when passing id in URL
        //                                                                                            // From here on out, refer to parkInfo instead of npsJsonInfo
        //        await GetParkDescription(park, parkInfo);
        //        await GetFullParkName(park, parkInfo);
        //        await GetParkState(park, parkInfo);
        //        await GetLatLong(park, parkInfo);
        //        await db.SaveChangesAsync();
        //    }
        //}
    }
}