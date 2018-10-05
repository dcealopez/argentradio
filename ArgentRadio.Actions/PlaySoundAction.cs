namespace ArgentRadio.Actions
{
    /// <summary>
    /// Acción de reprodución de un sonido
    /// </summary>
    [Action(InternalName = "playsound")]
    internal class PlaySoundAction : IAction
    {
        public static PlaySoundAction Instance = new PlaySoundAction();

        public void Execute(params object[] args)
        {
            // TODO: Implementar funcionalidad para reproducir un sonido            
        }
    }
}
