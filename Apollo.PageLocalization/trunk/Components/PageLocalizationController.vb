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
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports System.Xml
Imports DataProvider = Apollo.DNN.Modules.PageLocalization.Data.DataProvider

Namespace Apollo.DNN.Modules.PageLocalization
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.PageLocalization
    ''' Class	 : DNN.Modules.PageLocalization.PageLocalizationController
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This controller class controls access to database functions for the page
    ''' localization module
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[erik]	17-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class PageLocalizationController
        Implements IPortable


#Region "Public Methods"

        Public Shared Function [Get](ByVal tabID As Integer, ByVal locale As String) As LocalizedTabInfo
            Return _
                CType( _
                    CBO.FillObject(DataProvider.Instance().GetTabLocalization(tabID, locale), _
                                   GetType(LocalizedTabInfo)), LocalizedTabInfo)
        End Function

        Public Shared Function GetByItem(ByVal itemID As Integer) As LocalizedTabInfo
            Return _
                CType( _
                    CBO.FillObject(DataProvider.Instance().GetTabLocalizationByItem(itemID), _
                                   GetType(LocalizedTabInfo)), LocalizedTabInfo)
        End Function

        Public Shared Function List(ByVal portalId As Integer) As ArrayList
            Return _
                CBO.FillCollection(DataProvider.Instance().ListTabLocalization(portalId), GetType(LocalizedTabInfo))
        End Function

        Public Shared Function GetByTabs(ByVal tabID As Integer) As ArrayList
            Return _
                CBO.FillCollection(DataProvider.Instance().GetTabLocalizationByTabs(tabID), GetType(LocalizedTabInfo))
        End Function

        Public Shared Function GetTabsToRedirect(ByVal Locale As String, ByVal PortalId As Integer, ByVal tabID As Integer) _
            As ArrayList
            Return _
                CBO.FillCollection(DataProvider.Instance().GetTabsToRedirect(Locale, PortalId, tabID), _
                                   GetType(LocalizedTabInfo))
        End Function

        Public Shared Function GetByLocaleAndParent(ByVal locale As String, ByVal portalId As Integer, _
                                             ByVal ParentID As Integer, ByVal includeParent As Boolean) As ArrayList
            Return _
                CBO.FillCollection( _
                                   DataProvider.Instance().GetTabLocalizationByLocaleAndParent(locale, portalId, _
                                                                                               ParentID, includeParent), _
                                   GetType(LocalizedTabInfo))
        End Function

        Public Shared Function Add(ByVal objLocalizedTabInfo As LocalizedTabInfo, ByVal CreatedByUserId As Integer) As Integer
            Dim returnValue As Integer =
                CType( _
                    DataProvider.Instance().AddTabLocalization(objLocalizedTabInfo.TabID, objLocalizedTabInfo.Locale, _
                                                               objLocalizedTabInfo.TabName, objLocalizedTabInfo.Title, _
                                                               objLocalizedTabInfo.Description, _
                                                               objLocalizedTabInfo.KeyWords, _
                                                               objLocalizedTabInfo.IsVisible, _
                                                               objLocalizedTabInfo.PageHeadText, CreatedByUserId), Integer)
            ClearCache()
            Return returnValue
        End Function

        Public Shared Sub Update(ByVal objLocalizedTabInfo As LocalizedTabInfo, ByVal CreatedByUserId As Integer)
            DataProvider.Instance().UpdateTabLocalization(objLocalizedTabInfo.itemID, objLocalizedTabInfo.TabID, _
                                                          objLocalizedTabInfo.Locale, objLocalizedTabInfo.TabName, _
                                                          objLocalizedTabInfo.Title, objLocalizedTabInfo.Description, _
                                                          objLocalizedTabInfo.KeyWords, objLocalizedTabInfo.IsVisible, _
                                                          objLocalizedTabInfo.PageHeadText, CreatedByUserId)
            ClearCache()
        End Sub

        Public Shared Sub Delete(ByVal itemID As Integer)
            DataProvider.Instance().DeleteTabLocalization(itemID)
            ClearCache()
        End Sub

        Public Shared Sub FillDefaults(ByVal PortalID As Integer, ByVal CreatedByUserId As Integer)
            DataProvider.Instance().FillDefaults(PortalID, CreatedByUserId)
            ClearCache()
        End Sub

        Public Shared Sub UpdateOrAdd(ByVal objLocalizedTabInfo As LocalizedTabInfo, ByVal CreatedByUserId As Integer)
            DataProvider.Instance().UpdateOrAddTabLocalization(objLocalizedTabInfo.itemID, objLocalizedTabInfo.TabID, _
                                                               objLocalizedTabInfo.Locale, objLocalizedTabInfo.TabName, _
                                                               objLocalizedTabInfo.Title, _
                                                               objLocalizedTabInfo.Description, _
                                                               objLocalizedTabInfo.KeyWords, _
                                                               objLocalizedTabInfo.IsVisible, _
                                                               objLocalizedTabInfo.PageHeadText, CreatedByUserId)
            ClearCache()
        End Sub


        Public Shared Sub ClearCache()
            DataCache.ClearCache("ApolloPageLocalization")
        End Sub


#End Region


