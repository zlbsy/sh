using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;

namespace App.Util{
	public class Global {
        public static MUser User;
        public static void Initialize()
        {
            User = new MUser();
        }
	}
}