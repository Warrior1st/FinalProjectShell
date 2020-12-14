using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FinalProject
{
    class GameBackground : DrawableGameComponent 
    {
        Texture2D texture;
        Rectangle screenRectangle;
        /*public*/ Song backGroundMusic;

        Vector2 position;
        public GameBackground(Game game) : base(game)
        {
            DrawOrder = 0;
            screenRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            position = new Vector2(0, 0);
            //Game.Services.AddService<GameBackground>(this);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.Draw(texture, position, Color.White);
            if(position.X<-Game.GraphicsDevice.Viewport.Width)
            {
                position = Vector2.Zero;
                sb.Draw(texture,position, Color.White);
            }
            sb.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            position.X -= 1;
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("images\\gameBackground");
            backGroundMusic = Game.Content.Load<Song>("sounds\\backSound");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = .5f;
            MediaPlayer.Play(backGroundMusic);


            base.LoadContent();
        }
    }
}
