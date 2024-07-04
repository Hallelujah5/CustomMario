using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;

namespace CustomMario.Map
{
    public abstract class MapObject
    {
        public abstract void Draw();
        public abstract Rectangle Rect();
    }

    public class GrassBlock : MapObject
    {
        Point2D _pt;
        Bitmap _tile;
        Rectangle _rect;

        public GrassBlock(double x, double y)               
        {
            _tile = new Bitmap("GrassTile", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\grassblock.png");
            _pt.X = x;
            _pt.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), _tile.Height, _tile.Height);
        }
        public override void Draw()
        {
          _tile.Draw(_pt.X, _pt.Y);
            //Console.WriteLine("Drawn tile at X: " + _pt.X + "  Y: " +_pt.Y );
            //Console.WriteLine("the tile size is " + _tile.Height);
        }


        public override Rectangle Rect() { return _rect; }


    }












}
