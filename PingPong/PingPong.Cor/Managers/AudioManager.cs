using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace PingPong.Corе.Managers
{
    /// <summary>
    /// Відтворює звуки гри.  
    /// Спочатку завантажує звуки через LoadSound, потім PlaySound.
    /// </summary>
    public class AudioManager
    {
        private readonly Dictionary<string, SoundPlayer> _sounds
            = new Dictionary<string, SoundPlayer>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Завантажує WAV-файл за ключем.
        /// </summary>
        /// <param name="key">Логічна назва звуку (наприклад, "score", "bounce").</param>
        /// <param name="filePath">Шлях до .wav-файлу.</param>
        public void LoadSound(string key, string filePath)
        {
            var player = new SoundPlayer(filePath);
            try
            {
                player.Load();
                _sounds[key] = player;
            }
            catch (Exception ex)
            {
                // Логування помилки завантаження
                Console.WriteLine($"AudioManager: не вдалося завантажити звук '{key}' з '{filePath}': {ex.Message}");
            }
        }

        /// <summary>
        /// Відтворює звук, якщо він був завантажений.
        /// </summary>
        public void PlaySound(string key)
        {
            if (_sounds.TryGetValue(key, out var player))
            {
                try
                {
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"AudioManager: помилка при відтворенні звуку '{key}': {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"AudioManager: звук '{key}' не знайдено.");
            }
        }
    }

}
