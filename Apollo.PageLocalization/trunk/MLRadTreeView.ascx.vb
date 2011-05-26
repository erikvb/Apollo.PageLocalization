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

    Partial Public Class MLRadTreeView
        Inherits DotNetNuke.UI.Skins.SkinObjectBase

#Region "Private Members"

        'variables and structures
        Private PagesQueue As Queue
        Private AuthPages As ArrayList
        Private arrayShowPath As ArrayList
        Private dnnSkinSrc As String = PortalSettings.ActiveTab.SkinSrc.Replace("\"c, "/"c).Replace("//", "/")
        Private dnnSkinPath As String = dnnSkinSrc.Substring(0, dnnSkinSrc.LastIndexOf("/"c))

        Private Structure qElement
            Dim TVNode As RadTreeNode
            Dim page As TabInfo
            Dim item As Integer
        End Structure

        Private _Style As String = String.Empty

        'other treeview properties
        Private _EnableToolTips As Boolean = True
        Private _CopyChildItemLink As Boolean = False
        Private _EnableUserMenus As Boolean = True
        Private _EnableAdminMenus As Boolean = True
        Private _EnableXmlOutput As Boolean = False
        Private _EnablePageIdCssClass As Boolean = False
        Private _EnableLOD As Boolean = False
        Private _MaxLevel As Integer = -1
        Private _ShowOnlyCurrent As String = String.Empty
        Private _PagesToExclude As String

#End Region

#Region "Public Properties"

        Public Property CausesValidation() As Boolean
            Get
                Return TV.CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                TV.CausesValidation = Value
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

        Public Property Dir() As String
            Get
                Return TV.Attributes("dir")
            End Get
            Set(ByVal Value As String)
                TV.Attributes("dir") = Value
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

        Public Property EnableEmbeddedBaseStylesheet() As Boolean
            Get
                Return TV.EnableEmbeddedBaseStylesheet
            End Get
            Set(ByVal Value As Boolean)
                TV.EnableEmbeddedBaseStylesheet = Value
            End Set
        End Property

        Public Property EnableEmbeddedScripts() As Boolean
            Get
                Return TV.EnableEmbeddedScripts
            End Get
            Set(ByVal Value As Boolean)
                TV.EnableEmbeddedScripts = Value
            End Set
        End Property

        Public Property EnableEmbeddedSkins() As Boolean
            Get
                Return TV.EnableEmbeddedSkins
            End Get
            Set(ByVal Value As Boolean)
                TV.EnableEmbeddedSkins = Value
            End Set
        End Property

        Public Property EnableLOD() As Boolean
            Get
                Return _EnableLOD
            End Get
            Set(ByVal Value As Boolean)
                _EnableLOD = Value
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

        Public Property EnableUserMenus() As Boolean
            Get
                Return _EnableUserMenus
            End Get
            Set(ByVal Value As Boolean)
                _EnableUserMenus = Value
            End Set
        End Property

        Public Property EnableXmlOutput() As Boolean
            Get
                Return _EnableXmlOutput
            End Get
            Set(ByVal Value As Boolean)
                _EnableXmlOutput = Value
            End Set
        End Property

        Public Property Height() As System.Web.UI.WebControls.Unit
            Get
                Return TV.Height
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                TV.Height = Value
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

        Public Property MultipleSelect() As Boolean
            Get
                Return TV.MultipleSelect
            End Get
            Set(ByVal Value As Boolean)
                TV.MultipleSelect = Value
            End Set
        End Property

        Public Property LoadingMessage() As String
            Get
                Return TV.LoadingMessage
            End Get
            Set(ByVal Value As String)
                TV.LoadingMessage = Value
            End Set
        End Property

        Public Property LoadingMessagePosition() As TreeViewLoadingStatusPosition
            Get
                Return TV.LoadingStatusPosition
            End Get
            Set(ByVal Value As TreeViewLoadingStatusPosition)
                TV.LoadingStatusPosition = Value
            End Set
        End Property

        Public Property OnClientContextMenuItemClicked() As String
            Get
                Return TV.OnClientContextMenuItemClicked
            End Get
            Set(ByVal value As String)
                TV.OnClientContextMenuItemClicked = value
            End Set
        End Property

        Public Property OnClientContextMenuItemClicking() As String
            Get
                Return TV.OnClientContextMenuItemClicking
            End Get
            Set(ByVal value As String)
                TV.OnClientContextMenuItemClicking = value
            End Set
        End Property

        Public Property OnClientContextMenuShowing() As String
            Get
                Return TV.OnClientContextMenuShowing
            End Get
            Set(ByVal value As String)
                TV.OnClientContextMenuShowing = value
            End Set
        End Property

        Public Property OnClientContextMenuShown() As String
            Get
                Return TV.OnClientContextMenuShown
            End Get
            Set(ByVal value As String)
                TV.OnClientContextMenuShown = value
            End Set
        End Property

        Public Property OnClientKeyPressing() As String
            Get
                Return TV.OnClientKeyPressing
            End Get
            Set(ByVal value As String)
                TV.OnClientKeyPressing = value
            End Set
        End Property

        Public Property OnClientDoubleClick() As String
            Get
                Return TV.OnClientDoubleClick
            End Get
            Set(ByVal Value As String)
                TV.OnClientDoubleClick = Value
            End Set
        End Property

        Public Property OnClientMouseOut() As String
            Get
                Return TV.OnClientMouseOut
            End Get
            Set(ByVal Value As String)
                TV.OnClientMouseOut = Value
            End Set
        End Property

        Public Property OnClientMouseOver() As String
            Get
                Return TV.OnClientMouseOver
            End Get
            Set(ByVal Value As String)
                TV.OnClientMouseOver = Value
            End Set
        End Property

        Public Property OnClientNodeChecked() As String
            Get
                Return TV.OnClientNodeChecked
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeChecked = Value
            End Set
        End Property

        Public Property OnClientNodeChecking() As String
            Get
                Return TV.OnClientNodeChecking
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeChecking = Value
            End Set
        End Property

        Public Property OnClientNodeClicked() As String
            Get
                Return TV.OnClientNodeClicked
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeClicked = Value
            End Set
        End Property

        Public Property OnClientNodeClicking() As String
            Get
                Return TV.OnClientNodeClicking
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeClicking = Value
            End Set
        End Property

        Public Property OnClientNodeCollapsed() As String
            Get
                Return TV.OnClientNodeCollapsed
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeCollapsed = Value
            End Set
        End Property

        Public Property OnClientNodeCollapsing() As String
            Get
                Return TV.OnClientNodeCollapsing
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeCollapsing = Value
            End Set
        End Property

        Public Property OnClientNodeDragging() As String
            Get
                Return TV.OnClientNodeDragging
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeDragging = Value
            End Set
        End Property

        Public Property OnClientNodeDragStart() As String
            Get
                Return TV.OnClientNodeDragStart
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeDragStart = Value
            End Set
        End Property

        Public Property OnClientNodeDropped() As String
            Get
                Return TV.OnClientNodeDropped
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeDropped = Value
            End Set
        End Property

        Public Property OnClientNodeDropping() As String
            Get
                Return TV.OnClientNodeDropping
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeDropping = Value
            End Set
        End Property

        Public Property OnClientNodeEdited() As String
            Get
                Return TV.OnClientNodeEdited
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeEdited = Value
            End Set
        End Property

        Public Property OnClientNodeEditing() As String
            Get
                Return TV.OnClientNodeEditing
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeEditing = Value
            End Set
        End Property

        Public Property OnClientNodeExpanded() As String
            Get
                Return TV.OnClientNodeExpanded
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeExpanded = Value
            End Set
        End Property

        Public Property OnClientNodeExpanding() As String
            Get
                Return TV.OnClientNodeExpanding
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodeExpanding = Value
            End Set
        End Property

        Public Property OnClientNodePopulated() As String
            Get
                Return TV.OnClientNodePopulated
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodePopulated = Value
            End Set
        End Property

        Public Property OnClientNodePopulating() As String
            Get
                Return TV.OnClientNodePopulating
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodePopulating = Value
            End Set
        End Property

        Public Property OnClientNodePopulationFailed() As String
            Get
                Return TV.OnClientNodePopulationFailed
            End Get
            Set(ByVal Value As String)
                TV.OnClientNodePopulationFailed = Value
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

        Public Property SingleExpandPath() As Boolean
            Get
                Return TV.SingleExpandPath
            End Get
            Set(ByVal Value As Boolean)
                TV.SingleExpandPath = Value
            End Set
        End Property

        Public Property ShowLineImages() As Boolean
            Get
                Return TV.ShowLineImages
            End Get
            Set(ByVal Value As Boolean)
                TV.ShowLineImages = Value
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

        Public Property Skin() As String
            Get
                Return TV.Skin
            End Get
            Set(ByVal Value As String)
                TV.Skin = Value
            End Set
        End Property

        Public Property Style() As String
            Get
                Return _Style
            End Get
            Set(ByVal Value As String)
                _Style = Value
            End Set
        End Property

        Public Property Width() As System.Web.UI.WebControls.Unit
            Get
                Return TV.Width
            End Get
            Set(ByVal Value As System.Web.UI.WebControls.Unit)
                TV.Width = Value
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
            SetTreeViewProperties()

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
                    tabElement.TVNode = New RadTreeNode

                    If (EnablePageIdCssClass) Then
                        tabElement.TVNode.CssClass &= " rtvPageId" & currentTab.TabID.ToString
                    End If

                    If CheckShowOnlyCurrent(currentTab.TabID, currentTab.ParentId, StartingItemId, iRootGroupId) AndAlso _
                       CheckMenuVisibility(currentTab) Then
                        iItemIndex = iItemIndex + 1
                        tabElement.item = iItemIndex
                        PagesQueue.Enqueue(AuthPages.Count)
                        TV.Nodes.Add(tabElement.TVNode)
                    End If

                    AuthPages.Add(tabElement)
                End If
            Next i
            BuildTreeView()
            If (TV.Nodes.Count = 0) Then
                TV.Visible = False
            End If
        End Sub

#End Region

#Region "Private Helper Methods"

        Private Function IsTabVisible(ByVal tab As TabInfo) As Boolean
            If (tab.IsVisible = True And tab.IsDeleted = False) And _
            ((tab.StartDate < Now And tab.EndDate > Now) Or AdminMode) And _
            (Permissions.TabPermissionController.CanViewPage(tab) And Not CheckToExclude(tab.TabName, tab.TabID)) Then
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

        Private Sub CheckShowPath(ByVal tab As TabInfo, ByRef node As RadTreeNode)
            If (arrayShowPath.Contains(tab.TabID)) Then
                If (PortalSettings.ActiveTab.TabID.Equals(tab.TabID)) Then
                    node.Selected = True
                    node.Expanded = True
                End If
                node.ExpandParentNodes()
            End If
        End Sub

        Private Sub SetTreeViewProperties()
            If (Style <> String.Empty) Then
                Style += "; "
                Try
                    For Each cStyle As String In Style.Split(";"c)
                        If (cStyle.Trim.Length > 0) Then
                            TV.Style.Add(cStyle.Split(":"c)(0), cStyle.Split(":"c)(1))
                        End If
                    Next
                Catch
                End Try
            End If
        End Sub

        Private Sub SetNodeProperties(ByRef node As RadTreeNode, ByVal tab As TabInfo)
            node.Value = tab.TabID.ToString()

            If (Not tab.DisableLink) Then
                If (tab.FullUrl.StartsWith("*") And tab.FullUrl.IndexOf("*", 1) <> -1) Then
                    node.NavigateUrl = tab.FullUrl.Substring(tab.FullUrl.IndexOf("*", 1) + 1)
                    node.Target = tab.FullUrl.Substring(1, tab.FullUrl.IndexOf("*", 1) - 1)
                Else
                    node.NavigateUrl = tab.FullUrl
                End If
            ElseIf (CopyChildItemLink AndAlso tab.Level >= MaxLevel) Then
                Dim j As Integer = 0
                'check if there are any child items and use a href from the first one
                While (j < AuthPages.Count AndAlso _
                  (CType(AuthPages(j), qElement).page.ParentId <> tab.TabID OrElse _
                  CType(AuthPages(j), qElement).page.DisableLink))
                    j = j + 1
                End While
                If (j < AuthPages.Count) Then
                    ' child item found. use its link
                    node.NavigateUrl = CType(AuthPages(j), qElement).page.FullUrl
                End If
            End If

            If ((EnableToolTips) AndAlso (tab.Description <> String.Empty)) Then
                node.ToolTip = tab.Description
            End If

            If ((tab.IconFile <> String.Empty) AndAlso (node.ImageUrl.Length = 0)) Then
                Dim imgaddress As String

                If (tab.IconFile.StartsWith("~")) Then
                    imgaddress = Me.Page.ResolveUrl(tab.IconFile)
                Else
                    imgaddress = PortalSettings.HomeDirectory & tab.IconFile
                End If
                node.ImageUrl = imgaddress
            End If

            node.Text = tab.TabName
        End Sub

        Private Sub RadTree1_NodeExpand(ByVal o As Object, ByVal e As RadTreeNodeEventArgs) Handles TV.NodeExpand
            'function to handle Load-On-Demand callbacks
            Dim i As Integer
            Dim node As RadTreeNode
            Dim portalID As Integer = CType(IIf(PortalSettings.ActiveTab.IsSuperTab, -1, PortalSettings.PortalId), Integer)
            Dim desktopTabs As List(Of TabInfo) = TabController.GetTabsBySortOrder(portalID)
            For i = 0 To desktopTabs.Count - 1
                Dim tab As TabInfo = CType(desktopTabs(i), TabInfo)
                If (tab.ParentId.ToString = e.Node.Value) Then
                    node = New RadTreeNode()

                    SetNodeProperties(node, tab)

                    If (tab.HasChildren) Then
                        node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack
                    End If

                    e.Node.Nodes.Add(node)
                End If
            Next i
        End Sub

        Private Sub BuildTreeView()
            While Not (PagesQueue.Count = 0)
                Dim pageID As Integer = CInt(PagesQueue.Dequeue())
                Dim currentElement As qElement = CType(AuthPages(pageID), qElement)
                Dim page As TabInfo = currentElement.page

                CheckShowPath(page, currentElement.TVNode)

                'add Tab properties
                SetNodeProperties(currentElement.TVNode, page)

                'attach child items the current one
                If (page.Level < MaxLevel Or MaxLevel < 0) Then
                    If (EnableLOD AndAlso currentElement.page.HasChildren) Then
                        currentElement.TVNode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack
                    Else
                        Dim iItemIndex, j As Integer
                        Dim childElement As qElement
                        iItemIndex = 0
                        For j = 0 To AuthPages.Count - 1
                            childElement = CType(AuthPages(j), qElement)
                            If (childElement.page.ParentId = page.TabID) Then
                                currentElement.TVNode.Nodes.Add(childElement.TVNode)
                                PagesQueue.Enqueue(j)
                                iItemIndex = iItemIndex + 1
                                childElement.item = iItemIndex
                                AuthPages(j) = childElement
                            End If
                        Next j
                    End If
                End If
            End While
        End Sub

#End Region

    End Class

End Namespace