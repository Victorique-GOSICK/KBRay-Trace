//http://learnopengl-cn.readthedocs.io/zh/latest/05%20Advanced%20Lighting/04%20Normal%20Mapping/
//http://www.cnblogs.com/lookof/p/3509970.html

using System;
using System.Collections.Generic;

public class DifferentialGeometry
{
    public KBVector3 HitPoint;
    public KBVector3 Normal;
    public float U;
    public float V;
    public KBVector3 DPDU;
    public KBVector3 DPDV;
    public KBVector3 DNDU;
    public KBVector3 DNDV;
    public KBVector3 DPDX;
    public KBVector3 DPDY;
    public float DUDX;
    public float DVDX;
    public float DUDY;
    public float DVDY;
    public KBShape Shape;

    public DifferentialGeometry(KBVector3 hitPoint, KBVector3 dpdu, KBVector3 dpdv, KBVector3 dndu, KBVector3 dndv,
        float u, float v, KBShape shape)
    {
        HitPoint = hitPoint;
        Normal = KBVector3.Cross(dpdu, dpdv);
        DPDU = dpdu;
        DPDV = dpdv;
        DNDU = dndu;
        DNDV = dpdv;
        U = u;
        V = v;
        Shape = shape;
        DUDX = 0.0f;
        DVDX = 0.0f;
        DUDY = 0.0f;
        DVDY = 0.0f;
    }
}
