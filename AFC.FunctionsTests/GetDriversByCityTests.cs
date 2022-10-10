using Microsoft.VisualStudio.TestTools.UnitTesting;
using AFC.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using AFC.Models;
using AFC.Services;
using Newtonsoft.Json;

namespace AFC.Functions.Tests
{
    [TestClass()]
    public class GetDriversByCityTests : FunctionTest
    {
        [TestMethod()]
        public async Task Get_Drivers_Should_Return_404()
        {
            HttpClient client = new HttpClient();
            var responseMessage = await client.GetAsync("http://localhost:7014/api/drivers/Hamburg");
            ResultMessage? resultObject = JsonConvert.DeserializeObject<ResultMessage>(responseMessage.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(404, resultObject != null ? resultObject.StatusCode : -1);
        }

        [TestMethod()]
        public async Task Get_Drivers_Should_Return_200()
        {
            HttpClient client = new HttpClient();
            var responseMessage = await client.GetAsync("http://localhost:7014/api/drivers/Berlin");
            ResultMessage? resultObject = JsonConvert.DeserializeObject<ResultMessage>(responseMessage.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(200, resultObject != null ? resultObject.StatusCode : -1);
        }

        [TestMethod()]
        public async Task Get_Drivers_Should_Return_500()
        {
            HttpClient client = new HttpClient();
            var responseMessage = await client.GetAsync("http://localhost:7014/api/drivers/Frankfurt");
            ResultMessage? resultObject = JsonConvert.DeserializeObject<ResultMessage>(responseMessage.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(500, resultObject != null ? resultObject.StatusCode : -1);
        }
    }
}