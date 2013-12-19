using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity_War
{
    class Bullets
    {
        private static List<Bullet> bullets = new List<Bullet>();
        
        public static List<Bullet> getBullets()
        {
            return bullets;
        }
        public static void remove(int b)
        {
            bullets.RemoveAt(b);
        }
        public static void add(Bullet b)
        {
            bullets.Add(b);
        }
        public static void clear()
        {
            bullets = new List<Bullet>();
        }
    }
}
