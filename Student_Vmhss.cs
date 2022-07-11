using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Sprint1
{
    class Student_Vmhss
    {
        //1. Students//

        //Insert student
        public static void Insert(SqlConnection con)
        {
        startInsert:
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Insert_student";
        FnValidation:
            Console.Write("Enter First name: ");
            string fn = Console.ReadLine();
            Regex reg = new Regex("[^a-zA-z ]");
            if (reg.IsMatch(fn))
            {
                Console.WriteLine("Enter proper first name");
                goto FnValidation;
            }
            else
            {
                cmd.Parameters.AddWithValue("@FirstName", fn);
            }
        LnValidation:
            Console.Write("Enter Last name: ");
            string ln = Console.ReadLine();
            if (reg.IsMatch(ln))
            {
                Console.WriteLine("Enter proper Last name");
                goto LnValidation;
            }
            else
            {
                cmd.Parameters.AddWithValue("@LastName", ln);
            }
        DOBValidation:
            Console.Write("Enter DOB (YYYY-MM-DD): ");
            string dob = Console.ReadLine();
          
            Regex regDOB = new Regex("(19|20)[0-9]{2}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$");
            if (regDOB.IsMatch(dob))
            {
                DateTime d = Convert.ToDateTime(dob);
                DateTime d1 = DateTime.Today;
                int r = d.Year;
                int s = d1.Year;
                int sub;
                sub = s - r;

                if (sub > 15 && sub < 100)
                {
                    cmd.Parameters.AddWithValue("@DOB", dob);
                }                  
            }
            else
            {
                Console.WriteLine("Enter proper DOB ");
                goto DOBValidation;
            }
        
        GenValidation:
            Console.Write("Enter Gender: ");
            string g = Console.ReadLine();
            Regex regG = new Regex("[M|F]");
            if (regG.IsMatch(g))
            {
                cmd.Parameters.AddWithValue("@Gender", g);
            }
            else
            {
                Console.WriteLine("Enter proper gender");
                goto GenValidation;
            }
        PhValidation:
            Console.Write("Enter Phone no: ");
            string 
                ph = Console.ReadLine();
            Regex regPh = new Regex("^[6-9][0-9]{9}$");        
            if (regPh.IsMatch(ph) )
            {             
                cmd.Parameters.AddWithValue("@Phone", ph);
            }
            else
            {
                Console.WriteLine("Enter proper phone number with 10 digits and should start with 6 or 7 or 8 or 9");
                goto PhValidation;        
            }           
            Console.Write("Enter Address: ");
            string ad = Console.ReadLine();
            cmd.Parameters.AddWithValue("@Address", ad);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Console.WriteLine("\nNew student has been added successfully");
            Console.WriteLine("Do you want to add another student? If Yes, Enter Y or Enter B for back or Enter H for homepage or Press Enter to exit.");
            string x = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (x == "Y")
            {
                goto startInsert;
            }
            if (x == "B")
            {
                StudentsPage(con);
            }
            if (x == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }
        }
        //Display student
        public static void Display(SqlConnection con)
        {
            con.Open();
            string str = "select * from Student_VMHSS";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("Id\tFirst Name\t\t\t\t\t\tLast Name");
            Console.WriteLine("--------------------------------------------------------------------------");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine($"\n{dr[0]}\t{dr[1]}\t{dr[2]}");
                    Console.WriteLine("--------------------------------------------------------------------------");
                }
            }
            con.Close();
            Console.WriteLine("If you want to display specific student's details, Enter Y.\nEnter B for back.\nIf you want to go to home screen, Enter H.");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "Y")
            {
                DisplayOne(con);
            }
            if (e == "B")
            {
                StudentsPage(con);
            }
            if (e == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }
        }
        //DisplayOne student
        public static void DisplayOne(SqlConnection con)
        {
            con.Open();
            Console.WriteLine("Enter student id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string str = $"select * from Student_VMHSS where Id = {id}";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("-----------------------------------------------------------------");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine($"Id: {dr[0]}\nFirst Name:{dr[1]}\nLast Name: {dr[2]}\n{dr.GetDateTime(3).ToString("dd/MM/yyyy")}\nGender: {dr[4]}\nPhone: {dr[5]}\nAddress: {dr[6]}");
                }
            }
            Console.WriteLine("-----------------------------------------------------------------");
            con.Close();
            Console.WriteLine("If you want to display specific student's details, Enter Y.\nEnter B for back.\nIf you want to go to home screen, Enter H.");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "Y")
            {
                DisplayOne(con);
            }
            if (e == "B")
            {
                StudentsPage(con);
            }
            if (e == "H")
            {
                HomePage(con);
            }
               
            else
            {
                Console.WriteLine("Exit");
            }
        }
        //Update student
        public static void Update(SqlConnection con)
        {
        startUpdate:
            Console.Write("Enter Student Id: ");
            int Id = Convert.ToInt32(Console.ReadLine());
        back:
            Console.WriteLine("\n1 - First name\n2 - Last name\n3 - DOB\n4 - Gender\n5 - Phone\n6 - Address\nEnter the property to edit:");
            int ch = Convert.ToInt32(Console.ReadLine());

            if (ch == 1)
            {
            one:
                Console.Write("Enter First name: ");
                string fn = Console.ReadLine();
                Regex reg = new Regex("[^a-zA-z]");
                if (reg.IsMatch(fn))
                {
                    Console.WriteLine("Enter proper first name");
                    goto one;
                }
                else
                {
                    string str = $"update Student_VMHSS set FirstName  = '{fn}' where Id = {Id}";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Updated successfully");
                }
                Console.WriteLine("Do you want to update another student? If Yes, Enter Y or Enter B for back or Enter S for students page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startUpdate;
                }
                if (x == "B")
                {
                    goto back;
                }
                if (x == "S")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }

            if (ch == 2)
            {
            two:
                Console.Write("Enter Last name: ");
                string ln = Console.ReadLine();
                Regex reg = new Regex("[^a-zA-z]");
                if (reg.IsMatch(ln))
                {
                    Console.WriteLine("Enter proper Last name");
                    goto two;
                }
                else
                {
                    string str = $"update Student_VMHSS set LastName  = '{ln}' where Id = {Id}";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Updated successfully");
                }
                Console.WriteLine("Do you want to update another student? If Yes, Enter Y or Enter B for back or Enter S for students page or Enter H for home page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startUpdate;
                }
                if (x == "B")
                {
                    goto back;
                }
                if (x == "S")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            if (ch == 3)
            {
            Three:
                Console.Write("Enter DOB (YYYY-MM-DD): ");
                string dob = Console.ReadLine();
             
                Regex regDOB = new Regex("(19|20)[0-9]{2}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$");
                if (regDOB.IsMatch(dob))
                {
                    DateTime d = Convert.ToDateTime(dob);
                    DateTime d1 = DateTime.Today;
                    int r = d.Year;
                    int s = d1.Year;
                    int sub;
                    sub = s - r;
                    if (sub > 15 && sub < 100)
                    {
                        string str = $"update Student_VMHSS set DOB  = '{dob}' where Id = {Id}";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = str;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Console.WriteLine("Updated successfully");                 
                    }
                }
                else
                {
                    Console.WriteLine("Enter proper DOB ");
                    goto Three;
                }
           
                Console.WriteLine("Do you want to update another student? If Yes, Enter Y or Enter B for back or Enter S for students page or Enter H for homepage or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startUpdate;
                }
                if (x == "B")
                {
                    goto back;
                }
                if (x == "S")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            if (ch == 4)
            {
            Four:
                Console.Write("Enter Gender (M/F): ");
                string g = Console.ReadLine();
                Regex regG = new Regex("[M|F]");
                if (regG.IsMatch(g))
                {
                    string str = $"update Student_VMHSS set Gender  = '{g}' where Id = {Id}";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Updated successfully");
                }
                else
                {
                    Console.WriteLine("Enter proper gender");
                    goto Four;
                }
                Console.WriteLine("Do you want to update another student? If Yes, Enter Y or Enter B for back or Enter H for home page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startUpdate;
                }
                if (x == "B")
                {
                    goto back;
                }
                if (x == "S")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            if (ch == 5)
            {
            Five:
                Console.Write("Enter Phone no: ");
                string ph = Console.ReadLine();
                Regex regPh = new Regex("^[6-9][0-9]{9}$");
                if (regPh.IsMatch(ph))
                {
                    string str = $"update Student_VMHSS set Phone  = '{ph}' where Id = {Id}";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Updated successfully");
                }
                else
                {
                    Console.WriteLine("Enter proper phone number with 10 digits and should start with 6 or 7 or 8 or 9");
                    goto Five;
                }
                Console.WriteLine("Do you want to update another student ? If Yes, Enter Y or Enter B for back or Enter S for students page or Enter H for home page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startUpdate;
                }
                if (x == "B")
                {
                    goto back;
                }
                if (x == "S")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            if (ch == 6)
            {
                Console.Write("Enter Address: ");
                string ad = Console.ReadLine();
                string str = $"update Student_VMHSS set Address  = '{ad}' where Id = {Id}";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = str;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                Console.WriteLine("Updated successfully");
                Console.WriteLine("Do you want to update another student? If Yes, Enter Y or Enter B for back or Enter S for students page or Enter H for home page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startUpdate;
                }
                if (x == "B")
                {
                    goto back;
                }
                if (x == "S")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
        }
        //Delete student
        public static void Delete(SqlConnection con)
        {
        startDelete:
            try
            {
                Console.Write("Enter Student Id: ");
                int id = Convert.ToInt32(Console.ReadLine());
                string cc = $"select * from Student_VMHSS";
                SqlCommand cmd = new SqlCommand(cc);
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    con.Close();
                    string str = $"delete from Enrollment_details where sID = {id}";
                    SqlCommand cmdr = new SqlCommand(str);
                    cmdr.Connection = con;
                    con.Open();
                    cmdr.ExecuteNonQuery();
                    con.Close();

                    string str1 = $"delete from Student_VMHSS where Id = {id}";
                    SqlCommand cmdr1 = new SqlCommand(str1);
                    cmdr1.Connection = con;
                    con.Open();
                    cmdr1.ExecuteNonQuery();
                    con.Close();

                    Console.WriteLine("\nStudent has been deleted successfully\n");
                }
                else
                {
                    Console.WriteLine("ID not found");
                    con.Close();
                    goto startDelete;
                }
                Console.WriteLine("Do you want to delete another student? If Yes, Enter Y or Enter B for back page or Enter H for home page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startDelete;
                }
                if (x == "B")
                {
                    StudentsPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            catch
            {
                Console.WriteLine("Id not found.");
                goto startDelete;
            }
        }
        

        //2. Courses//

        //Insert course
        public static void InsertCourse(SqlConnection con)
        {
        startCInsert:
            //Course name
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Insert_course";

            Console.Write("Enter course name: ");
            string cn = Console.ReadLine();
            cmd.Parameters.AddWithValue("@CName", cn);


        startCInsertsub:
            Console.Write("Enter subjects name: ");
            string sub = Console.ReadLine();
            Regex RegSub = new Regex("[, ]");
            if (RegSub.IsMatch(sub))
            {
                cmd.Parameters.AddWithValue("@CSub", sub);
            }
            else
            {
                Console.WriteLine("Invalid input");
                goto startCInsertsub;
            }

        there:
            Console.Write("Enter course duration: ");
            int du = Convert.ToInt32(Console.ReadLine());
            if (du > 8)
            {
                Console.WriteLine("Maximum duration should be 8 months");
                goto there;
            }
            else
            {
                cmd.Parameters.AddWithValue("@CDuration", du);
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            Console.WriteLine("\nNew course has been added successfully");
            Console.WriteLine("Do you want to update another course? If Yes, Enter Y or Enter C for Courses page or Enter H for home page or Press Enter to exit.");
            string x = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (x == "Y")
            {
                goto startCInsert;
            }
            if (x == "C")
            {
                CoursesPage(con);
            }
            if (x == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }
        }
        //Display Courses
        public static void DisplayCourse(SqlConnection con)
        {
            con.Open();
            string str = "select * from Course_details";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("--------------------");
            Console.WriteLine("Id\tCourse Name");
            Console.WriteLine("--------------------");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine($"\n{dr[0]}\t{dr[1]}");
                    Console.WriteLine("--------------------");
                }
            }
            con.Close();

            Console.WriteLine("If you want to display specific course details, Enter Y.\nIf you want to go to course section, Enter C.\nIf you want to go to home screen, Enter H.");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "Y")
            {
                DisplayOneCourse(con);
            }
            if (e == "C")
            {
                CoursesPage(con);
            }
            if (e == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }
        }
        //DisplayOne course
        public static void DisplayOneCourse(SqlConnection con)
        {
            con.Open();
            Console.WriteLine("Enter course id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string str = $"select * from Course_details where Id = {id}";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("-----------------------------------------------------------------");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine($"\nId:{dr[0]}\nCourse Name:{dr[1]}\nSubjects Name: {dr[2]}\nDuration: {dr[3] + " months"}");
                }
            }
            Console.WriteLine("-----------------------------------------------------------------");
            con.Close();

            Console.WriteLine("Emter Y to display another course.\nIf you want to go to course section, Enter C.\nIf you want to go to home screen, Enter H.");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "Y")
            {
                DisplayOneCourse(con);
            }
            if (e == "C")
            {
                CoursesPage(con);
            }
            if (e == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }
        }
        //Update course
        public static void UpdateCourse(SqlConnection con)
        {
        startCUpdate:
            Console.Write("Enter course Id: ");
            int Id = Convert.ToInt32(Console.ReadLine());
        cBack:
            Console.WriteLine("Enter the property to edit: \n1 - Course name\n2 - Subjects\n3 - Duration");
            int ch = Convert.ToInt32(Console.ReadLine());

            if (ch == 1)
            {
                //Cone:
                Console.Write("Enter course name: ");
                string cn = Console.ReadLine();

                string str = $"update Course_details set CName  = '{cn}' where Id = {Id}";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = str;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                Console.WriteLine("Updated successfully");

                Console.WriteLine("Do you want to update another course? If Yes, Enter Y, Enter B for back or Enter C for courses page or Enter H for home page or Press any key.");
                string x = Console.ReadLine();
                if (x == "Y")
                {
                    goto startCUpdate;
                }
                if (x == "B")
                {
                    goto cBack;
                }
                if (x == "C")
                {
                    CoursesPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            if (ch == 2)
            {
            Ctwo:
                Console.Write("Enter Subjects name: ");
                string sn = Console.ReadLine();
                Regex regsubj = new Regex("[, ]");
                if (regsubj.IsMatch(sn))
                {
                    string str = $"update Course_details set CSub  = '{sn}' where Id = {Id}";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Updated successfully");
                }
                else
                {
                    Console.WriteLine("Enter proper Subjects  name");
                    goto Ctwo;
                }
                Console.WriteLine("Do you want to update another course? If Yes, Enter Y, Enter B for back or Enter C for courses page or Enter H for home screen or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startCUpdate;
                }
                if (x == "B")
                {
                    goto cBack;
                }
                if (x == "C")
                {
                    CoursesPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            if (ch == 3)
            {
            Cthree:
                Console.Write("Enter course duration: ");
                int dur = Convert.ToInt32(Console.ReadLine());
                if (dur > 8)
                {
                    Console.WriteLine("Maximum duration should be 8 months");
                    goto Cthree;
                }
                else
                {
                    string str = $"update Course_details set CDuration  = '{dur}' where Id = {Id}";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Updated successfully");
                }
                Console.WriteLine("Do you want to update another course? If Yes, Enter Y, Enter B for back or Enter C for courses page or Enter H for home screen or Press Enter to exit.");
                string x = Console.ReadLine();
                if (x == "Y")
                {
                    goto startCUpdate;
                }
                if (x == "B")
                {
                    goto cBack;
                }
                if (x == "C")
                {
                    CoursesPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
        }
        //Delete course
        public static void DeleteCourse(SqlConnection con)
        {
       
        startCDelete:
            try
            {
                Console.Write("Enter Course Id: ");
                int id = Convert.ToInt32(Console.ReadLine());
                string cc = $"select * from Course_details where Id = {id}";
                SqlCommand cmd = new SqlCommand(cc);
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    con.Close();
                    string str = $"delete from Enrollment_details where CId = {id}";
                    SqlCommand cmdr = new SqlCommand(str);
                    cmdr.Connection = con;
                    con.Open();
                    cmdr.ExecuteNonQuery();
                    con.Close();

                    string str1 = $"delete from Course_details where Id = {id}";
                    SqlCommand cmdr1 = new SqlCommand(str1);
                    cmdr1.Connection = con;
                    con.Open();
                    cmdr1.ExecuteNonQuery();
                    con.Close();

                    Console.WriteLine("\nCourse has been deleted successfully\n");
                }
            }
            catch
            {
                Console.WriteLine("Course Id not found.");
                goto startCDelete;
            }
           
            Console.WriteLine("Do you want to delete another course? If Yes, Enter Y, Enter B for back or Enter H for home screen or Press Enter to exit.");
            string x = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (x == "Y")
            {
                goto startCDelete;
            }
            if (x == "B")
            {
                CoursesPage(con);
            }
            if (x == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }
        }
     
        //3. Enrollment//

        //Display enrollment
        public static void DisplayEnrollment(SqlConnection con)
        {
            con.Open();
            string str = "select * from Enrollment_details";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("eId\tsID\tcID\tDate");
            Console.WriteLine("--------------------------------------------------------------------------");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine($"\n{dr[0]}\t{dr[1]}\t{dr[2]}\t{dr.GetDateTime(3).ToString("dd/MM/yyyy")}");
                    Console.WriteLine("--------------------------------------------------------------------------");
                }
            }
            
            con.Close();
            Console.WriteLine("\nEnter E for Enrollment page.\nIf you want to go to home screen, Enter H.");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "E")
            {
                EnrollmentPage(con);
            }
            if (e == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }

        }
        //Insert enrollment
        public static void InsertEnrollment(SqlConnection con)
        {
            try
            {
            startEInsert:
                //Course name
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Insert_enrollment";

                Regex regenr = new Regex("[a-zA-Z0-9-]");
            startCInsertEid:
                Console.Write("Enter enrollment id: ");
                string eid = Console.ReadLine();
                if (regenr.IsMatch(eid))
                {
                    cmd.Parameters.AddWithValue("@eID", eid);
                }
                else
                {
                    Console.WriteLine("Invalid input. Value should not be null");
                    goto startCInsertEid;
                }

            startEInsertSid:
                Console.Write("Enter student id: ");
                string sid = /*Convert.ToInt32*/Console.ReadLine();

                if (regenr.IsMatch(sid))
                {
                    cmd.Parameters.AddWithValue("@sID", sid);
                }
                else
                {
                    Console.WriteLine("Invalid input. Value should not be null");
                    goto startEInsertSid;
                }

            startEInsertCid:
                Console.Write("Enter course id: ");
                string cid = /*Convert.ToInt32*/(Console.ReadLine());
                if (regenr.IsMatch(cid))
                {
                    cmd.Parameters.AddWithValue("@CId", cid);
                }
                else
                {
                    Console.WriteLine("Invalid input. Value should not be null");
                    goto startEInsertCid;
                }
            startEinsertadm:
                Regex regAdm = new Regex("[0-9 -]");
                Console.WriteLine("Enter admission date (YYYY-MM-DD):");
                string adm = Console.ReadLine();
                DateTime d = Convert.ToDateTime(adm);
                DateTime d1 = DateTime.Today;
                int r = d.Year;
                int s = d1.Year;
                int sub;
                sub = s - r;

                if (regAdm.IsMatch(adm) && sub >= 0)
                {
                    cmd.Parameters.AddWithValue("@Adm", adm);
                }
                else
                {
                    Console.WriteLine("Enter proper date");
                    goto startEinsertadm;
                }

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                Console.WriteLine("\nNew course enrollment has been added successfully");
                Console.WriteLine("Do you want to enroll another course ? If Yes, Enter Y or Enter E for Enrollment page or Enter H for home page or Press Enter to exit.");
                string x = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------------------------");
                if (x == "Y")
                {
                    goto startEInsert;
                }
                if (x == "E")
                {
                    EnrollmentPage(con);
                }
                if (x == "H")
                {
                    HomePage(con);
                }
                else
                {
                    Console.WriteLine("Exit");
                }
            }
            catch
            {
                Console.WriteLine("Invalid Id entered");
                InsertEnrollment(con);
            }
        }
        //Update enrollment
        public static void UpdateEnrollment(SqlConnection con)
        {
            startE:
            try
            {
            startEUpdate:
                Console.Write("Enter enrollment Id: ");
                string Id = Console.ReadLine();
            cBack:
                Console.WriteLine("Enter the property to edit: \n1 - Course id\n2 - Admission Date\n3 - Both");
                int ch = Convert.ToInt32(Console.ReadLine());

                if (ch == 1)
                {
                    //Cone:
                    Console.Write("Enter course id: ");
                    int cid = Convert.ToInt32(Console.ReadLine());

                    string str = $"update Enrollment_details set CId  = {cid} where eId = '{Id}'";
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Console.WriteLine("Course Id Updated successfully");

                    Console.WriteLine("Do you want to update another course enrollment? If Yes, Enter Y, Enter E for enrollment section or Enter B for back or Enter H for home page or Press enter to exit.");
                    string x = Console.ReadLine();
                    Console.WriteLine("-----------------------------------------------------------------");
                    if (x == "Y")
                    {
                        goto startEUpdate;
                    }
                    if (x == "B")
                    {
                        goto cBack;
                    }
                    if (x == "E")
                    {
                        EnrollmentPage(con);
                    }
                    if (x == "H")
                    {
                        HomePage(con);
                    }
                    else
                    {
                        Console.WriteLine("Exit");
                    }
                }
                if (ch == 2)
                {
                startEupdateadm:
                    Regex regAdm = new Regex("[0-9 -]");
                    Console.WriteLine("Enter admission date (YYYY-MM-DD):");
                    string adm = Console.ReadLine();
                    DateTime d = Convert.ToDateTime(adm);
                    DateTime d1 = DateTime.Today;
                    int r = d.Year;
                    int s = d1.Year;
                    int sub;
                    sub = s - r;

                    if (regAdm.IsMatch(adm) && sub >= 0)
                    {

                        string str = $"update Enrollment_details set Adm  = '{adm}' where eId = '{Id}'";
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = str;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = con;

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Console.WriteLine("Admission date has been Updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Enter proper date");
                        goto startEupdateadm;
                    }

                    Console.WriteLine("Do you want to update another course enrollment? If Yes, Enter Y, Enter E for enrollment section, Enter B for back or Enter H for home page or Press enter to exit.");
                    string x = Console.ReadLine();
                    Console.WriteLine("-----------------------------------------------------------------");
                    if (x == "Y")
                    {
                        goto startEUpdate;
                    }
                    if (x == "B")
                    {
                        goto cBack;
                    }
                    if (x == "E")
                    {
                        EnrollmentPage(con);
                    }
                    if (x == "H")
                    {
                        HomePage(con);
                    }
                    else
                    {
                        Console.WriteLine("Exit");
                    }
                }
                if (ch == 3)
                {
                    //Eone:
                    Console.Write("Enter course id: ");
                    int cid = Convert.ToInt32(Console.ReadLine());
                    string str0 = $"update Enrollment_details set CId  = {cid} where eId = '{Id}'";
                    con.Open();
                    SqlCommand cmdr = new SqlCommand();
                    cmdr.CommandText = str0;
                    cmdr.CommandType = System.Data.CommandType.Text;
                    cmdr.Connection = con;
                    cmdr.ExecuteNonQuery();
                    con.Close();

                    Console.Write("Enter admission date: ");
                    string adm = Console.ReadLine();

                    string str = $"update Enrollment_details set Adm  = '{adm}' where eId = '{Id}'";
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = str;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();

                    Console.WriteLine("Admission date has been Updated successfully");

                    Console.WriteLine("Do you want to update another course enrollment? If Yes, Enter Y, Enter E for enrollment section, Enter B for back or Enter H for home page or Press enter to exit.");
                    string x = Console.ReadLine();
                    Console.WriteLine("-----------------------------------------------------------------");
                    if (x == "Y")
                    {
                        goto startEUpdate;
                    }
                    if (x == "B")
                    {
                        goto cBack;
                    }
                    if (x == "E")
                    {
                        EnrollmentPage(con);
                    }
                    if (x == "H")
                    {

                        HomePage(con);
                    }
                    else
                    {
                        Console.WriteLine("Exit");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Invalid id entered");
                goto startE;
            }
       
        }
        //Delete enrollment
        public static void DeleteEnrollment(SqlConnection con)
        {
        startEDelete:
            Console.Write("Enter enrollment Id: ");
            string id = Console.ReadLine();
            string cc = $"select * from Enrollment_details where eId = '{id}'";
            SqlCommand cmd = new SqlCommand(cc);
            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                con.Close();
                string str = $"delete from Enrollment_details where eId = '{id}'";
                SqlCommand cmdr = new SqlCommand(str);
                cmdr.Connection = con;
                con.Open();
                cmdr.ExecuteNonQuery();
                con.Close();
                Console.WriteLine("\nEnrollment has been deleted successfully\n");
            }
            else
            {
                Console.WriteLine("Enrollment ID not found");
                con.Close();
                goto startEDelete;
            }
            Console.WriteLine("Do you want to delete another enrollment? If Yes, Enter Y, Enter E for Enrollment page or Enter H for home screen or Press Enter to exit.");
            string x = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (x == "Y")
            {
                goto startEDelete;
            }
            if (x == "E")
            {
                EnrollmentPage(con);
            }
            if (x == "H")
            {
                HomePage(con);
            }
            else
            {
                Console.WriteLine("Exit");
            }

        }
        
        //4. All details//

        //All students
        public static void DisplayViewStudents(SqlConnection con)
        {
            con.Open();
            string str = "select * from vw_allStudents";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("Id\tFirst Name\t\t\t\t\t  Last Name");
            Console.WriteLine("--------------------------------------------------------------------------");
            while (dr.Read())
            {
                Console.WriteLine(dr[0]+"\t"+dr[1]+dr[2]);
            }
            Console.WriteLine("--------------------------------------------------------------------------");
            con.Close();
            Console.WriteLine("If you want to display specific student's details, Enter Y.\nEnter B for back.\nIf you want to go to home screen, Enter H.");
            Console.WriteLine("Enter your choice:");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "Y")
            {
                DisplayViewOneStudent(con);
            }
            if (e == "B")
            {
                Console.WriteLine("\nEnter 1 to display all students \nEnter 2 to display specific student\nEnter 3 to go back\nEnter your choice: ");
                int f = Convert.ToInt32(Console.ReadLine());
                if (f == 1)
                {
                    DisplayViewStudents(con);
                }
                if (f == 2)
                {
                    DisplayViewOneStudent(con);
                }
            }
            if (e == "H")
            {
                HomePage(con);
            }
        }
        //Specific student
        public static void DisplayViewOneStudent(SqlConnection con)
        {
            con.Open();
            Console.WriteLine("Enter student id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string str = $"select * from vw_allStudents where Id = {id}";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("--------------------------------------------------------------------------");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine($"\nId: {dr[0]}\nName:{dr[1]+" "+dr[2]}\nCourse name: {dr[3]}\nAdmission date: {dr.GetDateTime(4).ToString("dd/MM/yyyy")}\n");
                }
            }
            Console.WriteLine("--------------------------------------------------------------------------");
            con.Close();
            Console.WriteLine("If you want to display specific student's details, Enter Y.\nEnter B for back.\nIf you want to go to home screen, Enter H.");
            Console.WriteLine("Enter your choice:");
            string e = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------");
            if (e == "Y")
            {
                DisplayOne(con);
            }
            if (e == "B")
            {
                Console.WriteLine("\nEnter 1 to display all students \nEnter 2 to display specific student\nEnter 3 to go back\nEnter your choice: ");
                int f = Convert.ToInt32(Console.ReadLine());
                if (f == 1)
                {
                    DisplayViewStudents(con);
                }
                if (f == 2)
                {
                    DisplayViewOneStudent(con);
                }
                if (f == 3)
                {
                    HomePage(con);
                }
            }
            if (e == "H")
            {
                HomePage(con);
            }
        }

        //Student page
        public static void StudentsPage(SqlConnection con)
        {
            Console.WriteLine("\nEnter 1 to add new student  \nEnter 2 to display students list  \nEnter 3 to update student details \nEnter 4 to delete student details\nEnter 5 to go back\nPress 'Enter' to Exit\n");

            Console.Write("\nActivity: ");

            int a = Convert.ToInt32(Console.ReadLine());

            if (a == 1)
            {
                Insert(con);
            }
            if (a == 2)
            {
                Console.WriteLine("\nEnter your choice: \nEnter 1 to display all students\nEnter 2 to display specific student\nEnter 3 to go back");
                int b = Convert.ToInt32(Console.ReadLine());
                if (b == 1)
                {
                    Display(con);
                }
                if (b == 2)
                {
                    DisplayOne(con);
                }
                 if (b == 3)
                {
                    StudentsPage(con);
                }
            }
            if (a == 3)
            {
                Update(con);
            }
            if (a == 4)
            {
                Delete(con);
            }
            if (a == 5)
            {
                HomePage(con);
            }
        }
        //Course page
        public static void CoursesPage(SqlConnection con)
        {
            Console.WriteLine("\nEnter 1 to add new course  \nEnter 2 to display courses list  \nEnter 3 to update course's details \nEnter 4 to delete course's details\nEnter 5 to go back\nPress 'Enter' to Exit\n");

            Console.Write("\nActivity: ");

            int a = Convert.ToInt32(Console.ReadLine());

            if (a == 1)
            {
                InsertCourse(con);
            }

            if (a == 2)
            {
                Console.WriteLine("\nEnter your choice: \nEnter 1 to display all courses' details\nEnter 2 to display specific course's details\nEnter 3 to go back\n");
                int b = Convert.ToInt32(Console.ReadLine());
                if (b == 1)
                {
                    DisplayCourse(con);
                }
                if (b == 2)
                {
                    DisplayOneCourse(con);
                }
                if (b == 3)
                {
                    CoursesPage(con);
                }
            }
            if (a == 3)
            {
                UpdateCourse(con);
            }
            if (a == 4)
            {
                DeleteCourse(con);
            }
            if (a == 5)
            {
                HomePage(con);
            }
        }
        //Enrollment page
        public static void EnrollmentPage(SqlConnection con)
        {
            Console.WriteLine("\nEnter 1 to add new enrollment  \nEnter 2 to display enrollment list  \nEnter 3 to update enrollment details \nEnter 4 to delete enrollment details\nEnter 5 to go back \nPress'Enter' to Exit\n");

            Console.Write("\nActivity: ");

            int a = Convert.ToInt32(Console.ReadLine());

            if (a == 1)
            {
                InsertEnrollment(con);
            }

            if (a == 2)
            {
                DisplayEnrollment(con);
            }
            if (a == 3)
            {
                UpdateEnrollment(con);
            }
            if (a == 4)
            {
                DeleteEnrollment(con);
            }
            if (a == 5)
            {
                HomePage(con);
            }
        }
        //Home page
        public static void HomePage(SqlConnection con)
        {
            bool T = true;
            while (T)
            {
                Console.WriteLine("\nEnter 1 to view Students data\nEnter 2 to view Courses data\nEnter 3 to view Enrollment data\nEnter 4 to view Complete students' course details\nEnter 5 to Exit\nChoose any one from above options: ");
                int pref = Convert.ToInt32(Console.ReadLine());
                if (pref == 1)
                {

                    Console.WriteLine("\nEnter 1 to Add new student  \nEnter 2 to Display students list  \nEnter 3 to Update student details \nEnter 4 to Delete student details  \nPress 'Enter' to Exit\n");

                    Console.Write("\nActivity: ");

                    int a = Convert.ToInt32(Console.ReadLine());

                    if (a == 1)
                    {
                        Insert(con);
                    }

                    if (a == 2)
                    {
                        Console.WriteLine("\nEnter 1 for All courses\nEnter 2 for Specific course\nEnter your choice: ");
                        int b = Convert.ToInt32(Console.ReadLine());
                        if (b == 1)
                        {
                            Display(con);
                        }
                        if (b == 2)
                        {
                            DisplayOne(con);
                        }
                    }
                    if (a == 3)
                    {
                        Update(con);
                    }
                    if (a == 4)
                    {
                        Delete(con);
                    }
                    if (a == 5)
                    {
                        Console.WriteLine("\nPress Enter to Exit\n");
                    }
                }
                if (pref == 2)
                {
                    Console.WriteLine("\nEnter 1 to Add new course  \nEnter 2 to Display courses list  \nEnter 3 to Update course's details \nEnter 4 to Delete course's details \nPress 'Enter' to Exit\n");

                    Console.Write("\nChoose your activity: ");

                    int a = Convert.ToInt32(Console.ReadLine());

                    if (a == 1)
                    {
                        InsertCourse(con);
                    }

                    if (a == 2)
                    {
                        Console.WriteLine("\nEnter 1 to display all courses\nEnter 2 to display specific course\nEnter your choice: ");
                        int b = Convert.ToInt32(Console.ReadLine());
                        if (b == 1)
                        {
                            DisplayCourse(con);
                        }
                        if (b == 2)
                        {
                            DisplayOneCourse(con);
                        }
                    }
                    if (a == 3)
                    {
                        UpdateCourse(con);
                    }
                    if (a == 4)
                    {
                        DeleteCourse(con);
                    }
                    if (a == 5)
                    {
                        Console.WriteLine("\nPress Enter to Exit\n");
                    }
                }
                if (pref == 3)
                {
                    Console.WriteLine("\nEnter 1 to add new enrollment  \nEnter 2 to display enrollment list  \nEnter 3 to update enrollment details \nEnter 4 to delete enrollment details  \nPress 'Enter' to Exit\n");

                    Console.Write("\nChoose your activity: ");

                    int a = Convert.ToInt32(Console.ReadLine());

                    if (a == 1)
                    {
                        InsertEnrollment(con);
                    }

                    if (a == 2)
                    {
                        DisplayEnrollment(con);
                    }
                    if (a == 3)
                    {
                        UpdateEnrollment(con);
                    }
                    if (a == 4)
                    {
                        DeleteEnrollment(con);
                    }
                    if (a == 5)
                    {
                        Console.WriteLine("\nPress Enter to Exit\n");
                    }
                }
                if (pref == 4)
                {
                    Console.WriteLine("\nEnter 1 to display all students \nEnter 2 to display specific student\nEnter 3 to go back\nEnter your choice: ");
                    int f = Convert.ToInt32(Console.ReadLine());
                    if (f == 1)
                    {
                        DisplayViewStudents(con);
                    }
                    if (f == 2)
                    {
                        DisplayViewOneStudent(con);
                    }
                    if (f == 3)
                    {
                        HomePage(con);
                    }
                }
                if (pref == 5)
                {
                    T = false;
                    Console.WriteLine("Press Enter to exit");
                }
                Console.ReadLine();
            }            
        }

        //Main//
        static void Main(string[] args)
        {
            string connstr = "Data Source=LAPTOP-FT3BTS8F;Initial Catalog=Vivekananda_School;Integrated Security=True";
            SqlConnection con = new SqlConnection(connstr);

            try
            {
                bool T = true;
                while (T)
                {
                    Console.WriteLine("\nEnter 1 to view Students data\nEnter 2 to view Courses data\nEnter 3 to view Enrollment data\nEnter 4 to view Complete students' course details\nEnter 5 to Exit\nChoose any one from above options: ");
                    int pref = Convert.ToInt32(Console.ReadLine());
                    if (pref == 1)
                    {
                        
                        Console.WriteLine("\nEnter 1 to Add new student  \nEnter 2 to Display students list  \nEnter 3 to Update student details \nEnter 4 to Delete student details\nEnter 5 to go back\nEnter 6 to Exit\n");

                        Console.Write("\nActivity: ");

                        int a = Convert.ToInt32(Console.ReadLine());

                        if (a == 1)
                        {
                            Insert(con);
                        }

                        if (a == 2)
                        {
                            Console.WriteLine("\nEnter 1 to display all students\nEnter 2 to display specific student\nEnter 3 to go back\nEnter your choice: ");
                            int b = Convert.ToInt32(Console.ReadLine());
                            if (b == 1)
                            {
                                Display(con);
                            }
                            if (b == 2)
                            {
                                DisplayOne(con);
                            }
                            if (b == 3)
                            {
                                StudentsPage(con);
                            }
                        }
                        if (a == 3)
                        {
                            Update(con);
                        }
                        if (a == 4)
                        {
                            Delete(con);
                        }
                        if (a == 5)
                        {
                            HomePage(con);
                        }
                        if (a == 6)
                        {
                            T = false;
                            Console.WriteLine("\nPress Enter to Exit\n");
                        }
                        
                    }
                    if (pref == 2)
                    {
                        Console.WriteLine("\nEnter 1 to Add new course  \nEnter 2 to Display courses list  \nEnter 3 to Update course's details \nEnter 4 to Delete course's details\nEnter 5 to go back\nEnter 6 to Exit\n");

                        Console.Write("\nChoose your activity: ");

                        int a = Convert.ToInt32(Console.ReadLine());

                        if (a == 1)
                        {
                            InsertCourse(con);
                        }

                        if (a == 2)
                        {
                            Console.WriteLine("\nEnter 1 to display all courses\nEnter 2 to display specific course\nEnter 3 to go back\nEnter your choice: ");
                            int b = Convert.ToInt32(Console.ReadLine());
                            if (b == 1)
                            {
                                DisplayCourse(con);
                            }
                            if (b == 2)
                            {
                                DisplayOneCourse(con);
                            }
                            if (b==3)
                            {
                                CoursesPage(con);
                            }
                                
                        }
                        if (a == 3)
                        {
                            UpdateCourse(con);
                        }
                        if (a == 4)
                        {
                            DeleteCourse(con);
                        }
                        if (a == 5)
                        {
                            HomePage(con);
                        }
                        if (a == 6)
                        {
                            T = false;
                            Console.WriteLine("\nPress Enter to Exit\n");
                        }
                    }
                    if (pref == 3)
                    {
                        Console.WriteLine("\nEnter 1 to add new enrollment  \nEnter 2 to display enrollment list  \nEnter 3 to update enrollment details \nEnter 4 to delete enrollment details\nEnter 5 to go back\nEnter 6 to Exit\n");

                        Console.Write("\nChoose your activity: ");

                        int a = Convert.ToInt32(Console.ReadLine());

                        if (a == 1)
                        {
                            InsertEnrollment(con);
                        }

                        if (a == 2)
                        {
                            DisplayEnrollment(con);                        
                        }
                        if (a == 3)
                        {
                            UpdateEnrollment(con);
                        }
                        if (a == 4)
                        {
                            DeleteEnrollment(con);
                        }
                        if (a == 5)
                        {
                            HomePage(con);
                        }
                        if (a == 6)
                        {
                            T = false;
                            Console.WriteLine("\nPress Enter to Exit\n");
                        }
                    }
                    if (pref == 4)
                    {
                        Console.WriteLine("\nEnter 1 to display all students \nEnter 2 to display specific student\nEnter 3 to go back\nEnter your choice: ");
                        int f = Convert.ToInt32(Console.ReadLine());
                        if (f == 1)
                        {
                            DisplayViewStudents(con);
                        }
                        if (f == 2)
                        {
                            DisplayViewOneStudent(con);
                        }
                        if (f == 3)
                        {
                            HomePage(con);
                        }
                    }
                    if (pref == 5)
                    {
                        T = false;
                        Console.WriteLine("Press Enter to exit");
                    }
                }
                Console.ReadLine();
            }
            catch
            {
                Console.WriteLine("INVALID INPUT");
            }           
        }
    }
}
