using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;

namespace Essence.View.Models
{
    public interface IRenderContext2D
    {
        Transform2D ModelView { get; }

        BoundingBox2d BoundingBoxInView { get; }
        BoundingBox2d BoundingBoxInModel { get; }

        void Transform(Transform2D transform);

        void PushAttrib(int attribs);
        void PopAttrib();
    }
}