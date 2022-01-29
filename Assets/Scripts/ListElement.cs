using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ListElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fileDate;
    [SerializeField] private TextMeshProUGUI fileName;
	[SerializeField] private Image image; 

	public bool Edited { get; set; }

	public void Reset (string name, DateTime time)
	{
		TimeSpan timeDifference = DateTime.Now.Subtract(time);
		fileName.text = $"Name: {name}";
		fileDate.text = $"Time passed from file creation: {timeDifference.ToString("dd\\.hh\\:mm\\:ss")}";
	}

	public void SetSprite (Sprite sprite)
	{
		image.sprite = sprite;
	}

	public class Pool : MonoMemoryPool<string, DateTime, Sprite, ListElement> 
	{
		protected override void Reinitialize (string name, DateTime time, Sprite sprite, ListElement listElement)
		{
			listElement.Reset(name, time);
			listElement.SetSprite(sprite);
		}
	}
}
