using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PingPong.Cor.Core
{
    /// <summary>
    /// Базовий клас для всіх 3D-об’єктів у грі
    /// </summary>
    public abstract class GameObject3D
    {
        private TranslateTransform3D _translation = new TranslateTransform3D();

        public MeshGeometry3D Mesh { get; protected set; }
        public Material Material { get; protected set; }

        public Transform3D Transform
        {
            get => new MatrixTransform3D(_translation.Value);
            set => _translation = value as TranslateTransform3D ?? new TranslateTransform3D();
        }

        public Point3D Position
        {
            get => new Point3D(_translation.OffsetX, _translation.OffsetY, _translation.OffsetZ);
            set
            {
                _translation.OffsetX = value.X;
                _translation.OffsetY = value.Y;
                _translation.OffsetZ = value.Z;
            }
        }

        public Vector3D Velocity { get; set; } = new Vector3D();

        protected void InitializeModel(MeshGeometry3D mesh, Material material)
        {
            Mesh = mesh;
            Material = material;
        }

        public virtual void Update(double deltaTime)
        {
            Position = new Point3D(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime,
                Position.Z + Velocity.Z * deltaTime);
        }

        public virtual void OnCollision(GameObject3D other) { }
    }

}
