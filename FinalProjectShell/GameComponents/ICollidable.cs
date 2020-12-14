using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public interface ICollidable
    {
        Rectangle CollisionBox { get; }
        void HandleCollision(ICollidable collidable);
    }
}
