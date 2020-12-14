using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace FinalProject
{
    enum PlayerState
    {
        Idle,
        Jumping,
        Walking
    }
    class Ghost : Obstacles, ICollidable
    {
        public const int WIDTH = 300;
        public const int HEIGHT = 200;
        const float INITIAL_UP_ACCELERATION = -5f;
        const float GRAVITY = .5f;
        const int SPEED = 2;
        const double FRAME_DURATION = 0.1;
        private const int REAL_WIDTH = 100;
        private const int REAL_HEIGHT = 60;
        Dictionary<PlayerState, Texture2D> textures;
        public Color[] ghostData;
        Dictionary<PlayerState, List<Rectangle>> sourceRectangles;

        PlayerState state;
        KeyboardState prevKs;

        public Vector2 position;
        Vector2 velocity;
        float currentUpAcceleration = 0f;

        SpriteEffects spriteEffects;

        int currentFrame;
        double frameTimer;

        bool isGrounded = true;
        bool isJumping = false;

        

        float groundYCoordiante => Game.GraphicsDevice.Viewport.Height - HEIGHT;

        float groundXCoordiante => Game.GraphicsDevice.Viewport.Height - WIDTH;

        int width => textures[state].Width;

        int height => textures[state].Height;

        bool drawBorder;

        public Ghost(Game game) : base(game)
        {
            textures = new Dictionary<PlayerState, Texture2D>();
            sourceRectangles = new Dictionary<PlayerState, List<Rectangle>>();

            state = PlayerState.Idle;

            currentFrame = 0;
            spriteEffects = SpriteEffects.FlipHorizontally;

            velocity = Vector2.Zero;
            drawBorder = false;

            DrawOrder = int.MaxValue - 1;

            
        }

        //public override Rectangle ghostRectangle
        //{
        //    get
        //    {
        //        Rectangle rect = textures[state].Bounds;
        //        rect.Location = position.ToPoint();
        //        return rect;
        //    }
        //}

        public Rectangle CollisionBox => new Rectangle((int)position.X, (int)position.Y, REAL_WIDTH, REAL_HEIGHT);

        protected override void LoadContent()
        {
            textures.Add(PlayerState.Idle, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));
            textures.Add(PlayerState.Jumping, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));
            textures.Add(PlayerState.Walking, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));
            ghostData = new Color[textures[state].Width * textures[state].Height];
            textures[state].GetData(ghostData);

            //position.X = MathHelper.Clamp(position.X, 0, Game.GraphicsDevice.Viewport.Width - WIDTH*2);
            //position.Y = MathHelper.Clamp(position.Y, 0, Game.GraphicsDevice.Viewport.Height - HEIGHT*2);

            sourceRectangles.Add(PlayerState.Idle, new List<Rectangle>());
            sourceRectangles.Add(PlayerState.Jumping, new List<Rectangle>());
            sourceRectangles.Add(PlayerState.Walking, new List<Rectangle>());

            int idleFrameCount = 3;

            for (int i = 0; i < idleFrameCount; i++)
            {
                Rectangle rect = new Rectangle(i * WIDTH, 0, WIDTH, HEIGHT);
                sourceRectangles[PlayerState.Idle].Add(rect);
            }
            for (int i = 0; i < idleFrameCount; i++)
            {
                Rectangle rect = new Rectangle(i * WIDTH, 0, WIDTH, HEIGHT);
                sourceRectangles[PlayerState.Jumping].Add(rect);
            }
            for (int i = 0; i < idleFrameCount; i++)
            {
                Rectangle rect = new Rectangle(i * WIDTH, 0, WIDTH, HEIGHT);
                sourceRectangles[PlayerState.Walking].Add(rect);
            }

            position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - WIDTH / 2,
                                   Game.GraphicsDevice.Viewport.Height / 2 - HEIGHT / 2);


           

            base.LoadContent();
        }
        public override void Initialize()
        {
            position.X = MathHelper.Clamp(position.X, 0, Game.GraphicsDevice.Viewport.Width - WIDTH * 2);
            position.Y = MathHelper.Clamp(position.Y, 0, Game.GraphicsDevice.Viewport.Height - HEIGHT * 2);

            base.Initialize();

        }
        
        public override void Update(GameTime gameTime)
        {
            position.X = MathHelper.Clamp(position.X, 0, Game.GraphicsDevice.Viewport.Width - groundXCoordiante / 1.9f);
            position.Y = MathHelper.Clamp(position.Y, 0, Game.GraphicsDevice.Viewport.Height - groundYCoordiante / 1.5f);
            KeyboardState ks = Keyboard.GetState();
           // spriteEffects = SpriteEffects.FlipHorizontally;
            velocity.Y = SPEED;
            UpdateKeyboard();
            UpdateJumping();

            LookForZombieCollision();
            LookForHandCollision();



            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer >= FRAME_DURATION)
            {
                frameTimer = 0;
                currentFrame++;
            }
            if (currentFrame >= sourceRectangles.Count())
            {
                currentFrame = 0;
            }
            if (isGrounded == false && isJumping == false)
            {
                // BUG: this should happen if we just walk off a platform
                // but happens in other cases too.  
                currentUpAcceleration += GRAVITY;
                velocity.Y = currentUpAcceleration;
            }

            if (position.Y >= Game.GraphicsDevice.Viewport.Height)
            {
                // if we finally made it to the ground, 
                // reset that our ground variables
                position.Y = Game.GraphicsDevice.Viewport.Height;
                isGrounded = true;
                isJumping = false;
            }


            position += velocity;
            velocity = Vector2.Zero;
            base.Update(gameTime);
        }

        private void LookForHandCollision()
        {
            for (int i = 0; i < Game.Components.OfType<Hand>().Count(); i++)
            {
                Hand hand = Game.Components.OfType<Hand>().ElementAt(i);
                //Zombie zombie = new Zombie(Game);
                if (hand.CollisionBox.Intersects(CollisionBox))
                {
                    hand.HandleCollision(this);
                }
            }
        }

        private void LookForZombieCollision()
        {
            for (int i = 0; i < Game.Components.OfType<Zombie>().Count(); i++)
            {
                Zombie zombie = Game.Components.OfType<Zombie>().ElementAt(i);
                //Zombie zombie = new Zombie(Game);
                if (zombie.CollisionBox.Intersects(CollisionBox))
                {
                    zombie.HandleCollision(this);
                    
                }
            }
        }

        private void UpdateKeyboard()
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Space) && prevKs.IsKeyUp(Keys.Space) && isJumping == false && isGrounded == true)
            {
                state = PlayerState.Jumping;
                currentUpAcceleration = INITIAL_UP_ACCELERATION;
                velocity.Y = currentUpAcceleration;
                isJumping = true;
                isGrounded = false;
            }

            //if (ks.IsKeyDown(Keys.B) && prevKs.IsKeyUp(Keys.B))
            //{
            //    drawBorder = !drawBorder;
            //}

            if (ks.IsKeyDown(Keys.Right))
            {
                velocity.X = SPEED;
                if (isJumping == false)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    state = PlayerState.Walking;
                }

            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                velocity.X = -SPEED;
                if (isJumping == false)
                {
                    spriteEffects = SpriteEffects.None;
                    state = PlayerState.Walking;
                }
            }
            else
            {
                velocity.X = 0;
                if (isJumping == false)
                {
                    state = PlayerState.Idle;
                }
                //if (position.Y >= groundYCoordiante/ 1.5f)
                //{
                //    position.Y = groundYCoordiante / 1.5f;
                //    state = PlayerState.Idle;
                //}
            }
            prevKs = ks;
        }
        private void UpdateJumping()
        {                

            if (isJumping && isGrounded == false)
            {
                if (position.Y == groundYCoordiante / 1.5f)
                {
                    currentUpAcceleration = 0;
                    state = PlayerState.Jumping;
                    isJumping = false;
                    isGrounded = true;
                }

       

            }
            if(isGrounded==false && isJumping==true)
            {
                velocity.Y = currentUpAcceleration;
                currentUpAcceleration += GRAVITY;

                if (position.Y >= groundYCoordiante / 1.5f)
                {
                    position.Y = groundYCoordiante / 1.5f;
                    state = PlayerState.Idle;
                    isGrounded = true;
                    isJumping = false;
                }
            }
            
            
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.Draw(textures[state], position, sourceRectangles[state][currentFrame], Color.Pink, 0f, Vector2.Zero, .3f, spriteEffects , 0f);
            sb.End();
            base.Draw(gameTime);
        }

        public void HandleCollision(ICollidable collidable)
        {
            string score = Game.Services.GetService<Score>().score.ToString();
            Game.Components.Remove(this);
            ((Game1)Game).HideAllScenes();
            Game.Services.GetService<EndScene>().Show();
        }
    }
}
