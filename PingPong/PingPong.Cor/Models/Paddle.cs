using PingPong.Cor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace PingPong.Core.Models
{
    /// <summary>
    /// Ракетка — підклас GameObject3D.
    /// Розміри приблизні (товщина, висота, ширина).
    /// </summary>
    public class Paddle : GameObject3D
    {
        // Розміри ракетки в метрах
        public double Width { get; } = 0.02;   // товщина
        public double Height { get; } = 0.15;   // висота
        public double Depth { get; } = 0.25;   // ширина (уздовж плоскості ракетки)

        /// <summary>
        /// Півдіагональ боксу для спрощеного сферичного колайдера.
        /// </summary>
        public double CollisionRadius
        {
            get
            {
                // половина довжини просторової діагоналі прямокутного паралелепіпеда
                return Math.Sqrt(Width * Width + Height * Height + Depth * Depth) / 2.0;
            }
        }

        public Paddle()
        {
            // Пів-розміри для зручності побудови
            double hw = Width / 2;
            double hh = Height / 2;
            double hd = Depth / 2;

            // Вершини паралелепіпеда
            var positions = new Point3DCollection
            {
                // передня грань (z = -hd)
                new Point3D(-hw, -hh, -hd), // 0
                new Point3D( hw, -hh, -hd), // 1
                new Point3D( hw,  hh, -hd), // 2
                new Point3D(-hw,  hh, -hd), // 3

                // задня грань (z = +hd)
                new Point3D(-hw, -hh,  hd), // 4
                new Point3D( hw, -hh,  hd), // 5
                new Point3D( hw,  hh,  hd), // 6
                new Point3D(-hw,  hh,  hd), // 7
            };

            // Індекси для 12 трикутників (2 на кожну з 6 граней)
            var indices = new Int32Collection
            {
                // передня грань
                0,1,2,  0,2,3,
                // задня грань
                5,4,7,  5,7,6,
                // верхня грань
                3,2,6,  3,6,7,
                // нижня грань
                4,5,1,  4,1,0,
                // ліва грань
                4,0,3,  4,3,7,
                // права грань
                1,5,6,  1,6,2
            };

            var mesh = new MeshGeometry3D
            {
                Positions = positions,
                TriangleIndices = indices
                // Якщо потрібно, можна додати Normals і TextureCoordinates
            };

            // Простий матеріал (можна замінити на текстурний через ResourceManager)
            var material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));

            // Ініціалізуємо модель всередині базового класу
            InitializeModel(mesh, material);

            // За замовчуванням ракетка розташована в центрі сцени
            // Позицію гравець встановить через PlayerController
            this.Position = new Point3D(0, 0, 0);
        }

        /// <summary>
        /// Можна перевизначити, щоб додати специфічну реакцію при зіткненні з м’ячем.
        /// Наприклад, змінити кут відбивання залежно від позиції удару.
        /// </summary>
        public override void OnCollision(GameObject3D other)
        {
            // Якщо "other" — це м’яч, можна додати додаткову логіку:
            //   var ball = other as Ball;
            //   if (ball != null) { ... }
            base.OnCollision(other);
        }
    }
}
