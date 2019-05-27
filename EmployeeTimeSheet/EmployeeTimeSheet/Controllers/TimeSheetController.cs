using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using EmployeeTimeSheet.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace EmployeeTimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly ILogger<TimeSheetController> _logger;
        private readonly IMapper _mapper;

        public TimeSheetController(ILogger<TimeSheetController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        // GET: api/TimeSheet

    [HttpGet]
        public async  Task<IActionResult> GetAsync(string id,string status,string year,string month)
        {
            var client = new HttpClient();
            string Uri;
 
            if (id ==null )
            {
                _logger.LogError("should insert id" );
                
                return BadRequest(new { message = "should insert id"  });
            }
           

            if ((Convert.ToInt32(id))<=0)
            {
                _logger.LogDebug("should insert correct id ");
                _logger.LogError("should insert correct id ");
                return BadRequest(new { message = "should insert correct id " });
            }
            if (month != null && year != null)
            {
                int required_month = (Convert.ToInt32(month));
                int required_year = (Convert.ToInt32(year));
                DateTime start_date = new DateTime(required_year, required_month, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime end_date = new DateTime(required_year, required_month + 1, 1, 0, 0, 0, DateTimeKind.Utc);

                end_date = end_date.Subtract(TimeSpan.FromDays(1));
                var start = start_date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                var end = end_date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                Uri = "https://sandbox.api.sap.com/successfactors/odata/v2/EmployeeTime?%24top=20&%24filter=userId%20eq%20" + id + "%20and%20approvalStatus%20eq%20'" + status + "'%20%20and%20startDate%20gt%20datetime'" + start + "'%20and%20startDate%20lt%20datetime'" + end + "'&%24select=approvalStatus,endDate,startDate,timeType,userId";


            }
            else
            {
                Uri = "https://sandbox.api.sap.com/successfactors/odata/v2/EmployeeTime?filter=userId eq  "+ id +" and approvalStatus eq '" + status+"'&%24select=approvalStatus,endDate,startDate,timeType,userId" ;

            }


            

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("APIKey", "uHHKozCr0U0gnAuADDzdGaoZ3zqDGBKM");

            var response = await client.GetAsync(Uri);
            if(response.IsSuccessStatusCode)
            {
              
                var result = await response.Content.ReadAsStringAsync();
                var Content = JsonConvert.DeserializeObject<RootObject>(result);
                var data = new List<RData>();
               


                
                data = Content.d.results
                       .Select(x => new RData
                       {
                           approvalStatus = x.approvalStatus,
                           endDate = x.endDate,
                           timeType = x.timeType,
                           startDate = x.startDate,
                       })
                        .ToList();

                if (data == null)
                {
                    _logger.LogInformation("no data with this information ");
                    return Ok(new { message = "no data with this information ",result_data= data });
                }


                return Ok(data);

            }

            else
            {
                _logger.LogDebug("error in connection ");
             
                return BadRequest(new { message = "error in connection" });
            }
          
              
        }







    }
}
