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
    class CollidableEnemyPyramidObject : CollidablePrimitiveObject
    {
        #region Fields
        private float moveSpeed;
        private float movenentRange, maxRange, minRange;
        private bool moveRight;
        #endregion Fields

        public CollidableEnemyPyramidObject(string id, ActorType actorType, StatusType statusType, Transform3D transform,
            EffectParameters effectParameters, IVertexData vertexData,
            ICollisionPrimitive collisionPrimitive, ObjectManager objectManager,
            float moveSpeed, float moveRange)
            : base(id, actorType, statusType, transform, effectParameters, vertexData, collisionPrimitive, objectManager)
        {
            this.moveSpeed = moveSpeed;
            this.movenentRange = moveRange;
            this.maxRange = transform.Translation.X + movenentRange;
            this.minRange = transform.Translation.X - movenentRange;
            //Debug.WriteLine("MoveRange = " + movenentRange);
            //Debug.WriteLine("MinRange = " + minRange);
            //Debug.WriteLine("MaxRange = " + maxRange);
            this.moveRight = true;
        }

        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine("is updating");
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

        protected override void HandleStrafe(GameTime gameTime)
        {
            Debug.WriteLine("Is strafing");
            if (moveRight)
            {
                if (this.Transform3D.Translation.X >= maxRange)
                {
                    moveRight = false;
                    Debug.WriteLine("changing direction");
                }
                else 
                {
                    this.Transform3D.TranslateBy(new Vector3(moveSpeed, 0, 0));
                   Debug.WriteLine("position = " + this.Transform3D.Translation.X);
                }
            }
            else 
            {
                if (this.Transform3D.Translation.X <= minRange)
                {
                    moveRight = true;
                    Debug.WriteLine("changing direction");
                }
                else
                {
                    this.Transform3D.TranslateBy(new Vector3(-moveSpeed, 0, 0));
                    Debug.WriteLine("position = " + this.Transform3D.Translation.X);
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
                    (collidee as DrawnActor3D).EffectParameters.DiffuseColor = Color.Blue;

                }
                else if (collidee.ActorType == ActorType.NPC)
                {
                    //reset player to spawn and subtract one health

                }
            }
        }
    }
}
