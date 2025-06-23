using PingPong.Core.Core;
using PingPong.Core.Managers;
using PingPong.Core.Models;
using PingPong.Corе.Helpers;
using PingPong.Corе.Managers;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using InputManager = PingPong.Core.Core.InputManager;
using ResourceManager = PingPong.Corе.Managers.ResourceManager;

namespace PingPong.Core.Controllers
{
    /// <summary>
    /// Основна логіка гри: ініціалізація, оновлення всіх систем і ігровий цикл.
    /// </summary>
    public class Game
    {
        private readonly Viewport3D _viewport;
        private readonly PerspectiveCamera _camera;
        private readonly DispatcherTimer _timer;

        private readonly PhysicsEngine _physics = null!;
        private readonly CollisionManager _collision = new CollisionManager();
        private readonly ScoreManager _score = new ScoreManager();
        private readonly CameraController _cameraController;
        private readonly PlayerController _playerController;

        private readonly InputManager _input;
        private readonly MouseHelper _mouseHelper;

        private readonly Paddle _paddle;
        private readonly Ball _ball;
        private readonly Table _table;
        private readonly ModelVisual3D _sceneRoot;
        public ScoreManager ScoreManager => _score;

        public Game(Viewport3D viewport)
        {
            _viewport = viewport;
            _camera = new PerspectiveCamera { FieldOfView = 60 };

            _camera.Position = new Point3D(0, 1.5, 3);
            _camera.LookDirection = new Vector3D(0, 0, -1);
            _camera.UpDirection = new Vector3D(0, 1, 0);

            _sceneRoot = new ModelVisual3D();
            _viewport.Children.Add(_sceneRoot);

            _table = new Table();
            _paddle = new Paddle();
            _ball = new Ball();

            _table = new Table();
            _paddle = new Paddle();
            _ball = new Ball();

            var tableVisual = new ModelVisual3D
            {
                Content = new GeometryModel3D(_table.Mesh, _table.Material),
                Transform = _table.Transform
            };
            var paddleVisual = new ModelVisual3D
            {
                Content = new GeometryModel3D(_paddle.Mesh, _paddle.Material),
                Transform = _paddle.Transform
            };
            var ballVisual = new ModelVisual3D
            {
                Content = new GeometryModel3D(_ball.Mesh, _ball.Material),
                Transform = _ball.Transform
            };

            ModelVisual3D light = new ModelVisual3D();
            light.Content = new DirectionalLight(Colors.White, new Vector3D(0, -1, 0));
            _sceneRoot.Children.Add(light);
            _sceneRoot.Children.Add(tableVisual);
            _sceneRoot.Children.Add(paddleVisual);
            _sceneRoot.Children.Add(ballVisual);

            _mouseHelper = new MouseHelper(_viewport, _camera);
            _input = new InputManager(_viewport, _mouseHelper);

            _playerController = new PlayerController(_paddle, _paddle, _table, _input);
            _cameraController = new CameraController(_camera, _paddle);

            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _timer.Tick += OnTick;
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private void OnTick(object? sender, EventArgs e)
        {
            double dt = _timer.Interval.TotalSeconds;

            _playerController.Update(dt);
            PhysicsEngine.Update(_ball, dt);
            _collision.HandleCollision(_ball, _paddle);
            _collision.HandleCollision(_ball, _table);

            _cameraController.Update();
        }
    }
}
