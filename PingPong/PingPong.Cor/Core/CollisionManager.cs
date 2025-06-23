using PingPong.Cor.Core;
using PingPong.Core.Models;
using System.Windows.Media.Media3D;

namespace PingPong.Core.Core
{
    public class CollisionManager
    {
            public bool CheckCollision(GameObject3D a, GameObject3D b)
            {
                double dist = (a.Position - b.Position).Length;
                return dist < (GetRadius(a) + GetRadius(b));
            }

            public void HandleCollision(GameObject3D a, GameObject3D b)
            {
                if (!CheckCollision(a, b)) return;
                a.OnCollision(b);
                b.OnCollision(a);
            }

            private double GetRadius(GameObject3D obj)
            {
                return obj is Table ? 0.0 : (double)(obj as dynamic).CollisionRadius;
            }
        }
}
