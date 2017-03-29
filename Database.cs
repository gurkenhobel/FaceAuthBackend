using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FaceAuthService.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FaceAuthService
{

    class Database
    {

        #region singleton impl
        public static Database Instance { get; } = new Database();
        protected Database()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase(Config.LoadDBName());
        }
        #endregion

        private static MongoClient _client;
        private static IMongoDatabase _database;


        public async Task InsertUser()
        {
            var file = new LockerObject { name = "test-file" };
            var dir = new LockerDirectory { name = "test-dir" };
            var locker = new LockerDirectory() { name = "test-locker", content = new List<LockerObject>() { file, dir } };
            var doc = new User() { name = "test", locker = locker };

            var collection = _database.GetCollection<User>("users");
            await collection.InsertOneAsync(doc);
        }

        public async Task<User> AddUser(User newUser)
        {
            try
            {
                Console.WriteLine(newUser.ToJson());

                var collection = _database.GetCollection<User>("users");

                if (newUser.locker == null)
                {
                    var locker = new LockerDirectory() { name = $"{newUser.name}-locker", content = new List<LockerObject> { new LockerObject { name = "test1" }, new LockerObject { name = "test2" } } };
                    newUser.locker = locker;
                }

                await collection.InsertOneAsync(newUser);
                var filter = Builders<User>.Filter.Eq("email", newUser.email);
                var result = await collection.FindAsync<User>(filter);
                return result.First();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddUser: {ex.ToString()}");
                return null;
            }

        }

        public List<User> GetAllUsers()
        {
            var collection = _database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Empty;
            var res = collection.Find(filter);
            var reslist = res.ToList();

            return reslist;
        }

        public User GetUser(ObjectId id)
        {
            var collection = _database.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq("_id", id);
            var res = collection.Find(filter);
            if (res.Count() != 1)
            {
                return null;
            }
            return res.First();
        }

        public void AddFile(User user, LockerObject file)
        {
            try
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq("_id", user._id);
                var update = Builders<User>.Update.AddToSet("locker.content", file);
                collection.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"addfile: {ex.ToString()}");
            }
        }

        public void DeleteFile(User user, LockerObject file)
        {
            try
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq("_id", user._id);
                var update = Builders<User>.Update.PullFilter(u => u.locker.content,
                                                              f => f.name == file.name);
                collection.UpdateOne(filter, update);   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"addfile: {ex.ToString()}");
            }
        }
    }
}