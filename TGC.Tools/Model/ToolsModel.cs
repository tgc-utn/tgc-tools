using Microsoft.DirectX.Direct3D;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Shaders;
using TGC.Core.Sound;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Tools.Camara;
using TGC.Tools.Example;
using TGC.Tools.Forms;
using TGC.Tools.Properties;

namespace TGC.Tools.Model
{
    /// <summary>
    ///     Controlador principal de la aplicaci�n
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

        #region Singleton

        /// <summary>
        ///     Constructor privado para poder hacer el singleton
        /// </summary>
        private ToolsModel()
        {
        }

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

            //Inicio Device
            //D3DDevice.Instance.InitializeD3DDevice(control);
            //D3DDevice.Instance.Device.DeviceReset += OnResetDevice;

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

        #region Getters and Setters and Public Methods

        /// <summary>
        ///     Direct3D Device
        /// </summary>
        public Device D3dDevice
        {
            get { return D3DDevice.Instance.Device; }
        }

        /// <summary>
        ///     Utilidad de camara en Primera Persona
        /// </summary>
        public TgcFpsCamera FpsCamera { get; private set; }

        /// <summary>
        ///     Utilidad de camara que rota alrededor de un objetivo
        /// </summary>
        public TgcRotationalCamera RotCamera { get; private set; }

        /// <summary>
        ///     Utilidad de camara en tercera persona que sigue a un objeto que se mueve desde atr�s
        /// </summary>
        public TgcThirdPersonCamera ThirdPersonCamera { get; private set; }

        /// <summary>
        ///     Tiempo en segundos transcurridos desde el �ltimo frame.
        ///     Solo puede ser invocado cuando se esta ejecutando un bloque de render() de un TgcExample
        /// </summary>
        public float ElapsedTime { get; private set; }

        /// <summary>
        ///     Control gr�fico de .NET utilizado para el panel3D sobre el cual renderiza el
        ///     Device de Direct3D
        /// </summary>
        public Control Panel3d { get; private set; }

        /// <summary>
        ///     Herramienta para configurar texturas en el Device
        /// </summary>
        public TexturesManager TexturesManager { get; private set; }

        /// <summary>
        ///     Configura la posicion de la c�mara
        /// </summary>
        /// <param name="pos">Posici�n de la c�mara</param>
        /// <param name="lookAt">Punto hacia el cu�l se quiere ver</param>
        public void setCamera(TGCVector3 pos, TGCVector3 lookAt)
        {
            D3dDevice.Transform.View = TGCMatrix.LookAtLH(pos, lookAt, TGCVector3.Up);

            //Imprimir posicion
            var statusPos = "Position: [" + TgcParserUtils.printFloat(pos.X) + ", " + TgcParserUtils.printFloat(pos.Y) +
                            ", " + TgcParserUtils.printFloat(pos.Z) + "] " +
                            "- LookAt: [" + TgcParserUtils.printFloat(lookAt.X) + ", " +
                            TgcParserUtils.printFloat(lookAt.Y) + ", " + TgcParserUtils.printFloat(lookAt.Z) + "]";
            Form.setStatusPosition(statusPos);
        }

        /// <summary>
        ///     C�mara actual que utiliza el framework
        /// </summary>
        public TgcCamera CurrentCamera { get; set; }

        /// <summary>
        ///     Utilidad para manejo de shaders
        /// </summary>
        public TgcShaders Shaders { get; private set; }

        public String Media
        {
            get { return Settings.Default.MediaDirectory; }
        }

        #endregion Getters and Setters and Public Methods
    }
}