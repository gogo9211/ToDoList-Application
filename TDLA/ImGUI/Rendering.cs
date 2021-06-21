using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TDLA.DBController;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace TDLA.ImGUI
{
    class Rendering
    {
        private static Sdl2Window window;
        private static GraphicsDevice gd;
        private static CommandList cl;
        private static ImGuiController controller;

        private static Vector3 clearColor = new Vector3(0.45f, 0.55f, 0.6f);
        public static void RenderUI()
        {
            VeldridStartup.CreateWindowAndGraphicsDevice(new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "ToDo List Application"), new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true), out window, out gd);

            window.Resized += () =>
            {
                gd.MainSwapchain.Resize((uint)window.Width, (uint)window.Height);
                controller.WindowResized(window.Width, window.Height);
            };

            cl = gd.ResourceFactory.CreateCommandList();
            controller = new ImGuiController(gd, gd.MainSwapchain.Framebuffer.OutputDescription, window.Width, window.Height);

            while (window.Exists)
            {
                InputSnapshot snapshot = window.PumpEvents();
                if (!window.Exists)
                    break;

                controller.Update(1f / 60f, snapshot);

                Program.ui.DrawUI();

                cl.Begin();
                cl.SetFramebuffer(gd.MainSwapchain.Framebuffer);
                cl.ClearColorTarget(0, new RgbaFloat(clearColor.X, clearColor.Y, clearColor.Z, 1f));

                controller.Render(gd, cl);

                cl.End();
                gd.SubmitCommands(cl);
                gd.SwapBuffers(gd.MainSwapchain);
            }

            gd.WaitForIdle();

            controller.Dispose();
            cl.Dispose();
            gd.Dispose();
        }
    }
}
