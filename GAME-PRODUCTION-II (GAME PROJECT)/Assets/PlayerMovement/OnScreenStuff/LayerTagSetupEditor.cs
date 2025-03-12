using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class LayerTagSetupEditor
{
    static LayerTagSetupEditor()
    {
        // Create the layer if it doesn't exist
        CreateLayerIfNotExists("Interactable");

        // Create the tag if it doesn't exist
        CreateTagIfNotExists("Interactable");
    }

    static void CreateLayerIfNotExists(string layerName)
    {
        // Check if the layer already exists
        int layerIndex = LayerMask.NameToLayer(layerName);
        if (layerIndex == -1)
        {
            Debug.Log($"Layer '{layerName}' does not exist. Please create it.");
            // Automatically add the layer if it's missing (Layer creation needs to be done manually by the user, as Unity doesn't support this via script)
            return;
        }
    }

    static void CreateTagIfNotExists(string tagName)
    {
        // Check if the tag already exists
        bool tagExists = false;
        foreach (string tag in UnityEditorInternal.InternalEditorUtility.tags)
        {
            if (tag == tagName)
            {
                tagExists = true;
                break;
            }
        }

        if (!tagExists)
        {
            Debug.Log($"Tag '{tagName}' does not exist. Please create it.");
            // Normally you can't add a tag via script, so you'll need to instruct the user to add it manually
        }
    }
}
