using System.IO;
using System.Media;

namespace ArgentRadio.Actions
{
    /// <summary>
    /// Acción de reprodución de un sonido
    /// </summary>
    [Action(InternalName = "playsound")]
    internal class PlaySoundAction : IAction
    {
        public static readonly PlaySoundAction Instance = new PlaySoundAction();

        /// <summary>
        /// Reproductor de sonido global para todas las acciones de sonido
        /// </summary>
        public static SoundPlayer SoundPlayer;

        public void Execute(params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return;
            }

            if (!File.Exists((string)args[0]))
            {
                return;
            }

            // Inicializar el reproductor de sonido si no lo está
            if (SoundPlayer == null)
            {
                SoundPlayer = new SoundPlayer();

                // Cargar el sonido si la ruta cambia
                SoundPlayer.SoundLocationChanged += (sender, e) =>
                {
                    SoundPlayer.LoadAsync();
                };

                // Parar el sonido anterior (si lo había) y reproducir el que se acaba de cargar
                SoundPlayer.LoadCompleted += (sender, e) =>
                {
                    SoundPlayer.Stop();
                    SoundPlayer.Play();
                };
            }

            if (SoundPlayer.SoundLocation == (string)args[0] && SoundPlayer.IsLoadCompleted)
            {
                SoundPlayer.Stop();
                SoundPlayer.Play();
                return;
            }

            SoundPlayer.SoundLocation = (string)args[0];
        }
    }
}
