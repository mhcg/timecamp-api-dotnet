// Copyright (c) 2020 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TimeCampAPI.Core.Interfaces;
using TimeCampAPI.Core.Models;
using System;
using System.Text;
using System.Linq;

namespace TimeCampAPI.Core.Services
{
    public partial class TimeCampService : ITimeCampService
    {
        private static string _timeCampToken = "";

        private HttpClient HttpClient { get; }

        private static JsonSerializerOptions JsonSerializerOptions { get; }
            = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        public TimeCampService(HttpClient httpClient)
        {
            SanityCheck(httpClient);

            HttpClient = httpClient;
        }

        /// <summary>
        /// Set the TimeCamp Token required to access the API.
        /// </summary>
        /// <exception cref="ArgumentNullException">Token cannot be empty.</exception>
        public static string TimeCampToken
        {
            private get => _timeCampToken;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException();

                _timeCampToken = value;
            }
        }

        /// <summary>
        /// Get TimeCamp Time Entires.
        /// </summary>
        /// <param name="fromDate">From Date to get Time Entries.</param>
        /// <param name="toDate">To Date to get Time Entries.</param>
        /// <param name="taskIDs">List of Task IDs to filter the Time Enteries.</param>
        /// <param name="userIDs">List of User IDs to filter the Time Enteries.</param>
        /// <returns>List of TimeEntry objects.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If From Date is before To Date.</exception>
        public async Task<IEnumerable<TimeEntry>> GetTimeEntries(
            DateTime fromDate, DateTime toDate,
            IEnumerable<string>? taskIDs = null,
            IEnumerable<string>? userIDs = null)
        {
            // Sanity check of params etc.
            SanityCheck();
            if (toDate < fromDate)
                throw new ArgumentOutOfRangeException("To Date cannot be earlier from From Date.");

            // Call entries API.
            var uri = @"https://www.timecamp.com/third_party/api" +
                @"/entries" +
                @"/format/json" +
                $"/from/{fromDate.ToString(DATE_FORMAT)}" +
                $"/to/{toDate.ToString(DATE_FORMAT)}";
            if ((taskIDs ?? new List<string>()).Any())
            {
                uri += $"/task_ids/{string.Join(',', taskIDs!)}";
            }
            if ((userIDs ?? new List<string>()).Any())
            {
                uri += $"/user_ids/{string.Join(',', userIDs!)}";
            }
            var response = await HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            // Parse response and return.
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync
                <IList<TimeEntry>>(responseStream, JsonSerializerOptions);
            return result;
        }

        #region Helper Methods

        /// <summary>
        /// Helper method to make sure it's sane to call HttpClient.
        /// </summary>
        /// <param name="httpClient">HttpClient object to check.</param>
        /// <exception cref="NotSupportedException">TimeCamp Token must be set.</exception>
        /// <exception cref="ArgumentNullException">HttpClient cannot be null.</exception>
        private static void SanityCheck(HttpClient httpClient)
        {
            if (httpClient == null)
                throw new ArgumentNullException("HttpClient cannot be null.");

            if (!httpClient.DefaultRequestHeaders.Contains("Authorization"))
                throw new NotSupportedException("TimeCamp Token is missing.");
        }

        /// <summary>
        /// Calls SanityCheck on this instance.
        /// </summary>
        private void SanityCheck() => SanityCheck(this.HttpClient);

        #endregion
    }
}
