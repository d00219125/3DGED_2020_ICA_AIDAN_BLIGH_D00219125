using GDGame;
using GDLibrary.Actors;
using GDLibrary.Enums;
using GDLibrary.Events;
using GDLibrary.Interfaces;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace GDLibrary.MyGame
{
    /// <summary>
    /// Moveable, collidable player using keyboard and checks for collisions
    /// </summary>
    public class CollidablePlayerObject : CollidablePrimitiveObject
    {
        #region Fields
        private float moveSpeed, strafeSpeed, groundPos, jumpHeight;
        bool IsJumpingUp, isOnLevel1, isPlayerPaused, coolingDown;
        private KeyboardManager keyboardManager;
        private Keys[] moveKeys;
        double restTime;
        int lives;
        CameraManager<Camera3D> cameraManager;
        #endregion Fields

        public CollidablePlayerObject(string id, ActorType actorType, StatusType statusType, Transform3D transform,
            EffectParameters effectParameters, IVertexData vertexData,
            ICollisionPrimitive collisionPrimitive, ObjectManager objectManager,
            Keys[] moveKeys, float moveSpeed, float strafeSpeed, KeyboardManager keyboardManager, CameraManager<Camera3D> cameraManager, int lives)
            : base(id, actorType, statusType, transform, effectParameters, vertexData, collisionPrimitive, objectManager)
        {
            this.moveKeys = moveKeys;
            this.moveSpeed = moveSpeed;
            this.strafeSpeed = strafeSpeed;
            this.groundPos = transform.Translation.Y;
            this.jumpHeight = GameConstants.playerJumpHeight;
            this.IsJumpingUp = false;
            //for movement
            this.keyboardManager = keyboardManager;
            this.isOnLevel1 = true;
            this.isPlayerPaused = true;
            this.coolingDown = false;
            this.cameraManager = cameraManager;
            this.lives = lives;
        }

        public override void Update(GameTime gameTime)
        {
            // if (gameTime.ElapsedGameTime.TotalSeconds > 12) { }
            if (gameTime.TotalGameTime.TotalSeconds > 15 && isOnLevel1 && isPlayerPaused)
            {
                cameraManager.ActiveCameraIndex++;
                this.isPlayerPaused = false;
            }
            if (!isOnLevel1 && isPlayerPaused && !coolingDown) 
            {
                //gameTime.TotalGameTime.Negate();
                startCooldown(gameTime);
                coolingDown = true;
            }
            else if (!isOnLevel1&& gameTime.TotalGameTime.TotalSeconds >= restTime ) 
            {
                this.isPlayerPaused = false;
            }
            //makes player move foreward
            //Transform3D.TranslateIncrement = Transform3D.Look * gameTime.ElapsedGameTime.Milliseconds* moveSpeed;
            //read any input and store suggested increments
            HandleStrafe(gameTime);

            //have we collided with something?
            Collidee = CheckAllCollisions(gameTime);

            //how do we respond to this collidee e.g. pickup?
            HandleCollisionResponse(Collidee);

            //if no collision then move - see how we set this.Collidee to null in HandleCollisionResponse()
            //below when we hit against a zone
            if (Collidee == null)
            {
                ApplyInput(gameTime);
            }

            //reset translate and rotate and update primitive
            base.Update(gameTime);
        }

        private void startCooldown(GameTime gameTime)
        {
            this.restTime = gameTime.TotalGameTime.TotalSeconds + 5.0;
        }

        protected override void HandleStrafe(GameTime gameTime)
        {
            if (!isPlayerPaused) 
            {
                Transform3D.TranslateIncrement
                    = Transform3D.Look * gameTime.ElapsedGameTime.Milliseconds
                            * moveSpeed;

                if (keyboardManager.IsKeyDown(moveKeys[2])) //Left
            {
                Transform3D.TranslateIncrement +=
                    -Transform3D.Right * gameTime.ElapsedGameTime.Milliseconds * strafeSpeed;
                //Transform3D.RotateIncrement = gameTime.ElapsedGameTime.Milliseconds * rotationSpeed;
            }
            else if (keyboardManager.IsKeyDown(moveKeys[3])) //Right
            {
                Transform3D.TranslateIncrement +=
                    Transform3D.Right * gameTime.ElapsedGameTime.Milliseconds * strafeSpeed;
            }
            if (keyboardManager.IsFirstKeyPress(moveKeys[4])) //jump
            {
                object[] parameters = { "jump" };
                EventDispatcher.Publish(new EventData(EventCategoryType.Sound,
                    EventActionType.OnPlay2D, parameters));
                System.Diagnostics.Debug.WriteLine("Jump button pressed");
                if (!IsJumpingUp)
                {
                    IsJumpingUp = true;
                }
            }
            if (Transform3D.Translation.Y < jumpHeight && IsJumpingUp) 
            {
                System.Diagnostics.Debug.WriteLine("Moving Up");
                Transform3D.TranslateIncrement +=
                        Transform3D.Up * gameTime.ElapsedGameTime.Milliseconds * strafeSpeed;
            }
            if (Transform3D.Translation.Y >= jumpHeight /*&& IsJumpingUp == true*/)
                {
                    IsJumpingUp = false;
                }
            if (Transform3D.Translation.Y > groundPos && !IsJumpingUp) 
            {
                Transform3D.TranslateIncrement -=
                        Transform3D.Up * gameTime.ElapsedGameTime.Milliseconds * strafeSpeed;
            }
            }
            
        }

        /********************************************************************************************/

        //this is where you write the application specific CDCR response for your game
        protected override void HandleCollisionResponse(Actor collidee)
        {
            if (collidee is CollidableZoneObject)
            {
                CollidableZoneObject simpleZoneObject = collidee as CollidableZoneObject;

                //do something based on the zone type - see Main::InitializeCollidableZones() for ID
                if (simpleZoneObject.ID.Equals("finish line 1"))
                {
                    //publish an event e.g sound, health progress
                    object[] parameters = { "win" };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Sound,
                        EventActionType.OnPlay2D, parameters));
                    
                    Transform3D.Translation = GameConstants.playerLevel2StartPos;
                    isOnLevel1 = false;
                    isPlayerPaused = true;

                    //object[] parameters2 = { "move to level 2" };
                    //EventDispatcher.Publish(new EventData(EventCategoryType.Player,
                    //    EventActionType.OnPlay2D, parameters2));
                }

                if (simpleZoneObject.ID.Equals("finish line 2"))
                {
                    //publish an event e.g sound, health progress
                    object[] parameters = { "win" };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Sound,
                        EventActionType.OnPlay2D, parameters));

                    object[] parameters2 = { "youWinMenu" };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Menu,
                        EventActionType.OnWin, parameters2));

                    isPlayerPaused = true;
                    Transform3D.Translation = new Vector3(0, 0, 0);
                }
                //IMPORTANT - setting this to null means that the ApplyInput() method will get called and the player can move through the zone.
                Collidee = null;
            }
            else if (collidee is CollidablePrimitiveObject)
            {
                //the boxes on the left that we loaded from level loader
                if (collidee.ActorType == ActorType.CollidablePickup)
                {
                    //remove the object
                    object[] parameters = { collidee };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Object, EventActionType.OnRemoveActor, parameters));
                }
                //the boxes on the right that move up and down
                else if (collidee.ActorType == ActorType.CollidableDecorator)
                {
                    //(collidee as DrawnActor3D).EffectParameters.DiffuseColor = Color.Blue;

                }
                else if (collidee.ActorType == ActorType.NPC)
                {
                    object[] parameters = { "Die" };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Sound,
                        EventActionType.OnPlay2D, parameters));
                    respawn();
                    //(collidee as DrawnActor3D).EffectParameters.DiffuseColor = Color.Black;
                }
                else if (collidee.ActorType == ActorType.CollidableGround) 
                {
                    object[] parameters = { "bump" };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Sound,
                        EventActionType.OnPlay2D, parameters));
                }
            }
            
        }
        private void respawn() 
        {
            lives--;
            if (isOnLevel1)
            {
                Transform3D.Translation = GameConstants.playerLevel1StartPos;
            }
            else 
            {
                Transform3D.Translation = GameConstants.playerLevel2StartPos;
            }
            if (lives < 1) 
            {
                Transform3D.Translation = new Vector3(0,0,0);
                object[] parameters = { "youLoseMenu" };
                EventDispatcher.Publish(new EventData(EventCategoryType.Menu,
                    EventActionType.OnLose, parameters));
                isPlayerPaused = true;
            }
        }
    }
}