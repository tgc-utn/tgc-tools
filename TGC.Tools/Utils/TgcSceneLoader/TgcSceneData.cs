using TGC.Tools.Utils.PortalRendering;

namespace TGC.Tools.Utils.TgcSceneLoader
{
    public class TgcSceneData
    {
        //Info de LightMaps
        public string lightmapsDir;

        public bool lightmapsEnabled;
        public TgcMaterialData[] materialsData;

        public TgcMeshData[] meshesData;
        public string name;

        public float[] pMax;

        //BoundingBox de la escena
        public float[] pMin;

        //Datos de PortalRendering
        public TgcPortalRenderingData portalData;

        public string texturesDir;
    }
}