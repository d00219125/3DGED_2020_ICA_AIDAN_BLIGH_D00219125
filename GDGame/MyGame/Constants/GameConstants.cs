﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDGame
{
    public class GameConstants
    {
        #region Common

        private static readonly float strafeSpeedMultiplier = 0.75f;
        public static readonly Keys[] KeysOne = { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space};
        public static readonly Keys[] KeysTwo = { Keys.U, Keys.J, Keys.H, Keys.K };
        //   public static readonly Keys[] KeysThree = { Keys.U, Keys.J, Keys.H, Keys.K };
        #endregion Common

        #region String IDs
        public static readonly string Primitive_WireframeOriginHelper = "wireframe origin helper";
        public static readonly string Primitive_UnlitTexturedQuad = "unlit textured quad";
        public static readonly string Primitive_LitTexturedQuad = "lit textured quad";
        public static readonly string Primitive_LitTexturedPyramid = "lit textured pyramid";
        public static readonly string Primitive_LitTexturedCube = "lit textured cube";

        public static readonly string Effect_UnlitTextured = "unlit textured";
        public static readonly string Effect_LitTextured = "lit textured";
        public static readonly string Effect_UnlitWireframe = "unlit wireframe";

        public static readonly string Camera_NonCollidableFirstPerson = "Noncollidable First person";
        public static readonly string Camera_NonCollidableFlight = "Noncollidable Flight";
        public static readonly string Camera_NonCollidableSecurity = "Noncollidable security";
        public static readonly string Camera_NonCollidableCurveMainArena = "Noncollidable curve - main arena";
        public static readonly string Camera_CollidableThirdPerson = "Collidable Third Person";

        public static readonly string Controllers_NonCollidableFirstPerson = "1st person controller";
        public static readonly string Controllers_NonCollidableFlight = "Flight controller";
        public static readonly string Controllers_NonCollidableSecurity = "pan controller";
        public static readonly string Controllers_NonCollidableCurveMainArena = "main arena - fly through - 1";
        public static readonly string Controllers_CollidableThirdPerson = "3rd person controller";

        #endregion String IDs

        #region Collidable First Person Camera

        public static readonly Keys[] CameraMoveKeys = { Keys.W, Keys.S, Keys.A, Keys.D, //F,B,L,R
                                        Keys.Space, //Jump
                                        Keys.C, //Crouch
                                        Keys.LeftShift, Keys.RightShift}; //Other

        //JigLib related collidable camera properties
        public static readonly float CollidableCameraJumpHeight = 15;

        public static readonly float CollidableCameraMoveSpeed = 1f;
        public static readonly float CollidableCameraStrafeSpeed = 0.6f * CollidableCameraMoveSpeed;
        public static readonly float CollidableCameraCapsuleRadius = 2;
        public static readonly float CollidableCameraViewHeight = 8; //how tall is the first person player?
        public static readonly float CollidableCameraMass = 10;

        #endregion Collidable First Person Camera

        #region First Person Camera

        public static readonly float moveSpeed = 0.2f;
        public static readonly float strafeSpeed = strafeSpeedMultiplier * moveSpeed;
        public static readonly float rotateSpeed = 0.01f;

        #endregion First Person Camera

        #region Flight Camera

        public static readonly float flightMoveSpeed = 0.8f;
        public static readonly float flightStrafeSpeed = strafeSpeedMultiplier * flightMoveSpeed;
        public static readonly float flightRotateSpeed = 0.01f;

        #endregion Flight Camera

        #region Security Camera

        private static readonly float angularSpeedMultiplier = 10;
        public static readonly float lowAngularSpeed = 10;
        public static readonly float mediumAngularSpeed = lowAngularSpeed * angularSpeedMultiplier;
        public static readonly float hiAngularSpeed = mediumAngularSpeed * angularSpeedMultiplier;

        #endregion Security Camera

        #region Car

        public static readonly float carMoveSpeed = 0.08f;
        public static readonly float carRotateSpeed = 0.06f;

        #endregion Car

        //#region Player
        //Nials Stuff
        //public static readonly float playerMoveSpeed = 0.05f;
        //public static readonly float playerStrafeSpeed = strafeSpeedMultiplier * moveSpeed;
        //public static readonly float playerRotateSpeed = 0.04f;
        //public static readonly float playerCamOffsetX = 100f;
        //public static readonly float playerCamOffsetY = 100f;
        //#endregion Player

        #region Player
        public static readonly Vector3 playerStartPos = new Vector3(0, 0, 0);
        public static readonly float playerMoveSpeed = .05f;
        public static readonly float playerStrafeSpeed = .03f;
        public static readonly float playerCamOffsetX = 10;
        public static readonly float playerCamOffsetY = 10;
        public static readonly Keys[] playerJumpButton = { Keys.Space };
        #endregion Player

        #region Level
        public static readonly Vector3 wallScale = new Vector3(100, 1000, 100);
        public static readonly Vector3 wallRotationLeft = new Vector3(90, 0, 90);
        public static readonly Vector3 wallRotationRight = new Vector3(-270, -180, -90);
        #endregion Level
    }
}