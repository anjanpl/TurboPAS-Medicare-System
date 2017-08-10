Imports System.ComponentModel

Namespace VisitelBusiness.DataObject
    Public Class CityDataObject
        <DataObjectField(True, True, False, True)>
        Public Property CityId As Integer

        Public Property CityName As String

        Public Sub New()
            Me.CityId = Int32.MinValue
            Me.CityName = String.Empty
        End Sub
    End Class
End Namespace

