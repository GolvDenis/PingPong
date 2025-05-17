using PingPong.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PingPong.Core.Controllers
{
    /// <summary>
    /// Керування камерою: слідує за м’ячем із фіксованим зсувом і плавним інтерполюванням.
    /// </summary>
    public class CameraController
    {
        private readonly PerspectiveCamera _camera;
        private readonly Ball _ball;

        /// <summary>
        /// Відступ камери від м’яча у локальних координатах (X — вправо, Y — вгору, Z — назад).
        /// </summary>
        public Vector3D Offset { get; set; }

        /// <summary>
        /// Швидкість інтерполяції (чим більше — тим швидше слідує за м’ячем).
        /// </summary>
        public double SmoothSpeed { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="camera">Камера, якою керуємо.</param>
        /// <param name="ball">М’яч, за яким слідкуємо.</param>
        /// <param name="offset">Початковий зсув від м’яча.</param>
        /// <param name="smoothSpeed">Швидкість інтерполяції.</param>
        public CameraController(
            PerspectiveCamera camera,
            Ball ball,
            Vector3D offset,
            double smoothSpeed = 5.0)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            _ball = ball ?? throw new ArgumentNullException(nameof(ball));
            Offset = offset;
            SmoothSpeed = smoothSpeed;
        }

        /// <summary>
        /// Оновити положення камери. Викликається в кожному кадрі.
        /// </summary>
        /// <param name="deltaTime">Час у секундах з останнього оновлення.</param>
        public void Update(double deltaTime)
        {
            // Бажана позиція камери = позиція м’яча + вектор зсуву
            Point3D desiredPos = new Point3D(
                _ball.Position.X + Offset.X,
                _ball.Position.Y + Offset.Y,
                _ball.Position.Z + Offset.Z);

            // Плавне інтерполювання: Lerp між поточною і бажаною позиціями
            double t = 1 - Math.Exp(-SmoothSpeed * deltaTime);
            Point3D currentPos = _camera.Position;
            Point3D newPos = new Point3D(
                currentPos.X + (desiredPos.X - currentPos.X) * t,
                currentPos.Y + (desiredPos.Y - currentPos.Y) * t,
                currentPos.Z + (desiredPos.Z - currentPos.Z) * t);

            _camera.Position = newPos;

            // Дивимося прямо на м’яч
            Vector3D lookDir = _ball.Position - newPos;
            lookDir.Normalize();
            _camera.LookDirection = lookDir;

            // За потреби можна фіксувати UpDirection:
            _camera.UpDirection = new Vector3D(0, 1, 0);
        }
    }
}
