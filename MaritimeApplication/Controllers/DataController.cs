using Microsoft.VisualBasic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using MaritimeApplication.Models;
using System.IO;
using System.Web;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MaritimeApplication.Controllers
{
    public class DataController : Controller
    {
        private MaritimeDatabase_1Entities2 db = new MaritimeDatabase_1Entities2();

        // GET: Data
        public ActionResult Index()
        {
            if(db.Data.Find(0) != null)
            {
                //Send the Different end values of the data over to the View to be displayed
                if(TempData["Mean"] == null)
                {
                    return RedirectToAction("Details");
                }
                string viewMean = TempData["Mean"].ToString();
                ViewBag.Mean = viewMean;
                string viewStandardDeviation = TempData["StandardDeviation"].ToString();
                ViewBag.StandardDeviation = viewStandardDeviation;
                int[] viewBinFrequencies = (int[])TempData["binFrequencies"];
                ViewBag.binFrequencies = viewBinFrequencies;
            }
           
            return View(db.Data.ToList());
        }
        //Add File is called when you upload the file, when the upload file button is pressed
        public ActionResult AddFile(HttpPostedFileBase uploadedFile)
        {
            string filePath = string.Empty;
            //If the file uploaded isn't null or empty
            if (uploadedFile != null)
            {
                //Map a path to save the file locally, if it doesn't exist then create the path and directory
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Save the file as the same name as when it as uploaded
                filePath = path + Path.GetFileName(uploadedFile.FileName);
                string extension = Path.GetExtension(uploadedFile.FileName);
                uploadedFile.SaveAs(filePath);

                //Read in all of the data contained in the file
                string csvData = System.IO.File.ReadAllText(filePath);

                //Split each number by the comma, this is so there is an array containing all of the values in the file
                string[] splitCSVData = csvData.Split(',');

                //Build a connection string to the sql database
                SqlConnection sqlConnB = new SqlConnection(
                    new SqlConnectionStringBuilder()
                    {
                        DataSource = "(localdb)\\Projectsv13",
                        InitialCatalog = "MaritimeDatabase_1",

                    }.ConnectionString
                );
                int count = 0;

                //Loop through each string that is inside the array of the file data
                foreach (string s in splitCSVData)
                {
                    //Initalise the command type and the connection to send the data to the database
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = sqlConnB;

                    //As the final value in the file contains End of Line characters,
                    //they need to be removed to add the data to the int column in the database
                    if (s.Contains("\r\n"))
                    {
                        //Get rid of the End of Line characters
                        String trimmedS = s.Replace("\r\n", string.Empty);

                        //The command is inserting the data value of this current iteration of the loop,
                        //and then adding both its loop iteration number as the ID and the value its self in to the NumberData column
                        //and then the position counter is being incremented each iteration
                        cmd.CommandText = "INSERT INTO Data(Id, NumberData) VALUES(@count, @var)";
                        cmd.Parameters.AddWithValue("@var", trimmedS);
                        cmd.Parameters.AddWithValue("@count", count);
                        count++;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO Data(Id, NumberData) VALUES(@count, @var)";
                        cmd.Parameters.AddWithValue("@var", s);
                        cmd.Parameters.AddWithValue("@count", count);
                        count++;
                    }

                    //The connection to the database is being opened so that the insert command above can be run,
                    //once this has been done the connection to the database is being closed
                    sqlConnB.Open();
                    cmd.ExecuteNonQuery();
                    sqlConnB.Close();

                }
                //A second connecton to the database is being initalised and opened
                SqlCommand com = new SqlCommand();
                com.CommandType = System.Data.CommandType.Text;
                com.Connection = sqlConnB;
                sqlConnB.Open();

                //A reader is being initalised that will read through the data being returned from the SQL command below
                SqlDataReader reader = null;
                com.CommandText = "SELECT NumberData FROM Data";
                reader = com.ExecuteReader();
                //Different variables are being initalised for mean and standard deviation calculations
                //They are being initalised here so they can be used outside of loop
                double mean = 0;
                double totalMean = 0;
                double standDeviPart2 = 0;
                double standardDeviation = 0;
                //An array and list being initalised so the data read from the database can stored in for the calculations
                int[] counterArray = new int[10];
                List<double> Numbers = new List<double>();

                //Checking if the database that the reader is reading from has rows that can be read
                if (reader.HasRows)
                {
                    //Initlalise the counter that shows how many rows are in the current column.
                    int readCount = reader.FieldCount;
                    //While the reader is able to continue reading 
                    while (reader.Read())
                    {
                        //For loop that iterates untill the end of the database
                        for (int i = 0; i < readCount; i++)
                        {
                            //The data in the database is being read and added to a list for later use
                            string myMeanData = reader["NumberData"].ToString();
                            double myDataValue = Convert.ToDouble(reader["NumberData"]);
                            Numbers.Add(myDataValue);
                            //The values that are read in are being summed
                            mean += myDataValue;
                        }

                        //The sum of all the values is being divided by the total number of values to find the mean of the data
                        totalMean = mean / Numbers.Count;

                    }
                }
                sqlConnB.Close();
                //The list of values is being converted in to an array
                double[] numArr = Numbers.ToArray();
                int binLength = 10;
                int NoOfBins = 10;

                //Iterating over the data in the array to start to calculate the standard deviation
                for (int i = 0; i < numArr.Length; i++)
                {
                    double standDeviPart1 = Math.Pow(numArr[i] - totalMean, 2);
                    standDeviPart2 += standDeviPart1;

                    //Finding the frequency of the numbers in bins of 10
                    for(int k = 0; k < NoOfBins; k++)
                    {
                        if(numArr[i] >= (k*binLength) && numArr[i] < ((k+1)*binLength))
                        {
                            counterArray[k]++;
                        }
                    }
                }
                //The final part of the standard deviation calculations
                double standDeviPart3 = standDeviPart2 / numArr.Length;
                standardDeviation = Math.Sqrt(standDeviPart3);

                //Putting the end values in to Temporary data stores to sent to the View
                TempData["Mean"] = totalMean;
                TempData["StandardDeviation"] = standardDeviation;
                TempData["binFrequencies"] = counterArray;


            }
            //Redirect to the Index view
            return RedirectToAction("Index");
        }

        //Wipe is called when the "Wipe the Database" button is clicked
        //It connects to the database and runs a command to delete any data inside the database
        public ActionResult Wipe()
        {
            SqlConnection sqlConnB = new SqlConnection(
            new SqlConnectionStringBuilder()
                {
                    DataSource = "(localdb)\\Projectsv13",
                    InitialCatalog = "MaritimeDatabase_1",

                }.ConnectionString
            );

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = sqlConnB;

            cmd.CommandText = "DELETE FROM Data";

            sqlConnB.Open();
            cmd.ExecuteNonQuery();
            sqlConnB.Close();

            return RedirectToAction("Index");
        }



        // GET: Data/Details/5
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: Data/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Data/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Create([Bind(Include = "Id,NumberData")] Datum datum)
        {
            if (ModelState.IsValid)
            {
                db.Data.Add(datum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(datum);
        }

        // GET: Data/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Information.IsNothing(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var datum = db.Data.Find(id);
            if (Information.IsNothing(datum))
                return HttpNotFound();
            return View(datum);
        }

        // POST: Data/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit([Bind(Include = "Id,NumberData")] Datum datum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(datum);
        }

        // GET: Data/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Information.IsNothing(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var datum = db.Data.Find(id);
            if (Information.IsNothing(datum))
                return HttpNotFound();
            return View(datum);
        }

        // POST: Data/Delete/5
        [HttpPost()]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteConfirmed(int id)
        {
            var datum = db.Data.Find(id);
            db.Data.Remove(datum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}

