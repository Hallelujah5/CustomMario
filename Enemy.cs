using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = System.Drawing.Rectangle;
using CustomMario.Map;
using System.Reflection;
using System.Xml;

namespace CustomMario
{       

    public class Enemies
    {
        List<Enemy> _enemies;
        public Enemies()
        {
            _enemies = new List<Enemy>();
        }

        public void Add(Enemy _enemy)
        {
            _enemies.Add(_enemy); // add all the enemies into a list and Draw all of them
        }
        public List<Enemy> GetEnemylist()
        {
            return _enemies;
        }
        public void Draw(List<Rectangle> rects, Rectangle Mario_hitbox)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                Enemy _enemy = _enemies[i];
                _enemy.Moving(rects, Mario_hitbox);

            }
        }


    }

    public abstract class Enemy
    {
        public abstract void Draw();
        public abstract void setHitbox();
        public abstract void Moving(List<Rectangle> rects, Rectangle Mario_hitbox);
        public Boolean Collision(List<Rectangle> rects, Rectangle RECT)     //rects is the list<> rects of the map, RECT is the directional hitbox of the character.
        {

            foreach (Rectangle rect in rects)
            {

                if (RECT.IntersectsWith(rect))
                {
                    return true;                       //returns true if collide 
                }

            }
            return false;                //else return false
        }

        public double AntiGlitch(List<Rectangle> rects, Rectangle RECT, double speedX, double speedY)   //calculates a safe speed value to apply to the player's position.
        {
            double length;
            if (speedX == 0)
            {
                length = speedY;
            }
            else { length = speedX; }

            Boolean negative;
            if (speedX < 0 || speedY < 0)       //if speedX is negative, means player is going left, negative speedY means jumping/going up.
            {
                negative = true;        //left, up
            }
            else { negative = false; }      //right, down

            Rectangle c_rect = RECT;         //placeholder for the position (top, bottom, left, right) of the 4 rect hitbox of the player.

            for (double i = 0; i < Math.Abs(length) - 1; i++)       //Absolute value
            {
                if (speedX != 0)
                {
                    if (speedX > 0) { c_rect.X++; }     //calculates the X movable distance before colliding
                    else { c_rect.X--; }
                }
                if (speedY != 0)
                {
                    if (speedY > 0) { c_rect.Y++; }     //calc for Y factor
                    else { c_rect.Y--; }
                }
                foreach (Rectangle rect in rects)
                {
                    if (c_rect.IntersectsWith(rect))
                    {
                        if (!negative) { return i; }
                        else { return i * -1; }       //returns the movable safe distance before colliding
                    }
                }
            }
            return length;
        }


    }
    public class Goomba : Enemy
    {
        public Bitmap _moving;
        const int speed = 7;
        Point2D _location;
        private Rectangle _rectUp;
        private Rectangle _rectDown;
        private Rectangle _rectLeft;
        private Rectangle _rectRight;

        private Rectangle _hitbox;

        public Goomba(double x, double y)
        {
            _moving = new Bitmap("goombaMove", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\goomba.png");  

            _location.X = x * 75;
            _location.Y = y;
       
        }

        public Boolean onAir(List<Rectangle> rects)
        {

            foreach (Rectangle rect in rects)
            {

                if (_rectDown.IntersectsWith(rect))         //if bottom hitbox intersects with the terrain tiles, it means is on the ground.
                {
                    return false;
                }

            }
            return true;            //else is on air
        }

        //Goomba has a hitbox.
        //When it touches a player, the player dies.
        //Moves backwards when collide with a wall/a cliff


        public override void setHitbox()
        {
            _rectUp = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 60, 1);
            _rectDown = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y) + 60, 60, 1);
            _rectLeft = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y), 1, 60);
            _rectRight = new Rectangle(Convert.ToInt32(_location.X)+60, Convert.ToInt32(_location.Y), 1, 60);

            _hitbox = new Rectangle(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y),60, 57);

            SplashKit.DrawRectangle(Color.Red, _rectUp.X, _rectUp.Y, _rectUp.Width, _rectUp.Height);
            SplashKit.DrawRectangle(Color.Green, _rectDown.X, _rectDown.Y, _rectDown.Width, _rectDown.Height);
            SplashKit.DrawRectangle(Color.Blue, _rectLeft.X, _rectLeft.Y, _rectLeft.Width, _rectLeft.Height);
            SplashKit.DrawRectangle(Color.Yellow, _rectRight.X, _rectRight.Y, _rectRight.Width, _rectRight.Height);

            SplashKit.DrawRectangle(Color.Purple, _hitbox.X, _hitbox.Y, _hitbox.Width, _hitbox.Height);
        }

        public void debugLocation()
        {
            SplashKit.DrawText("X: " + _location.X / 75, Color.Black, _location.X + 10, _location.Y - 30);
            SplashKit.DrawText("Y: " + _location.Y, Color.Black, _location.X + 10, _location.Y - 20);
        }

        // Check collision with platforms


            
        float yVelocity = 0;
        bool moving;
        float gravityVlc = 1;
        bool _movingRight = true;
        public override void Moving(List<Rectangle> rects, Rectangle Mario_hitbox)
        {
            setHitbox();
            debugLocation();

            if (_movingRight)                       //Patrolling behaviour
            {
                if (Collision(rects, _rectRight))
                {
                    _movingRight = false; // Switch direction    
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectRight, -speed, 0);
                    _location.X -= distance;
                }
            }
            else
            {
                if (Collision(rects, _rectLeft))
                {
                    _movingRight = true;  //moves right 
                }
                else
                {
                    double distance = AntiGlitch(rects, _rectLeft, speed, 0);
                    _location.X -= distance;
                }

            }

            if (yVelocity >= 0 && onAir(rects))
            {
                moving = true;
                yVelocity += gravityVlc;            //faster falling speed as the player is falling 

                if (Math.Abs(yVelocity) > 30)
                {
                    yVelocity = yVelocity < 0 ? -30 : 30;             //cap at 50
                }
                double speed = AntiGlitch(rects, _rectDown, 0, yVelocity);
                _location.Y += speed;
                if (speed == 0)     //indicate ground collision
                {
                    yVelocity = 0;      //stops falling
                }
            }





            //Logics check for Goomba collision with Mario
            //Minus one life or game over
            if (_hitbox.IntersectsWith(Mario_hitbox))
            {
                Console.WriteLine("Mario");
            }






            Draw();
        }


        public override void Draw()
        {
            SplashKit.DrawBitmap(_moving, _location.X, _location.Y); 
        }





    }
}
