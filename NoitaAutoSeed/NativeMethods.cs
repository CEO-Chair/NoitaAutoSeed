using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NoitaSeedHelper;
public class NativeMethods
{
    [DllImport("kernel32.dll")]
    public static extern nint OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(nint hProcess, nint lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

    public static readonly uint PROCESS_VM_READ = 0x10;
}
