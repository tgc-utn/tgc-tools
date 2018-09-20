using TGC.Core.Mathematica;
using TGC.Tools.MeshCreator.Primitives;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Gizmos
{
    /// <summary>
    ///     Gizmo generico
    /// </summary>
    public abstract class EditorGizmo
    {
        public EditorGizmo(MeshCreatorModifier control)
        {
            Control = control;
        }

        /// <summary>
        ///     Control
        /// </summary>
        public MeshCreatorModifier Control { get; }

        /// <summary>
        ///     Activar o desactivar gizmo
        /// </summary>
        public abstract void setEnabled(bool enabled);

        /// <summary>
        ///     Actualizar estado
        /// </summary>
        public abstract void update();

        /// <summary>
        ///     Dibujar gizmo, sin Z Buffer
        /// </summary>
        public abstract void render();

        /// <summary>
        ///     Mover gizmo de lugar para acomodarse a la nueva posicion del objeto seleccionado
        /// </summary>
        public abstract void move(EditorPrimitive selectedPrimitive, TGCVector3 movement);

        /// <summary>
        ///     Liberar recursos
        /// </summary>
        public abstract void dipose();
    }
}