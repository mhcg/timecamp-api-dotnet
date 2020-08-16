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
using System.Threading.Tasks;
using TimeCampAPI.Core.Models;

namespace TimeCampAPI.Core.Interfaces
{
    public interface ITimeCampService
    {
        /// <summary>
        /// Get TimeCamp Time Entires.
        /// </summary>
        /// <param name="fromDate">From Date to get Time Entries.</param>
        /// <param name="toDate">To Date to get Time Entries.</param>
        /// <param name="taskIDs">List of Task IDs to filter the Time Enteries.</param>
        /// <param name="userIDs">List of User IDs to filter the Time Enteries.</param>
        /// <returns>List of TimeEntry objects.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If From Date is before To Date.</exception>
        Task<IEnumerable<TimeEntry>> GetTimeEntries(
            DateTime fromDate, DateTime toDate,
            IEnumerable<string>? taskIDs = null,
            IEnumerable<string>? userIDs = null);
    }
}
