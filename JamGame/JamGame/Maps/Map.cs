﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JamGame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Maps
{
    public class Map
    {
        #region Vars
        private MapStateManager mapStateManager;
        #endregion

        #region Properties
        public string Name
        {
            get;
            private set;
        }
        public MapStateManager StateManager
        {
            get
            {
                return mapStateManager;
            }
        }
        #endregion

        public Map(string name)
        {
            Name = name;
        }

        public void Load()
        {
            MapProcessor mapProcessor = new MapProcessor(@"Maps\MapFiles\" + Name + ".xml");
            mapStateManager = new MapStateManager(mapProcessor.LoadMapStates());

            mapStateManager.Start();
        }

        public void Update(GameTime gameTime)
        {
            mapStateManager.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            mapStateManager.Draw(spriteBatch);
        }
    }
}
