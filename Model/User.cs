
using System;
using MongoDB.Bson;

namespace FaceAuthService.Model
{
    [Serializable]
    class User
    {
        public ObjectId _id{get;set;}
        public string name{get;set;}
        public string email {get;set;}
        public LockerDirectory locker {get;set;}
    }
}