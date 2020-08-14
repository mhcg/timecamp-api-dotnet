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

using System.Text.Json.Serialization;
using System;
using System.Diagnostics.CodeAnalysis;

namespace TimeCampAPI.Core.Models
{
    public class TimeEntry
    {
        /// <summary>
        /// ID from TimeCamp.
        /// </summary>
        [JsonPropertyName("id")]
        public string TimeEntryID { get; set; } = string.Empty;

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        [JsonConverter(typeof(JsonConverters.DurationInSecondsToTimeSpanConverter))]
        public TimeSpan Duration { get; set; } = default;

        /// <summary>
        /// TimeCamp User ID.
        /// </summary>
        [JsonPropertyName("user_id")]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// TimeCamp Username.
        /// </summary>
        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// TimeCamp Task ID.
        /// </summary>
        [JsonPropertyName("task_id")]
        public string TaskID { get; set; } = string.Empty;

        /// <summary>
        /// Last modified date/time.
        /// </summary>
        [JsonPropertyName("last_modify")]
        [JsonConverter(typeof(JsonConverters.TimeCampDateTimeConverter))]
        public DateTime LastModified { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The Time Entry start date.
        /// </summary>
        [JsonPropertyName("date")]
        [JsonConverter(typeof(JsonConverters.TimeCampDateTimeConverter))]
        public DateTime TimeEntryDate { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// The Time Entry start time.
        /// </summary>
        [JsonPropertyName("start_time")]
        [JsonConverter(typeof(JsonConverters.TimeCampTimeToTimeSpanConverter))]
        public TimeSpan StartTime { get; set; } = TimeSpan.MaxValue;

        /// <summary>
        /// The Time Entry end time.
        /// </summary>
        [JsonPropertyName("end_time")]
        [JsonConverter(typeof(JsonConverters.TimeCampTimeToTimeSpanConverter))]
        public TimeSpan EndTime { get; set; } = TimeSpan.MaxValue;

        /// <summary>
        /// TimeCamp Task Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string TaskName { get; set; } = string.Empty;

        /// <summary>
        /// TimeCamp Time Entry Notes field.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Is the Time Entry billable?
        /// </summary>
        [JsonConverter(typeof(JsonConverters.ZeroOrOtherToFalseOrTrueConverter))]
        public bool Billable { get; set; } = false;
        public string BillableString { get => Billable ? "Billable" : "Non-Billable"; }

        /// <summary>
        /// For integrations with Trello, Pivotal Tracker, etc., "0" if no integration.
        /// </summary>
        [JsonPropertyName("addons_external_id")]
        public string AddonsExternalID { get; set; } = string.Empty;

        /// <summary>
        /// TimeCamp Invoice ID.
        /// </summary>
        [JsonPropertyName("invoiceId")]
        public string InvoiceID { get; set; } = string.Empty;

        /// <summary>
        /// TimeCamp Time Entry Task color.
        /// </summary>
        [JsonPropertyName("color")]
        [JsonConverter(typeof(JsonConverters.HTMLColorCodeConverter))]
        public System.Drawing.Color Color { get; set; }

        public override string? ToString() =>
            $"{TimeEntryID} : " +
            $"{TaskName} ({TaskID}) : " +
            $"{TimeEntryDate.ToLongDateString()} : " +
            $"{StartTime} - {EndTime} : " +
            $"{Duration} : " +
            $"{BillableString} : " +
            $"{UserName} ({UserID}).";

        #region IEquatable.

        public bool Equals([AllowNull] TimeEntry other)
            => (other != null && this.TimeEntryID == other.TimeEntryID);

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            TimeEntry? timeentryObj = obj as TimeEntry;
            if (timeentryObj == null)
                return false;
            else
                return Equals(timeentryObj);
        }

        public override int GetHashCode()
            => this.TimeEntryID.GetHashCode();

        public static bool operator ==(TimeEntry? timeentry1, TimeEntry? timeentry2)
            => (((object?)timeentry1) == null || ((object?)timeentry2) == null)
                ? Object.Equals(timeentry1, timeentry2)
                : timeentry1.Equals(timeentry2);

        public static bool operator !=(TimeEntry? timeentry1, TimeEntry? timeentry2)
            => (((object?)timeentry1) == null || ((object?)timeentry2) == null)
                ? !Object.Equals(timeentry1, timeentry2)
                : !timeentry1.Equals(timeentry2);

        #endregion
    }
}
