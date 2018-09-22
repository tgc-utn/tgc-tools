using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Diagnostics;
using TGC.Core.Camara;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.Utils;

namespace TGC.Tools.TerrainEditor
{
    /// <summary>
    ///     Copypaste de TgcFPSCamera con algunas modificaciones para que se adapte a la altura del terreno.
    /// </summary>
    public class TerrainFpsCamera : TgcCamera
    {
        //Constantes de movimiento
        private const float DEFAULT_ROTATION_SPEED = 2f;

        private const float DEFAULT_MOVEMENT_SPEED = 100f;
        private const float HEAD_POSITION = 50f;
        private const float DEFAULT_JUMP_SPEED = 100f;
        private readonly TGCVector3 CAMERA_ACCELERATION = new TGCVector3(400f, 400f, 400f);
        private readonly TGCVector3 CAMERA_POS = new TGCVector3(0.0f, 1.0f, 0.0f);

        private readonly TGCVector3 CAMERA_VELOCITY = new TGCVector3(DEFAULT_MOVEMENT_SPEED, DEFAULT_JUMP_SPEED,
            DEFAULT_MOVEMENT_SPEED);

        private readonly TGCVector3 DEFAULT_UP_VECTOR = new TGCVector3(0.0f, 1.0f, 0.0f);

        //Ejes para ViewTGCMatrix
        private readonly TGCVector3 WORLD_XAXIS = new TGCVector3(1.0f, 0.0f, 0.0f);

        private readonly TGCVector3 WORLD_YAXIS = new TGCVector3(0.0f, 1.0f, 0.0f);
        private readonly TGCVector3 WORLD_ZAXIS = new TGCVector3(0.0f, 0.0f, 1.0f);

        private float accumPitchDegrees;
        private TGCVector3 eye;
        private bool moveBackwardsPressed;
        private bool moveDownPressed;

        //Banderas de Input
        private bool moveForwardsPressed;

        private bool moveLeftPressed;
        private bool moveRightPressed;
        private bool moveUpPressed;
        private TGCVector3 viewDir;
        private TGCVector3 xAxis;
        private TGCVector3 yAxis;
        private TGCVector3 zAxis;

        private TgcD3dInput Input { get; }

        /// <summary>
        ///     Crea la cámara con valores iniciales.
        ///     Aceleración desactivada por Default
        /// </summary>
        public TerrainFpsCamera(SmartTerrain terrain)
        {
            Terrain = terrain;
            resetValues();
        }

        /// <summary>
        ///     Crea la cámara con valores iniciales.
        ///     Aceleración desactivada por Default
        /// </summary>
        public TerrainFpsCamera(TgcD3dInput input)
        {
            Input = input;
            resetValues();
        }

        /// <summary>
        ///     Actualiza los valores de la camara
        /// </summary>
        public override void UpdateCamera(float elapsedTime)
        {
            //Imprimir por consola la posicion actual de la camara
            if ((Input.keyDown(Key.LeftShift) || Input.keyDown(Key.RightShift)) && Input.keyPressed(Key.P))
            {
                Debug.Write(TGCVector3.PrintVector3(getPosition()));
                return;
            }

            var heading = 0.0f;
            var pitch = 0.0f;

            //Obtener direccion segun entrada de teclado
            var direction = getMovementDirection(Input);

            pitch = Input.YposRelative * RotationSpeed;
            heading = Input.XposRelative * RotationSpeed;

            //Solo rotar si se esta aprentando el boton del mouse configurado
            if (Input.buttonDown(RotateMouseButton))
            {
                rotate(heading, pitch, 0.0f);
            }

            updatePosition(direction, elapsedTime);
        }

        public TGCVector3 getPosition()
        {
            return eye;
        }

