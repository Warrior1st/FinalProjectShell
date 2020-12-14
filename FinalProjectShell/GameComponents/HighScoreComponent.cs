using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    class HighScoreComponent : DrawableGameComponent
    {
        int highScore = 0;
        string fontValue => $"High Score: {highScore}";
        List<int> listScore = new List<int>();

        SpriteFont font;
       
        public HighScoreComponent(Game game) : base(game)
        {
            using(StreamReader reader = new StreamReader(@"score.txt"))
            {
                while (!reader.EndOfStream)
                {
                    listScore.Add(int.Parse(reader.ReadLine()));
                }
            }
            highScore = listScore.Max();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.DrawString(font, fontValue, Vector2.Zero, Color.Red);
            sb.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("fonts\\scorefont");
            base.LoadContent();
        }
    }
}
