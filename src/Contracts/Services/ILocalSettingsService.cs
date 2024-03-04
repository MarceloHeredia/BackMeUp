using BackMeUp.Models;
using System.Linq.Expressions;

namespace BackMeUp.Contracts.Services;

public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(Expression<Func<Settings, T>> selector);
    Task SaveSettingAsync<T>(Expression<Func<Settings, T>> selector, T value);
}