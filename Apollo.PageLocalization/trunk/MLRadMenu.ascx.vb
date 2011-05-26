'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports System.Drawing
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports Apollo.DNN_Localization

Namespace Apollo.DNN.SkinObjects

    Partial Public Class MLRadMenu
        Inherits UI.Skins.SkinObjectBase

#Region "Private Variables"

        'variables and structures
        Private PagesQueue As Queue
        Private AuthPages As ArrayList
        Private arrayShowPath As ArrayList
        Private dnnSkinSrc As String = PortalSettings.ActiveTab.SkinSrc.Replace("\"c, "/"c).Replace("//", "/")
        Private dnnSkinPath As String = dnnSkinSrc.Substring(0, dnnSkinSrc.LastIndexOf("/"c))

        Private Structure qElement
            Dim radMenuItem As RadMenuItem
            Dim page As TabInfo
            Dim item As Integer
        End Structure

        Private _Style As System.String = String.Empty

        'menu group properties
        Private _GroupFlow As ItemFlow = ItemFlow.Vertical
        Private _GroupExpandDirection As ExpandDirection = ExpandDirection.Auto
        Private _GroupOffsetX As System.Int32 = 0
        Private _GroupOffsetY As System.Int32 = 0
        Private _GroupWidth As System.Web.UI.WebControls.Unit = Unit.Empty
        Private _GroupHeight As System.Web.UI.WebControls.Unit = Unit.Empty

        'menu item properties
        Private _ItemCssClass As System.String = String.Empty
        Private _ItemDisabledCssClass As System.String = String.Empty
        Private _ItemExpandedCssClass As System.String = String.Empty
        Private _ItemFocusedCssClass As System.String = String.Empty
        Private _ItemClickedCssClass As System.String = String.Empty
        Private _ItemImageUrl As System.String = String.Empty
        Private _ItemHoveredImageUrl As System.String = String.Empty
        Private _ItemTarget As System.String = String.Empty
        Private _ItemBackColor As System.Drawing.Color = Color.Empty
        Private _ItemBorderColor As System.Drawing.Color = Color.Empty
        Private _ItemBorderWidth As System.Web.UI.WebControls.Unit = Unit.Empty
        Private _ItemBorderStyle As System.Web.UI.WebControls.BorderStyle = BorderStyle.None
        Private _ItemForeColor As System.Drawing.Color = Color.Empty
        Private _ItemHeight As System.Web.UI.WebControls.Unit = Unit.Empty
        Private _ItemWidth As System.Web.UI.WebControls.Unit = Unit.Empty

        'menu ROOT item properties
        Private _RootItemCssClass As System.String = String.Empty
        Private _RootItemDisabledCssClass As System.String = String.Empty
        Private _RootItemExpandedCssClass As System.String = String.Empty
        Private _RootItemFocusedCssClass As System.String = String.Empty
        Private _RootItemClickedCssClass As System.String = String.Empty
        Private _RootItemImageUrl As System.String = String.Empty
        Private _RootItemHoveredImageUrl As System.String = String.Empty
        Private _RootItemTarget As System.String = String.Empty
        Private _RootItemBackColor As System.Drawing.Color = Color.Empty
        Private _RootItemBorderColor As System.Drawing.Color = Color.Empty
        Private _RootItemBorderWidth As System.Web.UI.WebControls.Unit = Unit.Empty
        Private _RootItemBorderStyle As System.Web.UI.WebControls.BorderStyle = BorderStyle.None
        Private _RootItemForeColor As System.Drawing.Color = Color.Empty
        Private _RootItemHeight As System.Web.UI.WebControls.Unit = Unit.Empty
        Private _RootItemWidth As System.Web.UI.WebControls.Unit = Unit.Empty

        'separator and first/last items
        Private _HeaderFirstItem As String
        Private _HeaderFirstItemCssClass As String
        Private _HeaderFirstItemCssClassClicked As String
        Private _HeaderFirstItemCssClassOver As String

        Private _HeaderSeparator As String
        Private _HeaderSeparatorCssClass As String
        Private _HeaderSeparatorCssClassClicked As String
        Private _HeaderSeparatorCssClassOver As String

        Private _HeaderLastItem As String
        Private _HeaderLastItemCssClass As String
        Private _HeaderLastItemCssClassClicked As String
        Private _HeaderLastItemCssClassOver As String

        Private _ChildGroupFirstItem As String
        Private _ChildGroupFirstItemCssClass As String
        Private _ChildGroupFirstItemCssClassClicked As String
        Private _ChildGroupFirstItemCssClassOver As String

        Private _ChildGroupSeparator As String
        Private _ChildGroupSeparatorCssClass As String
        Private _ChildGroupSeparatorCssClassClicked As String
        Private _ChildGroupSeparatorCssClassOver As String

        Private _ChildGroupLastItem As String
        Private _ChildGroupLastItemCssClass As String
        Private _ChildGroupLastItemCssClassClicked As String
        Private _ChildGroupLastItemCssClassOver As String

        Private _SelectedPathHeaderItemCss As String
        Private _SelectedPathItemCss As String
        Private _SelectedPathItemImage As String
        Private _SelectedPathHeaderItemImage As String

        'other properties
        Private _ShowPath As Boolean = False
        Private _EnableToolTips As Boolean
        Private _ImagesOnlyMenu As Boolean = False
        Private _EnableLevelCss As Boolean = False
        Private _EnableItemCss As Boolean = False
        Private _EnableRootItemCss As Boolean = False
        Private _MaxLevelNumber As Integer = 10
        Private _MaxItemNumber As Integer = 20
        Private _MaxLevel As Integer = -1
        Private _ShowOnlyCurrent As String = String.Empty
        Private _EnablePageIdCssClass As Boolean = False
        Private _EnablePageIcons As Boolean = True
        Private _EnableUserMenus As Boolean = True
        Private _EnableAdminMenus As Boolean = True
        Private _CopyChildItemLink As Boolean = False
        Private _PagesToExclude As String = String.Empty

#End Region

#Region "Public Functions"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="match"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindItem(ByVal match As System.Predicate(Of Telerik.Web.UI.RadMenuItem)) As Telerik.Web.UI.RadMenuItem
            Return RadMenu1.FindItem(match)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindItemByText(ByVal text As String) As RadMenuItem
            Return RadMenu1.FindItemByText(text)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="ignoreCase"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindItemByText(ByVal text As String, ByVal ignoreCase As Boolean) As Telerik.Web.UI.RadMenuItem
            Return RadMenu1.FindItemByText(text, ignoreCase)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="url"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindItemByUrl(ByVal url As String) As Telerik.Web.UI.RadMenuItem
            Return RadMenu1.FindItemByUrl(url)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Items() As RadMenuItemCollection
            Return RadMenu1.Items
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllItems() As System.Collections.Generic.IList(Of Telerik.Web.UI.RadMenuItem)
            Return RadMenu1.GetAllItems
        End Function

#End Region

#Region "Public Properties"

        Public Property AutoScrollMinimumHeight() As Integer
            Get
                Return RadMenu1.AutoScrollMinimumHeight
            End Get
            Set(ByVal Value As Integer)
                RadMenu1.AutoScrollMinimumHeight = Value
            End Set
        End Property

        Public Property AutoScrollMinimumWidth() As Integer
            Get
                Return RadMenu1.AutoScrollMinimumWidth
            End Get
            Set(ByVal Value As Integer)
                RadMenu1.AutoScrollMinimumWidth = Value
            End Set
        End Property

        Public Property ExpandAnimationType() As AnimationType
            Get
                Return RadMenu1.ExpandAnimation.Type
            End Get
            Set(ByVal Value As AnimationType)
                RadMenu1.ExpandAnimation.Type = Value
            End Set
        End Property

        Public Property ExpandAnimationDuration() As Integer
            Get
                Return RadMenu1.ExpandAnimation.Duration
            End Get
            Set(ByVal Value As Integer)
                RadMenu1.ExpandAnimation.Duration = Value
            End Set
        End Property

        Public Property CausesValidation() As System.Boolean
            Get
                Return RadMenu1.CausesValidation
            End Get
            Set(ByVal Value As System.Boolean)
                RadMenu1.CausesValidation = Value
            End Set
        End Property

        Public Property ChildGroupFirstItem() As String
            Get
                Return _ChildGroupFirstItem
            End Get
            Set(ByVal Value As String)
                _ChildGroupFirstItem = Value
            End Set
        End Property

        Public Property ChildGroupFirstItemCssClass() As String
            Get
                Return _ChildGroupFirstItemCssClass
            End Get
            Set(ByVal Value As String)
                _ChildGroupFirstItemCssClass = Value
            End Set
        End Property

        Public Property ChildGroupFirstItemCssClassClicked() As String
            Get
                Return _ChildGroupFirstItemCssClassClicked
            End Get
            Set(ByVal Value As String)
                _ChildGroupFirstItemCssClassClicked = Value
            End Set
        End Property

        Public Property ChildGroupFirstItemCssClassOver() As String
            Get
                Return _ChildGroupFirstItemCssClassOver
            End Get
            Set(ByVal Value As String)
                _ChildGroupFirstItemCssClassOver = Value
            End Set
        End Property

        Public Property ChildGroupSeparator() As String
            Get
                Return _ChildGroupSeparator
            End Get
            Set(ByVal Value As String)
                _ChildGroupSeparator = Value
            End Set
        End Property

        Public Property ChildGroupSeparatorCssClass() As String
            Get
                Return _ChildGroupSeparatorCssClass
            End Get
            Set(ByVal Value As String)
                _ChildGroupSeparatorCssClass = Value
            End Set
        End Property

        Public Property ChildGroupSeparatorCssClassClicked() As String
            Get
                Return _ChildGroupSeparatorCssClassClicked
            End Get
            Set(ByVal Value As String)
                _ChildGroupSeparatorCssClassClicked = Value
            End Set
        End Property

        Public Property ChildGroupSeparatorCssClassOver() As String
            Get
                Return _ChildGroupSeparatorCssClassOver
            End Get
            Set(ByVal Value As String)
                _ChildGroupSeparatorCssClassOver = Value
            End Set
        End Property

        Public Property ChildGroupLastItem() As String
            Get
                Return _ChildGroupLastItem
            End Get
            Set(ByVal Value As String)
                _ChildGroupLastItem = Value
            End Set
        End Property

        Public Property ChildGroupLastItemCssClass() As String
            Get
                Return _ChildGroupLastItemCssClass
            End Get
            Set(ByVal Value As String)
                _ChildGroupLastItemCssClass = Value
            End Set
        End Property

        Public Property ChildGroupLastItemCssClassClicked() As String
            Get
                Return _ChildGroupLastItemCssClassClicked
            End Get
            Set(ByVal Value As String)
                _ChildGroupLastItemCssClassClicked = Value
            End Set
        End Property

        Public Property ChildGroupLastItemCssClassOver() As String
            Get
                Return _ChildGroupLastItemCssClassOver
            End Get
            Set(ByVal Value As String)
                _ChildGroupLastItemCssClassOver = Value
            End Set
        End Property

        Public Property CollapseAnimationType() As AnimationType
            Get
                Return RadMenu1.CollapseAnimation.Type
            End Get
            Set(ByVal Value As AnimationType)
                RadMenu1.CollapseAnimation.Type = Value
            End Set
        End Property

        Public Property CollapseAnimationDuration() As Integer
            Get
                Return RadMenu1.CollapseAnimation.Duration
            End Get
            Set(ByVal Value As Integer)
                RadMenu1.CollapseAnimation.Duration = Value
            End Set
        End Property

        Public Property CollapseDelay() As Integer
            Get
                Return RadMenu1.CollapseDelay
            End Get
            Set(ByVal Value As Integer)
                RadMenu1.CollapseDelay = Value
            End Set
        End Property

        Public Property ClickToOpen() As Boolean
            Get
                Return RadMenu1.ClickToOpen
            End Get
            Set(ByVal Value As Boolean)
                RadMenu1.ClickToOpen = Value
            End Set
        End Property

        Public Property CopyChildItemLink() As Boolean
            Get
                Return _CopyChildItemLink
            End Get
            Set(ByVal Value As Boolean)
                _CopyChildItemLink = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return RadMenu1.CssClass
            End Get
            Set(ByVal Value As String)
                RadMenu1.CssClass = Value
            End Set
        End Property

        Public Property Dir() As String
            Get
                Return RadMenu1.Attributes("dir")
            End Get
            Set(ByVal Value As String)
                RadMenu1.Attributes("dir") = Value
            End Set
        End Property

        Public Property EnableAdminMenus() As Boolean
            Get
                Return _EnableAdminMenus
            End Get
            Set(ByVal Value As Boolean)
                _EnableAdminMenus = Value
            End Set
        End Property

        Public Property EnableAutoScroll() As Boolean
            Get
                Return RadMenu1.EnableAutoScroll
            End Get
            Set(ByVal Value As Boolean)
                RadMenu1.EnableAutoScroll = Value
            End Set
        End Property

        Public Property EnableEmbeddedBaseStylesheet() As Boolean
            Get
                Return RadMenu1.EnableEmbeddedBaseStylesheet
            End Get
            Set(ByVal Value As Boolean)
                RadMenu1.EnableEmbeddedBaseStylesheet = Value
            End Set
        End Property

        Public Property EnableEmbeddedScripts() As Boolean
            Get
                Return RadMenu1.EnableEmbeddedScripts
            End Get
            Set(ByVal Value As Boolean)
                RadMenu1.EnableEmbeddedScripts = Value
            End Set
        End Property

        Public Property EnableEmbeddedSkins() As Boolean
            Get
                Return RadMenu1.EnableEmbeddedSkins
            End Get
            Set(ByVal Value As Boolean)
                RadMenu1.EnableEmbeddedSkins = Value
            End Set
        End Property

        Public Property EnableItemCss() As Boolean
            Get
                Return _EnableItemCss
            End Get
            Set(ByVal Value As Boolean)
                _EnableItemCss = Value
            End Set
        End Property

        Public Property EnableLevelCss() As Boolean
            Get
                Return _EnableLevelCss
            End Get
            Set(ByVal Value As Boolean)
                _EnableLevelCss = Value
            End Set
        End Property

        Public Property EnablePageIcons() As Boolean
            Get
                Return _EnablePageIcons
            End Get
            Set(ByVal Value As Boolean)
                _EnablePageIcons = Value
            End Set
        End Property

        Public Property EnablePageIdCssClass() As Boolean
            Get
                Return _EnablePageIdCssClass
            End Get
            Set(ByVal Value As Boolean)
                _EnablePageIdCssClass = Value
            End Set
        End Property

        Public Property EnableRootItemCss() As Boolean
            Get
                Return _EnableRootItemCss
            End Get
            Set(ByVal Value As Boolean)
                _EnableRootItemCss = Value
            End Set
        End Property

        Public Property EnableScreenBoundaryDetection() As Boolean
            Get
                Return RadMenu1.EnableScreenBoundaryDetection
            End Get
            Set(ByVal Value As Boolean)
                RadMenu1.EnableScreenBoundaryDetection = Value
            End Set
        End Property

        Public Property EnableToolTips() As Boolean
            Get
                Return _EnableToolTips
            End Get
            Set(ByVal Value As Boolean)
                _EnableToolTips = Value
            End Set
        End Property

        Public Property EnableUserMenus() As Boolean
            Get
                Return _EnableUserMenus
            End Get
            Set(ByVal Value As Boolean)
                _EnableUserMenus = Value
            End Set
        End Property

        Public Property ExpandDelay() As Integer
            Get
                Return RadMenu1.ExpandDelay
            End Get
            Set(ByVal Value As Integer)
                RadMenu1.ExpandDelay = Value
            End Set
        End Property

        Public Property Flow() As ItemFlow
            Get
                Return RadMenu1.Flow
            End Get
            Set(ByVal Value As ItemFlow)
                RadMenu1.Flow = Value
            End Set
        End Property

        Public Property GroupExpandDirection() As ExpandDirection
            Get
                Return _GroupExpandDirection
            End Get
            Set(ByVal Value As ExpandDirection)
                _GroupExpandDirection = Value
            End Set
        End Property

        Public Property GroupFlow() As ItemFlow
            Get
                Return _GroupFlow
            End Get
            Set(ByVal Value As ItemFlow)
                _GroupFlow = Value
            End Set
        End Property

        Public Property GroupHeight() As System.Web.UI.WebControls.Unit
            Get
                Return _GroupHeight
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _GroupHeight = Value
            End Set
        End Property

        Public Property GroupOffsetX() As Integer
            Get
                Return _GroupOffsetX
            End Get
            Set(ByVal Value As Integer)
                _GroupOffsetX = Value
            End Set
        End Property

        Public Property GroupOffsetY() As Integer
            Get
                Return _GroupOffsetY
            End Get
            Set(ByVal Value As Integer)
                _GroupOffsetY = Value
            End Set
        End Property

        Public Property GroupWidth() As System.Web.UI.WebControls.Unit
            Get
                Return _GroupWidth
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _GroupWidth = Value
            End Set
        End Property

        Public Property Height() As System.Web.UI.WebControls.Unit
            Get
                Return RadMenu1.Height
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                RadMenu1.Height = Value
            End Set
        End Property

        Public Property ImagesOnlyMenu() As Boolean
            Get
                Return _ImagesOnlyMenu
            End Get
            Set(ByVal Value As Boolean)
                _ImagesOnlyMenu = Value
            End Set
        End Property

        Public Property ItemBackColor() As System.Drawing.Color
            Get
                Return _ItemBackColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                _ItemBackColor = Value
            End Set
        End Property

        Public Property ItemBorderColor() As System.Drawing.Color
            Get
                Return _ItemBorderColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                _ItemBorderColor = Value
            End Set
        End Property

        Public Property ItemBorderStyle() As System.Web.UI.WebControls.BorderStyle
            Get
                Return _ItemBorderStyle
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.BorderStyle)
                _ItemBorderStyle = Value
            End Set
        End Property

        Public Property ItemBorderWidth() As System.Web.UI.WebControls.Unit
            Get
                Return _ItemBorderWidth
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _ItemBorderWidth = Value
            End Set
        End Property

        Public Property ItemClickedCssClass() As String
            Get
                Return _ItemClickedCssClass
            End Get
            Set(ByVal Value As String)
                _ItemClickedCssClass = Value
            End Set
        End Property

        Public Property ItemCssClass() As String
            Get
                Return _ItemCssClass
            End Get
            Set(ByVal Value As String)
                _ItemCssClass = Value
            End Set
        End Property

        Public Property ItemDisabledCssClass() As String
            Get
                Return _ItemDisabledCssClass
            End Get
            Set(ByVal Value As String)
                _ItemDisabledCssClass = Value
            End Set
        End Property

        Public Property ItemExpandedCssClass() As String
            Get
                Return _ItemExpandedCssClass
            End Get
            Set(ByVal Value As String)
                _ItemExpandedCssClass = Value
            End Set
        End Property

        Public Property ItemFocusedCssClass() As String
            Get
                Return _ItemFocusedCssClass
            End Get
            Set(ByVal Value As String)
                _ItemFocusedCssClass = Value
            End Set
        End Property

        Public Property ItemForeColor() As System.Drawing.Color
            Get
                Return _ItemForeColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                _ItemForeColor = Value
            End Set
        End Property

        Public Property ItemHeight() As System.Web.UI.WebControls.Unit
            Get
                Return _ItemHeight
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _ItemHeight = Value
            End Set
        End Property

        Public Property ItemHoveredImageUrl() As String
            Get
                Return _ItemHoveredImageUrl
            End Get
            Set(ByVal Value As String)
                _ItemHoveredImageUrl = Value
            End Set
        End Property

        Public Property ItemImageUrl() As String
            Get
                Return _ItemImageUrl
            End Get
            Set(ByVal Value As String)
                _ItemImageUrl = Value
            End Set
        End Property

        Public Property ItemTarget() As String
            Get
                Return _ItemTarget
            End Get
            Set(ByVal Value As String)
                _ItemTarget = Value
            End Set
        End Property

        Public Property ItemWidth() As System.Web.UI.WebControls.Unit
            Get
                Return _ItemWidth
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _ItemWidth = Value
            End Set
        End Property

        Public Property HeaderFirstItem() As String
            Get
                Return _HeaderFirstItem
            End Get
            Set(ByVal Value As String)
                _HeaderFirstItem = Value
            End Set
        End Property

        Public Property HeaderFirstItemCssClass() As String
            Get
                Return _HeaderFirstItemCssClass
            End Get
            Set(ByVal Value As String)
                _HeaderFirstItemCssClass = Value
            End Set
        End Property

        Public Property HeaderFirstItemCssClassClicked() As String
            Get
                Return _HeaderFirstItemCssClassClicked
            End Get
            Set(ByVal Value As String)
                _HeaderFirstItemCssClassClicked = Value
            End Set
        End Property

        Public Property HeaderFirstItemCssClassOver() As String
            Get
                Return _HeaderFirstItemCssClassOver
            End Get
            Set(ByVal Value As String)
                _HeaderFirstItemCssClassOver = Value
            End Set
        End Property

        Public Property HeaderLastItem() As String
            Get
                Return _HeaderLastItem
            End Get
            Set(ByVal Value As String)
                _HeaderLastItem = Value
            End Set
        End Property

        Public Property HeaderLastItemCssClass() As String
            Get
                Return _HeaderLastItemCssClass
            End Get
            Set(ByVal Value As String)
                _HeaderLastItemCssClass = Value
            End Set
        End Property

        Public Property HeaderLastItemCssClassClicked() As String
            Get
                Return _HeaderLastItemCssClassClicked
            End Get
            Set(ByVal Value As String)
                _HeaderLastItemCssClassClicked = Value
            End Set
        End Property

        Public Property HeaderLastItemCssClassOver() As String
            Get
                Return _HeaderLastItemCssClassOver
            End Get
            Set(ByVal Value As String)
                _HeaderLastItemCssClassOver = Value
            End Set
        End Property

        Public Property HeaderSeparator() As String
            Get
                Return _HeaderSeparator
            End Get
            Set(ByVal Value As String)
                _HeaderSeparator = Value
            End Set
        End Property

        Public Property HeaderSeparatorCssClass() As String
            Get
                Return _HeaderSeparatorCssClass
            End Get
            Set(ByVal Value As String)
                _HeaderSeparatorCssClass = Value
            End Set
        End Property

        Public Property HeaderSeparatorCssClassClicked() As String
            Get
                Return _HeaderSeparatorCssClassClicked
            End Get
            Set(ByVal Value As String)
                _HeaderSeparatorCssClassClicked = Value
            End Set
        End Property

        Public Property HeaderSeparatorCssClassOver() As String
            Get
                Return _HeaderSeparatorCssClassOver
            End Get
            Set(ByVal Value As String)
                _HeaderSeparatorCssClassOver = Value
            End Set
        End Property

        Public Property MaxItemNumber() As Integer
            Get
                Return _MaxItemNumber
            End Get
            Set(ByVal Value As Integer)
                _MaxItemNumber = Value
            End Set
        End Property

        Public Property MaxLevel() As Integer
            Get
                Return _MaxLevel
            End Get
            Set(ByVal Value As Integer)
                _MaxLevel = Value
            End Set
        End Property

        Public Property MaxLevelNumber() As Integer
            Get
                Return _MaxLevelNumber
            End Get
            Set(ByVal Value As Integer)
                _MaxLevelNumber = Value
            End Set
        End Property

        Public Property OnClientMouseOver() As String
            Get
                Return RadMenu1.OnClientMouseOver
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientMouseOver = Value
            End Set
        End Property

        Public Property OnClientMouseOut() As String
            Get
                Return RadMenu1.OnClientMouseOut
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientMouseOut = Value
            End Set
        End Property

        Public Property OnClientItemFocus() As String
            Get
                Return RadMenu1.OnClientItemFocus
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemFocus = Value
            End Set
        End Property

        Public Property OnClientItemBlur() As String
            Get
                Return RadMenu1.OnClientItemBlur
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemBlur = Value
            End Set
        End Property

        Public Property OnClientItemClicking() As String
            Get
                Return RadMenu1.OnClientItemClicking
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemClicking = Value
            End Set
        End Property

        Public Property OnClientItemClicked() As String
            Get
                Return RadMenu1.OnClientItemClicked
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemClicked = Value
            End Set
        End Property

        Public Property OnClientItemOpened() As String
            Get
                Return RadMenu1.OnClientItemOpened
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemOpened = Value
            End Set
        End Property

        Public Property OnClientItemOpening() As String
            Get
                Return RadMenu1.OnClientItemOpening
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemOpening = Value
            End Set
        End Property

        Public Property OnClientItemClosed() As String
            Get
                Return RadMenu1.OnClientItemClosed
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemClosed = Value
            End Set
        End Property

        Public Property OnClientItemClosing() As String
            Get
                Return RadMenu1.OnClientItemClosing
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientItemClosing = Value
            End Set
        End Property

        Public Property OnClientLoad() As String
            Get
                Return RadMenu1.OnClientLoad
            End Get
            Set(ByVal Value As String)
                RadMenu1.OnClientLoad = Value
            End Set
        End Property

        Public Property PagesToExclude() As String
            Get
                Return _PagesToExclude
            End Get
            Set(ByVal Value As String)
                _PagesToExclude = Value
            End Set
        End Property

        Public Property RootItemCssClass() As String
            Get
                Return _RootItemCssClass
            End Get
            Set(ByVal Value As String)
                _RootItemCssClass = Value
            End Set
        End Property

        Public Property RootItemDisabledCssClass() As String
            Get
                Return _RootItemDisabledCssClass
            End Get
            Set(ByVal Value As String)
                _RootItemDisabledCssClass = Value
            End Set
        End Property

        Public Property RootItemExpandedCssClass() As String
            Get
                Return _RootItemExpandedCssClass
            End Get
            Set(ByVal Value As String)
                _RootItemExpandedCssClass = Value
            End Set
        End Property

        Public Property RootItemFocusedCssClass() As String
            Get
                Return _RootItemFocusedCssClass
            End Get
            Set(ByVal Value As String)
                _RootItemFocusedCssClass = Value
            End Set
        End Property

        Public Property RootItemClickedCssClass() As String
            Get
                Return _RootItemClickedCssClass
            End Get
            Set(ByVal Value As String)
                _RootItemClickedCssClass = Value
            End Set
        End Property

        Public Property RootItemImageUrl() As String
            Get
                Return _RootItemImageUrl
            End Get
            Set(ByVal Value As String)
                _RootItemImageUrl = Value
            End Set
        End Property

        Public Property RootItemHoveredImageUrl() As String
            Get
                Return _RootItemHoveredImageUrl
            End Get
            Set(ByVal Value As String)
                _RootItemHoveredImageUrl = Value
            End Set
        End Property

        Public Property RootItemTarget() As String
            Get
                Return _RootItemTarget
            End Get
            Set(ByVal Value As String)
                _RootItemTarget = Value
            End Set
        End Property

        Public Property RootItemBackColor() As System.Drawing.Color
            Get
                Return _RootItemBackColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                _RootItemBackColor = Value
            End Set
        End Property

        Public Property RootItemBorderColor() As System.Drawing.Color
            Get
                Return _RootItemBorderColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                _RootItemBorderColor = Value
            End Set
        End Property

        Public Property RootItemBorderWidth() As System.Web.UI.WebControls.Unit
            Get
                Return _RootItemBorderWidth
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _RootItemBorderWidth = Value
            End Set
        End Property

        Public Property RootItemBorderStyle() As System.Web.UI.WebControls.BorderStyle
            Get
                Return _RootItemBorderStyle
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.BorderStyle)
                _RootItemBorderStyle = Value
            End Set
        End Property

        Public Property RootItemForeColor() As System.Drawing.Color
            Get
                Return _RootItemForeColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                _RootItemForeColor = Value
            End Set
        End Property

        Public Property RootItemHeight() As System.Web.UI.WebControls.Unit
            Get
                Return _RootItemHeight
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _RootItemHeight = Value
            End Set
        End Property

        Public Property RootItemWidth() As System.Web.UI.WebControls.Unit
            Get
                Return _RootItemWidth
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _RootItemWidth = Value
            End Set
        End Property

        Public Property SelectedPathHeaderItemCss() As String
            Get
                Return _SelectedPathHeaderItemCss
            End Get
            Set(ByVal Value As String)
                _SelectedPathHeaderItemCss = Value
            End Set
        End Property

        Public Property SelectedPathItemCss() As String
            Get
                Return _SelectedPathItemCss
            End Get
            Set(ByVal Value As String)
                _SelectedPathItemCss = Value
            End Set
        End Property

        Public Property SelectedPathItemImage() As String
            Get
                Return _SelectedPathItemImage
            End Get
            Set(ByVal Value As String)
                _SelectedPathItemImage = Value
            End Set
        End Property

        Public Property SelectedPathHeaderItemImage() As String
            Get
                Return _SelectedPathHeaderItemImage
            End Get
            Set(ByVal Value As String)
                _SelectedPathHeaderItemImage = Value
            End Set
        End Property

        Public Property ShowOnlyCurrent() As String
            Get
                Return _ShowOnlyCurrent
            End Get
            Set(ByVal Value As String)
                _ShowOnlyCurrent = Value
            End Set
        End Property

        Public Property ShowPath() As Boolean
            Get
                Return _ShowPath
            End Get
            Set(ByVal Value As Boolean)
                _ShowPath = Value
            End Set
        End Property

        Public Property Skin() As String
            Get
                Return RadMenu1.Skin
            End Get
            Set(ByVal Value As String)
                RadMenu1.Skin = Value
            End Set
        End Property

        Public Property Style() As String
            Get
                Return _Style
            End Get
            Set(ByVal value As String)
                _Style = value
            End Set
        End Property

        Public Property Width() As System.Web.UI.WebControls.Unit
            Get
                Return RadMenu1.Width
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                RadMenu1.Width = Value
            End Set
        End Property

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim objTabController As New TabController
            Dim i, iItemIndex, iRootGroupId As Integer
            Dim temp As qElement
            Dim StartingItemId As Integer

            AuthPages = New ArrayList
            PagesQueue = New Queue
            arrayShowPath = New ArrayList
            iItemIndex = 0
            '---------------------------------------------------

            SetMenuProperties()

            'optional code to support displaying a specific branch of the page tree
            GetShowOnlyCurrent(objTabController, StartingItemId, iRootGroupId)
            'Fixed: For i = 0 To Me.PortalSettings.DesktopTabs.Count - 1
            'Dim portalID As Integer = CType(IIf(PortalSettings.ActiveTab.IsSuperTab, -1, PortalSettings.PortalId), Integer)
            Dim portalID As Integer = PortalSettings.PortalId
            Dim desktopTabs As New List(Of TabInfo)
            desktopTabs.AddRange(TabController.GetTabsBySortOrder(portalID))
            If UserController.GetCurrentUserInfo.IsSuperUser Then
                desktopTabs.AddRange(TabController.GetTabsBySortOrder(Null.NullInteger))
            End If
            For i = 0 To desktopTabs.Count - 1
                Dim currentTab As TabInfo = LocalizeTab.getLocalizedTab(CType(desktopTabs(i), TabInfo))
                If (currentTab.TabID = PortalSettings.ActiveTab.TabID) Then
                    FillShowPathArray(arrayShowPath, currentTab.TabID, objTabController)
                End If
                If IsTabVisible(currentTab) Then
                    temp = New qElement
                    temp.page = currentTab
                    temp.radMenuItem = New RadMenuItem
                    If CheckShowOnlyCurrent(currentTab.TabID, currentTab.ParentId, StartingItemId, iRootGroupId) AndAlso _
                       CheckMenuVisibility(currentTab) Then
                        If (iItemIndex > 0 And HeaderSeparator <> String.Empty) Then
                            RadMenu1.Items.Add(CreateHeaderSeparatorItem())
                        End If
                        iItemIndex = iItemIndex + 1
                        temp.item = iItemIndex
                        PagesQueue.Enqueue(AuthPages.Count)
                        RadMenu1.Items.Add(temp.radMenuItem)
                    End If
                    AuthPages.Add(temp)
                End If
            Next i
            'insert first item if enabled
            If (HeaderFirstItem <> String.Empty) Then
                RadMenu1.Items.Insert(0, CreateHeaderFirstItem())
            End If
            'insert last item if enabled
            If (HeaderLastItem <> String.Empty) Then
                RadMenu1.Items.Add(CreateHeaderLastItem())
            End If
            BuildMenu(RadMenu1.Items)
            If (0 = RadMenu1.Items.Count) Then
                RadMenu1.Visible = False
            End If
        End Sub

