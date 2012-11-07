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
Imports System.Diagnostics
Imports Apollo.DNN_Localization
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Entities.Tabs


Namespace Apollo.DNN.SkinObjects
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.PageLocalization
    ''' Class	 : DNN.SkinObjects.MLBreadCrumb
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Provides a localized version of the breadcrumb skinobject
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' 	[erik]	17-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class MLBreadCrumb
        Inherits SkinObjectBase

        ' private members
        Private _separator As String
        Private _cssClass As String
        Private _rootLevel As String

        Const MyFileName As String = "MLBreadcrumb.ascx"

        ' protected controls
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' label placeholder for the actual breadcrumb
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected WithEvents lblBreadCrumb As Label

#Region "Public Members"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The separator to be used between levels. Defaults to /images/breadcrumb.gif if no 
        ''' seperator is passed to the object
        ''' </summary>
        ''' <returns>String</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Separator() As String
            Get
                Return _separator
            End Get
            Set(ByVal Value As String)
                _separator = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Provides a means to set the CSS class to be used by the object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property CssClass() As String
            Get
                Return _cssClass
            End Get
            Set(ByVal Value As String)
                _cssClass = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Provides a way to determine at what level the breadcrumb should start. The 
        ''' default value is 1, if Root should be included, pass -1 to the object
        ''' </summary>
        ''' <returns>string</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property RootLevel() As String
            Get
                Return _rootLevel
            End Get
            Set(ByVal Value As String)
                _rootLevel = Value
            End Set
        End Property

#End Region



        '*******************************************************
        '
        ' The Page_Load server event handler on this page is used
        ' to populate the role information for the page
        '
        '*******************************************************

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            If Not Page.IsPostBack Then

                ' public attributes
                Dim strSeparator As String
                If Separator <> "" Then
                    If Separator.IndexOf("src=") <> -1 Then
                        Separator = Replace(Separator, "src=""", "src=""" & PortalSettings.ActiveTab.SkinPath)
                    End If
                    strSeparator = Separator
                Else
                    strSeparator = "&nbsp;<img alt=""*"" src=""" & ApplicationPath & "/images/breadcrumb.gif"">&nbsp;"
                End If

                Dim strCssClass As String
                If CssClass <> "" Then
                    strCssClass = CssClass
                Else
                    strCssClass = "SkinObject"
                End If

                Dim intRootLevel As Integer
                If RootLevel <> "" Then
                    intRootLevel = Integer.Parse(RootLevel)
                Else
                    intRootLevel = 1
                End If

                Dim strBreadCrumbs As String = ""

                If intRootLevel = -1 Then
                    strBreadCrumbs += _
                        String.Format(Localization.GetString("Root", Localization.GetResourceFile(Me, MyFileName)), _
                                       GetPortalDomainName(PortalSettings.PortalAlias.HTTPAlias, Request, True), strCssClass)
                    intRootLevel = 0
                End If

                ' process bread crumbs

                Dim intTab As Integer
                For intTab = intRootLevel To PortalSettings.ActiveTab.BreadCrumbs.Count - 1

                    If intTab <> intRootLevel Then
                        strBreadCrumbs += strSeparator
                    End If
                    Dim _
                        objTab As LocalizedTabInfo = _
                            LocalizeTab.getLocalizedTab(CType(PortalSettings.ActiveTab.BreadCrumbs(intTab), TabInfo))


                    If objTab.DisableLink Then
                        strBreadCrumbs += "<span class=""" & strCssClass & """>" & objTab.TabName & "</span>"
                    Else
                        strBreadCrumbs += "<a href=""" & objTab.FullUrl & """ class=""" & strCssClass & """>" & _
                                          objTab.TabName & "</a>"
                    End If
                Next
                lblBreadCrumb.Text = Convert.ToString(strBreadCrumbs)

            End If

        End Sub
    End Class
End Namespace
