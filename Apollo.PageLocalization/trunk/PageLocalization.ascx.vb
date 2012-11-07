'
' copyright (c) 2005-2009 by Erik van Ballegoij ( erik@apollo-software.nl ) ( http://www.apollo-software.nl )
'
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports Apollo.DNN_Localization
Imports Apollo.DNN.Modules.PageLocalization.Components
Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security
Imports DotNetNuke.UI.Skins
Imports System.Reflection
Imports DotNetNuke.Security.Permissions
Imports System.Collections.Generic
Imports Telerik.Web.UI


Namespace Apollo.DNN.Modules.PageLocalization
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.PageLocalization
    ''' Class	 : DNN.Modules.PageLocalization.PageLocalization
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This is the admin module for PageLocalization. This module provides an interface
    ''' for localizing tabnames and titles. 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[erik]	17-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class PageLocalization
        Inherits PortalModuleBase

        Private ReadOnly minApiVersion As Version = New Version(2, 1, 0, 0)

        Private PagesQueue As Queue
        Private AuthPages As ArrayList
        Private arrayShowPath As ArrayList

        Private EnableAdminMenus As Boolean = False

        Private Structure qElement
            Dim TVNode As RadTreeNode
            Dim page As TabInfo
            Dim item As Integer
        End Structure


#Region "Enums"

        Private Enum eImageType
            FolderClosed = 0
            FolderOpen = 1
            Page = 2
            GotoParent = 3
        End Enum

#End Region

        Private _localLocale As String = ""

        Private Property localLocale() As String
            Get
                If _localLocale = "" Then
                    If ViewState("Locale") Is Nothing Then
                        ViewState("Locale") = Globalization.CultureInfo.CurrentCulture.Name
                    End If
                    _localLocale = CType(ViewState("Locale"), String)
                End If
                Return _localLocale
            End Get
            Set(ByVal value As String)
                _localLocale = value
                ViewState("Locale") = value
            End Set
        End Property

        Private Property ItemId() As Integer
            Get
                If ViewState("itemId") Is Nothing Then
                    Return -1
                Else
                    Return CType(ViewState("itemId"), Integer)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("itemId") = value
            End Set
        End Property

        Private _tabController As TabController
        Private ReadOnly Property tabController As TabController
            Get
                If _tabcontroller Is Nothing Then
                    _tabcontroller = New TabController
                End If
                Return _tabController
            End Get
        End Property

