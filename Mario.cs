using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;


namespace CustomMario
{
    public class Mario
    {
        Bitmap mIdle, mJump, mMoving, mDuck, currentPose;
        private Sprite _player;
        private const int Xspeed = 10;
        private Point2D _location;
        bool _facingRight = true;
        bool _loaded;
        List<Rectangle> rects;
        public Mario(double x, double y)
        {
            //Movements
            mDuck = SplashKit.LoadBitmap("Duck", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Duckin.png");
            mIdle = SplashKit.LoadBitmap("Idle", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Idlingfinal.png");
            mJump = SplashKit.LoadBitmap("Jump", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Jumpinfinal.png");
            mMoving = SplashKit.LoadBitmap("Mario", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Walk.png");



            //animation script
            mMoving.SetCellDetails(mMoving.Width / 2, mMoving.Height / 2, 2, 2, 4);
            AnimationScript walkingScript = SplashKit.LoadAnimationScript("WalkingScript", "walkingScript.txt");
            _player = SplashKit.CreateSprite(mMoving, walkingScript);

            _player.StartAnimation(0);
            currentPose = mIdle;

            _location.X = x;
            _location.Y = y;

            _loaded = true;
            Console.WriteLine("Mario loaded successfully!");

        }

        //public void Draw()
        //{
        //    _player.Draw(100, 500);
        //}


        public Point2D getLocation()
        {
            return _location;
        }






        //Hitboxes
        private Rectangle _rectUp;
        private Rectangle _rectDown;
        private Rectangle _rectLeft;
        private Rectangle _rectRight;
        private Rectangle _hitbox;
                
        public void setHitbox()         //sets the 4 small 1x1 rectangle hitboxes indicating top, bottom, left, right of the character.
        {
                                          //int x, int y, int width, int height
                                          //character bitmap is 80x86

            _rectUp = new Rectangle();          
            _rectDown = new Rectangle(Convert.ToInt32(_location.X) + 40, Convert.ToInt32(_location.Y) + 89, 1, 1);
            _rectLeft = new Rectangle();
            _rectRight = new Rectangle();

            _hitbox = new Rectangle();

            //should be handled after genmap.cs
        }


        //onAir
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




        // Check collision with platforms
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
        public Rectangle Rect()
        {
            return _hitbox;
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



        //====Gravity data====
        private float gravityVlc = 1;      

        private float yVelocity = 0;        //represents the player's vertical velocity while airborne
        private float xVelocity = 0;        //represents the horizontal velocity while airborne

        private bool airRight = true;       //faces right while mid-air
        private bool onGround = false;     //false by default 

        bool moving ;

        public void HandleInput(List<Rectangle> rects)
        {
            moving = false;
            setHitbox();
            movingLoop();
            //Idle when not jump, not duck, not moving A D
            //Show running animation when A D






            //if (!SplashKit.KeyDown(KeyCode.WKey) && !(SplashKit.KeyDown(KeyCode.SKey)) )
            //{

            //}  


            //if (SplashKit.KeyDown(KeyCode.WKey) && (SplashKit.KeyDown(KeyCode.SKey)))
            //{
            //    Console.WriteLine("Drawing idle");
            //    Draw(currentPose, _facingRight);
            //}
            //else
            //{
            //    if (SplashKit.KeyDown(KeyCode.AKey)) { _player.StartAnimation("WalkLeft"); }
            //    if (SplashKit.KeyDown(KeyCode.DKey)) { _player.StartAnimation("WalkRight"); }
            //}



            //set current pose for character 

            if (!moving && !onAir(rects))       //if not moving and not mid-air then idle pose
            {
                currentPose = mIdle;        
            }


            if (onAir(rects) && xVelocity > 0)      //represents the player's horizontal velocity while airborne
            {
                moving = true;
                if (airRight)   //if character is facing right
                {
                    double distance = AntiGlitch(rects, _rectRight, -xVelocity, 0);     //calculates safe distance before colliding with terrains
                    _location.X += distance;
                    if (distance == 0) { xVelocity = 0;}           //indicates ground collision
                }   
                else  //faces left
                {
                    double distance = AntiGlitch(rects, _rectLeft, -xVelocity, 0);           //calculates safe distance before colliding with terrains
                    _location.X += distance;
                    if (distance == 0) { xVelocity = 0;}        //indicates ground collision
                }
            }

            if (!onAir(rects))      //if on ground then xVelocity = 0. Note: differentiate between xVelocity and Xspeed, one is for moving mid-air and one is for moving on ground.
            {
                xVelocity = 0;
            }


            //Jumping
            if (SplashKit.KeyDown(KeyCode.WKey) && !onAir(rects) && !Collision(rects, _rectUp) )        //if press W, is on ground, and doesn't collide with ceiling, then Jump
            {
                yVelocity -= 25;
                currentPose = mJump;
            }

            //jumping up
            if (yVelocity < 0)      //negative indicates going up
            {
                yVelocity += gravityVlc;     //incrementing the Y velocity to ensure the player slow down as they reach the peak of their jump.
                if (Math.Abs(yVelocity) > 30)
                {
                    yVelocity = yVelocity < 0 ? -30 : 30;           //cap the maximum falling/jumping velocity to 50.
                }
                double speed = AntiGlitch(rects, _rectUp, 0, yVelocity);        //calculates safe distance
                _location.Y += speed;
                if (speed == 0)     //speed = 0 indicate collision with ceiling
                {
                    yVelocity = 0;      //stops going up immediately
                }
            }
            //falling down
            if (yVelocity >= 0 && onAir(rects))
            {
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


            //Moving left and right
            if (SplashKit.KeyDown(KeyCode.DKey) && !Collision(rects,_rectRight) )       //if press D and no collision on right hitbox
            {
                moving = true;
                _facingRight = true;
                if (!onAir(rects)) { _player.StartAnimation("WalkRight"); _player.UpdateAnimation(); }    //since jumping will display the jump bitmap.
                double distance = AntiGlitch(rects, _rectRight, Xspeed, 0);
                _location.X += distance;
            }
            else if(SplashKit.KeyDown(KeyCode.AKey) &&!Collision(rects, _rectLeft))
            {
                moving = true;
                _facingRight = false;
                if (!onAir(rects)) { _player.StartAnimation("WalkLeft"); _player.UpdateAnimation(); }
                double distance = AntiGlitch(rects,_rectLeft, -Xspeed, 0);
                _location.X += distance;
            }

           

            //Duck







            ////Animation update
            //if (SplashKit.KeyTyped(KeyCode.DKey))
            //{
            //    _player.StartAnimation("WalkRight");        //Right
            //}
            //if (SplashKit.KeyTyped(KeyCode.AKey))
            //{
            //    _player.StartAnimation("WalkLeft");         //Left
            //}
            //if (SplashKit.KeyDown(KeyCode.SKey))
            //{
            //    currentPose = mDuck;            //Duck
            //}

            ////Speed
            //if (SplashKit.KeyDown(KeyCode.DKey))
            //{
            //    _player.X += XSpeed;
            //}
            //if (SplashKit.KeyDown(KeyCode.AKey))
            //{
            //    _player.X -= XSpeed;
            //}

            //if (SplashKit.KeyTyped(KeyCode.WKey) && onGround)         //Jump
            //{
            //    yVelocity = jumpSpeed;
            //    onGround = false;
            //    currentPose = mJump;
            //}
        }


        public void movingLoop()
        {
            if (SplashKit.KeyTyped(KeyCode.AKey)) { _player.StartAnimation("WalkLeft"); }
            if (SplashKit.KeyTyped(KeyCode.DKey)) { _player.StartAnimation("WalkRight"); }
            else
            {
                Draw(currentPose, _facingRight);
            }
        }







        public void Draw(Bitmap mario, bool facingRight)
        {
            if (facingRight)
            {
                SplashKit.DrawBitmap(mario, _location.X, _location.Y);

            }
            else
            {
                SplashKit.DrawBitmap(mario, _location.X, _location.Y, SplashKit.OptionFlipY());
            }
        }
    }
}