        /// <summary>
        ///     Carga los valores default de la camara
        /// </summary>
        public void resetValues()
        {
            accumPitchDegrees = 0.0f;
            RotationSpeed = DEFAULT_ROTATION_SPEED;
            eye = new TGCVector3(0.0f, 0.0f, 0.0f);
            xAxis = new TGCVector3(1.0f, 0.0f, 0.0f);
            yAxis = new TGCVector3(0.0f, 1.0f, 0.0f);
            zAxis = new TGCVector3(0.0f, 0.0f, 1.0f);
            viewDir = new TGCVector3(0.0f, 0.0f, 1.0f);
            LookAt = eye + viewDir;
            HeadPosition = HEAD_POSITION;
            AccelerationEnable = false;
            Acceleration = CAMERA_ACCELERATION;
            currentVelocity = new TGCVector3(0.0f, 0.0f, 0.0f);
            velocity = CAMERA_VELOCITY;
            viewTGCMatrix = TGCMatrix.Identity;
            setPosition(CAMERA_POS + HeadPosition * WORLD_YAXIS);

            RotateMouseButton = TgcD3dInput.MouseButtons.BUTTON_LEFT;
        }

        /// <summary>
        ///     Configura la posicion de la cámara
        /// </summary>
        private void setCamera(TGCVector3 eye, TGCVector3 target, TGCVector3 up)
        {
            this.eye = eye;

            zAxis = target - eye;
            zAxis.Normalize();

            viewDir = zAxis;
            LookAt = eye + viewDir;

            xAxis = TGCVector3.Cross(up, zAxis);
            xAxis.Normalize();

            yAxis = TGCVector3.Cross(zAxis, xAxis);
            yAxis.Normalize();
            //xAxis.Normalize();

            viewTGCMatrix = TGCMatrix.Identity;

            viewTGCMatrix.M11 = xAxis.X;
            viewTGCMatrix.M21 = xAxis.Y;
            viewTGCMatrix.M31 = xAxis.Z;
            viewTGCMatrix.M41 = -TGCVector3.Dot(xAxis, eye);

            viewTGCMatrix.M12 = yAxis.X;
            viewTGCMatrix.M22 = yAxis.Y;
            viewTGCMatrix.M32 = yAxis.Z;
            viewTGCMatrix.M42 = -TGCVector3.Dot(yAxis, eye);

            viewTGCMatrix.M13 = zAxis.X;
            viewTGCMatrix.M23 = zAxis.Y;
            viewTGCMatrix.M33 = zAxis.Z;
            viewTGCMatrix.M43 = -TGCVector3.Dot(zAxis, eye);

            // Extract the pitch angle from the view TGCMatrix.
            accumPitchDegrees = Geometry.RadianToDegree((float)-Math.Asin(viewTGCMatrix.M23));
        }

        /// <summary>
        ///     Configura la posicion de la cámara
        /// </summary>
        public void setCamera(TGCVector3 pos, TGCVector3 lookAt)
        {
            setCamera(pos, lookAt, DEFAULT_UP_VECTOR);
        }

        /// <summary>
        ///     Moves the camera by dx world units to the left or right; dy
        ///     world units upwards or downwards; and dz world units forwards
        ///     or backwards.
        /// </summary>
        private void move(float dx, float dy, float dz)
        {
            var auxEye = eye;
            TGCVector3 forwards;

            // Calculate the forwards direction. Can't just use the camera's local
            // z axis as doing so will cause the camera to move more slowly as the
            // camera's view approaches 90 degrees straight up and down.
            forwards = TGCVector3.Cross(xAxis, WORLD_YAXIS);
            forwards.Normalize();

            auxEye += xAxis * dx;
            auxEye += WORLD_YAXIS * dy;
            auxEye += forwards * dz;
            if (FpsModeEnable)
            {
                HeadPosition += dy;

                float y;
                if (Terrain.interpoledHeight(auxEye.X, auxEye.Z, out y))
                {
                    auxEye.Y = y;
                    auxEye += HeadPosition * WORLD_YAXIS;
                }
            }
            setPosition(auxEye);
        }

        /// <summary>
        ///     Rotates the camera based on its current behavior.
        ///     Note that not all behaviors support rolling.
        ///     This Camera class follows the left-hand rotation rule.
        ///     Angles are measured clockwise when looking along the rotation
        ///     axis toward the origin. Since the Z axis is pointing into the
        ///     screen we need to negate rolls.
        /// </summary>
        private void rotate(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            rollDegrees = -rollDegrees;
            rotateFirstPerson(headingDegrees, pitchDegrees);
            reconstructViewTGCMatrix(true);
        }

