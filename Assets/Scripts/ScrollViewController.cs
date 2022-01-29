using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

public class ScrollViewController : MonoBehaviour
{
    [SerializeField] private string resourcesPath;
    [SerializeField] private Transform elementsParent;
    [SerializeField] private Button resetButton;
    private ListElement.Pool _pool;
    private Dictionary<Sprite, ListElement> spawned = new Dictionary<Sprite, ListElement>();

   [Inject]
    private void Construct (ListElement.Pool pool)
	{
        _pool = pool;
    }

    private void Start()
    {
        ResetElements();
        resetButton.onClick.AddListener(ResetElements);
    }

	private void OnDestroy ()
	{
        resetButton.onClick.RemoveListener(ResetElements);
    }

	private void ResetElements ()
	{
        Sprite[] folderSprites = Resources.LoadAll<Sprite>(resourcesPath);

        foreach (Sprite sprite in folderSprites)
        {
                string x = AssetDatabase.GetAssetPath(sprite);
                DateTime creationTime = System.IO.File.GetCreationTime(x);

                if (spawned.ContainsKey(sprite))
                {
                    spawned[sprite].Reset(sprite.name, creationTime);
                    spawned[sprite].Edited = true;
                }
                else
                {
                    ListElement createdListElement = _pool.Spawn(sprite.name, creationTime, sprite);
                    createdListElement.Edited = true;
                    spawned.Add(sprite, createdListElement);
                    createdListElement.transform.SetParent(elementsParent);
                }
        }

        RemoveMissingSprites();
    }

    private void RemoveMissingSprites ()
	{
        KeyValuePair<Sprite, ListElement> pair;

        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            pair = spawned.ElementAt(i);

            if (pair.Value.Edited == false)
            {
                spawned.Remove(pair.Key);
                _pool.Despawn(pair.Value);
                continue;
            }

            pair.Value.Edited = false;
        }
    }

}
