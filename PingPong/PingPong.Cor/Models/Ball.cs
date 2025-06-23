using PingPong.Cor.Core;
using PingPong.Core.Core;
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
        public double Radius { get; } = 0.02;
        public double CollisionRadius => Radius;
        public SpinType CurrentSpin { get; set; }

        private readonly AxisAngleRotation3D _spinRotation;
        public Vector3D AngularVelocity { get; set; } = new Vector3D();

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
            var material = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));

            InitializeModel(mesh, material);

            _spinRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            Transform = new RotateTransform3D(_spinRotation, new Point3D(0, 0, 0));

            // Початкова позиція: центр столу, трохи вище поверхні
            this.Position = new Point3D(0, Radius + 0.76, 0);
        }


        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            if (CurrentSpin != SpinType.None)
            {
                var axis = GetSpinAxis(CurrentSpin);
                _spinRotation.Axis = axis;
                _spinRotation.Angle += AngularVelocity.Length * deltaTime * (180 / Math.PI);
            }
        }

        public override void OnCollision(GameObject3D other)
        {
            Velocity = PhysicsEngine.Reflect(Velocity, this, other);
            CurrentSpin = other is Paddle ? CurrentSpin : SpinType.None;
        }

        private Vector3D GetSpinAxis(SpinType spin) => spin switch
        {
            SpinType.Topspin => new Vector3D(1, 0, 0),
            SpinType.Backspin => new Vector3D(-1, 0, 0),
            SpinType.Sidespin => new Vector3D(0, 1, 0),
            _ => new Vector3D(0, 0, 0)
        };
    }
}
