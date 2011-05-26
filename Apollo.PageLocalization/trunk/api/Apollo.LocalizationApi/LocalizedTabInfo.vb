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
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Modules
Imports System.Xml.Serialization
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Security.Permissions

Namespace Apollo.DNN_Localization
    ''' -----------------------------------------------------------------------------
    ''' Project	 : Apollo.LocalizationApi
    ''' Class	 : DNN_Localization.LocalizedTabInfo
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this is the data abstaction info class for the localized tabs. It contains a clone
    ''' of the related real tab, and all properties of the realtab are available (readonly)
    ''' 
    ''' this class also supports sorting, and comparing. This makes it easy to sort the arraylist
    ''' with localized tabs, and it enables quick comparison of dnn core tabinfo object with this object
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[erik]	13-5-2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class LocalizedTabInfo
        Inherits TabInfo
        Implements IComparable
        Implements IHydratable


#Region "Private Members"
        Private _tabRef As TabInfo

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' create a new LocalizedTabInfo instance. This instance contains all information
        ''' off the original tab, and localized where possible
        ''' </summary>
        ''' <param name="tab"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	13-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal tab As TabInfo)
            itemID = -1
            Locale = Localization.SystemLocale
            defaultName = tab.TabName
            indentedDefName = tab.TabName.PadLeft(tab.TabName.Length + 3 * tab.Level, "."c)

            TabID = tab.TabID
            TabOrder = tab.TabOrder
            PortalID = tab.PortalID
            TabName = tab.LocalizedTabName
            ' Me.AuthorizedRoles = tab.AuthorizedRoles 'obsolete in dnn 5
            IsVisible = tab.IsVisible
            ParentId = tab.ParentId
            Level = tab.Level
            IconFile = tab.IconFile
            ' Me.AdministratorRoles = tab.AdministratorRoles 'obsolete in dnn 5
            DisableLink = tab.DisableLink
            Title = tab.Title
            Description = tab.Description
            KeyWords = tab.KeyWords
            IsDeleted = tab.IsDeleted
            Url = tab.Url
            SkinSrc = tab.SkinSrc
            ContainerSrc = tab.ContainerSrc
            TabPath = tab.TabPath
            StartDate = tab.StartDate
            EndDate = tab.EndDate
            ' Me.TabPermissions = tab.TabPermissions changed in dnn 5, set up automatically in the base class property
            HasChildren = tab.HasChildren
            SkinPath = tab.SkinPath
            ContainerPath = tab.ContainerPath
            BreadCrumbs = tab.BreadCrumbs
            IsSuperTab = tab.IsSuperTab

            'new properties added 04-08-2008
            PageHeadText = tab.PageHeadText
            RefreshInterval = tab.RefreshInterval
            IsSecure = tab.IsSecure

            'new properties added 06-01-2009
            SkinDoctype = tab.SkinDoctype

            UniqueId = tab.UniqueId
            VersionGuid = tab.VersionGuid
            LocalizedVersionGuid = tab.LocalizedVersionGuid

            'Default Language Guid should be initialised to a null Guid
            DefaultLanguageGuid = tab.DefaultLanguageGuid

            SiteMapPriority = tab.SiteMapPriority



        End Sub


#End Region

