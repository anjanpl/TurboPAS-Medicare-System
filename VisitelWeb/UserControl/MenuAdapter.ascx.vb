
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Menu Generating
' Author: Anjan Kumar Paul
' Start Date: 18 Jan 2015
' End Date: 18 Jan 2015
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                18 Jan 2015      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Text
Imports System.Collections.Generic
Imports VisitelBusiness
Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl
    Public Class MenuAdapter
        Inherits System.Web.UI.UserControl

        Protected strMenu As String = String.Empty

        Private strbMenu As StringBuilder
        Private hasChildLink As Boolean = False
        Private HelpSect As SortedList(Of String, Integer) = Nothing
        Private MenuList As List(Of MenuDataObject)

        Private objShared As SharedWebControls
        Private ConVisitel As SqlConnection

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            ConVisitel = objShared.ConVisitel
            strbMenu = New StringBuilder()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            Dim objBlMenu As New BLMenu()

            'If Session(StaticSettings.SessionField.MENU_SOURCE) Is Nothing Then

            MenuList = objBlMenu.SelectMenuInfo(ConVisitel, Convert.ToInt32(Session(StaticSettings.SessionField.USER_ID)))

            strbMenu.Append("<ul class=""sf-menu"">")
            DrawMenu()
            strbMenu.Append("</ul>")
            strMenu = strbMenu.ToString()
            Session(StaticSettings.SessionField.MENU_SOURCE) = strMenu
            'Else
            'strMenu = DirectCast(Session(StaticSettings.SessionField.MENU_SOURCE), [String])
            'End If
        End Sub

        ''' <summary>
        ''' Drawing Parent Menu
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DrawMenu()

            Dim strMenuText As String, strMenuUrl As String, curURL As String
            Dim intID As Integer

            Dim MenuDataList As List(Of MenuDataObject) = (From p In MenuList Where p.ParentId = -1).ToList()

            Dim IsForwardSlash As Boolean = False

            For Each Menu In MenuDataList

                strMenuText = Menu.MenuText
                strMenuUrl = Menu.NavigateUrl
                intID = Menu.MenuId
                curURL = Request.FilePath

                Dim SubMenuDataList As List(Of MenuDataObject) = (From p In MenuList Where p.ParentId = intID).ToList()

                hasChildLink = False
                CheckChildLink(SubMenuDataList)

                If (hasChildLink OrElse Not strMenuUrl.Equals("#")) Then
                    strbMenu.Append(vbTab)
                    strbMenu.Append("<li>")

                    Dim URLBase As String = ResolveUrl("~")

                    IsForwardSlash = If(URLBase.IndexOf("/") > -1, True, False)

                    If IsForwardSlash Then
                        strMenuUrl = strMenuUrl.Replace("\", "/")
                    End If

                    If (Not strMenuUrl.Equals("#")) Then
                        strbMenu.Append("<a href=""").Append(URLBase & strMenuUrl).Append(""">").Append(strMenuText).Append("</a>")
                    Else

                        strbMenu.Append("<a href=""").Append(curURL & Convert.ToString("#", Nothing)).Append(""">").Append(strMenuText).Append("</a>")
                    End If

                    If (hasChildLink) Then
                        strbMenu.Append("<ul>")
                        DrawChild(SubMenuDataList)
                        strbMenu.Append("</ul>")
                    End If

                    strbMenu.Append("</li>")

                End If

            Next

            'MenuItem mnuBack = new MenuItem();
            'mnuBack.Text = "<a onclick='javascript:history.back();return false;' href='#'><img id='imgBack' src='" + ResolveUrl("~") + "Images/back_button.png" + "' width=\"25px\" height=\"25px\" alt=\"Back\" /></a>";
            'mnuMain.Items.Add(mnuBack);

            strbMenu.Append(vbTab)
            'strbMenu.Append("<li>")
            'strbMenu.Append("<a href=""").Append(ResolveUrl("~") + "Help/Default.aspx").Append(""">").Append("Help").Append("</a>")
            'BuildHelpLinks()
            'strbMenu.Append("</li>")
            strbMenu.Append("<li>")
            strbMenu.Append("<a href=""").Append(ResolveUrl("~") + "LoginPage.aspx?op=Logout").Append(""">").Append("Logout").Append("</a>")
            strbMenu.Append("</li>")
        End Sub

        ''' <summary>
        ''' Deawing Child Menu
        ''' </summary>
        ''' <param name="MenuDataList"></param>
        ''' <remarks></remarks>
        Private Sub DrawChild(MenuDataList As List(Of MenuDataObject))

            Dim strMenuText As String, strMenuUrl As String, curURL As String
            Dim intID As Integer

            curURL = Request.FilePath

            Dim IsForwardSlash As Boolean = False

            For Each Menu In MenuDataList

                strMenuText = Menu.MenuText
                strMenuUrl = Menu.NavigateUrl
                intID = Convert.ToInt32(Menu.MenuId, Nothing)

                Dim SubMenuDataList As List(Of MenuDataObject) = (From p In MenuList Where p.ParentId = intID).ToList()

                hasChildLink = False
                CheckChildLink(SubMenuDataList)

                If hasChildLink OrElse strMenuUrl <> "#" Then
                    strbMenu.Append(vbTab & vbTab)
                    strbMenu.Append("<li>")

                    Dim URLBase As String = ResolveUrl("~")
                    IsForwardSlash = If(URLBase.IndexOf("/") > -1, True, False)
                    If IsForwardSlash Then
                        strMenuUrl = strMenuUrl.Replace("\", "/")
                    End If

                    If strMenuUrl <> "#" Then
                        strbMenu.Append("<a href=""").Append(URLBase & strMenuUrl).Append(""">").Append(strMenuText).Append("</a>")
                    Else
                        strbMenu.Append("<a href=""").Append(curURL & Convert.ToString("#", Nothing)).Append(""">").Append(strMenuText).Append("</a>")
                    End If

                    If hasChildLink Then
                        strbMenu.Append("<ul>")
                        DrawChild(SubMenuDataList)
                        strbMenu.Append("</ul>")
                    End If

                    strbMenu.Append("</li>")

                End If

            Next

        End Sub

        ''' <summary>
        ''' Checking Child Menu in Recursively
        ''' </summary>
        ''' <param name="MenuDataList"></param>
        ''' <remarks></remarks>
        Private Sub CheckChildLink(MenuDataList As List(Of MenuDataObject))

            Dim strMenuText As String, strMenuUrl As String
            Dim intID As Integer

            For Each Menu In MenuDataList

                strMenuText = Menu.MenuText
                strMenuUrl = Menu.NavigateUrl
                intID = Menu.MenuId

                Dim SubMenuDataList As List(Of MenuDataObject) = (From p In MenuList Where p.ParentId = intID).ToList()

                If (Not strMenuUrl.Equals("#")) Then
                    hasChildLink = True
                    Return
                Else
                    If SubMenuDataList IsNot Nothing AndAlso SubMenuDataList.Count > 0 Then
                        CheckChildLink(SubMenuDataList)
                    End If
                End If
            Next
        End Sub

        'Private Sub BuildHelpLinks()
        '    Dim _OCDA As New OnlineContentDA()
        '    Dim IsForwardSlash As Boolean = False
        '    Dim URLBase As String = ResolveUrl("~")
        '    HelpSect = Util.GetEnumDataSource(Of Enumerators.HelpSection)()
        '    Dim curURL As String = Request.FilePath
        '    Dim strMenuUrl As String = ""

        '    strbMenu.Append("<ul>")


        '    For Each kvp As KeyValuePair(Of String, Integer) In HelpSect


        '        Dim icnt As IList(Of OnlineContent) = _OCDA.GetByFolderID(kvp.Value)

        '        If icnt.Count > 0 Then
        '            If kvp.Key = "Trade_Fiance_Document_Management" Then
        '                strbMenu.Append("<li>")
        '                strbMenu.Append("<a href=""").Append(curURL & Convert.ToString("#")).Append(""">").Append("Trade Fin. Doc. Management.").Append("</a>")
        '            Else
        '                strbMenu.Append("<li>")
        '                strbMenu.Append("<a href=""").Append(curURL & Convert.ToString("#")).Append(""">").Append(kvp.Key.ToString().Replace("_", " ")).Append("</a>")
        '            End If

        '            strbMenu.Append("<ul>")

        '            For Each oc As OnlineContent In icnt
        '                strbMenu.Append("<li>")

        '                IsForwardSlash = If(URLBase.IndexOf("/") > -1, True, False)

        '                strMenuUrl = "Help\Default.aspx?CntID=" + oc.CntId.ToString()

        '                If IsForwardSlash Then
        '                    strMenuUrl = strMenuUrl.Replace("\", "/")
        '                End If


        '                strbMenu.Append("<a href=""").Append(URLBase & strMenuUrl).Append(""">").Append(oc.Title).Append("</a>")

        '                strbMenu.Append("</li>")
        '            Next

        '            strbMenu.Append("</ul>")

        '            strbMenu.Append("</li>")


        '        End If
        '    Next
        '    strbMenu.Append("</ul>")

        'End Sub

    End Class
End Namespace
