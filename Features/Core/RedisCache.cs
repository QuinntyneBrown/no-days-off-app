using System;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace NoDaysOffApp.Features.Core
{
    public class RedisCache : Cache
    {
        private static volatile RedisCache _current = null;
        private IDatabase _database { get; set; }
        private ConnectionMultiplexer _connection { get; set; }

        public RedisCache()
        {
            _connection = ConnectionMultiplexer.Connect(RedisCacheConfiguration.Config.ConnectionString);
            _database = _connection.GetDatabase();
        }

        public static RedisCache Current
        {
            get
            {
                if (_current == null)
                    _current = new RedisCache();
                return _current;
            }
        }

        public override void Add(object objectToCache, string key)
        {
            _database.StringSet(key, JsonConvert.SerializeObject(objectToCache, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All,
            }));
        }

        public override void Add<T>(object objectToCache, string key) => Add(objectToCache, key);

        public override void Add<T>(object objectToCache, string key, double cacheDuration)
        {
            _database.StringSet(key, JsonConvert.SerializeObject(objectToCache, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.All
            }), TimeSpan.FromMinutes(cacheDuration));
        }

        public override void ClearAll()
        {
            foreach (var endpoint in _connection.GetEndPoints(true))
            {
                var server = _connection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public override bool Exists(string key) => _database.KeyExists(key);

        public override T Get<T>(string key)
        {
            RedisValue redisValue = _database.StringGet(key);

            if (redisValue.IsNull)
                return default(T);

            return JsonConvert.DeserializeObject<T>(redisValue
                                    , new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                        TypeNameHandling = TypeNameHandling.All
                                    });
        }

        public override object Get(string key)
        {
            try
            {
                RedisValue redisValue = _database.StringGet(key);

                if (redisValue.IsNull)
                    return null;

                return JsonConvert.DeserializeObject(redisValue
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                            TypeNameHandling = TypeNameHandling.All
                        });

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public override void Remove(string key) => _database.KeyDelete(key);
    }
}