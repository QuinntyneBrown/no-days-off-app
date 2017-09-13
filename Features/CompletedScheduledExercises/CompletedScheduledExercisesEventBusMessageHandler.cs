using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.CompletedScheduledExercises
{
    public interface ICompletedScheduledExercisesEventBusMessageHandler: IEventBusMessageHandler { }

    public class CompletedScheduledExercisesEventBusMessageHandler: ICompletedScheduledExercisesEventBusMessageHandler
    {
        public CompletedScheduledExercisesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == CompletedScheduledExercisesEventBusMessages.AddedOrUpdatedCompletedScheduledExerciseMessage)
                {
                    _cache.Remove(CompletedScheduledExercisesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == CompletedScheduledExercisesEventBusMessages.RemovedCompletedScheduledExerciseMessage)
                {
                    _cache.Remove(CompletedScheduledExercisesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private readonly ICache _cache;
    }
}
