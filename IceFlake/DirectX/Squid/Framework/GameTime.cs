using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Framework
{
    public class GameTime
    {
        public float ElapsedSeconds { get; private set; }
        public float ElapsedMilliseconds { get; private set; }
        public float TotalSeconds { get; private set; }
        public float TotalMilliseconds { get; private set; }

        public void Update(float delta)
        {
            ElapsedMilliseconds = delta;
            ElapsedSeconds = delta * .001f;

            TotalMilliseconds += ElapsedMilliseconds;
            TotalSeconds += ElapsedSeconds;
        }
    }
}
