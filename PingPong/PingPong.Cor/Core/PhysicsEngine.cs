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
        public static Vector3D Gravity { get; set; } = new Vector3D(0, -9.81, 0);

        public static void Update(GameObject3D obj, double deltaTime)
        {
            obj.Velocity += Gravity * deltaTime;
            obj.Position = new Point3D(
                obj.Position.X + obj.Velocity.X * deltaTime,
                obj.Position.Y + obj.Velocity.Y * deltaTime,
                obj.Position.Z + obj.Velocity.Z * deltaTime);
        }

        public static Vector3D Reflect(Vector3D velocity, GameObject3D obj, GameObject3D other)
        {
            // Compute normal from obj and other
            Vector3D normal = (obj.Position - other.Position);
            normal.Normalize();
            // Reflect v across normal: v - 2*(v·n)*n
            double dot = Vector3D.DotProduct(velocity, normal);
            return velocity - 2 * dot * normal;
        }
    }
}
