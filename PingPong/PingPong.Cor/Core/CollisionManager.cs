using PingPong.Cor.Core;
using PingPong.Core.Models;
using System.Windows.Media.Media3D;

namespace PingPong.Core.Core
{
    public class CollisionManager
    {
        /// <summary>
        /// Перевірка на зіткнення між двома об'єктами (простіше за все на основі відстані).
        /// </summary>
        public bool CheckCollision(GameObject3D obj1, GameObject3D obj2)
        {
            double distance = (obj1.Position - obj2.Position).Length;
            // Якщо відстань між об'єктами менша за суму їхніх радіусів (припускаємо сферичні об'єкти)
            return distance < 1.0; // Наприклад, 1.0 — це радіус "зіткнення" (можна коригувати)
        }

        /// <summary>
        /// Обробка зіткнень, наприклад, відбивання м'яча від ракетки чи меж гри.
        /// </summary>
        public void HandleCollision(GameObject3D obj1, GameObject3D obj2)
        {
            if (CheckCollision(obj1, obj2))
            {
                // Для спрощення, якщо зіткнення є — міняємо напрямок об'єкта.
                obj1.Velocity = -obj1.Velocity; // Простий відскок
                // Тут можна додати більш складну логіку для обробки зіткнень
            }
        }
    }
}
