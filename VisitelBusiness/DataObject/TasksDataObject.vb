Namespace VisitelBusiness.DataObject
    Public Class TasksDataObject
        Public Property TaskId As Integer

        Public Property ClientId As Integer

        Public Property TaskBath As Boolean

        Public Property TaskDress As Boolean

        Public Property TaskExcercise As Boolean

        Public Property TaskFeeding As Boolean

        Public Property TaskGrooming As Boolean

        Public Property TaskLaundry As Boolean

        Public Property TaskToileting As Boolean

        Public Property TaskTranAmb As Boolean

        Public Property TaskAmbulation As Boolean

        Public Property TaskCleaning As Boolean

        Public Property TaskHairSkin As Boolean

        Public Property TaskMealPrep As Boolean

        Public Property TaskEscort As Boolean

        Public Property TaskShopping As Boolean

        Public Property TaskAsst As Boolean

        Public Property TaskOther As Boolean

        Public Property TaskOtherSpec As String

        Public Property TaskWalking As Boolean

        Public Property UpdateDate As DateTime

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Property SSMATimeStamp As TimeSpan

        Public Sub New()
            Me.TaskId = Int32.MinValue
            Me.ClientId = Int32.MinValue
            Me.TaskBath = False
            Me.TaskDress = False
            Me.TaskExcercise = False
            Me.TaskFeeding = False
            Me.TaskGrooming = False
            Me.TaskLaundry = False
            Me.TaskToileting = False
            Me.TaskTranAmb = False
            Me.TaskAmbulation = False
            Me.TaskCleaning = False
            Me.TaskHairSkin = False
            Me.TaskMealPrep = False
            Me.TaskEscort = False
            Me.TaskShopping = False
            Me.TaskAsst = False
            Me.TaskOther = False
            Me.TaskWalking = False
            Me.TaskOtherSpec = String.Empty
            Me.UpdateBy = String.Empty
            Me.UpdateDate = DateTime.MinValue
            Me.CompanyId = Me.UserId = Int32.MinValue
            Me.SSMATimeStamp = TimeSpan.MinValue
        End Sub
    End Class
End Namespace