using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace EmployeeTimeSheet.Services
{
    public class TimeSheetService
    {
        
       
        public async Task<HttpResponseMessage> GetData(string id,string status, string year, string month, string url, 
            string Accept, string APIKey)
        {
            
           
            var client = new HttpClient();
            string uri = url + id + "";
            string start = "";
            string end = "";

            if (month != null && year != null)
            {
                int requiredmMonth = (Convert.ToInt32(month));
                int requiredYear = (Convert.ToInt32(year));
                DateTime start_date = new DateTime(requiredYear, requiredmMonth, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime end_date = new DateTime(requiredYear, requiredmMonth + 1, 1, 0, 0, 0, DateTimeKind.Utc);

                end_date = end_date.Subtract(TimeSpan.FromDays(1));
                start = start_date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                end = end_date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }

            if (status == null && (month == null || year == null))
            {
                uri = uri + " &%24select=approvalStatus,endDate,startDate,timeType,userId";
            }
            if (status == null && (month != null && year != null))
            {
                uri = uri + "%20%20and%20startDate%20gt%20datetime'" + start + "'%20and%20startDate%20lt%20datetime'" + end + 
                    "'&%24select=approvalStatus,endDate,startDate,timeType,userId";
            }
            if (status != null && (month == null || year == null))
            {
                uri = uri + " and approvalStatus eq '" + status + "'&%24select=approvalStatus,endDate,startDate,timeType,userId";

            }
            if (status != null && (month != null && year != null))
            {
                uri = uri + "%20and%20approvalStatus%20eq%20'" + status + "'%20%20and%20startDate%20gt%20datetime'"
                    + start + "'%20and%20startDate%20lt%20datetime'" 
                    + end + "'&%24select=approvalStatus,endDate,startDate,timeType,userId";


            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", Accept);
            client.DefaultRequestHeaders.Add("APIKey", APIKey);

            var response = await client.GetAsync(uri);

            return response;


        }
    }
}
