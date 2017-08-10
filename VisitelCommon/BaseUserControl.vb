Imports System.Web.UI.HtmlControls
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Web.UI.WebControls

Namespace VisitelCommon

    Public Class BaseUserControl
        Inherits System.Web.UI.UserControl

        ''' <summary>
        ''' Loading javascript file
        ''' </summary>
        ''' <param name="fileName"></param>
        Protected Sub LoadJS(FileName As String)
            Dim myJs As New HtmlGenericControl()
            myJs.TagName = "script"
            myJs.Attributes.Add("type", "text/javascript")
            myJs.Attributes.Add("language", "javascript")
            myJs.Attributes.Add("src", ResolveUrl(Convert.ToString("~/") & FileName))

            Page.Header.Controls.Add(myJs)

        End Sub

        ''' <summary>
        ''' Loading css file
        ''' </summary>
        ''' <param name="fileName"></param>
        Protected Sub LoadCss(FileName As String)

            Dim css As New HtmlLink()
            'css.Href = ResolveClientUrl((Convert.ToString("~/Css/") & FileName) + ".css")
            css.Href = ResolveUrl((Convert.ToString("~/Css/") & FileName) + ".css")
            css.Attributes("type") = "text/css"
            css.Attributes("rel") = "stylesheet"
            css.Attributes("media") = "screen"

            Page.Header.Controls.Add(css)

        End Sub

        ''' <summary>
        ''' This is for initializing controls with respecting value
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="target"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

        Public Property GridViewSortDirection() As SortDirection
            Get
                If ViewState("sortDirection") Is Nothing Then
                    ViewState("sortDirection") = SortDirection.Ascending
                End If

                Return DirectCast(ViewState("sortDirection"), SortDirection)
            End Get
            Set(value As SortDirection)
                ViewState("sortDirection") = value
            End Set
        End Property

        Public Sub ClearDropDownSelectedIndex(ByRef DropDownListControl As DropDownList)

            DropDownListControl.SelectedIndex = If((DropDownListControl.Items.Count > 0), 0, DropDownListControl.SelectedIndex)

        End Sub

        Public Sub GridViewAllSelectCheckboxSelectChange(ByRef CheckboxControl As CheckBox, ByRef CurrentRow As GridViewRow, ByRef WorkingGridView As GridView,
                                                         ByRef CheckBoxSelect As CheckBox, SelectCheckboxControlName As String)

            For Each row As GridViewRow In WorkingGridView.Rows
                If row.RowType.Equals(DataControlRowType.DataRow) Then

                    'row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked

                    DirectCast(row.FindControl(SelectCheckboxControlName), CheckBox).Checked = CheckboxControl.Checked
                End If
            Next

            If (WorkingGridView.ShowFooter) Then
                CurrentRow = WorkingGridView.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl(SelectCheckboxControlName), CheckBox)
                CheckBoxSelect.Checked = CheckboxControl.Checked
            End If

        End Sub

        Public Sub GridViewControlsOnEdit(ByRef WorkingGridView As GridView, CheckAllCheckboxControlName As String, SelectCheckboxControlName As String)

            Dim chkAll As CheckBox = DirectCast(WorkingGridView.HeaderRow.FindControl(CheckAllCheckboxControlName), CheckBox)

            For Each row As GridViewRow In WorkingGridView.Rows
                If row.RowType.Equals(DataControlRowType.DataRow) Then

                    'Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    Dim isChecked As Boolean = DirectCast(row.FindControl(SelectCheckboxControlName), CheckBox).Checked

                    For i As Integer = 1 To row.Cells.Count - 1

                        If row.Cells(i).Controls.OfType(Of DropDownList)().ToList().Count > 0 Then
                            row.Cells(i).Controls.OfType(Of DropDownList)().FirstOrDefault().Visible = isChecked
                        End If

                        If Not isChecked Then
                            chkAll.Checked = False
                        End If
                    Next
                End If
            Next

        End Sub

        ''' <summary>
        ''' Making Button Active/InActive depending on Grid View Row selection
        ''' </summary>
        ''' <remarks></remarks>
        Public Function DetermineButtonInActivity(ByRef WorkingGridView As GridView, SelectCheckboxControlName As String) As Boolean

            Dim ItemChecked As Boolean = False

            Dim CurrentRow As GridViewRow
            Dim CheckBoxSelect As CheckBox

            For Each row As GridViewRow In WorkingGridView.Rows
                If row.RowType.Equals(DataControlRowType.DataRow) Then
                    ItemChecked = DirectCast(row.FindControl(SelectCheckboxControlName), CheckBox).Checked
                    If (ItemChecked) Then
                        Exit For
                    End If
                End If
            Next

            If (WorkingGridView.ShowFooter) Then
                If (Not ItemChecked) Then
                    If (WorkingGridView.Rows.Count > 0) Then
                        CurrentRow = WorkingGridView.FooterRow
                        CheckBoxSelect = DirectCast(CurrentRow.FindControl(SelectCheckboxControlName), CheckBox)
                        ItemChecked = CheckBoxSelect.Checked
                    End If
                End If
            End If

            Return ItemChecked

        End Function

        ''' <summary>
        ''' Secured FTP Operations
        ''' </summary>
        ''' <param name="FTPSiteIp"></param>
        ''' <param name="UserId"></param>
        ''' <param name="Password"></param>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SFTPOperations(FTPSiteIp As String, UserId As String, Password As String, ByRef SFTPRequestsParam As SFTPRequestsParam) As Boolean

            Dim IsSuccess As Boolean = False

            Dim SFTPClient As New SFTPRequests(String.Format(FTPSiteIp), UserId, Password) 'Create Object Instance 

            Try
                Select Case SFTPRequestsParam.FTPOperationType
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.UploadFile)
                        SFTPClient.UploadFile(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.DownloadFile)
                        SFTPClient.DownloadFile(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.DeleteFile)
                        SFTPClient.DeleteFile(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.RenameFile)
                        SFTPClient.RenameFile(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.CreateDirectory)
                        SFTPClient.CreateDirectory(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.GetFileCreatedDateTime)
                        SFTPClient.GetFileCreatedDateTime(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.GetFileSize)
                        SFTPClient.GetFileSize(SFTPRequestsParam)
                        Exit Select
                    Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.DirectoryListDetailed)
                        SFTPClient.ListDirectory(SFTPRequestsParam)
                        Exit Select
                End Select

                If (String.IsNullOrEmpty(SFTPRequestsParam.ErrorMessage)) Then
                    IsSuccess = True
                End If

            Catch ex As Exception

            Finally
                SFTPClient = Nothing
            End Try

            Return IsSuccess

        End Function


        ''' <summary>
        ''' Not Implemented
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FTPOperations()

            Dim RemotePath As String = String.Empty, RemoteFileName As String = String.Empty, LocalFileName As String = String.Empty

            'Dim FTPClient As New FTPRequests(String.Format("ftp://{0}", TextBoxFtpSite.Text.Trim()),
            '                                 DropDownListUserName.SelectedValue, TextBoxPassword.Text.Trim()) 'Create Object Instance 

            'Dim FTPClient As New FTPRequests(String.Format(TextBoxFtpSite.Text.Trim()),
            '                                 "TurboTest", TextBoxPassword.Text.Trim()) 'Create Object Instance 


            RemoteFileName = "Yourfile.txt"
            LocalFileName = "Yourfile.txt"

            RemotePath = "Inbound/"

            'FTPClient.UploadFile(String.Format(RemotePath & "{0}", RemoteFileName), String.Format(TextBoxSaveLocation.Text.Trim() & "{0}", LocalFileName)) 'Upload a File 

            'FTPClient.DownloadFile("etc/test.txt", "C:\Users\metastruct\Desktop\test.txt") 'Download a File 

            'FTPClient.DeleteFile("etc/test.txt") 'Delete a File 

            'FTPClient.RenameFile("etc/test.txt", "test2.txt") 'Rename a File 

            'FTPClient.CreateDirectory("etc/test") 'Create a New Directory 

            'Dim FileDateTime As String = FTPClient.GetFileCreatedDateTime("etc/test.txt") 'Get the Date/Time a File was Created 
            'Console.WriteLine(FileDateTime)

            'Dim FileSize As String = FTPClient.GetFileSize("etc/test.txt") 'Get the Size of a File 
            'Console.WriteLine(FileSize)

            'Get Contents of a Directory (Names Only)
            'Dim SimpleDirectoryListing As String() = FTPClient.DirectoryListDetailed("/etc")
            'For i As Integer = 0 To SimpleDirectoryListing.Count() - 1
            'Console.WriteLine(SimpleDirectoryListing(i))
            'Next

            'Get Contents of a Directory with Detailed File/Directory Info 
            'Dim DetailDirectoryListing As String() = FTPClient.DirectoryListDetailed("/etc")
            'For i As Integer = 0 To DetailDirectoryListing.Count() - 1
            'Console.WriteLine(DetailDirectoryListing(i))
            'Next

            'Release Resources 

            'FTPClient = Nothing

        End Sub

    End Class

End Namespace

