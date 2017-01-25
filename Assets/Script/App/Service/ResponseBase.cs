using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;


namespace App.Service{
    public class ResponseBase {
        public int result;
        public bool isScuess{
            get{ 
                return result == 1;
            }
        }
	}
}