        /// <summary>
        ///     This method applies a scaling factor to the rotation angles prior to
        ///     using these rotation angles to rotate the camera. This method is usually
        ///     called when the camera is being rotated using an input device (such as a
        ///     mouse or a joystick).
        /// </summary>
        private void rotateSmoothly(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            headingDegrees *= RotationSpeed;
            pitchDegrees *= RotationSpeed;
            rollDegrees *= RotationSpeed;

            rotate(headingDegrees, pitchDegrees, rollDegrees);
        }

        /// <summary>
        ///     Moves the camera using Newton's second law of motion. Unit mass is
        ///     assumed here to somewhat simplify the calculations. The direction vector
        ///     is in the range [-1,1].
        /// </summary>
        private void updatePosition(TGCVector3 direction, float elapsedTimeSec)
        {
            if (TGCVector3.LengthSq(currentVelocity) != 0.0f)
            {
                // Only move the camera if the velocity vector is not of zero length.
                // Doing this guards against the camera slowly creeping around due to
                // floating point rounding errors.

                TGCVector3 displacement;
                if (AccelerationEnable)
                {
                    displacement = currentVelocity * elapsedTimeSec +
                                   0.5f * Acceleration * elapsedTimeSec * elapsedTimeSec;
                }
                else
                {
                    displacement = currentVelocity * elapsedTimeSec;
                }

                // Floating point rounding errors will slowly accumulate and cause the
                // camera to move along each axis. To prevent any unintended movement
                // the displacement vector is clamped to zero for each direction that
                // the camera isn't moving in. Note that the updateVelocity() method
                // will slowly decelerate the camera's velocity back to a stationary
                // state when the camera is no longer moving along that direction. To
                // account for this the camera's current velocity is also checked.

                if (direction.X == 0.0f && Math.Abs(currentVelocity.X) < 1e-6f)
                    displacement.X = 0.0f;

                if (direction.Y == 0.0f && Math.Abs(currentVelocity.Y) < 1e-6f)
                    displacement.Y = 0.0f;

                if (direction.Z == 0.0f && Math.Abs(currentVelocity.Z) < 1e-6f)
                    displacement.Z = 0.0f;

                move(displacement.X, displacement.Y, displacement.Z);
            }

            // Continuously update the camera's velocity vector even if the camera
            // hasn't moved during this call. When the camera is no longer being moved
            // the camera is decelerating back to its stationary state.

            if (AccelerationEnable)
            {
                updateVelocity(direction, elapsedTimeSec);
            }
            else
            {
                updateVelocityNoAcceleration(direction);
            }
        }

        private void setPosition(TGCVector3 pos)
        {
            eye = pos;

            reconstructViewTGCMatrix(false);
        }

        private void rotateFirstPerson(float headingDegrees, float pitchDegrees)
        {
            accumPitchDegrees += pitchDegrees;

            if (accumPitchDegrees > 90.0f)
            {
                pitchDegrees = 90.0f - (accumPitchDegrees - pitchDegrees);
                accumPitchDegrees = 90.0f;
            }

            if (accumPitchDegrees < -90.0f)
            {
                pitchDegrees = -90.0f - (accumPitchDegrees - pitchDegrees);
                accumPitchDegrees = -90.0f;
            }

            var heading = Geometry.DegreeToRadian(headingDegrees);
            var pitch = Geometry.DegreeToRadian(pitchDegrees);

            TGCMatrix rotMtx;
            Vector4 result;

            // Rotate camera's existing x and z axes about the world y axis.
            if (heading != 0.0f)
            {
                rotMtx = TGCMatrix.RotationY(heading);

                result = TGCVector3.Transform(xAxis, rotMtx);
                xAxis = new TGCVector3(result.X, result.Y, result.Z);

                result = TGCVector3.Transform(zAxis, rotMtx);
                zAxis = new TGCVector3(result.X, result.Y, result.Z);
            }

            // Rotate camera's existing y and z axes about its existing x axis.
            if (pitch != 0.0f)
            {
                rotMtx = TGCMatrix.RotationAxis(xAxis, pitch);

                result = TGCVector3.Transform(yAxis, rotMtx);
                yAxis = new TGCVector3(result.X, result.Y, result.Z);

                result = TGCVector3.Transform(zAxis, rotMtx);
                zAxis = new TGCVector3(result.X, result.Y, result.Z);
            }
        }

