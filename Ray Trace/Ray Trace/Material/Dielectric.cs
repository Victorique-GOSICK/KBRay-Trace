using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dielectric : Material
{
    public override bool Scatter(KBRay ray, IntersectParams intersectParams, ref KBColor attenuation, ref KBRay scatterRay)
    {
        return base.Scatter(ray, intersectParams, ref attenuation, ref scatterRay);
    }
}
