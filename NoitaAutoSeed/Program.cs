using NoitaSeedHelper;
using System.Diagnostics;

namespace NoitaAutoSeed;

public class Program
{
    public static async Task Main()
    {
        SeedReader reader = new SeedReader();

        uint lastSeed = 0;

        while (true)
        {
            if (!reader.Attached)
            {
                Console.WriteLine("Attaching to noita");
                await reader.AttachToNoita();
                Console.WriteLine("Attached");
            }

            uint seed = reader.ReadSeed();

            if (seed > 0 && lastSeed != seed)
            {
                lastSeed = seed;

                Console.WriteLine($"New seed: {seed}");

                // Open the seed in noitool
                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = $"https://noitool.com/?seed={seed}",
                });
            }

            Thread.Sleep(250);
        }
    }
}