        /// <summary>
        ///     Updates the camera's velocity based on the supplied movement direction
        ///     and the elapsed time (since this method was last called). The movement
        ///     direction is the in the range [-1,1].
        /// </summary>
        private void updateVelocity(TGCVector3 direction, float elapsedTimeSec)
        {
            if (direction.X != 0.0f)
            {
                // Camera is moving along the x axis.
                // Linearly accelerate up to the camera's max speed.

                currentVelocity.X += direction.X * Acceleration.X * elapsedTimeSec;

                if (currentVelocity.X > velocity.X)
                    currentVelocity.X = velocity.X;
                else if (currentVelocity.X < -velocity.X)
                    currentVelocity.X = -velocity.X;
            }
            else
            {
                // Camera is no longer moving along the x axis.
                // Linearly decelerate back to stationary state.

                if (currentVelocity.X > 0.0f)
                {
                    if ((currentVelocity.X -= Acceleration.X * elapsedTimeSec) < 0.0f)
                        currentVelocity.X = 0.0f;
                }
                else
                {
                    if ((currentVelocity.X += Acceleration.X * elapsedTimeSec) > 0.0f)
                        currentVelocity.X = 0.0f;
                }
            }

            if (direction.Y != 0.0f)
            {
                // Camera is moving along the y axis.
                // Linearly accelerate up to the camera's max speed.

                currentVelocity.Y += direction.Y * Acceleration.Y * elapsedTimeSec;

                if (currentVelocity.Y > velocity.Y)
                    currentVelocity.Y = velocity.Y;
                else if (currentVelocity.Y < -velocity.Y)
                    currentVelocity.Y = -velocity.Y;
            }
            else
            {
                // Camera is no longer moving along the y axis.
                // Linearly decelerate back to stationary state.

                if (currentVelocity.Y > 0.0f)
                {
                    if ((currentVelocity.Y -= Acceleration.Y * elapsedTimeSec) < 0.0f)
                        currentVelocity.Y = 0.0f;
                }
                else
                {
                    if ((currentVelocity.Y += Acceleration.Y * elapsedTimeSec) > 0.0f)
                        currentVelocity.Y = 0.0f;
                }
            }

            if (direction.Z != 0.0f)
            {
                // Camera is moving along the z axis.
                // Linearly accelerate up to the camera's max speed.

                currentVelocity.Z += direction.Z * Acceleration.Z * elapsedTimeSec;

                if (currentVelocity.Z > velocity.Z)
                    currentVelocity.Z = velocity.Z;
                else if (currentVelocity.Z < -velocity.Z)
                    currentVelocity.Z = -velocity.Z;
            }
            else
            {
                // Camera is no longer moving along the z axis.
                // Linearly decelerate back to stationary state.

                if (currentVelocity.Z > 0.0f)
                {
                    if ((currentVelocity.Z -= Acceleration.Z * elapsedTimeSec) < 0.0f)
                        currentVelocity.Z = 0.0f;
                }
                else
                {
                    if ((currentVelocity.Z += Acceleration.Z * elapsedTimeSec) > 0.0f)
                        currentVelocity.Z = 0.0f;
                }
            }
        }

        /// <summary>
        ///     Actualizar currentVelocity sin aplicar aceleracion
        /// </summary>
        private void updateVelocityNoAcceleration(TGCVector3 direction)
        {
            currentVelocity.X = velocity.X * direction.X;
            currentVelocity.Y = velocity.Y * direction.Y;
            currentVelocity.Z = velocity.Z * direction.Z;
        }

