using System;
using System.Collections.Generic;
using System.Text;
using GDLibrary.Actors;
using GDLibrary.Enums;
using GDLibrary.Events;
using GDLibrary.Interfaces;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using Microsoft.Xna.Framework;

using System.Diagnostics;

namespace GDGame.MyGame.Actors
{
    class CollidableEnemySphereObject : CollidablePrimitiveObject
    {
        #region Fields
        private float moveSpeed;
        private float movenentRange, maxXRange, minXRange, maxYRange, minYRange;
        private float yValue;
        private bool moveRight, moveDown;
        private Vector3 startPos;
        #endregion Fields

        public CollidableEnemySphereObject(string id, ActorType actorType, StatusType statusType, Transform3D transform,
            EffectParameters effectParameters, IVertexData vertexData,
            ICollisionPrimitive collisionPrimitive, ObjectManager objectManager,
            float moveSpeed, float moveRange)
            : base(id, actorType, statusType, transform, effectParameters, vertexData, collisionPrimitive, objectManager)
        {
            this.moveSpeed = moveSpeed;
            this.movenentRange = moveRange;

            this.moveRight = true;
            this.moveDown = true;
            //this.startPos = new Vector3(transform.Translation.X, transform.Translation.Y, transform.Translation.Z);
            //this.yValue = transform.Translation.Y;
            this.maxXRange = transform.Translation.X + movenentRange+1;
            this.minXRange = transform.Translation.X - movenentRange;
            this.maxYRange = transform.Translation.Y +1;
            this.minYRange = 5f;
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine("is updating");
            HandleStrafe(gameTime);
            Collidee = CheckAllCollisions(gameTime);
            HandleCollisionResponse(Collidee);
            if (Collidee == null)
            {
                ApplyInput(gameTime);
            }
            //reset translate and rotate and update primitive
            base.Update(gameTime);
        }

        protected override void HandleStrafe(GameTime gameTime)
        {
            
            if (moveRight)
            {
                if (this.Transform3D.Translation.X >= maxXRange)
                {
                    moveRight = false;
                }
                else
                {
                    this.Transform3D.TranslateBy(new Vector3(moveSpeed, 0, 0));
                }
            }
            else
            {
                if (this.Transform3D.Translation.X <= minXRange)
                {
                    moveRight = true;
                    //Debug.WriteLine("changing direction");
                }
                else
                {
                    this.Transform3D.TranslateBy(new Vector3(-moveSpeed, 0, 0));
                    //Debug.WriteLine("position = " + this.Transform3D.Translation.X);
                }
            }

            if (moveDown)
            {
                if (this.Transform3D.Translation.Y <= minYRange)
                {
                    moveDown = false;
                    //Debug.WriteLine("changing direction to up");
                }
                
                else
                {
                    this.Transform3D.TranslateBy(new Vector3(0,-moveSpeed, 0));
                    //Debug.WriteLine("position = " + this.Transform3D.Translation.Y);
                }
            }
            else
            {
               if (this.Transform3D.Translation.Y >= maxYRange)
                {
                    moveDown = true;
                    //Debug.WriteLine("changing direction to down");
                }
                else
                {
                    this.Transform3D.TranslateBy(new Vector3( 0,moveSpeed, 0));
                    //Debug.WriteLine("position = " + this.Transform3D.Translation.Y);
                }
            }

            //if (moveRight && moveDown)
            //{
            //    if (this.Transform3D.Translation.X >= maxXRange)
            //    {
            //        moveRight = false;
            //        Debug.WriteLine("changing X direction to left");
            //    }
            //    if (this.Transform3D.Translation.Y <= minYRange)
            //    {
            //        moveDown = false;
            //        Debug.WriteLine("changing Y direction to up");
            //    }
            //    this.Transform3D.TranslateBy(new Vector3(moveSpeed, -moveSpeed / 2, 0));
            //}
            //else if (moveRight && !moveDown) 
            //{
            //    if (this.Transform3D.Translation.X >= maxXRange)
            //    {
            //        moveRight = false;
            //        Debug.WriteLine("changing X direction to left");
            //    }
            //    if (this.Transform3D.Translation.Y >= maxYRange)
            //    {
            //        moveDown = true;
            //        Debug.WriteLine("changing Y direction to down");
            //    }
            //    this.Transform3D.TranslateBy(new Vector3(moveSpeed, moveSpeed / 2, 0));
            //}
        }

        /********************************************************************************************/

        //this is where you write the application specific CDCR response for your game
        protected override void HandleCollisionResponse(Actor collidee)
        {
            if (collidee is CollidableZoneObject)
            {
                CollidableZoneObject simpleZoneObject = collidee as CollidableZoneObject;

                //do something based on the zone type - see Main::InitializeCollidableZones() for ID
                if (simpleZoneObject.ID.Equals("sound and camera trigger zone 1"))
                {
                    //publish an event e.g sound, health progress
                    object[] parameters = { "win" };
                    EventDispatcher.Publish(new EventData(EventCategoryType.Sound,
                        EventActionType.OnPlay2D, parameters));
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
                    // (collidee as DrawnActor3D).EffectParameters.DiffuseColor = Color.Blue;

                }
                else if (collidee.ActorType == ActorType.NPC)
                {
                    //reset player to spawn and subtract one health

                }
            }
        }
    }
}
//if (yValue <= 0)
            //{
            //    yValue = System.MathF.Sin((float)(1f * gameTime.TotalGameTime.TotalSeconds));
            //}
            //else if (yValue >= startPos.Y) 
            //{
            //    yValue = System.MathF.Sin((float)(1f * gameTime.TotalGameTime.TotalSeconds));
            //}

            //yValue += System.MathF.Sin((float)(1f * gameTime.TotalGameTime.TotalSeconds));
            //Debug.WriteLine("Is strafing");
            //if (moveRight)
            //{
            //    if (this.Transform3D.Translation.X >= movenentRange)
            //    {
            //        moveRight = false;
            //        Debug.WriteLine("changing direction");
            //    }
                
            //    this.Transform3D.TranslateBy(new Vector3(moveSpeed, System.MathF.Sin((float)(moveSpeed/2 * gameTime.TotalGameTime.TotalSeconds)), 0));
            //    Debug.WriteLine("position = " + this.Transform3D.Translation.X);
            //}
            //else
            //{
            //    if (this.Transform3D.Translation.X <= startPos.X)
            //    {
            //        moveRight = true;
            //        Debug.WriteLine("changing direction");
            //    }
            //    this.Transform3D.TranslateBy(new Vector3(-moveSpeed, System.MathF.Sin((float)(moveSpeed/2 * gameTime.TotalGameTime.TotalSeconds)), 0));
            //    Debug.WriteLine("position = " + this.Transform3D.Translation.X);
            //}