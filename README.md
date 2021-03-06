# UnityScriptableObjectIcons
## What?
Its just an Editor script, which allows you to have icons for Scriptable Objects from a Sprite or Texture2D Field.

Without the Editorscript:

![asset view disabled](Images/disabled.PNG)

With the Editorscript enabled:

![asset view enabled](Images/enabled.PNG)

Icons can be Enabled/Disabled in the Assetmenu:

![asset view enabled](Images/assetmenu.PNG)

## Usage

To add an thumbnail for Scriptable just add the [ScriptableObjectIcon] attribute to the supported field, which you want to use as the thumbnail:

```csharp
[CreateAssetMenu(fileName = "GenericItem")]
public class BlockType : ScriptableObject
{
    [ScriptableObjectIcon]
    public Sprite sprite;
    //or
    [ScriptableObjectIcon]
    public Texture2D texture;    


    //...
    public List<Field> OtherFields {get;set;}

}
```

## Future Development

- allow properties (Is it needed?)
- allow other texture/sprite types
