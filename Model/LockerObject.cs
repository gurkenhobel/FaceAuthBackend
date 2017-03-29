using MongoDB.Bson;

namespace FaceAuthService.Model
{
    class LockerObject
    {
        public string name {get;set;}

        public bool encrypted  {get;set;}
    }
}