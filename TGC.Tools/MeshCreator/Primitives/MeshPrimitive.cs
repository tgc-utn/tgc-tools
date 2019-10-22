using Microsoft.DirectX.Direct3D;
using System;
using TGC.Core.BoundingVolumes;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Tools.UserControls;

namespace TGC.Tools.MeshCreator.Primitives
{
    /// <summary>
    ///     Primitiva que representa un Mesh importado
    /// </summary>
    public class MeshPrimitive : EditorPrimitive
    {
        private bool editablePolyEnabled;
        private TgcMesh mesh;
        private TGCVector2[] originalUVCoords;
        private TGCVector2 uvOffset;
        private TGCVector2 uvTile;

        public MeshPrimitive(MeshCreatorModifier control, TgcMesh mesh)
            : base(control)
        {
            //this.Name = mesh.Name + "_" + EditorPrimitive.PRIMITIVE_COUNT++;
            Name = mesh.Name;
            this.mesh = mesh;
            editablePolyEnabled = false;

            //Ver si tiene texturas
            if (mesh.RenderType == TgcMesh.MeshRenderType.DIFFUSE_MAP ||
                mesh.RenderType == TgcMesh.MeshRenderType.DIFFUSE_MAP_AND_LIGHTMAP)
            {
                //Tiene, habilitar la edicion
                ModifyCaps.ChangeTexture = true;
                ModifyCaps.ChangeOffsetUV = true;
                ModifyCaps.ChangeTilingUV = true;
                ModifyCaps.TextureNumbers = mesh.DiffuseMaps.Length;
                originalUVCoords = mesh.getTextureCoordinates();
            }
            else
            {
                //No tiene textura, deshabilitar todo
                ModifyCaps.ChangeTexture = false;
                ModifyCaps.ChangeOffsetUV = false;
                ModifyCaps.ChangeTilingUV = false;
                ModifyCaps.TextureNumbers = 0;
            }

            UserProperties = this.mesh.UserProperties;
            uvOffset = new TGCVector2(0, 0);
            uvTile = new TGCVector2(1, 1);

            //Layer
            if (this.mesh.Layer != null && this.mesh.Layer.Length > 0)
            {
                Layer = this.mesh.Layer;
            }
            else
            {
                Layer = control.CurrentLayer;
            }

            //Ubicar mesh en el origen de coordenadas respecto del centro de su AABB
            setMeshToOrigin();
        }

        /// <summary>
        ///     EditablePoly del mesh
        /// </summary>
        public EditablePoly.EditablePoly EditablePoly { get; private set; }

        public override TgcBoundingAxisAlignBox BoundingBox
        {
            get { return mesh.BoundingBox; }
        }

        public override bool AlphaBlendEnable
        {
            get { return mesh.AlphaBlendEnable; }
            set { mesh.AlphaBlendEnable = value; }
        }

        public override TGCVector2 TextureOffset
        {
            get { return uvOffset; }
            set
            {
                uvOffset = value;
                updateTextureCoordinates();
            }
        }

        public override TGCVector2 TextureTiling
        {
            get { return uvTile; }
            set
            {
                uvTile = value;
                updateTextureCoordinates();
            }
        }

        public override TGCVector3 Position
        {
            get { return mesh.Position; }
            set { mesh.Position = value; }
        }

        public override TGCVector3 Rotation
        {
            get { return mesh.Rotation; }
        }

        public override TGCVector3 Scale
        {
            get { return mesh.Scale; }
            set { mesh.Scale = value; }
        }

        /// <summary>
        ///     Mover vertices del mesh al centro de coordenadas
        /// </summary>
        private void setMeshToOrigin()
        {
            //Desplazar los vertices del mesh para que tengan el centro del AABB en el origen
            var center = mesh.BoundingBox.calculateBoxCenter();
            moveMeshVertices(-center);

            //Ubicar el mesh en donde estaba originalmente
            mesh.BoundingBox.setExtremes(mesh.BoundingBox.PMin - center, mesh.BoundingBox.PMax - center);
            mesh.Position = center;
        }

        /// <summary>
        ///     Mover fisicamente los vertices del mesh
        /// </summary>
        private void moveMeshVertices(TGCVector3 offset)
        {
            switch (mesh.RenderType)
            {
                case TgcMesh.MeshRenderType.VERTEX_COLOR:
                    var verts1 = (TgcSceneLoader.VertexColorVertex[])mesh.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.VertexColorVertex), LockFlags.ReadOnly, mesh.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts1.Length; i++)
                    {
                        verts1[i].Position = verts1[i].Position + offset;
                    }
                    mesh.D3dMesh.SetVertexBufferData(verts1, LockFlags.None);
                    mesh.D3dMesh.UnlockVertexBuffer();
                    break;

