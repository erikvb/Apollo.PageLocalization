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
Imports Apollo.DNN_Localization.helper
Imports DotNetNuke.Common.Utilities

Namespace Apollo.DNN_Localization
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.LocalizationApi
    ''' Class	 : DNN_Localization.LocalizationController
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' minimal data controller class, which in essence does nothing more than providing a way to get a list 
    ''' of localized tabs (by locale)
    ''' </summary>
    ''' <history>
    ''' 	[erik]	13-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class LocalizationController

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this function checks the availability of the PageLocalization module, by checking whether some key tables/procedures
        ''' exist in the db
        ''' This functin makes use of caching, making use of a <see cref="helper.booleanWrapper">boolean wrapper</see>
        ''' </summary>
        ''' <returns>Boolean: if false then no localization will be available</returns>
        ''' <example>
        ''' This is a code sample from <see cref="LocalizeTab.getLocalizedTabsArray"/>
        ''' <code language="Visual Basic">
        ''' Dim objPageLocalizationController As New LocalizationController
        ''' If objPageLocalizationController.LocalizationAvailable Then
        '''     Dim localTabIndex As Integer = -1
        '''     localTabIndex = arrLocalTabs.BinarySearch(objTab)
        '''     If localTabIndex &gt;= 0 Then
        '''         Return CType(arrLocalTabs(localTabIndex), LocalizedTabInfo)
        '''     Else
        '''         Return New LocalizedTabInfo(objTab)
        '''     End If
        ''' Else
        '''     Return New LocalizedTabInfo(objTab)
        ''' End If
        ''' </code>
        ''' </example>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erikvb]	13-5-2005	Created
        '''     [erikvb]    07-01-2009  modified to use DNN 5.0 caching
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function LocalizationAvailable() As Boolean
            Dim cacheKey As String = Constants.LocalizationAvailableCacheKey
            Dim _
                blwrap As booleanWrapper = _
                    CBO.GetCachedObject(Of booleanWrapper)( _
                                                             New CacheItemArgs(cacheKey, _
                                                                                Constants. _
                                                                                   LocalizationAvailableCacheTimeOut, _
                                                                                Constants. _
                                                                                   LocalizationAvailableCachePriority), _
                                                             AddressOf GetLocalizationAvailableCallBack)
            Return blwrap.value
        End Function

        Private Function GetLocalizationAvailableCallBack() As booleanWrapper
            Return CBO.FillObject(Of booleanWrapper)(DataProvider.Instance().testAvailabilty())
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets an arraylist of all localized tabs for this locale.
        ''' </summary>
        ''' <param name="locale"></param>
        ''' <param name="portalId"></param>
        ''' <returns>arraylist</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	13-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetByLocale(ByVal locale As String, ByVal portalId As Integer) As ArrayList
            Return _
                CBO.FillCollection(DataProvider.Instance().GetTabLocalizationByLocale(locale, portalId), _
                                    GetType(LocalizedTabInfo))
        End Function

        ''' <summary>
        ''' Gets the last datetime a tab was modified
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns></returns>
        Public Shared Function GetLastTabUpdate(ByVal portalId As Integer) As DateTime
            Return CBO.FillObject(Of DateTime)(DataProvider.Instance().GetLastTabUpdate(portalId))
        End Function

#End Region
    End Class
End Namespace
