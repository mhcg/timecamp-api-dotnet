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

using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeCampAPI.Core.Services;
using TimeCampAPI.Core.Interfaces;
using System.Collections.Generic;
using TimeCampAPI.Core.Models.TimeCamp;
using System;

namespace TimeCampAPI.Tests
{
    public class GeneralTests
    {
        #region Tests setup.

        private static IServiceProvider Services { get; set; } = null!;

        /// <summary>
        /// Set up Services for use by tests.
        /// </summary>
        public GeneralTests()
        {
            var args = Array.Empty<string>();
            Services = CreateHostBuilder(args).Build()
                .Services.CreateScope().ServiceProvider;
        }

        /// <summary>
        /// Create a Generic Host.
        /// </summary>
        /// <param name="args">Would normally come from Program.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient<ITimeCampService, TimeCampService>();
            })
            .UseConsoleLifetime();

        #endregion

        [Fact]
        public async void CanAccessService()
        {
            var nonsenseDate = new DateTime(1901, 1, 1);
            var result = await Services.GetRequiredService<ITimeCampService>()
                    .GetTimeEntriesAsync(nonsenseDate, nonsenseDate);
            Assert.NotNull(result);
            Assert.Equal(result, new List<TimeEntry>());
        }
    }
}
