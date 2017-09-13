using NoDaysOffApp.Features.Core;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;

namespace NoDaysOffApp.Features.Exercises
{
    public interface IExercisesEventBusMessageHandler: IEventBusMessageHandler { }

    public class ExercisesEventBusMessageHandler: IExercisesEventBusMessageHandler
    {
        public ExercisesEventBusMessageHandler(ICache cache)
        {
            _cache = cache;
        }

        public void Handle(JObject message)
        {
            try
            {
                if ($"{message["type"]}" == ExercisesEventBusMessages.AddedOrUpdatedExerciseMessage)
                {
                    _cache.Remove(ExercisesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
                }

                if ($"{message["type"]}" == ExercisesEventBusMessages.RemovedExerciseMessage)
                {
                    _cache.Remove(ExercisesCacheKeyFactory.Get(new Guid(message["tenantUniqueId"].ToString())));
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
