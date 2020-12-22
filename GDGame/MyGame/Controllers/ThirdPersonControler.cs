using System;
using System.Collections.Generic;
using System.Text;
using GDLibrary.Actors;
using GDLibrary.Controllers;
using GDLibrary.Enums;
using GDLibrary.Interfaces;
using GDLibrary.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDGame.MyGame.Controllers
{
    class ThirdPersonControler : Controller
    {
        #region fields
        private KeyboardManager keyboardManager;
        private Camera3D camera3D;
        private float moveSpeed;
        private Keys[][] moveKeys;
        #endregion fields

        #region constructor
        public ThirdPersonControler(string id, ControllerType controllerType, KeyboardManager keyboardManager,
            Camera3D camera3D, float movespeed, Keys[][] moveKeys) : base(id, controllerType) 
        {
            this.keyboardManager = keyboardManager;
            this.camera3D = camera3D;
            this.moveSpeed = movespeed;
            this.moveKeys = moveKeys;
            //need to make updatable by event or...
        }
        #endregion constructor

        public override void Update(GameTime gameTime, IActor actor)
        {
            if (actor != null) 
            {
                HandleMovement(gameTime, actor);
                HandleCamera(gameTime, actor);
            }
            base.Update(gameTime, actor);
        }

        private void HandleMovement(GameTime gameTime, IActor actor)
        {
            //think this might have to be collidable primitive object
            //PrimitiveObject character = actor as PrimitiveObject;
            CollidablePrimitiveObject character = actor as CollidablePrimitiveObject;
            Vector3 moveVector = Vector3.Zero;
            //blocky constantly moves foreward
            moveVector.Z += moveSpeed;
            //strafe left
            if (keyboardManager.IsKeyDown( Keys.A)){
                moveVector.X -= moveSpeed;
            }
            //strafe right
            else if (keyboardManager.IsKeyDown(Keys.D))
            {
                moveVector.X += moveSpeed;
            }
            character.Transform3D.TranslateBy(moveVector);
            //to do... check for collisions and do event stuff here
        }
        private void HandleCamera(GameTime gameTime, IActor actor)
        {
            Vector3 playerPos = actor.Tra
        }
    }
}
