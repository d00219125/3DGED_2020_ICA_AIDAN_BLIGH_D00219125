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
    class ThirdPersonBlockyController : Controller
    {
        #region fields
        private KeyboardManager keyboardManager;
        private Camera3D camera3D;
        private float moveSpeed;
        private Keys[][] moveKeys;
        #endregion fields

        #region constructor
        public ThirdPersonBlockyController(string id, ControllerType controllerType, KeyboardManager keyboardManager,
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
            Actor3D blocky = actor as Actor3D;
            if (actor != null) 
            {
                HandleMovement(gameTime, blocky);
                HandleCamera(gameTime, blocky);
            }
            base.Update(gameTime, actor);
        }

        private void HandleMovement(GameTime gameTime, Actor3D actor)
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
        private void HandleCamera(GameTime gameTime, Actor3D actor)
        {
            Vector3 playerPos = actor.Transform3D.Translation;
            playerPos.X += GameConstants.playerCamOffsetX;
            playerPos.Y += GameConstants.playerCamOffsetY;
        }
    }
}
