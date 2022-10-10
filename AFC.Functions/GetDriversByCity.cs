using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AFC.Models;
using System.Collections.Generic;
using System.Net;
using AFC.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace AFC.Functions
{
    public class GetDriversByCity
    {
        private readonly IDriverService _driverService;

        public GetDriversByCity(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [FunctionName("GetDriversByCity")]
        [OpenApiOperation(operationId: "GetDriversByCity", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "city", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The city parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ResultMessage), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(ResultMessage), Description = "The NotFound response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ResultMessage), Description = "The ServerError response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "drivers/{city}")] HttpRequest req, ILogger log, string city)
        {
            log.LogInformation("GetDriversByCity function processed a request.");
            int totalRecords = 0;
            ResultMessage result = new ResultMessage();
            result.StatusCode = (int)HttpStatusCode.OK;
            result.Message = "Successful";
            List<Driver> drivers = new List<Driver>();
            try
            {
                drivers = _driverService.GetDriversByCity(city);
                if (drivers == null || (drivers != null && drivers.Count == 0))
                {
                    result.StatusCode = (int)HttpStatusCode.NotFound;
                    result.Message = "No available drivers in this city.";
                }
            }
            catch(Exception ex)
            {
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Message = "Error occured.";
                log.Log(LogLevel.Error, "Error occured in GetDriversByCity:" + city, ex);
            }
            result.Data = drivers;
            result.TotalRecordsCount = totalRecords;
            req.HttpContext.Response.StatusCode = result.StatusCode;
            return new JsonResult(result);
        }
    }
}
