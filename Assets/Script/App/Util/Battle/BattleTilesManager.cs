using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;

namespace App.Util.Battle{
    public class BattleTilesManager{
        private static CBattlefield cBattlefield;
        private static App.Model.Master.MBaseMap topMapMaster;
        public static void Init(CBattlefield controller, App.Model.Master.MBaseMap mBaseMap){
            cBattlefield = controller;
            topMapMaster = mBaseMap;
        }
        public static void ShowCharacterMovingArea(VCharacter vCharacter, int index, Vector2 coordinate){
            List<Vector2> coordinates = new List<Vector2>();
            if (coordinate.y > 0)
            {
                if (coordinate.y % 2 == 0)
                {
                    if (coordinate.x > 0)
                    {
                        coordinates.Add(new Vector2(coordinate.x - 1, coordinate.y - 1));
                    }
                    coordinates.Add(new Vector2(coordinate.x, coordinate.y - 1));
                }
                else
                {
                    coordinates.Add(new Vector2(coordinate.x, coordinate.y - 1));
                    if (coordinate.x + 1 < topMapMaster.width)
                    {
                        coordinates.Add(new Vector2(coordinate.x + 1, coordinate.y - 1));
                    }
                }
            }
            if (coordinate.x + 1 < topMapMaster.width)
            {
                coordinates.Add(new Vector2(coordinate.x + 1, coordinate.y));
            }
            if (coordinate.y + 1 < topMapMaster.height)
            {
                if (coordinate.y % 2 == 0)
                {
                    coordinates.Add(new Vector2(coordinate.x, coordinate.y + 1));
                    if (coordinate.x > 0)
                    {
                        coordinates.Add(new Vector2(coordinate.x - 1, coordinate.y + 1));
                    }
                }
                else
                {
                    if (coordinate.x + 1 < topMapMaster.width)
                    {
                        coordinates.Add(new Vector2(coordinate.x + 1, coordinate.y + 1));
                    }
                    coordinates.Add(new Vector2(coordinate.x, coordinate.y + 1));
                }
            }
            if (coordinate.x > 0)
            {
                coordinates.Add(new Vector2(coordinate.x - 1, coordinate.y));
            }
            List<VTile> tiles = new List<VTile>();
            VBaseMap vBaseMap = cBattlefield.GetVBaseMap();
            foreach(Vector2 vec in coordinates){
                int i = (int)vec.y * vBaseMap.mapWidth + (int)vec.x;
                VTile tile = vBaseMap.tileUnits[i];
                tiles.Add(tile);
            }
            vBaseMap.SetTilesColor(tiles, Color.red);
        }
        public static void Destory(){
            
        }
    }
}