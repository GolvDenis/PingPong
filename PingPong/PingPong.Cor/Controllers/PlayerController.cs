using PingPong.Core.Core;
using PingPong.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using PingPong.Corе.Helpers;


using InputManager = PingPong.Core.Core.InputManager;
using Direction = PingPong.Core.Core.Direction;


namespace PingPong.Core.Controllers
{
    /// <summary>
    /// Керування позицією гравця навколо столу (WASD) і орієнтацією ракетки за курсором миші.
    /// </summary>
    public class PlayerController
    {
        private readonly InputManager _input;
        private readonly Paddle _paddle;
        private readonly MouseHelper _mouseHelper;

        /// <summary>
        /// Швидкість пересування гравця (м/с).
        /// </summary>
        public double MoveSpeed { get; set; } = 1.5;

        /// <summary>
        /// Межі поля (для обмеження руху). 
        /// </summary>
        public double BoundaryX { get; set; }
        public double BoundaryZ { get; set; }

        public PlayerController(InputManager inputManager, Paddle paddle, MouseHelper mouseHelper,
                                double moveSpeed = 1.5, double boundaryX = 1.0, double boundaryZ = 0.75)
        {
            _input = inputManager ?? throw new ArgumentNullException(nameof(inputManager));
            _paddle = paddle ?? throw new ArgumentNullException(nameof(paddle));
            _mouseHelper = mouseHelper ?? throw new ArgumentNullException(nameof(mouseHelper));

            MoveSpeed = moveSpeed;
            BoundaryX = boundaryX;
            BoundaryZ = boundaryZ;
        }

        /// <summary>
        /// Оновити позицію та орієнтацію ракетки. Викликається в кожному кадрі.
        /// </summary>
        public void Update(double deltaTime)
        {
            // 1) Рух WASD по горизонтальній площині XZ
            var dirs = _input.GetActiveDirections();
            Vector3D move = new Vector3D();
            if (dirs.Contains(Direction.Forward)) move.Z -= 1;
            if (dirs.Contains(Direction.Backward)) move.Z += 1;
            if (dirs.Contains(Direction.Left)) move.X -= 1;
            if (dirs.Contains(Direction.Right)) move.X += 1;

            if (move.Length > 0)
            {
                move.Normalize();
                move *= MoveSpeed * deltaTime;

                var pos = _paddle.Position;
                pos.X = Math.Clamp(pos.X + move.X, -BoundaryX, BoundaryX);
                pos.Z = Math.Clamp(pos.Z + move.Z, -BoundaryZ, BoundaryZ);
                _paddle.Position = pos;
            }

            // 2) Орієнтація ракетки за курсором миші
            // Отримуємо положення миші в 3D (проекція на площину столу)
            Point3D target = _mouseHelper.ScreenToWorldOnPlane(
                _input.GetMousePosition(),
                planeY: 0.76     // висота поверхні столу
            );

            // Вектор від ракетки до точки на столі під курсором
            Vector3D look = target - _paddle.Position;
            if (look.Length > 0.001)
            {
                look.Normalize();
                // Обчислюємо кут повороту навколо осі Y
                double yaw = Math.Atan2(look.X, look.Z) * (180.0 / Math.PI);

                // Створюємо поворот навколо центру ракетки
                var axis = new Vector3D(0, 1, 0);
                var rotation = new AxisAngleRotation3D(axis, yaw);
                var rotTransform = new RotateTransform3D(rotation, new Point3D(0, 0, 0));

                // Застосовуємо лише цей поворот (без зміни позиції)
                var group = new Transform3DGroup();
                group.Children.Add(rotTransform);
                group.Children.Add(new TranslateTransform3D(_paddle.Position.X,
                                                            _paddle.Position.Y,
                                                            _paddle.Position.Z));
                _paddle.Transform = group;
            }
        }
    }
}
