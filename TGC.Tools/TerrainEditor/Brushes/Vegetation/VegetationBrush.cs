﻿using TGC.Tools.TerrainEditor.Instances;

namespace TGC.Tools.TerrainEditor.Brushes.Vegetation
{
    public class VegetationBrush : VegetationPicker
    {
        private string vegetationName;

        public VegetationBrush(TgcTerrainEditor editor) : base(editor)
        {
        }

        /// <summary>
        ///     Setea el nombre del mesh que le va a pedir a InstanceManager
        /// </summary>
        /// <param name="name"></param>
        public void setVegetation(string name)
        {
            vegetationName = name;
            removeFloatingVegetation();
        }

        public override bool mouseMove(TgcTerrainEditor editor)
        {
            if (Mesh == null)
                Mesh = InstancesManager.Instance.newMeshInstanceOf(vegetationName);
            var pos = Position;
            base.mouseMove(editor);
            if (pos == Position) Enabled = false;
            return false;
        }

        protected override void addVegetation(TgcTerrainEditor editor)
        {
            var scale = Mesh.Scale;
            var rotation = Mesh.Rotation;
            base.addVegetation(editor);

            Mesh = InstancesManager.Instance.newMeshInstanceOf(vegetationName);
            Mesh.Scale = scale;
            Mesh.Rotation = rotation;
            Mesh.Position = Position;
        }
    }
}