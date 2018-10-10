using System;
using System.Runtime.InteropServices;

namespace ArgentRadio.Samp
{
    /// <summary>
    /// Clase de ayuda para acceso avanzado a procesos
    /// </summary>
    internal static class ProcessAccess
    {
        /// <summary>
        /// Flags con las formas de abrir un handle para un proceso
        /// </summary>
        [Flags]
        internal enum AccessFlags
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        /// <summary>
        /// Obtiene el handle del proceso de la ventana que tiene foco
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Abre un handle del proceso con el PID y los flags de acceso especificados como
        /// parámetros
        /// </summary>
        /// <param name="dwDesiredAccess">flags de acceso</param>
        /// <param name="bInheritHandle">
        /// si es true, el proceso actual herederá el proceso abierto
        /// </param>
        /// <param name="dwProcessId">PID del proceso del cuál se obtendrá el handle</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle,
            int dwProcessId);

        /// <summary>
        /// Obtiene el PID del hilo de la ventana del proceso cuyo handle se pasa como parámetro
        /// </summary>
        /// <param name="hProcess">handle del proceso</param>
        /// <param name="dwProcessId">PID del hilo de la ventana</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowThreadProcessId(IntPtr hProcess, out int dwProcessId);

        /// <summary>
        /// Lee la memoria de un proceso (a un byte[])
        /// </summary>
        /// <param name="hProcess">handle del proceso</param>
        /// <param name="lpBaseAddress">dirección de memoria</param>
        /// <param name="lpBuffer">buffer donde guardar la memoria léida</param>
        /// <param name="dwSize">longitud del dato a leer</param>
        /// <param name="lpNumberOfBytesRead">número de bytes leídos</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress,
            byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// Lee la memoria de un proceso (a un int)
        /// </summary>
        /// <param name="hProcess">handle del proceso</param>
        /// <param name="lpBaseAddress">dirección de memoria</param>
        /// <param name="lpBuffer">dónde guardar la memoria léida</param>
        /// <param name="dwSize">longitud del dato a leer</param>
        /// <param name="lpNumberOfBytesRead">número de bytes leídos</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress,
            out int lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// Lee la memoria de un proceso (a un short)
        /// </summary>
        /// <param name="hProcess">handle del proceso</param>
        /// <param name="lpBaseAddress">dirección de memoria</param>
        /// <param name="lpBuffer">dónde guardar la memoria léida</param>
        /// <param name="dwSize">longitud del dato a leer</param>
        /// <param name="lpNumberOfBytesRead">número de bytes leídos</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress,
            out short lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);
    }
}