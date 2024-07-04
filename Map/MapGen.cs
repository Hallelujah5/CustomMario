using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;


namespace CustomMario.Map
{
    public class MapGen
    {
        List<MapObject> _MapObj;
        List<Rectangle> _rects;

        public MapGen()
        {
            _MapObj = new List<MapObject>();
        }

        public void Add(MapObject tile)
        {
            _MapObj.Add(tile);
        }

        public void Draw()
        {
            foreach (MapObject tile in _MapObj)
            {
                tile.Draw();                //Draw all the tiles from MapObject class
            }
           // Console.WriteLine("Number of tiles: " + _MapObj.Count);
        }

        public List<Rectangle> getRect()
        {
            if (_rects == null)
            {
                _rects = new List<Rectangle>();     // if list not exist, create new
            }
            foreach (MapObject obj in _MapObj)
            {
                _rects.Add(obj.Rect());             //add all the rectangles of the tiles to the list
            }
            return _rects;
        }
    }
    public class DrawMap
    {
        MapGen map;

        public void Draw()
        {
            map = new MapGen();
            for (int i = 0; i < 30; i++)
            {
                GrassBlock tile = new GrassBlock(75 * i, 627);
                map.Add(tile);
            }

            map.Draw();       //Draw() in MapGen class
        }

        public List<Rectangle> getRect()
        {
            return map.getRect();
        }





    }




}

