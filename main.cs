using System;
using System.Runtime.InteropServices;

class Program
{
    // Function declarations from the Nintendo SDK
    [DllImport("nn_acp_r")]
    private static extern void nn_acp_r_ShutdownAndRebootSystem(uint magic);

    [DllImport("nn_account")]
    private static extern Result nn_account_Initialize();

    [DllImport("nn_account")]
    private static extern Result nn_account_Finalize();

    [DllImport("nn_account")]
    private static extern Result nn_account_OpenPreselectedUser(out ulong userId);

    [DllImport("nn_account")]
    private static extern Result nn_account_GetUserNickname(out ulong userId, IntPtr buffer, ulong size);

    public static void Main()
    {
        Result result;

        // Initialize Nintendo Account
        result = nn_account_Initialize();
        if (result != Result.Success)
        {
            Console.WriteLine($"Failed to initialize Nintendo Account: {result}");
            return;
        }

        // Open the preselected user
        ulong userId;
        result = nn_account_OpenPreselectedUser(out userId);
        if (result != Result.Success)
        {
            Console.WriteLine($"Failed to open preselected user: {result}");
            nn_account_Finalize();
            return;
        }

        // Get the user's nickname
        const int bufferSize = 128;
        IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
        result = nn_account_GetUserNickname(out userId, buffer, bufferSize);
        if (result != Result.Success)
        {
            Console.WriteLine($"Failed to get user nickname: {result}");
        }
        else
        {
            string nickname = Marshal.PtrToStringAnsi(buffer);
            Console.WriteLine($"User's nickname: {nickname}");
        }

        // Free allocated memory
        Marshal.FreeHGlobal(buffer);

        // Shutdown and reboot the system
        nn_acp_r_ShutdownAndRebootSystem(0x120000000); // Replace with appropriate magic value

        // Finalize Nintendo Account
        nn_account_Finalize();
    }
}