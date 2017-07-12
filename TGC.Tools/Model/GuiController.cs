using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TGC.Tools.Example;
using TGC.Tools.Forms;
using TGC.Tools.MeshCreator;
using TGC.Tools.Utils;
using TGC.Tools.Utils._2D;
using TGC.Tools.Utils.Input;
using TGC.Tools.Utils.Modifiers;
using TGC.Tools.Utils.Shaders;
using TGC.Tools.Utils.Sound;
using TGC.Tools.Utils.TgcGeometry;
using TGC.Tools.Utils.TgcSceneLoader;

namespace TGC.Tools.Model
{
    /// <summary>
    ///     Controlador principal de la aplicaci�n
    /// </summary>
    public class GuiController
    {
        private TgcExample currentExample;

        private TgcD3dDevice tgcD3dDevice;

        #region Singleton

        private static volatile GuiController instance;

        /// <summary>
        ///     Permite acceder a una instancia de la clase GuiController desde cualquier parte del codigo.
        /// </summary>
        public static GuiController Instance
        {
            get { return instance; }
        }

        /// <summary>
        ///     Crear nueva instancia. Solo el MainForm lo hace
        /// </summary>
        internal static void newInstance()
        {
            instance = new GuiController();
        }

        #endregion Singleton

        #region Internal Methods

        private GuiController()
        {
        }

        /// <summary>
        ///     Crea todos los modulos necesarios de la aplicacion
        /// </summary>
        internal void initGraphics(ToolsForm mainForm, Control panel3d)
        {
            MainForm = mainForm;
            Panel3d = panel3d;
            panel3d.Focus();

            //Iniciar graficos
            tgcD3dDevice = new TgcD3dDevice(panel3d);
            TexturesManager = new TgcTexture.Manager();
            tgcD3dDevice.OnResetDevice(tgcD3dDevice.D3dDevice, null);

            //Iniciar otras herramientas
            TexturesPool = new TgcTexture.Pool();
            Text3d = new TgcDrawText(tgcD3dDevice.D3dDevice);
            D3dInput = new TgcD3dInput(mainForm, panel3d);
            FpsCamera = new TgcFpsCamera();
            RotCamera = new TgcRotationalCamera();
            ThirdPersonCamera = new TgcThirdPersonCamera();
            AxisLines = new TgcAxisLines(tgcD3dDevice.D3dDevice);
            Modifiers = new TgcModifiers(mainForm.getModifiersPanel());
            ElapsedTime = -1;
            Frustum = new TgcFrustum();
            Mp3Player = new TgcMp3Player();
            DirectSound = new TgcDirectSound();
            CurrentCamera = RotCamera;
            CustomRenderEnabled = false;
            Drawer2D = new TgcDrawer2D();
            Shaders = new TgcShaders();

            //toogles
            RotCamera.Enable = true;
            FpsCamera.Enable = false;
            ThirdPersonCamera.Enable = false;
            FpsCounterEnable = true;
            AxisLines.Enable = true;

            //Cargar shaders del framework
            Shaders.loadCommonShaders();

            //Cargar ejemplo default
            executeExample(new TgcMeshCreator());
        }

        /// <summary>
        ///     Hacer render del ejemplo
        /// </summary>
        internal void render()
        {
            var d3dDevice = tgcD3dDevice.D3dDevice;
            ElapsedTime = HighResolutionTimer.Instance.FrameTime;

            tgcD3dDevice.doClear();

            //Acutalizar input
            D3dInput.update();

            //Actualizar camaras (solo una va a estar activada a la vez)
            if (CurrentCamera.Enable)
            {
                CurrentCamera.updateCamera();
                CurrentCamera.updateViewMatrix(d3dDevice);
            }

            //actualizar posicion de pantalla en barra de estado de UI
            setStatusPosition();

            //actualizar el Frustum
            Frustum.updateVolume(d3dDevice.Transform.View, d3dDevice.Transform.Projection);

            //limpiar texturas
            TexturesManager.clearAll();

            //actualizar Listener3D
            DirectSound.updateListener3d();

            //Hacer render delegando control total al ejemplo
            if (CustomRenderEnabled)
            {
                //Ejecutar render del ejemplo
                if (currentExample != null)
                {
                    currentExample.render(ElapsedTime);
                }
            }

            //Hacer render asistido (mas sencillo para el ejemplo)
            else
            {
                //Iniciar escena 3D
                d3dDevice.BeginScene();

                //Actualizar contador de FPS si esta activo
                if (FpsCounterEnable)
                {
                    Text3d.drawText("FPS: " + HighResolutionTimer.Instance.FramesPerSecond, 0, 0, Color.Yellow);
                }

                //Ejecutar render del ejemplo
                if (currentExample != null)
                {
                    currentExample.render(ElapsedTime);
                }

                //Ejes cartesianos
                if (AxisLines.Enable)
                {
                    AxisLines.render();
                }

                //Finalizar escena 3D
                d3dDevice.EndScene();
            }

            d3dDevice.Present();
            //this.Invalidate();
        }

