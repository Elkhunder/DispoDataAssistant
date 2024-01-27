using System;
using System.Collections;
using System.Collections.Generic;

namespace DispoDataAssistant.Data.Models.Settings;

public class GeneralTypes : IEnumerable<String>
{
    public List<String> Types { get; set; }
    private readonly List<String> _collection = [];

    public GeneralTypes()
    {
        _collection =
        [
            "FontSize",
        ];

        Types = _collection;
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (string s in _collection)
        {
            yield return s;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
