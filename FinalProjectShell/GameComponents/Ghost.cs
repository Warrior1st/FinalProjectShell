using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    enum PlayerState
    {
        Idle,
        Jumping,
        Walking
    }
    class Ghost : DrawableGameComponent
    {
        public const int WIDTH = 300;
        public const int HEIGHT = 200;
        const float INITIAL_UP_ACCELERATION = -15f;
        const float GRAVITY = 0.5f;
        const int SPEED = 3;
        const double FRAME_DURATION = 0.1;

        Dictionary<PlayerState, Texture2D> textures;
        Dictionary<PlayerState, List<Rectangle>> sourceRectangles;

        PlayerState state;
        KeyboardState prevKs;

        Vector2 position;
        Vector2 velocity;
        float currentUpAcceleration = 0f;

        SpriteEffects spriteEffects;

        int currentFrame;
        double frameTimer;

        bool isGrounded = true;
        bool isJumping = false;

        //float groundYCoordiante => Game.GraphicsDevice.Viewport.Height - textures[state].Height;

        //int width => textures[state].Width;

        //int height => textures[state].Height;

        bool drawBorder;

        public Ghost(Game game) : base(game)
        {
            textures = new Dictionary<PlayerState, Texture2D>();
            sourceRectangles = new Dictionary<PlayerState, List<Rectangle>>();

            state = PlayerState.Idle;

            currentFrame = 0;
            spriteEffects = SpriteEffects.None;

            velocity = Vector2.Zero;
            drawBorder = false;

            DrawOrder = int.MaxValue - 1;
        }

        protected override void LoadContent()
        {
            textures.Add(PlayerState.Idle, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));
            textures.Add(PlayerState.Jumping, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));
            textures.Add(PlayerState.Walking, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));

            position.X = MathHelper.Clamp(position.X, 0, Game.GraphicsDevice.Viewport.Width - textures[state].Width);
            position.Y = MathHelper.Clamp(position.Y, 0, Game.GraphicsDevice.Viewport.Height - textures[state].Height);

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
            //position.X = MathHelper.Clamp(position.X, 0, Game.GraphicsDevice.Viewport.Width - textures[state].Width);
            //position.Y = MathHelper.Clamp(position.Y, 0, Game.GraphicsDevice.Viewport.Height - textures[state].Height);

            base.Initialize();
            
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            spriteEffects = SpriteEffects.None;
            velocity.Y = SPEED;
            UpdateKeyboard();
            UpdateJumping();



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

            if (ks.IsKeyDown(Keys.B) && prevKs.IsKeyUp(Keys.B))
            {
                drawBorder = !drawBorder;
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                velocity.X = SPEED;
                spriteEffects = SpriteEffects.FlipHorizontally;
                if (isJumping == false)
                {
                    state = PlayerState.Walking;
                }

            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                velocity.X = -SPEED;
                if (isJumping == false)
                {
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
            }
            prevKs = ks;
        }
        private void UpdateJumping()
        {
            if (isJumping && isGrounded == false)
            {
                velocity.Y = currentUpAcceleration;
                currentUpAcceleration += GRAVITY;

            }
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.Draw(textures[state], position, sourceRectangles[state][currentFrame], Color.White, 0f, Vector2.Zero, .3f, spriteEffects, 0f);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