        /// <summary>
        ///     Cuando se selecciona un ejemplo para ejecutar del TreeNode
        /// </summary>
        internal void executeSelectedExample(TgcExample example)
        {
            executeExample(example);
        }

        /// <summary>
        ///     Arranca a ejecutar un ejemplo.
        ///     Para el ejemplo anterior, si hay alguno.
        /// </summary>
        /// <param name="example"></param>
        internal void executeExample(TgcExample example)
        {
            stopCurrentExample();
            Modifiers.clear();
            resetDefaultConfig();
            FpsCamera.resetValues();
            RotCamera.resetValues();
            ThirdPersonCamera.resetValues();

            example.init();

            currentExample = example;
            Panel3d.Focus();
            MainForm.setCurrentExampleStatus("Ejemplo actual: " + example.getName());
            MainForm.Text = example.getName();
            Debug.Write("Ejecutando ejemplo: " + example.getName());
        }

        /// <summary>
        ///     Deja de ejecutar el ejemplo actual
        /// </summary>
        internal void stopCurrentExample()
        {
            if (currentExample != null)
            {
                currentExample.close();
                tgcD3dDevice.resetWorldTransofrm();
                Debug.Write("Ejemplo " + currentExample.getName() + " terminado");
                currentExample = null;
                ElapsedTime = -1;
            }
        }

        /// <summary>
        ///     Finaliza la ejecuci�n de la aplicacion
        /// </summary>
        internal void shutDown()
        {
            if (currentExample != null)
            {
                currentExample.close();
            }
            tgcD3dDevice.shutDown();
            TexturesPool.clearAll();
        }

        /// <summary>
        ///     Termina y vuelve a empezar el ejemplo actual, si hay alguno ejecutando.
        /// </summary>
        internal void resetCurrentExample()
        {
            if (currentExample != null)
            {
                var exampleBackup = currentExample;
                stopCurrentExample();
                executeExample(exampleBackup);
            }
        }

        /// <summary>
        ///     Vuelve la configuracion de render y otras cosas a la configuracion inicial
        /// </summary>
        internal void resetDefaultConfig()
        {
            MainForm.resetMenuOptions();
            AxisLines.Enable = true;
            FpsCamera.Enable = false;
            RotCamera.Enable = true;
            CurrentCamera = RotCamera;
            ThirdPersonCamera.Enable = false;
            FpsCounterEnable = true;
            tgcD3dDevice.setDefaultValues();
            Mp3Player.closeFile();
            CustomRenderEnabled = false;
        }

        /// <summary>
        ///     Cuando el Direct3D Device se resetea.
        ///     Se reinica el ejemplo actual, si hay alguno.
        /// </summary>
        internal void onResetDevice()
        {
            var exampleBackup = currentExample;
            if (exampleBackup != null)
            {
                stopCurrentExample();
            }
            tgcD3dDevice.doResetDevice();
            if (exampleBackup != null)
            {
                executeExample(exampleBackup);
            }
        }

        /// <summary>
        ///     Imprime por consola la posicion actual de la pantalla.
        ///     Ideal para copiar y pegar esos valores
        /// </summary>
        internal void printCurrentPosition()
        {
            MainForm.setStatusPosition(FpsCamera.getPositionCode());
        }

        /// <summary>
        ///     Hace foco en el panel 3D de la aplicacion.
        ///     Es util para evitar que el foco quede en otro contro, por ej. un boton,
        ///     y que los controles de navegacion respondan mal
        /// </summary>
        internal void focus3dPanel()
        {
            Panel3d.Focus();
        }

        /// <summary>
        ///     Actualiza en la pantalla principal la posicion actual de la camara
        /// </summary>
        private void setStatusPosition()
        {
            //Actualizar el textbox en todos los cuadros reduce los FPS en algunas PC
            if (MainForm.MostrarPosicionDeCamaraEnable)
            {
                var pos = CurrentCamera.getPosition();
                var lookAt = CurrentCamera.getLookAt();
                var statusPosition = "Position: [" + TgcParserUtils.printFloat(pos.X) + ", " +
                                     TgcParserUtils.printFloat(pos.Y) + ", " + TgcParserUtils.printFloat(pos.Z) + "] " +
                                     "- LookAt: [" + TgcParserUtils.printFloat(lookAt.X) + ", " +
                                     TgcParserUtils.printFloat(lookAt.Y) + ", " + TgcParserUtils.printFloat(lookAt.Z) +
                                     "]";
                MainForm.setStatusPosition(statusPosition);
            }
        }

