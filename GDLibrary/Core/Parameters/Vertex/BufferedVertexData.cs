﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace GDLibrary.Parameters
{
    /// <summary>
    /// This child class for drawing primitives where the vertex data is buffered on the GFX card in VRAM.
    ///            Note:
    ///            - The class is generic and can be used to draw VertexPositionColor, VertexPositionColorTexture, VertexPositionColorNormal types etc.
    ///            - For each draw the GFX card refers to vertex data that has already been buffered to VRAM
    ///           - This is a more efficient approach than either using the VertexData or DynamicBufferedVertexData classes if
    ///              you wish to draw a large number of primitives on screen.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BufferedVertexData<T> : VertexData<T>, ICloneable  where T : struct, IVertexType //, ICloneable 
    {
        #region Variables
        private VertexBuffer vertexBuffer;
        private GraphicsDevice graphicsDevice;
        private IndexBuffer indexBuffer;
        #endregion Variables

        #region Properties
        public VertexBuffer VertexBuffer
        {
            get
            {
                return vertexBuffer;
            }
            set
            {
                vertexBuffer = value;
            }
        }
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.graphicsDevice;
            }
        }
        #endregion Properties

        //allows developer to pass in vertices AND buffer - more efficient since buffer is defined ONCE outside of the object instead of a new VertexBuffer for EACH instance of the class
        public BufferedVertexData(GraphicsDevice graphicsDevice, T[] vertices,
            VertexBuffer vertexBuffer, IndexBuffer indexBuffer, PrimitiveType primitiveType, int primitiveCount)
            : base(vertices, primitiveType, primitiveCount)
        {
            this.graphicsDevice = graphicsDevice;
            this.VertexBuffer = vertexBuffer;

            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);

            this.indexBuffer = indexBuffer;
        }

        //buffer is created INSIDE the class so each class has a buffer - not efficient
        public BufferedVertexData(GraphicsDevice graphicsDevice,
            T[] vertices, PrimitiveType primitiveType, int primitiveCount)
            : base(vertices, primitiveType, primitiveCount)
        {
            this.graphicsDevice = graphicsDevice;
            this.VertexBuffer = new VertexBuffer(graphicsDevice,
                typeof(T), //need type to calculate "stride" of the underlying vertextype
                vertices.Length, BufferUsage.None);

            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        public void SetData(T[] vertices)
        {
            this.Vertices = vertices;
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        public override void Draw(GameTime gameTime, BasicEffect effect)
        {
            //this is what we want GFX to draw
            effect.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);

            effect.GraphicsDevice.Indices = this.indexBuffer;

            //draw!
            //effect.GraphicsDevice.DrawPrimitives(this.PrimitiveType, 0, this.PrimitiveCount);
            effect.GraphicsDevice.DrawIndexedPrimitives(this.PrimitiveType, 0,0, this.PrimitiveCount);

        }

        public new object Clone()
        {
            return new BufferedVertexData<T>(this.graphicsDevice,  //shallow - reference
                this.Vertices, //shallow - reference
                this.vertexBuffer,
                this.indexBuffer,
                this.PrimitiveType, //struct - deep
                this.PrimitiveCount); //deep - primitive
        }
    }
}