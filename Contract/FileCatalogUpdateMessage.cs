using System.Collections.Generic;
using FaceAuthService.Model;

namespace FaceAuthService.Contract
{
    class FileCatalogUpdateMessage : Message
    {

        public List<LockerObject> files;


        public FileCatalogUpdateMessage()
        {
            MessageType = MessageType.FileCatalogUpdate;
        }
    }
}