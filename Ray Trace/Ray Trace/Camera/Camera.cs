using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Camera
{
    public Int32 PixelsWidth;
    public Int32 PixelsHeight;

    public virtual KBRay RayCast(float u, float v)
    {
        return new KBRay(KBVector3.ZERO, KBVector3.ZERO);
    }

}
