using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
    public class MTopMap : MBase {
        public MTopMap(){
            viewModel = new VMTopMap ();
        }
        public VMTopMap ViewModel { get { return (VMTopMap)viewModel; } }
        public int MapId{
            set{
                this.ViewModel.MapId.Value = value;
            }
            get{ 
                return this.ViewModel.MapId.Value;
            }
        }
        public MTile[] Tiles{
            set{
                this.ViewModel.Tiles.Value = value;
            }
            get{ 
                return this.ViewModel.Tiles.Value;
            }
        }
	}
}