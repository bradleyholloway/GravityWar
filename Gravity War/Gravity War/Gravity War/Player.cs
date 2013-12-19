using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gravity_War
{
    class Player
    {
        public static Texture2D image;
        public static Vector2 origin;
        public static float scale;
        public static float radius;
        public static double timeStep;

        private Vector2 location;
        private Vector2 velocity;
        private double time;

        private Planet planet;
        private float angle;

        public Player(Planet planet, float angle)
        {
            this.planet = planet;
            this.angle = angle;
        }


        public Player(Vector2 location, Vector2 velocity)
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
            if (planet == null)
            {
                velocity += gravity * (float)timeStep;
                location += velocity * (float)timeStep;
            }
            else
                location = planet.getLocation() + BradleyXboxUtils.UTIL.magD(planet.getRadius(), angle);
        }
        public Vector2 getLocation()
        {
            return location;
        }

        public void move(Vector2 joystick)
        {
            if (Math.Abs(joystick.X) > Math.Abs(joystick.Y))
            {
                if (joystick.X > 0)
                    moveRight();
                else
                    moveLeft();
            }
            else
            {
                if (joystick.Y > 0)
                    moveUp();
                else
                    moveDown();
            }
        }
        private void moveRight()
        {
            if (angle > Math.PI / 2 && angle < Math.PI * 3 / 2)
            {
                angle += .1f;
            }
            else
            {
                angle -= .1f;
            }
        }
        private void moveLeft()
        {
            if (angle > Math.PI / 2 && angle < Math.PI * 3 / 2)
            {
                angle -= .1f;
            }
            else
            {
                angle += .1f;
            }
        }
        private void moveUp()
        {
            if (angle > Math.PI)
            {
                angle += .1f;
            }
            else
            {
                angle -= .1f;
            }
        }
        private void moveDown()
        {
            if (angle > Math.PI)
            {
                angle -= .1f;
            }
            else
            {
                angle += .1f;
            }
        }

        public float getRotation()
        {
            if (planet != null)
                return angle;
            return (float)(Math.Atan2(velocity.Y, velocity.X) + Math.PI / 2 - Math.PI / 6);
        }

    }
}