#Region "private methods"


        Private Sub doUpdate()
            Dim objSecurity As New PortalSecurity


            Try
                If ItemId <> -1 Then

                    Dim objLocalizedTabInfo As LocalizedTabInfo = PageLocalizationController.GetByItem(ItemId)
                    If Not (objLocalizedTabInfo Is Nothing) Then
                        objLocalizedTabInfo.TabName = _
                            objSecurity.InputFilter(txtTabName.Text, _
                                                     PortalSecurity.FilterFlag.NoScripting)
                        objLocalizedTabInfo.Title = _
                            objSecurity.InputFilter(txtTitle.Text, _
                                                     PortalSecurity.FilterFlag.NoScripting)
                        objLocalizedTabInfo.IsVisible = chkMenu.Checked And (ddlRedirTabs.SelectedIndex = 0)
                        objLocalizedTabInfo.Description = _
                            objSecurity.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoScripting)
                        objLocalizedTabInfo.KeyWords = _
                            objSecurity.InputFilter(txtKeyWords.Text, PortalSecurity.FilterFlag.NoScripting)
                        objLocalizedTabInfo.PageHeadText = _
                            objSecurity.InputFilter(txtPageHeadText.Text, _
                                                     PortalSecurity.FilterFlag.NoScripting)
                        PageLocalizationController.Update(objLocalizedTabInfo, UserId)
                        Skin.AddModuleMessage(Me, Localization.GetString("Update.Success", LocalResourceFile), _
                                               ModuleMessage.ModuleMessageType.GreenSuccess)

                        RedirectsController.Delete(objLocalizedTabInfo.Locale, objLocalizedTabInfo.TabID)
                        If ddlRedirTabs.SelectedValue <> "-1" Then
                            Dim objRedirectsInfo As New RedirectsInfo() With {.SrcTabId = objLocalizedTabInfo.TabID, .SrcLang = objLocalizedTabInfo.Locale, .ReTabId = Integer.Parse(ddlRedirTabs.SelectedValue), .ReLang = objLocalizedTabInfo.Locale}
                            RedirectsController.Add(objRedirectsInfo)
                        End If

                    End If

                End If
            Catch exc As Exception 'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Is the module in adminmode? (or rather, is the viewer admin?
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property AdminMode() As Boolean
            Get
                Return _
                    PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) Or _
                    TabPermissionController.CanAdminPage(PortalSettings.ActiveTab)
            End Get
        End Property

        'Private Function getTabList(ByVal parentId As Integer, ByVal includeParent As Boolean) As ArrayList
        '    Dim objPageLocalizationController As New PageLocalizationController
        '    Dim AllTabs As ArrayList
        '    Dim sec As New PortalSecurity()
        '    AllTabs = _
        '        objPageLocalizationController.GetByLocaleAndParent(localLocale, PortalId, parentId, includeParent)
        '    If _
        '        Settings.Contains("PageLocalization_CheckPermissions") AndAlso _
        '        CType(Settings("PageLocalization_CheckPermissions"), Boolean) Then
        '        For i As Integer = AllTabs.Count - 1 To 0 Step -1
        '            Dim tabInfo As LocalizedTabInfo = DirectCast(AllTabs(i), LocalizedTabInfo)
        '            If Not TabPermissionController.CanViewPage(tabInfo) Then
        '                AllTabs.RemoveAt(i)
        '            End If
        '        Next
        '    End If
        '    Return AllTabs
        'End Function


        'Private Function tabExpiredClause(ByVal aTab As TabInfo) As Boolean
        '    ' If cbIncludeExpired.Checked Then
        '    ' Return True
        '    ' Else
        '    Return ((aTab.StartDate < Now And aTab.EndDate > Now) Or (aTab.StartDate = Date.MinValue))
        '    ' End If
        'End Function

        Function checkVersion(ByRef apiVersion As Version) As Boolean
            Dim a As Assembly = Assembly.LoadFrom(ApplicationMapPath & "\bin\Apollo.LocalizationApi.dll")
            Dim n As AssemblyName = a.GetName
            apiVersion = n.Version
            Return Version.op_GreaterThanOrEqual(n.Version, minApiVersion)
        End Function

#End Region

