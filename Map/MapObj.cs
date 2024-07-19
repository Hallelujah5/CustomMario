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



    public class QuestionBlock : MapObject
    {
        Point2D _pt;
        Bitmap block;
        Rectangle _rect;
        //Sprite _sprite;

        public QuestionBlock(double x, double y)
        {
            block = new Bitmap("QuestionBlock", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\questionblock3.png");
            _pt.X = x;
            _pt.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), block.Width, block.Height);

            ////animation script
            //block.SetCellDetails(block.Width / 4, block.Height, 4, 1, 4);
            //AnimationScript walkingScript = SplashKit.LoadAnimationScript("questionBlockScript", "questionBlockScript.txt");
            //_sprite = SplashKit.CreateSprite(block, walkingScript);
            //_rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), Convert.ToInt32(block.Width / 4), Convert.ToInt32(block.Height));

        }
        public override void Draw()
        {
            //_sprite.StartAnimation("animate");
            //_sprite.Draw(_pt.X, _pt.Y);
            //_sprite.Update();
            //_sprite.UpdateAnimation();
            block.Draw(_pt.X, _pt.Y);
            
        }


        public override Rectangle Rect() { return _rect; }


    }





    public class usedBlock : MapObject
    {
        Point2D _pt;
        Bitmap block;
        Rectangle _rect;

        public usedBlock(double x, double y)
        {
            block = new Bitmap("usedBlock", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\usedblock2.png");
            _pt.X = x;
            _pt.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), block.Width, block.Height);
        }

        public override void Draw()
        {
            block.Draw(_pt.X, _pt.Y);
        }
         public override Rectangle Rect() { return _rect; }
    }




    //Invis wall
    public class InvisBlock : MapObject
        {
            Point2D point, size;
            Rectangle rect;


            public InvisBlock(double x, double y, double widthX, double widthY ) 
            {
                point.X = x;
                point.Y = y;
                size.X = widthX;
                size.Y = widthY;
            rect = new Rectangle(Convert.ToInt32(point.X), Convert.ToInt32(point.Y), Convert.ToInt32(size.X), Convert.ToInt32(size.Y));
            }

        public override void Draw()
        {
            SplashKit.FillRectangle(Color.RGBAColor(255, 0, 0, 128), rect.X, rect.Y, rect.Width, rect.Height);
        }
        public override Rectangle Rect() { return rect; }
    }



    //Pipe
    public class Pipe : MapObject
    {
        Point2D _pt;
        Bitmap _pipe;
        Rectangle _rect;
        public Pipe(double x, double y) 
        {
            _pipe = new Bitmap("Pipe", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\pipe2.png");
            _pt.X = x;
            _pt.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), _pipe.Width, _pipe.Height);
        }
        public override void Draw()
        {
            _pipe.Draw(_pt.X, _pt.Y);
        }
        public override Rectangle Rect() { return _rect;}
    }

    public class miniPipe : MapObject
    {
        Point2D _pt;
        Bitmap _pipe;
        Rectangle _rect;
        public miniPipe(double x, double y)
        {
            _pipe = new Bitmap("MiniPipe", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\pipe_mini.png");
            _pt.X = x;
            _pt.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), _pipe.Width, _pipe.Height);
        }
        public override void Draw()
        {
            _pipe.Draw(_pt.X, _pt.Y);
        }
        public override Rectangle Rect() { return _rect; }
    }




    //Stone block
    public class Stone : MapObject
    {
        Point2D _pt;
        Bitmap stone;
        Rectangle _rect;
        public Stone(double x, double y)
        {
            stone = new Bitmap("Stone", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\stoneBlock.png");
            _pt.X = x;
            _pt.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_pt.X), Convert.ToInt32(_pt.Y), stone.Width, stone.Height);
        }
        public override void Draw()
        {
            stone.Draw(_pt.X, _pt.Y);
        }
        public override Rectangle Rect() { return _rect;}
    }




    //Bushes
    public class Bush
    {
        Bitmap _bush;
        public Bush(double x, double y) 
        {
            _bush = new Bitmap("Bush", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\bush11.png");
            _bush.Draw(x, y);
        }
    }
    public class Bush2
    {
        Bitmap _bush;
        public Bush2(double x, double y)
        {
            _bush = new Bitmap("Bush2", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\bush22.png");
            _bush.Draw(x, y);
        }
    }

    public class Castle
    {
        Bitmap _castle;
        public Castle(double x, double y) 
        {
            _castle = new Bitmap("Castle", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\castle2.png");
            _castle.Draw(x, y);
        }
    }
}










