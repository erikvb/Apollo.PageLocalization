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
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Tabs
Imports Apollo.DNN_Localization.helper
Imports System.Threading
Imports System.Web
Imports DotNetNuke.Entities.Portals

Namespace Apollo.DNN_Localization
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.LocalizationApi
    ''' Class	 : DNN_Localization.LocalizeTab
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this class exposes methods for easy localization of tabs. With just one call, a tab can be localized
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[erik]	13-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class LocalizeTab
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this function gets an array of all available localized tabs for the specified
        ''' locale and portalID. The list is cached in the current HTTPcontext
        ''' 
        ''' this method also checks the last modification date of all tabs in this portal against
        ''' the last datetime localized tabs were loaded, and will invalidate cache if needs be
        ''' </summary>
        ''' <param name="locale"></param>
        ''' <param name="portalID"></param>
        ''' <returns>Arraylist with localized tabs</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	13-5-2005	Created
        ''' 	[erik]	7-1-2008	modified to work with DNN 5 Caching
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function getLocalizedTabsArray(ByVal locale As String, ByVal portalID As Integer) _
        As ArrayList
            Dim cacheKey As String = String.Format(Constants.LocalizedTabArrayCacheKey, portalID, locale)
            Dim cacheKeyLast As String = String.Format(Constants.LastTimeTabArrayCacheKey, portalID, locale)

            Dim tabsLastUpdate As DateTime = LocalizationController.GetLastTabUpdate(portalID)
            Dim cacheLastUpdate As Object = DataCache.GetCache(cacheKeyLast)
            If (cacheLastUpdate IsNot Nothing) AndAlso (tabsLastUpdate > DirectCast(cacheLastUpdate, DateTime)) Then
                DataCache.RemoveCache(cacheKey)
            End If
            Return (
                CBO.GetCachedObject(Of ArrayList)( _
                                                    New CacheItemArgs(cacheKey, Constants.LocalizedTabArrayCacheTimeOut, _
                                                                       Constants.LocalizedTabArrayCachePriority, locale, _
                                                                       portalID), _
                                                    AddressOf getLocalizedTabsArrayCallback))


        End Function

        ''' <summary>
        ''' Cache CallBack function that gets the actual localized tabs from the database
        ''' </summary>
        ''' <param name="cacheItemArgs"></param>
        ''' <returns></returns>
        Private Shared Function getLocalizedTabsArrayCallback(ByVal cacheItemArgs As CacheItemArgs) As Object
            Dim locale As String = DirectCast(cacheItemArgs.ParamList(0), String)
            Dim portalID As Integer = DirectCast(cacheItemArgs.ParamList(1), Integer)
            Dim cacheKey As String = String.Format(Constants.LastTimeTabArrayCacheKey, portalID, locale)


            Dim objPageLocalizationController As New LocalizationController
            If objPageLocalizationController.LocalizationAvailable Then

                Dim arrLocalTabs As ArrayList

                arrLocalTabs = objPageLocalizationController.GetByLocale(locale, portalID)
                arrLocalTabs.Sort()
                'sort on tabid, to make search easier
                DataCache.SetCache(cacheKey, DateTime.Now)
                Return arrLocalTabs
            Else
                Return Nothing
            End If

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this function gets an array of all available localized tabs. The current locale
        ''' and current portalid are taken from respectively current thread and current HTTPContext
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function getLocalizedTabsArray() As ArrayList
            If HttpContext.Current.Items("PortalSettings") Is Nothing Then
                Return Nothing
            Else
                Return _
                    getLocalizedTabsArray(Thread.CurrentThread.CurrentCulture.Name, _
                                           CType(HttpContext.Current.Items("PortalSettings"), PortalSettings).PortalId)
            End If
        End Function

        Public Shared Function getLocalizedTabsArray(ByVal Locale As String) As ArrayList
            If HttpContext.Current.Items("PortalSettings") Is Nothing Then
                Return Nothing
            Else
                Return _
                    getLocalizedTabsArray(Locale, _
                                           CType(HttpContext.Current.Items("PortalSettings"), PortalSettings).PortalId)
            End If
        End Function


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this function matches a dnn core tabinfo object to the available localized tabs
        ''' if a match is available, the matching localized tab is return. If no match is found,
        ''' then a new localizedtabinfo object is created and hydrated with info from the dnn core tabinfo object
        ''' The matching is done by way of doing a binary search in the arraylist. For this to work the
        ''' <see cref="LocalizedTabInfo"/> class implements System.IComparable
        ''' </summary>
        ''' <param name="objTab"></param>
        ''' <param name="arrLocalTabs"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	13-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function matchLocalTabToTab(ByVal objTab As TabInfo, ByVal arrLocalTabs As ArrayList) _
            As LocalizedTabInfo
            If (objTab Is Nothing) OrElse (arrLocalTabs Is Nothing) Then
                Return Nothing
            Else

                Dim objPageLocalizationController As New LocalizationController
                If objPageLocalizationController.LocalizationAvailable Then
                    Dim localTabIndex As Integer = -1
                    localTabIndex = arrLocalTabs.BinarySearch(objTab)
                    If localTabIndex >= 0 Then
                        Return CType(arrLocalTabs(localTabIndex), LocalizedTabInfo)
                    Else
                        Return New LocalizedTabInfo(objTab)
                    End If
                Else
                    Return New LocalizedTabInfo(objTab)
                End If
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this public function calls the protected getlocalizedtab function. This is the point of entry for the class
        ''' a localized tab is returned
        ''' </summary>
        ''' <param name="objTab"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	13-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function getLocalizedTab(ByVal objTab As TabInfo) As LocalizedTabInfo
            If objTab Is Nothing Then
                Return Nothing
            Else
                Dim returnValue As LocalizedTabInfo = matchLocalTabToTab(objTab, getLocalizedTabsArray())
                If returnValue Is Nothing Then
                    returnValue = New LocalizedTabInfo(objTab)
                End If
                Return returnValue
            End If
        End Function

        Public Shared Function getLocalizedTab(ByVal objTab As TabInfo, ByVal Locale As String) As LocalizedTabInfo
            If objTab Is Nothing Then
                Return Nothing
            Else
                Dim returnValue As LocalizedTabInfo = matchLocalTabToTab(objTab, getLocalizedTabsArray(Locale))
                If returnValue Is Nothing Then
                    returnValue = New LocalizedTabInfo(objTab)
                End If
                Return returnValue
            End If
        End Function

        Public Shared Function getLocalizedTab(ByVal objTab As TabInfo, ByVal Locale As String, ByVal PortalId As Integer) As LocalizedTabInfo
            If objTab Is Nothing Then
                Return Nothing
            Else
                Dim returnValue As LocalizedTabInfo = matchLocalTabToTab(objTab, getLocalizedTabsArray(Locale, PortalId))
                If returnValue Is Nothing Then
                    returnValue = New LocalizedTabInfo(objTab)
                End If
                Return returnValue
            End If
        End Function
    End Class
End Namespace
