@ModelType IEnumerable(Of MaritimeApplication.Models.Datum)
@Code
    ViewData("Title") = "Index"
End Code

@If ViewBag.Mean Is Nothing Then
    @<h2>Upload Your File Below</h2>

    @Using (Html.BeginForm("AddFile", "Data", FormMethod.Post, New With {.enctype = "multipart/form-data"}))

       @<input type="file" name="uploadedFile" id="uploadedFile" />     
       @<input type="submit" />

    End Using

End If


<br />

@Using (Html.BeginForm("Wipe", "Data"))
@<input type="submit" value="Wipe Database" style="float: right" />
End Using

<h3>
    Mean Of the data uploaded:
@If ViewBag.Mean IsNot Nothing Then
    @ViewBag.Mean
End IF
    <br />
    Standard Deviation of the data uploaded:
    @If ViewBag.StandardDeviation IsNot Nothing Then
        @ViewBag.StandardDeviation
    End If
        <br />
        Bin Frequencies of the data uploaded:
        @If ViewBag.binFrequencies IsNot Nothing Then
            Dim list As New List(Of Integer)
            @For Each item In ViewBag.binFrequencies

                list.Add(item)

            Next


            @<!-- This code below will put the Bin Frequency data in to a graph
                  This however adds the chart as a graphic and as such takes over the full screen size
                  I was unsure how to import this chart as an image without having to alter the application structure

               If True Then
                    Dim myChart = New Chart(width:=600, height:=400).AddTitle("Data Frequency in Bins of 10").AddSeries(name:="Data Input",
                    xValue:={"0 - 10", "10 - 20", "20 - 30", "30 - 40", "40 - 50", "50 - 60", "60 - 70", "70 - 80", "80 - 90", "90 - 100"},
                    yValues:={list}).Write()
               End If
            -->

            @<h4>Numbers Between 0 - 10: @list(0)</h4>
            @<h4>Numbers Between 10 - 20: @list(1)</h4>
            @<h4>Numbers Between 20 - 30: @list(2)</h4>
            @<h4>Numbers Between 30 - 40: @list(3)</h4>
            @<h4>Numbers Between 40 - 50: @list(4)</h4>
            @<h4>Numbers Between 50 - 60: @list(5)</h4>
            @<h4>Numbers Between 60 - 70: @list(6)</h4>
            @<h4>Numbers Between 70 - 80: @list(7)</h4>
            @<h4>Numbers Between 80 - 90: @list(8)</h4>
            @<h4>Numbers Between 90 - 100: @list(9)</h4>


        End If
            <br />
</h3>



<Table Class="table">
            <tr>
                    <th>
        @Html.DisplayNameFor(Function(model) model.NumberData)
                </th>
                <th></th>
            </tr>


        @For Each item In Model
        @<tr>
            <td>
                @Html.DisplayFor(Function(modelItem) item.NumberData)
            </td>
        </tr>
Next

</Table>
