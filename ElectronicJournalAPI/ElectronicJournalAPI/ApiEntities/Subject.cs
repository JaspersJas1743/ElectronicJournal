using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.ApiEntities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public double Average { get; set; }

        public IEnumerable<Mark> Marks { get; set; }

        public class MarksResponse
        {
            public double Average { get; set; }
            public IEnumerable<Mark> Marks { get; set; }
        }

        public async Task GetMarks(CancellationToken cancellationToken = default)
        {
            MarksResponse response = await ApiClient.GetAsync<MarksResponse>(
                apiMethod: "Marks/GetMarks",
                argQuery: new Dictionary<string, string> { { "LessonId", Id.ToString() } },
                cancellationToken: cancellationToken
            );
            Debug.WriteLine(String.Join(", ", response?.Marks));
            Average = response.Average;
            Marks = response.Marks;
        }
    }
}