        /// <summary>
        ///     Reconstruct the view TGCMatrix.
        /// </summary>
        private void reconstructViewTGCMatrix(bool orthogonalizeAxes)
        {
            if (orthogonalizeAxes)
            {
                // Regenerate the camera's local axes to orthogonalize them.

                zAxis.Normalize();

                yAxis = TGCVector3.Cross(zAxis, xAxis);
                yAxis.Normalize();

                xAxis = TGCVector3.Cross(yAxis, zAxis);
                xAxis.Normalize();

                viewDir = zAxis;
                LookAt = eye + viewDir;
            }

            // Reconstruct the view TGCMatrix.

            viewTGCMatrix.M11 = xAxis.X;
            viewTGCMatrix.M21 = xAxis.Y;
            viewTGCMatrix.M31 = xAxis.Z;
            viewTGCMatrix.M41 = -TGCVector3.Dot(xAxis, eye);

            viewTGCMatrix.M12 = yAxis.X;
            viewTGCMatrix.M22 = yAxis.Y;
            viewTGCMatrix.M32 = yAxis.Z;
            viewTGCMatrix.M42 = -TGCVector3.Dot(yAxis, eye);

            viewTGCMatrix.M13 = zAxis.X;
            viewTGCMatrix.M23 = zAxis.Y;
            viewTGCMatrix.M33 = zAxis.Z;
            viewTGCMatrix.M43 = -TGCVector3.Dot(zAxis, eye);

            viewTGCMatrix.M14 = 0.0f;
            viewTGCMatrix.M24 = 0.0f;
            viewTGCMatrix.M34 = 0.0f;
            viewTGCMatrix.M44 = 1.0f;
        }

        /// <summary>
        ///     Obtiene la direccion a moverse por la camara en base a la entrada de teclado
        /// </summary>
        private TGCVector3 getMovementDirection(TgcD3dInput d3dInput)
        {
            var direction = new TGCVector3(0.0f, 0.0f, 0.0f);

            //Forward
            if (d3dInput.keyDown(Key.W))
            {
                if (!moveForwardsPressed)
                {
                    moveForwardsPressed = true;
                    currentVelocity = new TGCVector3(currentVelocity.X, currentVelocity.Y, 0.0f);
                }

                direction.Z += 1.0f;
            }
            else
            {
                moveForwardsPressed = false;
            }

            //Backward
            if (d3dInput.keyDown(Key.S))
            {
                if (!moveBackwardsPressed)
                {
                    moveBackwardsPressed = true;
                    currentVelocity = new TGCVector3(currentVelocity.X, currentVelocity.Y, 0.0f);
                }

                direction.Z -= 1.0f;
            }
            else
            {
                moveBackwardsPressed = false;
            }

            //Strafe right
            if (d3dInput.keyDown(Key.D))
            {
                if (!moveRightPressed)
                {
                    moveRightPressed = true;
                    currentVelocity = new TGCVector3(0.0f, currentVelocity.Y, currentVelocity.Z);
                }

                direction.X += 1.0f;
            }
            else
            {
                moveRightPressed = false;
            }

            //Strafe left
            if (d3dInput.keyDown(Key.A))
            {
                if (!moveLeftPressed)
                {
                    moveLeftPressed = true;
                    currentVelocity = new TGCVector3(0.0f, currentVelocity.Y, currentVelocity.Z);
                }

                direction.X -= 1.0f;
            }
            else
            {
                moveLeftPressed = false;
            }

            //Jump
            if (d3dInput.keyDown(Key.Space))
            {
                if (!moveUpPressed)
                {
                    moveUpPressed = true;
                    currentVelocity = new TGCVector3(currentVelocity.X, 0.0f, currentVelocity.Z);
                }

                direction.Y += 1.0f;
            }
            else
            {
                moveUpPressed = false;
            }

            //Crouch
            if (d3dInput.keyDown(Key.LeftControl))
            {
                if (!moveDownPressed)
                {
                    moveDownPressed = true;
                    currentVelocity = new TGCVector3(currentVelocity.X, 0.0f, currentVelocity.Z);
                }

                direction.Y -= 1.0f;
            }
            else
            {
                moveDownPressed = false;
            }

            return direction;
        }

