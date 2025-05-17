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
    /// Базовий клас для всіх 3D-об’єктів у грі.
    /// Надає базові властивості для рендерингу, трансформацій та фізики.
    /// </summary>
    public abstract class GameObject3D
    {
        /// <summary>
        /// Геометрія об'єкта (MeshGeometry3D). Використовується для рендерингу у WPF.
        /// </summary>
        public MeshGeometry3D Mesh { get; protected set; }

        /// <summary>
        /// Матеріал (колір, текстура тощо).
        /// </summary>
        public Material Material { get; protected set; }

        /// <summary>
        /// Трансформації (позиція, обертання, масштаб).
        /// </summary>
        public Transform3D Transform { get; set; } = Transform3D.Identity;

        /// <summary>
        /// Позиція у світових координатах.
        /// </summary>
        public Point3D Position
        {
            get
            {
                if (Transform is TranslateTransform3D tt)
                    return tt.OffsetXYZ();
                return new Point3D();
            }
            set
            {
                Transform = new TranslateTransform3D(value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Швидкість руху (для простого фізичного рушія).
        /// </summary>
        public Vector3D Velocity { get; set; } = new Vector3D();

        /// <summary>
        /// Ініціалізує модель (геометрію, матеріал) об'єкта.
        /// Має викликатися у конструкторі підкласу.
        /// </summary>
        protected void InitializeModel(MeshGeometry3D mesh, Material material)
        {
            Mesh = mesh;
            Material = material;
        }

        /// <summary>
        /// Оновлення об'єкта: рух, фізика, анімації.
        /// Викликається кожен кадр у Game.Update().
        /// </summary>
        /// <param name="deltaTime">Час у секундах з останнього оновлення.</param>
        public virtual void Update(double deltaTime)
        {
            // За замовчуванням рух за інерцією
            Position = new Point3D(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime,
                Position.Z + Velocity.Z * deltaTime);
        }

        /// <summary>
        /// Обробка колізії з іншим об'єктом.
        /// За потреби перевизначати у підкласах.
        /// </summary>
        public virtual void OnCollision(GameObject3D other)
        {
            // Пусто за замовчуванням
        }
    }

    internal static class TranslateTransform3DExtensions
    {
        public static Point3D OffsetXYZ(this TranslateTransform3D tt)
        {
            return new Point3D(tt.OffsetX, tt.OffsetY, tt.OffsetZ);
        }
    }
}
