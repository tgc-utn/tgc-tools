using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TGC.Core.Collision;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Text;
using TGC.Tools.Example;
using TGC.Tools.Model;
using TGC.Tools.TerrainEditor.Brushes;
using TGC.Tools.TerrainEditor.Instances;
using TGC.Tools.UserControls;
using Font = System.Drawing.Font;

namespace TGC.Tools.TerrainEditor
{
    /// <summary>
    ///     Ejemplo TgcTerrainEditor:
    ///     Unidades Involucradas:
    ///     # Unidad 7 - Optimización - Heightmap
    ///     Herramienta para crear terrenos por Heightmaps, ubicar objetos sobre el terreno
    ///     y luego exportarlos para ser usados en otro ejemplo.
    ///     Autor: Daniela Kazarian
    /// </summary>
    public class TgcTerrainEditor : TGCExampleTools
    {
        private TerrainEditorModifier Modifier { get; set; }
        public bool RenderBoundingBoxes { get; set; }
        private TerrainFpsCamera TerrainFpsCamera { get; set; }

        public TgcTerrainEditor(string mediaDir, string shadersDir, Panel modifiersPanel) : base(mediaDir, shadersDir, modifiersPanel)
        {
            Category = "Utils";
            Name = "TerrainEditor";
            Description = @"Terrain editor.
                            Camara: Desplazamiento: W A S D Ctrl Space, Para rotar mantener presionado el boton derecho.
                            Ocultar vegetacion: V
                            Cambiar modo de picking: P
                            Modo primera persona: F (Rotacion con boton izquierdo)
                            Mostar AABBs: B";
        }

        public override void Init()
        {
            //Configurar FPS Camara
            TerrainFpsCamera = new TerrainFpsCamera(Input);
            Camera = TerrainFpsCamera;
            Camera.RotateMouseButton = cameraRotationButton;
            Camera.setCamera(new TGCVector3(-722.6171f, 495.0046f, -31.2611f), new TGCVector3(164.9481f, 35.3185f, -61.5394f));

            Terrain = new SmartTerrain();
            brush = new DummyBrush();
            Vegetation = new List<TgcMesh>();

            pickingRay = new TgcPickingRay(Input);
            ShowVegetation = true;
            mouseMove = Panel3d_MouseMove;
            mouseLeave = Panel3d_MouseLeave;
            noBrush = new DummyBrush();

            ToolsModel.Instance.Panel3d.MouseMove += mouseMove;
            ToolsModel.Instance.Panel3d.MouseLeave += mouseLeave;

            labelFPS = new TgcText2D();
            labelFPS.Text = "Press F to go back to edition mode";
            labelFPS.changeFont(new Font("Arial", 12, FontStyle.Bold));
            labelFPS.Color = Color.Red;
            labelFPS.Align = TgcText2D.TextAlign.RIGHT;

            labelVegetationHidden = new TgcText2D();
            labelVegetationHidden.Text = "Press V to show vegetation";
            labelVegetationHidden.changeFont(new Font("Arial", 12, FontStyle.Bold));
            labelVegetationHidden.Color = Color.GreenYellow;
            labelVegetationHidden.Format = DrawTextFormat.Bottom | DrawTextFormat.Center;

            //Crear Modifier especial para este editor
            Modifier = AddTerrainEditorModifier(this);
        }

        private void Panel3d_MouseLeave(object sender, EventArgs e)
        {
            Brush.mouseLeave(this);
        }

        private void Panel3d_MouseMove(object sender, MouseEventArgs e)
        {
            Brush.mouseMove(this);
        }

        /// <summary>
        ///     Retorna la posicion del mouse en el terreno, usando el metodo de picking configurado.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool mousePositionInTerrain(out TGCVector3 position)
        {
            pickingRay.updateRay();

            if (planePicking)
                return Terrain.intersectRayTGCPlane(pickingRay.Ray, out position);
            return Terrain.intersectRay(pickingRay.Ray, out position);
        }

        /// <summary>
        ///     Configura la escala del terreno.
        /// </summary>
        /// <param name="scaleXZ"></param>
        /// <param name="scaleY"></param>
        public void setScale(float scaleXZ, float scaleY)
        {
            var scaleRatioXZ = scaleXZ / Terrain.ScaleXZ;
            Terrain.ScaleXZ = scaleXZ;
            Terrain.ScaleY = scaleY;

            updateVegetationScale(scaleRatioXZ);
            updateVegetationY();
            TerrainFpsCamera.UpdateCamera(ElapsedTime);
        }

