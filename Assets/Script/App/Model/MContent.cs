using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
    public enum ContentType{
        clothes,
        horse,
        weapon,
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