using App.Common.Utilities.LifeTime;

namespace App.Common.Utilities.Storage;

public interface ILocalStorage   : IScopedDependency
{
    Task         SetAsync<TItem>(string key , TItem data);
    Task<TItem?> GetAsync<TItem>(string key);
    Task         DeleteAsync(string key);
    Task         UpdateAsync<TItem>(string key , TItem data);
}