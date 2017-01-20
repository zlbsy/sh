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
		public VMProperty<int> Hat = new VMProperty<int>();
		public VMProperty<string> Horse = new VMProperty<string>();
		public VMProperty<int> Body = new VMProperty<int>();
		public VMProperty<WeaponType> WeaponType = new VMProperty<WeaponType>();
		public VMProperty<MoveType> MoveType = new VMProperty<MoveType>();
		public VMProperty<ActionType> Action = new VMProperty<ActionType>();
	}
}