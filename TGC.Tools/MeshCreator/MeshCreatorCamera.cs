using Microsoft.DirectX.DirectInput;
using TGC.Core.BoundingVolumes;
using TGC.Core.Camara;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using Device = Microsoft.DirectX.Direct3D.Device;

namespace TGC.Tools.MeshCreator
{
    /// <summary>
    ///     Camara rotacional customizada para el MeshCreator
    /// </summary>
    public class MeshCreatorCamera : TgcCamera
    {
        public static float DEFAULT_ZOOM_FACTOR = 0.15f;
        public static float DEFAULT_CAMERA_DISTANCE = 10f;
        public static float DEFAULT_ROTATION_SPEED = 100f;
        private float diffX;
        private float diffY;
        private float diffZ;
        private TGCVector3 nextPos;

        private TGCVector3 upVector;
        private TGCMatrix viewTGCMatrix;

        private TgcD3dInput Input { get; }

        public MeshCreatorCamera(TgcD3dInput input)
        {
            Input = input;
            resetValues();
        }

        /// <summary>
        ///     Actualiza los valores de la camara
        /// </summary>
        public void updateCamera(float elapsedTime)
        {
            if (!Enable)
            {
                return;
            }

            //Obtener variacion XY del mouse
            var mouseX = 0f;
            var mouseY = 0f;
            if (Input.keyDown(Key.LeftAlt) && Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_MIDDLE))
            {
                mouseX = Input.XposRelative;
                mouseY = Input.YposRelative;

                diffX += mouseX * elapsedTime * RotationSpeed;
                diffY += mouseY * elapsedTime * RotationSpeed;
            }
            else
            {
                diffX += mouseX;
                diffY += mouseY;
            }

            //Calcular rotacion a aplicar
            var rotX = -diffY / FastMath.PI + BaseRotX;
            var rotY = diffX / FastMath.PI + BaseRotY;

            //Truncar valores de rotacion fuera de rango
            if (rotX > FastMath.PI * 2 || rotX < -FastMath.PI * 2)
            {
                diffY = 0;
                rotX = 0;
            }

            //Invertir Y de UpVector segun el angulo de rotacion
            if (rotX < -FastMath.PI / 2 && rotX > -FastMath.PI * 3 / 2)
            {
                upVector.Y = -1;
            }
            else if (rotX > FastMath.PI / 2 && rotX < FastMath.PI * 3 / 2)
            {
                upVector.Y = -1;
            }
            else
            {
                upVector.Y = 1;
            }

            //Determinar distancia de la camara o zoom segun el Mouse Wheel
            if (Input.WheelPos != 0)
            {
                diffZ += ZoomFactor * Input.WheelPos * -1;
            }
            var distance = -CameraDistance * diffZ;

            //Limitar el zoom a 0
            if (distance > 0)
            {
                distance = 0;
            }

            //Realizar Transformacion: primero alejarse en Z, despues rotar en X e Y y despues ir al centro de la cmara
            var m = TGCMatrix.Translation(0, 0, -distance)
                    * TGCMatrix.RotationX(rotX)
                    * TGCMatrix.RotationY(rotY)
                    * TGCMatrix.Translation(CameraCenter);

            //Extraer la posicion final de la matriz de transformacion
            nextPos.X = m.M41;
            nextPos.Y = m.M42;
            nextPos.Z = m.M43;

            //Hacer efecto de Pan View
            if (!Input.keyDown(Key.LeftAlt) && Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_MIDDLE))
            {
                var dx = -Input.XposRelative;
                var dy = Input.YposRelative;
                var panSpeedZoom = PanSpeed * FastMath.Abs(distance);

                var d = CameraCenter - nextPos;
                d.Normalize();

                var n = TGCVector3.Cross(d, upVector);
                n.Normalize();

                var up = TGCVector3.Cross(n, d);
                var desf = TGCVector3.Scale(up, dy * panSpeedZoom) - TGCVector3.Scale(n, dx * panSpeedZoom);
                nextPos = nextPos + desf;
                CameraCenter = CameraCenter + desf;
            }

