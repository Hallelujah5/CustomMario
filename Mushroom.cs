using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMario.Map;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;

namespace CustomMario
{
    public class Mushroom
    {
        Bitmap mushroom;
        Rectangle mushroomRect;
        Point2D _location;

        public Mushroom(double x, double y)
        {
            mushroom = new Bitmap("mushroom", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\mushroom2.png");
            _location.X = x;
            _location.Y = y;
            
        }
        

       












    }
}
