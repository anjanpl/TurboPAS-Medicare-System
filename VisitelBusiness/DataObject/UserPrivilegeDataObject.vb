Imports System.Xml.Serialization

Namespace VisitelBusiness.DataObject

    <Serializable()> _
    <XmlType("UserPrivilegeDataObject")> _
    Public Class UserPrivilegeDataObject
        Inherits BaseDataObject

        Private m_UserId As Int64
        Public Property UserId() As Int64
            Get
                Return m_UserId
            End Get
            Set(value As Int64)
                m_UserId = value
            End Set
        End Property

        Private m_PermissionId As Int32
        Public Property PermissionId() As Int32
            Get
                Return m_PermissionId
            End Get
            Set(value As Int32)
                m_PermissionId = value
            End Set
        End Property

        Private m_HasPermission As Boolean
        Public Property HasPermission() As Boolean
            Get
                Return m_HasPermission
            End Get
            Set(value As Boolean)
                m_HasPermission = value
            End Set
        End Property

        Private m_RoleId As Int32
        Public Property RoleId() As Int32
            Get
                Return m_RoleId
            End Get
            Set(value As Int32)
                m_RoleId = value
            End Set
        End Property

        Private m_PermissionName As [String]
        Public Property PermissionName() As [String]
            Get
                Return m_PermissionName
            End Get
            Set(value As [String])
                m_PermissionName = value
            End Set
        End Property

        Private m_ModuleName As [String]
        Public Property ModuleName() As [String]
            Get
                Return m_ModuleName
            End Get
            Set(value As [String])
                m_ModuleName = value
            End Set
        End Property

        Private m_ViewOrder As Int32
        Public Property ViewOrder() As Int32
            Get
                Return m_ViewOrder
            End Get
            Set(value As Int32)
                m_ViewOrder = value
            End Set
        End Property

        Public Sub New()
            Me.UserId = Int64.MinValue
            Me.PermissionId = InlineAssignHelper(Me.RoleId, InlineAssignHelper(Me.ViewOrder, Int32.MinValue))
            Me.HasPermission = False
            Me.PermissionName = InlineAssignHelper(Me.ModuleName, String.Empty)
        End Sub

    End Class

End Namespace
