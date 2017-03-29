using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FaceAuthService
{
    static class Config
    {
        private static string _cfgDir = Directory.GetCurrentDirectory() + "/cfg/";

        public static string LoadDBName()
        {
            string res = null;

            res = File.ReadAllText(_cfgDir + "db.cfg");

            return res;
        }

        public static int LoadPort()
        {
            int res = -1;

            res = int.Parse(File.ReadAllText(_cfgDir + "port.cfg"));

            return res;
        }
    }
}
