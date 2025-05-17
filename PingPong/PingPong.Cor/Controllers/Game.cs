using PingPong.Core.Core;
using PingPong.Core.Managers;
using PingPong.Core.Models;
using PingPong.Corе.Helpers;
using PingPong.Corе.Managers;
using System.Resources;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ResourceManager = PingPong.Corе.Managers.ResourceManager;

namespace PingPong.Core.Controllers
{
    /// <summary>
    /// Основна логіка гри: ініціалізація, оновлення всіх систем і ігровий цикл.
    /// </summary>
    public class Game
    {
        // Сцена та камера
        private readonly Viewport3D _viewport;
        private readonly PerspectiveCamera _camera;

        // Менеджери і рушії
        private readonly InputManager _input;
        private readonly MouseHelper _mouseHelper;
        private readonly CollisionManager _collisionManager;
        private readonly PhysicsEngine _physicsEngine;
        private readonly ScoreManager _scoreManager;
        private readonly AudioManager _audioManager;
        private readonly ResourceManager _resourceManager;

        // Моделі
        private readonly Table _table;
        private readonly Paddle _paddle;
        private readonly Ball _ball;

        // Контролери
        private readonly PlayerController _playerController;
        private readonly CameraController _cameraController;

        private DateTime _lastUpdateTime;

        public Game(Viewport3D viewport, PerspectiveCamera camera)
        {
            _viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));

            // 1) Ініціалізуємо базові системи
            _input = new InputManager();
            _collisionManager = new CollisionManager();
            _physicsEngine = new PhysicsEngine();
            _scoreManager = new ScoreManager();
            _audioManager = new AudioManager();
            _resourceManager = new ResourceManager();

            // 2) Створюємо моделі
            _table = new Table();
            _paddle = new Paddle();
            _ball = new Ball();

            // Додаємо їх у сцену (Viewport3D) через ResourceManager
            _resourceManager.AddToScene(_viewport, _table);
            _resourceManager.AddToScene(_viewport, _paddle);
            _resourceManager.AddToScene(_viewport, _ball);

            // 3) Створюємо MouseHelper та контролери
            _mouseHelper = new MouseHelper(_viewport, _camera);
            _playerController = new PlayerController(
                _input,
                _paddle,
                _mouseHelper,
                moveSpeed: 1.5,
                boundaryX: _table.Width / 2 - _paddle.Depth / 2,
                boundaryZ: _table.Depth / 2 - _paddle.Depth / 2
            );
            // Камера стоїть за м’ячем на (0,2,-5) і слідує з плавністю 5.0
            _cameraController = new CameraController(
                _camera,
                _ball,
                offset: new Vector3D(0, 2, -5),
                smoothSpeed: 5.0
            );

            // 4) Підписуємося на цикл рендерінгу
            CompositionTarget.Rendering += OnRendering;
            _lastUpdateTime = DateTime.Now;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var delta = (now - _lastUpdateTime).TotalSeconds;
            _lastUpdateTime = now;
            Update(delta);
        }

        /// <summary>
        /// Оновлює всі підсистеми гри.
        /// </summary>
        private void Update(double deltaTime)
        {
            // 1) Зчитуємо стан клавіш/миші
            // (InputManager оновлюється автоматично через WPF події в GameWindow)

            // 2) Оновлюємо контролери
            _playerController.Update(deltaTime);
            _cameraController.Update(deltaTime);

            // 3) Актуалізуємо фізику та колізії м’яча
            _physicsEngine.ApplyPhysics(_ball, deltaTime);
            _physicsEngine.HandleCollisions(_ball);

            // Стежимо за відскоками
            if (_collisionManager.CheckCollision(_ball, _paddle))
                _collisionManager.HandleCollision(_ball, _paddle);
            if (_collisionManager.CheckCollision(_ball, _table))
                _collisionManager.HandleCollision(_ball, _table);

            // 4) Оновлюємо модель м’яча (рух + обертання)
            _ball.Update(deltaTime);

            // 5) Перевіряємо, чи м’яч вийшов за межі столу (програш / очки)
            if (Math.Abs(_ball.Position.Z) > _table.Depth / 2 + _ball.Radius)
            {
                bool playerScores = _ball.Position.Z < 0;
                _scoreManager.RegisterPoint(playerScores);
                _audioManager.PlaySound(playerScores ? "score.wav" : "lose.wav");
                ResetBall();
            }

            // 6) Оновлюємо рахунок (UI) та програші
            _scoreManager.UpdateUI();
        }

        /// <summary>
        /// Скидає положення та швидкість м’яча.
        /// </summary>
        private void ResetBall()
        {
            _ball.Position = new Point3D(0, _ball.Radius + _table.Height, 0);
            _ball.Velocity = new Vector3D(0, 0, 0);
            _ball.CurrentSpin = SpinType.None;
            // за бажанням даємо початковий поштовх:
            _ball.Velocity = new Vector3D(0, 0, 2.0);
        }

        /// <summary>
        /// Викликаємо при виході з гри, щоб відписати події.
        /// </summary>
        public void Stop()
        {
            CompositionTarget.Rendering -= OnRendering;
        }
    }
}
