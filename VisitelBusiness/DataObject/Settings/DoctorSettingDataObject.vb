Namespace VisitelBusiness.DataObject.Settings
    Public Class DoctorSettingDataObject
        Inherits BaseDataObject

        Public Property DoctorId As Integer

        Public Property UPinNumber As String

        Public Property LicNumber As String

        Public Property FirstName As String

        Public Property LastName As String

        Public Property Address As String

        Public Property Suite As String

        Public Property CityId As Integer

        Public Property CityName As String

        Private m_StateId As String

        Public Property StateId() As Integer
            Get
                Return m_StateId
            End Get
            Set(value As Integer)
                m_StateId = value
            End Set
        End Property

        Public Property StateFullName As String

        Public Property Zip As String

        Public Property Phone As String

        Public Property Fax As String

        Public Property Status As Int16

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.DoctorId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, InlineAssignHelper(Me.CityId, InlineAssignHelper(Me.StateId, Int32.MinValue))))
            Me.UPinNumber = InlineAssignHelper(Me.LicNumber, InlineAssignHelper(Me.FirstName, InlineAssignHelper(Me.LastName,
                            InlineAssignHelper(Me.Address, InlineAssignHelper(Me.Suite, InlineAssignHelper(Me.StateFullName,
                            InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.Phone, InlineAssignHelper(Me.Fax,
                            InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.CityName, String.Empty))))))))))))

            Me.Status = Int16.MinValue
        End Sub
    End Class
End Namespace

