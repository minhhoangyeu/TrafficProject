using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Traffic.Utilities.Constants;
using Traffic.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Traffic.Utilities.Enums;
using Traffic.Application.Models.SendEmail;

namespace Traffic.Application.Interfaces
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailService(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<SendMailModel> SendMail()
        {
            throw new NotImplementedException();
        }

        //public async Task<SendEmailModel> SendMail()
        //{
        //    var result = new SendMailResponseModel
        //    {
        //        Status = Status.SUCCESS.ToString()
        //    };

        //    try
        //    {
        //        var endpoint = $"{_configuration["SMT:BaseUrl"]}{_configuration["SMT:Endpoint:Email"]}";
        //        var httpClient = HttpClientExtensions.SMTClient(_configuration);

        //        // get total PCIDs with non-PostCode
        //        var cards = await _cardManagementService.GetPCIDsWithNonPostCode();
        //        result.TotalCard = cards.Count;

        //        var data = new SendMailDto
        //        {
        //            Recipient = _configuration["SMT:Recipients"].Split(';'),
        //            MessageCode = _configuration["SMT:SendEmailMessageCode"],
        //            Subject = _configuration["SMT:Subject"],
        //            Content = EmailConstants.VNPOST.Content.Replace("{total_count}", cards.Count.ToString()).Replace("{table_data}", BuidTemplate(cards))
        //        };

        //        await HttpClientExtensions.PostAsync<HttpResponseMessage>(httpClient, endpoint, data);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Status = Status.ERROR.ToString();
        //        result.Description = ex.Message;
        //        _logger.LogError(CommonConstants.OccuredErrorJob, nameof(SendMail), ex.Message);
        //    }
        //    finally
        //    {
        //        // write log
        //        ActivityInfo activityInfo = new ActivityInfo
        //        {
        //            Severity = Severity.LOW.ToString(),
        //            Action = "Job send email cards have no Postcode",
        //            Data = {
        //                Request = null,
        //                Response = result
        //            },
        //            Result = result.Status
        //        };
        //        activityInfo.LogActivity(_httpContextAccessor, _logger);
        //    }

        //    return result;
        //}

        //private string BuidTemplate(List<NonPostCodeDto> items)
        //{
        //    var html = new StringBuilder();
        //    html.Append("<table style='border-collapse: collapse;border: 1px solid black;'>" +
        //                    "<tr>" +
        //                            "<th style='text-align: center; border: 1px solid black;'>" +
        //                                "PCID" +
        //                            "</th>" +
        //                            "<th style='text-align: center; border: 1px solid black;'>" +
        //                                "Manufacture Date" +
        //                            "</th>" +
        //                    "</tr>");

        //    foreach(var item in items)
        //    {
        //        html.Append("<tr><td style='text-align: center; border: 1px solid black;'>" + item.PCID + "</td><td style='text-align: center; border: 1px solid black;'>" + item.ManufactureDate + "</td></tr>");
        //    }

        //    html.Append("</table>");

        //    return html.ToString();
        //}
    }
}
