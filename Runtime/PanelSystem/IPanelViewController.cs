﻿using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public interface IPanelViewContainer
    {
        RectTransform ParentTransform { get; }
    }
    
    public interface IPanelViewController
    {
        PanelViewControllerState State { get; }
        PanelType PanelType { get; }
        PanelViewBase View { get; }
        IPanelViewContainer ParentViewContainer { get; }
        bool IsViewLoaded { get; }
        Task LoadViewAsync();
        Task HideAsync(bool immediate = false);
        Task ShowAsync(bool immediate = false);
        void SetParentViewContainer(IPanelViewContainer parent);
    }
}
