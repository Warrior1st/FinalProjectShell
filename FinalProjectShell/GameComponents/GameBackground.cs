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
    class GameBackground : DrawableGameComponent 
    {
        Texture2D texture;
        Rectangle screenRectangle;
        public GameBackground(Game game) : base(game)
        {
            DrawOrder = 0;
            screenRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.Draw(texture, screenRectangle, Color.White);
            sb.End();
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("images\\gameBackground");
            base.LoadContent();
        }
    }
}
