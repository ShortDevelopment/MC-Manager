using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MC_Manager.Portfolio
{
    public class PortfolioInfo
    {
        public PortfolioInfo(StorageFolder folder)
        {
            this.Folder = folder;
        }
        public StorageFolder Folder { get; private set; }

        public PortfolioInfo(StorageFolder folder, Worlds.WorldInfo worldInfo) : this(folder)
        {
            this.WorldInfo = worldInfo;
        }
        public Worlds.WorldInfo WorldInfo { get; private set; }

        public string Name
        {
            get
            {
                if (WorldInfo == null)
                    return $"{this.Folder.DisplayName} (server)";

                return $"{this.WorldInfo.Name} (local)";
            }
        }

        public async Task<PortfolioItemInfo[]> GetItemsAsync()
        {
            try
            {
                StorageFolder photosDir = await this.Folder.GetFolderAsync("photos");

                List<PortfolioItemInfo> items = new List<PortfolioItemInfo>();
                foreach (StorageFolder folder in await photosDir.GetFoldersAsync())
                {
                    items.AddRange((await folder.GetFilesAsync()).Select((file) => new PortfolioItemInfo(file)));
                }
                return items.ToArray();
            }
            catch
            {
                return new PortfolioItemInfo[] { };
            }
        }
    }
}
