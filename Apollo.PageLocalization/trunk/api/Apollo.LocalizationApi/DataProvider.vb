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
Imports DotNetNuke.Framework

Namespace Apollo.DNN_Localization
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' constructor
        ''' </summary>
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
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Shared Sub CreateProvider()
            objProvider = _
                CType( _
                    Reflection.CreateObject("Apollo.DNN_Localization.SqlDataProvider,Apollo.LocalizationApi", _
                                             "Apollo.DNN_Localization.dataProvider"), DataProvider)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' return the provider
        ''' </summary>
        ''' <returns></returns>
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
        ''' Gets an IDataReader (multiple records) with all localized tabs for this locale. Admin tabs and invisible tabs are not selected
        ''' </summary>
        ''' <returns></returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function GetTabLocalizationByLocale(ByVal locale As String, ByVal portalId As Integer) _
            As IDataReader

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the availability of pageLocalization. With this function you can 
        ''' easily test whether the PageLocalization module is available. 
        ''' </summary>
        ''' <returns>True if PageLocalization is available</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public MustOverride Function testAvailabilty() As IDataReader

        Public MustOverride Function GetLastTabUpdate(ByVal portalId As Integer) As IDataReader


#End Region
    End Class
End Namespace
