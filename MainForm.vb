Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Public Class MainForm
    ' Declare the buttons with WithEvents to handle their events
    Private WithEvents btnViewSubmissions As New Button
    Private WithEvents btnCreateNewSubmission As New Button

    Private submissions As List(Of Submission)
    Private currentIndex As Integer = 0
    Private stopwatch As New Stopwatch()
    Private timer As New Timer()

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf MainForm_KeyDown

        ' Initialize the buttons
        btnViewSubmissions.Text = "View Submissions (Ctrl + V)"
        btnViewSubmissions.Top = 20
        btnViewSubmissions.Left = 20
        btnViewSubmissions.Width = 200

        btnCreateNewSubmission.Text = "Create New Submission (Ctrl + N)"
        btnCreateNewSubmission.Top = 60
        btnCreateNewSubmission.Left = 20
        btnCreateNewSubmission.Width = 200

        ' Add buttons to the form
        Me.Controls.Add(btnViewSubmissions)
        Me.Controls.Add(btnCreateNewSubmission)

        LoadSubmissions()
    End Sub

    ' Event handlers for the buttons
    Private Sub btnViewSubmissions_Click(sender As Object, e As EventArgs) Handles btnViewSubmissions.Click
        BuildViewSubmission()
    End Sub

    Private Sub btnCreateNewSubmission_Click(sender As Object, e As EventArgs) Handles btnCreateNewSubmission.Click
        BuildCreateSubmission()
    End Sub

    Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.V Then
            BuildViewSubmission()
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            BuildCreateSubmission()
        End If
    End Sub

    Private Sub BuildViewSubmission()
        Dim viewForm As New Form
        viewForm.Text = "View Submissions"
        viewForm.KeyPreview = True

        ' Create controls for displaying submission data
        Dim lblName As New Label With {.Text = "Name:", .Top = 20, .Left = 20, .Width = 100}
        Dim txtName As New TextBox With {.Top = 20, .Left = 120, .Width = 200, .ReadOnly = True}
        Dim lblEmail As New Label With {.Text = "Email:", .Top = 60, .Left = 20, .Width = 100}
        Dim txtEmail As New TextBox With {.Top = 60, .Left = 120, .Width = 200, .ReadOnly = True}
        Dim lblPhone As New Label With {.Text = "Phone Number:", .Top = 100, .Left = 20, .Width = 100}
        Dim txtPhone As New TextBox With {.Top = 100, .Left = 120, .Width = 200, .ReadOnly = True}
        Dim lblGithubLink As New Label With {.Text = "Github Link:", .Top = 140, .Left = 20, .Width = 100}
        Dim txtGithubLink As New TextBox With {.Top = 140, .Left = 120, .Width = 200, .ReadOnly = True}
        Dim lblStopwatchTime As New Label With {.Text = "Stopwatch Time:", .Top = 180, .Left = 20, .Width = 100}
        Dim txtStopwatchTime As New TextBox With {.Top = 180, .Left = 120, .Width = 200, .ReadOnly = True}
        Dim btnEdit As New Button With {.Text = "Edit (Ctrl + E)", .Top = 260, .Left = 20, .Width = 100}
        Dim btnDelete As New Button With {.Text = "Delete (Ctrl + D)", .Top = 260, .Left = 130, .Width = 100}


        ' Create Previous and Next buttons
        Dim btnPrevious As New Button With {.Text = "Previous (Ctrl + P)", .Top = 220, .Left = 20, .Width = 150}
        Dim btnNext As New Button With {.Text = "Next (Ctrl + N)", .Top = 220, .Left = 180, .Width = 150}

        ' Add controls to the form
        viewForm.Controls.Add(lblName)
        viewForm.Controls.Add(txtName)
        viewForm.Controls.Add(lblEmail)
        viewForm.Controls.Add(txtEmail)
        viewForm.Controls.Add(lblPhone)
        viewForm.Controls.Add(txtPhone)
        viewForm.Controls.Add(lblGithubLink)
        viewForm.Controls.Add(txtGithubLink)
        viewForm.Controls.Add(lblStopwatchTime)
        viewForm.Controls.Add(txtStopwatchTime)
        viewForm.Controls.Add(btnPrevious)
        viewForm.Controls.Add(btnNext)
        viewForm.Controls.Add(btnEdit)
        viewForm.Controls.Add(btnDelete)


        ' Handle Previous and Next button clicks
        AddHandler btnPrevious.Click, Sub(sender, e)
                                          currentIndex -= 1
                                          If currentIndex < 0 Then currentIndex = submissions.Count - 1
                                          LoadSubmission(submissions(currentIndex), txtName, txtEmail, txtPhone, txtGithubLink, txtStopwatchTime)
                                      End Sub
        AddHandler btnNext.Click, Sub(sender, e)
                                      currentIndex += 1
                                      If currentIndex >= submissions.Count Then currentIndex = 0
                                      LoadSubmission(submissions(currentIndex), txtName, txtEmail, txtPhone, txtGithubLink, txtStopwatchTime)
                                  End Sub

        ' Handle keyboard shortcuts
        AddHandler viewForm.KeyDown, Sub(sender, e)
                                         If e.Control AndAlso e.KeyCode = Keys.P Then
                                             btnPrevious.PerformClick()
                                         ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
                                             btnNext.PerformClick()
                                         End If
                                     End Sub

        AddHandler btnEdit.Click, Sub(sender, e)
                                      If currentIndex >= 0 AndAlso currentIndex < submissions.Count Then
                                          BuildEditSubmission(currentIndex)
                                      End If
                                  End Sub

        AddHandler btnDelete.Click, Sub(sender, e)
                                        If currentIndex >= 0 AndAlso currentIndex < submissions.Count Then
                                            Dim confirmation = MessageBox.Show("Are you sure you want to delete this submission?", "Delete Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                            If confirmation = DialogResult.Yes Then
                                                DeleteSubmission(currentIndex)
                                                LoadSubmissions()
                                                If submissions.Count > 0 Then
                                                    currentIndex = 0
                                                    LoadSubmission(submissions(currentIndex), txtName, txtEmail, txtPhone, txtGithubLink, txtStopwatchTime)
                                                Else
                                                    ' Clear controls if no submissions left
                                                    txtName.Clear()
                                                    txtEmail.Clear()
                                                    txtPhone.Clear()
                                                    txtGithubLink.Clear()
                                                    txtStopwatchTime.Clear()
                                                End If
                                            End If
                                        End If
                                    End Sub

        ' Show the first submission initially
        If submissions.Count > 0 Then
            LoadSubmission(submissions(0), txtName, txtEmail, txtPhone, txtGithubLink, txtStopwatchTime)
        End If

        ' Show the form
        viewForm.ShowDialog()
    End Sub

    Private Sub LoadSubmission(submission As Submission, txtName As TextBox, txtEmail As TextBox, txtPhone As TextBox, txtGithubLink As TextBox, txtStopwatchTime As TextBox)
        txtName.Text = submission.Name
        txtEmail.Text = submission.Email
        txtPhone.Text = submission.Phone
        txtGithubLink.Text = submission.GithubLink
        txtStopwatchTime.Text = submission.StopwatchTime
    End Sub

    Private Sub BuildCreateSubmission()
        Dim createForm As New Form
        createForm.Text = "Create New Submission"
        createForm.KeyPreview = True

        ' Create controls for input
        Dim lblName As New Label With {.Text = "Name:", .Top = 20, .Left = 20, .Width = 100}
        Dim txtName As New TextBox With {.Top = 20, .Left = 120, .Width = 200}
        Dim lblEmail As New Label With {.Text = "Email:", .Top = 60, .Left = 20, .Width = 100}
        Dim txtEmail As New TextBox With {.Top = 60, .Left = 120, .Width = 200}
        Dim lblPhone As New Label With {.Text = "Phone Number:", .Top = 100, .Left = 20, .Width = 100}
        Dim txtPhone As New TextBox With {.Top = 100, .Left = 120, .Width = 200}
        Dim lblGithubLink As New Label With {.Text = "Github Link:", .Top = 140, .Left = 20, .Width = 100}
        Dim txtGithubLink As New TextBox With {.Top = 140, .Left = 120, .Width = 200}
        Dim lblStopwatchTime As New Label With {.Text = "Stopwatch Time:", .Top = 180, .Left = 20, .Width = 100}
        Dim txtStopwatchTime As New TextBox With {.Top = 180, .Left = 120, .Width = 200, .ReadOnly = True}

        ' Toggle stopwatch button
        Dim btnToggleStopwatch As New Button With {.Text = "Toggle Stopwatch (Ctrl + T)", .Top = 220, .Left = 20, .Width = 150}
        AddHandler btnToggleStopwatch.Click, Sub(sender, e)
                                                 If stopwatch.IsRunning Then
                                                     stopwatch.Stop()
                                                 Else
                                                     stopwatch.Start()
                                                 End If
                                             End Sub

        ' Update stopwatch display
        AddHandler timer.Tick, Sub(sender, e)
                                   txtStopwatchTime.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
                               End Sub
        timer.Interval = 1000 ' Update every second
        timer.Start()

        ' Submit button
        Dim btnSubmit As New Button With {.Text = "Submit (Ctrl + S)", .Top = 260, .Left = 20, .Width = 100}
        AddHandler btnSubmit.Click, Sub(sender, e)
                                        Dim submission As New Submission With {
                                            .Name = txtName.Text,
                                            .Email = txtEmail.Text,
                                            .Phone = txtPhone.Text,
                                            .GithubLink = txtGithubLink.Text,
                                            .StopwatchTime = txtStopwatchTime.Text
                                        }
                                        SubmitForm(submission)
                                        createForm.Close()
                                    End Sub

        ' Add controls to the form
        createForm.Controls.Add(lblName)
        createForm.Controls.Add(txtName)
        createForm.Controls.Add(lblEmail)
        createForm.Controls.Add(txtEmail)
        createForm.Controls.Add(lblPhone)
        createForm.Controls.Add(txtPhone)
        createForm.Controls.Add(lblGithubLink)
        createForm.Controls.Add(txtGithubLink)
        createForm.Controls.Add(lblStopwatchTime)
        createForm.Controls.Add(txtStopwatchTime)
        createForm.Controls.Add(btnToggleStopwatch)
        createForm.Controls.Add(btnSubmit)

        ' Handle keyboard shortcuts
        AddHandler createForm.KeyDown, Sub(sender, e)
                                           If e.Control AndAlso e.KeyCode = Keys.T Then
                                               btnToggleStopwatch.PerformClick()
                                           ElseIf e.Control AndAlso e.KeyCode = Keys.S Then
                                               btnSubmit.PerformClick()
                                           End If
                                       End Sub

        ' Show the form
        createForm.ShowDialog()
    End Sub

    Private Sub SubmitForm(submission As Submission)
        Try
            Using client As New WebClient()
                client.Headers(HttpRequestHeader.ContentType) = "application/json"
                Dim jsonData As String = JsonConvert.SerializeObject(New With {
                    Key .name = submission.Name,
                    Key .email = submission.Email,
                    Key .phone = submission.Phone,
                    Key .github_link = submission.GithubLink,
                    Key .stopwatch_time = submission.StopwatchTime
                })
                Dim response As String = client.UploadString("http://localhost:3000/submit", "POST", jsonData)
                MessageBox.Show("Submission successful!")
            End Using
        Catch ex As WebException
            Using response = ex.Response
                Dim httpResponse = CType(response, HttpWebResponse)
                If httpResponse.StatusCode = HttpStatusCode.BadRequest Then
                    Using data = response.GetResponseStream()
                        Using reader = New StreamReader(data)
                            Dim errorText As String = reader.ReadToEnd()
                            MessageBox.Show($"Error: {errorText}")
                        End Using
                    End Using
                Else
                    MessageBox.Show($"An error occurred: {ex.Message}")
                End If
            End Using
        End Try
    End Sub

    Private Sub LoadSubmissions()
        ' Make API call to load submissions
        Using client As New WebClient()
            Dim jsonData As String = client.DownloadString("http://localhost:3000/read")
            submissions = JsonConvert.DeserializeObject(Of List(Of Submission))(jsonData)
        End Using
    End Sub
End Class

Private Sub BuildEditSubmission(index As Integer)
    Dim editForm As New Form
    editForm.Text = "Edit Submission"

    ' Load submission data for editing
    Dim submission = submissions(index)

    ' Create controls for editing
    Dim lblName As New Label With {.Text = "Name:", .Top = 20, .Left = 20, .Width = 100}
    Dim txtName As New TextBox With {.Top = 20, .Left = 120, .Width = 200, .Text = submission.Name}
    Dim lblEmail As New Label With {.Text = "Email:", .Top = 60, .Left = 20, .Width = 100}
    Dim txtEmail As New TextBox With {.Top = 60, .Left = 120, .Width = 200, .Text = submission.Email}
    Dim lblPhone As New Label With {.Text = "Phone Number:", .Top = 100, .Left = 20, .Width = 100}
    Dim txtPhone As New TextBox With {.Top = 100, .Left = 120, .Width = 200, .Text = submission.Phone}
    Dim lblGithubLink As New Label With {.Text = "Github Link:", .Top = 140, .Left = 20, .Width = 100}
    Dim txtGithubLink As New TextBox With {.Top = 140, .Left = 120, .Width = 200, .Text = submission.GithubLink}
    Dim lblStopwatchTime As New Label With {.Text = "Stopwatch Time:", .Top = 180, .Left = 20, .Width = 100}
    Dim txtStopwatchTime As New TextBox With {.Top = 180, .Left = 120, .Width = 200, .ReadOnly = True, .Text = submission.StopwatchTime}

    ' Save and Cancel buttons
    Dim btnSave As New Button With {.Text = "Save (Ctrl + S)", .Top = 220, .Left = 20, .Width = 100}
    Dim btnCancel As New Button With {.Text = "Cancel", .Top = 220, .Left = 130, .Width = 100}

    ' Handle Save and Cancel button clicks
    AddHandler btnSave.Click, Sub(sender, e)
                                  submission.Name = txtName.Text
                                  submission.Email = txtEmail.Text
                                  submission.Phone = txtPhone.Text
                                  submission.GithubLink = txtGithubLink.Text
                                  UpdateSubmission(index, submission)
                                  editForm.Close()
                              End Sub

    AddHandler btnCancel.Click, Sub(sender, e)
                                    editForm.Close()
                                End Sub

    ' Add controls to the form
    editForm.Controls.Add(lblName)
    editForm.Controls.Add(txtName)
    editForm.Controls.Add(lblEmail)
    editForm.Controls.Add(txtEmail)
    editForm.Controls.Add(lblPhone)
    editForm.Controls.Add(txtPhone)
    editForm.Controls.Add(lblGithubLink)
    editForm.Controls.Add(txtGithubLink)
    editForm.Controls.Add(lblStopwatchTime)
    editForm.Controls.Add(txtStopwatchTime)
    editForm.Controls.Add(btnSave)
    editForm.Controls.Add(btnCancel)

    ' Show the form
    editForm.ShowDialog()
End Sub

Private Sub UpdateSubmission(index As Integer, updatedSubmission As Submission)
    Try
        Using client As New WebClient()
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            Dim jsonData As String = JsonConvert.SerializeObject(New With {
                Key .name = updatedSubmission.Name,
                Key .email = updatedSubmission.Email,
                Key .phone = updatedSubmission.Phone,
                Key .github_link = updatedSubmission.GithubLink,
                Key .stopwatch_time = updatedSubmission.StopwatchTime
            })
            Dim response As String = client.UploadString($"http://localhost:3000/submit?index={index}", "POST", jsonData)
            MessageBox.Show("Submission updated successfully!")
        End Using
    Catch ex As WebException
        MessageBox.Show($"An error occurred: {ex.Message}")
    End Try
End Sub

Private Sub DeleteSubmission(index As Integer)
    Try
        Using client As New WebClient()
            Dim response As String = client.UploadString($"http://localhost:3000/delete?index={index}", "POST", "")
            MessageBox.Show("Submission deleted successfully!")
        End Using
    Catch ex As WebException
        MessageBox.Show($"An error occurred: {ex.Message}")
    End Try
End Sub

Private Sub LoadSubmissions(Optional searchEmail As String = "")
    ' Make API call to load submissions
    Using client As New WebClient()
        Dim jsonData As String = client.DownloadString($"http://localhost:3000/read")
        submissions = JsonConvert.DeserializeObject(Of List(Of Submission))(jsonData)
    End Using

    ' Filter submissions if searchEmail is provided
    If Not String.IsNullOrEmpty(searchEmail) Then
        submissions = submissions.Where(Function(s) s.Email.ToLower().Contains(searchEmail.ToLower())).ToList()
    End If
End Sub


Public Class Submission
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property GithubLink As String
    Public Property StopwatchTime As String
End Class
