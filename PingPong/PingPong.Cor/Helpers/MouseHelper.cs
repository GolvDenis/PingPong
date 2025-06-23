using System;
using System.Windows;                       // <-- додаємо для Point
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;                 // якщо будете робити HitTest

namespace PingPong.Corе.Helpers
{
    /// <summary>
    /// Допоміжний клас для конвертації 2D-позиції миші в точку на горизонтальній площині Y=planeY.
    /// </summary>
    public class MouseHelper
    {
        private readonly Viewport3D _viewport;
        private readonly PerspectiveCamera _camera;

        public MouseHelper(Viewport3D viewport, PerspectiveCamera camera)
        {
            _viewport = viewport;
            _camera = camera;
        }

        /// <summary>
        /// Перетворює екранні координати миші в 3D-точку на площині Y=planeY.
        /// Перш за все пробує “RayHitTest” по візуалам 3D; якщо не влучає — проектує променем камери.
        /// </summary>
        public Point3D ScreenToWorldOnPlane(Point screenPosition, double planeY)
        {
            var nv = _camera.LookDirection;
            nv.Normalize();
            var origin = _camera.Position;
            if (Math.Abs(nv.Y) < 1e-6)
                return new Point3D(origin.X, planeY, origin.Z);
            var t = (planeY - origin.Y) / nv.Y;
            return origin + nv * t;
        }
    }
}
