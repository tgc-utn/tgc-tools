using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX;
using TGC.Tools.Model;
using TGC.Tools.Utils.TgcGeometry;
using TGC.Tools.Utils.TgcSceneLoader;

namespace TGC.Tools.RoomsEditor
{
    /// <summary>
    ///     Información lógica y visual de una pared de un Room
    /// </summary>
    public class RoomsEditorWall
    {
        private RoomsEditorRoom room;
        private Types type;
        
        /// <summary>
        /// Que tipo de pared es en un room
        /// </summary>
        public enum Types
        {
            Roof,
            Floor,
            East,
            West,
            North,
            South
        }

        public RoomsEditorWall(RoomsEditorRoom room, string name, Types type)
        {
            this.room = room;
            this.type = type;
            Name = name;
            WallSegments = new List<TgcPlaneWall>();
            IntersectingRooms = new List<RoomsEditorRoom>();

            //cargar valores default de la pared
            Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice,
                room.RoomPanel.mapView.defaultTextureImage);
            AutoAdjustUv = true;
            UTile = 1f;
            VTile = 1f;

            switch (type)
            {
                case Types.Roof:
                    Normal = new Vector3(0, -1, 0);
                    break;
                case Types.Floor:
                    Normal = new Vector3(0, 1, 0);
                    break;
                case Types.East:
                    Normal = new Vector3(-1, 0, 0);
                    break;
                case Types.West:
                    Normal = new Vector3(1, 0, 0);
                    break;
                case Types.North:
                    Normal = new Vector3(0, 0, -1);
                    break;
                case Types.South:
                    Normal = new Vector3(0, 0, 1);
                    break;
            }
        }

        public Vector3 Normal { get; private set; }

        /// <summary>
        ///     Segmentos 3d de pared
        /// </summary>
        public List<TgcPlaneWall> WallSegments { get; }

        /// <summary>
        ///     Textura general de todos los segmentos de la pared
        /// </summary>
        public TgcTexture Texture { get; set; }

        /// <summary>
        ///     Cantidad de tile de la textura en coordenada U
        /// </summary>
        public float UTile { get; set; }

        /// <summary>
        ///     Cantidad de tile de la textura en coordenada V
        /// </summary>
        public float VTile { get; set; }

        /// <summary>
        ///     Auto ajustar coordenadas UV en base a la relación de tamaño de la pared y la textura
        /// </summary>
        public bool AutoAdjustUv { get; set; }

        /// <summary>
        ///     Rooms contra los cuales intersecta esta pared
        /// </summary>
        public List<RoomsEditorRoom> IntersectingRooms { get; }

        /// <summary>
        ///     Nombre que indentifica que pared es: North, East, Floor, etc.
        /// </summary>
        public string Name { get; }

        public void dispose()
        {
            foreach (var wall3d in WallSegments)
            {
                wall3d.dispose();
            }
            Texture.dispose();
        }

        public void render()
        {
            foreach (var wall3d in WallSegments)
            {
                wall3d.render();
            }
        }

        public IEnumerable<TgcMesh> ToMeshes()
        {
            return this.WallSegments.Select((wall, ind) =>
            {
                wall.Normal = this.Normal;
                return wall.toMesh(room.Name + "-" + this.Name + "-" + ind.ToString());
            }); 
        } 
    }
}