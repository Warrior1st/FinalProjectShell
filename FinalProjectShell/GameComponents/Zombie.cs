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
    class Zombie : Obstacles, ICollidable
    {
        private const int ZOMBIE_GROUND = 100;
        private const int ZOMBIE_SPEED = 2;
        //Dictionary<PlayerState, Texture2D> textures;
        //Dictionary<PlayerState, List<Rectangle>> sourceRectangles;
        public Texture2D textureObstacle;
        public Color[] zombieData;
        Vector2 obstaclePosition;
        Rectangle rectangle;

        //List<Texture2D> listObstacles = new List<Texture2D>();
        
        float positionFromGround = 0f;
        public Rectangle CollisionBox => new Rectangle((int)obstaclePosition.X, (int)obstaclePosition.Y, textureObstacle.Width, textureObstacle.Height);

        SoundEffect sfx;
        bool isPlaying;
        public Zombie(Game game) : base(game)
        {
            DrawOrder = int.MaxValue - 2;

        }

        public Zombie(Game game,double time) : base(game)
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
            obstaclePosition.X -= ZOMBIE_SPEED;
            //if (PerPixelCollision(gho))
            //{

            //}
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {

            textureObstacle = Game.Content.Load<Texture2D>("images\\zombie");
            //textureObstacle.GetData(zombieData);
            positionFromGround = (GraphicsDevice.Viewport.Height - textureObstacle.Height- ZOMBIE_GROUND);
            //obstaclePosition = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            obstaclePosition = new Vector2(Game.GraphicsDevice.Viewport.Width - textureObstacle.Width, positionFromGround);
            rectangle = new Rectangle(0, 0, textureObstacle.Width, textureObstacle.Height);

            sfx = Game.Content.Load<SoundEffect>("sounds\\zombie_sound");
            base.LoadContent();
        }
        

        public void HandleCollision(ICollidable collidable)
        {
            if (isPlaying == false) {
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
        //public override Rectangle zombieRectangle {
        //    get
        //    {
        //        Rectangle rect = textureObstacle.Bounds;
        //        rect.Location = obstaclePosition.ToPoint();
        //        return rect;
        //    }
        //}

    }
}
