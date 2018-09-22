using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Collision;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;
using TGC.Core.Text;
using TGC.Tools.Model;

namespace TGC.Tools.TerrainEditor.Brushes.Vegetation
{
    public class VegetationPicker : ITerrainEditorBrush
    {
        public enum RotationAxis
        {
            X,
            Y,
            Z
        }

        protected Font font;
        protected TgcText2D label;
        private TGCVector3 rotationAxis = new TGCVector3(0, 1, 0);
        private TGCVector3 scaleAxis = new TGCVector3(1, 1, 1);
        private readonly TgcStaticSound sound;

        public VegetationPicker(TgcTerrainEditor editor)
        {
            label = new TgcText2D();
            label.Color = Color.Yellow;
            label.Align = TgcText2D.TextAlign.LEFT;
            font = new Font("Arial", 12, FontStyle.Bold);
            label.changeFont(font);

            Rotation = RotationAxis.Y;
            SoundEnabled = true;
            sound = new TgcStaticSound();
            sound.loadSound(editor.MediaDir + "Sound\\pisada arena dcha.wav", -2000, editor.DirectSound.DsDevice);
        }

        protected TgcMesh Mesh { get; set; }

        public bool Enabled { get; set; }
        public TGCVector3 Position { get; set; }
        public RotationAxis Rotation { get; set; }

        public TGCVector3 ScaleAxis
        {
            get { return scaleAxis; }
            set { scaleAxis = TGCVector3.Normalize(value); }
        }

        public bool SoundEnabled { get; set; }

        private TgcMesh pickVegetation(TgcTerrainEditor editor)
        {
            var ray = new TgcPickingRay(editor.Input);
            ray.updateRay();
            float minT = -1;
            TgcMesh closerMesh = null;
            foreach (var v in editor.Vegetation)
            {
                TGCVector3 q;
                if (TgcCollisionUtils.intersectRayAABB(ray.Ray, v.BoundingBox, out q))
                {
                    float t = 0;
                    if (q != ray.Ray.Origin)
                    {
                        if (ray.Ray.Direction.X != 0) t = (q.X - ray.Ray.Origin.X) / ray.Ray.Direction.X;
                        else if (ray.Ray.Direction.Y != 0) t = (q.Y - ray.Ray.Origin.Y) / ray.Ray.Direction.Y;
                        else if (ray.Ray.Direction.Z != 0) t = (q.Z - ray.Ray.Origin.Z) / ray.Ray.Direction.Z;
                    }

                    if (minT == -1 || t < minT)
                    {
                        minT = t;
                        closerMesh = v;
                    }
                }
            }

            if (closerMesh != null) editor.removeVegetation(closerMesh);

            return closerMesh;
        }

        private void updateMeshScaleAndRotation(TgcTerrainEditor editor)
        {
            if (editor.Input.keyDown(Key.J))
            {
                rotate(FastMath.ToRad(60 * editor.ElapsedTime));
            }
            else if (editor.Input.keyDown(Key.L))
            {
                rotate(FastMath.ToRad(-60 * editor.ElapsedTime));
            }
            else if (editor.Input.keyDown(Key.I))
            {
                Mesh.Scale += ScaleAxis * 1.5f * editor.ElapsedTime;
            }
            else if (editor.Input.keyDown(Key.K))
            {
                Mesh.Scale -= ScaleAxis * 1.5f * editor.ElapsedTime;
            }
        }

        private void rotate(float p)
        {
            switch (Rotation)
            {
                case RotationAxis.X:
                    // mesh.rotateX(p);
                    Mesh.Rotation = new TGCVector3((Mesh.Rotation.X + p) % FastMath.TWO_PI, Mesh.Rotation.Y, Mesh.Rotation.Z);

                    break;

                case RotationAxis.Y:
                    //mesh.rotateY(p);
                    Mesh.Rotation = new TGCVector3(Mesh.Rotation.X, (Mesh.Rotation.Y + p) % FastMath.TWO_PI, Mesh.Rotation.Z);
                    break;

                case RotationAxis.Z:
                    //mesh.rotateZ(p);
                    Mesh.Rotation = new TGCVector3(Mesh.Rotation.X, Mesh.Rotation.Y, (Mesh.Rotation.Z + p) % FastMath.TWO_PI);
                    break;
            }
        }

        protected virtual void addVegetation(TgcTerrainEditor editor)
        {
            editor.addVegetation(Mesh);
            if (SoundEnabled) playSound();
            Mesh = null;
        }

        public void playSound()
        {
            sound.play();
        }

        private void renderFloatingVegetation()
        {
            if (Mesh != null)
            {
                Mesh.Render();
                Mesh.BoundingBox.Render();
                label.render();
            }
        }

        public void removeFloatingVegetation()
        {
            if (Mesh != null)
            {
                Mesh.Dispose();
                Mesh = null;
            }
        }

        #region ITerrainEditorBrush

        public virtual bool mouseMove(TgcTerrainEditor editor)
        {
            TGCVector3 pos;
            Enabled = true;
            if (Mesh != null)
            {
                if (editor.mousePositionInTerrain(out pos))
                {
                    Position = pos;
                    Mesh.Position = pos;

                    setLabelPosition(editor);
                }
            }

            return false;
        }

        private void setLabelPosition(TgcTerrainEditor editor)
        {
            label.Text = string.Format("\"{0}\"( {1} ; {2} ; {3} )", Mesh.Name, Mesh.Position.X, Mesh.Position.Y,
                Mesh.Position.Z);
            SizeF nameSize;

            using (var g = ToolsModel.Instance.Panel3d.CreateGraphics())
            {
                nameSize = g.MeasureString(label.Text, font);
            }

            label.Size = nameSize.ToSize();
            label.Position = new Point((int)(editor.Input.Xpos - nameSize.Width / 2),
                (int)(editor.Input.Ypos + nameSize.Height + 5));
        }

        public bool mouseLeave(TgcTerrainEditor editor)
        {
            Enabled = false;
            return false;
        }

        public virtual bool update(TgcTerrainEditor editor)
        {
            var changes = false;
            if (Enabled)
            {
                if (Mesh != null) updateMeshScaleAndRotation(editor);

                if (editor.Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    if (Mesh != null)
                    {
                        addVegetation(editor);
                        changes = true;
                    }
                    else
                    {
                        Mesh = pickVegetation(editor);
                        if (Mesh != null)
                        {
                            changes = true;
                            setLabelPosition(editor);
                        }
                    }
                }

                if (editor.Input.keyPressed(Key.Delete))
                {
                    removeFloatingVegetation();
                }

                if (editor.Input.keyPressed(Key.Z))
                {
                    if (editor.HasVegetation)
                    {
                        removeFloatingVegetation();
                        Mesh = editor.vegetationPop();
                    }
                }
            }

            return changes;
        }

        public void render(TgcTerrainEditor editor)
        {
            editor.doRender();
            if (Enabled) renderFloatingVegetation();
        }

        public void dispose()
        {
            removeFloatingVegetation();
            label.Dispose();
            sound.dispose();
        }

        #endregion ITerrainEditorBrush
    }
}