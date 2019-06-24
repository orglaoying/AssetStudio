using System;
using System.Runtime.InteropServices;

namespace AssetStudio
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public const float Rad2Deg = 57.29578f;


        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new ArgumentOutOfRangeException(nameof(index), "Invalid Quaternion index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new ArgumentOutOfRangeException(nameof(index), "Invalid Quaternion index!");
                }
            }
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2) ^ (W.GetHashCode() >> 1);
        }

        public override bool Equals(object other)
        {
            if (!(other is Quaternion))
                return false;
            return Equals((Quaternion)other);
        }

        public bool Equals(Quaternion other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        public Vector3 QuaternionToEuler()
        {
            Vector3 result;

            float test = X * Y + Z * W;
            // singularity at north pole
            if (test > 0.499)
            {
                result.X = 0;
                result.Y = 2 * (float)Math.Atan2(X, W);
                result.Z = (float)Math.PI / 2;
            }
            // singularity at south pole
            else if (test < -0.499)
            {
                result.X = 0;
                result.Y = (float)(-2 * Math.Atan2(X, W));
                result.Z = (float)(-Math.PI / 2);
            }
            else
            {
                result.X = (float)(Rad2Deg * Math.Atan2(2 * X * W- 2 * Y * Z, 1 - 2 * X * X - 2 * Z * Z));
                result.Y = (float)(Rad2Deg * Math.Atan2(2 * Y * W - 2 * X * Z, 1 - 2 * Y * Y - 2 * Z * Z));
                result.Z = (float)(Rad2Deg * Math.Asin(2 * X * Y + 2 * Z * W));

                if (result.X < 0) result.X += 360;
                if (result.Y < 0) result.Y += 360;
                if (result.Z < 0) result.Z += 360;
            }
            return result;
        }

        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        private static bool IsEqualUsingDot(float dot)
        {
            return dot > 1.0f - kEpsilon;
        }

        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return !(lhs == rhs);
        }

        private const float kEpsilon = 0.000001F;
    }
}
