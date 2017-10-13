using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PerspectiveCamera : Camera
{
    KBVector3 OriginPos;
    KBVector3 Lower_left_corner;
    KBVector3 Horizontal;
    KBVector3 Vertical;

    public PerspectiveCamera()
    {
        OriginPos = KBVector3.ZERO;
        Lower_left_corner = new KBVector3(-2.0f, -1.0f, -1.0f);
        Horizontal = new KBVector3(4.0f, 0.0f, 0.0f);
        Vertical = new KBVector3(0.0f, 2.0f, 0.0f);
    }

    public override KBRay RayCast(float u, float v)
    {
        return new KBRay(OriginPos, (Lower_left_corner + u * Horizontal + v * Vertical).Normalize());
    }
}
