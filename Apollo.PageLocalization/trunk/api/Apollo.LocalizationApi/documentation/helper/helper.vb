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
Namespace Apollo.DNN_Localization.helper
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.LocalizationApi
    ''' Class	 : DNN_Localization.helper.booleanWrapper
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This Helper class acts as a wrapper for a boolean value. This makes it possible to use caching for the contained value
    ''' </summary>
    ''' <history>
    ''' 	[erik]	13-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class booleanWrapper
        Private _value As Boolean

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this is the property that gets or set the contained value
        ''' </summary>
        ''' <returns>Boolean</returns>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property value() As Boolean
            Get
                Return _value
            End Get
            Set(ByVal Value As Boolean)
                _value = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This constructor initializes the contained value to False
        ''' </summary>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
            value = False
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This constructor initializes the contained value to newValue
        ''' </summary>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal newValue As Boolean)
            value = newValue
        End Sub
    End Class
End Namespace
