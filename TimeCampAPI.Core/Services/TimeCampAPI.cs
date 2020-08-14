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

using System;
using System.Collections.Generic;
using System.Text;

namespace TimeCampAPI.Core.Services
{
    public partial class TimeCampService
    {
        /// <summary>
        /// Base URL for all TimeCamp API calls.
        /// </summary>
        private const string BASE_PATH
            = @"https://www.timecamp.com/third_party/api";

        /// <summary>
        /// TimeCamp API date format for API calls
        /// </summary>
        private const string DATE_FORMAT = @"yyyy-MM-dd";

        /// <summary>
        /// Contains all the TimeCamp API URLs.
        /// </summary>
        internal static class ApiMethodUrls
        {
            /// <summary>
            /// TimeCamp API Token.
            /// </summary>
            internal static string TimeCampToken { get; set; } = "";

            /// <summary>
            /// TimeCamp Time Entries.
            /// </summary>
            internal static class TimeEntries
            {
                internal static string GetTimeEntries(
                    DateTime fromDate, DateTime toDate,
                    IEnumerable<string>? taskIDs = null,
                    IEnumerable<string>? userIDs = null)
                {
                    SanityCheck();

                    // TODO: Implement TaskIDs and UserIDs.
                    if (taskIDs != null || userIDs != null)
                        throw new NotImplementedException();

                    var uriBuilder = new StringBuilder($"{BASE_PATH}/entries/format/json");
                    uriBuilder.Append($"/api_token/${TimeCampToken}");
                    uriBuilder.Append($"/from/${fromDate.ToString(DATE_FORMAT)}");
                    uriBuilder.Append($"/to/${toDate.ToString(DATE_FORMAT)}");
                    return uriBuilder.ToString();
                }
            }

            /// <summary>
            /// Helper method to make sure using this is sane right now.
            /// </summary>
            /// <exception cref="NotSupportedException">If TimeCampToken is empty.</exception>
            private static void SanityCheck()
            {
                if (string.IsNullOrWhiteSpace(TimeCampToken))
                    throw new NotSupportedException("TimeCamp Token cannot be empty.");
            }
        }
    }
}
