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
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Common.Utilities

Namespace Apollo.DNN.Modules.PageLocalization.Data


    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.PageLocalization.SqlDataProvider
    ''' Class	 : DNN.Modules.PageLocalization.SqlDataProvider
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this class implements the SQL Server specific commands for the
    ''' page localization module.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[erik]	17-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SqlDataProvider
        Inherits DataProvider

#Region "Private Members"

        Private Const ProviderType As String = "data"

        Private _
            _providerConfiguration As ProviderConfiguration = _
                ProviderConfiguration.GetProviderConfiguration(ProviderType)

        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim _
                objProvider As Provider = _
                    CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

            ' Read the attributes for this provider
            _connectionString = Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region "General Public Methods"

        Private Function GetNull(ByVal Field As Object) As Object
            Return Null.GetNull(Field, DBNull.Value)
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
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function GetTabLocalization(ByVal tabID As Integer, ByVal locale As String) As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationGet", tabID, _
                                            locale), IDataReader)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets the localized info for the requested item
        ''' </summary>
        ''' <param name="itemID">integer, itemid of the localized tab</param>
        ''' <returns>IDataReader ... one record</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function GetTabLocalizationByItem(ByVal itemID As Integer) As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationGetByItemID", _
                                            itemID), IDataReader)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets all localized tabs
        ''' </summary>
        ''' <param name="portalId"></param>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function ListTabLocalization(ByVal portalId As Integer) As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationList", portalId),  _
                    IDataReader)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' gets all localizations by tabsid
        ''' </summary>
        ''' <param name="tabID"></param>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function GetTabLocalizationByTabs(ByVal tabID As Integer) As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationGetByTabs", tabID),  _
                    IDataReader)
        End Function

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
        Public Overrides Function GetTabsToRedirect(ByVal Locale As String, ByVal PortalId As Integer, _
                                                    ByVal tabID As Integer) As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationGetTabsToRedirect", _
                                            Locale, PortalId, tabID), IDataReader)
        End Function


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets all localizations by Locale and ParentID. This function is used to build the admin interface of the module.
        ''' </summary>
        ''' <param name="locale">String</param>
        ''' <param name="portalId">Integer</param>
        ''' <param name="parentID">Integer</param>
        ''' <param name="includeParent">Boolean</param>
        ''' <returns>IDataReader ... multiple records</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function GetTabLocalizationByLocaleAndParent(ByVal locale As String, ByVal portalId As Integer, _
                                                                      ByVal parentID As Integer, _
                                                                      ByVal includeParent As Boolean) As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & _
                                            "Apollo_TabLocalizationGetByLocaleAndParent", locale, portalId, parentID, _
                                            includeParent), IDataReader)
        End Function

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
        ''' <seealso cref="SqlDataProvider.FillDefaults"/>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function AddTabLocalization(ByVal tabID As Integer, ByVal locale As String, _
                                                     ByVal tabName As String, ByVal title As String, _
                                                     ByVal description As String, ByVal keywords As String, _
                                                     ByVal isVisible As Boolean, ByVal pageheadertext As String, ByVal CreatedByUserId As Integer) _
            As Integer
            Return _
                CType( _
                    SqlHelper.ExecuteScalar(ConnectionString, _
                                            DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationAdd", tabID, _
                                            locale, tabName, GetNull(title), GetNull(description), GetNull(keywords), _
                                            isVisible, GetNull(pageheadertext)), Integer)
        End Function

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
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateTabLocalization(ByVal itemID As Integer, ByVal tabID As Integer, _
                                                   ByVal locale As String, ByVal tabName As String, _
                                                   ByVal title As String, ByVal description As String, _
                                                   ByVal keywords As String, ByVal isVisible As Boolean, _
                                                   ByVal PageHeaderText As String, ByVal CreatedByUserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, _
                                      DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationUpdate", itemID, tabID, _
                                      locale, tabName, GetNull(title), GetNull(description), GetNull(keywords), _
                                      isVisible, GetNull(PageHeaderText), CreatedByUserId)
        End Sub

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
        Public Overrides Sub DeleteTabLocalization(ByVal itemID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, _
                                      DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationDelete", itemID)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a set of default values for the requested locale and portal
        ''' </summary>
        ''' <param name="locale"></param>
        ''' <param name="portalId"></param>
        ''' <seealso cref="SqlDataProvider.AddTabLocalization"/>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub FillDefaults(ByVal portalId As Integer, ByVal CreatedByUserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, _
                                      DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationFillDefaults", portalId, CreatedByUserId)
        End Sub

        Public Overrides Sub UpdateOrAddTabLocalization(ByVal itemID As Integer, ByVal tabID As Integer, _
                                                        ByVal locale As String, ByVal tabName As String, _
                                                        ByVal title As String, ByVal description As String, _
                                                        ByVal keywords As String, ByVal isVisible As Boolean, _
                                                        ByVal PageHeaderText As String, ByVal CreatedByUserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, _
                                      DatabaseOwner & ObjectQualifier & "Apollo_TabLocalizationUpdateOrAdd", itemID, _
                                      tabID, locale, tabName, GetNull(title), GetNull(description), _
                                      GetNull(keywords), isVisible, GetNull(PageHeaderText), CreatedByUserId)
        End Sub

#End Region
    End Class
End Namespace