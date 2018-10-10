using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ArgentRadio.Samp
{
    /// <summary>
    /// Clase estática para obtener información del
    /// proceso de SAMP
    /// </summary>
    public static class SampInfo
    {
        /// <summary>
        /// Direcciones de memoria y offsets de SAMP
        /// </summary>
        internal enum Address
        {
            /// <summary>
            /// Dirección de memoria donde se ubica el nombre del jugador local
            /// </summary>
            LocalPlayerNameAddress = 0x219A77,

            /// <summary>
            /// Dirección de memoria donde se ubica la información del chat
            /// </summary>
            SampChatInfoAddress = 0x21A0EC,

            /// <summary>
            /// Dirección de memoria donde se ubica la información de SAMP
            /// </summary>
            SampInfoAddress = 0x21A100,

            /// <summary>
            /// Dirección de memoria donde se ubica la información de la pool de jugadores
            /// </summary>
            SampPlayerPoolInfoAddress = 0x3C5,

            /// <summary>
            /// Dirección de memoria donde se ubica la información de la pool del jugador local
            /// </summary>
            SampLocalPlayerPoolInfoAddress = 0x8,

            /// <summary>
            /// (Offset) Dirección de memoria donde se ubica la dirección IP del servidor al que
            /// se está conectado
            /// </summary>
            SampServerIpAddressOffset = 0x1C,

            /// <summary>
            /// (Offset) Dirección de memoria donde se ubica el ID del jugador local
            /// </summary>
            SampLocalPlayerIdAddressOffset = 0x0
        }

        /// <summary>
        /// Límites de longitud de datos de SAMP
        /// </summary>
        internal enum Limit
        {
            /// <summary>
            /// Tamaño máximo para el nombre de un jugador
            /// </summary>
            PlayerNameMaxLength = 25,

            /// <summary>
            /// Tamaño máximo para el nombre de un servidor
            /// </summary>
            ServerNameMaxLength = 255,

            /// <summary>
            /// Tamaño máximo para la IP del servidor
            /// </summary>
            ServerIpMaxLength = 257,

            /// <summary>
            /// Tamaño máximo de un mensaje de chat
            /// </summary>
            MaxChatMessageMaxLength = 144,

            /// <summary>
            /// Cantidad máxima de entradas de chat que SAMP almacena internamente
            /// </summary>
            MaxChatEntries = 99
        }

        /// <summary>
        /// Dirección de memoria base del módulo "samp.dll" del proceso "gta_sa"
        /// </summary>
        internal static IntPtr SampModuleBaseAddress;

        /// <summary>
        /// Handle del proceso "gta_sa"
        /// </summary>
        internal static IntPtr GtaSaProcessHandle;

        /// <summary>
        /// Inicializa la clase estableciendo la dirección base del módulo
        /// "samp.dll" y el handle del proceso "gta_sa"
        /// </summary>
        public static void Initialize()
        {
            GtaSaProcessHandle = GetGtaSaProcessHandle();
            SampModuleBaseAddress = GetSampModuleBaseAddress();
        }

        /// <summary>
        /// Comprueba si el proceso "gta_sa" está en ejecución
        /// </summary>
        /// <returns>true si el proceso "gta_sa" está en ejecución, false si no</returns>
        public static bool IsGtaSaRunning()
        {
            return GetGtaSaProcess() != null;
        }

        /// <summary>
        /// Comprueba si la ventana del proceso "gta_sa" tiene el foco
        /// </summary>
        /// <returns>true si la ventana "gta_sa" tiene el foco, false si no</returns>
        public static bool IsGtaSaWindowFocused()
        {
            var gtaSaProcess = GetGtaSaProcess();

            if (gtaSaProcess == null)
            {
                return false;
            }

            IntPtr activeWindowProcessHandle = ProcessAccess.GetForegroundWindow();

            if (activeWindowProcessHandle == IntPtr.Zero)
            {
                return false;
            }

            ProcessAccess.GetWindowThreadProcessId(activeWindowProcessHandle,
                out int activeWindowProcessId);

            return gtaSaProcess.Id == activeWindowProcessId;
        }

        /// <summary>
        /// Devuelve el nombre del jugador local
        /// </summary>
        /// <returns>el nombre del jugador local</returns>
        public static string GetLocalPlayerName()
        {
            if (GtaSaProcessHandle == IntPtr.Zero)
            {
                return string.Empty;
            }

            var buffer = new byte[(int) Limit.PlayerNameMaxLength];

            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                (int) SampModuleBaseAddress + (int) Address.LocalPlayerNameAddress,
                buffer, buffer.Length, IntPtr.Zero);

            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Devuelve el ID del jugador local
        /// </summary>
        /// <returns>el ID del jugador local</returns>
        public static short GetLocalPlayerId()
        {
            if (GtaSaProcessHandle == IntPtr.Zero)
            {
                return -1;
            }

            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                (int) SampModuleBaseAddress + (int) Address.SampInfoAddress,
                out int previousAddress, sizeof(int), IntPtr.Zero);

            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                previousAddress + (int) Address.SampPlayerPoolInfoAddress,
                out previousAddress, sizeof(int), IntPtr.Zero);

            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                previousAddress + (int) Address.SampLocalPlayerPoolInfoAddress,
                out previousAddress, sizeof(int), IntPtr.Zero);

            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                previousAddress + (int) Address.SampLocalPlayerIdAddressOffset,
                out short localPlayerId, sizeof(short), IntPtr.Zero);

            return localPlayerId;
        }

        /// <summary>
        /// Devuelve un mensaje del chat
        /// </summary>
        /// <param name="index">
        /// el índice del mensaje de chat a recuperar (siendo 99 el último recibido)
        /// </param>
        /// <returns>el mensaje del chat con el índice especificado</returns>
        public static string GetChatMessage(int index)
        {
            if (GtaSaProcessHandle == IntPtr.Zero)
            {
                return string.Empty;
            }

            if (index < 0 || index > (int) Limit.MaxChatEntries)
            {
                return string.Empty;
            }

            var message = new byte[(int) Limit.MaxChatMessageMaxLength];

            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                (int) SampModuleBaseAddress + (int) Address.SampChatInfoAddress,
                out int previousAddress, sizeof(int), IntPtr.Zero);

            // Nos posicionamos en la estructura donde se almacenan los mensajes
            // Cada mensaje ocupa 252 bytes, así que nos posicionamos en el mensaje directamente
            // Sólo queremos el mensaje, así que avanzamos 28 bytes más para acceder a él
            ProcessAccess.ReadProcessMemory(GtaSaProcessHandle,
                previousAddress + ((0x136 + (index * 252)) + 28),
                message, message.Length, IntPtr.Zero);

            return Encoding.UTF8.GetString(message, 0, message.Length).Trim();
        }

        /// <summary>
        /// Obtiene el proceso "gta_sa" si está en ejecución
        /// </summary>
        /// <returns>el proceso "gta_sa" si está en ejecución, null si no</returns>
        internal static Process GetGtaSaProcess()
        {
            return Process.GetProcesses()
                .FirstOrDefault(process => process.ProcessName == "gta_sa");
        }

        /// <summary>
        /// Obtiene el handle del proceso "gta_sa" si está en ejecución
        /// </summary>
        /// <returns>
        /// el handle del proceso "gta_sa" si está en ejecución, puntero nulo si no
        /// </returns>
        internal static IntPtr GetGtaSaProcessHandle()
        {
            var gtaSaProcess = GetGtaSaProcess();

            return gtaSaProcess == null
                ? IntPtr.Zero
                : ProcessAccess.OpenProcess((int) ProcessAccess.AccessFlags.VirtualMemoryRead,
                    false, gtaSaProcess.Id);
        }

        /// <summary>
        /// Obtiene la dirección de memoria base del módulo "samp.dll"
        /// del proceso "gta_sa" si está en ejecución
        /// </summary>
        /// <returns>
        /// la dirección de memoria base del módulo "samp.dll" del proceso "gta_sa" si está en
        /// ejecución, puntero nulo si no
        /// </returns>
        internal static IntPtr GetSampModuleBaseAddress()
        {
            var gtaSaProcess = GetGtaSaProcess();

            return gtaSaProcess?.Modules.OfType<ProcessModule>()
                       .Where(processModule => processModule.ModuleName == "samp.dll")
                       .Select(processModule => processModule.BaseAddress)
                       .FirstOrDefault() ?? IntPtr.Zero;
        }
    }
}