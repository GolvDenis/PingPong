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
            _viewport = viewport
                ?? throw new ArgumentNullException(nameof(viewport));
            _camera = camera
                ?? throw new ArgumentNullException(nameof(camera));
        }

        /// <summary>
        /// Перетворює екранні координати миші в 3D-точку на площині Y=planeY.
        /// Перш за все пробує “RayHitTest” по візуалам 3D; якщо не влучає — проектує променем камери.
        /// </summary>
        public Point3D ScreenToWorldOnPlane(Point screenPosition, double planeY)
        {
            // 1) Hit-тест: якщо потрапили по якомусь 3D-об’єкту (наприклад, по столу), повертаємо
            var result = VisualTreeHelper.HitTest(_viewport, screenPosition) as RayHitTestResult;
            if (result != null)
                return result.PointHit;

            // 2) Інакше — робимо найпростіше перетинання променя камери з площиною Y=planeY
            var origin = _camera.Position;
            var direction = _camera.LookDirection;
            direction.Normalize();

            if (Math.Abs(direction.Y) < 1e-6)
                return new Point3D(origin.X, planeY, origin.Z);

            double t = (planeY - origin.Y) / direction.Y;
            return origin + direction * t;
        }
    }
}
