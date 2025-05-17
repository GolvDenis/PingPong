using PingPong.Cor.Core;
using PingPong.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PingPong.Core.Core
{
    public class PhysicsEngine
    {
        private const double Gravity = -9.8; // Гравітація в одиницях на секунду в квадраті (для спрощення)

        /// <summary>
        /// Оновлює швидкість і позицію об'єкта з урахуванням фізики (гравітація, швидкість).
        /// </summary>
        public void ApplyPhysics(GameObject3D obj, double deltaTime)
        {
            // Застосування гравітації (якщо об'єкт має швидкість по Y)
            if (obj.Position.Y > 0) // Якщо об'єкт не на землі
            {
                obj.Velocity += new Vector3D(0, Gravity * deltaTime, 0);
            }

            // Оновлення позиції об'єкта на основі його швидкості
            obj.Position += obj.Velocity * deltaTime;
        }

        /// <summary>
        /// Оновлення швидкості об'єкта в залежності від зіткнень. ч
        /// </summary>
        public void HandleCollisions(GameObject3D obj)
        {
            // Можна додати логіку для зіткнень, яка змінює швидкість об'єкта
            if (obj.Position.Y < 0) // Якщо об'єкт потрапив нижче за поверхню (наприклад, земля)
            {
                obj.Position = new Point3D(obj.Position.X, 0, obj.Position.Z); // Встановлюємо на землю
                obj.Velocity = new Vector3D(obj.Velocity.X, 0, obj.Velocity.Z); // Обнуляємо вертикальну швидкість
            }
        }
    }
}
