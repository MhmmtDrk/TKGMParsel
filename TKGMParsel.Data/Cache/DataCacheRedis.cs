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

        public void Clear()
        {
            try
            {
                if (_connection.Value.IsConnected)
                {
                    var server = _db.Multiplexer.GetServer(redisConnection.Split(',')[0].ToString());
                    foreach (var item in server.Keys())
                        _db.KeyDelete(item);
                }
            }
            catch (Exception)
            {

            }
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
        //public T GetString<T>(string key)
        //{
        //    try
        //    {
        //        if (_connection.Value.IsConnected)
        //        {
        //            var value = _db.StringGet(key);
        //            if (!value.IsNull)
        //                return JsonConvert.DeserializeObject<T>(value);
        //            else return default(T);
        //        }
        //        else return default(T);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public bool IsSet(string key)
        //{
        //    try
        //    {
        //        if (_connection.Value.IsConnected)
        //        {
        //            return _db.KeyExists(key);
        //        }
        //        else return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

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

        //public void RemoveByPattern(string pattern)
        //{
        //    _ = Task.Run(() =>
        //    {
        //        try
        //        {
        //            if (!_connection.Value.IsConnected) return;
        //            var server = _db.Multiplexer.GetServer(redisConnection.Split(',')[0].ToString());
        //            foreach (var item in server.Keys(Convert.ToInt32(redisConnection.Split(',')[5].ToString().Split('=')[1]), pattern: "" + pattern + ""))
        //                _db.KeyDelete(item);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }).ConfigureAwait(false);

        //}

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

        //public void SetString(string key, object data)
        //{
        //    try
        //    {
        //        SetString(key, data, DateTime.Now.AddDays(7));
        //        //if (_connection.Value.IsConnected)
        //        //{
        //        //    if (data == null)
        //        //        return;

        //        //    _db.StringSet(key, JsonConvert.SerializeObject(data));
        //        //    var expiresIn = DateTime.Now.AddDays(30);
        //        //    _db.KeyExpire(key, expiresIn);
        //        //}
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //public void SetString(string key, object data, DateTime exDate)
        //{
        //    try
        //    {
        //        if (_connection.Value.IsConnected)
        //        {
        //            if (data == null)
        //                return;

        //            var date = DateTime.Now;
        //            if (exDate > date)
        //            {
        //                var time = exDate.TimeOfDay - date.TimeOfDay;
        //                _db.StringSet(key, JsonConvert.SerializeObject(data), time);

        //            }
        //            else
        //                _db.StringSet(key, JsonConvert.SerializeObject(data));
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //<-- Begin Cache Helper -->
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
