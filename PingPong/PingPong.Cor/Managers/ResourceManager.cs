using PingPong.Cor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace PingPong.Corе.Managers
{
    /// <summary>
    /// Відповідає за завантаження та додавання ресурсів у сцену.
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// Додає GameObject3D у Viewport3D.
        /// Конвертує Mesh + Material у GeometryModel3D та упаковує в ModelVisual3D.
        /// </summary>
        public void AddToScene(Viewport3D viewport, GameObject3D obj)
        {
            var geometry = new GeometryModel3D
            {
                Geometry = obj.Mesh,
                Material = obj.Material,
                BackMaterial = obj.Material
            };

            var model = new ModelVisual3D
            {
                Content = geometry,
                Transform = obj.Transform
            };

            viewport.Children.Add(model);
        }

        /// <summary>
        /// Можна розширити методи для завантаження текстур, моделей зі файлів тощо.
        /// </summary>
    }

}