                case TgcMesh.MeshRenderType.DIFFUSE_MAP:
                    var verts2 = (TgcSceneLoader.DiffuseMapVertex[])mesh.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.DiffuseMapVertex), LockFlags.ReadOnly, mesh.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts2.Length; i++)
                    {
                        verts2[i].Position = verts2[i].Position + offset;
                    }
                    mesh.D3dMesh.SetVertexBufferData(verts2, LockFlags.None);
                    mesh.D3dMesh.UnlockVertexBuffer();
                    break;

                case TgcMesh.MeshRenderType.DIFFUSE_MAP_AND_LIGHTMAP:
                    var verts3 = (TgcSceneLoader.DiffuseMapAndLightmapVertex[])mesh.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.DiffuseMapAndLightmapVertex), LockFlags.ReadOnly,
                        mesh.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts3.Length; i++)
                    {
                        verts3[i].Position = verts3[i].Position + offset;
                    }
                    mesh.D3dMesh.SetVertexBufferData(verts3, LockFlags.None);
                    mesh.D3dMesh.UnlockVertexBuffer();
                    break;
            }
        }

        /// <summary>
        ///     Activar/Desactivar modo editablePoly
        /// </summary>
        public void enableEditablePoly(bool enabled, EditablePoly.EditablePoly.PrimitiveType primitiveType)
        {
            if (enabled)
            {
                if (EditablePoly == null)
                {
                    EditablePoly = new EditablePoly.EditablePoly(Control, mesh);
                }
                else
                {
                    EditablePoly.updateValuesFromMesh(mesh);
                }
                EditablePoly.setPrimitiveType(primitiveType);
            }
            else
            {
                //Acomodar mesh luego de haber sido editado internamente
                applyMeshTransformToVertices(mesh);
                setMeshToOrigin();
            }
            editablePolyEnabled = enabled;
        }

        /// <summary>
        ///     Actualizacion de estado en Render loop del editablePoly
        /// </summary>
        public void doEditablePolyUpdate()
        {
            EditablePoly.update();
        }

        public override void render()
        {
            if (editablePolyEnabled)
            {
                EditablePoly.render();
            }
            else
            {
                mesh.Render();
            }
        }

        public override void dispose()
        {
            mesh.Dispose();
            mesh = null;
            originalUVCoords = null;
            if (EditablePoly != null)
            {
                EditablePoly.dispose();
                EditablePoly = null;
            }
        }

        public override void setSelected(bool selected)
        {
            this.selected = selected;
            var color = selected ? MeshCreatorUtils.SELECTED_OBJECT_COLOR : MeshCreatorUtils.UNSELECTED_OBJECT_COLOR;
            mesh.BoundingBox.setRenderColor(color);
        }

        public override void initCreation(TGCVector3 gridPoint)
        {
            throw new NotImplementedException(
                "Nunca se deberia iniciar una creacion de primitiva para un Mesh. Siempre se importan");
        }

        public override void doCreation()
        {
            throw new NotImplementedException(
                "Nunca se deberia iniciar una creacion de primitiva para un Mesh. Siempre se importan");
        }

        public override void move(TGCVector3 move)
        {
            mesh.Move(move);
        }

        public override void setTexture(TgcTexture texture, int slot)
        {
            var newTextures = new TgcTexture[mesh.DiffuseMaps.Length];
            for (var i = 0; i < newTextures.Length; i++)
            {
                if (i != slot)
                {
                    newTextures[i] = mesh.DiffuseMaps[i].Clone();
                }
                else
                {
                    newTextures[i] = texture;
                }
            }
            mesh.changeDiffuseMaps(newTextures);
        }

        public override TgcTexture getTexture(int slot)
        {
            return mesh.DiffuseMaps[slot];
        }

        /// <summary>
        ///     Agregar una nueva textura al mesh
        /// </summary>
        public void addNexTexture(TgcTexture t)
        {
            mesh.addDiffuseMap(t);
            ModifyCaps.TextureNumbers = mesh.DiffuseMaps.Length;
        }

        /// <summary>
        ///     Eliminar textura existente
        /// </summary>
        /// <param name="n"></param>
        public void deleteTexture(int n)
        {
            mesh.deleteDiffuseMap(n, 0);
            ModifyCaps.TextureNumbers = mesh.DiffuseMaps.Length;
        }

        public override void setRotationFromPivot(TGCVector3 rotation, TGCVector3 pivot)
        {
            mesh.Rotation = rotation;
            var translation = pivot - mesh.Position;
            var m = TGCMatrix.Translation(-translation) * TGCMatrix.RotationYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                    TGCMatrix.Translation(translation);
            mesh.Move(new TGCVector3(m.M41, m.M42, m.M43));
        }

        public override TgcMesh createMeshToExport()
        {
            mesh.Name = Name;
            mesh.Layer = Layer;
            mesh.UserProperties = UserProperties;

            var cloneMesh = mesh.clone(mesh.Name);
            return cloneMesh;
        }

        public override EditorPrimitive clone()
        {
            mesh.Name = Name;
            mesh.Layer = Layer;
            mesh.UserProperties = UserProperties;

            //Clonar mesh y aplicar transformacion a los vertices
            var cloneMesh = mesh.clone(mesh.Name);
            applyMeshTransformToVertices(cloneMesh);

            //Calcular nuevo bounding box
            //cloneMesh.createBoundingBox();

            return new MeshPrimitive(Control, cloneMesh);
        }

        /// <summary>
        ///     Actualizar coordenadas de textura del mesh en base al offset y tiling
        /// </summary>
        private void updateTextureCoordinates()
        {
            switch (mesh.RenderType)
            {
                case TgcMesh.MeshRenderType.DIFFUSE_MAP:
                    var verts = (TgcSceneLoader.DiffuseMapVertex[])mesh.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.DiffuseMapVertex), LockFlags.ReadOnly, mesh.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts.Length; i++)
                    {
                        verts[i].Tu = uvOffset.X + originalUVCoords[i].X * uvTile.X;
                        verts[i].Tv = uvOffset.Y + originalUVCoords[i].Y * uvTile.Y;
                    }
                    mesh.D3dMesh.SetVertexBufferData(verts, LockFlags.None);
                    mesh.D3dMesh.UnlockVertexBuffer();
                    break;

                case TgcMesh.MeshRenderType.DIFFUSE_MAP_AND_LIGHTMAP:
                    var verts2 = (TgcSceneLoader.DiffuseMapAndLightmapVertex[])mesh.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.DiffuseMapAndLightmapVertex), LockFlags.ReadOnly,
                        mesh.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts2.Length; i++)
                    {
                        verts2[i].Tu0 = uvOffset.X + originalUVCoords[i].X * uvTile.X;
                        verts2[i].Tv0 = uvOffset.Y + originalUVCoords[i].Y * uvTile.Y;
                    }
                    mesh.D3dMesh.SetVertexBufferData(verts2, LockFlags.None);
                    mesh.D3dMesh.UnlockVertexBuffer();
                    break;
            }
        }

        public override void updateBoundingBox()
        {
            applyMeshTransformToVertices(mesh);
        }

        /// <summary>
        ///     Transformar fisicamente los vertices del mesh segun su transformacion actual
        /// </summary>
        private void applyMeshTransformToVertices(TgcMesh m)
        {
            //Transformacion actual
            var transform = TGCMatrix.Scaling(m.Scale)
                            * TGCMatrix.RotationYawPitchRoll(m.Rotation.Y, m.Rotation.X, m.Rotation.Z)
                            * TGCMatrix.Translation(m.Position);

            switch (m.RenderType)
            {
                case TgcMesh.MeshRenderType.VERTEX_COLOR:
                    var verts1 = (TgcSceneLoader.VertexColorVertex[])m.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.VertexColorVertex), LockFlags.ReadOnly, m.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts1.Length; i++)
                    {
                        verts1[i].Position = TGCVector3.transform(verts1[i].Position, transform);
                    }
                    m.D3dMesh.SetVertexBufferData(verts1, LockFlags.None);
                    m.D3dMesh.UnlockVertexBuffer();
                    break;

                case TgcMesh.MeshRenderType.DIFFUSE_MAP:
                    var verts2 = (TgcSceneLoader.DiffuseMapVertex[])m.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.DiffuseMapVertex), LockFlags.ReadOnly, m.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts2.Length; i++)
                    {
                        verts2[i].Position = TGCVector3.transform(verts2[i].Position, transform);
                    }
                    m.D3dMesh.SetVertexBufferData(verts2, LockFlags.None);
                    m.D3dMesh.UnlockVertexBuffer();
                    break;

                case TgcMesh.MeshRenderType.DIFFUSE_MAP_AND_LIGHTMAP:
                    var verts3 = (TgcSceneLoader.DiffuseMapAndLightmapVertex[])m.D3dMesh.LockVertexBuffer(
                        typeof(TgcSceneLoader.DiffuseMapAndLightmapVertex), LockFlags.ReadOnly, m.D3dMesh.NumberVertices);
                    for (var i = 0; i < verts3.Length; i++)
                    {
                        verts3[i].Position = TGCVector3.transform(verts3[i].Position, transform);
                    }
                    m.D3dMesh.SetVertexBufferData(verts3, LockFlags.None);
                    m.D3dMesh.UnlockVertexBuffer();
                    break;
            }

            //Quitar movimientos del mesh
            m.Position = TGCVector3.Empty;
            m.Scale = TGCVector3.One;
            m.Rotation = TGCVector3.Empty;
            m.Transform = TGCMatrix.Identity;
            m.AutoTransform = true;

            //Calcular nuevo bounding box
            m.createBoundingBox();
        }
    }
}