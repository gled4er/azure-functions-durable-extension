﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.DurableTask
{
    /// <summary>
    /// Abstract class for <see cref="DurableOrchestrationClient"/>
    /// </summary>
    public abstract class DurableOrchestrationClientBase
    {
        /// <summary>
        /// Creates an HTTP response for checking the status of the specified instance. 
        /// </summary>
        /// <param name="request">The HTTP request that triggered the current function.</param>
        /// <param name="instanceId">The unique ID of the instance to check.</param>
        /// <returns>An HTTP response which may include a 202 and locaton header.</returns>
        public abstract HttpResponseMessage CreateCheckStatusResponse(HttpRequestMessage request, string instanceId);

        /// <summary>
        /// Starts a new execution of the specified orchestrator function.
        /// </summary>
        /// <param name="orchestratorFunctionName">The name of the orchestrator function to start.</param>
        /// <param name="input">JSON-serializeable input value for the orchestrator function.</param>
        /// <returns>A task that completes when the start message is enqueued.</returns>
        /// <exception cref="ArgumentException">
        /// The specified function does not exist, is disabled, or is not an orchestrator function.
        /// </exception>
        public virtual Task<string> StartNewAsync(string orchestratorFunctionName, object input)
        {
            return this.StartNewAsync(orchestratorFunctionName, string.Empty, input);
        }

        /// <summary>
        /// Starts a new execution of the specified orchestrator function.
        /// </summary>
        /// <param name="orchestratorFunctionName">The name of the orchestrator function to start.</param>
        /// <param name="instanceId">A unique ID to use for the new orchestration instance.</param>
        /// <param name="input">JSON-serializeable input value for the orchestrator function.</param>
        /// <returns>A task that completes when the start message is enqueued.</returns>
        /// <exception cref="ArgumentException">
        /// The specified function does not exist, is disabled, or is not an orchestrator function.
        /// </exception>
        public abstract Task<string> StartNewAsync(string orchestratorFunctionName, string instanceId, object input);

        /// <summary>
        /// Sends an event notification message to a running orchestration instance.
        /// </summary>
        /// <param name="instanceId">The ID of the orchestration instance that will handle the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventData">The JSON-serializeable data associated with the event.</param>
        /// <returns>A task that completes when the event notification message has been enqueued.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public abstract Task RaiseEventAsync(string instanceId, string eventName, object eventData);

        /// <summary>
        /// Terminates a running orchestration instance.
        /// </summary>
        /// <param name="instanceId">The ID of the orchestration instance to terminate.</param>
        /// <param name="reason">The reason for terminating the orchestration instance.</param>
        /// <returns>A task that completes when the terminate message is enqueued.</returns>
        public abstract Task TerminateAsync(string instanceId, string reason);

        /// <summary>
        /// Gets the status of the specified orchestration instance.
        /// </summary>
        /// <param name="instanceId">The ID of the orchestration instance to query.</param>
        /// <returns>Returns a task which completes when the status has been fetched.</returns>
        public abstract Task<DurableOrchestrationStatus> GetStatusAsync(string instanceId);
    }
}
