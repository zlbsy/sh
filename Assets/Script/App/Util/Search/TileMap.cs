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
        public VTile GetTile(int index){
            Vector2 coordinate = baseMapMaster.GetCoordinateFromIndex(index);
            return GetTile(coordinate);
        }
        public VTile GetTile(Vector2 coordinate){
            int i = (int)coordinate.y * vBaseMap.mapWidth + (int)coordinate.x;
            return vBaseMap.tileUnits[i];
        }
        public List<Vector2> GetNeighboringCoordinates(Vector2 coordinate){
            return GetNeighboringCoordinates((int)coordinate.x, (int)coordinate.y);
        }
        public List<Vector2> GetNeighboringCoordinates(int coordinateX, int coordinateY){
            List<Vector2> coordinates = new List<Vector2>();
            if (coordinateY > 0)
            {
                if (coordinateY % 2 == 0)
                {
                    if (coordinateX > 0)
                    {
                        coordinates.Add(new Vector2(coordinateX - 1, coordinateY - 1));
                    }
                    coordinates.Add(new Vector2(coordinateX, coordinateY - 1));
                }
                else
                {
                    coordinates.Add(new Vector2(coordinateX, coordinateY - 1));
                    if (coordinateX + 1 < baseMapMaster.width)
                    {
                        coordinates.Add(new Vector2(coordinateX + 1, coordinateY - 1));
                    }
                }
            }
            if (coordinateX + 1 < baseMapMaster.width)
            {
                coordinates.Add(new Vector2(coordinateX + 1, coordinateY));
            }
            if (coordinateY + 1 < baseMapMaster.height)
            {
                if (coordinateY % 2 == 0)
                {
                    coordinates.Add(new Vector2(coordinateX, coordinateY + 1));
                    if (coordinateX > 0)
                    {
                        coordinates.Add(new Vector2(coordinateX - 1, coordinateY + 1));
                    }
                }
                else
                {
                    if (coordinateX + 1 < baseMapMaster.width)
                    {
                        coordinates.Add(new Vector2(coordinateX + 1, coordinateY + 1));
                    }
                    coordinates.Add(new Vector2(coordinateX, coordinateY + 1));
                }
            }
            if (coordinateX > 0)
            {
                coordinates.Add(new Vector2(coordinateX - 1, coordinateY));
            }
            return coordinates;
        }
    }
}