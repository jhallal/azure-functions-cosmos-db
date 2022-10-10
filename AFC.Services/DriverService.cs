using AFC.Data;
using AFC.Models;

namespace AFC.Services
{
    public class DriverService : IDriverService
    {
        public List<Driver> GetDriversByCity(string city)
        {
            //This function should query the Cosmos DB to get the drivers
            List<Driver> drivers = new List<Driver>();
            if (city == "Berlin") //Only return data if the city is Berlin
            {
                //for (int i = 1; i <= 100; i++)
                //{
                //    Driver driver = new Driver
                //    {
                //        Id = i,
                //        FirstName = "First " + i,
                //        LastName = "Last " + i,
                //        Location = "Berlin"
                //    };
                //    drivers.Add(driver);
                //}
                DriverDAL driveDAL = new DriverDAL();
                drivers = driveDAL.QueryItemsAsync(city).Result;
            }
            else if (city != "Hamburg") // TODO:This is just to test the 500 error
                throw new Exception();
            return drivers.ToList();
        }
    }
}