#End Region

#Region "Private Helper Functions"

        Private Function IsTabVisible(ByVal tab As TabInfo) As Boolean
            If (tab.IsVisible = True And tab.IsDeleted = False) And _
            ((tab.StartDate < Now And tab.EndDate > Now) Or AdminMode) And _
            (Permissions.TabPermissionController.CanViewPage(tab) And Not CheckToExclude(tab.TabName, tab.TabID)) Then
                '(PortalSecurity.IsInRoles(tab.AuthorizedRoles) And Not CheckToExclude(tab.TabName, tab.TabID)) Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function CheckToExclude(ByVal tabName As String, ByVal tabId As Int32) As Boolean
            If (PagesToExclude = String.Empty) Then
                Return False
            End If
            Dim temp As String() = PagesToExclude.Split(New Char() {","c})
            If (temp.Length = 0) Then
                Return False
            End If
            Dim item As String
            For Each item In temp
                Try
                    If tabId = Int32.Parse(item.Trim()) Then
                        Return True
                    End If
                Catch
                    If tabName = item.Trim() Then
                        Return True
                    End If
                End Try
            Next
            Return False
        End Function

        Private Sub GetShowOnlyCurrent(ByVal objTabController As TabController, ByRef StartingItemId As Integer, ByRef iRootGroupId As Integer)
            StartingItemId = 0
            iRootGroupId = 0
            'check if we have a value to work with
            If (ShowOnlyCurrent = String.Empty) Then
                Exit Sub
            End If
            'check if user specified an ID
            If (Char.IsDigit(ShowOnlyCurrent.Chars(0))) Then
                Try
                    StartingItemId = Integer.Parse(ShowOnlyCurrent)
                Catch ex As Exception
                End Try
            End If
            'check if user specified a page name
            If (ShowOnlyCurrent.StartsWith("PageItem:")) Then
                Dim temptab As TabInfo = objTabController.GetTabByName(ShowOnlyCurrent.Substring(("PageItem:").Length), PortalSettings.PortalId)
                If (Not temptab Is Nothing) Then
                    StartingItemId = temptab.TabID
                End If
            End If
            'RootItem
            If ("RootItem" = ShowOnlyCurrent) Then
                iRootGroupId = PortalSettings.ActiveTab.TabID
                While (CType(objTabController.GetTab(iRootGroupId, PortalSettings.PortalId, True), TabInfo).ParentId <> -1)
                    iRootGroupId = CType(objTabController.GetTab(iRootGroupId, PortalSettings.PortalId, True), TabInfo).ParentId
                End While
            End If
        End Sub

        Private Function CheckShowOnlyCurrent(ByVal tabId As Integer, ByVal parentId As Integer, ByVal StartingItemId As Integer, ByVal iRootGroupId As Integer) As Boolean
            If _
             (String.Empty = ShowOnlyCurrent AndAlso parentId = -1) OrElse _
             ("ChildItems" = ShowOnlyCurrent AndAlso parentId = PortalSettings.ActiveTab.TabID) OrElse _
             ("CurrentItem" = ShowOnlyCurrent AndAlso tabId = PortalSettings.ActiveTab.TabID) OrElse _
             ("RootItem" = ShowOnlyCurrent AndAlso iRootGroupId = parentId) OrElse _
             (StartingItemId > 0 AndAlso parentId = StartingItemId) _
            Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function CheckMenuVisibility(ByVal tab As TabInfo) As Boolean
            If (Not EnableAdminMenus AndAlso (tab.TabName = "Admin" Or tab.IsSuperTab)) Then
                Return False
            End If
            If (Not EnableUserMenus AndAlso Not (tab.TabName = "Admin" Or tab.IsSuperTab)) Then
                Return False
            End If
            Return True
        End Function

        Private Sub FillShowPathArray(ByRef arrayShowPath As ArrayList, ByVal selectedTabID As Integer, ByVal objTabController As TabController)
            Dim tempTab As TabInfo = objTabController.GetTab(selectedTabID, PortalSettings.PortalId, True)
            While (tempTab.ParentId <> -1)
                arrayShowPath.Add(tempTab.TabID)
                tempTab = objTabController.GetTab(tempTab.ParentId, PortalSettings.PortalId, True)
            End While
            arrayShowPath.Add(tempTab.TabID)
        End Sub

        Private Sub CheckShowPath(ByVal tabId As Integer, ByVal menuItemToCheck As RadMenuItem, ByVal pageName As String)
            If (arrayShowPath.Contains(tabId)) Then
                If (menuItemToCheck.Level > 0) Then
                    If SelectedPathItemCss <> String.Empty Then
                        menuItemToCheck.CssClass = menuItemToCheck.CssClass & " " & SelectedPathItemCss
                    End If
                    If (SelectedPathItemImage <> String.Empty) Then
                        menuItemToCheck.ImageUrl = SelectedPathItemImage.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName)
                    End If
                Else
                    If (SelectedPathHeaderItemCss <> String.Empty) Then
                        menuItemToCheck.CssClass = menuItemToCheck.CssClass & " " & SelectedPathHeaderItemCss
                    End If
                    If (SelectedPathHeaderItemImage <> String.Empty) Then
                        menuItemToCheck.ImageUrl = SelectedPathHeaderItemImage.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName)
                    End If
                End If
            End If
        End Sub

        Private Function CreateHeaderSeparatorItem() As RadMenuItem
            Dim menuItem As New RadMenuItem
            menuItem.IsSeparator = True
            If (HeaderSeparator <> String.Empty) Then
                menuItem.ImageUrl = HeaderSeparator.Replace("*SkinPath*", dnnSkinPath)
            End If
            If (HeaderSeparatorCssClass <> String.Empty) Then
                menuItem.CssClass = HeaderSeparatorCssClass
            End If
            If (HeaderSeparatorCssClassOver <> String.Empty) Then
                menuItem.FocusedCssClass = HeaderSeparatorCssClassOver
            End If
            If (HeaderSeparatorCssClassClicked <> String.Empty) Then
                menuItem.ClickedCssClass = HeaderSeparatorCssClassClicked
            End If
            Return menuItem
        End Function

        Private Function CreateHeaderFirstItem() As RadMenuItem
            Dim menuItem As New RadMenuItem
            menuItem.IsSeparator = True
            If (HeaderFirstItem <> String.Empty) Then
                menuItem.ImageUrl = HeaderFirstItem.Replace("*SkinPath*", dnnSkinPath)
            End If
            If (HeaderFirstItemCssClass <> String.Empty) Then
                menuItem.CssClass = HeaderFirstItemCssClass
            End If
            If (HeaderFirstItemCssClassOver <> String.Empty) Then
                menuItem.FocusedCssClass = HeaderFirstItemCssClassOver
            End If
            If (HeaderFirstItemCssClassClicked <> String.Empty) Then
                menuItem.ClickedCssClass = HeaderFirstItemCssClassClicked
            End If
            Return menuItem
        End Function

        Private Function CreateHeaderLastItem() As RadMenuItem
            Dim menuItem As New RadMenuItem
            menuItem.IsSeparator = True
            If (HeaderLastItem <> String.Empty) Then
                menuItem.ImageUrl = HeaderLastItem.Replace("*SkinPath*", dnnSkinPath)
            End If
            If (HeaderLastItemCssClass <> String.Empty) Then
                menuItem.CssClass = HeaderLastItemCssClass
            End If
            If (HeaderLastItemCssClassOver <> String.Empty) Then
                menuItem.FocusedCssClass = HeaderLastItemCssClassOver
            End If
            If (HeaderLastItemCssClassClicked <> String.Empty) Then
                menuItem.ClickedCssClass = HeaderLastItemCssClassClicked
            End If
            Return menuItem
        End Function

        Private Function CreateSeparatorItem() As RadMenuItem
            Dim menuItem As New RadMenuItem
            menuItem.IsSeparator = True
            If (ChildGroupSeparator <> String.Empty) Then
                menuItem.ImageUrl = ChildGroupSeparator.Replace("*SkinPath*", dnnSkinPath)
            End If
            If (ChildGroupSeparatorCssClass <> String.Empty) Then
                menuItem.CssClass = ChildGroupSeparatorCssClass
            End If
            If (ChildGroupSeparatorCssClassOver <> String.Empty) Then
                menuItem.FocusedCssClass = ChildGroupSeparatorCssClassOver
            End If
            If (ChildGroupSeparatorCssClassClicked <> String.Empty) Then
                menuItem.ClickedCssClass = ChildGroupSeparatorCssClassClicked
            End If
            Return menuItem
        End Function

        Private Function CreateFirstItem() As RadMenuItem
            Dim menuItem As New RadMenuItem
            menuItem.IsSeparator = True
            If (ChildGroupFirstItem <> String.Empty) Then
                menuItem.ImageUrl = ChildGroupFirstItem.Replace("*SkinPath*", dnnSkinPath)
            End If
            If (ChildGroupFirstItemCssClass <> String.Empty) Then
                menuItem.CssClass = ChildGroupFirstItemCssClass
            End If
            If (ChildGroupFirstItemCssClassOver <> String.Empty) Then
                menuItem.FocusedCssClass = ChildGroupFirstItemCssClassOver
            End If
            If (ChildGroupFirstItemCssClassClicked <> String.Empty) Then
                menuItem.ClickedCssClass = ChildGroupFirstItemCssClassClicked
            End If
            Return menuItem
        End Function

        Private Function CreateLastItem() As RadMenuItem
            Dim menuItem As New RadMenuItem
            menuItem.IsSeparator = True
            If (ChildGroupLastItem <> String.Empty) Then
                menuItem.ImageUrl = ChildGroupLastItem.Replace("*SkinPath*", dnnSkinPath)
            End If
            If (ChildGroupLastItemCssClass <> String.Empty) Then
                menuItem.CssClass = ChildGroupLastItemCssClass
            End If
            If (ChildGroupLastItemCssClassOver <> String.Empty) Then
                menuItem.FocusedCssClass = ChildGroupLastItemCssClassOver
            End If
            If (ChildGroupLastItemCssClassClicked <> String.Empty) Then
                menuItem.ClickedCssClass = ChildGroupLastItemCssClassClicked
            End If
            Return menuItem
        End Function

        Private Sub SetMenuProperties()
            If (Style <> String.Empty) Then
                Style += "; "
                Try
                    For Each cStyle As String In Style.Split(";"c)
                        If (cStyle.Trim.Length > 0) Then
                            RadMenu1.Style.Add(cStyle.Split(":"c)(0), cStyle.Split(":"c)(1))
                        End If
                    Next
                Catch
                End Try
            End If
        End Sub

        Private Sub SetGroupProperties(ByVal groupSettings As RadMenuItemGroupSettings)
            groupSettings.Flow = GroupFlow
            groupSettings.ExpandDirection = GroupExpandDirection
            groupSettings.OffsetX = GroupOffsetX
            groupSettings.OffsetY = GroupOffsetY

            If (Not GroupWidth.IsEmpty) Then
                groupSettings.Width = GroupWidth
            End If

            If (Not GroupHeight.IsEmpty) Then
                groupSettings.Height = GroupHeight
            End If
        End Sub

        Private Sub SetItemProperties(ByVal currentMenuItem As RadMenuItem, ByVal iLevel As Integer, ByVal iItem As Integer, ByVal pageName As String, ByVal pageId As Integer)
            Dim sLevel As String = CType(IIf(EnableLevelCss And iLevel < MaxLevelNumber, "Level" + iLevel.ToString, String.Empty), String)
            Dim sItem As String = CType(IIf(iItem <= MaxItemNumber And ((EnableItemCss And iLevel > 0) Or (EnableRootItemCss And iLevel = 0)), iItem.ToString, String.Empty), String)
            Dim pageIdCssClass As String = "rmItem" & pageId

            If (ItemCssClass <> String.Empty) Then
                currentMenuItem.CssClass = sLevel & ItemCssClass & sItem
            End If

            If (EnablePageIdCssClass) Then
                currentMenuItem.CssClass &= " " & pageIdCssClass
            End If

            If (ItemDisabledCssClass <> String.Empty) Then
                currentMenuItem.DisabledCssClass = sLevel & ItemDisabledCssClass & sItem
            End If

            If (ItemExpandedCssClass <> String.Empty) Then
                currentMenuItem.ExpandedCssClass = sLevel & ItemExpandedCssClass & sItem
            End If

            If (ItemFocusedCssClass <> String.Empty) Then
                currentMenuItem.FocusedCssClass = sLevel & ItemFocusedCssClass & sItem
            End If

            If (ItemClickedCssClass <> String.Empty) Then
                currentMenuItem.ClickedCssClass = sLevel & ItemClickedCssClass & sItem
            End If

            If (ItemImageUrl <> String.Empty) Then
                currentMenuItem.ImageUrl = ItemImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString)
            End If

            If (ItemHoveredImageUrl <> String.Empty) Then
                currentMenuItem.HoveredImageUrl = ItemHoveredImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString)
            End If

            If (ItemTarget <> String.Empty) Then
                currentMenuItem.Target = ItemTarget
            End If

            If (Not ItemBackColor.Equals(System.Drawing.Color.Empty)) Then
                currentMenuItem.BackColor = ItemBackColor
            End If

            If (Not ItemBorderColor.Equals(System.Drawing.Color.Empty)) Then
                currentMenuItem.BorderColor = ItemBorderColor
            End If

            If (Not ItemBorderWidth.IsEmpty) Then
                currentMenuItem.BorderWidth = ItemBorderWidth
            End If

            If (ItemBorderStyle <> System.Web.UI.WebControls.BorderStyle.None) Then
                currentMenuItem.BorderStyle = ItemBorderStyle
            End If


            If (Not ItemForeColor.Equals(System.Drawing.Color.Empty)) Then
                currentMenuItem.ForeColor = ItemForeColor
            End If

            If (Not ItemHeight.IsEmpty) Then
                currentMenuItem.Height = ItemHeight
            End If

            If (Not ItemWidth.IsEmpty) Then
                currentMenuItem.Width = ItemWidth
            End If
        End Sub

        Private Sub SetRootItemProperties(ByVal currentMenuItem As RadMenuItem, ByVal iLevel As Integer, ByVal iItem As Integer, ByVal pageName As String, ByVal pageId As Integer)
            Dim sLevel As String = CType(IIf(EnableLevelCss And iLevel < MaxLevelNumber, "Level" + iLevel.ToString, String.Empty), String)
            Dim sItem As String = CType(IIf(iItem <= MaxItemNumber And ((EnableItemCss And iLevel > 0) Or (EnableRootItemCss And iLevel = 0)), iItem.ToString, String.Empty), String)
            Dim pageIdCssClass As String = "rmPageId" & pageId

            If (RootItemCssClass <> String.Empty) Then
                currentMenuItem.CssClass = sLevel & RootItemCssClass & sItem
            End If

            If (EnablePageIdCssClass) Then
                currentMenuItem.CssClass &= " " & pageIdCssClass
            End If

            If (RootItemDisabledCssClass <> String.Empty) Then
                currentMenuItem.DisabledCssClass = sLevel & RootItemDisabledCssClass & sItem
            End If

            If (RootItemExpandedCssClass <> String.Empty) Then
                currentMenuItem.ExpandedCssClass = sLevel & RootItemExpandedCssClass & sItem
            End If

            If (RootItemFocusedCssClass <> String.Empty) Then
                currentMenuItem.FocusedCssClass = sLevel & RootItemFocusedCssClass & sItem
            End If

            If (RootItemClickedCssClass <> String.Empty) Then
                currentMenuItem.ClickedCssClass = sLevel & RootItemClickedCssClass & sItem
            End If

            If (RootItemImageUrl <> String.Empty) Then
                currentMenuItem.ImageUrl = RootItemImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString)
            End If

            If (RootItemHoveredImageUrl <> String.Empty) Then
                currentMenuItem.HoveredImageUrl = RootItemHoveredImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", pageName).Replace("*PageID*", pageId.ToString)
            End If

            If (RootItemTarget <> String.Empty) Then
                currentMenuItem.Target = RootItemTarget
            End If

            If (Not RootItemBackColor.Equals(System.Drawing.Color.Empty)) Then
                currentMenuItem.BackColor = RootItemBackColor
            End If

            If (Not RootItemBorderColor.Equals(System.Drawing.Color.Empty)) Then
                currentMenuItem.BorderColor = RootItemBorderColor
            End If

            If (Not RootItemBorderWidth.IsEmpty) Then
                currentMenuItem.BorderWidth = RootItemBorderWidth
            End If

            If (RootItemBorderStyle <> System.Web.UI.WebControls.BorderStyle.None) Then
                currentMenuItem.BorderStyle = RootItemBorderStyle
            End If

            If (Not RootItemForeColor.Equals(System.Drawing.Color.Empty)) Then
                currentMenuItem.ForeColor = RootItemForeColor
            End If

            If (Not RootItemHeight.IsEmpty) Then
                currentMenuItem.Height = RootItemHeight
            End If


            If (Not RootItemWidth.IsEmpty) Then
                currentMenuItem.Width = RootItemWidth
            End If

        End Sub

        Private Sub BuildMenu(ByVal rootCollection As RadMenuItemCollection)
            Dim temp, temp2 As qElement
            Dim page As TabInfo
            Dim pageID, j, iItemIndex As Integer

            While Not (PagesQueue.Count = 0)
                pageID = CType(PagesQueue.Dequeue(), Integer)
                temp = CType(AuthPages(pageID), qElement)
                page = CType(temp.page, TabInfo)

                temp.radMenuItem.Text = page.TabName

                If (page.IconFile <> "" And EnablePageIcons) Then
                    'If (DotNetNuke.Common.glbAppVersion.StartsWith("05")) Then
                    If (page.IconFile.StartsWith("~")) Then
                        temp.radMenuItem.ImageUrl = Me.Page.ResolveUrl(page.IconFile)
                    Else
                        temp.radMenuItem.ImageUrl = PortalSettings.HomeDirectory & page.IconFile
                    End If
                    'Else
                    '    If (page.IsAdminTab Or page.IsSuperTab) Then
                    '        temp.radMenuItem.ImageUrl = CType(IIf(Me.Request.ApplicationPath <> "/", Me.Request.ApplicationPath.ToUpper, String.Empty), String) & "/images/" & page.IconFile
                    '    Else
                    '        temp.radMenuItem.ImageUrl = PortalSettings.HomeDirectory & page.IconFile
                    '    End If
                    'End If
                End If

                If (Not page.DisableLink) Then
                    If (page.FullUrl.StartsWith("*") And page.FullUrl.IndexOf("*", 1) <> -1) Then
                        temp.radMenuItem.NavigateUrl = page.FullUrl.Substring(page.FullUrl.IndexOf("*", 1) + 1)
                        temp.radMenuItem.Target = page.FullUrl.Substring(1, page.FullUrl.IndexOf("*", 1) - 1)
                    Else
                        temp.radMenuItem.NavigateUrl = page.FullUrl
                    End If
                ElseIf (CopyChildItemLink AndAlso page.Level >= MaxLevel) Then
                    j = 0
                    'check if there are any child items and use a href from the first one
                    While (j < AuthPages.Count AndAlso _
                      (CType(AuthPages(j), qElement).page.ParentId <> page.TabID OrElse _
                      CType(AuthPages(j), qElement).page.DisableLink))
                        j = j + 1
                    End While
                    If (j < AuthPages.Count) Then
                        ' child item found. use its link
                        temp.radMenuItem.NavigateUrl = CType(AuthPages(j), qElement).page.FullUrl
                    End If
                End If
                If (EnableToolTips And page.Description <> "") Then
                    temp.radMenuItem.ToolTip = page.Description
                End If

                'set all other item properties
                If (temp.radMenuItem.Level = 0) Then
                    SetRootItemProperties(temp.radMenuItem, page.Level, temp.item, page.TabName, page.TabID)
                Else
                    SetItemProperties(temp.radMenuItem, page.Level, temp.item, page.TabName, page.TabID)
                End If

                'check showpath
                If (ShowPath) Then
                    CheckShowPath(page.TabID, temp.radMenuItem, page.TabName)
                End If


                'image-only menu check
                If (ImagesOnlyMenu And temp.radMenuItem.ImageUrl <> String.Empty) Then
                    temp.radMenuItem.Text = String.Empty
                End If

                'attach child items the current one
                If (page.Level < MaxLevel Or MaxLevel < 0) Then
                    iItemIndex = 0
                    'set all group properties
                    SetGroupProperties(temp.radMenuItem.GroupSettings)
                    For j = 0 To AuthPages.Count - 1
                        temp2 = CType(AuthPages(j), qElement)
                        If (temp2.page.ParentId = page.TabID) Then
                            ' add a separator item if enabled
                            If (temp.radMenuItem.Items.Count > 0 And ChildGroupSeparator <> String.Empty) Then
                                temp.radMenuItem.Items.Add(CreateSeparatorItem())
                            End If
                            temp.radMenuItem.Items.Add(temp2.radMenuItem)
                            PagesQueue.Enqueue(j)
                            iItemIndex = iItemIndex + 1
                            temp2.item = iItemIndex
                            AuthPages(j) = temp2
                        End If
                    Next j
                    If (temp.radMenuItem.Items.Count > 0) Then
                        'insert first item if enabled
                        If (ChildGroupFirstItem <> String.Empty) Then
                            temp.radMenuItem.Items.Insert(0, CreateFirstItem())
                        End If
                        'insert last item if enabled
                        If (ChildGroupLastItem <> String.Empty) Then
                            temp.radMenuItem.Items.Add(CreateLastItem())
                        End If
                    End If
                End If
            End While
        End Sub

#End Region

    End Class

End Namespace