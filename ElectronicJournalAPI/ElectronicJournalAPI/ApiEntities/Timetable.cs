using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.ApiEntities
{
    public class Timetable
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Timetable(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public async Task<IEnumerable<StudyDay>> GetTimetable(CancellationToken cancellationToken = default)
        {
            return await ApiClient.GetAsync<IEnumerable<StudyDay>>(
                apiMethod: "Timetable/GetTimetable",
                argQuery: new Dictionary<string, string>
                {
                    ["StartDate"] = StartDate.ToString("MM.dd.yyyy"),
                    ["EndDate"] = EndDate.ToString("MM.dd.yyyy")
                },
                cancellationToken: cancellationToken
            );
        }
    }
}
