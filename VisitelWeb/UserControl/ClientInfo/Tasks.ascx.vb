Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo
    Public Class TasksControl
        Inherits BaseUserControl

        Private ControlName As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, DeleteConfirmation As String, SocialSecurityNumber As String,
            StateClientId As String
        Private objTasksDataObject As New TasksDataObject
        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "TasksControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonSaveTasks_Click(sender As Object, e As EventArgs)

            If (objTasksDataObject.ClientId > 0) Then
                GetUpdatedTask()

                Dim objBLTasks As New BLTasks()
                Dim IsSaved As Boolean = False
                Dim ErrorMessage As String = String.Empty

                Try
                    If (Convert.ToBoolean(HiddenFieldTasksIsNew.Value, Nothing)) Then
                        ErrorMessage = "Unable to Insert Task"
                        objBLTasks.InsertTasks(objShared.ConVisitel, objTasksDataObject)
                        IsSaved = True
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                    Else
                        ErrorMessage = "Unable to Update Task"
                        objTasksDataObject.UpdateBy = Convert.ToString(objShared.UserId)
                        objBLTasks.UpdateTasks(objShared.ConVisitel, objTasksDataObject)
                        IsSaved = True
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                    End If
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ErrorMessage)
                End Try

                objBLTasks = Nothing

                If IsSaved Then
                    HiddenFieldTasksIsNew.Value = Convert.ToString(True, Nothing)
                    Session(StaticSettings.SessionField.TASKS_CLEAR_CLICKED) = Nothing
                    GetData()
                End If
            End If
        End Sub

        Private Sub ButtonDeleteTasks_Click(sender As Object, e As EventArgs)

            If (Convert.ToInt64(HiddenFieldTaskId.Value) < 0) Then
                Return
            End If

            Dim ErrorMessage As String = "Unable to Delete"

            Try
                Dim objBLTasks As New BLTasks()
                objTasksDataObject = New TasksDataObject()
                objTasksDataObject.TaskId = Convert.ToInt32(HiddenFieldTaskId.Value)
                objTasksDataObject.CompanyId = objShared.CompanyId
                objTasksDataObject.UserId = objShared.UserId

                objBLTasks.DeleteTasks(objShared.ConVisitel, objTasksDataObject)
                objBLTasks = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                GetData()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ErrorMessage)
            End Try

        End Sub

        Private Sub ButtonClearTasks_Click(sender As Object, e As EventArgs)
            ClearControls()
            Session(StaticSettings.SessionField.TASKS_CLEAR_CLICKED) = 1

            GetData()
        End Sub

        ''' <summary>
        ''' Initializing Controls
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitializeControl()

            HiddenFieldTaskId.ClientIDMode = ClientIDMode.Static

            ButtonClearTasks.ClientIDMode = ClientIDMode.Static
            ButtonSaveTasks.ClientIDMode = ClientIDMode.Static
            ButtonDeleteTasks.ClientIDMode = ClientIDMode.Static

            AddHandler ButtonClearTasks.Click, AddressOf ButtonClearTasks_Click
            AddHandler ButtonSaveTasks.Click, AddressOf ButtonSaveTasks_Click
            AddHandler ButtonDeleteTasks.Click, AddressOf ButtonDeleteTasks_Click

        End Sub

        ''' <summary>
        ''' Importing Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & "     var DeleteTargetButton1 ='ButtonDeleteTasks'; " _
                        & "     var DeleteDialogHeader1 ='Client Task'; " _
                        & "     var DeleteDialogConfirmMsg1 ='Do you want to delete?'; " _
                        & "     var prm =''; " _
                        & "     jQuery(document).ready(function () {" _
                        & "         prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "         TaskDataDelete();" _
                        & "         prm.add_endRequest(TaskDataDelete); " _
                        & "     }); " _
                        & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/ClientInfo/" & ControlName & ".js")

        End Sub

        Public Sub SetClientId(ByVal clientId As Integer)
            objTasksDataObject = New TasksDataObject()
            objTasksDataObject.ClientId = clientId
            objTasksDataObject.CompanyId = objShared.CompanyId
            objTasksDataObject.UserId = objShared.UserId
            HiddenFieldTaskId.Value = clientId
        End Sub

        ''' <summary>
        ''' Setting StateClientId from Parent Page
        ''' </summary>
        ''' <param name="StateClientId"></param>
        ''' <remarks></remarks>
        Public Sub SetStateClientId(StateClientId As String)
            Me.StateClientId = StateClientId
        End Sub

        ''' <summary>
        ''' Setting SocialSecurityNumber from Parent Page
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <remarks></remarks>
        Public Sub SetSocialSecurityNumber(SocialSecurityNumber As String)
            Me.SocialSecurityNumber = SocialSecurityNumber
        End Sub

        Public Sub LoadTasks()
            GetData()
        End Sub

        Private Sub GetUpdatedTask()
            objTasksDataObject.TaskId = Convert.ToInt32(HiddenFieldTaskId.Value)
            objTasksDataObject.CompanyId = objShared.CompanyId
            objTasksDataObject.UserId = objShared.UserId

            objTasksDataObject.TaskBath = CheckBathing.Checked
            objTasksDataObject.TaskLaundry = CheckLaundry.Checked
            objTasksDataObject.TaskMealPrep = CheckMealPreparation.Checked
            objTasksDataObject.TaskDress = CheckDressing.Checked

            objTasksDataObject.TaskToileting = CheckToileting.Checked
            objTasksDataObject.TaskEscort = CheckEscort.Checked
            objTasksDataObject.TaskExcercise = CheckExcercisng.Checked
            objTasksDataObject.TaskTranAmb = CheckTransfer.Checked

            objTasksDataObject.TaskAmbulation = CheckAmbulation.Checked
            objTasksDataObject.TaskShopping = CheckShopping.Checked
            objTasksDataObject.TaskFeeding = CheckFeeding.Checked
            objTasksDataObject.TaskCleaning = CheckCleaning.Checked

            objTasksDataObject.TaskAsst = CheckAsst.Checked
            objTasksDataObject.TaskGrooming = CheckGrooming.Checked
            objTasksDataObject.TaskHairSkin = CheckHairSkin.Checked
            objTasksDataObject.TaskWalking = CheckWalking.Checked
            objTasksDataObject.TaskOther = CheckOther.Checked

            objTasksDataObject.TaskOtherSpec = TextOtherSpec.Text
            objTasksDataObject.UpdateBy = TextTasksUpdateBy.Text
        End Sub

        Private Sub GetCaptionFromResource()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", "TasksControl" & Convert.ToString(".resx"))

            LabelTasks.Text = Convert.ToString(ResourceTable("LabelTasks"), Nothing)
            LabelTasks.Text = If(LabelTasks.Text.Trim().Equals(String.Empty), "Tasks", LabelTasks.Text)

            CheckBathing.Text = Convert.ToString(ResourceTable("CheckBathing"), Nothing)
            CheckBathing.Text = If(CheckBathing.Text.Trim().Equals(String.Empty), "Bathing", CheckBathing.Text)

            CheckLaundry.Text = Convert.ToString(ResourceTable("CheckLaundry"), Nothing)
            CheckLaundry.Text = If(CheckLaundry.Text.Trim().Equals(String.Empty), "Laundry", CheckLaundry.Text)

            CheckMealPreparation.Text = Convert.ToString(ResourceTable("CheckMealPreparation"), Nothing)
            CheckMealPreparation.Text = If(CheckMealPreparation.Text.Trim().Equals(String.Empty), "Meal Preparation", CheckMealPreparation.Text)

            CheckDressing.Text = Convert.ToString(ResourceTable("CheckDressing"), Nothing)
            CheckDressing.Text = If(CheckDressing.Text.Trim().Equals(String.Empty), "Dressing", CheckDressing.Text)

            CheckToileting.Text = Convert.ToString(ResourceTable("CheckToileting"), Nothing)
            CheckToileting.Text = If(CheckToileting.Text.Trim().Equals(String.Empty), "Toileting", CheckToileting.Text)

            CheckEscort.Text = Convert.ToString(ResourceTable("CheckEscort"), Nothing)
            CheckEscort.Text = If(CheckEscort.Text.Trim().Equals(String.Empty), "Escort", CheckEscort.Text)

            CheckExcercisng.Text = Convert.ToString(ResourceTable("CheckExcercisng"), Nothing)
            CheckExcercisng.Text = If(CheckExcercisng.Text.Trim().Equals(String.Empty), "Excercisng", CheckExcercisng.Text)

            CheckTransfer.Text = Convert.ToString(ResourceTable("CheckTransfer"), Nothing)
            CheckTransfer.Text = If(CheckTransfer.Text.Trim().Equals(String.Empty), "Transfer", CheckTransfer.Text)

            CheckAmbulation.Text = Convert.ToString(ResourceTable("CheckAmbulation"), Nothing)
            CheckAmbulation.Text = If(CheckAmbulation.Text.Trim().Equals(String.Empty), "Ambulation", CheckAmbulation.Text)

            CheckShopping.Text = Convert.ToString(ResourceTable("CheckShopping"), Nothing)
            CheckShopping.Text = If(CheckShopping.Text.Trim().Equals(String.Empty), "Shopping", CheckShopping.Text)

            CheckFeeding.Text = Convert.ToString(ResourceTable("CheckFeeding"), Nothing)
            CheckFeeding.Text = If(CheckFeeding.Text.Trim().Equals(String.Empty), "Feeding", CheckFeeding.Text)

            CheckCleaning.Text = Convert.ToString(ResourceTable("CheckCleaning"), Nothing)
            CheckCleaning.Text = If(CheckCleaning.Text.Trim().Equals(String.Empty), "Cleaning", CheckCleaning.Text)

            CheckAsst.Text = Convert.ToString(ResourceTable("CheckAsst"), Nothing)
            CheckAsst.Text = If(CheckAsst.Text.Trim().Equals(String.Empty), "Asst. with Self-Administered Medications", CheckAsst.Text)

            CheckGrooming.Text = Convert.ToString(ResourceTable("CheckGrooming"), Nothing)
            CheckGrooming.Text = If(CheckGrooming.Text.Trim().Equals(String.Empty), "Shaving/Oral Care", CheckGrooming.Text)

            CheckHairSkin.Text = Convert.ToString(ResourceTable("CheckHairSkin"), Nothing)
            CheckHairSkin.Text = If(CheckHairSkin.Text.Trim().Equals(String.Empty), "Routine Hair/Skin Care", CheckHairSkin.Text)

            CheckWalking.Text = Convert.ToString(ResourceTable("CheckWalking"), Nothing)
            CheckWalking.Text = If(CheckWalking.Text.Trim().Equals(String.Empty), "Walking", CheckWalking.Text)

            CheckOther.Text = Convert.ToString(ResourceTable("CheckOther"), Nothing)
            CheckOther.Text = If(CheckOther.Text.Trim().Equals(String.Empty), "Other (Specify):", CheckOther.Text)

            LabelTasksUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelTasksUpdateBy.Text = If(LabelTasksUpdateBy.Text.Trim().Equals(String.Empty), "Update By", LabelTasksUpdateBy.Text)

            LabelTasksUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelTasksUpdateDate.Text = If(LabelTasksUpdateDate.Text.Trim().Equals(String.Empty), "Update Date", LabelTasksUpdateDate.Text)

            ButtonSaveTasks.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSaveTasks.Text = If(ButtonSaveTasks.Text.Trim().Equals(String.Empty), "Save", ButtonSaveTasks.Text)

            ButtonDeleteTasks.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDeleteTasks.Text = If(ButtonDeleteTasks.Text.Trim().Equals(String.Empty), "Delete", ButtonDeleteTasks.Text)

            ButtonClearTasks.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClearTasks.Text = If(ButtonClearTasks.Text.Trim().Equals(String.Empty), "Clear", ButtonClearTasks.Text)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            InsertMessage = If(InsertMessage.Trim().Equals(String.Empty), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            UpdateMessage = If(UpdateMessage.Trim().Equals(String.Empty), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(DeleteMessage.Trim().Equals(String.Empty), "Deleted Successfully", DeleteMessage)

            ResourceTable = Nothing

            'ButtonDeleteTasks.Attributes("onClick") = String.Format("return TaskDeleteConfirmation('Do you want to delete? ');")

            TextTasksUpdateBy.ReadOnly = True
            TextTasksUpdateDate.ReadOnly = True

        End Sub

        Private Sub GetData()

            If objTasksDataObject.ClientId > 0 And (Session(StaticSettings.SessionField.TASKS_CLEAR_CLICKED) Is Nothing Or Not Session(StaticSettings.SessionField.TASKS_CLEAR_CLICKED) = 1) Then
                Dim objBLTasks As New BLTasks()
                objBLTasks.SelectTasks(objShared.ConVisitel, objTasksDataObject)
                objBLTasks = Nothing
            End If

            If ((Session(StaticSettings.SessionField.TASKS_CLEAR_CLICKED) Is Nothing Or Not Session(StaticSettings.SessionField.TASKS_CLEAR_CLICKED) = 1)) Then
                HiddenFieldTaskId.Value = Integer.MinValue.ToString()
                HiddenFieldTasksIsNew.Value = Convert.ToString(True, Nothing)
                ClearControls()
            End If

            If (objTasksDataObject.TaskId > 0) Then
                CheckBathing.Checked = objTasksDataObject.TaskBath
                CheckLaundry.Checked = objTasksDataObject.TaskLaundry
                CheckMealPreparation.Checked = objTasksDataObject.TaskMealPrep
                CheckDressing.Checked = objTasksDataObject.TaskDress

                CheckToileting.Checked = objTasksDataObject.TaskToileting
                CheckEscort.Checked = objTasksDataObject.TaskEscort
                CheckExcercisng.Checked = objTasksDataObject.TaskExcercise
                CheckTransfer.Checked = objTasksDataObject.TaskTranAmb

                CheckAmbulation.Checked = objTasksDataObject.TaskAmbulation
                CheckShopping.Checked = objTasksDataObject.TaskShopping
                CheckFeeding.Checked = objTasksDataObject.TaskFeeding
                CheckCleaning.Checked = objTasksDataObject.TaskCleaning

                CheckAsst.Checked = objTasksDataObject.TaskAsst
                CheckGrooming.Checked = objTasksDataObject.TaskGrooming
                CheckHairSkin.Checked = objTasksDataObject.TaskHairSkin
                CheckWalking.Checked = objTasksDataObject.TaskWalking

                CheckOther.Checked = objTasksDataObject.TaskOther
                TextOtherSpec.Text = objTasksDataObject.TaskOtherSpec
                TextTasksUpdateBy.Text = objTasksDataObject.UpdateBy
                TextTasksUpdateDate.Text = objTasksDataObject.UpdateDate.ToString(objShared.DateFormat)

                HiddenFieldTaskId.Value = objTasksDataObject.TaskId.ToString()
                HiddenFieldTasksIsNew.Value = Convert.ToString(False, Nothing)
            End If

            TextOtherSpec.ReadOnly = (Not CheckOther.Checked)

        End Sub

        Private Sub ClearControls()
            CheckBathing.Checked = False
            CheckLaundry.Checked = False
            CheckMealPreparation.Checked = False
            CheckDressing.Checked = False
            CheckToileting.Checked = False
            CheckEscort.Checked = False
            CheckExcercisng.Checked = False
            CheckTransfer.Checked = False
            CheckAmbulation.Checked = False
            CheckShopping.Checked = False
            CheckFeeding.Checked = False
            CheckCleaning.Checked = False
            CheckAsst.Checked = False
            CheckGrooming.Checked = False
            CheckHairSkin.Checked = False
            CheckWalking.Checked = False
            CheckOther.Checked = False
            TextOtherSpec.Text = String.Empty
            TextTasksUpdateBy.Text = String.Empty
            TextTasksUpdateDate.Text = String.Empty
        End Sub

    End Class
End Namespace