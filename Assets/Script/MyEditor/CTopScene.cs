using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using System.IO;


namespace App.Controller{
    public class CTopScene : CScene {
        [SerializeField] private int width = 30;
        [SerializeField] private int height = 30;
        [SerializeField] private VTile tileUnit;
        [SerializeField] private VTopMap map;
        public override IEnumerator OnLoad( Request req ) 
		{  
            CreateMap();
			yield return 0;
        }
        void CreateMap()
        {
            List<VTile> tiles = new List<VTile>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject obj = GameObject.Instantiate (tileUnit.gameObject);
                    obj.name = "Tile_"+(i + 1)+"_"+(j + 1);
                    obj.transform.parent = map.transform;
                    obj.transform.localPosition = new Vector3(j * 0.69f + (i % 2) * 0.345f, -i * 0.6f, 0f);
                    tiles.Add(obj.GetComponent<VTile>());
                }
            }
            map.mapWidth = width;
            map.mapHeight = height;
            map.tileUnits = tiles.ToArray();
        }
	}
}