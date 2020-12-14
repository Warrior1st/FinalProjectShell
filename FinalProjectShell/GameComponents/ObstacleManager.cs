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
    class ObstacleManager : GameComponent
    {
        Random random=new Random();

        double creationTimer = 0;
        double timer = 0;
        double handTimer = 0;
        List<Obstacles> obstacles = new List<Obstacles>();

        public ObstacleManager(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            Zombie obstacle = new Zombie(Game,RandomTimeInterval());
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            creationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            timer = random.Next(2, 90) * random.Next(1,10);
            obstacles.Add(new Hand(Game, handTimer));
            obstacles.Add(new Zombie(Game, timer));
            Random randome = new Random();
            
            if(creationTimer>=timer)
            {
                creationTimer = 0;
                Game.Components.Add(obstacles[randome.Next(0, obstacles.Count - 1)]);
                

                //Game.Components.Add(new Zombie(Game, RandomTimeInterval()));

               
            }

            //else if(handTimer>=timer)
            //{
            //    creationTimer = 0;
            //    Game.Components.Add(new Hand(Game, RandomTimeInterval()));
            //}

            base.Update(gameTime);
        }

        private double RandomTimeInterval()
        {
            timer = random.Next(2, 6);
            handTimer = random.Next(2, 4);
            return timer;
        }
        
        
    }
}
