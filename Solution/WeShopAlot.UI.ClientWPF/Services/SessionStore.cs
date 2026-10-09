using System.IO;
using System.Text.Json;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>
    /// What Angular keeps in the browser's localStorage ("token" and "basket_id"), kept in a small file:
    /// %LOCALAPPDATA%\WeShopAlot\session.json. Lets the app remember the sign-in and basket between runs.
    /// </summary>
    public class SessionStore
    {
        private readonly string path;

        public SessionStore()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WeShopAlot");
            path = Path.Combine(folder, "session.json");
            Load();
        }

        public string? Token { get; set; }

        public string? BasketId { get; set; }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllText(path, JsonSerializer.Serialize(new SessionData(Token, BasketId)));
            }
            catch (IOException)
            {
                // Remembering the session is a convenience; the app works without it.
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(path)) return;
                var data = JsonSerializer.Deserialize<SessionData>(File.ReadAllText(path));
                Token = data?.Token;
                BasketId = data?.BasketId;
            }
            catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
            {
            }
        }

        private record SessionData(string? Token, string? BasketId);
    }
}
