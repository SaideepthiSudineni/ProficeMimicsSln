using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MimicsConsoleApp
{
    public class Program
    {
        // Stores the current visiting page
        static String current_state_url = "";

        //Stores url when pressed forward
        static Stack<String> next_stack = new Stack<String>();

        //Stores url when pressed backward
        static Stack<String> previous_stack = new Stack<String>();

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            bool isValid = true;
          
            Stack my_stack = new Stack();

            do
            {
                displayMenu();
                string ch = Convert.ToString(Console.ReadLine());
                using ( SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = "Data Source=LAPTOP-VVCD6TG2;Initial Catalog=Deepthi;Integrated Security=True";
                    switch (ch.ToLower())
                    {
                        case "e":
                            {
                                //Enter URL
                                Console.WriteLine("Enter URL: ");
                                string url = Convert.ToString(Console.ReadLine());

                                Uri uriResult;
                                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                                
                                if (result)
                                {
                                    // Push into backward_stack
                                    previous_stack.Push(url);

                                    // Visit the current URL
                                    visit_new_url(url);
                                    //current_state_url = url;
                                    Console.WriteLine("URL {" + current_state_url + "} is visited");
                                }
                                else
                                {
                                    Console.WriteLine("Entered URL is not a valid!!!");                                     
                                }
                                break;
                            }
                        case "p":
                            {
                                //Display previous URL
                                backward();                                
                                break;
                            }
                        case "n":
                            {
                                //Display next URL                                
                                forward();
                                break;
                            }                        
                        case "exit":
                            {
                                Console.WriteLine("Thank you!");
                                isValid = false;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Invalid choice. Please select from the following: ");
                                break;
                            }
                    }
                }
            } while (isValid);

        }
              

        // Function for when visit a url
        static void visit_new_url(String url)
        {
            // If current URL is empty
            if (current_state_url != "")
            {
                // Push into backward_stack
                previous_stack.Push(current_state_url);
            }

            // Set curr_state_url to url
            current_state_url = url;
           
        }

        // Function to handle state when the forward button is pressed
        static void forward()
        {
            // If current url is the last url
            if (next_stack.Count == 0 || current_state_url == next_stack.Peek())
            {
                Console.WriteLine("The next option is disabled!!!");
            }
            else
            {
                // Push current state to the backward stack
                previous_stack.Push(current_state_url);

               
                // Set current state to top of forward stack
                current_state_url = next_stack.Peek();
                Console.WriteLine("The next URL that was visited/entered by the user - " + next_stack.Peek());

                // Remove from forward stack
                next_stack.Pop();


            }
        }

        // Function to handle state when the backward button is pressed
        static void backward()
        {
            // If current url is the last url
            if (previous_stack.Count == 0)
            {
                Console.WriteLine("Previous option is disabled!!!");
            }
            else
            {
                // Push current url to the forward stack
                next_stack.Push(current_state_url);


                // Set current url to top of backward stack
                current_state_url = previous_stack.Peek();
                Console.WriteLine("The previous URL that was visited/entered by the user - " + current_state_url);

                // Pop it from backward stack
                previous_stack.Pop();

            }
        }


        private static void displayMenu()
        {
            Console.WriteLine("Type ‘e’ to Enter URL");
            Console.WriteLine("Type ‘p’ to Display previous URL");
            Console.WriteLine("Type ‘n’ to Display next URL");
            Console.WriteLine("Type ‘exit’ to Exit the application");
        }
    }
}
