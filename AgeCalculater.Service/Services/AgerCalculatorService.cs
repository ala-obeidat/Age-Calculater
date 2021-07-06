using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Grpc.Core;

using Microsoft.Extensions.Logging;

namespace AgeCalculater.Service
{
    public class AgerCalculatorService : Ager.AgerBase
    {
        private readonly ILogger<AgerCalculatorService> _logger;
        public AgerCalculatorService(ILogger<AgerCalculatorService> logger)
        {
            _logger = logger;
        }
        public override Task<AgeResponse> Calculate(AgeRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Calculating age ...");
            var birthDate = new DateTime(request.BithDate, DateTimeKind.Utc);
            var ageString = CalculateYourAge(birthDate);
            _logger.LogInformation("Send response");
            return Task.FromResult(new AgeResponse()
            {
                FullAge = ageString
            });
        }

        /// <summary>  
        /// For calculating age  
        /// </summary>  
        /// <param name="Dob">Enter Date of Birth to Calculate the age</param>  
        /// <returns> years, months,days, hours...</returns>  
        static string CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;

            return String.Format("Age: {0} Years {1} Months {2} Days",
            Years, Months, Days);
        }

    }
}
