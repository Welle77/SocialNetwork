using System;

namespace SocialNetwork
{
    public class SocialNetworkApplication
    {
        public void Start()
        {
            Console.WriteLine("Type 'q' to quit and 'h' to list these lines again");

            LongAssSwitchStatement();
        }

        private void LongAssSwitchStatement()
        {
            do
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    
                    default:
                        Console.WriteLine("Command not found");
                        break;
                }
            } while (true);
        }
    }
}