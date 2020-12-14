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
    class EndGameComponent : DrawableGameComponent
    {
        Texture2D texture;
        Rectangle screenRectangle;

        Vector2 position;
        public EndGameComponent(Game game) : base(game)
        {
            screenRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            position = Vector2.Zero;
        }
        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("images\\gameOver");

            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.Draw(texture, position, Color.Pink);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
