using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMCharacter : VMBase
	{
		public VMProperty<string> Name = new VMProperty<string>();
		public VMProperty<int> Head = new VMProperty<int>();
	}
}