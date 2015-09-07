using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace AncientHorrorClient
{
    public class MainApp : WaveEngine.Adapter.WPFApplication
    {
        /// <summary>
        /// The game
        /// </summary>
        private WEAncientHorrorProject.Game game;

        /// <summary>
        /// Gets the wave game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public WEAncientHorrorProject.Game Game
        {
            get
            {
                return this.game;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainApp"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public MainApp(int width, int height)
            : base(width, height)
        {
        }

        /// <summary>
        /// Perform further custom initialize for this instance.
        /// </summary>
        /// <exception cref="System.InvalidProgramException">License terms not agreed.</exception>
        public override void Initialize()
        {
            this.game = new WEAncientHorrorProject.Game();
            this.game.Initialize(this);
        }

        /// <summary>
        /// Perform further custom update for this instance.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed)
                {
                    this.FullScreen = !this.FullScreen;
                }

                this.game.UpdateFrame(elapsedTime);
            }
        }

        /// <summary>
        /// Perform further custom draw for this instance.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time from the last draw.</param>
        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                this.game.DrawFrame(elapsedTime);
            }
        }
    }
}