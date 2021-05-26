using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mateja_Bisevac_40_2017
{
    public class Vector2D
    {
        private double x;
        private double y;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2D()
        {
            Init(0, 0);
        }

        public Vector2D(double x, double y)
        {
            Init(x, y);
        }

        public Vector2D(Vector2D v)
        {
            Init(v.x, v.y);
        }

        private void Init(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double Intensity()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double DotProduct(Vector2D v)
        {
            return x * v.x + y * v.y;
        }

        public void Rotate(double phi)
        {
            double x1 = x * Math.Cos(phi) - y * Math.Sin(phi);
            double y1 = 1 * x * Math.Sin(phi) + y * Math.Cos(phi);

            x = x1;
            y = y1;
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            Vector2D v = new Vector2D(a);
            v.x += b.x;
            v.y += b.y;
            return v;
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            Vector2D v = new Vector2D(a);
            v.x -= b.x;
            v.y -= b.y;
            return v;
        }

        public static Vector2D operator *(Vector2D a, double k)
        {
            Vector2D v = new Vector2D(a);
            v.x *= k;
            v.y *= k;
            return v;
        }

        public static bool operator ==(Vector2D a, Vector2D b)
        {
            if (Math.Abs(a.x - b.x) < 1.0E-6 && Math.Abs(a.y - b.y) < 1.0E-6)
                return true;

            return false;
        }

        public static bool operator !=(Vector2D a, Vector2D b)
        {
            if (Math.Abs(a.x - b.x) < 1.0E-6 && Math.Abs(a.y - b.y) < 1.0E-6)
                return false;

            return true;
        }
        public static bool operator <(Vector2D a, Vector2D b)
        {
            if (a.x < b.x && a.y < b.y) return true;
            return false;
        }
        public static bool operator >(Vector2D a, Vector2D b)
        {
            if (a.x > b.x && a.y > b.y) return true;
            return false;
        }
        // odredjivanje relativne pozicije 2 vekotra 
        public  bool levoGore( Vector2D b)
        {
            if (this.x >= b.x && this.y <= b.y) return true;
            return false;
        }
        public  bool levoDole( Vector2D b)
        {
            if (this.x >= b.x && this.y >= b.y) return true;
            return false;
        }
        public  bool desnoDole(Vector2D b)
        {
            if (this.x <= b.x && this.y >= b.y) return true;
            return false;
        }
        public  bool desnoGore(Vector2D b)
        {
            if (this.x <= b.x && this.y <= b.y) return true;
            return false;
        }



        public override string ToString()
        {
            return "Vector2D(" + x + ", " + y + ")";
        }
    }
}
