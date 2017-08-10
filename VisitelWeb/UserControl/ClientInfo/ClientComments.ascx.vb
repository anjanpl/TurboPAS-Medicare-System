Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo
    Public Class ClientCommentsControl
        Inherits BaseUserControl

        Private ControlName As String
        Private objShared As SharedWebControls
        Private objComment As New ClientCommentsDataObject()
       
        Private LabelCommentIdText As String, LabelDateText As String, LabelCommentText As String, LabelUpdateByText As String, LabelUpdateDateText As String,
            InsertMessage As String, UpdateMessage As String, DeleteMessage As String, SocialSecurityNumber As String, StateClientId As String
        Private RowCount As Integer

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            ControlName = "ClientCommentControl"
            LabelTitle.Text = "Comments"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            InitializeControl()
            GetCaptionFromResource()
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
            LoadJavaScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub LoadJavaScript()

            LoadCommentJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var DeleteTargetButton4 ='ButtonDeleteComment'; " _
                        & " var DeleteDialogHeader4 ='Client Comment'; " _
                        & " var DeleteDialogConfirmMsg4 ='Do you want to delete?'; " _
                        & " var prm =''; " _
                        & " jQuery(document).ready(function () {" _
                        & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "     CommentsDataDelete();" _
                        & "     prm.add_endRequest(CommentsDataDelete); " _
                        & " }); " _
                 & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/ClientInfo/" & ControlName & ".js")
            DynamicJavascript(Page)
        End Sub


        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            HiddenFieldCommentId.ClientIDMode = ClientIDMode.Static
            HiddenFieldCommentIsNew.ClientIDMode = ClientIDMode.Static
            HiddenFieldCommentIndex.ClientIDMode = ClientIDMode.Static

            ButtonClearComment.ClientIDMode = ClientIDMode.Static
            ButtonCommunicationNotesComment.ClientIDMode = ClientIDMode.Static
            ButtonSaveComment.ClientIDMode = ClientIDMode.Static
            ButtonDeleteComment.ClientIDMode = ClientIDMode.Static

            AddHandler ButtonSaveComment.Click, AddressOf ButtonSaveComment_Click
            AddHandler ButtonDeleteComment.Click, AddressOf ButtonDeleteComment_Click
            AddHandler ButtonClearComment.Click, AddressOf ButtonClearComment_Click
        End Sub

        Public Sub LoadCommentJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var ClientCommentRowCounter = " & RowCount & "; " _
                        & " var CompanyId = " & objShared.CompanyId & "; " _
                        & " var ReportsViewPath ='" & objShared.GetPopupUrl("Reports/ReportsView.aspx") & "'; " _
                        & " var CommentsCalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                        & " var CommentsCalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                        & " var prm =''; " _
                        & " jQuery(document).ready(function () {" _
                        & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "     ClientCommentDateFieldsEvent();" _
                        & "     prm.add_endRequest(ClientCommentDateFieldsEvent); " _
                        & " }); " _
                 & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

        End Sub

        Private Sub DynamicJavascript(ByRef Control As Control)
            ScriptManager.RegisterClientScriptBlock(Control, Me.GetType(), "DateFields", "ClientCommentRowCounter = " & RowCount & ";ClientCommentDateFieldsEvent();", True)
        End Sub

        Private Sub ButtonSaveComment_Click(sender As Object, e As EventArgs)
            Dim isValid As Boolean
            Dim comments As New List(Of ClientCommentsDataObject)()

            isValid = ValidateAllComments(comments)

            If isValid Then
                Dim objBLComments As New BLClientComments()
                Dim IsSaved As Boolean = False
                Dim SaveErrorMessage As String = String.Empty
                Dim itemNo As Integer = 0

                For Each comment As ClientCommentsDataObject In comments
                    Try
                        If (comment.CommentId = Integer.MinValue) Then
                            SaveErrorMessage = "Unable to Insert New Comment"
                            objBLComments.InsertComment(objShared.ConVisitel, comment)
                            IsSaved = True
                        Else
                            SaveErrorMessage = "Unable to Update Comment with id " & comment.CommentId
                            comment.UpdateBy = Convert.ToString(objShared.UserId)
                            objBLComments.UpdateComment(objShared.ConVisitel, comment)
                            IsSaved = True
                        End If
                    Catch ex As SqlException
                        Exit For
                    End Try
                Next

                objBLComments = Nothing

                If IsSaved Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("All Comment Saved Successfully")
                    SetHiddenFields()
                    Session(StaticSettings.SessionField.COMMENT_CLEAR_CLICKED) = Nothing
                    GetData()
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveErrorMessage)
                End If
            Else
                Page.Validate()
            End If
        End Sub

        Private Sub ButtonDeleteComment_Click(sender As Object, e As EventArgs)

            If (Convert.ToInt64(HiddenFieldCommentId.Value) < 0) Then
                Return
            End If

            Dim ErrorMessage As String = "Unable to Delete"

            Try
                Dim comment As New ClientCommentsDataObject()
                comment.CommentId = Convert.ToInt32(HiddenFieldCommentId.Value)

                Dim index As Integer
                index = Convert.ToInt32(HiddenFieldCommentIndex.Value)

                Dim divComment As HtmlGenericControl
                divComment = MainDiv.FindControl("DivComment" & index)

                comment.ClientId = objComment.ClientId
                comment.CompanyId = objShared.CompanyId
                comment.UserId = objShared.UserId

                If comment.CommentId > 0 Then
                    Dim objBLComments As New BLClientComments()
                    objBLComments.DeleteComment(objShared.ConVisitel, comment)
                    objBLComments = Nothing

                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                    GetData()
                End If

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ErrorMessage)
            End Try
        End Sub

        Private Sub ButtonClearComment_Click(sender As Object, e As EventArgs)
            ClearControls()
            SetHiddenFields()
            Session(StaticSettings.SessionField.COMMENT_CLEAR_CLICKED) = 1

            GetData()
        End Sub

        Public Sub SetClientId(ByVal clientId As Integer)
            objComment = New ClientCommentsDataObject()
            objComment.ClientId = clientId
            objComment.CompanyId = objShared.CompanyId
            objComment.UserId = objShared.UserId
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

        Public Sub LoadComments()
            GetData()
        End Sub

        Private Function ValidateAllComments(ByRef comments As List(Of ClientCommentsDataObject))

            Dim isValid As Boolean = True

            For index As Integer = 0 To RowCount
                Dim comment As New ClientCommentsDataObject()
                comment.CompanyId = objShared.CompanyId
                comment.UserId = objShared.UserId

                Dim textValue As String

                Dim divComment As HtmlGenericControl
                divComment = MainDiv.FindControl("DivComment" & index)

                textValue = objShared.GetTextBoxValue(index, divComment, "TextComment").ToString().Trim()

                If String.IsNullOrEmpty(textValue) Then
                    objShared.EnableRequiredFieldValidator(index, divComment, "rfvComment", True)
                    isValid = False
                Else
                    comment.Comment = textValue
                End If

                If (isValid Or index = RowCount) Then
                    GetUpdatedComment(comment, divComment, index)

                    If index = RowCount Then
                        If (String.IsNullOrEmpty(comment.Comment)) Then

                            isValid = True
                        Else
                            isValid = isValid
                            If isValid Then
                                comments.Add(comment)
                            End If
                        End If
                    Else
                        comments.Add(comment)
                    End If
                Else
                    Exit For
                End If
            Next

            ValidateAllComments = isValid
        End Function

        Private Sub GetUpdatedComment(ByRef comment As ClientCommentsDataObject, ByVal containerControl As HtmlGenericControl, ByVal index As Integer)
            Dim textValue As Object

            textValue = objShared.GetTextBoxValue(index, containerControl, "TextCommentId")
            comment.CommentId = If(String.IsNullOrEmpty(textValue.ToString()), Integer.MinValue, Convert.ToInt32(textValue, Nothing))
            comment.ClientId = Convert.ToInt32(objShared.GetHiddenFieldValue(index, containerControl, "HiddenCommentClientId"))

            textValue = objShared.GetTextBoxValue(index, containerControl, "TextCommentDate")
            comment.CommentDate = If(String.IsNullOrEmpty(textValue), DateTime.Now, Convert.ToDateTime(textValue, Nothing))
            comment.Comment = objShared.GetTextBoxValue(index, containerControl, "TextComment")
        End Sub

        Private Sub GetData()

            Dim comments As New List(Of ClientCommentsDataObject)()

            If objComment.ClientId > 0 And (Session(StaticSettings.SessionField.COMMENT_CLEAR_CLICKED) Is Nothing Or Not Session(StaticSettings.SessionField.COMMENT_CLEAR_CLICKED) = 1) Then
                Dim objBLComments As New BLClientComments()
                comments = objBLComments.SelectComments(objShared.ConVisitel, objComment)
                objBLComments = Nothing
            End If

            RowCount = comments.Count
            ClearControls()

            RowCount = 0
            For Each item As ClientCommentsDataObject In comments
                CreateControls(item, RowCount)
                RowCount = RowCount + 1
            Next

            CreateBlankComment()
            RowCount = comments.Count

        End Sub

        Private Sub CreateBlankComment()
            Dim blankComment As New ClientCommentsDataObject()
            blankComment.ClientId = objComment.ClientId
            CreateControls(blankComment, RowCount)
        End Sub

        Private Sub CreateControls(comment As ClientCommentsDataObject, index As Integer)

            Dim columnIdCss As String = "column150 columnLeft", columnDateCss As String = "column150 columnLeft",
                columnCommentCss As String = "column850 columnLeft", columnUpdatedDateCss As String = "column100 columnLeft",
                columnUpdatedByCss As String = "column150 columnLeft", autoColumnCss As String = "columnAuto columnLeft",
                columnFirstCss As String = "column080 columnLeft",
                textIdCss As String = "width100", textDateCss As String = "dateField", textCommentCss As String = "width850",
                textUpdatedDateCss As String = "width100", textUpdatedByCss As String = "width150"

            Dim CommentBox As New HtmlGenericControl(objShared.GenericControlRow)
            CommentBox.Attributes.Add("class", "CommentServiceBox")
            MainDiv.Controls.Add(CommentBox)

            Dim UpdatePanelComment As New UpdatePanel()
            UpdatePanelComment.ID = "UpdatePanelComment" & index
            UpdatePanelComment.UpdateMode = UpdatePanelUpdateMode.Always
            UpdatePanelComment.Attributes.Add("OnClick", "SetCommentId(" & index & ")")
            CommentBox.Controls.Add(UpdatePanelComment)

            Dim ServiceBoxComment As New HtmlGenericControl(objShared.GenericControlRow)
            ServiceBoxComment.ID = "DivComment" & index
            ServiceBoxComment.Attributes.Add("class", "CommentBoxStyle ServiceFullComment ServiceBoxComment")
            UpdatePanelComment.ContentTemplateContainer.Controls.Add(ServiceBoxComment)

            Dim parentRow As HtmlGenericControl
            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")

            Dim labelControl As Label
            Dim textControl As TextBox
            Dim commentValue As String

            labelControl = objShared.AddLabel(index, parentRow, "LabelCommentId", "", True, autoColumnCss)
            labelControl.Text = LabelCommentIdText

            commentValue = If(comment.CommentId = Integer.MinValue, String.Empty, comment.CommentId.ToString())
            textControl = objShared.AddTextBox(index, parentRow, "TextCommentId", textIdCss, True, columnIdCss, False, Nothing, Nothing, "", False)
            textControl.Text = commentValue
            textControl.ClientIDMode = UI.ClientIDMode.Static
            textControl.ReadOnly = True

            labelControl = objShared.AddLabel(index, parentRow, "LabelCommentDate", "", True, autoColumnCss)
            labelControl.Text = LabelDateText

            'commentValue = If(comment.CommentDate = DateTime.MinValue, String.Empty, comment.CommentDate.ToString(objShared.DateFormatWithTime12Hour))
            commentValue = comment.CommentDate
            textControl = objShared.AddTextBox(index, parentRow, "TextCommentDate", textDateCss, True, columnDateCss, False, Nothing, Nothing, "", False)
            textControl.Text = commentValue
            textControl.ReadOnly = True
            textControl.ClientIDMode = ClientIDMode.Static

            ServiceBoxComment.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")

            labelControl = objShared.AddLabel(index, parentRow, "LabelComment", "", True, columnFirstCss)
            labelControl.Text = LabelCommentText

            Dim rfvComment As RequiredFieldValidator = Nothing
            textControl = objShared.AddTextBox(index, parentRow, "TextComment", textCommentCss & " whiteSpaceNormal", True, columnCommentCss, True, rfvComment, "rfvComment", "", False)
            textControl.Text = comment.Comment
            textControl.MaxLength = 8000
            textControl.TextMode = TextBoxMode.MultiLine
            textControl.Rows = 3

            ServiceBoxComment.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")

            labelControl = objShared.AddLabel(index, parentRow, "LabelUpdateBy", "", True, columnFirstCss)
            labelControl.Text = LabelUpdateByText

            textControl = objShared.AddTextBox(index, parentRow, "TextUpdateBy", textUpdatedByCss, True, columnUpdatedByCss, False, Nothing, Nothing, "", False)
            textControl.Text = comment.UpdateBy
            textControl.ReadOnly = True

            parentRow.Controls.Add(objShared.AddColumn(index, "", autoColumnCss, "&nbsp;"))

            labelControl = objShared.AddLabel(index, parentRow, "LabelUpdateDate", "", True, columnFirstCss)
            labelControl.Text = LabelUpdateDateText

            commentValue = comment.UpdateDate
            textControl = objShared.AddTextBox(index, parentRow, "TextUpdateDate", textUpdatedDateCss, True, columnUpdatedDateCss, False, Nothing, Nothing, "", False)
            textControl.Text = commentValue
            textControl.ReadOnly = True

            ServiceBoxComment.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            Dim divSpace As New HtmlGenericControl(objShared.GenericControlRow)
            divSpace.Attributes.Add("class", "divSpace03")
            parentRow.Controls.Add(divSpace)

            Dim hiddenClientId As HiddenField = objShared.AddHiddenField(index, parentRow, "HiddenCommentClientId", False, "")
            hiddenClientId.Value = comment.ClientId.ToString()
            parentRow.Controls.Add(hiddenClientId)

            ServiceBoxComment.Controls.Add(parentRow)

        End Sub

        Public Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", "ClientCommentsControl" & Convert.ToString(".resx"))

            LabelCommentIdText = Convert.ToString(ResourceTable("LabelCommentId"), Nothing)
            LabelCommentIdText = If(LabelCommentIdText.Trim().Equals(String.Empty), "Id", LabelCommentIdText)

            LabelDateText = Convert.ToString(ResourceTable("LabelDate"), Nothing)
            LabelDateText = If(LabelDateText.Trim().Equals(String.Empty), "Date", LabelDateText)

            LabelCommentText = Convert.ToString(ResourceTable("LabelComment"), Nothing)
            LabelCommentText = If(LabelCommentText.Trim().Equals(String.Empty), "Comments", LabelCommentText)

            LabelUpdateByText = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelUpdateByText = If(LabelUpdateByText.Trim().Equals(String.Empty), "Update By", LabelUpdateByText)

            LabelUpdateDateText = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelUpdateDateText = If(LabelUpdateDateText.Trim().Equals(String.Empty), "Update Date", LabelUpdateDateText)

            ButtonSaveComment.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSaveComment.Text = If(ButtonSaveComment.Text.Trim().Equals(String.Empty), "Save", ButtonSaveComment.Text)

            ButtonDeleteComment.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDeleteComment.Text = If(ButtonDeleteComment.Text.Trim().Equals(String.Empty), "Delete", ButtonDeleteComment.Text)

            ButtonClearComment.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClearComment.Text = If(ButtonClearComment.Text.Trim().Equals(String.Empty), "Clear", ButtonClearComment.Text)

            ButtonCommunicationNotesComment.Text = Convert.ToString(ResourceTable("ButtonCommunicationNotes"), Nothing)
            ButtonCommunicationNotesComment.Text = If(ButtonCommunicationNotesComment.Text.Trim().Equals(String.Empty),
                                                      "Communication Notes", ButtonCommunicationNotesComment.Text)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            InsertMessage = If(InsertMessage.Trim().Equals(String.Empty), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            UpdateMessage = If(UpdateMessage.Trim().Equals(String.Empty), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(DeleteMessage.Trim().Equals(String.Empty), "Deleted Successfully", DeleteMessage)

            ResourceTable = Nothing

        End Sub

        Private Sub ClearControls()
            MainDiv.Controls.Clear()

            If Not IsPostBack Then
                SetHiddenFields()
            End If

        End Sub

        Private Sub SetHiddenFields()
            HiddenFieldCommentId.Value = Convert.ToString(Integer.MinValue)
            HiddenFieldCommentIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldCommentIndex.Value = Convert.ToString(RowCount)
        End Sub
    End Class
End Namespace