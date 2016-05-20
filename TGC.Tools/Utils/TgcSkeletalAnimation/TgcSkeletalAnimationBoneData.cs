namespace TgcViewer.Utils.TgcSkeletalAnimation
{
    /// <summary>
    /// Información de animación de un hueso para una animación particular
    /// </summary>
    public class TgcSkeletalAnimationBoneData
    {
        public int id;
        public int keyFramesCount;

        public TgcSkeletalAnimationBoneFrameData[] keyFrames;
    }
}