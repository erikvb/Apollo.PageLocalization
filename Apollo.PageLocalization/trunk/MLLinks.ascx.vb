'
' DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Shaun Walker ( sales@perpetualmotion.ca ) of Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports Apollo.DNN_Localization
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Entities.Tabs
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Collections.Generic
Imports Apollo.DNN.Modules.PageLocalization.Components

Namespace Apollo.DNN.SkinObjects
    ''' -----------------------------------------------------------------------------
    ''' <summary></summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------


    Partial Class MLLinks
        Inherits SkinObjectBase

        ' private members
        Private _separator As String = ""
        Private _cssClass As String = ""
        Private _level As String = ""
        Private _alignment As String = ""
        Private _ignoreHidden As String = ""
        Private _showAdmin As String = ""
        Private _onlyAdmin As String = ""
        Private _showDisabled As Boolean = False
        Private _forceLinks As Boolean = True

        ' protected controls

#Region "Public Members"

        Public Property Separator() As String
            Get
                Return _separator
            End Get
            Set(ByVal Value As String)
                _separator = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _cssClass
            End Get
            Set(ByVal Value As String)
                _cssClass = Value
            End Set
        End Property

        Public Property Level() As String
            Get
                Return _level
            End Get
            Set(ByVal Value As String)
                _level = Value
            End Set
        End Property

        Public Property Alignment() As String
            Get
                Return _alignment
            End Get
            Set(ByVal Value As String)
                _alignment = Value
            End Set
        End Property


        Public Property IgnoreHidden() As String
            Get
                Return _ignoreHidden
            End Get
            Set(ByVal Value As String)
                _ignoreHidden = Value
            End Set
        End Property

        Public Property ShowAdmin() As String
            Get
                Return _showAdmin
            End Get
            Set(ByVal Value As String)
                _showAdmin = Value
            End Set
        End Property

        Private ReadOnly Property blnShowAdmin() As Boolean
            Get
                Try
                    If ShowAdmin <> "" Then
                        Return Boolean.Parse(ShowAdmin)
                    End If
                Catch
                End Try
                Return True
            End Get
        End Property

        Public Property OnlyAdmin() As String
            Get
                Return _onlyAdmin
            End Get
            Set(ByVal Value As String)
                _onlyAdmin = Value
            End Set
        End Property

        Private ReadOnly Property blnOnlyAdmin() As Boolean
            Get
                Try
                    If OnlyAdmin <> "" Then
                        Return Boolean.Parse(OnlyAdmin)
                    End If
                Catch
                End Try
                Return False
            End Get
        End Property

        Public Property ShowDisabled() As Boolean
            Get
                If _ignoreHidden <> "" Then
                    Try
                        _showDisabled = Not Boolean.Parse(IgnoreHidden)
                    Catch
                    End Try
                End If
                Return _showDisabled
            End Get
            Set(ByVal value As Boolean)
                _showDisabled = value
            End Set
        End Property

        Public Property ForceLinks() As Boolean
            Get
                Return _forceLinks
            End Get
            Set(ByVal value As Boolean)
                _forceLinks = value
            End Set
        End Property

#End Region


 
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' public attributes
            Dim strCssClass As String
            If CssClass <> "" Then
                strCssClass = CssClass
            Else
                strCssClass = "SkinObject"
            End If

            Dim strSeparator As String = String.Empty

            If Separator <> "" Then
                If Separator.IndexOf("src=") <> -1 Then
                    'Add the skinpath to image paths
                    Separator = Regex.Replace(Separator, "src=[']?", "$&" & PortalSettings.ActiveTab.SkinPath)
                End If
                'Wrap in a span
                Separator = String.Format("<span class=""{0}"">{1}</span>", strCssClass, Separator)
            Else
                strSeparator = "  "
            End If

            ' build links
            Dim strLinks As String = ""

            strLinks = BuildLinks(Level, strSeparator, strCssClass)

            If strLinks = "" Then
                strLinks = BuildLinks("", strSeparator, strCssClass)
            End If

            lblLinks.Text = strLinks


        End Sub

        Private Function BuildLinks(ByVal strLevel As String, ByVal strSeparator As String, ByVal strCssClass As String) _
            As String
            Dim sbLinks As New StringBuilder


            Dim portalTabs As List(Of TabInfo) = TabController.GetTabsBySortOrder(PortalSettings.PortalId)
            Dim hostTabs As List(Of TabInfo) = TabController.GetTabsBySortOrder(Null.NullInteger)

            If Not blnOnlyAdmin Then
                For Each objTab As TabInfo In portalTabs
                    sbLinks.Append( _
                                    ProcessLink( _
                                                 ProcessTab(LocalizeTab.getLocalizedTab(objTab), strLevel, strCssClass), _
                                                 sbLinks.ToString.Length))
                Next
            End If

            If blnShowAdmin Then
                For Each objTab As TabInfo In hostTabs
                    sbLinks.Append(ProcessLink(ProcessTab(objTab, strLevel, strCssClass), sbLinks.ToString.Length))
                Next
            End If
            Return sbLinks.ToString

        End Function

        ''' <summary>
        ''' Processes the tab.
        ''' </summary>
        ''' <param name="objTab">The obj tab.</param>
        ''' <param name="strLevel">The STR level.</param>
        ''' <param name="strCssClass">The STR CSS class.</param>
        ''' <returns></returns>
        Private Function ProcessTab(ByVal objTab As TabInfo, ByVal strLevel As String, ByVal strCssClass As String) _
            As String

            If Navigation.CanShowTab(objTab, AdminMode, ShowDisabled) Then
                Select Case strLevel.ToLowerInvariant

                    Case "same", "" 'Render tabs on the same level as the current tab
                        If objTab.ParentId = PortalSettings.ActiveTab.ParentId Then
                            Return AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                        End If

                    Case "child" 'Render the current tabs child tabs
                        If objTab.ParentId = PortalSettings.ActiveTab.TabID Then

                            Return AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                        End If
                    Case "parent" 'Render the current tabs parenttab
                        If objTab.TabID = PortalSettings.ActiveTab.ParentId Then
                            Return AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                        End If
                    Case "root" 'Render Root tabs
                        If objTab.Level = 0 Then
                            Return AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                        End If
                End Select

            End If

            Return ""

        End Function

        ''' <summary>
        ''' Processes the link.
        ''' </summary>
        ''' <param name="sLink">The s link.</param>
        ''' <param name="iLinksLength">Length of the i links.</param>
        ''' <returns></returns>
        Private Function ProcessLink(ByVal sLink As String, ByVal iLinksLength As Integer) As String

            If sLink = "" Then Return ""

            'wrap in a div if set to vertical
            If Alignment.ToLowerInvariant = "vertical" Then
                sLink = String.Concat("<div>", Separator, sLink, "</div>")
            Else
                'If not vertical, then render the separator
                If Not Separator = "" And iLinksLength > 0 Then
                    sLink = String.Concat(Separator, sLink)
                End If
            End If

            Return sLink

        End Function

        ''' <summary>
        ''' Adds the link.
        ''' </summary>
        ''' <param name="strTabName">Name of the STR tab.</param>
        ''' <param name="strURL">The STR URL.</param>
        ''' <param name="strCssClass">The STR CSS class.</param>
        ''' <returns></returns>
        Private Function AddLink(ByVal strTabName As String, ByVal strURL As String, ByVal strCssClass As String) _
            As String
            Return String.Format("<a class=""{0}"" href=""{1}"">{2}</a>", strCssClass, strURL, strTabName)
        End Function
    End Class
End Namespace
