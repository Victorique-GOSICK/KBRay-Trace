using System;
using System.Collections.Generic;
using System.Text;

//矩阵左乘 a*A 行矩阵
//左手坐标系
public struct KBMatrix4x4
{
    public float M11;
    public float M12;
    public float M13;
    public float M14;
    public float M21;
    public float M22;
    public float M23;
    public float M24;
    public float M31;
    public float M32;
    public float M33;
    public float M34;
    public float M41;
    public float M42;
    public float M43;
    public float M44;

    public static KBMatrix4x4 Identity
    {
        get { return _identity; }
    }

    public bool IsIdentity
    {
        get
        {
            return M11 == 1f && M22 == 1f && M33 == 1f && M44 == 1f &&
                   M12 == 0f && M13 == 0f && M14 == 0f &&
                   M21 == 0f && M23 == 0f && M24 == 0f &&
                   M31 == 0f && M32 == 0f && M34 == 0f &&
                   M41 == 0f && M42 == 0f && M43 == 0f;
        }
    }

    public KBMatrix4x4(float m11, float m12, float m13, float m14,
                       float m21, float m22, float m23, float m24,
                       float m31, float m32, float m33, float m34,
                       float m41, float m42, float m43, float m44)
    {
        this.M11 = m11;
        this.M12 = m12;
        this.M13 = m13;
        this.M14 = m14;

        this.M21 = m21;
        this.M22 = m22;
        this.M23 = m23;
        this.M24 = m24;

        this.M31 = m31;
        this.M32 = m32;
        this.M33 = m33;
        this.M34 = m34;

        this.M41 = m41;
        this.M42 = m42;
        this.M43 = m43;
        this.M44 = m44;
    }

    //行列式
    public float GetDeterminant()
    {
        // | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
        // | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
        // | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
        // | m n o p |
        //
        //   | f g h |
        // a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
        //   | n o p |
        //
        //   | e g h |     
        // b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
        //   | m o p |     
        //
        //   | e f h |
        // c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
        //   | m n p |
        //
        //   | e f g |
        // d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
        //   | m n o |
        //
        // Cost of operation
        // 17 adds and 28 muls.
        //
        // add: 6 + 8 + 3 = 17
        // mul: 12 + 16 = 28

        float a = M11, b = M12, c = M13, d = M14;
        float e = M21, f = M22, g = M23, h = M24;
        float i = M31, j = M32, k = M33, l = M34;
        float m = M41, n = M42, o = M43, p = M44;

        float kp_lo = k * p - l * o;
        float jp_ln = j * p - l * n;
        float jo_kn = j * o - k * n;
        float ip_lm = i * p - l * m;
        float io_km = i * o - k * m;
        float in_jm = i * n - j * m;

        return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
               b * (e * kp_lo - g * ip_lm + h * io_km) +
               c * (e * jp_ln - f * ip_lm + h * in_jm) -
               d * (e * jo_kn - f * io_km + g * in_jm);
    }

