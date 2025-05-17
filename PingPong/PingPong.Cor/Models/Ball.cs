using PingPong.Cor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PingPong.Core.Models
{
    /// <summary>
    /// Тип обертання м’яча.
    /// </summary>
    public enum SpinType
    {
        None,
        Topspin,
        Backspin,
        Sidespin
    }

    /// <summary>
    /// М’яч — підклас GameObject3D.
    /// Містить логіку для фізики руху й обертання.
    /// </summary>
    public class Ball : GameObject3D
    {
        /// <summary>
        /// Радіус м’яча (метри).
        /// </summary>
        public double Radius { get; } = 0.02;

        /// <summary>
        /// Для колізій: використовуємо сферу.
        /// </summary>
        public double CollisionRadius => Radius;

        /// <summary>
        /// Поточний тип обертання (спін).
        /// </summary>
        public SpinType CurrentSpin { get; set; } = SpinType.None;

        /// <summary>
        /// Ось і кутова швидкість обертання (рад/с).
        /// </summary>
        public Vector3D AngularVelocity { get; set; } = new Vector3D(0, 0, 0);

        public Ball()
        {
            // Генеруємо просту сферичну геометрію
            var mesh = new MeshGeometry3D();
            const int slices = 16;
            const int stacks = 12;
            for (int stack = 0; stack <= stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / stacks; // від +90° до -90°
                double y = Radius * Math.Sin(phi);
                double scale = Radius * Math.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = scale * Math.Cos(theta);
                    double z = scale * Math.Sin(theta);
                    mesh.Positions.Add(new Point3D(x, y, z));
                    mesh.Normals.Add(new Vector3D(x, y, z));
                    mesh.TextureCoordinates.Add(
                        new System.Windows.Point((double)slice / slices, (double)stack / stacks)
                    );
                }
            }
            // Індекси трикутників
            for (int stack = 0; stack < stacks; stack++)
            {
                for (int slice = 0; slice < slices; slice++)
                {
                    int first = (stack) * (slices + 1) + slice;
                    int second = (stack + 1) * (slices + 1) + slice;

                    // Кожен квад радіани розбиваємо на два трикутники
                    mesh.TriangleIndices.Add(first);
                    mesh.TriangleIndices.Add(second);
                    mesh.TriangleIndices.Add(first + 1);

                    mesh.TriangleIndices.Add(second);
                    mesh.TriangleIndices.Add(second + 1);
                    mesh.TriangleIndices.Add(first + 1);
                }
            }

            // Білий матеріал (можна замінити текстурою через ResourceManager)
            var material = new DiffuseMaterial(new SolidColorBrush(Colors.White));

            InitializeModel(mesh, material);

            // Початкова позиція: центр столу, трохи вище поверхні
            this.Position = new Point3D(0, Radius + 0.76, 0);
        }

        /// <summary>
        /// Оновлення позиції, швидкості та обертання.
        /// </summary>
        public override void Update(double deltaTime)
        {
            // Викликаємо базове оновлення (позиція за лінійною швидкістю)
            base.Update(deltaTime);

            // Якщо задано обертання, застосовуємо його до Transform3D
            if (AngularVelocity.Length > 0)
            {
                // Обчислюємо кут (в градусах) для цього кадру
                double angleDeg = AngularVelocity.Length * deltaTime * (180.0 / Math.PI);
                // Ось обертання — напрямок AngularVelocity
                var axis = AngularVelocity;
                axis.Normalize();

                // Створюємо обертання навколо локальної осі
                var rotate = new AxisAngleRotation3D(axis, angleDeg);
                var rotTransform = new RotateTransform3D(rotate, new Point3D(0, 0, 0));

                // Додаємо нове обертання до існуючих трансформацій
                var group = Transform as Transform3DGroup ?? new Transform3DGroup();
                group.Children.Add(rotTransform);
                Transform = group;
            }
        }

        /// <summary>
        /// Обробка зіткнення: змінюємо швидкість і додаємо спін залежно від CurrentSpin.
        /// </summary>
        public override void OnCollision(GameObject3D other)
        {
            // Інвертуємо лінійну швидкість (простіший відскок)
            Velocity = -Velocity;

            // Якщо зіткнулися з ракеткою, додаємо спін
            if (other is Paddle)
            {
                switch (CurrentSpin)
                {
                    case SpinType.Topspin:
                        AngularVelocity = new Vector3D(1, 0, 0) * 50.0;   // приклад
                        break;
                    case SpinType.Backspin:
                        AngularVelocity = new Vector3D(-1, 0, 0) * 50.0;
                        break;
                    case SpinType.Sidespin:
                        AngularVelocity = new Vector3D(0, 1, 0) * 50.0;
                        break;
                    default:
                        AngularVelocity = new Vector3D();
                        break;
                }

                // Можна трохи модифікувати лінійну швидкість відповідно до спіну
                Velocity += AngularVelocity * 0.001;
            }

            base.OnCollision(other);
        }
    }
}
