using System;
using System.Collections.Generic;
using System.Text;

public struct KBQuaternion
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public static KBQuaternion Identity
    {
        get { return new KBQuaternion(0, 0, 0, 1); }
    }

    public bool IsIdentity
    {
        get { return X == 0f && Y == 0f && Z == 0f && W == 1f; }
    }

    public KBQuaternion(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    public KBQuaternion(KBVector3 vectorPart, float scalarPart)
    {
        X = vectorPart.X;
        Y = vectorPart.Y;
        Z = vectorPart.Z;
        W = scalarPart;
    }

    public float Length()
    {
        float ls = X * X + Y * Y + Z * Z + W * W;
        return KBMathDefine.Sqrt(ls);
    }

    public float LengthSquared()
    {
        return X * X + Y * Y + Z * Z + W * W;
    }

    public static KBQuaternion Normalize(KBQuaternion value)
    {
        KBQuaternion ans;

        float ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;

        float invNorm = 1.0f / KBMathDefine.Sqrt(ls);

        ans.X = value.X * invNorm;
        ans.Y = value.Y * invNorm;
        ans.Z = value.Z * invNorm;
        ans.W = value.W * invNorm;

        return ans;
    }

    public static KBQuaternion Conjugate(KBQuaternion value)
    {
        KBQuaternion ans;

        ans.X = -value.X;
        ans.Y = -value.Y;
        ans.Z = -value.Z;
        ans.W = value.W;

        return ans;
    }

    public static KBQuaternion Inverse(KBQuaternion value)
    {
        //  -1   (       a              -v       )
        // q   = ( -------------   ------------- )
        //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )

        KBQuaternion ans;

        float ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
        float invNorm = 1.0f / ls;

        ans.X = -value.X * invNorm;
        ans.Y = -value.Y * invNorm;
        ans.Z = -value.Z * invNorm;
        ans.W = value.W * invNorm;

        return ans;
    }

    public static KBQuaternion CreateFromAxisAngle(KBVector3 axis, float angle)
    {
        KBQuaternion ans;

        float halfAngle = angle * 0.5f;
        float s = KBMathDefine.Sin(halfAngle);
        float c = KBMathDefine.Cos(halfAngle);

        ans.X = axis.X * s;
        ans.Y = axis.Y * s;
        ans.Z = axis.Z * s;
        ans.W = c;

        return ans;
    }

    public static KBQuaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
        float sr, cr, sp, cp, sy, cy;

        float halfRoll = roll * 0.5f;
        sr = KBMathDefine.Sin(halfRoll);
        cr = KBMathDefine.Cos(halfRoll);

        float halfPitch = pitch * 0.5f;
        sp = KBMathDefine.Sin(halfPitch);
        cp = KBMathDefine.Cos(halfPitch);

        float halfYaw = yaw * 0.5f;
        sy = KBMathDefine.Sin(halfYaw);
        cy = KBMathDefine.Cos(halfYaw);

        KBQuaternion result;

        result.X = cy * sp * cr + sy * cp * sr;
        result.Y = sy * cp * cr - cy * sp * sr;
        result.Z = cy * cp * sr - sy * sp * cr;
        result.W = cy * cp * cr + sy * sp * sr;

        return result;
    }

    public static KBQuaternion CreateFromRotationMatrix(KBMatrix4x4 matrix)
    {
        float trace = matrix.M11 + matrix.M22 + matrix.M33;

        KBQuaternion q = new KBQuaternion();

        if (trace > 0.0f)
        {
            float s = KBMathDefine.Sqrt(trace + 1.0f);
            q.W = s * 0.5f;
            s = 0.5f / s;
            q.X = (matrix.M23 - matrix.M32) * s;
            q.Y = (matrix.M31 - matrix.M13) * s;
            q.Z = (matrix.M12 - matrix.M21) * s;
        }
        else
        {
            if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
            {
                float s = KBMathDefine.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                float invS = 0.5f / s;
                q.X = 0.5f * s;
                q.Y = (matrix.M12 + matrix.M21) * invS;
                q.Z = (matrix.M13 + matrix.M31) * invS;
                q.W = (matrix.M23 - matrix.M32) * invS;
            }
            else if (matrix.M22 > matrix.M33)
            {
                float s = KBMathDefine.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                float invS = 0.5f / s;
                q.X = (matrix.M21 + matrix.M12) * invS;
                q.Y = 0.5f * s;
                q.Z = (matrix.M32 + matrix.M23) * invS;
                q.W = (matrix.M31 - matrix.M13) * invS;
            }
            else
            {
                float s = KBMathDefine.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                float invS = 0.5f / s;
                q.X = (matrix.M31 + matrix.M13) * invS;
                q.Y = (matrix.M32 + matrix.M23) * invS;
                q.Z = 0.5f * s;
                q.W = (matrix.M12 - matrix.M21) * invS;
            }
        }

        return q;
    }

    public static float Dot(KBQuaternion quaternion1, KBQuaternion quaternion2)
    {
        return quaternion1.X * quaternion2.X +
               quaternion1.Y * quaternion2.Y +
               quaternion1.Z * quaternion2.Z +
               quaternion1.W * quaternion2.W;
    }

    public static KBQuaternion Slerp(KBQuaternion quaternion1, KBQuaternion quaternion2, float amount)
    {
        const float epsilon = 1e-6f;

        float t = amount;

        float cosOmega = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                         quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

        bool flip = false;

        if (cosOmega < 0.0f)
        {
            flip = true;
            cosOmega = -cosOmega;
        }

        float s1, s2;

        if (cosOmega > (1.0f - epsilon))
        {
            // Too close, do straight linear interpolation.
            s1 = 1.0f - t;
            s2 = (flip) ? -t : t;
        }
        else
        {
            float omega = KBMathDefine.Acos(cosOmega);
            float invSinOmega = 1 / KBMathDefine.Sin(omega);

            s1 = KBMathDefine.Sin((1.0f - t) * omega) * invSinOmega;
            s2 = (flip)
                ? -KBMathDefine.Sin(t * omega) * invSinOmega
                : KBMathDefine.Sin(t * omega) * invSinOmega;
        }

        KBQuaternion ans;

        ans.X = s1 * quaternion1.X + s2 * quaternion2.X;
        ans.Y = s1 * quaternion1.Y + s2 * quaternion2.Y;
        ans.Z = s1 * quaternion1.Z + s2 * quaternion2.Z;
        ans.W = s1 * quaternion1.W + s2 * quaternion2.W;

        return ans;
    }

    public static KBQuaternion Lerp(KBQuaternion quaternion1, KBQuaternion quaternion2, float amount)
    {
        float t = amount;
        float t1 = 1.0f - t;

        KBQuaternion r = new KBQuaternion();

        float dot = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                    quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

        if (dot >= 0.0f)
        {
            r.X = t1 * quaternion1.X + t * quaternion2.X;
            r.Y = t1 * quaternion1.Y + t * quaternion2.Y;
            r.Z = t1 * quaternion1.Z + t * quaternion2.Z;
            r.W = t1 * quaternion1.W + t * quaternion2.W;
        }
        else
        {
            r.X = t1 * quaternion1.X - t * quaternion2.X;
            r.Y = t1 * quaternion1.Y - t * quaternion2.Y;
            r.Z = t1 * quaternion1.Z - t * quaternion2.Z;
            r.W = t1 * quaternion1.W - t * quaternion2.W;
        }

        float ls = r.X * r.X + r.Y * r.Y + r.Z * r.Z + r.W * r.W;
        float invNorm = 1.0f / KBMathDefine.Sqrt(ls);

        r.X *= invNorm;
        r.Y *= invNorm;
        r.Z *= invNorm;
        r.W *= invNorm;

        return r;
    }

    public static KBQuaternion Concatenate(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        float q1x = value2.X;
        float q1y = value2.Y;
        float q1z = value2.Z;
        float q1w = value2.W;

        float q2x = value1.X;
        float q2y = value1.Y;
        float q2z = value1.Z;
        float q2w = value1.W;

        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        ans.X = q1x * q2w + q2x * q1w + cx;
        ans.Y = q1y * q2w + q2y * q1w + cy;
        ans.Z = q1z * q2w + q2z * q1w + cz;
        ans.W = q1w * q2w - dot;

        return ans;
    }

    public static KBQuaternion Negate(KBQuaternion value)
    {
        KBQuaternion ans;

        ans.X = -value.X;
        ans.Y = -value.Y;
        ans.Z = -value.Z;
        ans.W = -value.W;

        return ans;
    }

    public static KBQuaternion Add(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        ans.X = value1.X + value2.X;
        ans.Y = value1.Y + value2.Y;
        ans.Z = value1.Z + value2.Z;
        ans.W = value1.W + value2.W;

        return ans;
    }

    public static KBQuaternion Subtract(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        ans.X = value1.X - value2.X;
        ans.Y = value1.Y - value2.Y;
        ans.Z = value1.Z - value2.Z;
        ans.W = value1.W - value2.W;

        return ans;
    }

    public static KBQuaternion Multiply(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        float q1x = value1.X;
        float q1y = value1.Y;
        float q1z = value1.Z;
        float q1w = value1.W;

        float q2x = value2.X;
        float q2y = value2.Y;
        float q2z = value2.Z;
        float q2w = value2.W;

        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        ans.X = q1x * q2w + q2x * q1w + cx;
        ans.Y = q1y * q2w + q2y * q1w + cy;
        ans.Z = q1z * q2w + q2z * q1w + cz;
        ans.W = q1w * q2w - dot;

        return ans;
    }

    public static KBQuaternion Multiply(KBQuaternion value1, float value2)
    {
        KBQuaternion ans;

        ans.X = value1.X * value2;
        ans.Y = value1.Y * value2;
        ans.Z = value1.Z * value2;
        ans.W = value1.W * value2;

        return ans;
    }

    public static KBQuaternion Divide(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        float q1x = value1.X;
        float q1y = value1.Y;
        float q1z = value1.Z;
        float q1w = value1.W;

        float ls = value2.X * value2.X + value2.Y * value2.Y +
                   value2.Z * value2.Z + value2.W * value2.W;
        float invNorm = 1.0f / ls;

        float q2x = -value2.X * invNorm;
        float q2y = -value2.Y * invNorm;
        float q2z = -value2.Z * invNorm;
        float q2w = value2.W * invNorm;

        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        ans.X = q1x * q2w + q2x * q1w + cx;
        ans.Y = q1y * q2w + q2y * q1w + cy;
        ans.Z = q1z * q2w + q2z * q1w + cz;
        ans.W = q1w * q2w - dot;

        return ans;
    }

    public static KBQuaternion operator -(KBQuaternion value)
    {
        KBQuaternion ans;

        ans.X = -value.X;
        ans.Y = -value.Y;
        ans.Z = -value.Z;
        ans.W = -value.W;

        return ans;
    }

    public static KBQuaternion operator +(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        ans.X = value1.X + value2.X;
        ans.Y = value1.Y + value2.Y;
        ans.Z = value1.Z + value2.Z;
        ans.W = value1.W + value2.W;

        return ans;
    }

    public static KBQuaternion operator -(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        ans.X = value1.X - value2.X;
        ans.Y = value1.Y - value2.Y;
        ans.Z = value1.Z - value2.Z;
        ans.W = value1.W - value2.W;

        return ans;
    }

    public static KBQuaternion operator *(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        float q1x = value1.X;
        float q1y = value1.Y;
        float q1z = value1.Z;
        float q1w = value1.W;

        float q2x = value2.X;
        float q2y = value2.Y;
        float q2z = value2.Z;
        float q2w = value2.W;

        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        ans.X = q1x * q2w + q2x * q1w + cx;
        ans.Y = q1y * q2w + q2y * q1w + cy;
        ans.Z = q1z * q2w + q2z * q1w + cz;
        ans.W = q1w * q2w - dot;

        return ans;
    }

    public static KBQuaternion operator *(KBQuaternion value1, float value2)
    {
        KBQuaternion ans;

        ans.X = value1.X * value2;
        ans.Y = value1.Y * value2;
        ans.Z = value1.Z * value2;
        ans.W = value1.W * value2;

        return ans;
    }

    public static KBQuaternion operator /(KBQuaternion value1, KBQuaternion value2)
    {
        KBQuaternion ans;

        float q1x = value1.X;
        float q1y = value1.Y;
        float q1z = value1.Z;
        float q1w = value1.W;

        float ls = value2.X * value2.X + value2.Y * value2.Y +
                   value2.Z * value2.Z + value2.W * value2.W;
        float invNorm = 1.0f / ls;

        float q2x = -value2.X * invNorm;
        float q2y = -value2.Y * invNorm;
        float q2z = -value2.Z * invNorm;
        float q2w = value2.W * invNorm;

        float cx = q1y * q2z - q1z * q2y;
        float cy = q1z * q2x - q1x * q2z;
        float cz = q1x * q2y - q1y * q2x;

        float dot = q1x * q2x + q1y * q2y + q1z * q2z;

        ans.X = q1x * q2w + q2x * q1w + cx;
        ans.Y = q1y * q2w + q2y * q1w + cy;
        ans.Z = q1z * q2w + q2z * q1w + cz;
        ans.W = q1w * q2w - dot;

        return ans;
    }

    public static bool operator ==(KBQuaternion value1, KBQuaternion value2)
    {
        return (value1.X == value2.X &&
                value1.Y == value2.Y &&
                value1.Z == value2.Z &&
                value1.W == value2.W);
    }

    public static bool operator !=(KBQuaternion value1, KBQuaternion value2)
    {
        return (value1.X != value2.X ||
                value1.Y != value2.Y ||
                value1.Z != value2.Z ||
                value1.W != value2.W);
    }

    public bool Equals(KBQuaternion other)
    {
        return (X == other.X &&
                Y == other.Y &&
                Z == other.Z &&
                W == other.W);
    }

    public override bool Equals(object obj)
    {
        if (obj is KBQuaternion)
        {
            return Equals((KBQuaternion)obj);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return unchecked(X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode());
    }
}
