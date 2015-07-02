using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuery.Canvas
{
    public class Color
    {
        public Color()
        {
            R = 0;
            G = 0;
            B = 0;
            A = 1;
        }

        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
            A = 1;
        }

        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public float R
        {
            get { return _r; }
            set { _r = value.Clamp(0, 1); }
        }

        public float G
        {
            get { return _g; }
            set { _g = value.Clamp(0, 1); }
        }

        public float B
        {
            get { return _b; }
            set { _b = value.Clamp(0, 1); }
        }

        public float A
        {
            get { return _a; }
            set { _a = value.Clamp(0, 1); }
        }

        private float _r;
        private float _g;
        private float _b;
        private float _a;

        public static Color operator +(Color c1, Color c2)
        {
            return new Color(
                c1.R + c2.R,
                c1.G + c2.G,
                c1.B + c2.B,
                c1.A + c2.A);
        }

        public static Color operator -(Color c1, Color c2)
        {
            return new Color(
                c1.R - c2.R,
                c1.G - c2.G,
                c1.B - c2.B,
                c1.A - c2.A);
        }

        public static Color operator *(Color c1, Color c2)
        {
            return new Color(
                c1.R * c2.R,
                c1.G * c2.G,
                c1.B * c2.B,
                c1.A * c2.A);
        }

        public static Color operator /(Color c1, Color c2)
        {
            return new Color(
                c1.R / c2.R,
                c1.G / c2.G,
                c1.B / c2.B,
                c1.A / c2.A);
        }

        public static Color operator %(Color c1, Color c2)
        {
            return new Color(
                c1.R % c2.R,
                c1.G % c2.G,
                c1.B % c2.B,
                c1.A % c2.A);
        }

        public override string ToString()
        {
            return string.Format("{{{0},{1},{2},{3}}}", R, G, B, A);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Color))
                return false;

            Color c = obj as Color;
            return R == c.R && G == c.G && B == c.B && A == c.A;
        }
    }
}