        /// <summary>
        ///     Carga el heightmap a partir de la textura del path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scaleXZ"></param>
        /// <param name="scaleY"></param>
        public void loadHeightmap(string path, float scaleXZ, float scaleY)
        {
            Terrain.loadHeightmap(path, scaleXZ, scaleY, TGCVector3.Empty);
            clearVegetation();
        }

        /// <summary>
        ///     Carga un heightmap plano.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="length"></param>
        /// <param name="level"></param>
        /// <param name="scaleXZ"></param>
        /// <param name="scaleY"></param>
        public void loadPlainHeightmap(int width, int length, int level, float scaleXZ, float scaleY)
        {
            Terrain.loadPlainHeightmap(width, length, level, scaleXZ, scaleY, TGCVector3.Empty);
            clearVegetation();
        }

        /// <summary>
        ///     Exporta el heightmap a un jpg.
        /// </summary>
        /// <param name="path"></param>
        public void save(string path)
        {
            var width = Terrain.HeightmapData.GetLength(0);
            var height = Terrain.HeightmapData.GetLength(1);
            var bitmap = new Bitmap(height, width);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var intensity = (int)Terrain.HeightmapData[i, j];
                    var pixel = Color.FromArgb(intensity, intensity, intensity);
                    bitmap.SetPixel(j, i, pixel);
                }
            }

