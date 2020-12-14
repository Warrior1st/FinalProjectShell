using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FinalProject
{
    class Hand : Obstacles, ICollidable
    {
        private const int HAND_GROUND = 100;
        private const int HAND_SPEED = 2;
        Texture2D textureObstacle;
        Vector2 obstaclePosition;
        Rectangle rectangle;

        float positionFromGround = 0f;

        SoundEffect sfx;
        bool isPlaying = false;


        public Rectangle CollisionBox => new Rectangle((int)obstaclePosition.X, (int)obstaclePosition.Y, textureObstacle.Width, textureObstacle.Height);

        public Hand(Game game) : base(game)
        {
            DrawOrder = int.MaxValue - 2;

        }

        public Hand(Game game, double time) : base(game)
        {


        }


        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            //sb.Draw(textureObstacle, obstaclePosition, Color.White);
            sb.Draw(textureObstacle, obstaclePosition, rectangle, Color.Pink);
            sb.End();
            base.Draw(gameTime);
        }



        public override void Update(GameTime gameTime)
        {
            obstaclePosition.X -= HAND_SPEED;
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {

            textureObstacle = Game.Content.Load<Texture2D>("images\\hand2");
            positionFromGround = (GraphicsDevice.Viewport.Height - textureObstacle.Height - HAND_GROUND);
            //obstaclePosition = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            obstaclePosition = new Vector2(Game.GraphicsDevice.Viewport.Width - textureObstacle.Width, positionFromGround);
            rectangle = new Rectangle(0, 0, textureObstacle.Width, textureObstacle.Height);

            sfx = Game.Content.Load<SoundEffect>("sounds\\zombie_sound");
            base.LoadContent();
        }

        public void HandleCollision(ICollidable collidable)
        {
            if (isPlaying == false)
            {
                sfx.Play(.5f, 0, 0);
                isPlaying = true;
            }
            string score = Game.Services.GetService<Score>().score.ToString();
            Game.Components.Remove(this);
            ((Game1)Game).HideAllScenes();
            Game.Services.GetService<EndScene>().Show();

            MediaPlayer.Stop();
            using (StreamWriter sr = new StreamWriter(@"score.txt", true))
            {
                sr.WriteLine(score);

            }
        }
    }
}
