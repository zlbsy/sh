using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
    public enum ContentType{
        equipment,
        item,
        character
    }
	public class MContent : MBase {
        public MContent(){
            
        }
        public ContentType type;
        public int content_id;
	}
}