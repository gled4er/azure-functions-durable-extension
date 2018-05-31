// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace VSSample
{
    public static class HttpStart
    {
        [FunctionName("HttpStart")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = "orchestrators/{functionName}")] HttpRequestMessage req,
            string functionName,
            ILogger log)
        {
            DurableOrchestrationClient durableOrchestrationClient = new DurableOrchestrationClient(null, null);
            var instanceId = await durableOrchestrationClient.StartNewAsync(functionName, null);
            // Function input comes from the request content.
            // dynamic eventData = await req.Content.ReadAsAsync<object>();
            // string instanceId = await starter.StartNewAsync(functionName, eventData);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            var res = durableOrchestrationClient.CreateCheckStatusResponse(req, instanceId);
            res.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(10));
            return res;
        }
    }
}