        /// <summary>
        ///     String de codigo para setear la camara desde ToolsModel, con la posicion actual y direccion de la camara
        /// </summary>
        internal string getPositionCode()
        {
            //TODO ver de donde carajo sacar el LookAt de esta camara
            var lookAt = LookAt;

            return "ToolsModel.Instance.setCamera(new TGCVector3(" +
                   TgcParserUtils.printFloat(eye.X) + "f, " + TgcParserUtils.printFloat(eye.Y) + "f, " +
                   TgcParserUtils.printFloat(eye.Z) + "f), new TGCVector3(" +
                   TgcParserUtils.printFloat(lookAt.X) + "f, " + TgcParserUtils.printFloat(lookAt.Y) + "f, " +
                   TgcParserUtils.printFloat(lookAt.Z) + "f));";
        }

        #region Getters y Setters

        private bool fpsModeEnable;
        private float previousY;

        public bool FpsModeEnable
        {
            get
            {
                if (Terrain == null) return false;
                return fpsModeEnable;
            }
            set
            {
                if (value && Terrain == null) return;

                if (value && !fpsModeEnable)
                {
                    previousY = eye.Y;
                    float y;
                    Terrain.interpoledHeight(eye.X, eye.Z, out y);
                    eye.Y = y + HeadPosition * WORLD_YAXIS.Y;
                }
                else if (!value && fpsModeEnable) eye.Y = previousY;
                fpsModeEnable = value;
                setPosition(eye);
            }
        }

        /// <summary>
        ///     Aceleracion de la camara en cada uno de sus ejes
        /// </summary>
        public TGCVector3 Acceleration { get; set; }

        /// <summary>
        ///     Activa o desactiva el efecto de Aceleración/Desaceleración
        /// </summary>
        public bool AccelerationEnable { get; set; }

        private TGCVector3 currentVelocity;

        /// <summary>
        ///     Velocidad de desplazamiento actual, teniendo en cuenta la aceleracion
        /// </summary>
        public TGCVector3 CurrentVelocity
        {
            get { return currentVelocity; }
        }

        private TGCVector3 velocity;

        /// <summary>
        ///     Velocidad de desplazamiento de la cámara en cada uno de sus ejes
        /// </summary>
        public TGCVector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        ///     Velocidad de desplazamiento de los ejes XZ de la cámara
        /// </summary>
        public float MovementSpeed
        {
            get { return velocity.X; }
            set
            {
                velocity.X = value;
                velocity.Z = value;
            }
        }

        /// <summary>
        ///     Velocidad de desplazamiento del eje Y de la cámara
        /// </summary>
        public float JumpSpeed
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        /// <summary>
        ///     Velocidad de rotacion de la cámara
        /// </summary>
        public float RotationSpeed { get; set; }

        private TGCMatrix viewTGCMatrix;

        /// <summary>
        ///     View TGCMatrix resultante
        /// </summary>
        public TGCMatrix ViewTGCMatrix
        {
            get { return viewTGCMatrix; }
        }

        /// <summary>
        ///     Posicion actual de la camara
        /// </summary>
        public TGCVector3 Position
        {
            get { return eye; }
        }

        /// <summary>
        ///     Punto hacia donde mira la cámara
        /// </summary>
        public TGCVector3 LookAt { get; private set; }

        /// <summary>
        ///     Boton del mouse que debe ser presionado para rotar la camara.
        ///     Por default es boton izquierdo.
        /// </summary>
        public TgcD3dInput.MouseButtons RotateMouseButton { get; set; }

        /// <summary>
        ///     Terreno del que tomara la altura.
        /// </summary>
        public SmartTerrain Terrain { get; set; }

        /// <summary>
        ///     Posicion de la cabeza respecto del suelo.
        /// </summary>
        public float HeadPosition { get; set; }

        #endregion Getters y Setters
    }
}