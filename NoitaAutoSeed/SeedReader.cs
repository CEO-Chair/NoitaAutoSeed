using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoitaSeedHelper;
public class SeedReader
{
    public bool Attached => NoitaProcess is not null && !NoitaProcess.HasExited;

    private Process? NoitaProcess;

    private nint NoitaHandle;

    private nint NoitaSeedPointer => NoitaProcess!.MainModule!.BaseAddress + 0xBEE850;

    public async Task AttachToNoita()
    {
        await Task.Run(() =>
        {
            while (!Attached)
            {
                IEnumerable<Process> noitaProcesses = Process.GetProcessesByName("noita").Where(x => !x.HasExited);
                int noitaProcessCount = noitaProcesses.Count();

                if (noitaProcessCount == 1)
                {
                    OpenProcess(noitaProcesses.First());
                }
                else if (noitaProcessCount > 1)
                {
                    OpenProcess(noitaProcesses.OrderByDescending(x => x.StartTime).First());
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        });
    }

    public uint ReadSeed()
    {
        byte[] readBuffer = new byte[4];

        NativeMethods.ReadProcessMemory(NoitaHandle, NoitaSeedPointer, readBuffer, readBuffer.Length, out _);

        return BitConverter.ToUInt32(readBuffer);
    }

    private void OpenProcess(Process process)
    {
        NoitaProcess = process;

        NoitaHandle = NativeMethods.OpenProcess(NativeMethods.PROCESS_VM_READ, false, (uint)NoitaProcess!.Id);
    }
}
