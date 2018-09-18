// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Runtime.InteropServices;
using scm = System.ComponentModel;
using uei = UnityEngine.Internal;

namespace UnityEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Quaterniond : IEquatable<Quaterniond>
    {
        // X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public double x;
        // Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public double y;
        // Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public double z;
        // W component of the Quaternion. Don't modify this directly unless you know quaternions inside out.
        public double w;

        // Access the x, y, z, w components using [0], [1], [2], [3] respectively.
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        // Constructs new Quaternion with given x,y,z,w components.
        public Quaterniond(double x, double y, double z, double w) { this.x = x; this.y = y; this.z = z; this.w = w; }

        // Set x, y, z and w components of an existing Quaternion.
        public void Set(double newX, double newY, double newZ, double newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        static readonly Quaterniond identityQuaternion = new Quaterniond(0F, 0F, 0F, 1F);

        // The identity rotation (RO). This quaternion corresponds to "no rotation": the object
        public static Quaterniond identity
        {
            get
            {
                return identityQuaternion;
            }
        }

        // Combines rotations /lhs/ and /rhs/.
        public static Quaterniond operator *(Quaterniond lhs, Quaterniond rhs)
        {
            return new Quaterniond(
                lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        // Rotates the point /point/ with /rotation/.
        public static Vector3d operator *(Quaterniond rotation, Vector3d point)
        {
            double x = rotation.x * 2F;
            double y = rotation.y * 2F;
            double z = rotation.z * 2F;
            double xx = rotation.x * x;
            double yy = rotation.y * y;
            double zz = rotation.z * z;
            double xy = rotation.x * y;
            double xz = rotation.x * z;
            double yz = rotation.y * z;
            double wx = rotation.w * x;
            double wy = rotation.w * y;
            double wz = rotation.w * z;

            Vector3d res;
            res.x = (1F - (yy + zz)) * point.x + (xy - wz) * point.y + (xz + wy) * point.z;
            res.y = (xy + wz) * point.x + (1F - (xx + zz)) * point.y + (yz - wx) * point.z;
            res.z = (xz - wy) * point.x + (yz + wx) * point.y + (1F - (xx + yy)) * point.z;
            return res;
        }

        // *undocumented*
        public const float kEpsilon = 0.000001F;

        // Is the dot product of two quaternions within tolerance for them to be considered equal?
        private static bool IsEqualUsingDot(float dot)
        {
            // Returns false in the presence of NaN values.
            return dot > 1.0f - kEpsilon;
        }

        // Are two quaternions equal to each other?
        public static bool operator ==(Quaterniond lhs, Quaterniond rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        // Are two quaternions different from each other?
        public static bool operator !=(Quaterniond lhs, Quaterniond rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

        // The dot product between two rotations.
        public static double Dot(Quaterniond a, Quaterniond b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        [uei.ExcludeFromDocs]
        public void SetLookRotation(Vector3d view)
        {
            Vector3d up = Vector3d.up;
            SetLookRotation(view, up);
        }

        // Creates a rotation with the specified /forward/ and /upwards/ directions.
        public void SetLookRotation(Vector3d view, [uei.DefaultValue("Vector3d.up")] Vector3d up)
        {
            this = LookRotation(view, up);
        }

        // Returns the angle in degrees between two rotations /a/ and /b/.
        public static float Angle(Quaterniond a, Quaterniond b)
        {
            float dot = Dot(a, b);
            return IsEqualUsingDot(dot) ? 0.0f : Mathf.Acos(Mathf.Min(Mathf.Abs(dot), 1.0F)) * 2.0F * Mathf.Rad2Deg;
        }

        // Makes euler angles positive 0/360 with 0.0001 hacked to support old behaviour of QuaternionToEuler
        private static Vector3d Internal_MakePositive(Vector3d euler)
        {
            float negativeFlip = -0.0001f * Mathf.Rad2Deg;
            float positiveFlip = 360.0f + negativeFlip;

            if (euler.x < negativeFlip)
                euler.x += 360.0f;
            else if (euler.x > positiveFlip)
                euler.x -= 360.0f;

            if (euler.y < negativeFlip)
                euler.y += 360.0f;
            else if (euler.y > positiveFlip)
                euler.y -= 360.0f;

            if (euler.z < negativeFlip)
                euler.z += 360.0f;
            else if (euler.z > positiveFlip)
                euler.z -= 360.0f;

            return euler;
        }

        public Vector3d eulerAngles
        {
            get { return Internal_MakePositive(Internal_ToEulerRad(this) * Mathf.Rad2Deg); }
            set { this = Internal_FromEulerRad(value * Mathf.Deg2Rad); }
        }
        public static Quaternion Euler(float x, float y, float z) { return Internal_FromEulerRad(new Vector3d(x, y, z) * Mathf.Deg2Rad); }
        public static Quaternion Euler(Vector3d euler) { return Internal_FromEulerRad(euler * Mathf.Deg2Rad); }
        public void ToAngleAxis(out float angle, out Vector3d axis) { Internal_ToAxisAngleRad(this, out axis, out angle); angle *= Mathf.Rad2Deg; }
        public void SetFromToRotation(Vector3d fromDirection, Vector3d toDirection) { this = FromToRotation(fromDirection, toDirection); }

        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float angle = Quaternion.Angle(from, to);
            if (angle == 0.0f) return to;
            return SlerpUnclamped(from, to, Mathf.Min(1.0f, maxDegreesDelta / angle));
        }

        public static Quaterniond Normalize(Quaterniond q)
        {
            float mag = Mathd.Sqrt(Dot(q, q));

            if (mag < Mathf.Epsilon)
                return Quaternion.identity;

            return new Quaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
        }

        public void Normalize()
        {
            this = Normalize(this);
        }

        public Quaterniond normalized
        {
            get { return Normalize(this); }
        }

        // used to allow Quaternions to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }

        // also required for being able to use Quaternions as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Quaternion)) return false;

            return Equals((Quaternion)other);
        }

        public bool Equals(Quaternion other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override string ToString()
        {
            //return UnityString.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", x, y, z, w);
            return new Quaternion(x, y, z, w).ToString();
        }

        public string ToString(string format)
        {
            //return UnityString.Format("({0}, {1}, {2}, {3})", x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format));
            return new Quaternion(x, y, z, w).ToString(format);
        }
    }
} //namespace
