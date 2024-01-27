using System;
using System.Collections;
using System.Collections.Generic;

namespace DispoDataAssistant.Data.Models.Settings;

public class IntegrationTypes : IEnumerable<string>
{
    private readonly List<String> _collection = [];
    public IntegrationTypes()
    {
        _collection =
        [
            "Service Now",
            "Twitch",
            "Facebook"
        ];
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
