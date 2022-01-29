using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ListElement listElementPrefab;
    public override void InstallBindings()
    {
        Container.BindMemoryPool<ListElement, ListElement.Pool>()
        .WithInitialSize(2)
        .FromComponentInNewPrefab(listElementPrefab)
        .UnderTransformGroup("Elements");
    }
}