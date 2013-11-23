﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamGame.Weapons
{
    // TODO: testi luokka - Weapon
    public abstract class Weapon
    {
        #region Vars
        private readonly Random random;

        private readonly string name;
        private readonly int minDamage;
        private readonly int maxDamage;
        private readonly int critChance;
        #endregion

        #region Properties
        public bool IsDrawing
        {
            get;
            protected set;
        }
        public int CritModifier
        {
            get;
            private set;
        }
        #endregion

        public Weapon(string name, int minDamage, int maxDamage, int critChance)
        {
            this.name = name;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.critChance = critChance;

            CritModifier = 2;

            random = new Random();
        }

        public abstract bool CanMakeDamage();

        public virtual int CalculateDamage()
        {
            int result = 0;

            if (CanMakeDamage())
            {
                int damageModifier = (random.Next(0, 100) <= critChance ? CritModifier : 1);

                result = random.Next(minDamage, maxDamage) * damageModifier;
            }

            return result;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void DrawEffects(SpriteBatch spriteBatch, Vector2 position);
    }
}
