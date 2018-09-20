using System.Collections.Generic;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Primitives
{
    /// <summary>
    ///     Primitiva generica para construir objetos
    /// </summary>
    public abstract class EditorPrimitive
    {
        public static int PRIMITIVE_COUNT = 1;

        protected bool selected;

        protected bool visible;

        public EditorPrimitive(MeshCreatorModifier control)
        {
            Control = control;
            selected = false;
            UserProperties = new Dictionary<string, string>();
            Layer = control.CurrentLayer;
            ModifyCaps = new ModifyCapabilities();
            visible = true;
        }

        /// <summary>
        ///     Indica si esta seleccionado
        /// </summary>
        public bool Selected
        {
            get { return selected; }
        }

        /// <summary>
        ///     Nombre
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Layer
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        ///     User props
        /// </summary>
        public Dictionary<string, string> UserProperties { get; set; }

        /// <summary>
        ///     AlphaBlending
        /// </summary>
        public abstract bool AlphaBlendEnable { get; set; }

        /// <summary>
        ///     BoundingBox
        /// </summary>
        public abstract TgcBoundingAxisAlignBox BoundingBox { get; }

        /// <summary>
        ///     Control
        /// </summary>
        public MeshCreatorModifier Control { get; }

        /// <summary>
        ///     Indica si esta visible
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        ///     Capacidades de edición en el panel de Modify
        /// </summary>
        public ModifyCapabilities ModifyCaps { get; }

        /// <summary>
        ///     Offset de la textura del objeto
        /// </summary>
        public abstract TGCVector2 TextureOffset { get; set; }

        /// <summary>
        ///     Tiling de la textura del objeto
        /// </summary>
        public abstract TGCVector2 TextureTiling { get; set; }

        /// <summary>
        ///     Posicion del objeto
        /// </summary>
        public abstract TGCVector3 Position { get; set; }

        /// <summary>
        ///     Rotacion del objeto
        /// </summary>
        public abstract TGCVector3 Rotation { get; }

        /// <summary>
        ///     Escala del objeto. Viene de la forma (1, 1, 1)
        /// </summary>
        public abstract TGCVector3 Scale { get; set; }

        /// <summary>
        ///     Selecciona o deselecciona el objeto
        /// </summary>
        public abstract void setSelected(bool selected);

        /// <summary>
        ///     Dibujar primitiva ya creada
        /// </summary>
        public abstract void render();

        /// <summary>
        ///     Liberar recursos
        /// </summary>
        public abstract void dispose();

        /// <summary>
        ///     Iniciar creacion de primitiva
        /// </summary>
        public abstract void initCreation(TGCVector3 gridPoint);

        /// <summary>
        ///     Ejecutar la creacion de la primitiva
        /// </summary>
        public abstract void doCreation();

        /// <summary>
        ///     Mover objeto
        /// </summary>
        public abstract void move(TGCVector3 move);

        /// <summary>
        ///     Cambiar textura del objeto
        /// </summary>
        public abstract void setTexture(TgcTexture texture, int slot);

        /// <summary>
        ///     Obtener textura del objeto
        /// </summary>
        public abstract TgcTexture getTexture(int slot);

        /// <summary>
        ///     Aplicar rotacion absoluta del objeto respecto de un pivote
        /// </summary>
        public abstract void setRotationFromPivot(TGCVector3 rotation, TGCVector3 pivot);

        /// <summary>
        ///     Crear un TgcMesh para exportar la escena
        /// </summary>
        public abstract TgcMesh createMeshToExport();

        /// <summary>
        ///     Clonar primitiva
        /// </summary>
        public abstract EditorPrimitive clone();

        /// <summary>
        ///     Actualizar el AABB
        /// </summary>
        public abstract void updateBoundingBox();

        /// <summary>
        ///     Estructura que indica que cosas se pueden alterar de esta primitiva
        ///     en el panel de Modify.
        ///     Por default todo activado.
        /// </summary>
        public class ModifyCapabilities
        {
            public bool ChangeOffsetUV = true;
            public bool ChangePosition = true;
            public bool ChangeRotation = true;
            public bool ChangeScale = true;
            public bool ChangeTexture = true;
            public bool ChangeTilingUV = true;
            public int TextureNumbers = 1;
        }
    }
}