using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity_War
{
    class Bullet 
    {
        public static Texture2D image;
        public static Vector2 origin;
        public static float scale;
        public static float radius;
        private Vector2 location;
        private Vector2 velocity;
        private double time;
        private static double timeStep = .5;

        public Bullet(Vector2 location, Vector2 velocity)
        {
            if (scale == 0)
            {
                origin = new Vector2(image.Width / 2, image.Height / 2);
                scale = 2 * radius / image.Width;
            }

            this.location = location;
            this.velocity = velocity;
            time = 5;
            
        }
        public void run(Vector2 gravity)
        {
            time+=timeStep;
            velocity += gravity*(float)timeStep;
            location += velocity*(float)timeStep;
        }
        public Vector2 getLocation()
        {
            return location;
        }
        public float getRotation()
        {
            return (float)(Math.Atan2(velocity.Y, velocity.X)+Math.PI/2-Math.PI/6);
        }

    }
}
