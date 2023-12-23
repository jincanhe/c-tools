using System;
using System.Text.Json.Serialization;

namespace renameItem
{
    public class ItemData
    {
        [JsonPropertyName("itemId")]  public long itemId { get; set; }
        [JsonPropertyName("name")]  public string name { get; set; }

        public ItemData()
        {

        }
    }
}