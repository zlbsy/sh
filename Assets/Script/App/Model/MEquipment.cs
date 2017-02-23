using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
	public class MEquipment : MBase {
        public MEquipment(){
            viewModel = new VMEquipment ();
        }
        public VMEquipment ViewModel { get { return (VMEquipment)viewModel; } }
        public int user_id;//
        public int character_id;//
        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public int EquipmentId{
            set{ 
                ViewModel.EquipmentId.Value = value;
            }
            get{ 
                return ViewModel.EquipmentId.Value;
            }
        }
        public App.Model.Master.MEquipment.EquipmentType EquipmentType{
            set{ 
                ViewModel.EquipmentType.Value = value;
            }
            get{ 
                return ViewModel.EquipmentType.Value;
            }
        }
        public App.Model.Master.MEquipment Master{
            get{ 
                return App.Util.Cacher.EquipmentCacher.Instance.GetEquipment(this.Id, this.EquipmentType);
            }
        }
	}
}