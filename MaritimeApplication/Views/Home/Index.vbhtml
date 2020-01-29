@Code
    ViewData("Title") = "Home Page"
End Code

    <div class="jumbotron">
        <h1>Welcome to My Web Application</h1>
        <p>
            This application allows you to upload a csv file, which it will then read and find
            the Numerical Mean, the Standard Deviation and the frequency of the data.
            <br />
            To Upload a data file press the button below to get started.
        </p>

        @Html.ActionLink("Click me to get started!", "Index", "Data")<br />

    </div>

