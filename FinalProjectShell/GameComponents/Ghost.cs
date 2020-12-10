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

        Vector2 position;
        Vector2 velocity;

        SpriteEffects spriteEffects;

        int currentFrame;
        double frameTimer;

        bool isGrounded = true;
        bool isJumping = false;
        
        public Ghost(Game game) : base(game)
        {
            textures = new Dictionary<PlayerState, Texture2D>();
            sourceRectangles = new Dictionary<PlayerState, List<Rectangle>>();

            state = PlayerState.Idle;

            currentFrame = 0;
            spriteEffects = SpriteEffects.None;

            velocity = Vector2.Zero;

            DrawOrder = int.MaxValue - 1;
        }

        protected override void LoadContent()
        {
            textures.Add(PlayerState.Idle, Game.Content.Load<Texture2D>("images\\enemy-sheet0"));

            sourceRectangles.Add(PlayerState.Idle, new List<Rectangle>());

            int idleFrameCount = 3;

            for (int i = 0; i < idleFrameCount; i++)
            {
                Rectangle rect = new Rectangle(i * WIDTH, 0, WIDTH, HEIGHT);
                sourceRectangles[PlayerState.Idle].Add(rect);
            }

            position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - WIDTH / 2,
                                   Game.GraphicsDevice.Viewport.Height / 2 - HEIGHT / 2);


            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            spriteEffects = SpriteEffects.None;
            velocity.Y = SPEED;

            if (ks.IsKeyDown(Keys.Right))
            {
                position.X += SPEED;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= SPEED;

            }
            if (position.Y == Game.GraphicsDevice.Viewport.Height)
            {
                position.Y = 0;
            }
            else
            {
                position.Y += 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (ks.IsKeyDown(Keys.Space))
            {

                position.Y -= 5 * 4;

            }
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
                if (isJumping == false)
                {
                    state = PlayerState.Walking;
                }

            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                velocity.X = -SPEED;
                effects = SpriteEffects.FlipHorizontally;
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
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.Draw(textures[state], position, sourceRectangles[state][currentFrame], Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