            //Obtener ViewTGCMatrix haciendo un LookAt desde la posicion final anterior al centro de la camara
            viewTGCMatrix = TGCMatrix.LookAtLH(nextPos, CameraCenter, upVector);
        }

        /// <summary>
        ///     Actualiza la ViewTGCMatrix, si es que la camara esta activada
        /// </summary>
        public void updateViewTGCMatrix(Device d3dDevice)
        {
            if (!Enable)
            {
                return;
            }

            d3dDevice.Transform.View = viewTGCMatrix;
        }

        public TGCVector3 getPosition()
        {
            return nextPos;
        }

        public TGCVector3 getLookAt()
        {
            return CameraCenter;
        }

        /// <summary>
        ///     Carga los valores default de la camara
        /// </summary>
        internal void resetValues()
        {
            upVector = TGCVector3.Up;
            CameraCenter = TGCVector3.Empty;
            nextPos = TGCVector3.Empty;
            CameraDistance = DEFAULT_CAMERA_DISTANCE;
            ZoomFactor = DEFAULT_ZOOM_FACTOR;
            RotationSpeed = DEFAULT_ROTATION_SPEED;
            diffX = 0f;
            diffY = 0f;
            diffZ = 1f;
            viewTGCMatrix = TGCMatrix.Identity;
            PanSpeed = 0.01f;
            BaseRotX = 0;
            BaseRotY = 0;
        }

        /// <summary>
        ///     Setear la camara con una determinada posicion y lookAt
        /// </summary>
        public void lookAt(TGCVector3 pos, TGCVector3 lookAt)
        {
            //TODO: solo funciona bien para hacer un TopView

            var v = pos - lookAt;
            var length = TGCVector3.Length(v);
            v.Scale(1 / length);

            CameraDistance = length;
            upVector = TGCVector3.Up;
            diffX = 0;
            diffY = 0.01f;
            diffZ = 1;
            BaseRotX = -FastMath.Acos(TGCVector3.Dot(new TGCVector3(0, 0, -1), v));
            //baseRotY = FastMath.Acos(TGCVector3.Dot(new TGCVector3(0, 0, -1), v));
            BaseRotY = 0;
            CameraCenter = lookAt;
        }

        /// <summary>
        ///     Setear la camara en una determinada posicion, indicando que punto mira
        ///     y con que angulos se rota en el eje X y el Y.
        /// </summary>
        /// <param name="lookAt">Punto que se mira</param>
        /// <param name="rotX">Cuanto rotar en el eje X</param>
        /// <param name="rotY">Cuanto rotar en el eje Y</param>
        /// <param name="distance">Distancia de la camara desde el punto de lookAt</param>
        public void setFixedView(TGCVector3 lookAt, float rotX, float rotY, float distance)
        {
            CameraDistance = distance;
            upVector = TGCVector3.Up;
            diffX = 0;
            diffY = 0.01f;
            diffZ = 1;
            BaseRotX = rotX;
            BaseRotY = rotY;
            CameraCenter = lookAt;
        }

        /// <summary>
        ///     Configura los parámetros de la cámara en funcion del BoundingBox de un modelo
        /// </summary>
        /// <param name="boundingBox">BoundingBox en base al cual configurar</param>
        public void targetObject(TgcBoundingAxisAlignBox boundingBox)
        {
            CameraCenter = boundingBox.calculateBoxCenter();
            var r = boundingBox.calculateBoxRadius();
            CameraDistance = 2 * r;
        }

        #region Getters y Setters

        /// <summary>
        ///     Habilita o no el uso de la camara
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     Centro de la camara sobre la cual se rota
        /// </summary>
        public TGCVector3 CameraCenter { get; set; }

        /// <summary>
        ///     Distance entre la camara y el centro
        /// </summary>
        public float CameraDistance { get; set; }

        /// <summary>
        ///     Velocidad con la que se hace Zoom
        /// </summary>
        public float ZoomFactor { get; set; }

        /// <summary>
        ///     Velocidad de rotacion de la camara
        /// </summary>
        public float RotationSpeed { get; set; }

        /// <summary>
        ///     Velocidad de paneo
        /// </summary>
        public float PanSpeed { get; set; }

        /// <summary>
        ///     Rotacion inicial que siempre tiene en el eje X
        /// </summary>
        public float BaseRotX { get; set; }

        /// <summary>
        ///     Rotacion inicial que siempre tiene en el eje Y
        /// </summary>
        public float BaseRotY { get; set; }

        /// <summary>
        ///     Configura el centro de la camara, la distancia y la velocidad de zoom
        /// </summary>
        public void setCamera(TGCVector3 cameraCenter, float cameraDistance, float zoomFactor)
        {
            CameraCenter = cameraCenter;
            CameraDistance = cameraDistance;
            ZoomFactor = zoomFactor;
        }

        /// <summary>
        ///     Configura el centro de la camara, la distancia
        /// </summary>
        public void setCamera(TGCVector3 cameraCenter, float cameraDistance)
        {
            CameraCenter = cameraCenter;
            CameraDistance = cameraDistance;
            ZoomFactor = DEFAULT_ZOOM_FACTOR;
        }

        #endregion Getters y Setters
    }
}