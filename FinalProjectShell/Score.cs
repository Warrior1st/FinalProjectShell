using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace FinalProject
{
    class Score : DrawableGameComponent
    {
        SpriteFont font;
        Vector2 position;
        public int score = 0;



        string scoreText => $"Score: {score}";



        public Score(Game game) : base(game)
        {
            position = Vector2.Zero;
            if (Game.Services.GetService<Score>() != null)
            {
                Game.Services.RemoveService(typeof(Score));
            }
            DrawOrder = int.MaxValue -3;
            Game.Services.AddService<Score>(this);
        }



        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("fonts\\scoreFont");
            base.LoadContent();
        }



        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            sb.Begin();
            sb.DrawString(font, scoreText, position, Color.Red);
            sb.End();
            base.Draw(gameTime);
        }



        public void AddScore(int newScoreValue)
        {
            if (newScoreValue > 0)
            {
                score += newScoreValue;
            }
        }

        public override void Update(GameTime gameTime)
        {
            score++;
            base.Update(gameTime);
        }
    }
}