using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;
using App.Controller;
using App.Util.Cacher;
using App.View;

namespace App.Util.Search{
    public class TileMap{
        private VBaseMap vBaseMap;
        private MBaseMap mBaseMap;
        private App.Model.Master.MBaseMap baseMapMaster;
        public TileMap(MBaseMap model, VBaseMap view){
            mBaseMap = model;
            vBaseMap = view;
            baseMapMaster = BaseMapCacher.Instance.Get(mBaseMap.MapId);
        }
        public List<Vector2> GetNeighboringCoordinates(Vector2 coordinate){
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
                    if (coordinate.x + 1 < baseMapMaster.width)
                    {
                        coordinates.Add(new Vector2(coordinate.x + 1, coordinate.y - 1));
                    }
                }
            }
            if (coordinate.x + 1 < baseMapMaster.width)
            {
                coordinates.Add(new Vector2(coordinate.x + 1, coordinate.y));
            }
            if (coordinate.y + 1 < baseMapMaster.height)
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
                    if (coordinate.x + 1 < baseMapMaster.width)
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
            return coordinates;
        }
    }
}