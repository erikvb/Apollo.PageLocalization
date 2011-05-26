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

Imports Apollo.DNN_Localization
Imports DotNetNuke.Security
Imports DotNetNuke.Entities.Tabs
Imports System.Collections.Generic
Imports Telerik.Web.UI

Namespace Apollo.DNN.SkinObjects

    Partial Public Class MLRadTabstrip
        Inherits DotNetNuke.UI.Skins.SkinObjectBase

#Region "Private Members"

        'variables and structures
        Private PagesQueue As Queue
        Private AuthPages As ArrayList
        Private arrayShowPath As ArrayList
        Private dnnSkinSrc As String = PortalSettings.ActiveTab.SkinSrc.Replace("\"c, "/"c).Replace("//", "/")
        Private dnnSkinPath As String = dnnSkinSrc.Substring(0, dnnSkinSrc.LastIndexOf("/"c))

        Private Structure qElement
            Dim TSTab As RadTab
            Dim page As TabInfo
            Dim item As Integer
        End Structure

        Private _Style As System.String = String.Empty

        'tab specific properties
        Private _TabChildGroupCssClass As String = String.Empty
        Private _TabCssClass As String = String.Empty
        Private _TabDisabledCssClass As String = String.Empty
        Private _TabHeight As System.Web.UI.WebControls.Unit = System.Web.UI.WebControls.Unit.Empty
        Private _TabImageUrl As String = String.Empty
        Private _TabSelectedCssClass As String = String.Empty
        Private _TabWidth As System.Web.UI.WebControls.Unit = System.Web.UI.WebControls.Unit.Empty

        'skinobject properties
        Private _ImagesOnlyTabStrip As Boolean = False
        Private _MaxLevel As Integer = -1
        Private _ItemsPerRow As Integer = -1
        Private _EnablePageIdCssClass As Boolean = False
        Private _EnableToolTips As Boolean = True
        Private _ShowOnlyCurrent As String = String.Empty
        Private _CopyChildItemLink As Boolean = False
        Private _EnableUserMenus As Boolean = True
        Private _EnableAdminMenus As Boolean = True
        Private _PagesToExclude As String

#End Region

#Region "Public Members"

        'tabstrip properties
        Public Property Align() As TabStripAlign
            Get
                Return TS.Align
            End Get
            Set(ByVal Value As TabStripAlign)
                TS.Align = Value
            End Set
        End Property

        Public Property CausesValidation() As Boolean
            Get
                Return TS.CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                TS.CausesValidation = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return TS.CssClass
            End Get
            Set(ByVal Value As String)
                TS.CssClass = Value
            End Set
        End Property

        Public Property Height() As System.Web.UI.WebControls.Unit
            Get
                Return TS.Height
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                TS.Height = Value
            End Set
        End Property

        Public Property MultiPageID() As String
            Get
                Return TS.MultiPageID
            End Get
            Set(ByVal Value As String)
                TS.MultiPageID = Value
            End Set
        End Property

        Public Property OnClientContextMenu() As String
            Get
                Return TS.OnClientContextMenu
            End Get
            Set(ByVal Value As String)
                TS.OnClientContextMenu = Value
            End Set
        End Property

        Public Property OnClientDoubleClick() As String
            Get
                Return TS.OnClientDoubleClick
            End Get
            Set(ByVal Value As String)
                TS.OnClientDoubleClick = Value
            End Set
        End Property

        Public Property OnClientLoad() As String
            Get
                Return TS.OnClientLoad
            End Get
            Set(ByVal Value As String)
                TS.OnClientLoad = Value
            End Set
        End Property

        Public Property OnClientMouseOut() As String
            Get
                Return TS.OnClientMouseOut
            End Get
            Set(ByVal Value As String)
                TS.OnClientMouseOut = Value
            End Set
        End Property

        Public Property OnClientMouseOver() As String
            Get
                Return TS.OnClientMouseOver
            End Get
            Set(ByVal Value As String)
                TS.OnClientMouseOver = Value
            End Set
        End Property

        Public Property OnClientTabSelected() As String
            Get
                Return TS.OnClientTabSelected
            End Get
            Set(ByVal Value As String)
                TS.OnClientTabSelected = Value
            End Set
        End Property

        Public Property OnClientTabSelecting() As String
            Get
                Return TS.OnClientTabSelecting
            End Get
            Set(ByVal Value As String)
                TS.OnClientTabSelecting = Value
            End Set
        End Property

        Public Property OnClientTabUnSelected() As String
            Get
                Return TS.OnClientTabUnSelected
            End Get
            Set(ByVal Value As String)
                TS.OnClientTabUnSelected = Value
            End Set
        End Property

        Public Property Orientation() As TabStripOrientation
            Get
                Return TS.Orientation
            End Get
            Set(ByVal Value As TabStripOrientation)
                TS.Orientation = Value
            End Set
        End Property

        Public Property PerTabScrolling() As Boolean
            Get
                Return TS.PerTabScrolling
            End Get
            Set(ByVal Value As Boolean)
                TS.PerTabScrolling = Value
            End Set
        End Property

        Public Property ReorderTabsOnSelect() As Boolean
            Get
                Return TS.ReorderTabsOnSelect
            End Get
            Set(ByVal Value As Boolean)
                TS.ReorderTabsOnSelect = Value
            End Set
        End Property

        Public Property ScrollButtonsPosition() As TabStripScrollButtonsPosition
            Get
                Return TS.ScrollButtonsPosition
            End Get
            Set(ByVal Value As TabStripScrollButtonsPosition)
                TS.ScrollButtonsPosition = Value
            End Set
        End Property

        Public Property ScrollChildren() As Boolean
            Get
                Return TS.ScrollChildren
            End Get
            Set(ByVal Value As Boolean)
                TS.ScrollChildren = Value
            End Set
        End Property

        Public Property ScrollPosition() As Integer
            Get
                Return TS.ScrollPosition
            End Get
            Set(ByVal Value As Integer)
                TS.ScrollPosition = Value
            End Set
        End Property

        Public Property Skin() As String
            Get
                Return TS.Skin
            End Get
            Set(ByVal Value As String)
                TS.Skin = Value
            End Set
        End Property

        Public Property EnableEmbeddedSkins() As Boolean
            Get
                Return TS.EnableEmbeddedSkins
            End Get
            Set(ByVal Value As Boolean)
                TS.EnableEmbeddedSkins = Value
            End Set
        End Property

        Public Property EnableEmbeddedBaseStylesheet() As Boolean
            Get
                Return TS.EnableEmbeddedBaseStylesheet
            End Get
            Set(ByVal Value As Boolean)
                TS.EnableEmbeddedBaseStylesheet = Value
            End Set
        End Property

        Public Property EnableEmbeddedScripts() As Boolean
            Get
                Return TS.EnableEmbeddedScripts
            End Get
            Set(ByVal Value As Boolean)
                TS.EnableEmbeddedScripts = Value
            End Set
        End Property

        Public Property Dir() As String
            Get
                Return TS.Attributes("dir")
            End Get
            Set(ByVal Value As String)
                TS.Attributes("dir") = Value
            End Set
        End Property

        Public Property Width() As System.Web.UI.WebControls.Unit
            Get
                Return TS.Width
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                TS.Width = Value
            End Set
        End Property

        Public Property ClickSelectedTab() As Boolean
            Get
                Return TS.ClickSelectedTab
            End Get
            Set(ByVal Value As Boolean)
                TS.ClickSelectedTab = Value
            End Set
        End Property

        Public Property UnSelectChildren() As Boolean
            Get
                Return TS.UnSelectChildren
            End Get
            Set(ByVal Value As Boolean)
                TS.UnSelectChildren = Value
            End Set
        End Property

        'tab specific properties
        Public Property TabChildGroupCssClass() As String
            Get
                Return _TabChildGroupCssClass
            End Get
            Set(ByVal Value As String)
                _TabChildGroupCssClass = Value
            End Set
        End Property

        Public Property TabCssClass() As String
            Get
                Return _TabCssClass
            End Get
            Set(ByVal Value As String)
                _TabCssClass = Value
            End Set
        End Property

        Public Property TabDisabledCssClass() As String
            Get
                Return _TabDisabledCssClass
            End Get
            Set(ByVal Value As String)
                _TabDisabledCssClass = Value
            End Set
        End Property

        Public Property TabHeight() As System.Web.UI.WebControls.Unit
            Get
                Return _TabHeight
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _TabHeight = Value
            End Set
        End Property

        Public Property TabImageUrl() As String
            Get
                Return _TabImageUrl
            End Get
            Set(ByVal Value As String)
                _TabImageUrl = Value
            End Set
        End Property

        Public Property TabSelectedCssClass() As String
            Get
                Return _TabSelectedCssClass
            End Get
            Set(ByVal Value As String)
                _TabSelectedCssClass = Value
            End Set
        End Property

        Public Property TabWidth() As System.Web.UI.WebControls.Unit
            Get
                Return _TabWidth
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                _TabWidth = Value
            End Set
        End Property

        'skinobject properties
        Public Property ShowOnlyCurrent() As String
            Get
                Return _ShowOnlyCurrent
            End Get
            Set(ByVal Value As String)
                _ShowOnlyCurrent = Value
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

        Public Property EnableToolTips() As Boolean
            Get
                Return _EnableToolTips
            End Get
            Set(ByVal Value As Boolean)
                _EnableToolTips = Value
            End Set
        End Property

        Public Property ItemsPerRow() As Integer
            Get
                Return _ItemsPerRow
            End Get
            Set(ByVal Value As Integer)
                _ItemsPerRow = Value
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

        Public Property ImagesOnlyTabStrip() As Boolean
            Get
                Return _ImagesOnlyTabStrip
            End Get
            Set(ByVal Value As Boolean)
                _ImagesOnlyTabStrip = Value
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

        Public Property EnableUserMenus() As Boolean
            Get
                Return _EnableUserMenus
            End Get
            Set(ByVal Value As Boolean)
                _EnableUserMenus = Value
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

        Public Property PagesToExclude() As String
            Get
                Return _PagesToExclude
            End Get
            Set(ByVal Value As String)
                _PagesToExclude = Value
            End Set
        End Property

        Public Property Style() As System.String
            Get
                Return _Style
            End Get
            Set(ByVal Value As System.String)
                _Style = Value
            End Set
        End Property

#End Region

#Region "Event Handlers"

        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim i, iItemIndex As Integer
            Dim iRootGroupId, StartingItemId As Integer
            Dim objTabController As New TabController

            AuthPages = New ArrayList
            PagesQueue = New Queue
            arrayShowPath = New ArrayList
            iItemIndex = 0
            '---------------------------------------------------
            SetTabStripProperties()

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
                    Dim tabElement As qElement = New qElement
                    tabElement.page = CType(desktopTabs(i), TabInfo)
                    tabElement.TSTab = New RadTab

                    If (EnablePageIdCssClass) Then
                        tabElement.TSTab.CssClass += " tsPageId" & currentTab.TabID.ToString
                    End If

                    If CheckShowOnlyCurrent(currentTab.TabID, currentTab.ParentId, StartingItemId, iRootGroupId) AndAlso _
                       CheckMenuVisibility(currentTab) Then
                        iItemIndex = iItemIndex + 1
                        tabElement.item = iItemIndex
                        PagesQueue.Enqueue(AuthPages.Count)
                        TS.Tabs.Add(tabElement.TSTab)
                    End If

                    AuthPages.Add(tabElement)
                End If
            Next i
            BuildTabStrip()
            If (TS.Tabs.Count = 0) Then
                TS.Visible = False
            End If
        End Sub

#End Region

#Region "Private Helper Methods"

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

        Private Sub CheckShowPath(ByVal tabId As Integer, ByVal tabName As String, ByRef tab As RadTab)
            If (arrayShowPath.Contains(tabId)) Then
                tab.Selected = True
                tab.SelectParents()
            End If
        End Sub

        Private Sub SetTabStripProperties()
            If (Style <> String.Empty) Then
                Style += "; "
                Try
                    For Each cStyle As String In Style.Split(";"c)
                        If (cStyle.Trim.Length > 0) Then
                            TS.Style.Add(cStyle.Split(":"c)(0), cStyle.Split(":"c)(1))
                        End If
                    Next
                Catch
                End Try
            End If
        End Sub

        Private Sub SetTabProperties(ByRef TSTab As RadTab)
            If (Me.TabChildGroupCssClass.Length > 0) Then
                TSTab.ChildGroupCssClass = Me.TabChildGroupCssClass
            End If
            If (Me.TabCssClass.Length > 0) Then
                TSTab.CssClass += " " & Me.TabCssClass
            End If
            If (Me.TabDisabledCssClass.Length > 0) Then
                TSTab.DisabledCssClass = Me.TabDisabledCssClass
            End If
            If (Not Me.Height.IsEmpty) Then
                TSTab.Height = Me.TabHeight
            End If
            If (Me.TabSelectedCssClass.Length > 0) Then
                TSTab.SelectedCssClass = Me.TabSelectedCssClass
            End If
            If (Not Me.TabWidth.IsEmpty) Then
                TSTab.Width = Me.TabWidth
            End If
        End Sub

        Private Sub BuildTabStrip()
            While Not (PagesQueue.Count = 0)
                Dim pageID As Integer = CInt(PagesQueue.Dequeue())
                Dim currentElement As qElement = CType(AuthPages(pageID), qElement)
                Dim page As TabInfo = currentElement.page

                'add Tab properties
                SetTabProperties(currentElement.TSTab)

                CheckShowPath(page.TabID, page.TabName, currentElement.TSTab)

                If ((ItemsPerRow > 0) AndAlso (currentElement.item Mod ItemsPerRow = 0)) Then
                    currentElement.TSTab.IsBreak = True
                End If

                If (Not page.DisableLink) Then
                    If (page.FullUrl.StartsWith("*") And page.FullUrl.IndexOf("*", 1) <> -1) Then
                        currentElement.TSTab.NavigateUrl = page.FullUrl.Substring(page.FullUrl.IndexOf("*", 1) + 1)
                        currentElement.TSTab.Target = page.FullUrl.Substring(1, page.FullUrl.IndexOf("*", 1) - 1)
                    Else
                        currentElement.TSTab.NavigateUrl = page.FullUrl
                    End If
                ElseIf (CopyChildItemLink AndAlso page.Level >= MaxLevel) Then
                    Dim j As Integer = 0
                    'check if there are any child items and use a href from the first one
                    While (j < AuthPages.Count AndAlso _
                      (CType(AuthPages(j), qElement).page.ParentId <> page.TabID OrElse _
                      CType(AuthPages(j), qElement).page.DisableLink))
                        j = j + 1
                    End While
                    If (j < AuthPages.Count) Then
                        ' child item found. use its link
                        currentElement.TSTab.NavigateUrl = CType(AuthPages(j), qElement).page.FullUrl
                    End If
                End If

                If ((EnableToolTips) AndAlso (page.Description <> String.Empty)) Then
                    currentElement.TSTab.ToolTip = page.Description
                End If

                If (Me.TabImageUrl.Length > 0) Then
                    currentElement.TSTab.ImageUrl = Me.TabImageUrl.Replace("*SkinPath*", dnnSkinPath).Replace("*PageName*", page.TabName)
                End If

                If ((page.IconFile <> String.Empty) AndAlso (currentElement.TSTab.ImageUrl.Length = 0)) Then
                    Dim imgaddress As String
                    'If (DotNetNuke.Common.glbAppVersion.StartsWith("05")) Then
                    If (page.IconFile.StartsWith("~")) Then
                        imgaddress = Me.Page.ResolveUrl(page.IconFile)
                    Else
                        imgaddress = PortalSettings.HomeDirectory & page.IconFile
                    End If
                    'Else
                    '    If (page.IsAdminTab Or page.IsSuperTab) Then
                    '        imgaddress = CType(IIf(Me.Request.ApplicationPath <> "/", Me.Request.ApplicationPath.ToUpper, String.Empty), String) & "/images/" & page.IconFile
                    '    Else
                    '        imgaddress = PortalSettings.HomeDirectory & page.IconFile
                    '    End If
                    'End If
                    currentElement.TSTab.ImageUrl = imgaddress
                End If

                If ((Not ImagesOnlyTabStrip) OrElse (ImagesOnlyTabStrip AndAlso currentElement.TSTab.ImageUrl.Length = 0)) Then
                    currentElement.TSTab.Text = page.TabName
                End If

                'attach child items the current one
                If (page.Level < MaxLevel Or MaxLevel < 0) Then
                    Dim iItemIndex, j As Integer
                    Dim childElement As qElement
                    iItemIndex = 0
                    For j = 0 To AuthPages.Count - 1
                        childElement = CType(AuthPages(j), qElement)
                        If (childElement.page.ParentId = page.TabID) Then
                            currentElement.TSTab.Tabs.Add(childElement.TSTab)
                            PagesQueue.Enqueue(j)
                            iItemIndex = iItemIndex + 1
                            childElement.item = iItemIndex
                            AuthPages(j) = childElement
                        End If
                    Next j
                End If
            End While
        End Sub

#End Region

    End Class

End Namespace