            bitmap.Save(path, ImageFormat.Jpeg);
            bitmap.Dispose();
        }

        public override void Dispose()
        {
            Terrain.dispose();
            clearVegetation();
            Modifier.dispose();
            labelFPS.Dispose();
            labelVegetationHidden.Dispose();

            ToolsModel.Instance.Panel3d.MouseMove -= mouseMove;
            ToolsModel.Instance.Panel3d.MouseLeave -= mouseLeave;
        }

        #region Fields

        private TgcPickingRay pickingRay;
        private ITerrainEditorBrush brush;
        private bool showVegetation;
        private bool mustUpdateVegetationPosition;
        private DummyBrush noBrush;
        private float previousSpeed;
        private readonly TgcD3dInput.MouseButtons cameraRotationButton = TgcD3dInput.MouseButtons.BUTTON_RIGHT;
        private readonly TgcD3dInput.MouseButtons cameraRotationFPSButton = TgcD3dInput.MouseButtons.BUTTON_LEFT;
        private MouseEventHandler mouseMove;
        private EventHandler mouseLeave;
        private SmartTerrain terrain;
        private TgcText2D labelFPS;
        private TgcText2D labelVegetationHidden;

        #endregion Fields

        #region Properties

        public List<TgcMesh> Vegetation { get; private set; }

        /// <summary>
        ///     Obtiene o configura el terreno a editar
        /// </summary>
        public SmartTerrain Terrain
        {
            get { return terrain; }
            set
            {
                terrain = value;
                Camera.Terrain = value;
            }
        }

        /// <summary>
        ///     Obtiene o configura el brush a utilizar
        /// </summary>
        public ITerrainEditorBrush Brush
        {
            get
            {
                if (Camera.FpsModeEnable) return noBrush;
                return brush;
            }
            set
            {
                if (value == null) brush = noBrush;
                else brush = value;
            }
        }

        /// <summary>
        ///     Determina si se renderizaran los objetos sobre el terreno
        /// </summary>
        public bool ShowVegetation
        {
            get { return showVegetation; }
            set
            {
                showVegetation = value;
                if (showVegetation && mustUpdateVegetationPosition) updateVegetationY();
            }
        }

        /// <summary>
        ///     Determina el tipo de picking a utilizar. Cuando esta en true, se hace picking contra el plano del terreno (no
        ///     depende de la altura). En false hace picking contra las montanias.
        /// </summary>
        protected bool planePicking;

        public bool PlanePicking
        {
            get { return planePicking; }
            set
            {
                planePicking = value;
                Brush.mouseMove(this);
            }
        }

        /// <summary>
        ///     Obtiene la camara del editor.
        /// </summary>
        public TerrainFpsCamera Camera { get; private set; }

        /// <summary>
        ///     Setea el modo FPS de la camara. Cuando ese modo esta activo no se puede editar el terreno.
        /// </summary>
        public bool FpsModeEnable
        {
            get { return Camera.FpsModeEnable; }
            set
            {
                if (value && !Camera.FpsModeEnable)
                {
                    Camera.RotateMouseButton = cameraRotationFPSButton;
                    previousSpeed = Camera.MovementSpeed;
                    Camera.MovementSpeed /= 2;
                }
                else if (!value && Camera.FpsModeEnable)
                {
                    Camera.RotateMouseButton = cameraRotationButton;
                    Camera.MovementSpeed = previousSpeed;
                }
                Camera.FpsModeEnable = value;
            }
        }

        #endregion Properties

        public override void Update()
        {
            PreUpdate();
            PostUpdate();
        }

        #region Render

        public override void Render()
        {
            PreRender();

            if (Input.keyPressed(Key.V))
                ShowVegetation ^= true;

            if (Input.keyPressed(Key.P))
                planePicking ^= true;

            if (Input.keyPressed(Key.F))
                FpsModeEnable ^= true;

            if (Input.keyPressed(Key.B))
                RenderBoundingBoxes ^= true;

            if (FpsModeEnable) labelFPS.render();

            if (!ShowVegetation) labelVegetationHidden.render();

            Terrain.Technique = "PositionColoredTextured";

            Brush.update(this);

            Brush.render(this);

            PostRender();
        }

        public void doRender()
        {
            Terrain.render();
            if (ShowVegetation) renderVegetation();
        }

        public void renderVegetation()
        {
            foreach (var v in Vegetation)
            {
                v.Render();
                if (RenderBoundingBoxes) v.BoundingBox.Render();
            }
        }

        #endregion Render

        #region Vegetation

        /// <summary>
        ///     Actualiza la altura de la vegetacion segun la altura del terreno.
        /// </summary>
        public void updateVegetationY()
        {
            if (ShowVegetation)
            {
                foreach (var v in Vegetation) Terrain.setObjectPosition(v);

                mustUpdateVegetationPosition = false;
            }
            else mustUpdateVegetationPosition = true;
        }

        /// <summary>
        ///     Elimina toda la vegetacion.
        /// </summary>
        public void clearVegetation()
        {
            Vegetation.Clear();
            InstancesManager.Clear();
        }

        /// <summary>
        ///     Actualiza la escala de la vegetacion segun la variacion de escala scaleRatioXZ
        /// </summary>
        /// <param name="scaleRatioXZ"></param>
        private void updateVegetationScale(float scaleRatioXZ)
        {
            foreach (var v in Vegetation)
            {
                v.Scale = new TGCVector3(v.Scale.X * scaleRatioXZ, v.Scale.Y * scaleRatioXZ, v.Scale.Z * scaleRatioXZ);
                v.Position = TGCVector3.TransformCoordinate(v.Position, TGCMatrix.Scaling(scaleRatioXZ, 1, scaleRatioXZ));
            }
        }

        /// <summary>
        ///     Exporta la vegetacion a un -TgcScene.xml
        /// </summary>
        /// <param name="name"></param>
        /// <param name="saveFolderPath"></param>
        public void saveVegetation(string name, string saveFolderPath)
        {
            InstancesManager.Instance.export(Vegetation, name, saveFolderPath);
        }

        public void addVegetation(TgcMesh v)
        {
            Vegetation.Add(v);
        }

        /// <summary>
        ///     Remueve y retorna el ultimo mesh
        /// </summary>
        /// <returns></returns>
        public TgcMesh vegetationPop()
        {
            var count = Vegetation.Count;

            if (count == 0) return null;

            var last = Vegetation[count - 1];

            removeVegetation(last);

            return last;
        }

        public bool HasVegetation
        {
            get { return Vegetation.Count > 0; }
        }

        /// <summary>
        ///     Remueve el mesh, no le hace Dispose.
        /// </summary>
        /// <param name="v"></param>
        public void removeVegetation(TgcMesh v)
        {
            Vegetation.Remove(v);
        }

        public void addVegetation(List<TgcMesh> list)
        {
            Vegetation.AddRange(list);
        }

        public void removeDisposedVegetation()
        {
            var aux = new List<TgcMesh>();
            foreach (var m in Vegetation) if (m.D3dMesh != null) aux.Add(m);
            Vegetation = aux;
        }

        #endregion Vegetation
    }

    public class DummyBrush : ITerrainEditorBrush
    {
        public bool mouseMove(TgcTerrainEditor editor)
        {
            return false;
        }

        public bool mouseLeave(TgcTerrainEditor editor)
        {
            return false;
        }

        public bool update(TgcTerrainEditor editor)
        {
            return false;
        }

        public void render(TgcTerrainEditor editor)
        {
            editor.doRender();
        }

        public void dispose()
        {
        }
    }
}