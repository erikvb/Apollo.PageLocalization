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

Namespace Apollo.DNN_Localization
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.LocalizationApi
    ''' Class	 : DNN_Localization.SqlDataProvider
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This class implements the MS SQL Server specific commands for the LocalizationAPI
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

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Default implementation of a dotnetnuke dataprovider
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	16-6-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
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

#Region "General private Methods"

        Private Function GetNull(ByVal Field As Object) As Object
            Return Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

#Region "TabLocalization Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets an IDataReader (multiple records) with all localized tabs for this locale. Admin tabs and invisible tabs are not selected
        ''' </summary>
        ''' <param name="locale">String -- [language]-[Region] combination</param>
        ''' <param name="portalid">integer -- current portal</param>
        ''' <returns>IDataReader</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function GetTabLocalizationByLocale(ByVal locale As String, ByVal portalid As Integer) _
            As IDataReader
            Return _
                CType( _
                    SqlHelper.ExecuteReader(ConnectionString, _
                                             String.Format("{0}{1}Apollo_TabLocalizationGetByLocale", DatabaseOwner, ObjectQualifier), _
                                             locale, portalid), IDataReader)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the availability of PageLocalization. This test checks whether key 
        ''' procedures and tables exist in the database ... These are installed by
        ''' the PageLocalization Module.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' This is the MS SQL Server command that is run in this function
        ''' <code language="T-SQL">
        ''' select value = case when count(*)=2 then 1 else 0 end 
        ''' from dbo.sysobjects 
        ''' where 
        '''     ((id = object_id(N&#39;{databaseOwner}{objectQualifier}Apollo_TabLocalization&#39;))
        '''     and (OBJECTPROPERTY(id, N&#39;IsUserTable&#39;) = 1)) 
        '''     or 
        '''     ((id = object_id(N&#39;{databaseOwner}{objectQualifier}Apollo_TabLocalizationGetByLocale&#39;))
        '''     and (OBJECTPROPERTY(id, N&#39;IsProcedure&#39;) = 1))
        ''' </code>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Function testAvailabilty() As IDataReader
            Dim strSQL As String = ""
            strSQL += "select value = case when count(*)=2 then 1 else 0 end "
            strSQL += "from  dbo.sysobjects "
            strSQL += "where "
            strSQL += "  ((id = object_id(N'{databaseOwner}{objectQualifier}Apollo_TabLocalization')) "
            strSQL += "  and (OBJECTPROPERTY(id, N'IsUserTable') = 1)) "
            strSQL += "  or "
            strSQL += "  ((id = object_id(N'{databaseOwner}{objectQualifier}Apollo_TabLocalizationGetByLocale')) "
            strSQL += "  and (OBJECTPROPERTY(id, N'IsProcedure') = 1))"

            strSQL = strSQL.Replace("{databaseOwner}", DatabaseOwner)
            strSQL = strSQL.Replace("{objectQualifier}", ObjectQualifier)

            Try
                Return CType(SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, strSQL), IDataReader)
            Catch
                ' error in SQL query
                Return Nothing
            End Try

        End Function

        Public Overrides Function GetLastTabUpdate(ByVal portalId As Integer) As IDataReader
            Return _
                 CType( _
                     SqlHelper.ExecuteReader(ConnectionString, _
                                              String.Format("{0}{1}Apollo_TabLocalizationGetLastTabUpdate", DatabaseOwner, ObjectQualifier), _
                                              portalId), IDataReader)
        End Function


#End Region
    End Class
End Namespace