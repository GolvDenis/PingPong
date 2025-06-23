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
using PingPong.Cor.Core;
using System.Windows.Controls;
using System.Windows;



namespace PingPong.Core.Controllers
{
    /// <summary>
    /// Керування позицією гравця навколо столу (WASD) і орієнтацією ракетки за курсором миші.
    /// </summary>
    public class PlayerController
    {
        private readonly GameObject3D _player;
        private readonly Paddle _paddle;
        private readonly Table _table;
        private readonly InputManager _input;
        private const double Speed = 2.5;

        public PlayerController(GameObject3D player, Paddle paddle, Table table, InputManager input)
        {
            _player = player;
            _paddle = paddle;
            _table = table;
            _input = input;
        }

        public void Update(double dt)
        {
            var dir = new Vector3D(_input.KeyboardDir.X, 0, -_input.KeyboardDir.Y);
            if (dir.Length > 0)
            {
                dir.Normalize();
                _player.Position += dir * Speed * dt;
            }

            var target = _input.MousePosition3D;
            var clampedX = Math.Clamp(target.X, -_table.Width / 2 + _paddle.Width / 2, _table.Width / 2 - _paddle.Width / 2);
            var clampedZ = Math.Clamp(target.Z, -_table.Depth / 2 + 0.1, _table.Depth / 2 - 0.1);
            _paddle.Position = new Point3D(clampedX, _table.Height + 0.05, clampedZ);
        }

    }
}