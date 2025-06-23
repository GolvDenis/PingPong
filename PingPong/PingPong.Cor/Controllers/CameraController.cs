using PingPong.Cor.Core;
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
        private readonly Paddle _paddle;
        private readonly Vector3D _offset = new Vector3D(0, 0.5, -0.5);

        public CameraController(PerspectiveCamera camera, Paddle paddle)
        {
            _camera = camera;
            _paddle = paddle;
        }

        /// <summary>
        /// Оновлює позицію та напрямок камери:
        /// - Камера завжди над гравцем
        /// - Дивиться на м'яч з плавним переміщенням
        /// </summary>
        public void Update()
        {
            _camera.Position = _paddle.Position + _offset;
            var forward = _paddle.Transform.Transform(new Point3D(0, 0, 1)) - _paddle.Position;
            forward.Normalize();
            _camera.LookDirection = forward;
        }
    }
}
