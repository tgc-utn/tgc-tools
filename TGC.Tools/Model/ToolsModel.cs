using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Shaders;
using TGC.Core.Sound;
using TGC.Core.Textures;
using TGC.Tools.Example;
using TGC.Tools.Forms;

namespace TGC.Tools.Model
{
    /// <summary>
    ///     Controlador principal de la aplicación
    /// </summary>
    public class ToolsModel
    {
        private ToolsForm Form { get; set; }

        /// <summary>
        /// Obtener o parar el estado del RenderLoop.
        /// </summary>
        public bool ApplicationRunning { get; set; }

        /// <summary>
        /// Controlador de sonido.
        /// </summary>
        public TgcDirectSound DirectSound { get; set; }

        /// <summary>
        /// Controlador de inputs.
        /// </summary>
        public TgcD3dInput Input { get; set; }

        /// <summary>
        /// Ejemplo actual.
        /// </summary>
        public TGCExampleTools CurrentExample { get; set; }

        /// <summary>
        ///     Control gráfico de .NET utilizado para el panel3D sobre el cual renderiza el
        ///     Device de Direct3D
        /// </summary>
        public Control Panel3d { get; private set; }

        #region Singleton

        /// <summary>
        ///     Constructor privado para poder hacer el singleton
        /// </summary>
        private ToolsModel() { }

        public static ToolsModel Instance { get; } = new ToolsModel();

        #endregion Singleton

        #region Internal Methods

        /// <summary>
        /// Inicia el device basado en el panel, el sonido, los inputs y carga los shaders basicos.
        /// </summary>
        /// <param name="form"> Ventana que contiene la aplicacion.</param>
        /// <param name="control"> Control donde van a correr los ejemplos.</param>
        /// <param name="pathCommonShaders"> Ruta con los shaders basicos.</param>
        public void InitGraphics(ToolsForm form, Panel control, string pathCommonShaders)
        {
            ApplicationRunning = true;
            Form = form;
            Panel3d = control;

            //Inicio Device
            D3DDevice.Instance.InitializeD3DDevice(control);
            D3DDevice.Instance.Device.DeviceReset += OnResetDevice;

            //Inicio inputs
            Input = new TgcD3dInput();
            Input.Initialize(Form, control);

            //Inicio sonido
            DirectSound = new TgcDirectSound();
            DirectSound.InitializeD3DDevice(control);

            //Cargar shaders del framework
            TgcShaders.Instance.loadCommonShaders(pathCommonShaders);
        }

        /// <summary>
        /// Verifica si existe la carpeta.
        /// </summary>
        /// <param name="path"> Ruta a verificar.</param>
        /// <returns> True si la carpeta no existe.</returns>
        public bool CheckFolder(string path)
        {
            return !Directory.Exists(path);
        }

        /// <summary>
        /// Se inicia el render loop del ejemplo.
        /// </summary>
        public void InitRenderLoop()
        {
            while (ApplicationRunning)
            {
                //Renderizo si es que hay un ejemplo activo
                if (CurrentExample != null)
                {
                    //Solo renderizamos si la aplicacion tiene foco, para no consumir recursos innecesarios
                    if (Form.ApplicationActive())
                    {
                        CurrentExample.Update();
                        CurrentExample.Render();
                    }
                    else
                    {
                        //Si no tenemos el foco, dormir cada tanto para no consumir gran cantidad de CPU
                        Thread.Sleep(100);
                    }
                }
                // Process application messages
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Actualiza el aspect ratio segun el estado del panel.
        /// </summary>
        /// <param name="panel"></param>
        public void UpdateAspectRatio(Panel panel)
        {
            D3DDevice.Instance.UpdateAspectRatioAndProjection(panel.Width, panel.Height);
        }

        /// <summary>
        /// Le activa o desactiva la herramienta de wireframe al ejemplo.
        /// </summary>
        /// <param name="state"> Estado que se quiere de la herramienta.</param>
        public void Wireframe(bool state)
        {
            if (state)
            {
                D3DDevice.Instance.FillModeWireFrame();
            }
            else
            {
                D3DDevice.Instance.FillModeWireSolid();
            }
        }

        /// <summary>
        /// Le activa o desactiva el contador de FPS al ejemplo.
        /// </summary>
        /// <param name="state"> Estado que se quiere de la herramienta.</param>
        public void ContadorFPS(bool state)
        {
            CurrentExample.FPS = state;
        }

        /// <summary>
        /// Le activa o desactiva los ejes cartesianos al ejemplo.
        /// </summary>
        /// <param name="state"> Estado que se quiere de la herramienta.</param>
        public void AxisLines(bool state)
        {
            CurrentExample.AxisLinesEnable = state;
        }

        /// <summary>
        ///     Arranca a ejecutar un ejemplo.
        ///     Para el ejemplo anterior, si hay alguno.
        /// </summary>
        /// <param name="example"></param>
        internal void ExecuteExample(TGCExampleTools example)
        {
            StopCurrentExample();

            //Ejecutar Init
            CurrentExample = example;
            //TODO esto no me cierra mucho OnResetDevice
            OnResetDevice(D3DDevice.Instance.Device, null);
            example.ResetDefaultConfig();
            example.DirectSound = DirectSound;
            example.Input = Input;
            example.Init();
        }

        /// <summary>
        /// Limpia el atributo de ExamplerLoader.
        /// </summary>
        public void ClearCurrentExample()
        {
            CurrentExample = null;
        }

        public void Dispose()
        {
            ApplicationRunning = false;

            StopCurrentExample();

            //Liberar Device al finalizar la aplicacion
            D3DDevice.Instance.Dispose();
            TexturesPool.Instance.clearAll();
        }

        /// <summary>
        ///  Deja de ejecutar el ejemplo actual
        /// </summary>
        public void StopCurrentExample()
        {
            if (CurrentExample != null)
            {
                CurrentExample.Dispose();
                CurrentExample = null;
            }
        }

        /// <summary>
        /// This event-handler is a good place to create and initialize any Direct3D related objects, which may become invalid during a device reset.
        /// </summary>
        public void OnResetDevice(object sender, EventArgs e)
        {
            //TODO antes hacia esto que no entiendo porque ToolsModel.Instance.onResetDevice();
            //pero solo detenia el ejemplo ejecutaba doResetDevice y lo volvia a cargar...
            DoResetDevice();
        }

        /// <summary>
        /// Hace las operaciones de Reset del device.
        /// </summary>
        public void DoResetDevice()
        {
            //Default values para el device
            CurrentExample.DeviceDefaultValues();

            //Reset Timer
            CurrentExample.ResetTimer();
        }

        #endregion Internal Methods
    }
}