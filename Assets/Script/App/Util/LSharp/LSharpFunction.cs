﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util.LSharp{
    public class LSharpFunction : LSharpBase {
        public override void analysis(){
            LSharpScript.Instance.analysis();
        }
	}
}