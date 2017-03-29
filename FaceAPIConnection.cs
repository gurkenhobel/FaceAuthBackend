using System;
using System.IO;
using System.Threading.Tasks;
using FaceAuthService.Model;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;

namespace FaceAuthService
{
    class FaceAPIConnection
    {
        #region singleton impl
        public static FaceAPIConnection Instance { get; } = new FaceAPIConnection();
        protected FaceAPIConnection() { }
        #endregion

        private FaceServiceClient _client;

        public void Connect()
        {
            try
            {
                _client = new FaceServiceClient("4b9dee9fb9114b39a7c88e59a89c781a"); //TODO: extract api key
            }
            catch (FaceAPIException ex)
            {
                Console.WriteLine($"face api connection: {ex.ErrorMessage}");
                return;
            }
            Console.WriteLine($"face api connection: connected");
        }

        public async Task<ObjectId> Identify(byte[] image)
        {
            ObjectId result = ObjectId.Empty;
            try
            {
                var face = await DetectFace(image);

                if(face == null)
                {
                    return ObjectId.Empty;
                }


                var resp = await _client.IdentifyAsync("users_id", new Guid[] { face.FaceId });
                if (resp.Length == 1)
                {
                    var apiId = resp.First().Candidates.First().PersonId;
                    var dbId = await _client.GetPersonAsync("users_id", apiId);
                    result = new ObjectId(dbId?.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Identify: {ex.Message}");
            }
            return result;
        }


        private async Task<Face> DetectFace(byte[] img)
        {
            try
            {
                Face[] faces;
                using (var stream = new MemoryStream(img))
                {
                    faces = await _client.DetectAsync(stream, true, false);
                }

                return faces?.Length > 0 ? faces[0] : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DetectFace: {ex.Message}");
                return null;
            }
        }

        public async Task CreateGroup()
        {
            await _client.CreatePersonGroupAsync("users_id", "users");
        }


        public async Task<bool> RegisterUser(User user, IEnumerable<byte[]> imgs)
        {
            try
            {
                var persRes = await _client.CreatePersonAsync("users_id", user._id.ToString());
                foreach (var img in imgs)
                {
                    using (var s = new MemoryStream(img))
                    {
                        try
                        {
                            var faceRes = await _client.AddPersonFaceAsync("users_id", persRes.PersonId, s);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine($"RegisterUser.AddImage: {e.Message}");
                        }
                    }
                }
                await _client.TrainPersonGroupAsync("users_id");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RegisterUser: {ex.ToString()}");
                return false;
            }

            return true;
        }
    }
}