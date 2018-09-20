using System.Drawing;
using TGC.Core.Collision;
using TGC.Core.Mathematica;

namespace TGC.Tools.MeshCreator.EditablePoly.Primitives
{
    /// <summary>
    ///     Primitiva generica
    /// </summary>
    public abstract class EditPolyPrimitive
    {
        protected bool selected;

        public EditPolyPrimitive()
        {
            selected = false;
        }

        /// <summary>
        ///     Indica si la primitiva esta seleccionada
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        ///     Tipo de primitiva
        /// </summary>
        public abstract EditablePoly.PrimitiveType Type { get; }

        /// <summary>
        ///     Proyectar primitva a rectangulo 2D en la pantalla
        /// </summary>
        /// <param name="transform">Mesh transform</param>
        /// <param name="box2D">Rectangulo 2D proyectado</param>
        /// <returns>False si es un caso degenerado de proyeccion y no debe considerarse</returns>
        public abstract bool projectToScreen(TGCMatrix transform, out Rectangle box2D);

        /// <summary>
        ///     Intersect ray againts primitive
        /// </summary>
        public abstract bool intersectRay(TgcRay tgcRay, TGCMatrix transform, out TGCVector3 q);

        /// <summary>
        ///     Centro de la primitiva
        /// </summary>
        public abstract TGCVector3 computeCenter();

        /// <summary>
        ///     Mover primitiva
        /// </summary>
        public abstract void move(TGCVector3 movement);
    }
}