    public static KBMatrix4x4 CreateScale(float xScale, float yScale, float zScale)
    {
        KBMatrix4x4 result;

        result.M11 = xScale;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = yScale;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = zScale;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateScale(float xScale, float yScale, float zScale, KBVector3 centerPoint)
    {
        KBMatrix4x4 result;

        float tx = centerPoint.X * (1 - xScale);
        float ty = centerPoint.Y * (1 - yScale);
        float tz = centerPoint.Z * (1 - zScale);

        result.M11 = xScale;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = yScale;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = zScale;
        result.M34 = 0.0f;
        result.M41 = tx;
        result.M42 = ty;
        result.M43 = tz;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateScale(float scale)
    {
        KBMatrix4x4 result;

        result.M11 = scale;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = scale;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = scale;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateScale(float scale, KBVector3 centerPoint)
    {
        KBMatrix4x4 result;

        float tx = centerPoint.X * (1 - scale);
        float ty = centerPoint.Y * (1 - scale);
        float tz = centerPoint.Z * (1 - scale);

        result.M11 = scale;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = scale;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = scale;
        result.M34 = 0.0f;
        result.M41 = tx;
        result.M42 = ty;
        result.M43 = tz;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateScale(KBVector3 scales)
    {
        KBMatrix4x4 result;

        result.M11 = scales.X;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = scales.Y;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = scales.Z;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateScale(KBVector3 scales, KBVector3 centerPoint)
    {
        KBMatrix4x4 result;

        float tx = centerPoint.X * (1 - scales.X);
        float ty = centerPoint.Y * (1 - scales.Y);
        float tz = centerPoint.Z * (1 - scales.Z);

        result.M11 = scales.X;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = scales.Y;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = scales.Z;
        result.M34 = 0.0f;
        result.M41 = tx;
        result.M42 = ty;
        result.M43 = tz;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateTranslation(KBVector3 position)
    {
        KBMatrix4x4 result;

        result.M11 = 1.0f;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = 1.0f;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = 1.0f;
        result.M34 = 0.0f;

        result.M41 = position.X;
        result.M42 = position.Y;
        result.M43 = position.Z;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateTranslation(float xPosition, float yPosition, float zPosition)
    {
        KBMatrix4x4 result;

        result.M11 = 1.0f;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = 1.0f;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = 1.0f;
        result.M34 = 0.0f;

        result.M41 = xPosition;
        result.M42 = yPosition;
        result.M43 = zPosition;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateRotationX(float radians)
    {
        KBMatrix4x4 result;

        float c = (float)KBMathDefine.Cos(radians);
        float s = (float)KBMathDefine.Sin(radians);

        // [  1  0  0  0 ]
        // [  0  c  s  0 ]
        // [  0 -s  c  0 ]
        // [  0  0  0  1 ]
        result.M11 = 1.0f;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = c;
        result.M23 = s;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = -s;
        result.M33 = c;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateRotationX(float radians, KBVector3 centerPoint)
    {
        KBMatrix4x4 result;

        float c = (float)KBMathDefine.Cos(radians);
        float s = (float)KBMathDefine.Sin(radians);

        float y = centerPoint.Y * (1 - c) + centerPoint.Z * s;
        float z = centerPoint.Z * (1 - c) - centerPoint.Y * s;

        // [  1  0  0  0 ]
        // [  0  c  s  0 ]
        // [  0 -s  c  0 ]
        // [  0  y  z  1 ]
        result.M11 = 1.0f;
        result.M12 = 0.0f;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = c;
        result.M23 = s;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = -s;
        result.M33 = c;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = y;
        result.M43 = z;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateRotationY(float radians)
    {
        KBMatrix4x4 result;

        float c = (float)KBMathDefine.Cos(radians);
        float s = (float)KBMathDefine.Sin(radians);

        // [  c  0 -s  0 ]
        // [  0  1  0  0 ]
        // [  s  0  c  0 ]
        // [  0  0  0  1 ]
        result.M11 = c;
        result.M12 = 0.0f;
        result.M13 = -s;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = 1.0f;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = s;
        result.M32 = 0.0f;
        result.M33 = c;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateRotationY(float radians, KBVector3 centerPoint)
    {
        KBMatrix4x4 result;

        float c = (float)KBMathDefine.Cos(radians);
        float s = (float)KBMathDefine.Sin(radians);

        float x = centerPoint.X * (1 - c) - centerPoint.Z * s;
        float z = centerPoint.Z * (1 - c) + centerPoint.X * s;

        // [  c  0 -s  0 ]
        // [  0  1  0  0 ]
        // [  s  0  c  0 ]
        // [  x  0  z  1 ]
        result.M11 = c;
        result.M12 = 0.0f;
        result.M13 = -s;
        result.M14 = 0.0f;
        result.M21 = 0.0f;
        result.M22 = 1.0f;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = s;
        result.M32 = 0.0f;
        result.M33 = c;
        result.M34 = 0.0f;
        result.M41 = x;
        result.M42 = 0.0f;
        result.M43 = z;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateRotationZ(float radians)
    {
        KBMatrix4x4 result;

        float c = (float)KBMathDefine.Cos(radians);
        float s = (float)KBMathDefine.Sin(radians);

        // [  c  s  0  0 ]
        // [ -s  c  0  0 ]
        // [  0  0  1  0 ]
        // [  0  0  0  1 ]
        result.M11 = c;
        result.M12 = s;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = -s;
        result.M22 = c;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = 1.0f;
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateRotationZ(float radians, KBVector3 centerPoint)
    {
        KBMatrix4x4 result;

        float c = (float)KBMathDefine.Cos(radians);
        float s = (float)KBMathDefine.Sin(radians);

        float x = centerPoint.X * (1 - c) + centerPoint.Y * s;
        float y = centerPoint.Y * (1 - c) - centerPoint.X * s;

        // [  c  s  0  0 ]
        // [ -s  c  0  0 ]
        // [  0  0  1  0 ]
        // [  x  y  0  1 ]
        result.M11 = c;
        result.M12 = s;
        result.M13 = 0.0f;
        result.M14 = 0.0f;
        result.M21 = -s;
        result.M22 = c;
        result.M23 = 0.0f;
        result.M24 = 0.0f;
        result.M31 = 0.0f;
        result.M32 = 0.0f;
        result.M33 = 1.0f;
        result.M34 = 0.0f;
        result.M41 = x;
        result.M42 = y;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBVector3 Multiply(KBMatrix4x4 matrix, KBVector3 pos)
    {
        float x = matrix.M11 * pos.X + matrix.M21 * pos.Y + matrix.M31 * pos.Z + matrix.M41;
        float y = matrix.M12 * pos.X + matrix.M22 * pos.Y + matrix.M32 * pos.Z + matrix.M42;
        float z = matrix.M13 * pos.X + matrix.M23 * pos.Y + matrix.M33 * pos.Z + matrix.M43;
        //
        return new KBVector3(x, y, z);
    }

    public static KBMatrix4x4 CreateFromAxisAngle(KBVector3 axis, float angle)
    {
        // a: angle
        // x, y, z: unit vector for axis.
        //
        // Rotation matrix M can compute by using below equation.
        //
        //        T               T
        //  M = uu + (cos a)( I-uu ) + (sin a)S
        //
        // Where:
        //
        //  u = ( x, y, z )
        //
        //      [  0 -z  y ]
        //  S = [  z  0 -x ]
        //      [ -y  x  0 ]
        //
        //      [ 1 0 0 ]
        //  I = [ 0 1 0 ]
        //      [ 0 0 1 ]
        //
        //
        //     [  xx+cosa*(1-xx)   yx-cosa*yx-sina*z zx-cosa*xz+sina*y ]
        // M = [ xy-cosa*yx+sina*z    yy+cosa(1-yy)  yz-cosa*yz-sina*x ]
        //     [ zx-cosa*zx-sina*y zy-cosa*zy+sina*x   zz+cosa*(1-zz)  ]
        //
        float x = axis.X, y = axis.Y, z = axis.Z;
        float sa = (float)KBMathDefine.Sin(angle), ca = (float)KBMathDefine.Cos(angle);
        float xx = x * x, yy = y * y, zz = z * z;
        float xy = x * y, xz = x * z, yz = y * z;

        KBMatrix4x4 result;

        result.M11 = xx + ca * (1.0f - xx);
        result.M12 = xy - ca * xy + sa * z;
        result.M13 = xz - ca * xz - sa * y;
        result.M14 = 0.0f;
        result.M21 = xy - ca * xy - sa * z;
        result.M22 = yy + ca * (1.0f - yy);
        result.M23 = yz - ca * yz + sa * x;
        result.M24 = 0.0f;
        result.M31 = xz - ca * xz + sa * y;
        result.M32 = yz - ca * yz - sa * x;
        result.M33 = zz + ca * (1.0f - zz);
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static bool Invert(KBMatrix4x4 matrix, out KBMatrix4x4 result)
    {
        //                                       -1
        // If you have matrix M, inverse Matrix M   can compute
        //
        //     -1       1      
        //    M   = --------- A
        //            det(M)
        //
        // A is adjugate (adjoint) of M, where,
        //
        //      T
        // A = C
        //
        // C is Cofactor matrix of M, where,
        //           i + j
        // C   = (-1)      * det(M  )
        //  ij                    ij
        //
        //     [ a b c d ]
        // M = [ e f g h ]
        //     [ i j k l ]
        //     [ m n o p ]
        //
        // First Row
        //           2 | f g h |
        // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
        //  11         | n o p |
        //
        //           3 | e g h |
        // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
        //  12         | m o p |
        //
        //           4 | e f h |
        // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
        //  13         | m n p |
        //
        //           5 | e f g |
        // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
        //  14         | m n o |
        //
        // Second Row
        //           3 | b c d |
        // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
        //  21         | n o p |
        //
        //           4 | a c d |
        // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
        //  22         | m o p |
        //
        //           5 | a b d |
        // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
        //  23         | m n p |
        //
        //           6 | a b c |
        // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
        //  24         | m n o |
        //
        // Third Row
        //           4 | b c d |
        // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
        //  31         | n o p |
        //
        //           5 | a c d |
        // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
        //  32         | m o p |
        //
        //           6 | a b d |
        // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
        //  33         | m n p |
        //
        //           7 | a b c |
        // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
        //  34         | m n o |
        //
        // Fourth Row
        //           5 | b c d |
        // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
        //  41         | j k l |
        //
        //           6 | a c d |
        // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
        //  42         | i k l |
        //
        //           7 | a b d |
        // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
        //  43         | i j l |
        //
        //           8 | a b c |
        // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
        //  44         | i j k |
        //
        // Cost of operation
        // 53 adds, 104 muls, and 1 div.
        float a = matrix.M11, b = matrix.M12, c = matrix.M13, d = matrix.M14;
        float e = matrix.M21, f = matrix.M22, g = matrix.M23, h = matrix.M24;
        float i = matrix.M31, j = matrix.M32, k = matrix.M33, l = matrix.M34;
        float m = matrix.M41, n = matrix.M42, o = matrix.M43, p = matrix.M44;

        float kp_lo = k * p - l * o;
        float jp_ln = j * p - l * n;
        float jo_kn = j * o - k * n;
        float ip_lm = i * p - l * m;
        float io_km = i * o - k * m;
        float in_jm = i * n - j * m;

        float a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
        float a12 = -(e * kp_lo - g * ip_lm + h * io_km);
        float a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
        float a14 = -(e * jo_kn - f * io_km + g * in_jm);

        float det = a * a11 + b * a12 + c * a13 + d * a14;

        if (KBMathDefine.Abs(det) < float.Epsilon)
        {
            result = new KBMatrix4x4(float.NaN, float.NaN, float.NaN, float.NaN,
                                   float.NaN, float.NaN, float.NaN, float.NaN,
                                   float.NaN, float.NaN, float.NaN, float.NaN,
                                   float.NaN, float.NaN, float.NaN, float.NaN);
            return false;
        }

        float invDet = 1.0f / det;

        result.M11 = a11 * invDet;
        result.M21 = a12 * invDet;
        result.M31 = a13 * invDet;
        result.M41 = a14 * invDet;

        result.M12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
        result.M22 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
        result.M32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
        result.M42 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

        float gp_ho = g * p - h * o;
        float fp_hn = f * p - h * n;
        float fo_gn = f * o - g * n;
        float ep_hm = e * p - h * m;
        float eo_gm = e * o - g * m;
        float en_fm = e * n - f * m;

        result.M13 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
        result.M23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
        result.M33 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
        result.M43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

        float gl_hk = g * l - h * k;
        float fl_hj = f * l - h * j;
        float fk_gj = f * k - g * j;
        float el_hi = e * l - h * i;
        float ek_gi = e * k - g * i;
        float ej_fi = e * j - f * i;

        result.M14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
        result.M24 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
        result.M34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
        result.M44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

        return true;
    }

    public static KBMatrix4x4 Transpose(KBMatrix4x4 matrix)
    {
        KBMatrix4x4 result;

        result.M11 = matrix.M11;
        result.M12 = matrix.M21;
        result.M13 = matrix.M31;
        result.M14 = matrix.M41;
        result.M21 = matrix.M12;
        result.M22 = matrix.M22;
        result.M23 = matrix.M32;
        result.M24 = matrix.M42;
        result.M31 = matrix.M13;
        result.M32 = matrix.M23;
        result.M33 = matrix.M33;
        result.M34 = matrix.M43;
        result.M41 = matrix.M14;
        result.M42 = matrix.M24;
        result.M43 = matrix.M34;
        result.M44 = matrix.M44;

        return result;
    }

    public static KBMatrix4x4 CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
    {
        if (nearPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        if (farPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

        if (nearPlaneDistance >= farPlaneDistance)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        KBMatrix4x4 result;

        result.M11 = 2.0f * nearPlaneDistance / width;
        result.M12 = result.M13 = result.M14 = 0.0f;

        result.M22 = 2.0f * nearPlaneDistance / height;
        result.M21 = result.M23 = result.M24 = 0.0f;

        var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
        result.M33 = negFarRange;
        result.M31 = result.M32 = 0.0f;
        result.M34 = -1.0f;

        result.M41 = result.M42 = result.M44 = 0.0f;
        result.M43 = nearPlaneDistance * negFarRange;

        return result;
    }

    public static KBMatrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
    {
        if (fieldOfView <= 0.0f || fieldOfView >= KBMathDefine.PI)
            throw new ArgumentOutOfRangeException(nameof(fieldOfView));

        if (nearPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        if (farPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

        if (nearPlaneDistance >= farPlaneDistance)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        float yScale = 1.0f / KBMathDefine.Tan(fieldOfView * 0.5f);
        float xScale = yScale / aspectRatio;

        KBMatrix4x4 result;

        result.M11 = xScale;
        result.M12 = result.M13 = result.M14 = 0.0f;

        result.M22 = yScale;
        result.M21 = result.M23 = result.M24 = 0.0f;

        result.M31 = result.M32 = 0.0f;
        var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
        result.M33 = negFarRange;
        result.M34 = -1.0f;

        result.M41 = result.M42 = result.M44 = 0.0f;
        result.M43 = nearPlaneDistance * negFarRange;

        return result;
    }

    public static KBMatrix4x4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
    {
        if (nearPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        if (farPlaneDistance <= 0.0f)
            throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

        if (nearPlaneDistance >= farPlaneDistance)
            throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

        KBMatrix4x4 result;

        result.M11 = 2.0f * nearPlaneDistance / (right - left);
        result.M12 = result.M13 = result.M14 = 0.0f;

        result.M22 = 2.0f * nearPlaneDistance / (top - bottom);
        result.M21 = result.M23 = result.M24 = 0.0f;

        result.M31 = (left + right) / (right - left);
        result.M32 = (top + bottom) / (top - bottom);
        var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
        result.M33 = negFarRange;
        result.M34 = -1.0f;

        result.M43 = nearPlaneDistance * negFarRange;
        result.M41 = result.M42 = result.M44 = 0.0f;

        return result;
    }

    public static KBMatrix4x4 CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
    {
        KBMatrix4x4 result;

        result.M11 = 2.0f / width;
        result.M12 = result.M13 = result.M14 = 0.0f;

        result.M22 = 2.0f / height;
        result.M21 = result.M23 = result.M24 = 0.0f;

        result.M33 = 1.0f / (zNearPlane - zFarPlane);
        result.M31 = result.M32 = result.M34 = 0.0f;

        result.M41 = result.M42 = 0.0f;
        result.M43 = zNearPlane / (zNearPlane - zFarPlane);
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
    {
        KBMatrix4x4 result;

        result.M11 = 2.0f / (right - left);
        result.M12 = result.M13 = result.M14 = 0.0f;

        result.M22 = 2.0f / (top - bottom);
        result.M21 = result.M23 = result.M24 = 0.0f;

        result.M33 = 1.0f / (zNearPlane - zFarPlane);
        result.M31 = result.M32 = result.M34 = 0.0f;

        result.M41 = (left + right) / (left - right);
        result.M42 = (top + bottom) / (bottom - top);
        result.M43 = zNearPlane / (zNearPlane - zFarPlane);
        result.M44 = 1.0f;

        return result;
    }

    //public static KBMatrix4x4 CreateShadow(KBVector3 lightDirection, KBPlane plane)
    //{
    //    KBPlane p = KBPlane.Normalize(plane);

    //    float dot = p.Normal.X * lightDirection.X + p.Normal.Y * lightDirection.Y + p.Normal.Z * lightDirection.Z;
    //    float a = -p.Normal.X;
    //    float b = -p.Normal.Y;
    //    float c = -p.Normal.Z;
    //    float d = -p.D;

    //    KBMatrix4x4 result;

    //    result.M11 = a * lightDirection.X + dot;
    //    result.M21 = b * lightDirection.X;
    //    result.M31 = c * lightDirection.X;
    //    result.M41 = d * lightDirection.X;

    //    result.M12 = a * lightDirection.Y;
    //    result.M22 = b * lightDirection.Y + dot;
    //    result.M32 = c * lightDirection.Y;
    //    result.M42 = d * lightDirection.Y;

    //    result.M13 = a * lightDirection.Z;
    //    result.M23 = b * lightDirection.Z;
    //    result.M33 = c * lightDirection.Z + dot;
    //    result.M43 = d * lightDirection.Z;

    //    result.M14 = 0.0f;
    //    result.M24 = 0.0f;
    //    result.M34 = 0.0f;
    //    result.M44 = dot;

    //    return result;
    //}

    //public static KBMatrix4x4 CreateReflection(KBPlane value)
    //{
    //    value = KBPlane.Normalize(value);

    //    float a = value.Normal.X;
    //    float b = value.Normal.Y;
    //    float c = value.Normal.Z;

    //    float fa = -2.0f * a;
    //    float fb = -2.0f * b;
    //    float fc = -2.0f * c;

    //    KBMatrix4x4 result;

    //    result.M11 = fa * a + 1.0f;
    //    result.M12 = fb * a;
    //    result.M13 = fc * a;
    //    result.M14 = 0.0f;

    //    result.M21 = fa * b;
    //    result.M22 = fb * b + 1.0f;
    //    result.M23 = fc * b;
    //    result.M24 = 0.0f;

    //    result.M31 = fa * c;
    //    result.M32 = fb * c;
    //    result.M33 = fc * c + 1.0f;
    //    result.M34 = 0.0f;

    //    result.M41 = fa * value.D;
    //    result.M42 = fb * value.D;
    //    result.M43 = fc * value.D;
    //    result.M44 = 1.0f;

    //    return result;
    //}

    public static KBMatrix4x4 CreateLookAt(KBVector3 cameraPosition, KBVector3 cameraTarget, KBVector3 cameraUpVector)
    {
        KBVector3 zaxis = KBVector3.Normalize(cameraPosition - cameraTarget);
        KBVector3 xaxis = KBVector3.Normalize(KBVector3.Cross(cameraUpVector, zaxis));
        KBVector3 yaxis = KBVector3.Cross(zaxis, xaxis);

        KBMatrix4x4 result;

        result.M11 = xaxis.X;
        result.M12 = yaxis.X;
        result.M13 = zaxis.X;
        result.M14 = 0.0f;
        result.M21 = xaxis.Y;
        result.M22 = yaxis.Y;
        result.M23 = zaxis.Y;
        result.M24 = 0.0f;
        result.M31 = xaxis.Z;
        result.M32 = yaxis.Z;
        result.M33 = zaxis.Z;
        result.M34 = 0.0f;
        result.M41 = -KBVector3.Dot(xaxis, cameraPosition);
        result.M42 = -KBVector3.Dot(yaxis, cameraPosition);
        result.M43 = -KBVector3.Dot(zaxis, cameraPosition);
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateWorld(KBVector3 position, KBVector3 forward, KBVector3 up)
    {
        KBVector3 zaxis = KBVector3.Normalize(-forward);
        KBVector3 xaxis = KBVector3.Normalize(KBVector3.Cross(up, zaxis));
        KBVector3 yaxis = KBVector3.Cross(zaxis, xaxis);

        KBMatrix4x4 result;

        result.M11 = xaxis.X;
        result.M12 = xaxis.Y;
        result.M13 = xaxis.Z;
        result.M14 = 0.0f;
        result.M21 = yaxis.X;
        result.M22 = yaxis.Y;
        result.M23 = yaxis.Z;
        result.M24 = 0.0f;
        result.M31 = zaxis.X;
        result.M32 = zaxis.Y;
        result.M33 = zaxis.Z;
        result.M34 = 0.0f;
        result.M41 = position.X;
        result.M42 = position.Y;
        result.M43 = position.Z;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateFromQuaternion(KBQuaternion quaternion)
    {
        KBMatrix4x4 result;

        float xx = quaternion.X * quaternion.X;
        float yy = quaternion.Y * quaternion.Y;
        float zz = quaternion.Z * quaternion.Z;

        float xy = quaternion.X * quaternion.Y;
        float wz = quaternion.Z * quaternion.W;
        float xz = quaternion.Z * quaternion.X;
        float wy = quaternion.Y * quaternion.W;
        float yz = quaternion.Y * quaternion.Z;
        float wx = quaternion.X * quaternion.W;

        result.M11 = 1.0f - 2.0f * (yy + zz);
        result.M12 = 2.0f * (xy + wz);
        result.M13 = 2.0f * (xz - wy);
        result.M14 = 0.0f;
        result.M21 = 2.0f * (xy - wz);
        result.M22 = 1.0f - 2.0f * (zz + xx);
        result.M23 = 2.0f * (yz + wx);
        result.M24 = 0.0f;
        result.M31 = 2.0f * (xz + wy);
        result.M32 = 2.0f * (yz - wx);
        result.M33 = 1.0f - 2.0f * (yy + xx);
        result.M34 = 0.0f;
        result.M41 = 0.0f;
        result.M42 = 0.0f;
        result.M43 = 0.0f;
        result.M44 = 1.0f;

        return result;
    }

    public static KBMatrix4x4 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
    {
        KBQuaternion q = KBQuaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
        return KBMatrix4x4.CreateFromQuaternion(q);
    }

    public static KBMatrix4x4 Lerp(KBMatrix4x4 matrix1, KBMatrix4x4 matrix2, float amount)
    {
        KBMatrix4x4 result;

        result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
        result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
        result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
        result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;

        result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
        result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
        result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
        result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;

        result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
        result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
        result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
        result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;

        result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
        result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
        result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
        result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;

        return result;
    }

    public static KBMatrix4x4 Negate(KBMatrix4x4 value)
    {
        KBMatrix4x4 result;

        result.M11 = -value.M11;
        result.M12 = -value.M12;
        result.M13 = -value.M13;
        result.M14 = -value.M14;
        result.M21 = -value.M21;
        result.M22 = -value.M22;
        result.M23 = -value.M23;
        result.M24 = -value.M24;
        result.M31 = -value.M31;
        result.M32 = -value.M32;
        result.M33 = -value.M33;
        result.M34 = -value.M34;
        result.M41 = -value.M41;
        result.M42 = -value.M42;
        result.M43 = -value.M43;
        result.M44 = -value.M44;

        return result;
    }

    public static KBMatrix4x4 Add(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        KBMatrix4x4 result;

        result.M11 = value1.M11 + value2.M11;
        result.M12 = value1.M12 + value2.M12;
        result.M13 = value1.M13 + value2.M13;
        result.M14 = value1.M14 + value2.M14;
        result.M21 = value1.M21 + value2.M21;
        result.M22 = value1.M22 + value2.M22;
        result.M23 = value1.M23 + value2.M23;
        result.M24 = value1.M24 + value2.M24;
        result.M31 = value1.M31 + value2.M31;
        result.M32 = value1.M32 + value2.M32;
        result.M33 = value1.M33 + value2.M33;
        result.M34 = value1.M34 + value2.M34;
        result.M41 = value1.M41 + value2.M41;
        result.M42 = value1.M42 + value2.M42;
        result.M43 = value1.M43 + value2.M43;
        result.M44 = value1.M44 + value2.M44;

        return result;
    }

    public static KBMatrix4x4 Subtract(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        KBMatrix4x4 result;

        result.M11 = value1.M11 - value2.M11;
        result.M12 = value1.M12 - value2.M12;
        result.M13 = value1.M13 - value2.M13;
        result.M14 = value1.M14 - value2.M14;
        result.M21 = value1.M21 - value2.M21;
        result.M22 = value1.M22 - value2.M22;
        result.M23 = value1.M23 - value2.M23;
        result.M24 = value1.M24 - value2.M24;
        result.M31 = value1.M31 - value2.M31;
        result.M32 = value1.M32 - value2.M32;
        result.M33 = value1.M33 - value2.M33;
        result.M34 = value1.M34 - value2.M34;
        result.M41 = value1.M41 - value2.M41;
        result.M42 = value1.M42 - value2.M42;
        result.M43 = value1.M43 - value2.M43;
        result.M44 = value1.M44 - value2.M44;

        return result;
    }

    public static KBMatrix4x4 Multiply(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        KBMatrix4x4 result;

        result.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
        result.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
        result.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
        result.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;

        result.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
        result.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
        result.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
        result.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;

        result.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
        result.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
        result.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
        result.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;

        result.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
        result.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
        result.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
        result.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;

        return result;
    }

    public static KBMatrix4x4 Multiply(KBMatrix4x4 value1, float value2)
    {
        KBMatrix4x4 result;

        result.M11 = value1.M11 * value2;
        result.M12 = value1.M12 * value2;
        result.M13 = value1.M13 * value2;
        result.M14 = value1.M14 * value2;
        result.M21 = value1.M21 * value2;
        result.M22 = value1.M22 * value2;
        result.M23 = value1.M23 * value2;
        result.M24 = value1.M24 * value2;
        result.M31 = value1.M31 * value2;
        result.M32 = value1.M32 * value2;
        result.M33 = value1.M33 * value2;
        result.M34 = value1.M34 * value2;
        result.M41 = value1.M41 * value2;
        result.M42 = value1.M42 * value2;
        result.M43 = value1.M43 * value2;
        result.M44 = value1.M44 * value2;

        return result;
    }

    public static KBMatrix4x4 operator -(KBMatrix4x4 value)
    {
        KBMatrix4x4 m;

        m.M11 = -value.M11;
        m.M12 = -value.M12;
        m.M13 = -value.M13;
        m.M14 = -value.M14;
        m.M21 = -value.M21;
        m.M22 = -value.M22;
        m.M23 = -value.M23;
        m.M24 = -value.M24;
        m.M31 = -value.M31;
        m.M32 = -value.M32;
        m.M33 = -value.M33;
        m.M34 = -value.M34;
        m.M41 = -value.M41;
        m.M42 = -value.M42;
        m.M43 = -value.M43;
        m.M44 = -value.M44;

        return m;
    }

    public static KBMatrix4x4 operator +(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        KBMatrix4x4 m;

        m.M11 = value1.M11 + value2.M11;
        m.M12 = value1.M12 + value2.M12;
        m.M13 = value1.M13 + value2.M13;
        m.M14 = value1.M14 + value2.M14;
        m.M21 = value1.M21 + value2.M21;
        m.M22 = value1.M22 + value2.M22;
        m.M23 = value1.M23 + value2.M23;
        m.M24 = value1.M24 + value2.M24;
        m.M31 = value1.M31 + value2.M31;
        m.M32 = value1.M32 + value2.M32;
        m.M33 = value1.M33 + value2.M33;
        m.M34 = value1.M34 + value2.M34;
        m.M41 = value1.M41 + value2.M41;
        m.M42 = value1.M42 + value2.M42;
        m.M43 = value1.M43 + value2.M43;
        m.M44 = value1.M44 + value2.M44;

        return m;
    }

    public static KBMatrix4x4 operator -(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        KBMatrix4x4 m;

        m.M11 = value1.M11 - value2.M11;
        m.M12 = value1.M12 - value2.M12;
        m.M13 = value1.M13 - value2.M13;
        m.M14 = value1.M14 - value2.M14;
        m.M21 = value1.M21 - value2.M21;
        m.M22 = value1.M22 - value2.M22;
        m.M23 = value1.M23 - value2.M23;
        m.M24 = value1.M24 - value2.M24;
        m.M31 = value1.M31 - value2.M31;
        m.M32 = value1.M32 - value2.M32;
        m.M33 = value1.M33 - value2.M33;
        m.M34 = value1.M34 - value2.M34;
        m.M41 = value1.M41 - value2.M41;
        m.M42 = value1.M42 - value2.M42;
        m.M43 = value1.M43 - value2.M43;
        m.M44 = value1.M44 - value2.M44;

        return m;
    }

    public static KBMatrix4x4 operator *(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        KBMatrix4x4 m;

        m.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
        m.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
        m.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
        m.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;

        m.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
        m.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
        m.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
        m.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;

        m.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
        m.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
        m.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
        m.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;

        m.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
        m.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
        m.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
        m.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;

        return m;
    }

    public static KBMatrix4x4 operator *(KBMatrix4x4 value1, float value2)
    {
        KBMatrix4x4 m;

        m.M11 = value1.M11 * value2;
        m.M12 = value1.M12 * value2;
        m.M13 = value1.M13 * value2;
        m.M14 = value1.M14 * value2;
        m.M21 = value1.M21 * value2;
        m.M22 = value1.M22 * value2;
        m.M23 = value1.M23 * value2;
        m.M24 = value1.M24 * value2;
        m.M31 = value1.M31 * value2;
        m.M32 = value1.M32 * value2;
        m.M33 = value1.M33 * value2;
        m.M34 = value1.M34 * value2;
        m.M41 = value1.M41 * value2;
        m.M42 = value1.M42 * value2;
        m.M43 = value1.M43 * value2;
        m.M44 = value1.M44 * value2;
        return m;
    }

    public static bool operator ==(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        return (value1.M11 == value2.M11 && value1.M22 == value2.M22 && value1.M33 == value2.M33 && value1.M44 == value2.M44 &&
                                            value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 &&
                value1.M21 == value2.M21 && value1.M23 == value2.M23 && value1.M24 == value2.M24 &&
                value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M34 == value2.M34 &&
                value1.M41 == value2.M41 && value1.M42 == value2.M42 && value1.M43 == value2.M43);
    }

    public static bool operator !=(KBMatrix4x4 value1, KBMatrix4x4 value2)
    {
        return (value1.M11 != value2.M11 || value1.M12 != value2.M12 || value1.M13 != value2.M13 || value1.M14 != value2.M14 ||
                value1.M21 != value2.M21 || value1.M22 != value2.M22 || value1.M23 != value2.M23 || value1.M24 != value2.M24 ||
                value1.M31 != value2.M31 || value1.M32 != value2.M32 || value1.M33 != value2.M33 || value1.M34 != value2.M34 ||
                value1.M41 != value2.M41 || value1.M42 != value2.M42 || value1.M43 != value2.M43 || value1.M44 != value2.M44);
    }

    private static readonly KBMatrix4x4 _identity = new KBMatrix4x4
      (
          1f, 0f, 0f, 0f,
          0f, 1f, 0f, 0f,
          0f, 0f, 1f, 0f,
          0f, 0f, 0f, 1f
      );

}
