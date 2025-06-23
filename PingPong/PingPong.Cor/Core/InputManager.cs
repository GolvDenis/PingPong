using PingPong.Corе.Helpers;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace PingPong.Core.Core
{
    public class InputManager
    {
        private readonly MouseHelper _mouse;
        private readonly UIElement _element;
        public Point MousePosition2D { get; private set; }
        public Point3D MousePosition3D => _mouse.ScreenToWorldOnPlane(MousePosition2D, 0.76);
        public bool IsLeftPressed { get; private set; }
        public Vector KeyboardDir { get; private set; }

        public InputManager(UIElement element, MouseHelper mouse)
        {
            _element = element;
            _mouse = mouse;
            Attach();
        }

        private void Attach()
        {
            _element.MouseMove += (s, e) => MousePosition2D = e.GetPosition(_element);
            _element.MouseDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) IsLeftPressed = true; };
            _element.MouseUp += (s, e) => { if (e.LeftButton == MouseButtonState.Released) IsLeftPressed = false; };
            _element.KeyDown += OnKey;
            _element.KeyUp += OnKeyUp;
            _element.Focusable = true;
            _element.Focus();
        }

        private void OnKey(object s, KeyEventArgs e)
        {
            KeyboardDir = e.Key switch
            { Key.W => new Vector(0, 1), Key.S => new Vector(0, -1), Key.A => new Vector(-1, 0), Key.D => new Vector(1, 0), _ => KeyboardDir };
        }

        private void OnKeyUp(object s, KeyEventArgs e) => KeyboardDir = new Vector(0, 0);


    }
}
