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

        public Table()
        {
            // Створення прямокутної геометрії для столу
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Верхня площина столу — прямокутник
            Point3D p0 = new Point3D(-Width / 2, 0, -Depth / 2);
            Point3D p1 = new Point3D(Width / 2, 0, -Depth / 2);
            Point3D p2 = new Point3D(Width / 2, 0, Depth / 2);
            Point3D p3 = new Point3D(-Width / 2, 0, Depth / 2);

            mesh.Positions = new Point3DCollection() { p0, p1, p2, p3 };
            mesh.TriangleIndices = new Int32Collection() { 0, 1, 2, 0, 2, 3 };

            // Текстура або однотонний колір
            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkGreen));

            InitializeModel(mesh, material);

            // Встановлюємо позицію по висоті
            this.Position = new Point3D(0, Height, 0);
        }
    }
}
