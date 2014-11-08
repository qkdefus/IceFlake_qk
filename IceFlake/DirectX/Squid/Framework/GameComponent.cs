using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public abstract class GameComponent
    {
        public Game Game { get; private set; }
        public GameComponent(Game game) { Game = game; }
        //public virtual void Update(GameTime time) { }
        public virtual void Update() { }

        public virtual void Load() { }
        public virtual void Unload() { }
    }

    public abstract class DrawableGameComponent : GameComponent
    {
        public DrawableGameComponent(Game game) : base(game) { }
        //public virtual void Draw(GameTime time) { }
        public virtual void Draw() { }

    }
}
