using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using CustomMario.Map;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;


namespace CustomMario
{
    public class Mario
    {
        Bitmap mIdle, mJump, mMoving, mDuck, mDie, mVictory,currentPose;
        //private Sprite _player;
        private const int Xspeed = 20;
        private Point2D _location;
        bool _facingRight = true;
    
        List<Rectangle> rects;
        SoundEffect _jumpUp;
        SoundEffect coinCollect;
        SoundEffect enemyDmg;
        SoundEffect up1;

        public Mario(double x, double y)
        {
            //Movements
            mDuck = SplashKit.LoadBitmap("Duck", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Duckin4.png");          //64x86
            mIdle = SplashKit.LoadBitmap("Idle", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Idlingfinal.png");     //77x86
            mJump = SplashKit.LoadBitmap("Jump", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Jumpinfinal4.png");   //85x92
            mMoving = SplashKit.LoadBitmap("Mario", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\Walk9.png");      //64x82
            mVictory = SplashKit.LoadBitmap("victoryPose", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\victory4.png");    //65x86
            mDie = SplashKit.LoadBitmap("diePose", "F:\\Projects\\repo\\CustomMario\\Resources\\images\\die2.png");     //58x87


            //SFX
            _jumpUp = SplashKit.LoadSoundEffect("SFXup", "F:\\Projects\\repo\\CustomMario\\Resources\\SFX\\SFXJump.mp3");

            ////animation script
            //mMoving.SetCellDetails(mMoving.Width / 2, mMoving.Height / 2, 2, 2, 4);
            //AnimationScript walkingScript = SplashKit.LoadAnimationScript("WalkingScript", "walkingScript.txt");
            //_player = SplashKit.CreateSprite(mMoving, walkingScript);


            currentPose = mIdle;
            //_player.StartAnimation(0);
            _location.X = x;
            _location.Y = y;


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

        public void debugLocation()
        {
            SplashKit.DrawText("X: " + _location.X/75, Color.Black, _location.X +10, _location.Y -30);
            SplashKit.DrawText("Y: " + _location.Y, Color.Black, _location.X +10, _location.Y -20);
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
                                          
            _rectUp = new Rectangle(Convert.ToInt32(_location.X) + 20, Convert.ToInt32(_location.Y) + 4, 36, 2);
            _rectDown = new Rectangle(Convert.ToInt32(_location.X) + 20, Convert.ToInt32(_location.Y) + 86, 36, 1);
            _rectLeft = new Rectangle(Convert.ToInt32(_location.X) + 5, Convert.ToInt32(_location.Y) + 5, 1, 81);
            _rectRight = new Rectangle(Convert.ToInt32(_location.X) + 65, Convert.ToInt32(_location.Y) + 5, 1, 81);
            //should be handled after genmap.cs

            _hitbox = new Rectangle(Convert.ToInt32(_location.X) + 10, Convert.ToInt32(_location.Y) + 4, 52, 80);

            SplashKit.DrawRectangle(Color.Red, _rectUp.X, _rectUp.Y, _rectUp.Width, _rectUp.Height);
            SplashKit.DrawRectangle(Color.Green, _rectDown.X, _rectDown.Y, _rectDown.Width, _rectDown.Height);
            SplashKit.DrawRectangle(Color.Blue, _rectLeft.X, _rectLeft.Y, _rectLeft.Width, _rectLeft.Height);
            SplashKit.DrawRectangle(Color.Yellow, _rectRight.X, _rectRight.Y, _rectRight.Width, _rectRight.Height);
            SplashKit.DrawRectangle(Color.Purple, _hitbox.X, _hitbox.Y, _hitbox.Width, _hitbox.Height);
        }
        public Rectangle getHitbox()
        {
            return _hitbox;
        }

        public Rectangle getRectdown()
        {
            return _rectDown;
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




        //Check collisions with active blocks (like question blocks)
        public Boolean collideActiveBlocks(List<Rectangle> rects, Rectangle RECT)     //rects is the list<> rects of the map, RECT is the directional hitbox of the character.
        {

            foreach (Rectangle rect in rects)
            {

                if (RECT.IntersectsWith(rect) && rect is QuestionBlock)
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



        //====Gravity data====
        private float gravityVlc = 1;      

        private float yVelocity = 0;        //represents the player's vertical velocity while airborne
        private float xVelocity = 0;        //represents the horizontal velocity while airborne

        private bool airRight = true;       //faces right while mid-air
        private bool onGround = false;     //false by default 

        bool moving ;
        bool ducked;
        public void HandleInput(List<Rectangle> rects)
        {
            ducked = false;
            moving = false;
            setHitbox();
            debugLocation();
            //Idle when not jump, not duck, not moving A D
            //Show running animation when A D


            //if (!SplashKit.KeyDown(KeyCode.WKey) && !(SplashKit.KeyDown(KeyCode.SKey)) )
            //{

            //}  

            if (collideActiveBlocks(rects, _rectUp))
            {
                Console.WriteLine("collision with an active block detected.");
            }


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

            if (!moving && !onAir(rects) && !ducked)       //if not ducking, moving and not mid-air then idle pose
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


            //Moving left and right
            if (!ducked)
            {
                if (SplashKit.KeyDown(KeyCode.DKey) && !Collision(rects, _rectRight))       //if press D and no collision on right hitbox
                {
                    moving = true;
                    _facingRight = true;
                    if (!onAir(rects)) { currentPose = mMoving; }    //since jumping will display the jump bitmap.
                    double distance = AntiGlitch(rects, _rectRight, Xspeed, 0);
                    _location.X += distance;
                }
                else if (SplashKit.KeyDown(KeyCode.AKey) && !Collision(rects, _rectLeft))
                {
                    moving = true;
                    _facingRight = false;
                    if (!onAir(rects)) { currentPose = mMoving; }
                    double distance = AntiGlitch(rects, _rectLeft, -Xspeed, 0);
                    _location.X += distance;
                }
            }

            //Jumping
            if (SplashKit.KeyDown(KeyCode.WKey) && !onAir(rects) && !Collision(rects, _rectUp) && !ducked)        //if press W, is on ground, not ducking and doesn't collide with ceiling, then Jump
            {
                SplashKit.PlaySoundEffect(_jumpUp);
                moving = true;
                yVelocity -= 25;
                currentPose = mJump;
            }

            //jumping up
            if (yVelocity < 0)      //negative indicates going up
            {
                moving = true;
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



            //Duck
            if (SplashKit.KeyDown(KeyCode.SKey) && !onAir(rects) && !moving)           //if on ground and press S
            {
                ducked = true;
                currentPose = mDuck;
            }


            //Falling off the map
            if (_location.Y > 700)
            {
                    Console.WriteLine("Mario has fallen off the map!");
                    _location.X = 50;
                    _location.Y = 50;
                
            }

            if (_location.X > 13500 && _location.X < 13725 && _location.Y > 500)
            {
                currentPose = mVictory;

            }







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
                Draw(currentPose, _facingRight);
        }

        //Victory state

        public void Winning()
        {
            currentPose = mVictory;


          

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