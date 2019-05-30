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

using EmployeeTimeSheet.Services;
using Microsoft.Extensions.Configuration;

namespace EmployeeTimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly ILogger<TimeSheetController> _logger;
        IConfiguration _iconfiguration;
      

        public TimeSheetController(ILogger<TimeSheetController> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;
        }
        // GET: api/TimeSheet

    [HttpGet]
        public async  Task<IActionResult> GetAsync(string id,string status,string year,string month)
        {

            string url = _iconfiguration.GetSection("Request").GetSection("Uri").Value;
            string Accept = _iconfiguration.GetSection("Request").GetSection("Accept").Value;
            string APIKey = _iconfiguration.GetSection("Request").GetSection("APIKey").Value;


            if (id ==null )
            {
                _logger.LogError("should insert id" );
                
                return BadRequest(new { message = "should insert id"  });
            }
            if ((Convert.ToInt32(id)) <= 0)
            {
                _logger.LogDebug("should insert correct id ");
                _logger.LogError("should insert correct id ");
                return BadRequest(new { message = "should insert correct id " });
            }
           TimeSheetService timeService = new TimeSheetService();
           var response =await timeService.GetData( id,  status,  year,  month,  url,  Accept,  APIKey);
            if (response.IsSuccessStatusCode)
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