#Region "Public Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This is the default, or rather un-localized, tab name
        ''' </summary>
        ''' <returns>string: unlocalized tab name</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        <XmlIgnore()> _
        Public Property defaultName() As String


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' this is a helper property ... makes for easy indenting tab names. Inserts 3 
        ''' dots for every level
        ''' </summary>
        ''' <returns>string: indented unlocalized tab name</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        <XmlIgnore()> _
        Public Property indentedDefName() As String

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The locale of the localized tab
        ''' </summary>
        ''' <returns>string: Locale [language-Region]</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        <XmlIgnore()> _
        Public Property Locale() As String

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Internal ID. If tab is not localized, this returns -1
        ''' </summary>
        ''' <returns>Integer: internal id</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        <XmlIgnore()> _
        Public Property itemID() As Integer

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Dynamic helper property, calculates if tab is expired, by way of:
        ''' </summary>
        ''' <returns>boolean: true if expired</returns>
        ''' <remarks>
        ''' The following calculation that is used:
        ''' <code>(Me.StartDate &gt; Date.Today) Or (Me.EndDate &lt; Date.Today)</code>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property Expired() As Boolean
            Get
                Return (StartDate > Date.Today) Or (EndDate < Date.Today)
            End Get
        End Property


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The base enddate property returns a null value for empty values. In order
        ''' to do correct date comparisons, a value of date.MaxValue is returned when 
        ''' the enddate is null
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erik]	16-6-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        <XmlElement("enddate")> _
        Public Shadows Property EndDate() As Date
            Get
                Return MyBase.EndDate
            End Get
            Set(ByVal Value As Date)
                If Value = Null.NullDate Then
                    MyBase.EndDate = Date.MaxValue
                Else
                    MyBase.EndDate = Value
                End If
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This function is the implementation of <see cref="System.IComparable.CompareTo"/>. Accepted
        ''' object types are <see cref="LocalizedTabInfo"/> and <see cref="DotNetNuke.Entities.Tabs.Tabinfo"/>.
        ''' Comparison is done based on tabID. If object type is of wrong type, an exception is thrown.
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' This is the implementation:
        ''' <code language="Visual Basic">
        ''' If TypeOf obj Is LocalizedTabInfo Then
        '''     Dim temp As LocalizedTabInfo = CType(obj, LocalizedTabInfo)
        '''     Return TabID.CompareTo(temp.TabID)
        ''' ElseIf TypeOf obj Is TabInfo Then
        '''     Dim temp As TabInfo = CType(obj, TabInfo)
        '''     Return TabID.CompareTo(temp.TabID)
        ''' End If
        ''' Throw New ArgumentException("object is not LocalizedTabInfo or TabInfo")
        ''' </code>
        ''' </remarks>
        ''' <history>
        ''' 	[erikvb]	17-5-2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Try
                If TypeOf obj Is LocalizedTabInfo Then
                    Dim temp As LocalizedTabInfo = CType(obj, LocalizedTabInfo)
                    Return TabID.CompareTo(temp.TabID)
                ElseIf TypeOf obj Is TabInfo Then
                    Dim temp As TabInfo = CType(obj, TabInfo)
                    Return TabID.CompareTo(temp.TabID)
                End If

            Catch ex As Exception

            End Try

            Throw New ArgumentException("object is not LocalizedTabInfo or TabInfo")
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This is a clone function for LocalizedTabInfo. All values are copied
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[erikvb]	17-5-2005	Created
        '''     [erikvb]    04-08-2008  added new properties
        '''     [erikvb]    16-01-2009  modified to support DNN 5
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shadows Function Clone() As LocalizedTabInfo

            ' create the object
            Dim objTabInfo As New LocalizedTabInfo

            objTabInfo.itemID = Me.itemID
            objTabInfo.Locale = Me.Locale
            objTabInfo.defaultName = Me.defaultName
            objTabInfo.indentedDefName = Me.indentedDefName

            ' assign the property values
            objTabInfo.TabID = Me.TabID
            objTabInfo.TabOrder = Me.TabOrder
            objTabInfo.PortalID = Me.PortalID
            objTabInfo.TabName = Me.TabName
            ' Me.AuthorizedRoles = tab.AuthorizedRoles 'obsolete in dnn 5
            objTabInfo.IsVisible = Me.IsVisible
            objTabInfo.ParentId = Me.ParentId
            objTabInfo.Level = Me.Level
            objTabInfo.IconFile = Me.IconFile
            ' Me.AdministratorRoles = tab.AdministratorRoles 'obsolete in dnn 5
            objTabInfo.DisableLink = Me.DisableLink
            objTabInfo.Title = Me.Title
            objTabInfo.Description = Me.Description
            objTabInfo.KeyWords = Me.KeyWords
            objTabInfo.IsDeleted = Me.IsDeleted
            objTabInfo.Url = Me.Url
            objTabInfo.SkinSrc = Me.SkinSrc
            objTabInfo.ContainerSrc = Me.ContainerSrc
            objTabInfo.TabPath = Me.TabPath
            objTabInfo.StartDate = Me.StartDate
            objTabInfo.EndDate = Me.EndDate
            ' Me.TabPermissions = tab.TabPermissions changed in dnn 5, set up automatically in the base class property
            objTabInfo.HasChildren = Me.HasChildren
            objTabInfo.SkinPath = Me.SkinPath
            objTabInfo.ContainerPath = Me.ContainerPath
            objTabInfo.IsSuperTab = Me.IsSuperTab
            objTabInfo.RefreshInterval = Me.RefreshInterval
            objTabInfo.PageHeadText = Me.PageHeadText
            objTabInfo.IsSecure = Me.IsSecure
            objTabInfo.SkinDoctype = Me.SkinDoctype

            If Not Me.BreadCrumbs Is Nothing Then
                objTabInfo.BreadCrumbs = New ArrayList
                For Each t As TabInfo In Me.BreadCrumbs
                    objTabInfo.BreadCrumbs.Add(t.Clone())
                Next
            End If

            ' initialize collections which are populated later
            objTabInfo.Panes = New ArrayList
            objTabInfo.Modules = New ArrayList


            Return objTabInfo

        End Function

#End Region


        ''' <summary>
        ''' This sub is the implementation of IHydratable. It shadows IHydratable of the baseclass
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <remarks></remarks>
        ''' <history>
        '''     [erikvb]    16-01-2009  modified to support DNN 5
        ''' </history>
        Public Shadows Sub Fill(ByVal dr As IDataReader) Implements IHydratable.Fill
            Dim objTabPermissionController As New TabPermissionController
            itemID = Convert.ToInt32(Null.SetNull(dr("ItemId"), itemID))
            TabID = Convert.ToInt32(Null.SetNull(dr("TabID"), TabID))
            TabOrder = Convert.ToInt32(Null.SetNull(dr("TabOrder"), TabOrder))
            PortalID = Convert.ToInt32(Null.SetNull(dr("PortalID"), PortalID))
            defaultName = Convert.ToString(Null.SetNull(dr("DefaultName"), TabName))
            indentedDefName = Convert.ToString(Null.SetNull(dr("indentedDefName"), TabName))
            Locale = Convert.ToString(Null.SetNull(dr("Locale"), TabName))
            TabName = Convert.ToString(Null.SetNull(dr("TabName"), TabName))
            IsVisible = Convert.ToBoolean(Null.SetNull(dr("IsVisible"), IsVisible))
            ParentId = Convert.ToInt32(Null.SetNull(dr("ParentId"), ParentId))
            Level = Convert.ToInt32(Null.SetNull(dr("Level"), Level))
            IconFile = Convert.ToString(Null.SetNull(dr("IconFile"), IconFile))
            DisableLink = Convert.ToBoolean(Null.SetNull(dr("DisableLink"), DisableLink))
            Title = Convert.ToString(Null.SetNull(dr("Title"), Title))
            Description = Convert.ToString(Null.SetNull(dr("Description"), Description))
            KeyWords = Convert.ToString(Null.SetNull(dr("KeyWords"), KeyWords))
            IsDeleted = Convert.ToBoolean(Null.SetNull(dr("IsDeleted"), IsDeleted))
            SkinSrc = Convert.ToString(Null.SetNull(dr("SkinSrc"), SkinSrc))
            ContainerSrc = Convert.ToString(Null.SetNull(dr("ContainerSrc"), ContainerSrc))
            TabPath = Convert.ToString(Null.SetNull(dr("TabPath"), TabPath))
            StartDate = Convert.ToDateTime(Null.SetNull(dr("StartDate"), StartDate))
            EndDate = Convert.ToDateTime(Null.SetNull(dr("EndDate"), EndDate))
            Url = Convert.ToString(Null.SetNull(dr("Url"), Url))
            HasChildren = Convert.ToBoolean(Null.SetNull(dr("HasChildren"), HasChildren))

            RefreshInterval = Convert.ToInt32(Null.SetNull(dr("RefreshInterval"), RefreshInterval))
            PageHeadText = Convert.ToString(Null.SetNull(dr("PageHeadText"), PageHeadText))
            IsSecure = Convert.ToBoolean(Null.SetNull(dr("IsSecure"), IsSecure))

            'obsolete in dnn 5
            'TabPermissions = objTabPermissionController.GetTabPermissionsCollectionByTabID(TabID, PortalID)
            'AdministratorRoles = objTabPermissionController.GetTabPermissions(TabPermissions, "EDIT")
            'AuthorizedRoles = objTabPermissionController.GetTabPermissions(TabPermissions, "VIEW")

            BreadCrumbs = Nothing
            Panes = Nothing
            Modules = Nothing

        End Sub

        Public Shadows Property KeyID() As Integer Implements IHydratable.KeyID
            Get
                Return itemID
            End Get
            Set(ByVal value As Integer)
                itemID = value
            End Set
        End Property
    End Class
End Namespace
