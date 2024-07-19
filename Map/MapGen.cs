using System;
using System.Collections.Generic;
using System.Data;
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
            for (int i = -2; i < 300; i++)
            {

                //Grass block
                if (i != 30 && i != 31 && i != 32 && i != 70 && i != 71 && i != 72 && i != 100 && i != 101 && i != 102 && i != 103 && i != 120 && i != 121 && i != 122 && i != 123 && i != 153 && i != 154 && i != 155 && i != 156 && i != 157)
                {
                    if (i < 190)
                    {
                        GrassBlock tile = new GrassBlock(75 * i, 627);
                        map.Add(tile);
                    }
                }

                //Question Block
                if (i == 31 || i == 53 || i == 58)
                {
                    if (i == 58)
                    {
                        QuestionBlock block2 = new QuestionBlock(60 * i, 80);  
                        map.Add(block2);
                    }
                    QuestionBlock block = new QuestionBlock(60 * i, 330);
                    map.Add(block);
                }
                if (i == 73)
                {
                    QuestionBlock block = new QuestionBlock(60 * i, 80);
                    map.Add(block);
                }
                if ( i == 108 || i == 109 || i == 110 || i == 111)
                {
                    QuestionBlock block3 = new QuestionBlock(60 * i, -190);
                    map.Add(block3);
                }
                if (i == 116)
                {
                    QuestionBlock used2 = new QuestionBlock(60 * i, -245);
                    map.Add(used2);
                }
                if (i == 140)
                {
                    QuestionBlock used2 = new QuestionBlock(60 * i, 20);
                    map.Add(used2);
                }



                //Used block
                if (i == 30 || i == 32 || i == 33 || i == 56 || i == 57 || i == 58 || i == 59 ||
                    i == 70 || i == 71 || i == 72 || i == 73 || i == 74 ||
                    i == 137 || i == 138 || i == 139 || i == 140 || i == 141)
                {

                    usedBlock used = new usedBlock(60 * i, 330);
                    map.Add(used);
                }
                if (i == 106 || i == 107 || i == 108 || i == 109 || i == 110 || i == 111 || i == 112 || i == 113 || i == 114)
                {
                   
                    usedBlock used = new usedBlock(60 * i, 90);
                    map.Add(used);
                }
                if ( i == 115 || i == 116 || i == 117 || i == 118)
                {

                    usedBlock used = new usedBlock(60 * i, 30);
                    map.Add(used);
                    if (i ==117)
                    {
                        usedBlock used2 = new usedBlock(60 * i, -245);
                        map.Add(used2);
                    }
                }
                if (i == 68 || i == 69){ usedBlock used3 = new usedBlock(60 * i, 390); map.Add(used3);}

                if (i == 72)
                {
                    usedBlock block = new usedBlock(60 * i, 80);
                    map.Add(block);
                }

                //Parkour part
            
                if (i == 175 || i == 174 || i == 173 || i == 172)
                {
                    usedBlock used = new usedBlock(60 * i, 270);
                    map.Add(used);
                }
                
                if (i == 167 || i == 166 || i == 165)
                {
                    usedBlock used = new usedBlock(60 * i, 10);
                    map.Add(used);
                }

                if (i == 175 || i == 174 || i == 173)
                {
                    usedBlock used = new usedBlock(60 * i, -180);
                    map.Add(used);
                }

                if (i == 180 || i == 181 || i == 182 || i == 183 || i == 184 || i == 185 || i == 186 || i == 187 || i == 188 || i == 189 || i == 190)
                {
                    if (i == 183 || i == 184 || i == 185 || i ==186)
                    {
                        QuestionBlock block3 = new QuestionBlock(60 * i, -420);
                        map.Add(block3);
                    }

                    Stone used = new Stone(60 * i, -180);
                    map.Add(used);
                }


                





                    //Castle
                    if (i == 180)
                {
                    Castle castle = new Castle(75 * i, 396);
                }




                    
                //Stones
                if (i == 101 || i == 104 || i == 103 || i == 102 || i == 131)
                {
                    if (i == 131) { Stone stone = new Stone(60 * i, 570); Stone stone2 = new Stone(60 * i, 510); map.Add(stone); map.Add(stone2); }
                    if (i == 101 || i == 104)
                    {
                        Stone stone = new Stone(60 * i, 570);
                        Stone stone2 = new Stone(60 * i, 510);
                        Stone stone3 = new Stone(60 * i, 450);
                        Stone stone4 = new Stone(60 * i, 390);
                        map.Add(stone);
                        map.Add(stone2);
                        map.Add(stone3);
                        map.Add(stone4);
                    }
                    if (i == 102 ||  i == 103)
                    {
                        Stone stone = new Stone(60 * i, 390);
                        map.Add(stone);
                    }
                }


                if (i == 155 || i == 156 || i == 157 || i == 158 || i == 159)
                {
                    if (i == 155)
                    {
                        Stone stone5 = new Stone(60 * i, 330);
                        map.Add(stone5);
                    }
                    if (i == 155 || i == 156)
                    {
                        Stone stone4 = new Stone(60 * i, 390);
                        map.Add(stone4);
                    }
                    if (i == 155 || i == 156 || i == 157)
                    {
                        Stone stone3 = new Stone(60 * i, 450);
                        map.Add(stone3);
                    }
                    if (i == 155 || i == 156 || i == 157 || i == 158)
                    {
                        Stone stone2 = new Stone(60 * i, 510);
                        map.Add(stone2);
                    }
                    Stone stone = new Stone(60 * i, 570);
                    map.Add(stone);
                }


                if (i == 190)
                {
                    Stone stone3 = new Stone(60 * i + 15, 510);
                    Stone stone4 = new Stone(60 * i + 15, 570);
                    map.Add(stone3);
                    map.Add(stone4);
                }
                if (i == 197)
                {
                    Stone stone3 = new Stone(60 * i + 30, 510);
                    Stone stone4 = new Stone(60 * i + 30, 570);
                    map.Add(stone3);
                    map.Add(stone4);
                }
                if(i == 196)
                {
                    Stone stone2 = new Stone(60 * i + 30, 510);
                    map.Add(stone2);
                }
                if (i == 191)
                {
                    Stone stone2 = new Stone(60 * i + 15, 510);
                    map.Add(stone2);
                }

                if (i == 218)
                {
                    Stone stone2 = new Stone(60 * i + 30, 570);
                    map.Add(stone2);
                }

                //Bushes
                if (i == 16 || i == 35 || i == 60 || i == 67 || i == 85 || i == 107 || i==130 || i ==164)
                {
                    Bush _bush = new Bush(75 * i, 590);         //small bush
                    SplashKit.DrawText("i:  " + i, Color.Black, 75* i , 330 );
                }



                if (i == 6 || i == 25 || i == 40 || i == 53 || i == 78 || i == 90 || i == 96 || i == 114 || i == 136 || i == 171) 
                {
                    Bush2 _bush2 = new Bush2(75 * i, 568);      //big bush
                    SplashKit.DrawText("i:  " + i, Color.Black, 75 * i, 330);
                }



                //Pipes
                if (i == 20 || i == 38 || i == 50 || i == 65 || i == 82 || i == 94 || i == 118 || i == 143) { Pipe _pipe = new Pipe(75*i,498); map.Add(_pipe); SplashKit.DrawText("i:  " + i, Color.Black, 75 * i, 330); }
            }




            //Invisible wall on left start
            for (int i = -2; i < 30; i++)
            {
                InvisBlock block = new InvisBlock(-150, 627 - 75 * i, 75, 75);
                map.Add(block);
            }

            map.Draw();       //Draw all block
        }

        public List<Rectangle> getRect()
        {
            return map.getRect();
        }





    }




}

