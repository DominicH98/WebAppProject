@ModelType IEnumerable(Of MaritimeApplication.Models.Datum)
@Code
    ViewData("Title") = "Details"
End Code



    <div style="padding:20%; width:100%; text-align:center">
        <h2>Oops, something went wrong</h2>
        <h4>Please clear the database before running the application again</h4>
        @Using (Html.BeginForm("Wipe", "Data"))
            @<input type="submit" value="Wipe Database" />
        End Using
    </div>
