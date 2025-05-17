using System.Windows;
using System.Windows.Input;

namespace PingPong.Core.Core
{
    public class InputManager
    {
        private readonly HashSet<Key> _pressedKeys = new();

        public void KeyDown(Key key)
        {
            _pressedKeys.Add(key);
        }

        public void KeyUp(Key key)
        {
            _pressedKeys.Remove(key);
        }

        /// <summary>
        /// Повертає напрямки, у яких наразі рухається гравець.
        /// </summary>
        public List<Direction> GetActiveDirections()
        {
            List<Direction> directions = new();

            if (_pressedKeys.Contains(Key.W)) directions.Add(Direction.Forward);
            if (_pressedKeys.Contains(Key.S)) directions.Add(Direction.Backward);
            if (_pressedKeys.Contains(Key.A)) directions.Add(Direction.Left);
            if (_pressedKeys.Contains(Key.D)) directions.Add(Direction.Right);

            return directions;
        }

        /// <summary>
        /// Чи натиснута клавіша миші?
        /// </summary>
        public bool IsMouseButtonDown(MouseButton button)
        {
            return Mouse.LeftButton == MouseButtonState.Pressed && button == MouseButton.Left ||
                   Mouse.RightButton == MouseButtonState.Pressed && button == MouseButton.Right;
        }

        /// <summary>
        /// Поточна позиція миші на екрані (вікні).
        /// </summary>
        public System.Windows.Point GetMousePosition()
        {
            return Mouse.GetPosition(System.Windows.Application.Current.MainWindow);
        }
    }
}
