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
        public int GetDistance(VTile tile1, VTile tile2){
            if (tile2.CoordinateY == tile1.CoordinateY)
            {
                return Mathf.Abs(tile2.CoordinateX - tile1.CoordinateX);
            }
            int distance = 0;
            int directionY = tile2.CoordinateY > tile1.CoordinateY ? 1 : -1;
            int x = tile1.CoordinateX;
            int y = tile1.CoordinateY;
            do{
                distance += 1;
                if(tile2.CoordinateX != x){
                    if(y % 2 == 0){
                        if(tile2.CoordinateX < x){
                            x -= 1;
                        }
                    }else{
                        if(tile2.CoordinateX > x){
                            x += 1;
                        }
                    }
                }
                y += directionY;
            }while (tile2.CoordinateY != y);
            return Mathf.Abs(tile2.CoordinateX - x) + distance;
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