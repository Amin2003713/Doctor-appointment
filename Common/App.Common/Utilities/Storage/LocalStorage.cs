#region

using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;

#endregion

namespace App.Common.Utilities.Storage;

public class LocalStorage(ILocalStorageService storage) : ILocalStorage
{
    public async Task SetAsync<TItem>(string key , TItem data)
    {
        var itemJson    = JsonSerializer.Serialize(data);
        var itemEncoded = Encoding.UTF32.GetBytes(itemJson);
        var base64      = Convert.ToBase64String(itemEncoded);

        await storage.SetItemAsync(key , base64);
    }

    public async Task<TItem?> GetAsync<TItem>(string key)
    {
        var resultBase64 = await storage.GetItemAsync<string>(key)!;
        if (resultBase64 is null)
            return default!;
        var resultEncoded = Convert.FromBase64String(resultBase64!);
        var itemJson      = Encoding.UTF32.GetString(resultEncoded);
        var item          = JsonSerializer.Deserialize<TItem>(itemJson);

        return item ?? default!;
    }


    public async Task UpdateAsync<TItem>(string key , TItem data)
    {
        var result = await GetAsync<TItem>(key);

        if (result is not null) await DeleteAsync(key);

        await SetAsync(key , data);
    }

    public async Task DeleteAsync(string key)
    {
        await storage.RemoveItemAsync(key);
    }
}

