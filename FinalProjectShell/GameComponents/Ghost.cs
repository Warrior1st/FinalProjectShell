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
    class Ghost : DrawableGameComponent
    {
        Texture2D texture;
        Vector2 position;
        public Ghost(Game game) : base(game)
        {
        }
    }
}
