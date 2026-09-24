using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace DualSenseGameBar
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "DualSenseGameBar_Unique_Mutex", out createdNew))
            {
                if (createdNew)
                {
                    Application.Run(new TrayApplicationContext());
                }
                else
                {
                    MessageBox.Show("La aplicación ya se está ejecutando en segundo plano.", "DualSense GameBar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

    public class TrayApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Thread listeningThread;

        public TrayApplicationContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Salir", Exit)
                }),
                Visible = true,
                Text = "DualSense GameBar"
            };

            listeningThread = new Thread(new ThreadStart(CoreLogic.StartListening));
            listeningThread.IsBackground = true;
            listeningThread.SetApartmentState(ApartmentState.STA);
            listeningThread.Start();
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }

    public class CoreLogic
    {
        [StructLayout(LayoutKind.Sequential)] 
        public struct JOYINFOEX { 
            public int dwSize, dwFlags, dwXpos, dwYpos, dwZpos, dwRpos, dwUpos, dwVpos, dwButtons, dwButtonNumber, dwPOV, dwReserved1, dwReserved2; 
        }
        [DllImport("winmm.dll")] public static extern int joyGetPosEx(int uJoyID, ref JOYINFOEX pji);

        [ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IApplicationActivationManager {
            IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] UInt32 options, [Out] out UInt32 processId);
        }
        [ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]
        class ApplicationActivationManager : IApplicationActivationManager {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public extern IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] UInt32 options, [Out] out UInt32 processId);
        }

        [DllImport("user32.dll")] static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public static void StartListening()
        {
            JOYINFOEX info = new JOYINFOEX();
            info.dwSize = Marshal.SizeOf(info);
            info.dwFlags = 0x80;

            IApplicationActivationManager appActiveManager = new ApplicationActivationManager();
            int[] lastButtons = new int[16];
            long lastTriggerTime = 0;

            while (true)
            {
                bool anyConnected = false;
                for (int i = 0; i < 16; i++)
                {
                    if (joyGetPosEx(i, ref info) == 0)
                    {
                        anyConnected = true;
                        int changed = (info.dwButtons ^ lastButtons[i]) & info.dwButtons;
                        
                        // Boton PS presionado
                        if ((changed & 4096) != 0 || (changed & 8192) != 0)
                        {
                            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            if (now - lastTriggerTime > 500)
                            {
                                // BYPASS: Otorgar token de Foreground al proceso oculto inyectando F24
                                keybd_event(0x87, 0, 0, UIntPtr.Zero);
                                keybd_event(0x87, 0, 0x02, UIntPtr.Zero);

                                // ABRIR GAME BAR EN UN HILO INDEPENDIENTE PARA EVITAR BLOQUEOS
                                Thread launcherThread = new Thread(() => {
                                    try {
                                        uint pid;
                                        appActiveManager.ActivateApplication("Microsoft.XboxGamingOverlay_8wekyb3d8bbwe!App", null, 0, out pid);
                                    } catch {}
                                });
                                launcherThread.SetApartmentState(ApartmentState.STA);
                                launcherThread.IsBackground = true;
                                launcherThread.Start();
                                
                                lastTriggerTime = now;
                            }
                        }
                        lastButtons[i] = info.dwButtons;
                    }
                    else
                    {
                        // Control desconectado en el slot 'i': limpiar memoria fantasma
                        lastButtons[i] = 0;
                    }
                }
                
                if (!anyConnected) Thread.Sleep(2000);
                else Thread.Sleep(30);
            }
        }
    }
}