#Region "Event Handlers"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
            If Not Page.IsPostBack Then
                ViewState("Locale") = CType(Page, PageBase).PageCulture.Name
            End If

        End Sub


        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load, Me.Load
            Try
                If ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "EDIT", ModuleConfiguration) Then
                    Dim apiVersion As Version = Nothing
                    If Not checkVersion(apiVersion) Then
                        Skin.AddModuleMessage(Me, _
                                               String.Format( _
                                                              Localization.GetString("wrongApiVersion.Text", _
                                                                                      LocalResourceFile), _
                                                              apiVersion.ToString, minApiVersion.ToString), _
                                               ModuleMessage.ModuleMessageType.RedError)
                        placeHolder.Visible = False
                    Else
                        If Page.IsPostBack = False Then
                            Dim dnnVersion As Version = DotNetNuke.Application.DotNetNukeContext.Current.Application.Version
                            If (dnnVersion >= New Version(5, 3, 0)) And (dnnVersion < New Version(5, 5, 0)) Then
                                If Not (Context.Items.Contains("dnnversionmessage")) Then
                                    Context.Items.Add("dnnversionmessage", "added")
                                    Skin.AddModuleMessage(Me, _
                                                           String.Format( _
                                                                          Localization.GetString("DNNVersionWarning.Text", _
                                                                                                  LocalResourceFile), _
                                                                          dnnVersion.ToString), _
                                                           ModuleMessage.ModuleMessageType.YellowWarning)
                                End If
                            End If

                            PageLocalizationController.FillDefaults(PortalId, UserId)
                            InitializeRTV()
                            cmdUpdate.ToolTip = Localization.GetString("cmdUpdate.Tooltip", LocalResourceFile)
                            cboEditLanguage.PortalId = PortalId
                            cboEditLanguage.LanguagesListType = LanguagesListType.Enabled
                            cboEditLanguage.DataBind()
                            cboEditLanguage.SetLanguage(localLocale)


                            'Localization.LoadCultureDropDownList(cboEditLanguage, CultureDropDownTypes.NativeName, _
                            '                                      localLocale)


                            ShowTabInfo(Integer.Parse(TV.SelectedValue))

                        End If
                        End If
                Else
                    Skin.AddModuleMessage(Me, Localization.GetString("Access.Denied", LocalResourceFile), _
                                               ModuleMessage.ModuleMessageType.YellowWarning)
                        placeHolder.Visible = False
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cboEditLanguage_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles cboEditLanguage.ItemChanged
            doUpdate()
            localLocale = CType(sender, DotNetNuke.Web.UI.WebControls.DnnLanguageComboBox).SelectedValue

            PageLocalizationController.FillDefaults(PortalId, UserId)
            ShowTabInfo(Integer.Parse(TV.SelectedValue))
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            doUpdate()
        End Sub

        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            MyBase.OnInit(e)
        End Sub



        Protected Sub ddlRedirTabs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles ddlRedirTabs.SelectedIndexChanged
            ensureMenuCheckbox()
        End Sub

