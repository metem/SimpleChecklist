using System;
using System.Runtime.Serialization;

namespace SimpleChecklist.Common.Entities
{
    [DataContract]
    public class PortableColor : IEquatable<PortableColor>
    {
        public PortableColor(int r, int g, int b, int a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public PortableColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
            A = 255;
        }

        public static PortableColor FromRgba(int r, int g, int b, int a)
        {
            return new PortableColor(r, g, b, a);
        }

        public static PortableColor FromRgb(int r, int g, int b)
        {
            return new PortableColor(r, g, b);
        }

        [DataMember]
        public int R { get; set; }

        [DataMember]
        public int G { get; set; }

        [DataMember]
        public int B { get; set; }

        [DataMember]
        public int A { get; set; }

        public bool Equals(PortableColor other)
        {
            return other != null && (other.R == R) && (other.G == G) && (other.B == B) && (other.A == A);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is PortableColor color && Equals(color);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R;
                hashCode = (hashCode*397) ^ G;
                hashCode = (hashCode*397) ^ B;
                hashCode = (hashCode*397) ^ A;
                return hashCode;
            }
        }
    }
}