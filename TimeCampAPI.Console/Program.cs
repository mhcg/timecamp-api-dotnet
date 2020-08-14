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
using static System.Console;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeCampAPI.Core.Interfaces;
using TimeCampAPI.Core.Services;
using System.Linq;
using CommandLine;

namespace TimeCampAPI.Console
{
    class Program
    {
        #region Command Line

        private static System.IServiceProvider Services { get; set; } = null!;
        private static ILogger<Program> Logger { get; set; } = null!;

        public class GlobalOptions
        {
            [Option('t', "token", Required = true, HelpText = "TimeCamp API Token.")]
            public string TimeCampToken { get; set; } = string.Empty;
        }

        [Verb("GetTimeEntries", HelpText = "Get Time Entries.")]
        public class GetTimeEntriesOptions : GlobalOptions
        {
            [Option('f', "from", Required = true, HelpText = "Date to get Time Entries From.")]
            public string FromDate { get; set; } = string.Empty;

            [Option('t', "to", Required = true, HelpText = "Date to get Time Entries To.")]
            public string ToDate { get; set; } = string.Empty;
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<GlobalOptions, GetTimeEntriesOptions>(args)
                .WithParsed<GlobalOptions>(options =>
                {
                    // Init the services stuff.
                    Services = CreateServiceProvider(options.TimeCampToken);
                    Logger = Services.GetRequiredService<ILogger<Program>>();
                    Logger.LogInformation("Services ready.");
                })
                .WithParsed<GetTimeEntriesOptions>(options =>
                {
                    var fromDate = DateTime.Parse(options.FromDate);
                    var toDate = DateTime.Parse(options.ToDate);
                    Task.Run(() => GetTimeEntriesAsync(fromDate, toDate)).Wait();
                });
        }

        #endregion

        /// <summary>
        /// Get Time Entries for the selected criteria.
        /// </summary>
        /// <param name="fromDate">Date to get Time Entries from.</param>
        /// <param name="toDate">Date to get Time Entries to.</param>
        /// <returns></returns>
        private static async Task GetTimeEntriesAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var timeCampService = Services.GetService<ITimeCampService>();

                var getTimeEntries = await timeCampService.GetTimeEntries(
                    fromDate: fromDate,
                    toDate: toDate,
                    taskIDs: null,
                    userIDs: null);

                if (0 == getTimeEntries.Count())
                {
                    WriteLine("Nothing found!");
                }
                else
                {
                    foreach (var timeEntry in getTimeEntries)
                    {
                        WriteLine(timeEntry.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                var message = $"An error occurred - {ex.Message}";
                WriteLine(message);
                Logger.LogError(ex, message);
            }
        }

        #region Helper Methods

        /// <summary>
        /// Helper method to create the Host Service Provider.
        /// </summary>
        /// <returns>Returns a <see cref="IServiceProvider">IServiceProvider</see>.</returns>
        private static System.IServiceProvider CreateServiceProvider(string timecampToken)
        {
            // Parameter check.
            if (string.IsNullOrWhiteSpace(timecampToken))
                throw new System.ArgumentNullException("TimeCamp Token cannot be empty.");

            TimeCampService.TimeCampToken = timecampToken;

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient<ITimeCampService, TimeCampService>();
                }).UseConsoleLifetime();

            var host = builder.Build();
            var serviceScope = host.Services.CreateScope();
            return serviceScope.ServiceProvider;
        }

        #endregion
    }
}