using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.ScheduledExercises
{
    public interface IScheduledExercisesEventBusMessageHandler: IEventBusMessageHandler { }

    public class ScheduledExercisesEventBusMessageHandler: IScheduledExercisesEventBusMessageHandler
    {
        public ScheduledExercisesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == ScheduledExercisesEventBusMessages.AddedOrUpdatedScheduledExerciseMessage)
                {
                    _cache.Remove(ScheduledExercisesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == ScheduledExercisesEventBusMessages.RemovedScheduledExerciseMessage)
                {
                    _cache.Remove(ScheduledExercisesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
