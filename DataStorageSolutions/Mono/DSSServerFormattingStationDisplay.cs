﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStorageSolutions.Buildables;
using DataStorageSolutions.Model;
using FCSCommon.Abstract;
using FCSCommon.Components;
using FCSCommon.Enums;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace DataStorageSolutions.Mono
{
    internal class DSSServerFormattingStationDisplay : AIDisplay
    {
        private DSSServerFormattingStationController _mono;
        private GridHelper _filterGrid;
        private int _page;
        private GridHelper _itemGrid;
        private GridHelper _categoryGrid;
        private readonly Color _startColor = Color.gray;
        private readonly Color _hoverColor = Color.white;
        private List<Filter> _filters => FilterList.GetFilters();
        private List<Filter> _grouped = new List<Filter>();

        internal void Setup(DSSServerFormattingStationController mono)
        {
            _mono = mono;
            
            if (FindAllComponents())
            {
                QuickLogger.Debug("Passed",true);
                _page = Animator.StringToHash("Page");
                _filterGrid?.DrawPage(1);
                PowerOnDisplay();
            }
        }

        public override void PowerOnDisplay()
        {
            GoToPage(FilterPages.Home);
        }

        public override void OnButtonClick(string btnName, object tag)
        {
            if (btnName == string.Empty) return;

            switch (btnName)
            {
                case "HomeBTN":
                    GoToPage(FilterPages.FilterPage);
                    break;

                //case "ItemBTN":
                //    _currentBase?.TakeItem((TechType)tag);
                //    break;

                case "AddServerBTN":
                    _mono.DumpContainer.OpenStorage();
                    break;

                case "RemoveServerBTN":
                    //TODO Give Player back a server
                    GoToPage(FilterPages.Home);
                    _mono.ToggleDummyServer();
                    _mono.GivePlayerItem();
                    break;

                case "AddCategoryBTN":
                    GoToPage(FilterPages.Categories);
                    break;

                case "AddItemBTN":
                    GoToPage(FilterPages.Items);
                    break;

                    //case "ColorPickerBTN":
                    //    //TODO Check if antenna is attached
                    //    _mono.AnimationManager.SetIntHash(_page, 3);
                    //    break;

                    //case "ColorItem":
                    //    if (_currentColorPage == ColorPage.Terminal)
                    //        _mono.TerminalColorManager.ChangeColorMask((Color)tag);
                    //    else
                    //        _mono.AntennaColorManager.ChangeColorMask((Color)tag);
                    //    break;
            }
        }

        public override bool FindAllComponents()
        {
            try
            {
                #region Canvas  
                var canvasGameObject = gameObject.GetComponentInChildren<Canvas>()?.gameObject;

                if (canvasGameObject == null)
                {
                    throw new MissingComponentException($"A component cant be found.\nMissing Component: Canvas");
                }
                #endregion

                #region Home Page
                var home = InterfaceHelpers.FindGameObject(canvasGameObject, "HomePage");
                #endregion

                #region Filter Page
                var filterPage = InterfaceHelpers.FindGameObject(canvasGameObject, "FilterPage");
                #endregion

                #region Category Page
                var categoryPage = InterfaceHelpers.FindGameObject(canvasGameObject, "CategoryPage");
                #endregion

                #region Item Page
                var itemPage = InterfaceHelpers.FindGameObject(canvasGameObject, "ItemPage");
                #endregion

                #region Filter Grid

                _filterGrid = _mono.gameObject.AddComponent<GridHelper>();
                _filterGrid.OnLoadDisplay += OnLoadFilterGrid;
                _filterGrid.Setup(3, DSSModelPrefab.FilterItemPrefab, filterPage, _startColor, _hoverColor, OnButtonClick); //Minus 1 ItemPerPage because of the added Home button

                #endregion

                #region Item Grid

                _itemGrid = _mono.gameObject.AddComponent<GridHelper>();
                _itemGrid.OnLoadDisplay += OnLoadItemsGrid;
                _itemGrid.Setup(3, DSSModelPrefab.FilterItemPrefab, itemPage, _startColor, _hoverColor, OnButtonClick); //Minus 1 ItemPerPage because of the added Home button

                #endregion

                #region Category Grid

                _categoryGrid = _mono.gameObject.AddComponent<GridHelper>();
                _categoryGrid.OnLoadDisplay += OnLoadCategoryGrid;
                _categoryGrid.Setup(3, DSSModelPrefab.FilterItemPrefab, categoryPage, _startColor, _hoverColor, OnButtonClick); //Minus 1 ItemPerPage because of the added Home button

                #endregion

                #region OpenRackBTNButton
                var removeServerBTN = InterfaceHelpers.FindGameObject(filterPage, "RemoveServerBTN");

                InterfaceHelpers.CreateButton(removeServerBTN, "RemoveServerBTN", InterfaceButtonMode.Background,
                    OnButtonClick, _startColor, _hoverColor, MAX_INTERACTION_DISTANCE, AuxPatchers.OpenServerRackPage());
                #endregion

                #region CloseRackBTNButton
                var addServerBTN = InterfaceHelpers.FindGameObject(home, "AddServerBTN");

                InterfaceHelpers.CreateButton(addServerBTN, "AddServerBTN", InterfaceButtonMode.Background,
                    OnButtonClick, _startColor, _hoverColor, MAX_INTERACTION_DISTANCE, AuxPatchers.CloseServerRackPage());
                #endregion

                #region AddItemBTN
                var itemPageBTN = InterfaceHelpers.FindGameObject(filterPage, "AddItemBTN");

                InterfaceHelpers.CreateButton(itemPageBTN, "AddItemBTN", InterfaceButtonMode.Background,
                    OnButtonClick, _startColor, _hoverColor, MAX_INTERACTION_DISTANCE, AuxPatchers.CloseServerRackPage());
                #endregion

                #region AddCategoryBTN
                var categoryBTN = InterfaceHelpers.FindGameObject(filterPage, "AddCategoryBTN");

                InterfaceHelpers.CreateButton(categoryBTN, "AddCategoryBTN", InterfaceButtonMode.Background,
                    OnButtonClick, _startColor, _hoverColor, MAX_INTERACTION_DISTANCE, AuxPatchers.CloseServerRackPage());
                #endregion

                return true;
            }
            catch (Exception e)
            {
                QuickLogger.Error($"{e.Message}: {e.StackTrace}");
                return false;
            }
        }

        private void OnLoadFilterGrid(DisplayData obj)
        {
            
        }

        private void OnLoadItemsGrid(DisplayData data)
        {
            _itemGrid.ClearPage();

            _grouped = new List<Filter>();

            QuickLogger.Debug("Load Filters");

            foreach (var filter in _filters)
            {
                if (!filter.IsCategory())
                {
                    _grouped.Add(filter);
                }
            }

            if (data.EndPosition > _grouped.Count)
            {
                data.EndPosition = _grouped.Count;
            }

            for (int i = data.StartPosition; i < data.EndPosition; i++)
            {
                GameObject buttonPrefab = Instantiate(data.ItemsPrefab);

                if (buttonPrefab == null || data.ItemsGrid == null)
                {
                    if (buttonPrefab != null)
                    {
                        QuickLogger.Debug("Destroying Tab", true);
                        Destroy(buttonPrefab);
                    }
                    return;
                }

                CreateButton(data, buttonPrefab, _grouped, i);
            }
            _itemGrid.UpdaterPaginator(_grouped.Count);
        }

        private void OnLoadCategoryGrid(DisplayData data)
        {
            _categoryGrid.ClearPage();
            
            _grouped = new List<Filter>();

            foreach (var filter in _filters)
            {
                if (filter.IsCategory())
                {
                    _grouped.Add(filter);
                }
            }

            if (data.EndPosition > _grouped.Count)
            {
                data.EndPosition = _grouped.Count;
            }

            for (int i = data.StartPosition; i < data.EndPosition; i++)
            {
                GameObject buttonPrefab = Instantiate(data.ItemsPrefab);

                if (buttonPrefab == null || data.ItemsGrid == null)
                {
                    if (buttonPrefab != null)
                    {
                        QuickLogger.Debug("Destroying Tab", true);
                        Destroy(buttonPrefab);
                    }
                    return;
                }
                
                CreateButton(data, buttonPrefab, _grouped, i);
            }
            _categoryGrid.UpdaterPaginator(_grouped.Count);
        }

        private void CreateButton(DisplayData data, GameObject buttonPrefab, List<Filter> grouped, int i)
        {
            buttonPrefab.transform.SetParent(data.ItemsGrid.transform, false);
            buttonPrefab.GetComponentInChildren<Text>().text = grouped[i].GetString();

            var mainBTN = buttonPrefab.AddComponent<InterfaceButton>();
            mainBTN.ButtonMode = InterfaceButtonMode.Background;
            mainBTN.STARTING_COLOR = _startColor;
            mainBTN.HOVER_COLOR = _hoverColor;
            mainBTN.BtnName = "BaseBTN";
            mainBTN.Tag = grouped[i].Types;
            mainBTN.OnButtonClick = OnButtonClick;
        }


        internal void GoToPage(FilterPages page)
        {
            QuickLogger.Debug($"Going to page{page} | {(int)page}");
            _mono.AnimationManager.SetIntHash(_page,(int)page);
        }
    }

    internal enum FilterPages
    {
        Blackout = 0,
        Home = 1,
        FilterPage = 2,
        Items = 3,
        Categories = 4
    }
}
