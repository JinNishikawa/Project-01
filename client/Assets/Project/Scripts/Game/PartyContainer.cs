using VContainer;
using VContainer.Unity;
using UnityEngine;

[RequireComponent(typeof(Party))]
public class PartyContainer : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<Party>();
    }
}   