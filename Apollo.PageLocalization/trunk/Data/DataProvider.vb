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
Namespace Apollo.DNN.Modules.PageLocalization.Data


    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.PageLocalization
    ''' Class	 : DNN.Modules.PageLocalization.DataProvider
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' provides abstract methods for accessing data
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[erik]	17-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' constructor
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Shared Sub New()
            CreateProvider()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' dynamically create provider. The provider is created using reflection, 
        ''' so it doesn't have to be created every time ...
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Shared Sub CreateProvider()
            objProvider = _
                CType(Reflection.CreateObject("data", "Apollo.DNN.Modules.PageLocalization.Data", "", True), DataProvider)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' return the provider
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region


#Region "TabLocalization Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets the localized info for the requested tab and locale
        ''' </summary>
        ''' <param name="tabID">integer, tab to localize</param>
        ''' <param name="locale">string, requested locale</param>
        ''' <returns>IDataReader ... one record</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function GetTabLocalization(ByVal tabID As Integer, ByVal locale As String) As IDataReader

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets the localized info for the requested item
        ''' </summary>
        ''' <param name="itemID">integer, itemid of the localized tab</param>
        ''' <returns>IDataReader ... one record</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function GetTabLocalizationByItem(ByVal itemID As Integer) As IDataReader

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets all localized tabs
        ''' </summary>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function ListTabLocalization(ByVal portalId As Integer) As IDataReader

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets all localizations by tabsid
        ''' </summary>
        ''' <param name="tabID"></param>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function GetTabLocalizationByTabs(ByVal tabID As Integer) As IDataReader

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets all redirect candidates
        ''' </summary>
        ''' <param name="tabID"></param>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <history>
        ''' 	[erik]	18-8-2008	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function GetTabsToRedirect(ByVal Locale As String, ByVal PortalId As Integer, _
                                                       ByVal tabID As Integer) As IDataReader


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets all localizations by Locale and ParentID. This function is used to build the admin interface of the module.
        ''' </summary>
        ''' <param name="locale"></param>
        ''' <param name="portalId"></param>
        ''' <param name="parentID"></param>
        ''' <param name="includeParent"></param>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function GetTabLocalizationByLocaleAndParent(ByVal locale As String, _
                                                                         ByVal portalId As Integer, _
                                                                         ByVal parentID As Integer, _
                                                                         ByVal includeParent As Boolean) As IDataReader

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Adds a specific localized tab. Actually this function is not used much, as the admin module calls the 
        ''' fill defaults function to fill all values based on the unlocalized tabs. 
        ''' </summary>
        ''' <param name="tabID">integer</param>
        ''' <param name="locale">string</param>
        ''' <param name="tabName">string</param>
        ''' <param name="title">string</param>
        ''' <param name="description">string</param>
        ''' <param name="keywords">string</param>
        ''' <param name="isVisible">Boolean</param>
        ''' <returns>integer: the itemid of the added tab</returns>
        ''' <seealso cref="DataProvider.FillDefaults"/>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function AddTabLocalization(ByVal tabID As Integer, ByVal locale As String, _
                                                        ByVal tabName As String, ByVal title As String, _
                                                        ByVal description As String, ByVal keywords As String, _
                                                        ByVal isVisible As Boolean, ByVal PageHeaderText As String, ByVal CreatedByUserId As Integer) _
            As Integer

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Updates the localized tabinfo
        ''' </summary>
        ''' <param name="itemID"></param>
        ''' <param name="tabID"></param>
        ''' <param name="locale"></param>
        ''' <param name="tabName"></param>
        ''' <param name="title"></param>
        ''' <param name="description"></param>
        ''' <param name="keywords"></param>
        ''' <param name="isVisible"></param>
        ''' <param name="PageHeaderText"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Sub UpdateTabLocalization(ByVal itemID As Integer, ByVal tabID As Integer, _
                                                      ByVal locale As String, ByVal tabName As String, _
                                                      ByVal title As String, ByVal description As String, _
                                                      ByVal keywords As String, ByVal isVisible As Boolean, _
                                                      ByVal PageHeaderText As String, ByVal CreatedByUserId As Integer)

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Deletes the localized tabinfo
        ''' </summary>
        ''' <param name="itemID"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Sub DeleteTabLocalization(ByVal itemID As Integer)

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a set of default values for the requested locale and portal
        ''' </summary>
        ''' <param name="locale"></param>
        ''' <param name="portalId"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <seealso cref="DataProvider.AddTabLocalization"/>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Sub FillDefaults(ByVal portalId As Integer, ByVal CreatedByUserId As Integer)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="itemID"></param>
        ''' <param name="tabID"></param>
        ''' <param name="locale"></param>
        ''' <param name="tabName"></param>
        ''' <param name="title"></param>
        ''' <param name="description"></param>
        ''' <param name="keywords"></param>
        ''' <param name="isVisible"></param>
        ''' <param name="PageHeaderText"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub UpdateOrAddTabLocalization(ByVal itemID As Integer, ByVal tabID As Integer, _
                                                           ByVal locale As String, ByVal tabName As String, _
                                                           ByVal title As String, ByVal description As String, _
                                                           ByVal keywords As String, ByVal isVisible As Boolean, _
                                                           ByVal PageHeaderText As String, ByVal CreatedByUserId As Integer)

#End Region
    End Class
End Namespace