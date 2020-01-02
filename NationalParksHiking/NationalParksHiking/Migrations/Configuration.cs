namespace NationalParksHiking.Migrations
{
    using Newtonsoft.Json;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NationalParksHiking.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NationalParksHiking.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
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
            //context.Parks.AddOrUpdate(
            // new Models.Park { Name = Models.Park., StreetAddress = "S Bell Rd", City = "Homer Glen", State = "IL", ZipCode = "60491", AverageRating = null }
            // )
        }
    }
}
