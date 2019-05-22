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

            if (id ==null )
            {
                _logger.LogError("should insert id" );
                
                return BadRequest(new { message = "should insert id"  });
            }
            if (status == null)
            {
                _logger.LogError("should insert status");
                return BadRequest(new { message = "should insert  approvalStatus" });
            }

            if ((Convert.ToInt32(id))<=0)
            {
                _logger.LogDebug("should insert correct id ");
                _logger.LogError("should insert correct id ");
                return BadRequest(new { message = "should insert correct id " });
            }

            if (month == null )
            {
                _logger.LogError("should insert month");

                return BadRequest(new { message = "should insert month" });
            }
            if (year == null)
            {
                _logger.LogError("should insert year");

                return BadRequest(new { message = "should insert year" });
            }


            var client = new HttpClient();
           
          var Uri = "https://sandbox.api.sap.com/successfactors/odata/v2/EmployeeTime?filter=userId eq  "+ id +" and approvalStatus eq '" + status+"'&%24select=approvalStatus,endDate,startDate,timeType,userId" ;
            //var Uri = "https://sandbox.api.sap.com/successfactors/odata/v2/EmployeeTime?filter=userId eq  " + id + " and approvalStatus eq '" + status + "'startDate gt "+ dtDateTimeS + "&%24select=approvalStatus,endDate,startDate,timeType,userId";

            //var Uri = "https://sandbox.api.sap.com/successfactors/odata/v2/EmployeeTime";

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("APIKey", "uHHKozCr0U0gnAuADDzdGaoZ3zqDGBKM");

            var response = await client.GetAsync(Uri);
            if(response.IsSuccessStatusCode)
            {
              
                var result = await response.Content.ReadAsStringAsync();
                var Content = JsonConvert.DeserializeObject<RootObject>(result);
               
                List<RData> ReturnedData = new List<RData>();
                RData ReturneObj = new RData();
                foreach (var item in Content.d.results)
                {
                    if(item.startDate.Month== (Convert.ToInt32(month)) && item.startDate.Year== Convert.ToInt32(year))
                    {
                        ReturneObj.approvalStatus = item.approvalStatus;
                        ReturneObj.endDate = item.endDate;
                        ReturneObj.timeType = item.timeType;
                        ReturneObj.startDate = item.startDate;
                        ReturnedData.Add(ReturneObj);

                    }


                    var x=item.startDate.Month;
                    var y = item.startDate.Year;
                }

                //var model = _mapper.Map<FinalData>(Content);

                if (ReturnedData == null)
                {
                    _logger.LogInformation("no data with this information ");
                    return Ok(new { message = "no data with this information ",data= ReturnedData });
                }


                return Ok(ReturnedData);

            }

            else
            {
                _logger.LogDebug("error in connection ");
             
                return BadRequest(new { message = "error in connection" });
            }
          
              
        }







    }
}
