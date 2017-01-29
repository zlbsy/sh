using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
	public class MTop : MBase {
        public MTop(){
			viewModel = new VMTop ();
		}
        public VMTop ViewModel { get { return (VMTop)viewModel; } }

        public MTopMap TopMap{
            set{
                this.ViewModel.TopMap.Value = value;
            }
            get{ 
                return this.ViewModel.TopMap.Value;
            }
        }
	}
}