using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BradleyXboxUtils;

namespace Gravity_War
{
    class Player
    {
        public static Texture2D image;
        public static Vector2 origin;
        public static float scale;
        public static float radius;
        public static double timeStep;
        public static long life = 10000000000000;

        private long health;

        private Vector2 location;
        private Vector2 velocity;
        private double time;

        private Planet planet;
        private float angle;

        private Input input;
        private ControlButton fireButton;
        private ControlButton jump;

        public Player(Planet planet, float angle, Input input)
        {
            health = life;
            if (scale == 0)
            {
                origin = new Vector2(image.Width / 2, image.Height / 2);
                scale = 2 * radius / image.Width;
            }
            this.input = input;
            this.planet = planet;
            this.angle = angle;
            this.jump = new ControlButton(60);
            this.fireButton = new ControlButton(10);
        }
        public void fire()
        {
            Bullets.add(new Bullet(this.location + BradleyXboxUtils.UTIL.magD(Player.radius*2, angle), BradleyXboxUtils.UTIL.magD(30, angle)));
        }
        public void checkHit()
        {
            for (int a = 0; a < Bullets.getBullets().Count; a++)
            {
                Bullet temp = Bullets.getBullets().ElementAt<Bullet>(a);
                if (BradleyXboxUtils.UTIL.distance(temp.getLocation(), this.location) <= Player.radius)
                {
                    health -= 10;
                    Bullets.remove(a);
                    a--;
                }

            }
        }
        public bool isDead()
        {
            return (health <= 0);
        }

        public void run()
        {
            Vector2 gravity = Planets.getGravityField(location);
            move();
            angle = (float)BradleyXboxUtils.UTIL.normalizeDirection(angle);
            
            if (planet == null)
            {
                velocity += gravity * (float)timeStep;
                velocity += BradleyXboxUtils.UTIL.magD(.3, angle);

                if (velocity.Length() > 10)
                {
                    velocity = UTIL.magD(10, Math.Atan2(velocity.Y, velocity.X));
                }
                
                location += velocity * (float)timeStep;
                
                

                if (Planets.collides(getLocation()))
                {
                    land(Planets.collide(getLocation()));
                }
            }
            else
            {   
                location = planet.getLocation() + BradleyXboxUtils.UTIL.magD(planet.getRadius(), angle);

                if (jump.update(input.getBottomActionButton()))
                {
                    launch();
                }
            }
            if (fireButton.update((input.getRightTrigger())>.5))
                fire();
            checkHit();

        }
        public void launch()
        {
            if (planet != null)
            {
                planet = null;
                velocity = BradleyXboxUtils.UTIL.magD(20, angle);
                location += 2 * velocity * (float)timeStep;
            }
        }
        public void land(Planet p)
        {
            planet = p;
            angle = (float)Math.Asin((location.Y - p.getLocation().Y) / p.getRadius());
            float angleX = (float)Math.Acos((location.X - p.getLocation().X) / p.getRadius());
            if (angle != angleX)
            {
                angle = angleX;
            }
            angle =(float) Math.Atan2((location.Y - p.getLocation().Y) / p.getRadius(),(location.X - p.getLocation().X) / p.getRadius());

        }

        public Vector2 getLocation()
        {
            return location;
        }

        public void move()
        {
            Vector2 joystick = input.getLeftJoystick();
            if (joystick.Length() < .1)
                return;
            if (Math.Abs(joystick.X) > Math.Abs(joystick.Y))
            {
                if (joystick.X > 0)
                    moveUp();
                else
                    moveDown();
            }
            else
            {
                if (joystick.Y > 0)
                    moveLeft();
                else
                    moveRight();
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
            //if (planet != null)
                return angle+(float)(Math.PI/2);// - Math.PI/6);
            //return (float)(Math.Atan2(velocity.Y, velocity.X) + Math.PI / 2);// - Math.PI / 6);
        }

        public bool airborne()
        {
            return (planet == null);
        }

        public bool getReset()
        {
            return input.getStart();
        }

    }
}
