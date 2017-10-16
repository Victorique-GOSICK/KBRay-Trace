using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Material
{
    public virtual bool Scatter(KBRay ray, IntersectParams intersectParams, ref KBColor attenuation, ref KBRay scatterRay)
    {
        return true;
    }
}