        #endregion Internal Methods

        #region Getters and Setters and Public Methods

        /// <summary>
        ///     Direct3D Device
        /// </summary>
        public Device D3dDevice
        {
            get { return tgcD3dDevice.D3dDevice; }
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
        ///     Utilidad para escribir texto dentro de la pantalla 3D
        /// </summary>
        public TgcDrawText Text3d { get; private set; }

        /// <summary>
        ///     Habilita o desactiva el contador de FPS
        /// </summary>
        public bool FpsCounterEnable { get; set; }

        /// <summary>
        ///     Utilidad para crear modificadores de variables de usuario, que son mostradas en el panel derecho de la aplicaci�n.
        /// </summary>
        public TgcModifiers Modifiers { get; private set; }

        /// <summary>
        ///     Utilidad para visualizar los ejes cartesianos
        /// </summary>
        public TgcAxisLines AxisLines { get; private set; }

        /// <summary>
        ///     Utilidad para acceder al Input de Teclado y Mouse
        /// </summary>
        public TgcD3dInput D3dInput { get; private set; }

        /// <summary>
        ///     Tiempo en segundos transcurridos desde el �ltimo frame.
        ///     Solo puede ser invocado cuando se esta ejecutando un bloque de render() de un TgcExample
        /// </summary>
        public float ElapsedTime { get; private set; }

        /// <summary>
        ///     Ventana principal de la aplicacion
        /// </summary>
        public ToolsForm MainForm { get; private set; }

        /// <summary>
        ///     Control gr�fico de .NET utilizado para el panel3D sobre el cual renderiza el
        ///     Device de Direct3D
        /// </summary>
        public Control Panel3d { get; private set; }

        /// <summary>
        ///     Frustum que representa el volumen de vision actual
        ///     Solo puede ser invocado cuando se esta ejecutando un bloque de render() de un TgcExample
        /// </summary>
        public TgcFrustum Frustum { get; private set; }

        /// <summary>
        ///     Pool de texturas
        /// </summary>
        public TgcTexture.Pool TexturesPool { get; private set; }

        /// <summary>
        ///     Herramienta para configurar texturas en el Device
        /// </summary>
        public TgcTexture.Manager TexturesManager { get; private set; }

        /// <summary>
        ///     Configura la posicion de la c�mara
        /// </summary>
        /// <param name="pos">Posici�n de la c�mara</param>
        /// <param name="lookAt">Punto hacia el cu�l se quiere ver</param>
        public void setCamera(Vector3 pos, Vector3 lookAt)
        {
            tgcD3dDevice.D3dDevice.Transform.View = Matrix.LookAtLH(pos, lookAt, new Vector3(0, 1, 0));

            //Imprimir posicion
            var statusPos = "Position: [" + TgcParserUtils.printFloat(pos.X) + ", " + TgcParserUtils.printFloat(pos.Y) +
                            ", " + TgcParserUtils.printFloat(pos.Z) + "] " +
                            "- LookAt: [" + TgcParserUtils.printFloat(lookAt.X) + ", " +
                            TgcParserUtils.printFloat(lookAt.Y) + ", " + TgcParserUtils.printFloat(lookAt.Z) + "]";
            MainForm.setStatusPosition(statusPos);
        }

        /// <summary>
        ///     Herramienta para reproducir archivos MP3s
        /// </summary>
        public TgcMp3Player Mp3Player { get; private set; }

        /// <summary>
        ///     Herramienta para manipular DirectSound
        /// </summary>
        public TgcDirectSound DirectSound { get; private set; }

        /// <summary>
        ///     Color con el que se limpia la pantalla
        /// </summary>
        public Color BackgroundColor
        {
            get { return tgcD3dDevice.ClearColor; }
            set { tgcD3dDevice.ClearColor = value; }
        }

        /// <summary>
        ///     C�mara actual que utiliza el framework
        /// </summary>
        public TgcCamera CurrentCamera { get; set; }

        /// <summary>
        ///     Activa o desactiva el renderizado personalizado.
        ///     Si esta activo, el ejemplo tiene la responsabilidad de hacer
        ///     el BeginScene y EndScene, y tampoco se dibuja el contador de FPS y Axis.
        /// </summary>
        public bool CustomRenderEnabled { get; set; }

        /// <summary>
        ///     Utilidad para renderizar Sprites 2D
        /// </summary>
        public TgcDrawer2D Drawer2D { get; private set; }

        /// <summary>
        ///     Utilidad para manejo de shaders
        /// </summary>
        public TgcShaders Shaders { get; private set; }

        #endregion Getters and Setters and Public Methods
    }
}