using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KBShape
{
    public Material Material;

    public virtual bool Intersect(KBRay ray, float minDistance, float maxDistance, ref IntersectParams intersectParams)
    {
        intersectParams = null;
        return true;
    }
}
