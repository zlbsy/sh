using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;


namespace App.Model{
	public class MBuilding : MBase {
        public MBuilding(){
			viewModel = new VMBuilding ();
		}
        public VMBuilding ViewModel { get { return (VMBuilding)viewModel; } }

        public int Id{
            set{
                this.ViewModel.Id.Value = value;
                this.ViewModel.Name.Value = Tile.name;
                this.ViewModel.TileId.Value = Master.tile_id;
            }
            get{ 
                return this.ViewModel.Id.Value;
            }
        }
        public Master.MBuilding Master{
            get{ 
                return App.Util.Cacher.BuildingCacher.Instance.Get(Id);
            }
        }
        public Master.MTile Tile{
            get{ 
                return App.Util.Cacher.TileCacher.Instance.Get(Master.tile_id);
            }
        }
	}
}