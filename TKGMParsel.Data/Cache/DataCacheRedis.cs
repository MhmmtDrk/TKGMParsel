using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TKGMParsel.Data.Cache
{
    public class DataCacheRedis
    {
        private static IDatabase _db;
        private static Lazy<ConnectionMultiplexer> _connection;
        public static IConfiguration _configuration;
        public string redisConnection;
        public DataCacheRedis(IConfiguration configuration)
        {
            _configuration = configuration;
            redisConnection = _configuration.GetConnectionString("RedisCS");
            CreateRedisDB(redisConnection);
        }
        private static IDatabase CreateRedisDB(string conn)
        {
            if (null == _db)
            {
                _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(conn));
                _db = _connection.Value.GetDatabase();
            }
            return _db;
        }
        public T Get<T>(string key)
        {
            try
            {
                if (_connection.Value.IsConnected)
                {
                    var rValue = _db.SetMembers(key);
                    if (rValue.Length == 0)
                        return default(T);

                    var result = Deserialize<T>(rValue.ToStringArray());
                    return result;
                }
                else return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }        
        public bool Remove(string key)
        {
            try
            {
                if (_connection.Value.IsConnected)
                {
                    return _db.KeyDelete(key);
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }        
        public void Set(string key, object data)
        {
            try
            {
                Set(key, data, DateTime.Now.AddMinutes(5));   
            }
            catch (Exception)
            {

            }
        }
        public void Set(string key, object data, DateTime exDate)
        {
            try
            {
                if (_connection.Value.IsConnected)
                {
                    if (data == null)
                        return;

                    var entryBytes = Serialize(data);
                    _db.SetAdd(key, entryBytes);

                    var date = DateTime.Now;
                    if (exDate > date)
                    {
                        var time = exDate - date;
                        _db.KeyExpire(key, time);
                    }
                }
            }
            catch (Exception)
            {

            }
        }      
        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        protected virtual T Deserialize<T>(string[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            string jsonString = "";
            foreach (var item in serializedObject)
                jsonString += item + ",";
            jsonString = jsonString.Substring(0, jsonString.Length - 1);
            //jsonString += "]";
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        //<-- End Cache Helper -->

    }
}
