using ManaSword.Debug;

namespace ManaSword.SaveLoadSystem
{
    public interface ISaveLoadEntity : IDebugMode
    {
        void Load();
        void Save();
    }
}