#End Region

        Private Sub ensureMenuCheckbox()
            If ddlRedirTabs.SelectedValue <> "-1" Then
                chkMenu.Checked = False
                chkMenu.Enabled = False
            Else
                chkMenu.Enabled = True
            End If

        End Sub

        Private Sub ShowTabInfo(ByVal tabId As Integer)
            Dim tabInfo As LocalizedTabInfo = PageLocalizationController.Get(tabId, localLocale)

            If tabInfo IsNot Nothing Then
                txtTabName.Text = tabInfo.TabName
                txtKeyWords.Text = tabInfo.KeyWords
                txtPageHeadText.Text = tabInfo.PageHeadText
                txtDescription.Text = tabInfo.Description
                txtTitle.Text = tabInfo.Title
                chkMenu.Checked = tabInfo.IsVisible

                ddlRedirTabs.Items.Clear()
                ddlRedirTabs.DataSource = _
                    PageLocalizationController.GetTabsToRedirect(localLocale, PortalId, tabInfo.TabID)
                ddlRedirTabs.DataTextField = "IndentedTabName"
                ddlRedirTabs.DataValueField = "TabId"
                ddlRedirTabs.DataBind()

                ddlRedirTabs.Items.Insert(0, _
                                           New RadComboBoxItem(Localization.GetString("NoneSelected", LocalResourceFile), _
                                                         "-1"))
                ddlRedirTabs.SelectedIndex = 0
                Dim objRedirectsInfo As RedirectsInfo = RedirectsController.Get(localLocale, tabId)
                If objRedirectsInfo IsNot Nothing Then
                    If ddlRedirTabs.Items.FindItemByValue(objRedirectsInfo.ReTabId.ToString) IsNot Nothing Then
                        ddlRedirTabs.SelectedValue = objRedirectsInfo.ReTabId.ToString
                    End If
                End If

                Dim dimItem As RadComboBoxItem = ddlRedirTabs.Items.FindItemByValue(tabId.ToString)
                If dimItem IsNot Nothing Then
                    dimItem.Enabled = False

                End If

                ItemId = tabInfo.itemID

                ensureMenuCheckbox()

            End If


        End Sub

        'Private Sub FillShowPathArray(ByRef arrayShowPath As ArrayList, ByVal selectedTabID As Integer)
        '    Dim tempTab As TabInfo = tabController.GetTab(selectedTabID, PortalSettings.PortalId, True)
        '    While (tempTab.ParentId <> -1)
        '        arrayShowPath.Add(tempTab.TabID)
        '        tempTab = tabController.GetTab(tempTab.ParentId, PortalSettings.PortalId, True)
        '    End While
        '    arrayShowPath.Add(tempTab.TabID)
        'End Sub

        Private Shared Function IsTabVisible(ByVal tab As TabInfo) As Boolean
            If (tab.IsDeleted = False) And _
                 (Permissions.TabPermissionController.CanViewPage(tab)) Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function getTopParent(ByVal tab As TabInfo) As TabInfo
            If tab.ParentId > 0 Then
                Return getTopParent(tabController.GetTab(tab.ParentId, PortalId, True))
            Else
                Return tab
            End If
        End Function
        Private Function CheckMenuVisibility(ByVal tab As TabInfo) As Boolean
            If (Not EnableAdminMenus AndAlso (tab.TabName = "Admin" Or tab.IsSuperTab Or getTopParent(tab).TabName = "Admin")) Then
                Return False
            End If
            Return True
        End Function


        Private Sub InitializeRTV()
            If TV.Nodes.Count = 0 Then

                Dim iItemIndex As Integer
                Dim iRootGroupId, StartingItemId As Integer

                StartingItemId = 0
                iRootGroupId = 0

                AuthPages = New ArrayList
                PagesQueue = New Queue
                arrayShowPath = New ArrayList
                iItemIndex = 0

                TV.Nodes.Clear()
                TreeBuildlevel(-1, Nothing)
                TV.Nodes(0).Selected = True
                If (TV.Nodes.Count = 0) Then
                    TV.Visible = False
                End If
            End If
        End Sub

        'Private Sub CheckShowPath(ByVal tab As TabInfo, ByRef node As RadTreeNode)
        '    If (arrayShowPath.Contains(tab.TabID)) Then
        '        If (PortalSettings.ActiveTab.TabID.Equals(tab.TabID)) Then
        '            node.Selected = True
        '            node.Expanded = True
        '        End If
        '        node.ExpandParentNodes()
        '    End If
        'End Sub

        Private Sub SetNodeProperties(ByRef node As RadTreeNode, ByVal tab As TabInfo)
            node.Value = tab.TabID.ToString()

            If tab.HasChildren Then
                node.ImageUrl = ResolveUrl("images/documents.png")
            Else
                node.ImageUrl = ResolveUrl("images/document_plain.png")

            End If

            node.Text = tab.TabName
        End Sub

  

        Private Sub TV_NodeClick(ByVal sender As Object, ByVal e As RadTreeNodeEventArgs) Handles TV.NodeClick
            'selectedTabId = Integer.Parse(e.Node.Value)
            ShowTabInfo(Integer.Parse(e.Node.Value))

        End Sub

        Private Sub TreeBuildlevel(ByVal ParentId As Integer, ByVal parentNode As RadTreeNode)
            Dim i As Integer
            Dim node As RadTreeNode
            Dim desktopTabs As List(Of TabInfo) = tabController.GetTabsByParent(ParentId, PortalId)
            For i = 0 To desktopTabs.Count - 1
                Dim tab As TabInfo = desktopTabs(i)

                If IsTabVisible(tab) And CheckMenuVisibility(tab) Then

                    node = New RadTreeNode()
                    SetNodeProperties(node, tab)

                    If (tab.HasChildren) Then
                        node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack
                    End If

                    If parentNode Is Nothing Then
                        TV.Nodes.Add(node)
                    Else
                        parentNode.Nodes.Add(node)

                    End If
                End If
            Next i
        End Sub

        Private Sub TV_NodeExpand(ByVal sender As Object, ByVal e As RadTreeNodeEventArgs) Handles TV.NodeExpand
            'function to handle Load-On-Demand callbacks
            TreeBuildlevel(Integer.Parse(e.Node.Value), e.Node)
        End Sub
    End Class
End Namespace
