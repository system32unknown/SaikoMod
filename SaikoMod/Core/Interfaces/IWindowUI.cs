namespace SaikoMod.Core.Interfaces
{
    public interface IWindowUI
    {
        /// <summary>Draws window content (excluding title bar).</summary>
        void Draw();

        /// <summary>Title displayed on the window.</summary>
        string Title { get; }

        /// <summary>Called when the window is closed.</summary>
        void OnClose();
    }
}