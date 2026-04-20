namespace Idology.Engine.Translations;

internal sealed class TranslationService : ITranslationService
{

    private readonly IDictionary<string, IDictionary<string, string>> _translationsByCulture;

    public TranslationService(IDictionary<string, string> translations)
    {
        _translationsByCulture = new Dictionary<string, IDictionary<string, string>>
        {
            { CultureInfo.CurrentUICulture.Name, translations }
        };
    }

    public LocalizedString this[string name]
        => new(
            name,
            _translationsByCulture[CultureInfo.CurrentUICulture.Name].TryGetValue(name, out var value)
                ? value
                : name,
            resourceNotFound: !_translationsByCulture[CultureInfo.CurrentUICulture.Name].ContainsKey(name));

    public LocalizedString this[string name, params object[] arguments]
        => new(
            name,
            string.Format(_translationsByCulture[CultureInfo.CurrentUICulture.Name].TryGetValue(name, out var value)
                ? value
                : name, arguments),
            resourceNotFound: !_translationsByCulture[CultureInfo.CurrentUICulture.Name].ContainsKey(name));

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        foreach (var kvp in _translationsByCulture[CultureInfo.CurrentUICulture.Name])
        {
            yield return new LocalizedString(kvp.Key, kvp.Value, resourceNotFound: false);
        }
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
        => this;
}