#Region "Optional Interfaces"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Exports module data. Because we have no way of knowing the tab id's in the 
        ''' new portal (assuming that the portal template function is used), its
        ''' pointless to export the tab id at all. Instead not only export the defaultname
        ''' of the tab, but also the name of its parent. Logic suggests it's not very common to have
        ''' 2 tabs with both the same name and the same parentname. We only export the non-admin tabs for the current portal
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	9-2-2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements IPortable.ExportModule
            ' Get PortalId and home dir
            Dim m As ModuleInfo = (New ModuleController).GetModule(ModuleID, -1, True)
            Dim PortalId As Integer = m.PortalID
            Dim exportArrayList As ArrayList = List(PortalId)
            Dim tabController As New TabController
            Dim strXML As String = ""
            If Not exportArrayList Is Nothing Then
                strXML += "<PageLocalization>" + Environment.NewLine
                strXML += "<Items>" + Environment.NewLine
                Dim MLTabinfo As LocalizedTabInfo
                Dim oldDefaultName As String = ""
                Dim tabElementOpen As Boolean = False
                For Each MLTabinfo In exportArrayList
                    If MLTabinfo.defaultName <> oldDefaultName Then
                        If tabElementOpen Then
                            strXML += "</Tab>" + Environment.NewLine
                        End If
                        strXML += "<Tab>" + Environment.NewLine
                        tabElementOpen = True
                        strXML += "<defaultName>" + MLTabinfo.defaultName + "</defaultName>" + Environment.NewLine
                        strXML += "<tabPath>" + MLTabinfo.TabPath + "</tabPath>" + Environment.NewLine
                        Dim tabinfo As TabInfo = tabController.GetTab(MLTabinfo.ParentId, PortalId, True)
                        If tabinfo Is Nothing Then
                            strXML += "<parentTabName></parentTabName>" + Environment.NewLine
                        Else
                            strXML += "<parentTabName>" + tabinfo.TabName + "</parentTabName>" + Environment.NewLine
                        End If
                    End If
                    strXML += "<localTab>" + Environment.NewLine
                    strXML += "<locale>" + MLTabinfo.Locale + "</locale>" + Environment.NewLine
                    strXML += "<localTabname>" + MLTabinfo.TabName + "</localTabname>" + Environment.NewLine
                    strXML += "<localTitle>" + MLTabinfo.Title + "</localTitle>" + Environment.NewLine
                    strXML += "<localDescription>" + MLTabinfo.Description + "</localDescription>" + Environment.NewLine
                    strXML += "<localKeywords>" + MLTabinfo.KeyWords + "</localKeywords>" + Environment.NewLine
                    strXML += "<localIsVisible>" + MLTabinfo.IsVisible.ToString + "</localIsVisible>" + _
                              Environment.NewLine
                    strXML += "</localTab>" + Environment.NewLine
                    oldDefaultName = MLTabinfo.defaultName
                Next
                If tabElementOpen Then
                    strXML += "</Tab>" + Environment.NewLine
                End If
                strXML += "</Items>" + vbCrLf
                strXML += "</PageLocalization>" + Environment.NewLine
            End If
            Return strXML
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Importing the data is somewhat difficult because we have no way of linking
        ''' to the original tabid. Instead, we try to find the correct tab by searching
        ''' on defaultTabName and ParentTabName. If we find a match, we will save the localized info for that tab
        ''' Saving is done with one call to a stored proc that decides whether to add a new 
        ''' record, or update an existing record. 
        ''' 
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <param name="Content"></param>
        ''' <param name="Version"></param>
        ''' <param name="UserID"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	9-2-2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, _
                                 ByVal UserID As Integer) Implements IPortable.ImportModule
            ' Get PortalId and home dir
            Dim m As ModuleInfo = (New ModuleController).GetModule(ModuleID, -1, True)
            Dim PortalId As Integer = m.PortalID

            Dim xmlPageLocalization As XmlNode = GetContent(Content, "PageLocalization")
            Dim xmlItems As XmlNode = xmlPageLocalization.SelectSingleNode("Items")
            Dim xmlMLTab As XmlNode
            Dim xmlLocalTab As XmlNode
            Dim tabController As New TabController
            'Dim allTabs As ArrayList = tabController.GetTabs(portalId)

            For Each xmlMLTab In xmlItems
                Dim tabDefaultName As String = xmlMLTab.Item("defaultName").InnerText
                Dim parentTabName As String = xmlMLTab.Item("parentTabName").InnerText
                Dim tabPath As String = xmlMLTab.Item("tabPath").InnerText
                Dim tabId As Integer = tabController.GetTabByTabPath(PortalId, tabPath)
                Dim originalTabInfo As TabInfo = tabController.GetTab(tabId, -1, True)
                If Not originalTabInfo Is Nothing Then
                    Dim xmlLocalTabs As XmlNodeList = xmlMLTab.SelectNodes("localTab")
                    If Not xmlLocalTabs Is Nothing Then
                        For Each xmlLocalTab In xmlLocalTabs
                            Dim MLTabinfo As New LocalizedTabInfo(originalTabInfo)
                            MLTabinfo.Locale = xmlLocalTab.Item("locale").InnerText
                            MLTabinfo.TabName = xmlLocalTab.Item("localTabname").InnerText
                            MLTabinfo.Title = xmlLocalTab.Item("localTitle").InnerText
                            MLTabinfo.Description = xmlLocalTab.Item("localDescription").InnerText
                            MLTabinfo.KeyWords = xmlLocalTab.Item("localKeywords").InnerText
                            MLTabinfo.IsVisible = Boolean.Parse(xmlLocalTab.Item("localIsVisible").InnerText)
                            UpdateOrAdd(MLTabinfo, UserController.GetCurrentUserInfo.UserID)
                        Next
                    End If
                End If
            Next

        End Sub


#End Region
    End Class
End Namespace
