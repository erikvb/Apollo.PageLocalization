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
Imports System.Web.Caching

Namespace Apollo.DNN_Localization.helper
    ''' <summary>
    ''' This class contains constants for the Localization API
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Constants
        Public Const LocalizedTabArrayCacheKey As String = "ApolloPageLocalization_{0}_LocalizedTabArrayCacheKey_{1}"
        Public Const LocalizedTabArrayCachePriority As CacheItemPriority = CacheItemPriority.High
        Public Const LocalizedTabArrayCacheTimeOut As Integer = 200
        Public Const LocalizationAvailableCacheKey As String = "ApolloPageLocalization_LocalizationAvailable"
        Public Const LocalizationAvailableCachePriority As CacheItemPriority = CacheItemPriority.High
        Public Const LocalizationAvailableCacheTimeOut As Integer = 200
        Public Const LastTimeTabArrayCacheKey As String = "ApolloPageLocalization_{0}_LocalizedTabArrayCacheKey_{1}_last"

    End Class
End Namespace

