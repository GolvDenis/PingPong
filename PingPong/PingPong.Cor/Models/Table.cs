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
    /// Клас представляє стіл для пінг-понгу (ігрове поле).
    /// Є статичним об’єктом.
    /// </summary>
    public class Table : GameObject3D
    {
        public double Width { get; } = 2.74;   // Стандартна ширина столу
        public double Height { get; } = 0.76;  // Висота від землі
        public double Depth { get; } = 1.525;  // Глибина (довжина) столу
        public Rect3D Bounds => new Rect3D(
            Position.X - Width / 2,
            Position.Y - 0.05,
            Position.Z - Depth / 2,
            Width, 0.1, Depth);


        public Table()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            double w = Width / 2;
            double d = Depth / 2;
            double topY = Height;
            double bottomY = Height - Depth;

            // Точки для верхньої стільниці (8 точок)
            Point3D[] points =
            {
        // Верхній прямокутник
        new Point3D(-w, topY, -d),    // 0
        new Point3D( w, topY, -d),    // 1
        new Point3D( w, topY,  d),    // 2
        new Point3D(-w, topY,  d),    // 3
        
        // Нижній прямокутник
        new Point3D(-w, bottomY, -d), // 4
        new Point3D( w, bottomY, -d), // 5
        new Point3D( w, bottomY,  d), // 6
        new Point3D(-w, bottomY,  d)  // 7
    };

            // Додаємо точки в меш
            foreach (var point in points)
            {
                mesh.Positions.Add(point);
            }

            // Нормалі (вертикальні для верху/низу, горизонтальні для боків)
            Vector3D upNormal = new Vector3D(0, 1, 0);
            Vector3D downNormal = new Vector3D(0, -1, 0);
            Vector3D frontNormal = new Vector3D(0, 0, -1);
            Vector3D backNormal = new Vector3D(0, 0, 1);
            Vector3D rightNormal = new Vector3D(1, 0, 0);
            Vector3D leftNormal = new Vector3D(-1, 0, 0);

            // Верхня грань
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.Normals.Add(upNormal);
            mesh.Normals.Add(upNormal);
            mesh.Normals.Add(upNormal);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.Normals.Add(upNormal);
            mesh.Normals.Add(upNormal);
            mesh.Normals.Add(upNormal);

            // Нижня грань
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(5);
            mesh.Normals.Add(downNormal);
            mesh.Normals.Add(downNormal);
            mesh.Normals.Add(downNormal);

            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(6);
            mesh.Normals.Add(downNormal);
            mesh.Normals.Add(downNormal);
            mesh.Normals.Add(downNormal);

            // Передня грань (Z = -d)
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(5);
            mesh.Normals.Add(frontNormal);
            mesh.Normals.Add(frontNormal);
            mesh.Normals.Add(frontNormal);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(1);
            mesh.Normals.Add(frontNormal);
            mesh.Normals.Add(frontNormal);
            mesh.Normals.Add(frontNormal);

            // Задня грань (Z = d)
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(6);
            mesh.Normals.Add(backNormal);
            mesh.Normals.Add(backNormal);
            mesh.Normals.Add(backNormal);

            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);
            mesh.Normals.Add(backNormal);
            mesh.Normals.Add(backNormal);
            mesh.Normals.Add(backNormal);

            // Права грань (X = w)
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(6);
            mesh.Normals.Add(rightNormal);
            mesh.Normals.Add(rightNormal);
            mesh.Normals.Add(rightNormal);

            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(2);
            mesh.Normals.Add(rightNormal);
            mesh.Normals.Add(rightNormal);
            mesh.Normals.Add(rightNormal);

            // Ліва грань (X = -w)
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.Normals.Add(leftNormal);
            mesh.Normals.Add(leftNormal);
            mesh.Normals.Add(leftNormal);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(4);
            mesh.Normals.Add(leftNormal);
            mesh.Normals.Add(leftNormal);
            mesh.Normals.Add(leftNormal);

            // Матеріал столу (темно-зелений)
            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkGreen));

            // Ініціалізація моделі
            InitializeModel(mesh, material);

            // Позиціонуємо стіл (центруємо по XZ, встановлюємо висоту)
            Position = new Point3D(0, Height, 0);

        }
    }
}
