using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AFC.Models
{
    public class ResultMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }

        [JsonProperty("totalCount")]
        public int TotalRecordsCount { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }
}
