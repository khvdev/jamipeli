﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using JamGame.Components;
using JamGame.GameObjects;
using JamGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JamGame.Maps;
using JamGame.GameObjects.Components;
using JamGame.DataTypes;
using JamGame.Weapons;
using JamGame.GameObjects.Monsters;

namespace JamGame.Entities
{
    public enum Facing
    {
        Left,
        Right
    }

    public class Player : DrawableGameObject
    {
        #region Vars
        private InputControlSetup defaultSetup;
        private InputController controller;
        private DirectionalArrow directionalArrow;
        private WeaponComponent weaponComponent;
        private TargetingComponent<Monster> targetingComponent;
        private Facing facing;

        private float speed = 15f;
        #endregion

        public Player(World world)
        {
            Size = new Size(100, 100);

            defaultSetup = new InputControlSetup();
            controller = new InputController(Game.Instance.InputManager);
            controller.ChangeSetup(defaultSetup);

            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(100), 1.0f);
            //body.Mass = 0.1f;
            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0f;
            body.LinearDamping = 5f; 
            body.UserData = this;
            Position = Vector2.Zero;

            components.Add(targetingComponent = new TargetingComponent<Monster>(this));
            components.Add(directionalArrow = new DirectionalArrow());
            components.Add(weaponComponent = new WeaponComponent(targetingComponent, new BaseballBat()));
            components.Add(new HealthComponent(100));

            Game.Instance.MapManager.OnMapChanged += new MapManagerEventHandler(MapManager_OnMapChanged);

            facing = Facing.Right;

            Initialize();
        }

        #region Event handlers
        private void MapManager_OnMapChanged(object sender, MapManagerEventArgs e)
        {
            Game.Instance.MapManager.Active.StateManager.OnStateFinished += new MapStateManagerEventHandle(StateManager_OnStateFinished);
            Game.Instance.MapManager.Active.StateManager.OnTransitionStart += new MapStateManagerEventHandle(StateManager_OnTransitionStart);
        }
        private void StateManager_OnTransitionStart(object sender, MapStateManagerEventArgs e)
        {
            directionalArrow.Enabled = false;
        }
        public void StateManager_OnStateFinished(object sender, MapStateManagerEventArgs e)
        {
            directionalArrow.Enabled = true;
        }
        #endregion

        public void Initialize()
        {
            InitKeyMaps();
            InitPadMaps();
        }

        private void InitKeyMaps()
        {
            var keymapper = defaultSetup.Mapper.GetInputBindProvider<KeyInputBindProvider>();
            keymapper.Map(new KeyTrigger("move left", Keys.A),
                (triggered, args) => 
                {
                    body.ApplyForce(new Vector2(-speed, 0));
                    facing = Facing.Left;
                });
            keymapper.Map(new KeyTrigger("move right", Keys.D),
                (triggered, args) =>
                {
                    body.ApplyForce(new Vector2(speed, 0));
                    facing = Facing.Right;
                });
            keymapper.Map(new KeyTrigger("move up", Keys.W),
                (triggered, args) => body.ApplyForce(new Vector2(0, -speed)));
            keymapper.Map(new KeyTrigger("move down", Keys.S),
                (triggered, args) => body.ApplyForce(new Vector2(0, speed)));
            keymapper.Map(new KeyTrigger("attck", Keys.Space),
                (triggered, args) =>
                {
                    weaponComponent.Attack();
                });
        }

        private GameObject NearestObject()
        {
            Game.Instance.GameObjects
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Game.Instance.Temp, new Rectangle((int) Position.X,
                (int) Position.Y, 100, 100), null, Color.Black, 0f, new Vector2(0.5f, 0.5f),SpriteEffects.None,0f );
        }

        private void InitPadMaps()
        {
            var padmapper = defaultSetup.Mapper.GetInputBindProvider<PadInputBindProvider>();
            padmapper.Map(new ButtonTrigger("move left", Buttons.LeftThumbstickLeft), (triggered, args) => body.ApplyForce(new Vector2(-speed, 0)));
            padmapper.Map(new ButtonTrigger("move right", Buttons.LeftThumbstickRight), (triggered, args) => body.ApplyForce(new Vector2(speed, 0)));
            padmapper.Map(new ButtonTrigger("move up", Buttons.LeftThumbstickUp), (triggered, args) => body.ApplyForce(new Vector2(0, -speed)));
            padmapper.Map(new ButtonTrigger("move down", Buttons.LeftThumbstickDown), (triggered, args) => body.ApplyForce(new Vector2(0, speed)));

        }
    }
}
