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
    class Obstacles : DrawableGameComponent
    {
        Ghost ghost;
        Zombie zombie;
        public Obstacles(Game game) : base(game)
        {
        }
        public override void Initialize()
        {
            ghost = new Ghost(Game);
            zombie = new Zombie(Game);
            base.Initialize();
        }

        //Per pixel collision detection 
        static bool PerPixelCollision(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            int top = MathHelper.Max(rect1.Top, rect2.Top);
            int bottom = MathHelper.Min(rect1.Bottom, rect2.Bottom);
            int left = MathHelper.Max(rect1.Left, rect2.Left);
            int right = MathHelper.Min(rect1.Right, rect2.Right);



            for (int row = top; row < bottom; row++)
            {
                for (int col = left; col < right; col++)
                {
                    Color colour1 = data1[(col - rect1.Left) + (row - rect1.Top) * rect1.Width];
                    Color colour2 = data2[(col - rect2.Left) + (row - rect2.Top) * rect2.Width];



                    if (colour1.A != 0 && colour2.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public override void Update(GameTime gameTime)
        //{
        //    if (PerPixelCollision(ghost.ghostRectangle, ghost.ghostData, zombie.zombieRectangle, zombie.zombieData ) == true)
        //    {
        //        ghost.position = Vector2.Zero;
        //    }
        //    base.Update(gameTime);
        //}

        //public virtual Rectangle zombieRectangle { get; set; }
        //{
        //    get
        //    {
        //        Rectangle rect = new Rectangle();
        //        //rect.Location =();
        //        return rect;
        //    }
            
        //}



        //public virtual Rectangle ghostRectangle { get; set; }
        ////{
        ////    get
        ////    {
        ////        Rectangle rect = new Rectangle()
        ////        rect.Location = innerMapPosition.ToPoint();
        ////        return rect;
        ////    }
        ////}


    }
}
