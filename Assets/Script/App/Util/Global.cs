using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;

namespace App.Util{
	public class Global {
        public static SUser SUser;
        public static string ssid;
        public static void Initialize()
        {
            SUser = new SUser();
        }
	}
}