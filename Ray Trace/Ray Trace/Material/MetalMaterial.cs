using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MetalMaterial : Material
{
    public KBColor Albedo;

    public MetalMaterial(KBColor albedo)
    {
        Albedo = albedo;
    }

    public override bool Scatter(KBRay ray, IntersectParams intersectParams, ref KBColor attenuation, ref KBRay scatterRay)
    {
        KBVector3 reflected = KBVector3.Reflect(ray.Direction, intersectParams.Normal);
        scatterRay = new KBRay(intersectParams.Point, (reflected + KBMath3D.RandomDir_In_Unit_Sphere() * 0.2f).Normalize());
        attenuation = Albedo;
        return KBVector3.Dot(reflected, intersectParams.Normal) > 0.0f;
    }
}
