namespace TgcViewer.Utils.Interpolation
{
    /// <summary>
    /// Utilidad para controlar el avance del tiempo hasta un tope determinado (como una cuenta regresiva)
    /// </summary>
    public class Temporizador
    {
        private float current;

        /// <summary>
        /// Valor actual
        /// </summary>
        public float Current
        {
            get { return current; }
            set { current = value; }
        }

        private float stopSegs;

        /// <summary>
        /// Fin de la cuenta regresiva, en segundos.
        /// </summary>
        public float StopSegs
        {
            get { return stopSegs; }
            set { stopSegs = value; }
        }

        /// <summary>
        /// Crear temporizador
        /// </summary>
        public Temporizador()
        {
        }

        /// <summary>
        /// Cargar valores iniciales del temporizador
        /// </summary>
        public void reset()
        {
            this.current = 0;
        }

        /// <summary>
        /// Avanzar el tiempo
        /// </summary>
        /// <returns>True si llego a su fin</returns>
        public bool update()
        {
            current += GuiController.Instance.ElapsedTime;
            if (current > stopSegs)
            {
                current = stopSegs;
                return true;
            }
            return false;
        }
    }
}