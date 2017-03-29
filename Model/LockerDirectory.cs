using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace FaceAuthService.Model
{
    [Serializable]
    class LockerDirectory : LockerObject
    {
        public List<LockerObject> content {get;set;}        
    }
}